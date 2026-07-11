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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Management;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

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

        private bool _regBackActive;
        private bool _settingEnabled;
        private int  _rpCount;
        private int  _scCount;

        public Registry_Backup_Form()
        {
            InitializeComponent();
            Load_All();
        }

        // --------------------------------------------------------
        //  Master refresh
        // --------------------------------------------------------
        private void Load_All()
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                Check_RegBack_Folder();
                Check_Periodic_Setting();
                Check_Scheduled_Task();
                Check_Snapshots();
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
            lvHives.BeginUpdate();
            lvHives.Items.Clear();
            _regBackActive = false;

            txtRegBackPath.Text = RegBack_Path;

            try
            {
                if (!Directory.Exists(RegBack_Path))
                {
                    txtRegBackStatus.Text = "Folder not found";
                    lvHives.EndUpdate();
                    return;
                }

                long totalBytes = 0;
                var files = Directory.GetFiles(RegBack_Path);

                foreach (var f in files)
                {
                    var info = new FileInfo(f);
                    totalBytes += info.Length;

                    var item = new ListViewItem(info.Name);
                    item.SubItems.Add(Format_Size(info.Length));
                    item.SubItems.Add(info.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss"));

                    if (info.Length == 0)
                        item.ForeColor = Color.Gray;

                    lvHives.Items.Add(item);
                }

                _regBackActive = totalBytes > 0;

                if (files.Length == 0)
                    txtRegBackStatus.Text = "Folder is empty";
                else if (_regBackActive)
                    txtRegBackStatus.Text = "ACTIVE - contains real backups ("
                                            + Format_Size(totalBytes) + ")";
                else
                    txtRegBackStatus.Text = "INACTIVE - all files are 0 bytes";
            }
            catch (Exception ex)
            {
                txtRegBackStatus.Text = "Error: " + ex.Message;
            }

            lvHives.EndUpdate();
        }

        private static string Format_Size(long bytes)
        {
            if (bytes == 0) return "0 bytes";
            if (bytes < 1024 * 1024) return (bytes / 1024.0).ToString("0.0") + " KB";
            return (bytes / (1024.0 * 1024.0)).ToString("0.0") + " MB";
        }

        // --------------------------------------------------------
        //  2. EnablePeriodicBackup setting
        // --------------------------------------------------------
        private void Check_Periodic_Setting()
        {
            _settingEnabled = false;
            try
            {
                object val = Registry.GetValue(Periodic_Key, "EnablePeriodicBackup", null);

                if (val == null)
                {
                    txtPeriodic.Text = "Not set (Windows default = disabled)";
                }
                else if (Convert.ToInt32(val) == 1)
                {
                    _settingEnabled = true;
                    txtPeriodic.Text = "EnablePeriodicBackup = 1 (enabled)";
                }
                else
                {
                    txtPeriodic.Text = "EnablePeriodicBackup = " + val + " (disabled)";
                }
            }
            catch (Exception ex)
            {
                txtPeriodic.Text = "Error: " + ex.Message;
            }
        }

        // --------------------------------------------------------
        //  3. RegIdleBackup scheduled task (Task Scheduler COM)
        // --------------------------------------------------------
        private void Check_Scheduled_Task()
        {
            try
            {
                Type schedType = Type.GetTypeFromProgID("Schedule.Service");
                dynamic service = Activator.CreateInstance(schedType);
                service.Connect();

                dynamic folder = service.GetFolder(Task_Folder);
                dynamic task   = folder.GetTask(Task_Name);

                DateTime lastRun = task.LastRunTime;
                DateTime nextRun = task.NextRunTime;
                int      result  = task.LastTaskResult;
                int      state   = task.State;
                bool     enabled = task.Enabled;

                txtTaskLastRun.Text = lastRun.Year < 2000
                    ? "Never"
                    : lastRun.ToString("yyyy-MM-dd HH:mm:ss");

                txtTaskNextRun.Text = nextRun.Year < 2000
                    ? "Not scheduled (runs during automatic maintenance)"
                    : nextRun.ToString("yyyy-MM-dd HH:mm:ss");

                txtTaskResult.Text = "0x" + result.ToString("X")
                    + (result == 0 ? " (success)" : "");

                txtTaskState.Text = Task_State_Name(state)
                    + (enabled ? "" : "  [task disabled]");
            }
            catch (Exception ex)
            {
                txtTaskLastRun.Text = "Task not found or inaccessible";
                txtTaskResult.Text  = ex.Message;
                txtTaskState.Text   = "";
                txtTaskNextRun.Text = "";
            }
        }

        private static string Task_State_Name(int state)
        {
            switch (state)
            {
                case 0:  return "Unknown";
                case 1:  return "Disabled";
                case 2:  return "Queued";
                case 3:  return "Ready";
                case 4:  return "Running";
                default: return "State " + state;
            }
        }

        // --------------------------------------------------------
        //  4. Restore points + shadow copies
        // --------------------------------------------------------
        private void Check_Snapshots()
        {
            _rpCount = 0;
            _scCount = 0;

            // Restore points — reuse the manager
            try
            {
                var points = Restore_Point_Manager.Get_Restore_Points();
                _rpCount = points.Count;

                txtRpCount.Text = _rpCount.ToString();
                txtRpNewest.Text = _rpCount == 0
                    ? "—"
                    : points[0].Creation_Time.ToString("yyyy-MM-dd HH:mm:ss")
                      + "  (" + points[0].Description + ")";
            }
            catch (Exception ex)
            {
                txtRpCount.Text  = "?";
                txtRpNewest.Text = "Error: " + ex.Message;
            }

            // Shadow copies
            try
            {
                DateTime newest = DateTime.MinValue;

                using (var searcher = new ManagementObjectSearcher(
                    @"root\cimv2", "SELECT InstallDate FROM Win32_ShadowCopy"))
                {
                    foreach (ManagementObject mo in searcher.Get())
                    {
                        _scCount++;
                        object installDate = mo["InstallDate"];
                        if (installDate == null) continue;

                        DateTime created = ManagementDateTimeConverter
                            .ToDateTime(installDate.ToString());
                        if (created > newest) newest = created;
                    }
                }

                txtScCount.Text = _scCount.ToString();
                txtScNewest.Text = _scCount == 0
                    ? "—"
                    : newest.ToString("yyyy-MM-dd HH:mm:ss");
            }
            catch (Exception ex)
            {
                txtScCount.Text  = "?";
                txtScNewest.Text = "Error: " + ex.Message;
            }
        }

        // --------------------------------------------------------
        //  Verdict
        // --------------------------------------------------------
        private void Build_Verdict()
        {
            var sb = new StringBuilder();

            if (_regBackActive)
                sb.Append("The built-in RegBack mechanism is working and holds real hive backups. ");
            else if (_settingEnabled)
                sb.Append("RegBack is enabled but the folder holds no data yet - it fills the next time " +
                          "RegIdleBackup runs (use Backup Now, then Refresh). ");
            else
                sb.Append("The built-in RegBack mechanism is disabled (the Windows default since 10 v1803). ");

            if (_rpCount > 0 || _scCount > 0)
            {
                sb.Append("However, you have " + _rpCount + " restore point(s) and "
                          + _scCount + " shadow cop" + (_scCount == 1 ? "y" : "ies")
                          + ", and every one of them contains a complete copy of the registry hives - "
                          + "so the registry IS backed up, as of each snapshot's timestamp.");
            }
            else
            {
                sb.Append("No restore points or shadow copies exist either - the registry currently has "
                          + "NO recoverable backups on this machine. Consider enabling RegBack and/or "
                          + "System Protection.");
            }

            txtVerdict.Text = sb.ToString();
        }

        // --------------------------------------------------------
        //  Buttons
        // --------------------------------------------------------
        private void Btn_Refresh_Click(object sender, EventArgs e)
        {
            Load_All();
        }

        private void Btn_Enable_RegBack_Click(object sender, EventArgs e)
        {
            var answer = MessageBox.Show(
                "This sets EnablePeriodicBackup = 1 so Windows resumes copying the registry hives " +
                "to the RegBack folder during automatic maintenance.\n\n" +
                "The backups use roughly 100-300 MB. Proceed?",
                "Enable RegBack", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (answer != DialogResult.Yes) return;

            try
            {
                Registry.SetValue(Periodic_Key, "EnablePeriodicBackup", 1, RegistryValueKind.DWord);
                MessageBox.Show(
                    "Enabled. The RegBack folder fills the next time RegIdleBackup runs - " +
                    "click Backup Now to run it immediately, then Refresh.",
                    "Enable RegBack", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Load_All();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to write the registry value: " + ex.Message +
                    "\n\nMake sure the app is running as Administrator.",
                    "Enable RegBack", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Btn_Backup_Now_Click(object sender, EventArgs e)
        {
            try
            {
                Type schedType = Type.GetTypeFromProgID("Schedule.Service");
                dynamic service = Activator.CreateInstance(schedType);
                service.Connect();

                dynamic folder = service.GetFolder(Task_Folder);
                dynamic task   = folder.GetTask(Task_Name);
                task.Run(null);

                MessageBox.Show(
                    "RegIdleBackup started. It usually finishes within a few seconds - " +
                    "click Refresh to see the results.\n\n" +
                    "Note: if the EnablePeriodicBackup setting is not enabled, the task " +
                    "runs but writes nothing.",
                    "Backup Now", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not run the task: " + ex.Message,
                    "Backup Now", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Btn_Open_Folder_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("explorer.exe", RegBack_Path);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not open folder: " + ex.Message,
                    "Open Folder", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Btn_Close_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
