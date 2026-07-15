using System;
using System.Collections.Generic;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Admin_Tools
{
    /// <summary>
    /// Creates system restore points via WMI, with helpers for the two Windows
    /// quirks that make creation silently fail: the 24-hour throttle and
    /// System Restore being disabled on the system drive.
    /// </summary>
    public static class Restore_Point_Creator
    {
        private const string Sr_Registry_Key =
            @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore";

        /// <summary>
        /// Creates a restore point. Returns true on success.
        /// NOTE: a return of true only means Windows accepted the call — if the
        /// throttle window is active, Windows reports success but skips creation.
        /// Check the throttle first and refresh the list afterward to confirm.
        /// </summary>
        public static bool Create_Restore_Point(string description, out string error)
        {
            error = null;
            try
            {
                var scope = new ManagementScope(@"\\.\root\default");
                scope.Connect();

                var path = new ManagementPath("SystemRestore");
                using (var sysRestore = new ManagementClass(scope, path, new ObjectGetOptions()))
                {
                    var inParams = sysRestore.GetMethodParameters("CreateRestorePoint");
                    inParams["Description"] = description;
                    inParams["RestorePointType"] = 12;   // MODIFY_SETTINGS (0 = APPLICATION_INSTALL)
                    inParams["EventType"] = 100;         // BEGIN_SYSTEM_CHANGE

                    var outParams = sysRestore.InvokeMethod("CreateRestorePoint", inParams, null);
                    uint result = (uint)outParams["ReturnValue"];

                    if (result != 0)
                    {
                        error = "CreateRestorePoint returned " + result +
                                (result == 1058 ? " (System Restore service is disabled)" : "");
                        return false;
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>Enables or disables System Restore protection on a drive (e.g. @"C:\").
        /// WARNING: disabling deletes all existing restore points immediately.</summary>
        public static bool Set_System_Restore(bool enable, string drive, out string error)
        {
            error = null;
            try
            {
                var scope = new ManagementScope(@"\\.\root\default");
                scope.Connect();

                var path = new ManagementPath("SystemRestore");
                using (var sysRestore = new ManagementClass(scope, path, new ObjectGetOptions()))
                {
                    string method = enable ? "Enable" : "Disable";
                    var inParams = sysRestore.GetMethodParameters(method);
                    inParams["Drive"] = drive;

                    var outParams = sysRestore.InvokeMethod(method, inParams, null);
                    uint result = (uint)outParams["ReturnValue"];
                    if (result != 0)
                    {
                        error = method + " returned " + result;
                        return false;
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Minutes Windows waits between allowed restore point creations.
        /// Default is 1440 (24 hours) when the registry value is absent.
        /// 0 means the throttle is disabled.
        /// </summary>
        public static int Get_Creation_Frequency_Minutes()
        {
            using (var key = Registry.LocalMachine.OpenSubKey(Sr_Registry_Key))
            {
                return key?.GetValue("SystemRestorePointCreationFrequency") is int minutes
                    ? minutes
                    : 1440;
            }
        }

        /// <summary>
        /// Sets SystemRestorePointCreationFrequency = 0 so every creation
        /// request is honored. Requires elevation (which this app has).
        /// </summary>
        public static bool Disable_Throttle(out string error)
        {
            error = null;
            try
            {
                using (var key = Registry.LocalMachine.CreateSubKey(Sr_Registry_Key, writable: true))
                {
                    key.SetValue("SystemRestorePointCreationFrequency", 0, RegistryValueKind.DWord);
                    return true;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Creation time of the newest existing restore point, or null if none
        /// exist (or WMI is unavailable). Used to predict the throttle.
        /// </summary>
        public static DateTime? Get_Newest_Creation_Time()
        {
            try
            {
                var scope = new ManagementScope(@"\\.\root\default");
                scope.Connect();

                using (var searcher = new ManagementObjectSearcher(
                    scope, new ObjectQuery("SELECT CreationTime FROM SystemRestore")))
                {
                    DateTime? newest = null;
                    foreach (ManagementObject mo in searcher.Get())
                    {
                        var created = ManagementDateTimeConverter.ToDateTime((string)mo["CreationTime"]);
                        if (!newest.HasValue || created > newest.Value)
                            newest = created;
                    }
                    return newest;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Best-effort check that System Restore is active on the system drive
        /// (RPSessionInterval = 1 when protection is on).
        /// </summary>
        public static bool Is_System_Restore_Enabled()
        {
            using (var key = Registry.LocalMachine.OpenSubKey(Sr_Registry_Key))
            {
                return key?.GetValue("RPSessionInterval") is int interval && interval > 0;
            }
        }
    }

    /// <summary>
    /// Create / Delete Selected handlers for the Restore Points form.
    /// Wire the buttons' Click events to Create_Button_Click and
    /// Delete_Selected_Button_Click in the designer.
    /// </summary>
    public partial class Restore_Point_List_Form
    {
        private async void Create_Button_Click(object sender, EventArgs e)
        {
            var createButton = (Button)sender;

            // 1. Make sure System Restore is on — offer to enable it if not.
            if (!Restore_Point_Creator.Is_System_Restore_Enabled())
            {
                var enableAnswer = MessageBox.Show(this,
                    "System Restore is not enabled on the system drive.\n\n" +
                    "Enable it now?",
                    "Restore Points", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (enableAnswer != DialogResult.Yes)
                    return;

                if (!Restore_Point_Creator.Set_System_Restore(true, @"C:\", out string srError))
                {
                    MessageBox.Show(this, "Could not enable System Restore:\n" + srError,
                        "Restore Points", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // 2. Predict the 24-hour throttle and offer to disable it.
            int frequencyMinutes = Restore_Point_Creator.Get_Creation_Frequency_Minutes();
            DateTime? newest = Restore_Point_Creator.Get_Newest_Creation_Time();

            if (frequencyMinutes > 0 && newest.HasValue &&
                DateTime.Now - newest.Value < TimeSpan.FromMinutes(frequencyMinutes))
            {
                TimeSpan age = DateTime.Now - newest.Value;
                var answer = MessageBox.Show(this,
                    "The newest restore point is only " + age.TotalHours.ToString("0.0") +
                    " hours old. Windows will silently skip creating a new one inside its " +
                    (frequencyMinutes / 60.0).ToString("0.#") + "-hour throttle window.\n\n" +
                    "Disable the throttle so restore points are always created?\n" +
                    "(Sets SystemRestorePointCreationFrequency = 0 in the registry.)\n\n" +
                    "Yes = disable throttle and create\n" +
                    "No = try anyway (likely skipped)\n" +
                    "Cancel = do nothing",
                    "Creation Throttle Active",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (answer == DialogResult.Cancel)
                    return;

                if (answer == DialogResult.Yes &&
                    !Restore_Point_Creator.Disable_Throttle(out string regError))
                {
                    MessageBox.Show(this, "Could not update the registry:\n" + regError,
                        "Restore Points", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // 3. Ask for a description.
            string description = Prompt_For_Description("AdminToolkit manual restore point");
            if (description == null)
                return; // cancelled

            // 4. Create off the UI thread. await resumes back on the UI thread,
            //    so no BeginInvoke is needed for the updates afterward.
            createButton.Enabled = false;
            Cursor = Cursors.WaitCursor;

            string error = null;
            bool ok = await Task.Run(() =>
                Restore_Point_Creator.Create_Restore_Point(description, out error));

            Cursor = Cursors.Default;
            createButton.Enabled = true;

            if (ok)
            {
                Load_Points();   // new point appears in the list
            }
            else
            {
                MessageBox.Show(this, "Failed to create restore point:\n" + error,
                    "Restore Points", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Delete_Selected_Button_Click(object sender, EventArgs e)
        {
            if (lvPoints.SelectedItems.Count == 0)
            {
                MessageBox.Show("Select one or more restore points first "
                    + "(Ctrl+click / Shift+click for multiple).",
                    "Delete Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var seqs = new List<uint>();
            var names = new StringBuilder();

            foreach (ListViewItem item in lvPoints.SelectedItems)
            {
                var rp = item.Tag as Restore_Point_Info;
                if (rp == null) continue;
                seqs.Add(rp.Sequence_Number);
                names.AppendLine("  #" + rp.Sequence_Number + "  " + rp.Description);
            }

            var answer = MessageBox.Show(
                "Permanently delete " + seqs.Count + " restore point(s)?\n\n" + names +
                "\nThis also deletes each point's underlying shadow copy.\n" +
                "There is NO undo — you cannot restore the system to these\n" +
                "points afterward.",
                "Delete Restore Points", MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (answer != DialogResult.Yes) return;

            Cursor = Cursors.WaitCursor;
            int deleted, failed;
            List<string> errors;
            Restore_Point_Delete.Delete_Many(seqs, out deleted, out failed, out errors);
            Cursor = Cursors.Default;

            string msg = deleted + " deleted, " + failed + " failed.";
            if (errors.Count > 0) msg += "\n\n" + string.Join("\n", errors.ToArray());

            MessageBox.Show(msg, "Delete Restore Points", MessageBoxButtons.OK,
                failed == 0 ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

            Load_Points();
        }

        /// <summary>Small modal prompt for the restore point description.</summary>
        private static string Prompt_For_Description(string defaultText)
        {
            using (var dialog = new Form())
            using (var textBox = new TextBox())
            using (var okButton = new Button())
            using (var cancelButton = new Button())
            {
                dialog.Text = "Create Restore Point";
                dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.MinimizeBox = false;
                dialog.MaximizeBox = false;
                dialog.ClientSize = new System.Drawing.Size(420, 110);

                var label = new Label
                {
                    Text = "Description:",
                    AutoSize = true,
                    Left = 12,
                    Top = 15
                };

                textBox.Text = defaultText;
                textBox.Left = 12;
                textBox.Top = 38;
                textBox.Width = 396;
                textBox.SelectAll();

                okButton.Text = "Create";
                okButton.DialogResult = DialogResult.OK;
                okButton.Left = 252;
                okButton.Top = 72;

                cancelButton.Text = "Cancel";
                cancelButton.DialogResult = DialogResult.Cancel;
                cancelButton.Left = 333;
                cancelButton.Top = 72;

                dialog.Controls.Add(label);
                dialog.Controls.Add(textBox);
                dialog.Controls.Add(okButton);
                dialog.Controls.Add(cancelButton);
                dialog.AcceptButton = okButton;
                dialog.CancelButton = cancelButton;

                if (dialog.ShowDialog() != DialogResult.OK)
                    return null;

                string text = textBox.Text.Trim();
                return text.Length == 0 ? null : text;
            }
        }
    }
}
