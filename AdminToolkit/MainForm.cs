
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;

namespace Admin_Tools
{
    public partial class MainForm : Form
    {
        // Tracks every tool this program has launched
        private readonly List<LaunchedTool> _launched = new List<LaunchedTool>();

        // The single detached output window (created on demand, reused)
        private OutputForm _outputForm;
        private bool _isElevated;
        private OutputForm _logForm;
       

        // Button state colors: green = ready, amber = tool running
        private static readonly System.Drawing.Color ReadyColor =
            System.Drawing.Color.FromArgb(198, 239, 206);
        private static readonly System.Drawing.Color RunningColor =
            System.Drawing.Color.FromArgb(255, 235, 156);



    private class LaunchedTool
        {
            public string Name;
            public Process Process;
            public ListViewItem Item;
            public Button Button;   // launcher button to re-enable on exit
        }

        public MainForm()
        {
            InitializeComponent();

            string appTitle = Assembly.GetExecutingAssembly()
       .GetCustomAttribute<AssemblyTitleAttribute>()?.Title ?? "Admin Toolkit";

            this.Text = $"{appTitle} Running on => {Environment.MachineName}";

            // Hide the email buttons until implemented)
            Email_Settings_Button.Enabled = false;
            Email_Settings_Button.Visible = false;
            Email_Log_Button.Enabled = false;
            Email_Log_Button.Visible = false;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // All launcher buttons start green (ready)
            foreach (Control c in grpTools.Controls)
            {
                var b = c as Button;
                if (b != null)
                {
                    b.UseVisualStyleBackColor = false;
                    b.BackColor = ReadyColor;
                }
            }

            _isElevated = IsRunningAsAdmin();
            Logger.BlankLine();
            Logger.Log("App", "Started on " + Environment.MachineName +
                              "  (elevated=" + _isElevated + ")");
            if (!_isElevated)
            {
                ShowElevationBanner();
                statusLabel.Text = "WARNING: not elevated - admin features will fail.";
            }

     
        }

        // ====================================================================
        //  SNAPSHOTS / RESTORE POINTS
        // ====================================================================

       

        // drive is like "C:\\"
      

   
       
      

    

    

        private static string DecodeRestorePointType(object t)
        {
            try
            {
                switch (Convert.ToInt32(t))
                {
                    case 0: return "Application install";
                    case 1: return "Application uninstall";
                    case 10: return "Device driver install";
                    case 12: return "Modify settings";
                    case 13: return "Cancelled operation";
                    default: return "Type " + t;
                }
            }
            catch { return Convert.ToString(t); }
        }

        /// <summary>Named System Restore points (root\default SystemRestore class).</summary>
        private string GetSystemRestorePoints()
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine();
            sb.AppendLine("SYSTEM RESTORE POINTS (named, from the System Restore wizard)");
            sb.AppendLine(new string('=', 60));
            try
            {
                using (var searcher = new ManagementObjectSearcher(
                    "root\\default", "SELECT * FROM SystemRestore"))
                {
                    int count = 0;
                    foreach (ManagementObject rp in searcher.Get())
                    {
                        DateTime created = ManagementDateTimeConverter.ToDateTime(
                            (string)rp["CreationTime"]);
                        sb.AppendLine(string.Format("#{0}  {1}  {2}",
                            rp["SequenceNumber"],
                            created.ToString("yyyy-MM-dd HH:mm"),
                            rp["Description"]));
                        sb.AppendLine("      Type: " +
                            DecodeRestorePointType(rp["RestorePointType"]));
                        count++;
                    }
                    if (count == 0)
                        sb.AppendLine("(none found)");
                }
            }
            catch (Exception ex)
            {
                sb.AppendLine("(unavailable: " + ex.Message + ")");
            }
            return sb.ToString();
        }

        /// <summary>Raw "vssadmin list shadows" output.</summary>
        private string GetVssAdminOutput()
        {
            try
            {
                var psi = new ProcessStartInfo("vssadmin.exe", "list shadows")
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };
                using (var p = Process.Start(psi))
                {
                    string output = p.StandardOutput.ReadToEnd();
                    string err = p.StandardError.ReadToEnd();
                    p.WaitForExit(15000);
                    return string.IsNullOrWhiteSpace(output) ? err : output;
                }
            }
            catch (Exception ex)
            {
                return "Failed to run vssadmin: " + ex.Message;
            }
        }


        /// <summary>True if this process is running with Administrator rights.</summary>
        private static bool IsRunningAsAdmin()
        {
            try
            {
                using (WindowsIdentity id = WindowsIdentity.GetCurrent())
                {
                    return new WindowsPrincipal(id)
                        .IsInRole(WindowsBuiltInRole.Administrator);
                }
            }
            catch { return false; }
        }

        /// <summary>
        /// Drop a red warning strip across the top when we're NOT elevated.
        /// Pushes the existing groups down and grows the form so nothing overlaps -
        /// no Designer changes needed.
        /// </summary>
        private void ShowElevationBanner()
        {
            const int h = 30;

            var banner = new Panel
            {
                Dock = DockStyle.Top,
                Height = h,
                BackColor = System.Drawing.Color.FromArgb(192, 0, 0)
            };
            banner.Controls.Add(new Label
            {
                Dock = DockStyle.Fill,
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold),
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Text = "NOT RUNNING AS ADMINISTRATOR  -  snapshot / VSS actions will fail. " +
                       "Close and restart with 'Run as administrator'."
            });

            // Move the absolutely-positioned groups down and make room at the top.
            foreach (Control c in Controls)
            {
                if (c is StatusStrip) continue;   // stays docked at the bottom
                c.Top += h;
            }
            Height += h;

            Controls.Add(banner);
            banner.BringToFront();
        }

        // ====================================================================
        //  ADMIN TOOL LAUNCHER
        // ====================================================================

        private void LaunchTool(string displayName, string fileName, string arguments)
        {
            LaunchTool(displayName, fileName, arguments, null);
        }

        private void LaunchTool(string displayName, string fileName,
            string arguments, Button launcher)
        {
            try
            {
                var psi = new ProcessStartInfo(fileName, arguments)
                {
                    UseShellExecute = true
                };
                Process p = Process.Start(psi);
                if (p == null)
                {
                    statusLabel.Text = displayName + " launched (no process handle).";
                    return;
                }

                // Lock the launcher button until the tool closes
                if (launcher != null)
                {
                    launcher.Enabled = false;
                    launcher.BackColor = RunningColor;
                }

                p.EnableRaisingEvents = true;

                var item = new ListViewItem(displayName);
                item.SubItems.Add(p.Id.ToString());
                item.SubItems.Add(DateTime.Now.ToString("HH:mm:ss"));
                item.SubItems.Add("Running");
                listViewProcesses.Items.Add(item);

                var tool = new LaunchedTool
                {
                    Name = displayName, Process = p, Item = item, Button = launcher
                };
                _launched.Add(tool);
                item.Tag = tool;

                p.Exited += (s, args) =>
                {
                    try
                    {
                        BeginInvoke((MethodInvoker)(() =>
                        {
                            tool.Item.SubItems[3].Text = "Closed";
                            ReleaseButton(tool);
                            statusLabel.Text = tool.Name + " closed.";
                        }));
                    }
                    catch { /* form closing */ }
                };

                statusLabel.Text = displayName + " launched (PID " + p.Id + ").";
                Logger.Log("Launch", displayName + " (PID " + p.Id + ")");
            }
            catch (Exception ex)
            {
                // Launch failed - make sure the button is usable again
                if (launcher != null)
                {
                    launcher.Enabled = true;
                    launcher.BackColor = ReadyColor;
                }
                MessageBox.Show("Could not launch " + displayName + ":\n\n" + ex.Message,
                    "Launch Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>Return a launcher button to the green/ready state.</summary>
        private void ReleaseButton(LaunchedTool tool)
        {
            if (tool.Button != null)
            {
                tool.Button.Enabled = true;
                tool.Button.BackColor = ReadyColor;
            }
        }

        private static string Sys32(string file)
        {
            return Path.Combine(Environment.SystemDirectory, file);
        }

        private void btnTaskScheduler_Click(object sender, EventArgs e)
        {
            LaunchTool("Task Scheduler", "mmc.exe", Sys32("taskschd.msc"), (Button)sender);
        }

        private void btnSystemProtection_Click(object sender, EventArgs e)
        {
            LaunchTool("System Protection", Sys32("SystemPropertiesProtection.exe"), "", (Button)sender);
        }

        private void btnRestoreWizard_Click(object sender, EventArgs e)
        {
            LaunchTool("System Restore Wizard", Sys32("rstrui.exe"), "", (Button)sender);
        }

        private void btnRegistryEditor_Click(object sender, EventArgs e)
        {
            LaunchTool("Registry Editor", "regedit.exe", "", (Button)sender);
        }

        private void btnEventViewer_Click(object sender, EventArgs e)
        {
            LaunchTool("Event Viewer", "mmc.exe", Sys32("eventvwr.msc"), (Button)sender);
        }

        private void btnServices_Click(object sender, EventArgs e)
        {
            LaunchTool("Services", "mmc.exe", Sys32("services.msc"), (Button)sender);
        }

        private void btnDiskManagement_Click(object sender, EventArgs e)
        {
            LaunchTool("Disk Management", "mmc.exe", Sys32("diskmgmt.msc"), (Button)sender);
        }

        private void btnComputerMgmt_Click(object sender, EventArgs e)
        {
            LaunchTool("Computer Management", "mmc.exe", Sys32("compmgmt.msc"), (Button)sender);
        }

        private void btnSystemInfo_Click(object sender, EventArgs e)
        {
            LaunchTool("System Information", Sys32("msinfo32.exe"), "", (Button)sender);
        }

        private void btnPerfMonitor_Click(object sender, EventArgs e)
        {
            LaunchTool("Performance Monitor", "mmc.exe", Sys32("perfmon.msc"), (Button)sender);
        }

        private void btnResourceMonitor_Click(object sender, EventArgs e)
        {
            LaunchTool("Resource Monitor", Sys32("resmon.exe"), "", (Button)sender);
        }

        private void btnDeviceManager_Click(object sender, EventArgs e)
        {
            LaunchTool("Device Manager", "mmc.exe", Sys32("devmgmt.msc"), (Button)sender);
        }

        private void btnLocalUsers_Click(object sender, EventArgs e)
        {
            LaunchTool("Local Users && Groups", "mmc.exe", Sys32("lusrmgr.msc"), (Button)sender);
        }

        private void btnFirewall_Click(object sender, EventArgs e)
        {
            LaunchTool("Windows Firewall", "mmc.exe", Sys32("wf.msc"), (Button)sender);
        }

        // ====================================================================
        //  PROCESS MONITOR / CONTROL
        // ====================================================================

        private LaunchedTool SelectedTool()
        {
            if (listViewProcesses.SelectedItems.Count == 0) return null;
            return listViewProcesses.SelectedItems[0].Tag as LaunchedTool;
        }

        private void btnCloseTool_Click(object sender, EventArgs e)
        {
            LaunchedTool tool = SelectedTool();
            if (tool == null)
            {
                statusLabel.Text = "Select a launched tool first.";
                return;
            }
            try
            {
                if (!tool.Process.HasExited)
                {
                    // Polite close - same as clicking the X button
                    if (!tool.Process.CloseMainWindow())
                    {
                        statusLabel.Text = tool.Name +
                            " has no main window to close - use End Task.";
                    }
                }
                else
                {
                    statusLabel.Text = tool.Name + " is already closed.";
                }
            }
            catch (Exception ex)
            {
                statusLabel.Text = "Close failed: " + ex.Message;
            }
        }

        private void btnKillTool_Click(object sender, EventArgs e)
        {
            LaunchedTool tool = SelectedTool();
            if (tool == null)
            {
                statusLabel.Text = "Select a launched tool first.";
                return;
            }
            try
            {
                if (!tool.Process.HasExited)
                {
                    tool.Process.Kill();
                    statusLabel.Text = tool.Name + " terminated.";
                    Logger.Log("Kill", tool.Name + " force-terminated.");
                }
                else
                {
                    statusLabel.Text = tool.Name + " is already closed.";
                }
            }
            catch (Exception ex)
            {
                statusLabel.Text = "End Task failed: " + ex.Message;
            }
        }

        private void btnRemoveClosed_Click(object sender, EventArgs e)
        {
            for (int i = listViewProcesses.Items.Count - 1; i >= 0; i--)
            {
                var tool = listViewProcesses.Items[i].Tag as LaunchedTool;
                bool exited = true;
                try { exited = tool == null || tool.Process.HasExited; } catch { }
                if (exited)
                {
                    if (tool != null) _launched.Remove(tool);
                    listViewProcesses.Items.RemoveAt(i);
                }
            }
        }

        // Safety net: poll status in case Exited didn't fire (e.g. shell handoff)
        private void timerStatus_Tick(object sender, EventArgs e)
        {
            foreach (LaunchedTool tool in _launched)
            {
                try
                {
                    if (tool.Process.HasExited &&
                        tool.Item.SubItems[3].Text != "Closed")
                    {
                        tool.Item.SubItems[3].Text = "Closed";
                        ReleaseButton(tool);
                    }
                }
                catch { }
            }
        }

        private void Quit_Button_Click(object sender, EventArgs e)
        {
            Logger.Log("App", "Quit - closing all launched tools.");
            for (int i = Application.OpenForms.Count - 1; i >= 0; i--)
            {
                if (Application.OpenForms[i] != this)
                    Application.OpenForms[i].Close();
            }
            Close();
        }


        public void Show_Log()
        {
            if (_logForm == null || _logForm.IsDisposed)
            {
                _logForm = new OutputForm();
                _logForm.Width = 850;
                _logForm.Location = new System.Drawing.Point(
                    Math.Max(0, Location.X + 60), Location.Y + 100);

                // Re-enable the button when the log window closes.
                _logForm.FormClosed += (s, ev) =>
                {
                    View_Log_Button.Enabled = true;
                    View_Log_Button.BackColor = ReadyColor;
                };

                _logForm.Show(this);   // owned, non-modal
            }

            _logForm.ShowLiveLog("Activity Log - " + Environment.MachineName,
                Logger.CurrentLogPath);

            if (_logForm.WindowState == FormWindowState.Minimized)
                _logForm.WindowState = FormWindowState.Normal;
            _logForm.BringToFront();

            View_Log_Button.Enabled = false;
            View_Log_Button.BackColor = RunningColor;

        }
        private void View_Log_Button_Click(object sender, EventArgs e)
        {
            Show_Log();

         
        }

        private void Email_Log_Button_Click(object sender, EventArgs e)
        {

            if (!EnsureEmailConfigured()) return;

            try
            {
                string src = Logger.CurrentLogPath;
                string tmp = System.IO.Path.Combine(System.IO.Path.GetTempPath(),
                    "AdminToolkit-" + Environment.MachineName + "-log.txt");

                using (var inFs = new System.IO.FileStream(src, System.IO.FileMode.Open,
                           System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite))
                using (var outFs = new System.IO.FileStream(tmp, System.IO.FileMode.Create))
                {
                    inFs.CopyTo(outFs);
                }

                string subject = "AdminToolkit log - " + Environment.MachineName +
                                 "  " + DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                string body = "Activity log attached from " + Environment.MachineName +
                              " (" + Environment.UserName + ").";

                statusLabel.Text = "Sending log...";
                Application.DoEvents();

                Emailer.Send(subject, body, tmp);

                statusLabel.Text = "Log emailed.";
                Logger.Log("Email", "Log emailed successfully.");
            }
            catch (Exception ex)
            {
                statusLabel.Text = "Email failed: " + ex.Message;
                Logger.Log("Email", "FAILED: " + ex.Message);

                bool looksLikeAuth =
                    ex is System.Net.Mail.SmtpException &&
                    ex.Message.IndexOf("auth", StringComparison.OrdinalIgnoreCase) >= 0;

                if (looksLikeAuth &&
                    MessageBox.Show(
                        "Could not send the log:\n\n" + ex.Message +
                        "\n\nThis looks like a login problem. Re-enter the password now?",
                        "Email Failed", MessageBoxButtons.YesNo, MessageBoxIcon.Error)
                    == DialogResult.Yes)
                {
                    Emailer.ClearPassword();     // drop the bad one
                    EnsureEmailConfigured();     // prompt for a fresh password
                }
                else if (!looksLikeAuth)
                {
                    MessageBox.Show("Could not send the log:\n\n" + ex.Message,
                        "Email Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        /// <summary>
        /// Ensure email is ready: server fields filled in and an encrypted password
        /// stored. Opens the settings file or prompts for the password as needed.
        /// Returns true only when everything is set.
        /// </summary>
        private bool EnsureEmailConfigured()
        {
            Emailer.SmtpConfig cfg = Emailer.LoadConfig();   // writes template if missing

            if (!cfg.ServerReady)
            {
                if (MessageBox.Show(
                        "Email isn't set up on this machine yet.\n\n" +
                        "Open the settings file to fill in the server, user, and recipient?",
                        "Email Setup Needed", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information) == DialogResult.Yes)
                    Emailer.OpenSettings();

                statusLabel.Text = "Complete the email settings, then try again.";
                return false;
            }

            if (!Emailer.HasPassword())
            {
                string pwd = PasswordBox.Show(this, "Email Password",
                    "Enter the SMTP / app password for " + cfg.User + ".\n\n" +
                    "It's encrypted for this account on this PC - never stored as plain text.");

                if (string.IsNullOrEmpty(pwd))
                {
                    statusLabel.Text = "Email password not set.";
                    return false;
                }

                Emailer.SavePassword(pwd);
                Logger.Log("Email", "SMTP password stored (encrypted).");
            }

            return true;
        }

        private void Email_Settings_Button_Click(object sender, EventArgs e)
        {
            Emailer.OpenSettings();   // opens email.settings in Notepad
            statusLabel.Text = "Editing email settings - save Notepad, then send again.";
        }

     




        private void Enable_Shadowing_Button_Click(object sender, EventArgs e)
        {
            var choices = BuildDriveChoices();
            if (choices.Count == 0)
            {
                statusLabel.Text = "No fixed NTFS drives found to protect.";
                return;
            }

            List<string> selected;
            using (var dlg = new DriveSelectDialog(choices))
            {
                if (dlg.ShowDialog(this) != DialogResult.OK)
                {
                    statusLabel.Text = "Shadowing setup cancelled.";
                    return;
                }
                selected = dlg.SelectedDrives;
            }

            if (selected.Count == 0)
            {
                statusLabel.Text = "No drives selected — nothing to do.";
                return;
            }

            statusLabel.Text = "Setting up shadowing...";
            Application.DoEvents();

            if (CreateShadowTask(selected, out string error))
            {
                var covered = GetFixedNtfsDrives().Where(IsShadowingActive).ToList();
                statusLabel.Text = "Shadowing active on " + string.Join(", ", covered) +
                    " — daily task covers all of them.";
            
            }
            else
            {
                MessageBox.Show(
                    "Setup failed.\n\n" + error + "\n\nCheck that you're elevated and that System " +
                    "Restore isn't disabled by group policy. See the log for details.",
                    "Setup Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                statusLabel.Text = "Shadowing setup failed.";
            }
        }



     

   

     

        // (Re)register AutoShadowCopy to snapshot exactly these drives.
     

        private static bool RunSchtasks(string args)
        {
            try
            {
                var psi = new ProcessStartInfo("schtasks.exe", args)
                {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };
                using (var p = Process.Start(psi)) { p.WaitForExit(30000); return p.ExitCode == 0; }
            }
            catch { return false; }
        }


    
        private void Help_Button_Click(object sender, EventArgs e)
        {
            using (var dlg = new HelpDialog())
                dlg.ShowDialog(this);
        }

     

        private void Disable_Shadowing_Button_Click(object sender, EventArgs e)
        {
            string sysDrive = Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows));

            // Currently-protected fixed NTFS drives
            var protectedDrives = DriveInfo.GetDrives()
                .Where(d => d.DriveType == DriveType.Fixed && d.IsReady &&
                            string.Equals(d.DriveFormat, "NTFS", StringComparison.OrdinalIgnoreCase) &&
                            IsShadowingActive(d.Name))
                .Select(d => d.Name)
                .ToList();

            if (protectedDrives.Count == 0)
            {
                statusLabel.Text = "Shadowing isn't active on any drive — nothing to turn off.";
                return;
            }

            // Picker: nothing pre-checked; user checks what to TURN OFF
            var choices = protectedDrives.Select(name =>
            {
                var di = new DriveInfo(name);
                string label = string.IsNullOrWhiteSpace(SafeLabel(di)) ? "Local Disk" : SafeLabel(di);
                double gb = di.TotalSize / 1024.0 / 1024.0 / 1024.0;
                bool isSys = name.Equals(sysDrive, StringComparison.OrdinalIgnoreCase);
                return new DriveChoice
                {
                    Name = name,
                    Display = string.Format("{0}  {1}  ({2:0} GB){3}",
                                name.TrimEnd('\\'), label, gb, isSys ? "   — system drive" : ""),
                    Checked = false
                };
            }).ToList();

            List<string> offList;
            using (var dlg = new DriveSelectDialog(choices,
                "Turn Off Shadowing",
                "Check the drives you want to TURN OFF (this deletes their snapshots):"))
            {
                if (dlg.ShowDialog(this) != DialogResult.OK) { statusLabel.Text = "Teardown cancelled."; return; }
                offList = dlg.SelectedDrives;
            }

            if (offList.Count == 0) { statusLabel.Text = "No drives selected — nothing to do."; return; }

            // Cascade: the system drive can't be turned off while other drives stay protected.
            bool turningOffSystem = offList.Any(d => d.Equals(sysDrive, StringComparison.OrdinalIgnoreCase));
            var survivors = protectedDrives.Except(offList, StringComparer.OrdinalIgnoreCase).ToList();
            bool cascaded = false;

            if (turningOffSystem && survivors.Count > 0)
            {
                offList = protectedDrives.ToList();   // everything comes off
                survivors.Clear();
                cascaded = true;
            }

            string willDelete = string.Join(", ", offList);
            string taskFate = survivors.Count > 0
                ? "The AutoShadowCopy task will be rebuilt to snapshot only: " + string.Join(", ", survivors) + "."
                : "The AutoShadowCopy task will be removed entirely.";
            string cascadeNote = cascaded
                ? "\n\nNote: turning off the system drive (" + sysDrive.TrimEnd('\\') +
                  ") also turns off the other protected drives — Windows requires this."
                : "";

            var confirm = MessageBox.Show(
                "This will permanently delete all snapshots on: " + willDelete + "." + cascadeNote +
                "\n\n" + taskFate + "\n\nSnapshots cannot be recovered. Continue?",
                "Confirm Turn Off Shadowing",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (confirm != DialogResult.Yes) { statusLabel.Text = "Teardown cancelled."; return; }

            statusLabel.Text = "Turning off shadowing on " + willDelete + "...";
            Application.DoEvents();

            if (TeardownShadowing(offList, survivors, out string error))
            {
                statusLabel.Text = "Shadowing turned off on " + willDelete +
                    (survivors.Count > 0 ? " — task now covers " + string.Join(", ", survivors) + "." : " — task removed.");
               
            }
            else
            {
                MessageBox.Show("Turn-off failed.\n\n" + error + "\n\nCheck the log for details.",
                    "Teardown Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                statusLabel.Text = "Teardown failed.";
            }
        }

        private void Restore_Points_Button_Click(object sender, EventArgs e)
        {
            using (var f = new Restore_Point_List_Form())
                f.ShowDialog(this);
        }

        private void Enable_Registry_Backup_Button_Click(object sender, EventArgs e)
        {
            using (var f = new Registry_Backup_Form())
                f.ShowDialog(this);
        }

        private void Snapshot_Operations_Button_Click(object sender, EventArgs e)
        {
            using (var f = new Shadow_Copy_Form())
                f.ShowDialog(this);
        }

     
    }
}
