// ============================================================
//  Snapshot_Delete_Manager.cs   (C# 7.3 / .NET Framework)
//  Deletes Volume Shadow Copies via WMI (Win32_ShadowCopy).
//
//  DESTRUCTIVE: a deleted snapshot is gone forever, and any
//  restore point riding on it dies with it. Callers are
//  responsible for confirming with the user first.
//
//  Requires reference:  System.Management
//  Requires:            elevation (run as Administrator)
// ============================================================

using System;
using System.Collections.Generic;
using System.Management;

namespace Admin_Tools
{
    public sealed class Snapshot_Delete_Result
    {
        public int Deleted_Count { get; set; }
        public int Failed_Count { get; set; }
        public List<string> Errors { get; set; }

        public Snapshot_Delete_Result()
        {
            Errors = new List<string>();
        }
    }

    public static class Snapshot_Delete_Manager
    {
        // --------------------------------------------------------
        //  Delete ONE snapshot by its shadow ID.
        //  Accepts the ID with or without braces, any case.
        // --------------------------------------------------------
        public static Snapshot_Delete_Result Delete_By_Id(string shadowId)
        {
            var result = new Snapshot_Delete_Result();

            string normalized = Normalize_Id(shadowId);
            if (normalized == null)
            {
                result.Failed_Count = 1;
                result.Errors.Add("Invalid shadow ID format: " + shadowId);
                return result;
            }

            try
            {
                using (var searcher = new ManagementObjectSearcher(
                    @"root\cimv2",
                    "SELECT * FROM Win32_ShadowCopy WHERE ID = '" + normalized + "'"))
                {
                    bool found = false;

                    foreach (ManagementObject mo in searcher.Get())
                    {
                        found = true;
                        mo.Delete();
                        result.Deleted_Count++;
                    }

                    if (!found)
                    {
                        result.Failed_Count = 1;
                        result.Errors.Add("No snapshot found with ID " + normalized +
                                          " (already deleted or aged out?)");
                    }
                }
            }
            catch (Exception ex)
            {
                result.Failed_Count = 1;
                result.Errors.Add(normalized + ": " + ex.Message);
            }

            return result;
        }

        // --------------------------------------------------------
        //  Delete MANY snapshots by ID (e.g. from a multi-select
        //  list). Keeps going on individual failures.
        // --------------------------------------------------------
        public static Snapshot_Delete_Result Delete_By_Ids(IEnumerable<string> shadowIds)
        {
            var total = new Snapshot_Delete_Result();

            foreach (var id in shadowIds)
            {
                var one = Delete_By_Id(id);
                total.Deleted_Count += one.Deleted_Count;
                total.Failed_Count += one.Failed_Count;
                total.Errors.AddRange(one.Errors);
            }

            return total;
        }

        // --------------------------------------------------------
        //  Delete all snapshots OLDER THAN a cutoff date,
        //  optionally restricted to one volume (drive letter,
        //  e.g. "C:"). Pass null for all volumes.
        // --------------------------------------------------------
        public static Snapshot_Delete_Result Delete_Older_Than(
            DateTime cutoff, string driveLetter)
        {
            var result = new Snapshot_Delete_Result();

            try
            {
                string volumeId = null;
                if (driveLetter != null)
                {
                    volumeId = Get_Volume_Id(driveLetter);
                    if (volumeId == null)
                    {
                        result.Errors.Add("Could not resolve volume for drive " + driveLetter);
                        return result;
                    }
                }

                using (var searcher = new ManagementObjectSearcher(
                    @"root\cimv2",
                    "SELECT ID, InstallDate, VolumeName FROM Win32_ShadowCopy"))
                {
                    // Collect first, then delete — deleting while
                    // enumerating a WMI collection is unreliable.
                    var toDelete = new List<string>();

                    foreach (ManagementObject mo in searcher.Get())
                    {
                        object installDate = mo["InstallDate"];
                        if (installDate == null) continue;

                        DateTime created = ManagementDateTimeConverter
                            .ToDateTime(installDate.ToString());
                        if (created >= cutoff) continue;

                        if (volumeId != null)
                        {
                            string vol = mo["VolumeName"] == null
                                ? "" : mo["VolumeName"].ToString();
                            if (!string.Equals(vol, volumeId,
                                    StringComparison.OrdinalIgnoreCase))
                                continue;
                        }

                        toDelete.Add(mo["ID"].ToString());
                    }

                    var batch = Delete_By_Ids(toDelete);
                    result.Deleted_Count = batch.Deleted_Count;
                    result.Failed_Count = batch.Failed_Count;
                    result.Errors.AddRange(batch.Errors);
                }
            }
            catch (Exception ex)
            {
                result.Errors.Add(ex.Message);
            }

            return result;
        }

        // --------------------------------------------------------
        //  Keep only the NEWEST n snapshots on a volume, delete
        //  the rest. driveLetter e.g. "C:", or null for all.
        // --------------------------------------------------------
        public static Snapshot_Delete_Result Keep_Newest(
            int keepCount, string driveLetter)
        {
            var result = new Snapshot_Delete_Result();

            try
            {
                string volumeId = driveLetter == null
                    ? null : Get_Volume_Id(driveLetter);

                var all = new List<KeyValuePair<DateTime, string>>();

                using (var searcher = new ManagementObjectSearcher(
                    @"root\cimv2",
                    "SELECT ID, InstallDate, VolumeName FROM Win32_ShadowCopy"))
                {
                    foreach (ManagementObject mo in searcher.Get())
                    {
                        object installDate = mo["InstallDate"];
                        if (installDate == null) continue;

                        if (volumeId != null)
                        {
                            string vol = mo["VolumeName"] == null
                                ? "" : mo["VolumeName"].ToString();
                            if (!string.Equals(vol, volumeId,
                                    StringComparison.OrdinalIgnoreCase))
                                continue;
                        }

                        all.Add(new KeyValuePair<DateTime, string>(
                            ManagementDateTimeConverter.ToDateTime(installDate.ToString()),
                            mo["ID"].ToString()));
                    }
                }

                // Oldest first; delete everything except the last keepCount
                all.Sort((a, b) => a.Key.CompareTo(b.Key));

                var toDelete = new List<string>();
                for (int i = 0; i < all.Count - keepCount; i++)
                    toDelete.Add(all[i].Value);

                var batch = Delete_By_Ids(toDelete);
                result.Deleted_Count = batch.Deleted_Count;
                result.Failed_Count = batch.Failed_Count;
                result.Errors.AddRange(batch.Errors);
            }
            catch (Exception ex)
            {
                result.Errors.Add(ex.Message);
            }

            return result;
        }

        // --------------------------------------------------------
        //  Helpers
        // --------------------------------------------------------

        // Returns the ID in canonical {GUID} form, or null if invalid.
        private static string Normalize_Id(string shadowId)
        {
            if (string.IsNullOrEmpty(shadowId)) return null;

            Guid g;
            string trimmed = shadowId.Trim().Trim('{', '}');
            if (!Guid.TryParse(trimmed, out g)) return null;

            return "{" + g.ToString().ToUpperInvariant() + "}";
        }

        // Resolves "C:" to its \\?\Volume{guid}\ name, which is what
        // Win32_ShadowCopy.VolumeName contains.
        private static string Get_Volume_Id(string driveLetter)
        {
            string letter = driveLetter.TrimEnd('\\');
            if (!letter.EndsWith(":")) letter += ":";

            using (var searcher = new ManagementObjectSearcher(
                @"root\cimv2",
                "SELECT DeviceID, DriveLetter FROM Win32_Volume WHERE DriveLetter = '"
                + letter + "'"))
            {
                foreach (ManagementObject mo in searcher.Get())
                    return mo["DeviceID"] == null ? null : mo["DeviceID"].ToString();
            }

            return null;
        }
    }
}