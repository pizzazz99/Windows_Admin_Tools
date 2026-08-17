using System.Management;
using System.Text;
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
        public static bool Create_Restore_Point(string Description, out string Error)
        {
            Error = null;
            try
            {
                var Scope = new ManagementScope(@"\\.\root\default");
                Scope.Connect();

                var Path = new ManagementPath("SystemRestore");
                using (var SysRestore = new ManagementClass(Scope, Path, new ObjectGetOptions()))
                {
                    var InParams = SysRestore.GetMethodParameters("CreateRestorePoint");
                    InParams["Description"] = Description;
                    InParams["RestorePointType"] = 12;   // MODIFY_SETTINGS (0 = APPLICATION_INSTALL)
                    InParams["EventType"] = 100;         // BEGIN_SYSTEM_CHANGE

                    var OutParams = SysRestore.InvokeMethod("CreateRestorePoint", InParams, null);
                    uint Result = (uint)OutParams["ReturnValue"];
                    if (Result != 0)
                    {
                        Error = "CreateRestorePoint returned " + Result +
                                (Result == 1058 ? " (System Restore service is disabled)" : "");
                        return false;
                    }

                    // Close the change window (END_SYSTEM_CHANGE). Without this, Windows
                    // silently suppresses all further restore point creation from this
                    // process until it exits.
                    var EndParams = SysRestore.GetMethodParameters("CreateRestorePoint");
                    EndParams["Description"] = Description;
                    EndParams["RestorePointType"] = 12;
                    EndParams["EventType"] = 101;   // END_SYSTEM_CHANGE
                    SysRestore.InvokeMethod("CreateRestorePoint", EndParams, null);

                    return true;
                }
            }
            catch (Exception Ex)
            {
                Error = Ex.Message;
                return false;
            }
        }

        /// <summary>Enables or disables System Restore protection on a drive (e.g. @"C:\").
        /// WARNING: disabling deletes all existing restore points immediately.</summary>
        public static bool Set_System_Restore(bool Enable, string Drive, out string Error)
        {
            Error = null;
            try
            {
                var Scope = new ManagementScope(@"\\.\root\default");
                Scope.Connect();

                var Path = new ManagementPath("SystemRestore");
                using (var SysRestore = new ManagementClass(Scope, Path, new ObjectGetOptions()))
                {
                    string Method = Enable ? "Enable" : "Disable";
                    var InParams = SysRestore.GetMethodParameters(Method);
                    InParams["Drive"] = Drive;

                    var OutParams = SysRestore.InvokeMethod(Method, InParams, null);
                    uint Result = (uint)OutParams["ReturnValue"];
                    if (Result != 0)
                    {
                        Error = Method + " returned " + Result;
                        return false;
                    }
                    return true;
                }
            }
            catch (Exception Ex)
            {
                Error = Ex.Message;
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
            using (var Key = Registry.LocalMachine.OpenSubKey(Sr_Registry_Key))
            {
                return Key?.GetValue("SystemRestorePointCreationFrequency") is int Minutes
                    ? Minutes
                    : 1440;
            }
        }

        /// <summary>
        /// Sets SystemRestorePointCreationFrequency = 0 so every creation
        /// request is honored. Requires elevation (which this app has).
        /// </summary>
        public static bool Disable_Throttle(out string Error)
        {
            Error = null;
            try
            {
                using (var Key = Registry.LocalMachine.CreateSubKey(Sr_Registry_Key, writable: true))
                {
                    Key.SetValue("SystemRestorePointCreationFrequency", 0, RegistryValueKind.DWord);
                    return true;
                }
            }
            catch (Exception Ex)
            {
                Error = Ex.Message;
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
                var Scope = new ManagementScope(@"\\.\root\default");
                Scope.Connect();

                using (var Searcher = new ManagementObjectSearcher(
                    Scope, new ObjectQuery("SELECT CreationTime FROM SystemRestore")))
                {
                    DateTime? Newest = null;
                    foreach (ManagementObject Management_Object in Searcher.Get())
                    {
                        var Created = ManagementDateTimeConverter.ToDateTime((string)Management_Object["CreationTime"]);
                        if (!Newest.HasValue || Created > Newest.Value)
                            Newest = Created;
                    }
                    return Newest;
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
            using (var Key = Registry.LocalMachine.OpenSubKey(Sr_Registry_Key))
            {
                return Key?.GetValue("RPSessionInterval") is int Interval && Interval > 0;
            }
        }


        /// <summary>Raw registry value: null when the value is absent
        /// (meaning Windows' 24-hour default applies).</summary>
        public static int? Get_Creation_Frequency_Raw()
        {
            using (var Key = Registry.LocalMachine.OpenSubKey(Sr_Registry_Key))
            {
                return Key?.GetValue("SystemRestorePointCreationFrequency") as int?;
            }
        }

        /// <summary>Sets the throttle value. Pass null to delete the value,
        /// restoring Windows' 24-hour default.</summary>
        public static bool Set_Creation_Frequency(int? Minutes, out string Error)
        {
            Error = null;
            try
            {
                using (var Key = Registry.LocalMachine.CreateSubKey(Sr_Registry_Key, writable: true))
                {
                    if (Minutes.HasValue)
                        Key.SetValue("SystemRestorePointCreationFrequency",
                            Minutes.Value, RegistryValueKind.DWord);
                    else
                        Key.DeleteValue("SystemRestorePointCreationFrequency",
                            throwOnMissingValue: false);
                    return true;
                }
            }
            catch (Exception Ex)
            {
                Error = Ex.Message;
                return false;
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
        private async void Create_Button_Click(object Sender, EventArgs E)
        {
            var CreateButton = (Button)Sender;

            // 1. Make sure System Restore is on — offer to enable it if not.
            if (!Ensure_System_Restore_Enabled())
                return;

            // 2. Ask for a description.
            string Description = Prompt_For_Description("AdminToolkit manual restore point");
            if (Description == null)
                return; // cancelled

            // 3. Temporarily lift the 24-hour throttle if it's active, so the
            //    point is guaranteed to be created (Windows otherwise silently
            //    skips creation and still reports success).
            int? OriginalThrottle = null;
            bool Overridden = false;

            if (Restore_Point_Creator.Get_Creation_Frequency_Minutes() != 0)
            {
                OriginalThrottle = Restore_Point_Creator.Get_Creation_Frequency_Raw();

                if (!Restore_Point_Creator.Set_Creation_Frequency(0, out string RegError))
                {
                    MessageBox.Show(this,
                        "Could not temporarily lift the creation throttle:\n" + RegError +
                        "\n\nWindows may silently skip this restore point.",
                        "Restore Points", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    Overridden = true;
                }
            }

            // Remember the highest sequence number BEFORE creating, so we can
            // detect when the new point becomes visible to WMI.
            uint MaxSeqBefore = _Points.Count > 0
                ? _Points.Max(P => P.Sequence_Number)
                : 0;

            // 4. Create off the UI thread, always restoring the throttle after.
            CreateButton.Enabled = false;
            Cursor = Cursors.WaitCursor;

            string Error = null;
            bool Ok;
            try
            {
                Ok = await Task.Run(() =>
                    Restore_Point_Creator.Create_Restore_Point(Description, out Error));
            }
            finally
            {
                if (Overridden)
                    Restore_Point_Creator.Set_Creation_Frequency(OriginalThrottle, out _);

                Cursor = Cursors.Default;
                CreateButton.Enabled = true;
            }

            if (Ok)
            {
                // The new point can take a few seconds to become visible to the
                // WMI enumeration. Poll until it shows (max ~15 s), then refresh.
                Cursor = Cursors.WaitCursor;
                try
                {
                    for (int I = 0; I < 15; I++)
                    {
                        var Check = await Task.Run(() =>
                            Restore_Point_Manager.Get_Restore_Points());

                        if (Check.Count > 0 &&
                            Check.Max(P => P.Sequence_Number) > MaxSeqBefore)
                            break;

                        await Task.Delay(1000);
                    }
                }
                finally
                {
                    Cursor = Cursors.Default;
                }

                Load_Points();   // new point appears in the list
            }
            else
            {
                MessageBox.Show(this, "Failed to create restore point:\n" + Error,
                    "Restore Points", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Delete_Selected_Button_Click(object Sender, EventArgs E )
        {
            if (lvPoints.SelectedItems.Count == 0)
            {
                MessageBox.Show("Select one or more restore points first "
                    + "(Ctrl+click / Shift+click for multiple).",
                    "Delete Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var Seqs = new List<uint>();
            var Names = new StringBuilder();

            foreach (ListViewItem Item in lvPoints.SelectedItems)
            {
                var Restore_Point = Item.Tag as Restore_Point_Info;
                if (Restore_Point == null) continue;
                Seqs.Add(Restore_Point.Sequence_Number);
                Names.AppendLine("  #" + Restore_Point.Sequence_Number + "  " + Restore_Point.Description);
            }

            var Answer = MessageBox.Show(
                "Permanently delete " + Seqs.Count + " restore point(s)?\n\n" + Names +
                "\nThis also deletes each point's underlying shadow copy.\n" +
                "There is NO undo — you cannot restore the system to these\n" +
                "points afterward.",
                "Delete Restore Points", MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (Answer != DialogResult.Yes) return;

            Cursor = Cursors.WaitCursor;
            int Deleted, Failed;
            List<string> Errors;
            Restore_Point_Delete.Delete_Many(Seqs, out Deleted, out Failed, out Errors);
            Cursor = Cursors.Default;

            string Msg = Deleted + " deleted, " + Failed + " failed.";
            if (Errors.Count > 0) Msg += "\n\n" + string.Join("\n", Errors.ToArray());

            MessageBox.Show(Msg, "Delete Restore Points", MessageBoxButtons.OK,
                Failed == 0 ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

            Load_Points();
        }

        /// <summary>Small modal prompt for the restore point description.</summary>
        private static string Prompt_For_Description(string DefaultText)
        {
            using (var Dialog = new Form())
            using (var TextBox = new TextBox())
            using (var OkButton = new Button())
            using (var CancelButton = new Button())
            {
                Dialog.Text = "Create Restore Point";
                Dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                Dialog.StartPosition = FormStartPosition.CenterParent;
                Dialog.MinimizeBox = false;
                Dialog.MaximizeBox = false;
                Dialog.ClientSize = new System.Drawing.Size(420, 110);

                var Label = new Label
                {
                    Text = "Description:",
                    AutoSize = true,
                    Left = 12,
                    Top = 15
                };

                TextBox.Text = DefaultText;
                TextBox.Left = 12;
                TextBox.Top = 38;
                TextBox.Width = 396;
                TextBox.SelectAll();

                OkButton.Text = "Create";
                OkButton.DialogResult = DialogResult.OK;
                OkButton.Left = 252;
                OkButton.Top = 72;

                CancelButton.Text = "Cancel";
                CancelButton.DialogResult = DialogResult.Cancel;
                CancelButton.Left = 333;
                CancelButton.Top = 72;

                Dialog.Controls.Add(Label);
                Dialog.Controls.Add(TextBox);
                Dialog.Controls.Add(OkButton);
                Dialog.Controls.Add(CancelButton);
                Dialog.AcceptButton = OkButton;
                Dialog.CancelButton = CancelButton;

                if (Dialog.ShowDialog() != DialogResult.OK)
                    return null;

                string Text = TextBox.Text.Trim();
                return Text.Length == 0 ? null : Text;
            }
        }
    }
}
