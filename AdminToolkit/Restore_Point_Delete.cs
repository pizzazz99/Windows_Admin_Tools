// ============================================================
//  Restore_Point_Delete.cs   (C# 7.3 / .NET Framework)
//  Deletes System Restore points by sequence number via the
//  SRRemoveRestorePoint API (SrClient.dll).
//
//  DESTRUCTIVE: removes the restore point AND its underlying
//  shadow copy. There is no undo. Callers must confirm with
//  the user first.
//
//  Requires: elevation (run as Administrator).
// ============================================================

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Admin_Tools
{
    public static class Restore_Point_Delete
    {
        [DllImport("SrClient.dll", SetLastError = false)]
        private static extern uint SRRemoveRestorePoint(uint dwRPNum);

        private const uint ERROR_SUCCESS = 0;
        private const uint ERROR_FILE_NOT_FOUND = 2;

        /// <summary>
        /// Delete one restore point. Returns null on success,
        /// or a human-readable error message on failure.
        /// </summary>
        public static string Delete(uint sequenceNumber)
        {
            try
            {
                uint rc = SRRemoveRestorePoint(sequenceNumber);

                if (rc == ERROR_SUCCESS) return null;
                if (rc == ERROR_FILE_NOT_FOUND)
                    return "#" + sequenceNumber +
                           ": not found (already deleted or aged out)";
                return "#" + sequenceNumber + ": error code " + rc;
            }
            catch (DllNotFoundException)
            {
                return "SrClient.dll not available on this system.";
            }
            catch (Exception Ex)
            {
                return "#" + sequenceNumber + ": " + Ex.Message;
            }
        }

        /// <summary>
        /// Delete many restore points. Returns (deleted, failed, errors).
        /// </summary>
        public static void Delete_Many(IEnumerable<uint> sequenceNumbers,
            out int deleted, out int failed, out List<string> errors)
        {
            deleted = 0;
            failed = 0;
            errors = new List<string>();

            foreach (uint seq in sequenceNumbers)
            {
                string err = Delete(seq);
                if (err == null) deleted++;
                else { failed++; errors.Add(err); }
            }
        }
    }
}