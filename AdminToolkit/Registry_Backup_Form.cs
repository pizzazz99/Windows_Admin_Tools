// ============================================================
//  Registry_Backup_Form.cs   (C# 7.3 / .NET Framework)
//  Answers: "do I have registry backups?"
//
//  Checks four sources:
//    1. RegBack folder      - built-in periodic hive backup
//    2. EnablePeriodicBackup registry setting
//    3. RegIdleBackup       - the scheduled task that fills RegBack
//    4. Restore points + shadow copies (each contains the hives)
//
//  Pairs with Registry_Backup_Form.Designer.cs.
//  Reuses Restore_Point_Manager for restore point data.
//
//  Usage from MainForm:
//      using (var f = new Registry_Backup_Form())
//          f.ShowDialog(this);
// ============================================================

using System.Diagnostics;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using Trace_Execution_Namespace;
using static Trace_Execution_Namespace.Trace_Execution;

namespace Admin_Tools
{
    public partial class Registry_Backup_Form : Form
    {
        private const string RegBack_Path =
            @"C:\Windows\System32\config\RegBack";

        private const string Periodic_Key =
            @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Configuration Manager";

        private const string Task_Folder = @"\Microsoft\Windows\Registry";
        private const string Task_Name   = "RegIdleBackup";

        private bool _RegBackActive;
        private bool _SettingEnabled;
        private int  _RpCount;
        private int  _ScCount;

        public Registry_Backup_Form()
        {
            InitializeComponent();
            Load_All();
        }

        // --------------------------------------------------------
        //  Master refresh
        // --------------------------------------------------------
        private async void Load_All()
        {
            using var Block = Trace_Block.Start_If_Enabled();
            Cursor = Cursors.WaitCursor;
            try
            {
                // File listing + one registry read — near-instant, stays on
                // the UI thread.
                Check_RegBack_Folder();
                Check_Periodic_Setting();

                // COM Task Scheduler activation and two WMI queries — off
                // the UI thread so the window (and the trace window) stay
                // responsive, since COM activation in particular can be slow.
                var TaskInfo = await Task.Run(() => Fetch_Scheduled_Task_Info());
                Apply_Scheduled_Task_Info(TaskInfo);

                var SnapshotInfo = await Task.Run(() => Fetch_Snapshot_Info());
                Apply_Snapshot_Info(SnapshotInfo);

                Build_Verdict();
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        // --------------------------------------------------------
        //  1. RegBack folder contents
        // --------------------------------------------------------
        private void Check_RegBack_Folder()
        {
            using var Block = Trace_Block.Start_If_Enabled();
            lvHives.BeginUpdate();
            lvHives.Items.Clear();
            _RegBackActive = false;

            txtRegBackPath.Text = RegBack_Path;

            try
            {
                if (!Directory.Exists(RegBack_Path))
                {
                    txtRegBackStatus.Text = "Folder not found";
                    lvHives.EndUpdate();
                    return;
                }

                long TotalBytes = 0;
                var Files = Directory.GetFiles(RegBack_Path);

                foreach (var File in Files)
                {
                    var Info = new FileInfo(File);
                    TotalBytes += Info.Length;

                    var Item = new ListViewItem(Info.Name);
                    Item.SubItems.Add(Format_Size(Info.Length));
                    Item.SubItems.Add(Info.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss"));

                    if (Info.Length == 0)
                        Item.ForeColor = Color.Gray;

                    lvHives.Items.Add(Item);
                }

                _RegBackActive = TotalBytes > 0;

                if (Files.Length == 0)
                    txtRegBackStatus.Text = "Folder is empty";
                else if (_RegBackActive)
                    txtRegBackStatus.Text = "ACTIVE - contains real backups ("
                                            + Format_Size(TotalBytes) + ")";
                else
                    txtRegBackStatus.Text = "INACTIVE - all files are 0 bytes";
            }
            catch (Exception Ex)
            {
                txtRegBackStatus.Text = "Error: " + Ex.Message;
            }

            lvHives.EndUpdate();
        }

        private static string Format_Size(long Bytes)
        {
            if (Bytes == 0) return "0 bytes";
            if (Bytes < 1024 * 1024) return (Bytes / 1024.0).ToString("0.0") + " KB";
            return (Bytes / (1024.0 * 1024.0)).ToString("0.0") + " MB";
        }

        // --------------------------------------------------------
        //  2. EnablePeriodicBackup setting
        // --------------------------------------------------------
        private void Check_Periodic_Setting()
        {
            using var Block = Trace_Block.Start_If_Enabled();
            _SettingEnabled = false;
            try
            {
                object Val = Registry.GetValue(Periodic_Key, "EnablePeriodicBackup", null);

                if (Val == null)
                {
                    txtPeriodic.Text = "Not set (Windows default = disabled)";
                }
                else if (Convert.ToInt32(Val) == 1)
                {
                    _SettingEnabled = true;
                    txtPeriodic.Text = "EnablePeriodicBackup = 1 (enabled)";
                }
                else
                {
                    txtPeriodic.Text = "EnablePeriodicBackup = " + Val + " (disabled)";
                }
            }
            catch (Exception Ex)
            {
                txtPeriodic.Text = "Error: " + Ex.Message;
            }
        }

        // --------------------------------------------------------
        //  3. RegIdleBackup scheduled task (Task Scheduler COM)
        // --------------------------------------------------------
        private sealed class Scheduled_Task_Info
        {
            public bool Found;
            public DateTime LastRun;
            public DateTime NextRun;
            public int Result;
            public int State;
            public bool Enabled;
            public string Error;
        }

        // No UI access — safe to run via Task.Run. COM activation/Connect()
        // is the slow part here.
        private static Scheduled_Task_Info Fetch_Scheduled_Task_Info()
        {
            using var Block = Trace_Block.Start_If_Enabled();
            try
            {
                Type SchedType = Type.GetTypeFromProgID("Schedule.Service");
                dynamic Service = Activator.CreateInstance(SchedType);
                Service.Connect();

                dynamic Folder = Service.GetFolder(Task_Folder);
                dynamic Task   = Folder.GetTask(Task_Name);

                return new Scheduled_Task_Info
                {
                    Found = true,
                    LastRun = Task.LastRunTime,
                    NextRun = Task.NextRunTime,
                    Result = Task.LastTaskResult,
                    State = Task.State,
                    Enabled = Task.Enabled
                };
            }
            catch (Exception Ex)
            {
                return new Scheduled_Task_Info { Found = false, Error = Ex.Message };
            }
        }

        private void Apply_Scheduled_Task_Info(Scheduled_Task_Info Info)
        {
            using var Block = Trace_Block.Start_If_Enabled();
            if (!Info.Found)
            {
                txtTaskLastRun.Text = "Task not found or inaccessible";
                txtTaskResult.Text  = Info.Error;
                txtTaskState.Text   = "";
                txtTaskNextRun.Text = "";
                return;
            }

            txtTaskLastRun.Text = Info.LastRun.Year < 2000
                ? "Never"
                : Info.LastRun.ToString("yyyy-MM-dd HH:mm:ss");

            txtTaskNextRun.Text = Info.NextRun.Year < 2000
                ? "Not scheduled (runs during automatic maintenance)"
                : Info.NextRun.ToString("yyyy-MM-dd HH:mm:ss");

            txtTaskResult.Text = "0x" + Info.Result.ToString("X")
                + (Info.Result == 0 ? " (success)" : "");

            txtTaskState.Text = Task_State_Name(Info.State)
                + (Info.Enabled ? "" : "  [task disabled]");
        }

        private static string Task_State_Name(int State)
        {
            switch (State)
            {
                case 0:  return "Unknown";
                case 1:  return "Disabled";
                case 2:  return "Queued";
                case 3:  return "Ready";
                case 4:  return "Running";
                default: return "State " + State;
            }
        }

        // --------------------------------------------------------
        //  4. Restore points + shadow copies
        // --------------------------------------------------------
        private sealed class Snapshot_Info
        {
            public int RpCount;
            public string RpNewestText;
            public string RpError;
            public int ScCount;
            public string ScNewestText;
            public string ScError;
        }

        // No UI access — safe to run via Task.Run.
        private static Snapshot_Info Fetch_Snapshot_Info()
        {
            using var Block = Trace_Block.Start_If_Enabled();
            var Info = new Snapshot_Info();

            // Restore points — reuse the manager
            try
            {
                var Restore_Points = Restore_Point_Manager.Get_Restore_Points();
                Info.RpCount = Restore_Points.Count;
                Info.RpNewestText = Info.RpCount == 0
                    ? "—"
                    : Restore_Points[0].Creation_Time.ToString("yyyy-MM-dd HH:mm:ss")
                      + "  (" + Restore_Points[0].Description + ")";
            }
            catch (Exception Ex)
            {
                Info.RpError = Ex.Message;
            }

            // Shadow copies
            try
            {
                DateTime Newest = DateTime.MinValue;

                using (var Searcher = new ManagementObjectSearcher(
                    @"root\cimv2", "SELECT InstallDate FROM Win32_ShadowCopy"))
                {
                    foreach (ManagementObject Management_Object in Searcher.Get())
                    {
                        Info.ScCount++;
                        object InstallDate = Management_Object["InstallDate"];
                        if (InstallDate == null) continue;

                        DateTime Created = ManagementDateTimeConverter
                            .ToDateTime(InstallDate.ToString());
                        if (Created > Newest) Newest = Created;
                    }
                }

                Info.ScNewestText = Info.ScCount == 0
                    ? "—"
                    : Newest.ToString("yyyy-MM-dd HH:mm:ss");
            }
            catch (Exception Ex)
            {
                Info.ScError = Ex.Message;
            }

            return Info;
        }

        private void Apply_Snapshot_Info(Snapshot_Info Info)
        {
            using var Block = Trace_Block.Start_If_Enabled();
            _RpCount = Info.RpCount;
            _ScCount = Info.ScCount;

            txtRpCount.Text = Info.RpError != null ? "?" : Info.RpCount.ToString();
            txtRpNewest.Text = Info.RpError != null ? "Error: " + Info.RpError : Info.RpNewestText;

            txtScCount.Text = Info.ScError != null ? "?" : Info.ScCount.ToString();
            txtScNewest.Text = Info.ScError != null ? "Error: " + Info.ScError : Info.ScNewestText;
        }

        // --------------------------------------------------------
        //  Verdict
        // --------------------------------------------------------
        private void Build_Verdict()
        {
            using var Block = Trace_Block.Start_If_Enabled();
            var String_Builder = new StringBuilder();

            if (_RegBackActive)
                String_Builder.Append("The built-in RegBack mechanism is working and holds real hive backups. ");
            else if (_SettingEnabled)
                String_Builder.Append("RegBack is enabled but the folder holds no data yet - it fills the next time " +
                          "RegIdleBackup runs (use Backup Now, then Refresh). ");
            else
                String_Builder.Append("The built-in RegBack mechanism is disabled (the Windows default since 10 v1803). ");

            if (_RpCount > 0 || _ScCount > 0)
            {
                String_Builder.Append("However, you have " + _RpCount + " restore point(s) and "
                          + _ScCount + " shadow cop" + (_ScCount == 1 ? "y" : "ies")
                          + ", and every one of them contains a complete copy of the registry hives - "
                          + "so the registry IS backed up, as of each snapshot's timestamp.");
            }
            else
            {
                String_Builder.Append("No restore points or shadow copies exist either - the registry currently has "
                          + "NO recoverable backups on this machine. Consider enabling RegBack and/or "
                          + "System Protection.");
            }

            txtVerdict.Text = String_Builder.ToString();
        }

        // --------------------------------------------------------
        //  Buttons
        // --------------------------------------------------------
        private void Btn_Refresh_Click(object Sender, EventArgs E)
        {
            using var Block = Trace_Block.Start_If_Enabled();
            Load_All();
        }

        private void Btn_Enable_RegBack_Click(object Sender, EventArgs E)
        {
            using var Block = Trace_Block.Start_If_Enabled();
            var Answer = MessageBox.Show(
                "This sets EnablePeriodicBackup = 1 so Windows resumes copying the registry hives " +
                "to the RegBack folder during automatic maintenance.\n\n" +
                "The backups use roughly 100-300 MB. Proceed?",
                "Enable RegBack", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (Answer != DialogResult.Yes) return;

            try
            {
                Registry.SetValue(Periodic_Key, "EnablePeriodicBackup", 1, RegistryValueKind.DWord);
                MessageBox.Show(
                    "Enabled. The RegBack folder fills the next time RegIdleBackup runs - " +
                    "click Backup Now to run it immediately, then Refresh.",
                    "Enable RegBack", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Load_All();
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Failed to write the registry value: " + Ex.Message +
                    "\n\nMake sure the app is running as Administrator.",
                    "Enable RegBack", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Btn_Backup_Now_Click(object Sender, EventArgs E)
        {
            using var Block = Trace_Block.Start_If_Enabled();
            try
            {
                Type SchedType = Type.GetTypeFromProgID("Schedule.Service");
                dynamic Service = Activator.CreateInstance(SchedType);
                Service.Connect();

                dynamic Folder = Service.GetFolder(Task_Folder);
                dynamic Task   = Folder.GetTask(Task_Name);
                Task.Run(null);

                MessageBox.Show(
                    "RegIdleBackup started. It usually finishes within a few seconds - " +
                    "click Refresh to see the results.\n\n" +
                    "Note: if the EnablePeriodicBackup setting is not enabled, the task " +
                    "runs but writes nothing.",
                    "Backup Now", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Could not run the task: " + Ex.Message,
                    "Backup Now", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Btn_Open_Folder_Click(object Sender, EventArgs E)
        {
            using var Block = Trace_Block.Start_If_Enabled();
            try
            {
                Process.Start("explorer.exe", RegBack_Path);
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Could not open folder: " + Ex.Message,
                    "Open Folder", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Btn_Close_Click(object Sender, EventArgs E)
        {
            using var Block = Trace_Block.Start_If_Enabled();
            Close();
        }
    }
}
