// ============================================================
//  Restore_Point_Manager.cs
//  Queries System Restore points (root\default : SystemRestore)
//  and cross-links each one to its underlying shadow copy
//  (root\cimv2 : Win32_ShadowCopy) by creation timestamp.
//
//  Requires NuGet package:  System.Management
//  Requires:                elevation (run as Administrator)
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
        public uint     Sequence_Number   { get; set; }
        public string   Description       { get; set; } = "";
        public uint     Restore_Point_Type{ get; set; }
        public uint     Event_Type        { get; set; }
        public DateTime Creation_Time     { get; set; }

        // Filled in by the shadow-copy correlation pass (may stay null)
        public string  Linked_Shadow_Id  { get; set; }
        public string  Linked_Device     { get; set; }

        public string Type_Name  => Restore_Point_Manager.Type_To_String(Restore_Point_Type);
        public string Event_Name => Restore_Point_Manager.Event_To_String(Event_Type);
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

            using var searcher = new ManagementObjectSearcher(
                scope, new ObjectQuery("SELECT * FROM SystemRestore"));

            foreach (ManagementObject mo in searcher.Get())
            {
                var rp = new Restore_Point_Info
                {
                    Sequence_Number    = (uint)mo["SequenceNumber"],
                    Description        = mo["Description"]?.ToString() ?? "",
                    Restore_Point_Type = (uint)mo["RestorePointType"],
                    Event_Type         = (uint)mo["EventType"],
                    Creation_Time      = ManagementDateTimeConverter
                                            .ToDateTime(mo["CreationTime"].ToString())
                };
                points.Add(rp);
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
                using var searcher = new ManagementObjectSearcher(
                    @"root\cimv2", "SELECT ID, DeviceObject, InstallDate FROM Win32_ShadowCopy");

                var shadows = new List<(string Id, string Device, DateTime Created)>();

                foreach (ManagementObject mo in searcher.Get())
                {
                    var installDate = mo["InstallDate"]?.ToString();
                    if (string.IsNullOrEmpty(installDate)) continue;

                    shadows.Add((
                        mo["ID"]?.ToString() ?? "",
                        mo["DeviceObject"]?.ToString() ?? "",
                        ManagementDateTimeConverter.ToDateTime(installDate)));
                }

                foreach (var rp in points)
                {
                    var match = shadows
                        .Select(s => new { s, Delta = Math.Abs((s.Created - rp.Creation_Time).TotalSeconds) })
                        .Where(x => x.Delta <= 5.0)
                        .OrderBy(x => x.Delta)
                        .FirstOrDefault();

                    if (match != null)
                    {
                        rp.Linked_Shadow_Id = match.s.Id;
                        rp.Linked_Device    = match.s.Device;
                    }
                }
            }
            catch
            {
                // Shadow copy correlation is best-effort; restore
                // point data is still valid without it.
            }
        }

        // --------------------------------------------------------
        //  Detail text — same visual style as Snapshot Details
        // --------------------------------------------------------
        public static string Build_Details_Text(Restore_Point_Info rp)
        {
            var sb  = new StringBuilder();
            var age = DateTime.Now - rp.Creation_Time;

            sb.AppendLine("RESTORE POINT DETAILS");
            sb.AppendLine(new string('=', 55));
            sb.AppendLine($"Sequence #    : {rp.Sequence_Number}");
            sb.AppendLine($"Created       : {rp.Creation_Time:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine($"Age           : {age.TotalDays:0.0} days");
            sb.AppendLine($"Description   : {rp.Description}");
            sb.AppendLine();
            sb.AppendLine("ATTRIBUTION");
            sb.AppendLine(new string('-', 55));
            sb.AppendLine($"Type          : {rp.Type_Name}  ({rp.Restore_Point_Type})");
            sb.AppendLine($"Event         : {rp.Event_Name}  ({rp.Event_Type})");
            sb.AppendLine();
            sb.AppendLine("LINKED SHADOW COPY (matched by timestamp)");
            sb.AppendLine(new string('-', 55));

            if (rp.Linked_Shadow_Id != null)
            {
                sb.AppendLine($"Shadow ID     : {rp.Linked_Shadow_Id}");
                sb.AppendLine($"Device object : {rp.Linked_Device}");
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
        public static string Type_To_String(uint t) => t switch
        {
            0  => "APPLICATION_INSTALL",
            1  => "APPLICATION_UNINSTALL",
            6  => "RESTORE",
            7  => "CHECKPOINT (scheduled/manual)",
            8  => "WINDOWS_SHUTDOWN",
            9  => "WINDOWS_BOOT",
            10 => "DEVICE_DRIVER_INSTALL",
            11 => "FIRSTRUN",
            12 => "MODIFY_SETTINGS",
            13 => "CANCELLED_OPERATION",
            14 => "BACKUP_RECOVERY",
            _  => $"UNKNOWN ({t})"
        };

        public static string Event_To_String(uint e) => e switch
        {
            100 => "BEGIN_SYSTEM_CHANGE",
            101 => "END_SYSTEM_CHANGE",
            102 => "BEGIN_NESTED_SYSTEM_CHANGE",
            103 => "END_NESTED_SYSTEM_CHANGE",
            _   => $"UNKNOWN ({e})"
        };
    }
}
