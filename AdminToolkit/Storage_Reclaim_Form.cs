// ============================================================
//  Storage_Reclaim_Form.cs   (C# 7.3 / .NET Framework)
//  Shows each shadow storage association (which drive's
//  snapshots, where they're stored, used / allocated / max)
//  and resizes the maximum via "vssadmin resize shadowstorage".
//
//  Shrinking the max below the used amount deletes the OLDEST
//  snapshots to fit and releases the allocation back to the
//  drive — the supported way to reclaim shadow storage space.
//
//  Pairs with Storage_Reclaim_Form.Designer.cs.
//  Requires:  System.Management reference, elevation.
//
//  Usage (e.g. from Shadow_Copy_Form):
//      using (var f = new Storage_Reclaim_Form())
//          f.ShowDialog(this);
// ============================================================

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Management;
using System.Text;
using System.Windows.Forms;

namespace Admin_Tools
{
    public partial class Storage_Reclaim_Form : Form
    {
        private sealed class Storage_Row
        {
            public string For_Volume;   // \\?\Volume{...}\ or drive letter
            public string On_Volume;
            public string For_Display;  // "C:" if resolvable
            public string On_Display;
            public ulong  Used;
            public ulong  Allocated;
            public ulong  Max;
            public bool   Unbounded;
        }

        // The unbounded sentinel is a near-max ulong
        private const ulong Unbounded_Threshold = 0xF000000000000000UL;

        public Storage_Reclaim_Form()
        {
            InitializeComponent();
            cmbUnit.SelectedIndex = 0;   // GB
            Load_Storage();
        }

        // --------------------------------------------------------
        //  Load associations from Win32_ShadowStorage
        // --------------------------------------------------------
        private void Load_Storage()
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                var volumeToDrive = Get_Volume_To_Drive_Map();

                lvStorage.BeginUpdate();
                lvStorage.Items.Clear();

                using (var searcher = new ManagementObjectSearcher(
                    @"root\cimv2", "SELECT * FROM Win32_ShadowStorage"))
                {
                    foreach (ManagementObject mo in searcher.Get())
                    {
                        var row = new Storage_Row();

                        row.For_Volume = Extract_DeviceId(Convert.ToString(mo["Volume"]));
                        row.On_Volume  = Extract_DeviceId(Convert.ToString(mo["DiffVolume"]));

                        string letter;
                        row.For_Display = volumeToDrive.TryGetValue(row.For_Volume, out letter)
                            ? letter : row.For_Volume;
                        row.On_Display = volumeToDrive.TryGetValue(row.On_Volume, out letter)
                            ? letter : row.On_Volume;

                        row.Used      = (ulong)mo["UsedSpace"];
                        row.Allocated = (ulong)mo["AllocatedSpace"];
                        row.Max       = (ulong)mo["MaxSpace"];
                        row.Unbounded = row.Max > Unbounded_Threshold;

                        var item = new ListViewItem(row.For_Display);
                        item.SubItems.Add(row.On_Display);
                        item.SubItems.Add(Format_Gb(row.Used));
                        item.SubItems.Add(Format_Gb(row.Allocated));
                        item.SubItems.Add(row.Unbounded ? "Unlimited" : Format_Gb(row.Max));
                        item.Tag = row;

                        lvStorage.Items.Add(item);
                    }
                }

                lvStorage.EndUpdate();

                if (lvStorage.Items.Count > 0)
                    lvStorage.Items[0].Selected = true;
                else
                    Text = "Shadow Storage - no associations found "
                         + "(is System Protection on?)";
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Could not read shadow storage: " + Ex.Message +
                    "\n\nMake sure the app is running as Administrator.",
                    "Shadow Storage", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private static string Format_Gb(ulong bytes)
        {
            return (bytes / (1024.0 * 1024.0 * 1024.0)).ToString("0.00") + " GB";
        }

        // Win32_ShadowStorage.Volume / DiffVolume are WMI reference
        // strings like:
        //   \\PC\root\cimv2:Win32_Volume.DeviceID="\\\\?\\Volume{guid}\\"
        // Extract and unescape the DeviceID.
        private static string Extract_DeviceId(string refPath)
        {
            if (string.IsNullOrEmpty(refPath)) return "";

            const string marker = "DeviceID=\"";
            int start = refPath.IndexOf(marker, StringComparison.OrdinalIgnoreCase);
            if (start < 0) return refPath;
            start += marker.Length;

            int end = refPath.LastIndexOf('"');
            if (end <= start) return refPath;

            string escaped = refPath.Substring(start, end - start);
            return escaped.Replace("\\\\", "\\");
        }

        private static Dictionary<string, string> Get_Volume_To_Drive_Map()
        {
            var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            using (var searcher = new ManagementObjectSearcher(
                @"root\cimv2",
                "SELECT DeviceID, DriveLetter FROM Win32_Volume WHERE DriveLetter IS NOT NULL"))
            {
                foreach (ManagementObject mo in searcher.Get())
                {
                    object dev    = mo["DeviceID"];
                    object letter = mo["DriveLetter"];
                    if (dev != null && letter != null)
                        map[dev.ToString()] = letter.ToString();
                }
            }

            return map;
        }

        // --------------------------------------------------------
        //  Unit combo — Unbounded needs no number
        // --------------------------------------------------------
        private void Cmb_Unit_Changed(object Sender, EventArgs e)
        {
            numMax.Enabled = cmbUnit.SelectedItem == null
                || cmbUnit.SelectedItem.ToString() != "Unbounded";
        }

        // --------------------------------------------------------
        //  Apply resize via vssadmin
        // --------------------------------------------------------
        private void Btn_Apply_Click(object Sender, EventArgs e)
        {
            if (lvStorage.SelectedItems.Count == 0)
            {
                MessageBox.Show("Select a storage row first.",
                    "Apply Resize", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var row = lvStorage.SelectedItems[0].Tag as Storage_Row;
            if (row == null) return;

            string unit = cmbUnit.SelectedItem.ToString();
            string maxArg;
            bool shrinking = false;

            if (unit == "Unbounded")
            {
                maxArg = "UNBOUNDED";
            }
            else if (unit == "GB")
            {
                ulong newBytes = (ulong)numMax.Value * 1024UL * 1024UL * 1024UL;
                shrinking = newBytes < row.Used;
                maxArg = numMax.Value.ToString("0") + "GB";

                // vssadmin refuses anything under 320 MB; 1 GB min here anyway
            }
            else   // "%"
            {
                if (numMax.Value > 100)
                {
                    MessageBox.Show("Percentage must be 1-100.",
                        "Apply Resize", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                maxArg = numMax.Value.ToString("0") + "%";
                // Can't compare % to used bytes without the volume size;
                // warn generically below.
                shrinking = true;
            }

            string warning =
                "Resize shadow storage for " + row.For_Display + " (stored on "
                + row.On_Display + ") to a maximum of " + maxArg + "?";

            if (unit != "Unbounded")
                warning += "\n\nCurrently used: " + Format_Gb(row.Used) +
                    (shrinking
                        ? "\n\n\u26A0 If the new maximum is below the used amount, " +
                          "Windows immediately deletes the OLDEST snapshots to fit. " +
                          "There is no undo."
                        : "");

            var answer = MessageBox.Show(warning, "Apply Resize",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (answer != DialogResult.Yes) return;

            // Prefer drive letters for vssadmin; fall back to volume GUID paths
            string forArg = row.For_Display.EndsWith(":")
                ? row.For_Display : row.For_Volume;
            string onArg = row.On_Display.EndsWith(":")
                ? row.On_Display : row.On_Volume;

            string args = "resize shadowstorage /for=" + forArg
                        + " /on=" + onArg + " /maxsize=" + maxArg;

            Cursor = Cursors.WaitCursor;
            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName               = "vssadmin.exe",
                    Arguments              = args,
                    UseShellExecute        = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError  = true,
                    CreateNoWindow         = true
                };

                string output;
                int exitCode;

                using (var p = Process.Start(psi))
                {
                    output   = p.StandardOutput.ReadToEnd()
                             + p.StandardError.ReadToEnd();
                    p.WaitForExit(60000);
                    exitCode = p.ExitCode;
                }

                if (exitCode == 0)
                {
                    MessageBox.Show(
                        "Resize succeeded.\n\nvssadmin " + args + "\n\n" + output.Trim(),
                        "Apply Resize", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(
                        "vssadmin returned exit code " + exitCode + ".\n\n" + output.Trim(),
                        "Apply Resize", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                Load_Storage();
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Resize failed: " + Ex.Message,
                    "Apply Resize", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void Btn_Refresh_Click(object Sender, EventArgs e)
        {
            Load_Storage();
        }

        private void Btn_Close_Click(object Sender, EventArgs e)
        {
            Close();
        }
    }
}
