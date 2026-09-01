// ============================================================
//  Shadow_Copy_Form.cs   (C# 7.3 / .NET Framework)
//  Dedicated window for Volume Shadow Copy management:
//    - list all snapshots (newest first) with drive letters
//    - live storage summary (used / allocated / max)
//    - create a snapshot of any fixed NTFS drive
//    - per-snapshot detail view (WMI properties)
//    - raw vssadmin output for troubleshooting
//    - Delete Selected / Delete Older Than / Keep Newest
//      (destructive ops via Snapshot_Delete_Manager)
//
//  Pairs with Shadow_Copy_Form.Designer.cs.
//  Requires:  System.Management reference, elevation.
//
//  Usage from MainForm:
//      using (var f = new Shadow_Copy_Form())
//          f.ShowDialog(this);
// ============================================================

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Trace_Execution_Namespace;
using static Trace_Execution_Namespace.Trace_Execution;

namespace Admin_Tools
{
    public partial class Shadow_Copy_Form : Form
    {
        // Per-row data carried in ListViewItem.Tag
        private sealed class Snapshot_Row
        {
            public string Id;
            public string Device;
            public string VolumeName;
            public string Drive;
            public DateTime Created;
            public ManagementObject Raw;   // kept for the details view
        }

        public Shadow_Copy_Form()
        {
            InitializeComponent();
            Fill_Drive_Combo();
            Load_Snapshots();
        }

        // --------------------------------------------------------
        //  Drive combo — fixed NTFS drives, system drive first
        // --------------------------------------------------------
        private void Fill_Drive_Combo()
        {
            cmbDrive.Items.Clear();

            string sysDrive = Path.GetPathRoot(Environment.SystemDirectory)
                                  .TrimEnd('\\');   // "C:"

            foreach (var d in DriveInfo.GetDrives())
            {
                if (d.DriveType != DriveType.Fixed) continue;
                if (!d.IsReady) continue;
                if (!string.Equals(d.DriveFormat, "NTFS",
                        StringComparison.OrdinalIgnoreCase)) continue;

                string letter = d.Name.TrimEnd('\\');   // "C:"
                if (string.Equals(letter, sysDrive, StringComparison.OrdinalIgnoreCase))
                    cmbDrive.Items.Insert(0, letter);
                else
                    cmbDrive.Items.Add(letter);
            }

            if (cmbDrive.Items.Count > 0)
                cmbDrive.SelectedIndex = 0;
        }

        // --------------------------------------------------------
        //  Snapshot list
        // --------------------------------------------------------
        private async void Load_Snapshots()
        {
            using var Block = Trace_Block.Start_If_Enabled();
            Cursor = Cursors.WaitCursor;
            try
            {
                // Off the UI thread: two WMI enumerations plus the per-row
                // property reads. Keeps the window responsive and lets the
                // trace window paint entries as they happen.
                var rows = await Task.Run(() =>
                {
                    var volumeToDrive = Get_Volume_To_Drive_Map();
                    var result = new List<Snapshot_Row>();

                    using (var searcher = new ManagementObjectSearcher(
                        @"root\cimv2", "SELECT * FROM Win32_ShadowCopy"))
                    {
                        foreach (ManagementObject mo in searcher.Get())
                        {
                            var row = new Snapshot_Row();
                            row.Id = Prop(mo, "ID");
                            row.Device = Prop(mo, "DeviceObject");
                            row.VolumeName = Prop(mo, "VolumeName");
                            row.Raw = mo;

                            string drive;
                            row.Drive = volumeToDrive.TryGetValue(row.VolumeName, out drive)
                                ? drive : "?";

                            string installDate = Prop(mo, "InstallDate");
                            row.Created = installDate.Length == 0
                                ? DateTime.MinValue
                                : ManagementDateTimeConverter.ToDateTime(installDate);

                            result.Add(row);
                        }
                    }

                    result.Sort((a, b) => b.Created.CompareTo(a.Created));   // newest first
                    return result;
                });

                lvSnapshots.BeginUpdate();
                lvSnapshots.Items.Clear();

                foreach (var row in rows)
                {
                    double ageDays = (DateTime.Now - row.Created).TotalDays;

                    bool clientAccessible = Prop(row.Raw, "ClientAccessible") == "True";

                    var item = new ListViewItem(
                        row.Created.ToString("yyyy-MM-dd HH:mm:ss"));
                    item.SubItems.Add(row.Drive);
                    item.SubItems.Add(ageDays.ToString("0.0"));
                    item.SubItems.Add(clientAccessible
                        ? "Previous Versions" : "Backup / Other");
                    item.SubItems.Add(row.Id);
                    item.Tag = row;

                    lvSnapshots.Items.Add(item);
                }

                lvSnapshots.EndUpdate();

                Update_Summary(rows.Count);
            }
            catch (Exception Ex)
            {
                lblSummary.Text = "Query failed: " + Ex.Message +
                                  "  (Is the app running as Administrator?)";
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void Update_Summary(int count)
        {
            using var Block = Trace_Block.Start_If_Enabled();
            string storage = Get_Storage_Summary();
            lblSummary.Text = count + " snapshot(s)"
                + (storage.Length > 0 ? "   |   " + storage : "");
        }

        // Maps \\?\Volume{guid}\ -> drive letter
        private static Dictionary<string, string> Get_Volume_To_Drive_Map()
        {
            using var Block = Trace_Block.Start_If_Enabled();
            var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            using (var searcher = new ManagementObjectSearcher(
                @"root\cimv2",
                "SELECT DeviceID, DriveLetter FROM Win32_Volume WHERE DriveLetter IS NOT NULL"))
            {
                foreach (ManagementObject mo in searcher.Get())
                {
                    string dev = Prop(mo, "DeviceID");
                    string letter = Prop(mo, "DriveLetter");
                    if (dev.Length > 0 && letter.Length > 0)
                        map[dev] = letter;
                }
            }

            return map;
        }

        // --------------------------------------------------------
        //  Storage summary (Win32_ShadowStorage, all volumes)
        // --------------------------------------------------------
        private static string Get_Storage_Summary()
        {
            using var Block = Trace_Block.Start_If_Enabled();
            try
            {
                ulong used = 0, allocated = 0, max = 0;
                bool unlimited = false;

                using (var searcher = new ManagementObjectSearcher(
                    @"root\cimv2",
                    "SELECT UsedSpace, AllocatedSpace, MaxSpace FROM Win32_ShadowStorage"))
                {
                    foreach (ManagementObject mo in searcher.Get())
                    {
                        used += (ulong)mo["UsedSpace"];
                        allocated += (ulong)mo["AllocatedSpace"];

                        ulong m = (ulong)mo["MaxSpace"];
                        // The "unbounded" sentinel is a near-max ulong
                        if (m > 0xF000000000000000UL) unlimited = true;
                        else max += m;
                    }
                }

                string maxText = unlimited ? "Unlimited" : Format_Gb(max);
                return "Storage: " + Format_Gb(used) + " used, "
                     + Format_Gb(allocated) + " allocated, max " + maxText;
            }
            catch
            {
                return "";
            }
        }

        private static string Format_Gb(ulong bytes)
        {
            return (bytes / (1024.0 * 1024.0 * 1024.0)).ToString("0.00") + " GB";
        }

        private static string Prop(ManagementObject mo, string name)
        {
            object v = mo[name];
            return v == null ? "" : v.ToString();
        }

        // --------------------------------------------------------
        //  Create snapshot
        // --------------------------------------------------------
        private void Btn_Create_Click(object Sender, EventArgs e)
        {
            using var Block = Trace_Block.Start_If_Enabled();
            if (cmbDrive.SelectedItem == null) return;
            string drive = cmbDrive.SelectedItem.ToString();   // "C:"

            Cursor = Cursors.WaitCursor;
            try
            {
                using (var shadowClass = new ManagementClass(@"root\cimv2",
                        "Win32_ShadowCopy", null))
                {
                    ManagementBaseObject inParams =
                        shadowClass.GetMethodParameters("Create");
                    inParams["Volume"] = drive + "\\";
                    inParams["Context"] = "ClientAccessible";

                    ManagementBaseObject outParams =
                        shadowClass.InvokeMethod("Create", inParams, null);

                    uint rc = (uint)outParams["ReturnValue"];
                    if (rc == 0)
                    {
                        lblSummary.Text = "Snapshot of " + drive + " created: "
                            + outParams["ShadowID"];
                        Load_Snapshots();
                    }
                    else
                    {
                        MessageBox.Show(
                            "Create failed (code " + rc + ").\n\n" + Create_Error_Text(rc),
                            "Create Snapshot", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Create failed: " + Ex.Message,
                    "Create Snapshot", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private static string Create_Error_Text(uint rc)
        {
            switch (rc)
            {
                case 1: return "Access denied - run as Administrator.";
                case 2: return "Invalid argument.";
                case 3: return "Volume not found.";
                case 4: return "Volume not supported (must be fixed NTFS).";
                case 5: return "Unsupported shadow copy context.";
                case 6: return "Insufficient storage - check shadow storage allocation.";
                case 7: return "Volume is in use.";
                case 8: return "Maximum number of shadow copies reached (64 per volume).";
                case 9: return "Another shadow copy operation is in progress - retry shortly.";
                case 10: return "Shadow copy provider vetoed the operation.";
                case 11: return "Shadow copy provider not registered.";
                case 12: return "Shadow copy provider failure.";
                default: return "Unknown error.";
            }
        }

        // --------------------------------------------------------
        //  Details — same style as the classic Snapshot Details box
        // --------------------------------------------------------
        private void Btn_Details_Click(object Sender, EventArgs e)
        {
            using var Block = Trace_Block.Start_If_Enabled();
            if (lvSnapshots.SelectedItems.Count == 0) return;

            var row = lvSnapshots.SelectedItems[0].Tag as Snapshot_Row;
            if (row == null) return;

            var mo = row.Raw;
            var sb = new StringBuilder();
            var age = DateTime.Now - row.Created;

            sb.AppendLine("SHADOW COPY DETAILS");
            sb.AppendLine(new string('=', 60));
            sb.AppendLine("Shadow ID      : " + row.Id);
            sb.AppendLine("Created        : " + row.Created.ToString("yyyy-MM-dd HH:mm:ss"));
            sb.AppendLine("Age            : " + age.TotalDays.ToString("0.0") + " days");
            sb.AppendLine("Drive          : " + row.Drive + "  (" + row.VolumeName + ")");
            sb.AppendLine("Device object  : " + row.Device);
            sb.AppendLine("Originating PC : " + Prop(mo, "OriginatingMachine"));
            sb.AppendLine("Service PC     : " + Prop(mo, "ServiceMachine"));
            sb.AppendLine("Provider ID    : " + Prop(mo, "ProviderID"));
            sb.AppendLine("Set ID         : " + Prop(mo, "SetID"));
            sb.AppendLine("State          : " + Prop(mo, "State"));
            sb.AppendLine();
            sb.AppendLine("FLAGS");
            sb.AppendLine(new string('-', 60));
            sb.AppendLine("Client accessible (Previous Versions) : " + Prop(mo, "ClientAccessible"));
            sb.AppendLine("Persistent                            : " + Prop(mo, "Persistent"));
            sb.AppendLine("No auto release                       : " + Prop(mo, "NoAutoRelease"));
            sb.AppendLine("Differential (copy-on-write)          : " + Prop(mo, "Differential"));
            sb.AppendLine("Hardware assisted                     : " + Prop(mo, "HardwareAssisted"));
            sb.AppendLine("Imported                              : " + Prop(mo, "Imported"));
            sb.AppendLine("Transportable                         : " + Prop(mo, "Transportable"));
            sb.AppendLine("Exposed locally                       : " + Prop(mo, "ExposedLocally"));
            sb.AppendLine("Exposed remotely                      : " + Prop(mo, "ExposedRemotely"));
            sb.AppendLine("Not surfaced                          : " + Prop(mo, "NotSurfaced"));
            sb.AppendLine("Created without writers               : " + (Prop(mo, "NoWriters")));

            Show_Text_Window("Snapshot Details - " + row.Id, sb.ToString());
        }

        // --------------------------------------------------------
        //  Raw vssadmin output (selected snapshot, or all if none selected)
        // --------------------------------------------------------
        private async void Btn_VssAdmin_Click(object Sender, EventArgs e)
        {
            using var Block = Trace_Block.Start_If_Enabled();
            // Get the selected snapshot's Shadow ID, if any.
            // *** Adjust these two lines to this form's ListView name and Tag type ***
            string shadowId = null;
            if (lvSnapshots.SelectedItems.Count > 0 &&
                lvSnapshots.SelectedItems[0].Tag is Snapshot_Row sr)
            {
                shadowId = sr.Id;   // "{GUID}" including braces
            }

            string arguments = shadowId == null
                ? "list shadows"
                : "list shadows /shadow=" + shadowId;

            Cursor = Cursors.WaitCursor;
            try
            {
                // vssadmin's read can take a while with many snapshots — off
                // the UI thread so the window (and the trace window) stay live.
                string output = await Task.Run(() =>
                {
                    var psi = new ProcessStartInfo
                    {
                        FileName = "vssadmin.exe",
                        Arguments = arguments,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    };

                    using (var p = Process.Start(psi))
                    {
                        string stdOut = p.StandardOutput.ReadToEnd();
                        string stdErr = p.StandardError.ReadToEnd();
                        p.WaitForExit();
                        return stdOut + (stdErr.Length > 0 ? "\r\n" + stdErr : "");
                    }
                });

                string title = shadowId == null
                    ? "vssadmin list shadows (all)"
                    : "vssadmin - " + shadowId;

                Show_Text_Window(title, output);
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Could not run vssadmin: " + Ex.Message,
                    "VSS Details", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        // --------------------------------------------------------
        //  Delete Selected
        // --------------------------------------------------------
        private void Btn_Delete_Selected_Click(object Sender, EventArgs e)
        {
            using var Block = Trace_Block.Start_If_Enabled();
            if (lvSnapshots.SelectedItems.Count == 0)
            {
                MessageBox.Show("Select one or more snapshots first "
                    + "(Ctrl+click / Shift+click for multiple).",
                    "Delete Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var ids = new List<string>();
            bool newestSelected = lvSnapshots.Items.Count > 0
                && lvSnapshots.Items[0].Selected;

            foreach (ListViewItem item in lvSnapshots.SelectedItems)
            {
                var row = item.Tag as Snapshot_Row;
                if (row != null) ids.Add(row.Id);
            }

            string warning =
                "Permanently delete " + ids.Count + " snapshot(s)?\n\n" +
                "\u2022 There is NO undo.\n" +
                "\u2022 Any restore point linked to a deleted snapshot dies with it.\n" +
                "\u2022 'Previous Versions' history from these points in time is lost." +
                (newestSelected
                    ? "\n\n\u26A0 The selection includes the NEWEST snapshot - " +
                      "your most recent recovery point."
                    : "");

            var answer = MessageBox.Show(warning, "Delete Snapshots",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (answer != DialogResult.Yes) return;

            Run_Delete(delegate { return Snapshot_Delete_Manager.Delete_By_Ids(ids); });
        }

        // --------------------------------------------------------
        //  Delete Older Than...
        // --------------------------------------------------------
        private void Btn_Delete_Older_Click(object Sender, EventArgs e)
        {
            using var Block = Trace_Block.Start_If_Enabled();
            int days;
            if (!Prompt_Number("Delete Older Than",
                    "Delete all snapshots older than (days):", 30, 1, 3650, out days))
                return;

            DateTime cutoff = DateTime.Now.AddDays(-days);

            var answer = MessageBox.Show(
                "Permanently delete every snapshot (all drives) created before\n"
                + cutoff.ToString("yyyy-MM-dd HH:mm") + "?\n\nThere is NO undo, and "
                + "restore points linked to deleted snapshots die with them.",
                "Delete Older Than", MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (answer != DialogResult.Yes) return;

            Run_Delete(delegate
            {
                return Snapshot_Delete_Manager.Delete_Older_Than(cutoff, null);
            });
        }

        // --------------------------------------------------------
        //  Keep Newest...
        // --------------------------------------------------------
        private void Btn_Keep_Newest_Click(object Sender, EventArgs e)
        {
            using var Block = Trace_Block.Start_If_Enabled();
            int keep;
            if (!Prompt_Number("Keep Newest",
                    "Keep only the newest N snapshots (all drives combined per drive), " +
                    "delete the rest. N =", 10, 1, 64, out keep))
                return;

            var answer = MessageBox.Show(
                "Keep the newest " + keep + " snapshot(s) and permanently delete "
                + "all older ones?\n\nThere is NO undo, and restore points linked to "
                + "deleted snapshots die with them.",
                "Keep Newest", MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (answer != DialogResult.Yes) return;

            Run_Delete(delegate
            {
                return Snapshot_Delete_Manager.Keep_Newest(keep, null);
            });
        }

        // --------------------------------------------------------
        //  Shared delete runner
        // --------------------------------------------------------
        private async void Run_Delete(Func<Snapshot_Delete_Result> operation)
        {
            using var Block = Trace_Block.Start_If_Enabled();
            Cursor = Cursors.WaitCursor;
            Snapshot_Delete_Result result;
            try
            {
                // Deleting many snapshots is a WMI call per snapshot — off the
                // UI thread so the window stays responsive for a large batch.
                result = await Task.Run(operation);
            }
            finally
            {
                Cursor = Cursors.Default;
            }

            string msg = result.Deleted_Count + " deleted, "
                       + result.Failed_Count + " failed.";
            if (result.Errors.Count > 0)
                msg += "\n\n" + string.Join("\n", result.Errors.ToArray());

            MessageBox.Show(msg, "Delete Snapshots", MessageBoxButtons.OK,
                result.Failed_Count == 0
                    ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

            Load_Snapshots();
        }

        // --------------------------------------------------------
        //  Small numeric prompt dialog (built in code)
        // --------------------------------------------------------
        private bool Prompt_Number(string title, string label,
            int defaultValue, int min, int max, out int value)
        {
            using var Block = Trace_Block.Start_If_Enabled();
            value = defaultValue;

            using (var form = new Form())
            {
                form.Text = title;
                form.ClientSize = new Size(360, 110);
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.StartPosition = FormStartPosition.CenterParent;
                form.MinimizeBox = false;
                form.MaximizeBox = false;
                form.ShowInTaskbar = false;

                var lbl = new Label
                {
                    Text = label,
                    Location = new Point(12, 12),
                    Size = new Size(336, 34)
                };

                var num = new NumericUpDown
                {
                    Minimum = min,
                    Maximum = max,
                    Value = defaultValue,
                    Location = new Point(12, 50),
                    Width = 100
                };

                var ok = new Button
                {
                    Text = "OK",
                    DialogResult = DialogResult.OK,
                    Location = new Point(176, 76),
                    Size = new Size(80, 26)
                };

                var cancel = new Button
                {
                    Text = "Cancel",
                    DialogResult = DialogResult.Cancel,
                    Location = new Point(262, 76),
                    Size = new Size(80, 26)
                };

                form.Controls.Add(lbl);
                form.Controls.Add(num);
                form.Controls.Add(ok);
                form.Controls.Add(cancel);
                form.AcceptButton = ok;
                form.CancelButton = cancel;

                if (form.ShowDialog(this) != DialogResult.OK) return false;

                value = (int)num.Value;
                return true;
            }
        }

        // --------------------------------------------------------
        //  Monospace text viewer (details / vssadmin output)
        // --------------------------------------------------------
        private void Show_Text_Window(string title, string text)
        {
            using var Block = Trace_Block.Start_If_Enabled();
            var form = new Form
            {
                Text = title,
                Width = 780,
                Height = 640,
                StartPosition = FormStartPosition.CenterParent,
                MinimizeBox = false,
                ShowInTaskbar = false
            };

            var txt = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Both,
                WordWrap = false,
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 10f),
                BackColor = Color.White,
                Text = text
            };

            var BtnCloseViewer = new Button { Text = "Close", Width = 90 };
            BtnCloseViewer.Click += delegate { form.Close(); };

            var panel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                FlowDirection = FlowDirection.RightToLeft,
                Height = 45,
                Padding = new Padding(8)
            };
            panel.Controls.Add(BtnCloseViewer);

            form.Controls.Add(txt);
            form.Controls.Add(panel);
            form.AcceptButton = BtnCloseViewer;
            form.Shown += delegate { txt.SelectionStart = 0; txt.SelectionLength = 0; };

            form.Show(this);
        }

        private void Btn_Refresh_Click(object Sender, EventArgs e)
        {
            using var Block = Trace_Block.Start_If_Enabled();
            Load_Snapshots();
        }

        private void Btn_Close_Click(object Sender, EventArgs e)
        {
            using var Block = Trace_Block.Start_If_Enabled();
            Close();
        }

        private void Reclaim_Space_Button_Click(object Sender, EventArgs e)
        {
            using var Block = Trace_Block.Start_If_Enabled();
            using (var f = new Storage_Reclaim_Form())
                f.ShowDialog(this);

            Load_Snapshots();   // resize may have deleted snapshots — refresh the grid
        }
   
    }
}
