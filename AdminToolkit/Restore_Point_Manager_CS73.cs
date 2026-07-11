// ============================================================
//  Restore_Point_Manager.cs   (C# 7.3 / .NET Framework version)
//  Queries System Restore points (root\default : SystemRestore)
//  and cross-links each one to its underlying shadow copy
//  (root\cimv2 : Win32_ShadowCopy) by creation timestamp.
//
//  Requires reference:  System.Management
//    (.NET Framework: Project > Add Reference > Assemblies >
//     System.Management — no NuGet needed)
//  Requires:            elevation (run as Administrator)
//
//  NOTE: adjust the namespace to match your project.
// ============================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;

namespace Admin_Tools
{
    public sealed class Restore_Point_Info
    {
        public uint Sequence_Number { get; set; }
        public string Description { get; set; }
        public uint Restore_Point_Type { get; set; }
        public uint Event_Type { get; set; }
        public DateTime Creation_Time { get; set; }

        // Filled in by the shadow-copy correlation pass (may stay null)
        public string Linked_Shadow_Id { get; set; }
        public string Linked_Device { get; set; }

        public string Type_Name
        {
            get { return Restore_Point_Manager.Type_To_String(Restore_Point_Type); }
        }

        public string Event_Name
        {
            get { return Restore_Point_Manager.Event_To_String(Event_Type); }
        }
    }

    public static class Restore_Point_Manager
    {
        // --------------------------------------------------------
        //  Main query — returns all restore points, newest first,
        //  each correlated (where possible) to a shadow copy.
        // --------------------------------------------------------
        public static List<Restore_Point_Info> Get_Restore_Points()
        {
            var points = new List<Restore_Point_Info>();

            var scope = new ManagementScope(@"\\.\root\default");
            scope.Connect();

            using (var searcher = new ManagementObjectSearcher(
                scope, new ObjectQuery("SELECT * FROM SystemRestore")))
            {
                foreach (ManagementObject mo in searcher.Get())
                {
                    var rp = new Restore_Point_Info();

                    rp.Sequence_Number = (uint)mo["SequenceNumber"];
                    rp.Description = mo["Description"] == null ? "" : mo["Description"].ToString();
                    rp.Restore_Point_Type = (uint)mo["RestorePointType"];
                    rp.Event_Type = (uint)mo["EventType"];
                    rp.Creation_Time = ManagementDateTimeConverter
                                                .ToDateTime(mo["CreationTime"].ToString());
                    points.Add(rp);
                }
            }

            Correlate_With_Shadow_Copies(points);

            return points.OrderByDescending(p => p.Creation_Time).ToList();
        }

        // --------------------------------------------------------
        //  Match each restore point to a shadow copy whose
        //  InstallDate is within a few seconds of the restore
        //  point's CreationTime. Windows creates them in the same
        //  operation, so a tight window is reliable.
        // --------------------------------------------------------
        private static void Correlate_With_Shadow_Copies(List<Restore_Point_Info> points)
        {
            try
            {
                var shadows = new List<Shadow_Entry>();

                using (var searcher = new ManagementObjectSearcher(
                    @"root\cimv2",
                    "SELECT ID, DeviceObject, InstallDate FROM Win32_ShadowCopy"))
                {
                    foreach (ManagementObject mo in searcher.Get())
                    {
                        object installDate = mo["InstallDate"];
                        if (installDate == null) continue;

                        var entry = new Shadow_Entry();
                        entry.Id = mo["ID"] == null ? "" : mo["ID"].ToString();
                        entry.Device = mo["DeviceObject"] == null ? "" : mo["DeviceObject"].ToString();
                        entry.Created = ManagementDateTimeConverter.ToDateTime(installDate.ToString());
                        shadows.Add(entry);
                    }
                }

                foreach (var rp in points)
                {
                    Shadow_Entry best = null;
                    double bestDelta = double.MaxValue;

                    foreach (var s in shadows)
                    {
                        double delta = Math.Abs((s.Created - rp.Creation_Time).TotalSeconds);
                        if (delta <= 20.0 && delta < bestDelta)
                        {
                            best = s;
                            bestDelta = delta;
                        }
                    }

                    if (best != null)
                    {
                        rp.Linked_Shadow_Id = best.Id;
                        rp.Linked_Device = best.Device;
                    }
                }
            }
            catch
            {
                // Shadow copy correlation is best-effort; restore
                // point data is still valid without it.
            }
        }

        private sealed class Shadow_Entry
        {
            public string Id;
            public string Device;
            public DateTime Created;
        }

        // --------------------------------------------------------
        //  Detail text — same visual style as Snapshot Details
        // --------------------------------------------------------
        public static string Build_Details_Text(Restore_Point_Info rp)
        {
            var sb = new StringBuilder();
            var age = DateTime.Now - rp.Creation_Time;

            sb.AppendLine("RESTORE POINT DETAILS");
            sb.AppendLine(new string('=', 55));
            sb.AppendLine("Sequence #    : " + rp.Sequence_Number);
            sb.AppendLine("Created       : " + rp.Creation_Time.ToString("yyyy-MM-dd HH:mm:ss"));
            sb.AppendLine("Age           : " + age.TotalDays.ToString("0.0") + " days");
            sb.AppendLine("Description   : " + rp.Description);
            sb.AppendLine();
            sb.AppendLine("ATTRIBUTION");
            sb.AppendLine(new string('-', 55));
            sb.AppendLine("Type          : " + rp.Type_Name + "  (" + rp.Restore_Point_Type + ")");
            sb.AppendLine("Event         : " + rp.Event_Name + "  (" + rp.Event_Type + ")");
            sb.AppendLine();
            sb.AppendLine("LINKED SHADOW COPY (matched by timestamp)");
            sb.AppendLine(new string('-', 55));

            if (rp.Linked_Shadow_Id != null)
            {
                sb.AppendLine("Shadow ID     : " + rp.Linked_Shadow_Id);
                sb.AppendLine("Device object : " + rp.Linked_Device);
            }
            else
            {
                sb.AppendLine("None found — its shadow copy may have been");
                sb.AppendLine("aged out (deleted) while the restore point");
                sb.AppendLine("metadata remains.");
            }

            return sb.ToString();
        }

        // --------------------------------------------------------
        //  Enum decoding (values from SrRestorePtApi.h)
        // --------------------------------------------------------
        public static string Type_To_String(uint t)
        {
            switch (t)
            {
                case 0: return "APPLICATION_INSTALL";
                case 1: return "APPLICATION_UNINSTALL";
                case 6: return "RESTORE";
                case 7: return "CHECKPOINT (scheduled/manual)";
                case 8: return "WINDOWS_SHUTDOWN";
                case 9: return "WINDOWS_BOOT";
                case 10: return "DEVICE_DRIVER_INSTALL";
                case 11: return "FIRSTRUN";
                case 12: return "MODIFY_SETTINGS";
                case 13: return "CANCELLED_OPERATION";
                case 14: return "BACKUP_RECOVERY";
                default: return "UNKNOWN (" + t + ")";
            }
        }

        public static string Event_To_String(uint e)
        {
            switch (e)
            {
                case 100: return "BEGIN_SYSTEM_CHANGE";
                case 101: return "END_SYSTEM_CHANGE";
                case 102: return "BEGIN_NESTED_SYSTEM_CHANGE";
                case 103: return "END_NESTED_SYSTEM_CHANGE";
                default: return "UNKNOWN (" + e + ")";
            }
        }
    }
}