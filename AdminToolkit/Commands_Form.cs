// ============================================================
//  Commands_Form.cs
//  Remote-triage command board (Windows built-ins only).
//  Each command button has a dedicated click handler that
//  passes a Triage_Command enum value to Execute_Command_Async.
//  Adding a command means: adding an enum member, a case in
//  Get_Command_Line, a button, and a one-line click handler.
//
//  RustDesk / Tailscale diagnostics live in Remote_Access_Form.
//
//  Usage from MainForm (modeless):
//      var f = new Commands_Form();
//      f.Show(this);
// ============================================================

using System;
using System.IO;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Admin_Tools
{
    public enum Triage_Command
    {
        // Identity & Sessions
        Whoami,
        Net_User,
        Local_Admins,
        Query_User,
        Net_Session,

        // Network
        Ipconfig,
        Arp,
        Route,
        Netstat,
        Nslookup,
        Flush_Dns,

        // System State
        System_Info,
        Hostname,
        Tasklist,
        Tasklist_Svc,
        Sc_Query,
        Driver_Query,

        // Disk & Storage
        Chkdsk,

        // Health & Logs
        Sfc_Verify,
        Battery_Report,
        System_Events,
        Uptime
    }

    public partial class Commands_Form : Form
    {
        public Commands_Form()
        {
            InitializeComponent();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!Commands_Panel.Enabled)   // a command is running
            {
                e.Cancel = true;
                return;
            }
            base.OnFormClosing(e);
        }

        // --------------------------------------------------------
        //  Enum -> command line
        // --------------------------------------------------------
        private static string Get_Command_Line(Triage_Command command)
        {
            switch (command)
            {
                // Identity & Sessions
                case Triage_Command.Whoami: return "whoami /all";
                case Triage_Command.Net_User: return "net user";
                case Triage_Command.Local_Admins: return "net localgroup administrators";
                case Triage_Command.Query_User: return "query user";
                case Triage_Command.Net_Session: return "net session";

                // Network
                case Triage_Command.Ipconfig: return "ipconfig /all";
                case Triage_Command.Arp: return "arp -a";
                case Triage_Command.Route: return "route print";
                case Triage_Command.Netstat: return "netstat -ano";
                case Triage_Command.Nslookup: return "nslookup google.com";
                case Triage_Command.Flush_Dns: return "ipconfig /flushdns";

                // System State
                case Triage_Command.System_Info: return "systeminfo";
                case Triage_Command.Hostname: return "hostname";
                case Triage_Command.Tasklist: return "tasklist";
                case Triage_Command.Tasklist_Svc: return "tasklist /svc";
                case Triage_Command.Sc_Query: return "sc query";
                case Triage_Command.Driver_Query: return "driverquery";

                // Disk & Storage
                case Triage_Command.Chkdsk: return "chkdsk";

                // Health & Logs
                case Triage_Command.Sfc_Verify: return "sfc /verifyonly";
                case Triage_Command.Battery_Report: return "powercfg /batteryreport";
                case Triage_Command.System_Events: return "wevtutil qe System /c:20 /rd:true /f:text";
                case Triage_Command.Uptime: return "net statistics workstation";

                default:
                    throw new ArgumentOutOfRangeException(nameof(command), command,
                        "No command line is defined for this Triage_Command value.");
            }
        }

        // --------------------------------------------------------
        //  Single execute method — every command handler funnels
        //  through here.
        // --------------------------------------------------------
        private async Task Execute_Command_Async(Triage_Command command)
        {
            string commandLine = Get_Command_Line(command);

            try
            {
                Commands_Panel.Enabled = false;
                Logger.Log("Command", commandLine);
                await Command_Runner.Run_Command_Async(commandLine, txtOutput);
            }
            catch (Exception ex)
            {
                txtOutput.Text = ex.ToString();
            }
            finally
            {
                Commands_Panel.Enabled = true;
            }
        }

        // --------------------------------------------------------
        //  Identity & Sessions
        // --------------------------------------------------------
        private async void btnWhoami_Click(object sender, EventArgs e)
            => await Execute_Command_Async(Triage_Command.Whoami);

        private async void btnNetUser_Click(object sender, EventArgs e)
            => await Execute_Command_Async(Triage_Command.Net_User);

        private async void btnLocalAdmins_Click(object sender, EventArgs e)
            => await Execute_Command_Async(Triage_Command.Local_Admins);

        private async void btnQueryUser_Click(object sender, EventArgs e)
            => await Execute_Command_Async(Triage_Command.Query_User);

        private async void btnNetSession_Click(object sender, EventArgs e)
            => await Execute_Command_Async(Triage_Command.Net_Session);

        // --------------------------------------------------------
        //  Network
        // --------------------------------------------------------
        private async void btnIpconfig_Click(object sender, EventArgs e)
            => await Execute_Command_Async(Triage_Command.Ipconfig);

        private async void btnArp_Click(object sender, EventArgs e)
            => await Execute_Command_Async(Triage_Command.Arp);

        private async void btnRoute_Click(object sender, EventArgs e)
            => await Execute_Command_Async(Triage_Command.Route);

        private async void btnNetstat_Click(object sender, EventArgs e)
            => await Execute_Command_Async(Triage_Command.Netstat);

        private async void btnNslookup_Click(object sender, EventArgs e)
            => await Execute_Command_Async(Triage_Command.Nslookup);

        private async void btnFlushDns_Click(object sender, EventArgs e)
            => await Execute_Command_Async(Triage_Command.Flush_Dns);

        // --------------------------------------------------------
        //  System State
        // --------------------------------------------------------
        private async void btnSystemInfo_Click(object sender, EventArgs e)
            => await Execute_Command_Async(Triage_Command.System_Info);

        private async void btnHostname_Click(object sender, EventArgs e)
            => await Execute_Command_Async(Triage_Command.Hostname);

        private async void btnTasklist_Click(object sender, EventArgs e)
            => await Execute_Command_Async(Triage_Command.Tasklist);

        private async void btnTasklistSvc_Click(object sender, EventArgs e)
            => await Execute_Command_Async(Triage_Command.Tasklist_Svc);

        private async void btnScQuery_Click(object sender, EventArgs e)
            => await Execute_Command_Async(Triage_Command.Sc_Query);

        private async void btnDriverQuery_Click(object sender, EventArgs e)
            => await Execute_Command_Async(Triage_Command.Driver_Query);

        // --------------------------------------------------------
        //  Disk & Storage
        // --------------------------------------------------------
        private async void btnChkdsk_Click(object sender, EventArgs e)
            => await Execute_Command_Async(Triage_Command.Chkdsk);

        // btnVol_Click is a locally-computed command — see the
        // "Locally-computed commands" section below.

        // --------------------------------------------------------
        //  Health & Logs
        // --------------------------------------------------------
        private async void btnSfcVerify_Click(object sender, EventArgs e)
            => await Execute_Command_Async(Triage_Command.Sfc_Verify);

        private async void btnBatteryReport_Click(object sender, EventArgs e)
            => await Execute_Command_Async(Triage_Command.Battery_Report);

        private async void btnSystemEvents_Click(object sender, EventArgs e)
            => await Execute_Command_Async(Triage_Command.System_Events);

        private async void btnUptime_Click(object sender, EventArgs e)
            => await Execute_Command_Async(Triage_Command.Uptime);

        // --------------------------------------------------------
        //  Output pane buttons
        // --------------------------------------------------------
        private void Btn_Clear_Click(object sender, EventArgs e)
        {
            txtOutput.Clear();
        }

        private void Btn_Copy_All_Click(object sender, EventArgs e)
        {
            if (txtOutput.TextLength == 0) return;
            Clipboard.SetText(txtOutput.Text);
        }

        private void Btn_Save_Click(object sender, EventArgs e)
        {
            if (txtOutput.TextLength == 0) return;

            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                dialog.FileName = Environment.MachineName + "_triage_" +
                                  DateTime.Now.ToString("yyyyMMdd_HHmm") + ".txt";

                if (dialog.ShowDialog(this) != DialogResult.OK) return;

                try
                {
                    File.WriteAllText(dialog.FileName, txtOutput.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Could not save the file:\n" + ex.Message,
                        "Admin Commands", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Btn_Close_Click(object sender, EventArgs e)
        {
            Close();
        }

        // --------------------------------------------------------
        //  Locally-computed commands (no shell)
        // --------------------------------------------------------
        private static string Get_Drives()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Drive  Label                 Total (GB)   Free (GB)   Used %");
            sb.AppendLine("-----  --------------------  ----------  ----------  ------");

            foreach (var d in DriveInfo.GetDrives())
            {
                if (!d.IsReady)
                {
                    sb.AppendLine($"{d.Name,-5}  (not ready)");
                    continue;
                }

                double totalGb = d.TotalSize / 1073741824.0;
                double freeGb = d.TotalFreeSpace / 1073741824.0;
                double usedPct = 100.0 * (d.TotalSize - d.TotalFreeSpace) / d.TotalSize;

                sb.AppendLine($"{d.Name,-5}  {d.VolumeLabel,-20}  {totalGb,10:N1}  {freeGb,10:N1}  {usedPct,5:N1}%");
            }
            return sb.ToString();
        }

        private void btnFreeSpacebyDrive_Click(object sender, EventArgs e)
        {
            txtOutput.Text = Get_Drives();
        }

        private static string Get_Volumes()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Drive  Label                 Serial Number   File System");
            sb.AppendLine("-----  --------------------  --------------  -----------");

            // Win32_LogicalDisk exposes the label and serial that
            // the shell "vol" command only reports for one drive.
            var scope = new ManagementObjectSearcher(
                "SELECT DeviceID, VolumeName, VolumeSerialNumber, FileSystem FROM Win32_LogicalDisk");

            foreach (ManagementObject mo in scope.Get())
            {
                string drive = mo["DeviceID"] as string ?? "";          // e.g. "C:"
                string label = mo["VolumeName"] as string ?? "";
                string fs = mo["FileSystem"] as string ?? "";
                string serial = Format_Serial(mo["VolumeSerialNumber"] as string);

                sb.AppendLine($"{drive,-5}  {label,-20}  {serial,-14}  {fs}");
            }
            return sb.ToString();
        }

        // WMI returns the serial as 8 hex chars with no separator;
        // "vol" shows it as XXXX-XXXX, so match that formatting.
        private static string Format_Serial(string raw)
        {
            if (string.IsNullOrEmpty(raw)) return "(none)";
            if (raw.Length == 8) return raw.Substring(0, 4) + "-" + raw.Substring(4, 4);
            return raw;
        }

        private void btnVol_Click(object sender, EventArgs e)
        {
            txtOutput.Text = Get_Volumes();
        }

        private void btnHotfixes_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder();
            sb.AppendLine("HotFix ID     Type              Installed On   Installed By");
            sb.AppendLine("------------  ----------------  -------------  --------------------");

            var scope = new ManagementObjectSearcher("SELECT HotFixID, Description, InstalledOn, InstalledBy FROM Win32_QuickFixEngineering");

            foreach (ManagementObject mo in scope.Get())
            {
                sb.AppendLine($"{mo["HotFixID"],-12}  {mo["Description"],-16}  {mo["InstalledOn"],-13}  {mo["InstalledBy"]}");
            }
            txtOutput.Text = sb.ToString();
        }
    }
}