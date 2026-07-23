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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;

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
      

        // Disk & Storage
        Chkdsk,

        // Health & Logs
        Sfc_Verify,
        Battery_Report,
        System_Events,
        Uptime,

        // System Config
        Driver_Query

    }

    public partial class Commands_Form : Form
    {
        public Commands_Form()
        {
            InitializeComponent();
            Message_Textbox.ReadOnly = true;   // you may already want this since it's a status line
            Message_Textbox.TabStop = false;
            Message_Textbox.Cursor = Cursors.Default;   // removes the I-beam hint that invites clicking/typing
            Clear_Message();
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
                Show_Text_Output();
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
        {
            Clear_Message();
            await Execute_Command_Async(Triage_Command.Whoami);
        }
        private async void btnNetUser_Click(object sender, EventArgs e)
        {
            Clear_Message();
            await Execute_Command_Async(Triage_Command.Net_User);
        }

        private async void btnLocalAdmins_Click(object sender, EventArgs e)
        {
            Clear_Message();
            await Execute_Command_Async(Triage_Command.Local_Admins);
        }

        private async void btnQueryUser_Click(object sender, EventArgs e)
        {
            Clear_Message();
            await Execute_Command_Async(Triage_Command.Query_User);
        }

        private async void btnNetSession_Click(object sender, EventArgs e)
        {
            Clear_Message();
            await Execute_Command_Async(Triage_Command.Net_Session);
        }

        // --------------------------------------------------------
        //  Network
        // --------------------------------------------------------
        private async void btnIpconfig_Click(object sender, EventArgs e)
        { 
            Clear_Message();
            await Execute_Command_Async(Triage_Command.Ipconfig);
        }
        private async void btnArp_Click(object sender, EventArgs e)
        {
            Clear_Message();
            await Execute_Command_Async(Triage_Command.Arp);
        }

        private async void btnRoute_Click(object sender, EventArgs e)
        {
            Clear_Message();
            await Execute_Command_Async(Triage_Command.Route);
        }

        private async void btnNetstat_Click(object sender, EventArgs e)
        {
            Clear_Message();
            await Execute_Command_Async(Triage_Command.Netstat);
        }

        private async void btnNslookup_Click(object sender, EventArgs e)
        {
            Clear_Message();
            await Execute_Command_Async(Triage_Command.Nslookup);
        }

        private async void btnFlushDns_Click(object sender, EventArgs e)
        {
            Clear_Message();
            await Execute_Command_Async(Triage_Command.Flush_Dns);
        }

        // --------------------------------------------------------
        //  System State
        // --------------------------------------------------------
        private async void btnSystemInfo_Click(object sender, EventArgs e)
        { 
            Clear_Message();
            await Execute_Command_Async(Triage_Command.System_Info);
        }

        private async void btnHostname_Click(object sender, EventArgs e)
        {
            Clear_Message();
            await Execute_Command_Async(Triage_Command.Hostname);
        }

        private async void btnTasklist_Click(object sender, EventArgs e)
        {
            Clear_Message();
            await Execute_Command_Async(Triage_Command.Tasklist);
        }   

        private async void btnTasklistSvc_Click(object sender, EventArgs e)
        {
            Clear_Message();
            await Execute_Command_Async(Triage_Command.Tasklist_Svc);
        }

        private async void btnScQuery_Click(object sender, EventArgs e)
        {
            Clear_Message();
            await Execute_Command_Async(Triage_Command.Sc_Query);
        }

        private async void btnDriverQuery_Click(object sender, EventArgs e)
        {
            Clear_Message();
            await Execute_Command_Async(Triage_Command.Driver_Query);
        }

        // --------------------------------------------------------
        //  Disk & Storage
        // --------------------------------------------------------
        private async void btnChkdsk_Click(object sender, EventArgs e)
        {
            Clear_Message();
            await Execute_Command_Async(Triage_Command.Chkdsk);
        }

        // btnVol_Click is a locally-computed command — see the
        // "Locally-computed commands" section below.

        // --------------------------------------------------------
        //  Health & Logs
        // --------------------------------------------------------
        private async void btnSfcVerify_Click(object sender, EventArgs e)
        {
            Clear_Message();
            await Execute_Command_Async(Triage_Command.Sfc_Verify);
        }

        private async void btnBatteryReport_Click(object sender, EventArgs e)
        {
            Clear_Message();
            await Execute_Command_Async(Triage_Command.Battery_Report);
        }

        private async void btnSystemEvents_Click(object sender, EventArgs e)
        {
            Clear_Message();
            await Execute_Command_Async(Triage_Command.System_Events);
        }

        private async void btnUptime_Click(object sender, EventArgs e)
        {
            Clear_Message();
            await Execute_Command_Async(Triage_Command.Uptime);
        }

        // --------------------------------------------------------
        //  Output pane buttons
        // --------------------------------------------------------
        private void Btn_Clear_Click(object sender, EventArgs e)
        {
            Clear_Message();
            txtOutput.Clear();
        }

        private void Btn_Copy_All_Click(object sender, EventArgs e)
        {
            Clear_Message();
            if (txtOutput.TextLength == 0) return;
            Clipboard.SetText(txtOutput.Text);
        }

        private void Btn_Save_Click(object sender, EventArgs e)
        {
            Clear_Message();
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
            Clear_Message();
            Show_Text_Output();
           
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
            Clear_Message();
            Show_Text_Output();
          
            txtOutput.Text = Get_Volumes();
        }

        private void oldbtnHotfixes_Click(object sender, EventArgs e)
        {
            Set_Message("Current list of Hot Fixes");

            Show_Text_Output();
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


        // --------------------------------------------------------
        //  Output-pane switching (text vs. list view)
        // --------------------------------------------------------
        private void Show_Text_Output()
        {
            lvUpdates.Visible = false;
            lvHotfixes.Visible = false;
            txtOutput.Visible = true;
        }

        private void Show_Updates_Output()
        {
            txtOutput.Visible = false;
            lvHotfixes.Visible = false;
            lvUpdates.Visible = true;
        }

        private void Show_Hotfixes_Output()
        {
            txtOutput.Visible = false;
            lvUpdates.Visible = false;
            lvHotfixes.Visible = true;
        }

        private void Show_List_Output()
        {
            txtOutput.Visible = false;
            lvUpdates.Visible = true;
        }



        private sealed class Hotfix_Record
        {
            public string HotFixID;
            public string Description;
            public string InstalledOn;
            public string InstalledBy;
            public string Caption;
            public string FixComments;
            public string Status;
        }

        private static List<Hotfix_Record> Get_Hotfixes()
        {
            var list = new List<Hotfix_Record>();

            var scope = new ManagementObjectSearcher(
                "SELECT HotFixID, Description, InstalledOn, InstalledBy, Caption, FixComments, Status FROM Win32_QuickFixEngineering");

            foreach (ManagementObject mo in scope.Get())
            {
                list.Add(new Hotfix_Record
                {
                    HotFixID = mo["HotFixID"] as string ?? "",
                    Description = mo["Description"] as string ?? "",
                    InstalledOn = mo["InstalledOn"] as string ?? "",
                    InstalledBy = mo["InstalledBy"] as string ?? "",
                    Caption = mo["Caption"] as string ?? "",
                    FixComments = mo["FixComments"] as string ?? "",
                    Status = mo["Status"] as string ?? ""
                });
            }

            list.Sort((a, b) => string.Compare(b.InstalledOn, a.InstalledOn, StringComparison.Ordinal));
            return list;
        }

        private void btnHotfixes_Click(object sender, EventArgs e)
        {
            Set_Message("Double-click an entry for details");
            Show_Hotfixes_Output();

            var hotfixes = Get_Hotfixes();

            lvHotfixes.Items.Clear();
            foreach (var h in hotfixes)
            {
                var item = new ListViewItem(h.HotFixID);
                item.SubItems.Add(h.Description);
                item.SubItems.Add(h.InstalledOn);
                item.SubItems.Add(h.InstalledBy);
                item.Tag = h;
                lvHotfixes.Items.Add(item);
            }
        }


        // --------------------------------------------------------
        //  Windows Update history
        // --------------------------------------------------------
        private sealed class Update_Record
        {
            public string Title;
            public string KB;
            public DateTime Date;
            public string Operation;
            public string ResultText;
            public string Description;
            public string SupportUrl;
        }

        private static readonly Regex KbPattern = new Regex(@"KB\d{6,7}", RegexOptions.IgnoreCase);

        private static string Extract_KB(string title, string description)
        {
            var m = KbPattern.Match(title ?? "");
            if (!m.Success) m = KbPattern.Match(description ?? "");
            return m.Success ? m.Value.ToUpperInvariant() : "";
        }

        private static string Get_Operation_Text(int op)
        {
            switch (op)
            {
                case 1: return "Install";
                case 2: return "Uninstall";
                default: return "Unknown";
            }
        }

        private static string Get_Result_Text(int code)
        {
            switch (code)
            {
                case 2: return "Succeeded";
                case 3: return "Succeeded (errors)";
                case 4: return "Failed";
                case 5: return "Aborted";
                case 1: return "In progress";
                default: return "Not started";
            }
        }

        // Uses the Windows Update Agent API via late-bound COM
        // (Microsoft.Update.Session) — Windows built-in, no project reference needed.
        private static List<Update_Record> Get_Update_History()
        {
            var list = new List<Update_Record>();

            Type sessionType = Type.GetTypeFromProgID("Microsoft.Update.Session");
            dynamic session = Activator.CreateInstance(sessionType);
            dynamic searcher = session.CreateUpdateSearcher();

            int count = searcher.GetTotalHistoryCount();
            if (count == 0) return list;

            dynamic history = searcher.QueryHistory(0, count);

            foreach (dynamic entry in history)
            {
                string title = entry.Title as string ?? "";
                string description = entry.Description as string ?? "";

                list.Add(new Update_Record
                {
                    Title = title,
                    Date = entry.Date,
                    Operation = Get_Operation_Text((int)entry.Operation),
                    ResultText = Get_Result_Text((int)entry.ResultCode),
                    Description = description,
                    SupportUrl = entry.SupportUrl as string ?? "",
                    KB = Extract_KB(title, description)
                });
            }

            list.Sort((a, b) => b.Date.CompareTo(a.Date));
            return list;
        }

        private async void btnUpdates_Click(object sender, EventArgs e)
        {
           
            Show_List_Output();
            Commands_Panel.Enabled = false;
            Cursor = Cursors.WaitCursor;
            try
            {
                Set_Message ("Double-click an entry for details");

                var updates = await Task.Run(() => Get_Update_History());
                
                lvUpdates.Items.Clear();
                foreach (var u in updates)
                {
                    var item = new ListViewItem(u.Date.ToString("yyyy-MM-dd HH:mm"));
                    item.SubItems.Add(u.Title);
                    item.SubItems.Add(u.KB);
                    item.SubItems.Add(u.Operation);
                    item.SubItems.Add(u.ResultText);
                    item.Tag = u;
                    lvUpdates.Items.Add(item);
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Could not retrieve update history:\n" + ex.Message,
                    "Admin Commands", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Commands_Panel.Enabled = true;
                Cursor = Cursors.Default;
                
            }
           
        }

        private void lvUpdates_DoubleClick(object sender, EventArgs e)
        {
            if (lvUpdates.SelectedItems.Count == 0) return;
            if (!(lvUpdates.SelectedItems[0].Tag is Update_Record rec)) return;

            string details =
                $"Title: {rec.Title}\r\n" +
                $"KB: {(string.IsNullOrEmpty(rec.KB) ? "(none found)" : rec.KB)}\r\n" +
                $"Date: {rec.Date}\r\n" +
                $"Operation: {rec.Operation}\r\n" +
                $"Result: {rec.ResultText}\r\n" +
                $"Support URL: {rec.SupportUrl}\r\n\r\n" +
                $"Description:\r\n{rec.Description}";

            MessageBox.Show(this, details, "Update Details",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void lvHotfixes_DoubleClick(object sender, EventArgs e)
        {
            if (lvHotfixes.SelectedItems.Count == 0) return;
            if (!(lvHotfixes.SelectedItems[0].Tag is Hotfix_Record rec)) return;

            string details =
                $"HotFix ID: {rec.HotFixID}\r\n" +
                $"Description: {rec.Description}\r\n" +
                $"Installed On: {rec.InstalledOn}\r\n" +
                $"Installed By: {(string.IsNullOrEmpty(rec.InstalledBy) ? "(unknown)" : rec.InstalledBy)}\r\n" +
                $"Status: {(string.IsNullOrEmpty(rec.Status) ? "(none)" : rec.Status)}\r\n" +
                $"Reference: {(string.IsNullOrEmpty(rec.Caption) ? "(none)" : rec.Caption)}\r\n\r\n" +
                $"Comments:\r\n{(string.IsNullOrEmpty(rec.FixComments) ? "(none)" : rec.FixComments)}";

            MessageBox.Show(this, details, "Hotfix Details",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void Set_Message(string text)
        {
            Message_Textbox.Text = text;
        }
        private void Clear_Message()
        {
            Message_Textbox.Text = "";
        }
    }
}