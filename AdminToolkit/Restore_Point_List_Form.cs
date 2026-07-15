// ============================================================
//  Restore_Point_List_Form.cs   (.NET 10 / WinForms)
//  Designer-based version. Pairs with
//  Restore_Point_List_Form.Designer.cs — all controls live
//  there; this file is logic only.
//
//  Usage from MainForm (modeless):
//      var f = new Restore_Point_List_Form();
//      f.Show(this);
// ============================================================

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Admin_Tools
{
    public partial class Restore_Point_List_Form : Form
    {
        private List<Restore_Point_Info> _points = new List<Restore_Point_Info>();

        // DisplayName -> InstallDate (day precision), read once per Load_Points
        private List<KeyValuePair<string, DateTime>> _installedPrograms =
            new List<KeyValuePair<string, DateTime>>();

        // Symlinks created by "Browse Files" — removed when the form closes
        private readonly List<string> _snapshotLinks = new List<string>();

        public Restore_Point_List_Form()
        {
            InitializeComponent();
            lvPoints.MultiSelect = true;
            Load_Points();
        }

        // --------------------------------------------------------
        //  Data load — sorted OLDEST first
        // --------------------------------------------------------
        private void Load_Points()
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                _points = Restore_Point_Manager.Get_Restore_Points();

                // Manager returns newest-first; flip to oldest-first
                _points.Reverse();

                _installedPrograms = Load_Installed_Programs();

                lvPoints.BeginUpdate();
                lvPoints.Items.Clear();

                foreach (var rp in _points)
                {
                    double ageDays = (DateTime.Now - rp.Creation_Time).TotalDays;

                    var item = new ListViewItem(rp.Sequence_Number.ToString());
                    item.SubItems.Add(rp.Creation_Time.ToString("yyyy-MM-dd HH:mm:ss"));
                    item.SubItems.Add(ageDays.ToString("0.0"));
                    item.SubItems.Add(rp.Type_Name);
                    item.SubItems.Add(rp.Event_Name);
                    item.SubItems.Add(rp.Description);
                    item.SubItems.Add(rp.Linked_Shadow_Id != null ? "Linked" : "—");
                    item.Tag = rp;

                    // Visual grouping hints
                    if (rp.Restore_Point_Type == 10)          // driver install
                        item.ForeColor = Color.DarkBlue;
                    else if (rp.Restore_Point_Type == 0 ||
                             rp.Restore_Point_Type == 1)      // app install/uninstall
                        item.ForeColor = Color.DarkGreen;
                    else if (rp.Restore_Point_Type == 13)     // cancelled
                        item.ForeColor = Color.Gray;

                    lvPoints.Items.Add(item);
                }

                lvPoints.EndUpdate();

                lblSummary.Text = _points.Count == 0
                    ? "No restore points found (System Protection may be off for the system drive)."
                    : _points.Count + " restore point(s)   |   oldest: "
                        + _points[0].Creation_Time.ToString("yyyy-MM-dd HH:mm")
                        + "   |   newest: "
                        + _points[_points.Count - 1].Creation_Time.ToString("yyyy-MM-dd HH:mm")
                        + Deleted_Summary_Text();

                Update_Status_Line();
                Clear_Details();

                // Auto-select newest (last row) so details aren't empty
                if (lvPoints.Items.Count > 0)
                {
                    var last = lvPoints.Items[lvPoints.Items.Count - 1];
                    last.Selected = true;
                    last.EnsureVisible();
                }
            }
            catch (Exception ex)
            {
                lblSummary.Text = "Query failed.";
                Clear_Details();
                txtNotes.Text = "WMI query failed: " + ex.Message +
                                "  Make sure the app is running as Administrator.";
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void Clear_Details()
        {
            txtSeq.Text = "";
            txtCreated.Text = "";
            txtAge.Text = "";
            txtDescription.Text = "";
            txtType.Text = "";
            txtEvent.Text = "";
            txtShadowId.Text = "";
            txtDevice.Text = "";
            txtNotes.Text = "";
        }

        // --------------------------------------------------------
        //  Selection -> details fields
        // --------------------------------------------------------
        private void Lv_Selection_Changed(object sender, EventArgs e)
        {
            if (lvPoints.SelectedItems.Count == 0) return;

            var rp = lvPoints.SelectedItems[0].Tag as Restore_Point_Info;
            if (rp == null) return;

            var age = DateTime.Now - rp.Creation_Time;

            txtSeq.Text = rp.Sequence_Number.ToString();
            txtCreated.Text = rp.Creation_Time.ToString("yyyy-MM-dd HH:mm:ss");
            txtAge.Text = age.TotalDays.ToString("0.0");
            txtDescription.Text = rp.Description;
            txtType.Text = rp.Type_Name + "  (" + rp.Restore_Point_Type + ")";
            txtEvent.Text = rp.Event_Name + "  (" + rp.Event_Type + ")";

            if (rp.Linked_Shadow_Id != null)
            {
                txtShadowId.Text = rp.Linked_Shadow_Id;
                txtDevice.Text = rp.Linked_Device;
            }
            else
            {
                txtShadowId.Text = "None found";
                txtDevice.Text = "Shadow copy may have been aged out (deleted) " +
                                   "while the restore point metadata remains.";
            }

            // Notes: type hint + gap info + what restoring would remove
            var sb = new StringBuilder();
            sb.AppendLine(Type_Hint(rp.Restore_Point_Type));

            string gap = Gap_Text(rp);
            if (gap.Length > 0) sb.AppendLine(gap);

            string removed = Installed_After_Text(rp);
            if (removed.Length > 0) sb.AppendLine(removed);

            txtNotes.Text = sb.ToString().TrimEnd();
        }

        private static string Type_Hint(uint t)
        {
            switch (t)
            {
                case 0:
                    return "Created by an application installer before making changes. " +
                                "The description is whatever name the installer passed in.";
                case 1: return "Created by an uninstaller before removing an application.";
                case 6:
                    return "Created automatically before a System Restore operation ran, " +
                                "so the restore itself can be undone.";
                case 7:
                    return "A checkpoint - either the scheduled automatic one or one " +
                                "created manually via System Protection.";
                case 10: return "Created by Windows before installing a device driver.";
                case 12: return "Created before a system settings change.";
                case 13: return "The operation that created this point was cancelled before completing.";
                case 14: return "Created by a backup/recovery operation.";
                default: return "No additional notes for this type.";
            }
        }

        // --------------------------------------------------------
        //  Extra data: gaps, installed-after, status line
        // --------------------------------------------------------

        /// <summary>Note when sequence numbers immediately before the
        /// selected point are missing (they were deleted or aged out —
        /// Windows never reuses sequence numbers).</summary>
        private string Gap_Text(Restore_Point_Info rp)
        {
            int idx = _points.IndexOf(rp);
            if (idx <= 0) return "";

            uint prev = _points[idx - 1].Sequence_Number;
            uint cur = rp.Sequence_Number;
            if (cur - prev <= 1) return "";

            uint missing = cur - prev - 1;
            return missing == 1
                ? "Gap: point #" + (prev + 1) + " no longer exists (deleted or aged out)."
                : "Gap: points #" + (prev + 1) + " through #" + (cur - 1) + " (" + missing +
                  " total) no longer exist (deleted or aged out).";
        }

        private string Deleted_Summary_Text()
        {
            uint missing = 0;
            for (int i = 1; i < _points.Count; i++)
                missing += _points[i].Sequence_Number - _points[i - 1].Sequence_Number - 1;

            return missing == 0 ? "" : "   |   " + missing + " deleted in this range";
        }

        /// <summary>Programs whose registry InstallDate is on/after the
        /// point's date — i.e. what a rollback to this point would remove.
        /// InstallDate is day-granular and some installers omit it, so
        /// this is best-effort.</summary>
        private string Installed_After_Text(Restore_Point_Info rp)
        {
            if (_installedPrograms.Count == 0) return "";

            var after = _installedPrograms
                .Where(p => p.Value.Date >= rp.Creation_Time.Date)
                .OrderBy(p => p.Value)
                .ToList();

            if (after.Count == 0)
                return "Restoring to this point would remove no currently installed programs " +
                       "(per registry install dates).";

            const int maxShown = 12;
            var sb = new StringBuilder();
            sb.Append("Restoring to this point would remove (installed on/after its date, day precision): ");
            sb.Append(string.Join(", ", after.Take(maxShown).Select(p => p.Key)));
            if (after.Count > maxShown)
                sb.Append(", and " + (after.Count - maxShown) + " more");
            sb.Append(".");
            return sb.ToString();
        }

        private static List<KeyValuePair<string, DateTime>> Load_Installed_Programs()
        {
            var result = new List<KeyValuePair<string, DateTime>>();
            try
            {
                Read_Uninstall_Key(Registry.LocalMachine,
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", result);
                Read_Uninstall_Key(Registry.LocalMachine,
                    @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall", result);
                Read_Uninstall_Key(Registry.CurrentUser,
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", result);
            }
            catch
            {
                // best-effort; an unreadable hive just means a shorter list
            }
            return result;
        }

        private static void Read_Uninstall_Key(RegistryKey root, string path,
            List<KeyValuePair<string, DateTime>> result)
        {
            using (var key = root.OpenSubKey(path))
            {
                if (key == null) return;

                foreach (var subName in key.GetSubKeyNames())
                {
                    using (var app = key.OpenSubKey(subName))
                    {
                        var name = app?.GetValue("DisplayName") as string;
                        var dateStr = app?.GetValue("InstallDate") as string;
                        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(dateStr))
                            continue;

                        if (DateTime.TryParseExact(dateStr, "yyyyMMdd",
                                CultureInfo.InvariantCulture, DateTimeStyles.None,
                                out DateTime installed))
                        {
                            result.Add(new KeyValuePair<string, DateTime>(name, installed));
                        }
                    }
                }
            }
        }

        /// <summary>Second header line: protection / throttle / shadow storage.</summary>
        private void Update_Status_Line()
        {
            string protection = Restore_Point_Creator.Is_System_Restore_Enabled()
                ? "On" : "OFF";

            int freq = Restore_Point_Creator.Get_Creation_Frequency_Minutes();
            string throttle = freq == 0
                ? "disabled (every request honored)"
                : (freq / 60.0).ToString("0.#") + " hour window";

            lblStatus.Text = "Protection: " + protection
                + "   |   Creation throttle: " + throttle
                + "   |   " + Get_Shadow_Storage_Summary();
        }

        private static string Get_Shadow_Storage_Summary()
        {
            try
            {
                var scope = new ManagementScope(@"\\.\root\cimv2");
                scope.Connect();

                ulong used = 0, max = 0;
                bool found = false, unbounded = false;

                using (var searcher = new ManagementObjectSearcher(
                    scope, new ObjectQuery("SELECT UsedSpace, MaxSpace FROM Win32_ShadowStorage")))
                {
                    foreach (ManagementObject mo in searcher.Get())
                    {
                        found = true;
                        used += (ulong)mo["UsedSpace"];

                        ulong m = (ulong)mo["MaxSpace"];
                        if (m == ulong.MaxValue) unbounded = true;
                        else max += m;
                    }
                }

                if (!found) return "Shadow storage: none allocated";

                string maxText = unbounded ? "unbounded" : Format_GB(max);
                return "Shadow storage: " + Format_GB(used) + " used / " + maxText + " max";
            }
            catch
            {
                return "Shadow storage: unavailable";
            }
        }

        private static string Format_GB(ulong bytes)
        {
            return (bytes / 1073741824.0).ToString("0.0") + " GB";
        }

        // --------------------------------------------------------
        //  Browse the snapshot's file system (read-only)
        // --------------------------------------------------------
        private void Btn_Browse_Click(object sender, EventArgs e)
        {
            if (lvPoints.SelectedItems.Count == 0)
            {
                MessageBox.Show(this, "Select a restore point first.",
                    "Browse Snapshot", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var rp = lvPoints.SelectedItems[0].Tag as Restore_Point_Info;
            if (rp == null) return;

            if (rp.Linked_Device == null)
            {
                MessageBox.Show(this,
                    "This restore point has no linked shadow copy, so there is " +
                    "no snapshot file system to browse.",
                    "Browse Snapshot", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string linkPath = Path.Combine(Path.GetTempPath(),
                "RestorePoint_" + rp.Sequence_Number);

            try
            {
                if (!Directory.Exists(linkPath))
                {
                    // Target must end with a backslash or the link won't browse.
                    Directory.CreateSymbolicLink(linkPath, rp.Linked_Device + @"\");
                    _snapshotLinks.Add(linkPath);
                }

                Process.Start("explorer.exe", linkPath);
                lblSummary.Text = "Snapshot #" + rp.Sequence_Number +
                    " mounted read-only at " + linkPath +
                    " — the link is removed when this window closes.";
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Could not mount the snapshot:\n" + ex.Message,
                    "Browse Snapshot", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            // Remove any snapshot symlinks we created. Deleting a directory
            // symlink removes only the link, never the snapshot behind it.
            foreach (var link in _snapshotLinks)
            {
                try
                {
                    if (Directory.Exists(link))
                        Directory.Delete(link, false);
                }
                catch
                {
                    // stale link in %TEMP% is harmless; ignore
                }
            }
            base.OnFormClosed(e);
        }

        // --------------------------------------------------------
        //  Buttons
        // --------------------------------------------------------
        private void Btn_Refresh_Click(object sender, EventArgs e)
        {
            Load_Points();
        }

        private void Btn_Copy_Click(object sender, EventArgs e)
        {
            if (txtSeq.Text.Length == 0) return;

            var sb = new StringBuilder();
            sb.AppendLine("RESTORE POINT DETAILS");
            sb.AppendLine(new string('=', 55));
            sb.AppendLine("Sequence #    : " + txtSeq.Text);
            sb.AppendLine("Created       : " + txtCreated.Text);
            sb.AppendLine("Age           : " + txtAge.Text + " days");
            sb.AppendLine("Description   : " + txtDescription.Text);
            sb.AppendLine("Type          : " + txtType.Text);
            sb.AppendLine("Event         : " + txtEvent.Text);
            sb.AppendLine("Shadow ID     : " + txtShadowId.Text);
            sb.AppendLine("Device        : " + txtDevice.Text);
            sb.AppendLine("Notes         : " + txtNotes.Text);
            sb.AppendLine("Status        : " + lblStatus.Text);

            Clipboard.SetText(sb.ToString());
            lblSummary.Text = "Details copied to clipboard.";
        }

        private void Btn_Close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Create_Restore_Point_Button_Click(object sender, EventArgs e)
        {
            Create_Button_Click(sender, e);
        }

        private void Delete_Selected_Restore_Point_Button_Click(object sender, EventArgs e)
        {
            Delete_Selected_Button_Click(sender, e);
        }
    }
}