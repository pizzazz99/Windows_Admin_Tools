// ============================================================
//  Remote_Access_Form.cs
//  Diagnostics board for the remote-access stack (Tailscale +
//  RustDesk). Mirrors Commands_Form: each app has its own
//  GroupBox that is enabled only when the app is detected.
//  App CLI commands run through the resolved exe path; config
//  files are read locally with secrets masked.
//
//  Usage from MainForm (modeless):
//      var f = new Remote_Access_Form();
//      f.Show(this);
// ============================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Admin_Tools
{
    public partial class Remote_Access_Form : Form
    {
        // Resolved exe paths for apps that were actually found.
        private readonly Dictionary<External_App, string> _appPaths =
            new Dictionary<External_App, string>();

        public Remote_Access_Form()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Detect_Apps();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!Apps_Panel.Enabled)   // a command is running
            {
                e.Cancel = true;
                return;
            }
            base.OnFormClosing(e);
        }

        // --------------------------------------------------------
        //  Detection — enable a group only if its app is present.
        // --------------------------------------------------------
        private void Detect_Apps()
        {
            Apply_App_State(External_App.Tailscale, grpTailscale);
            Apply_App_State(External_App.RustDesk, grpRustDesk);
        }

        private void Apply_App_State(External_App app, GroupBox group)
        {
            string path = App_Locator.Resolve(app);

            if (path != null)
            {
                _appPaths[app] = path;
                group.Enabled = true;
            }
            else
            {
                group.Enabled = false;
                group.Text += "  (not installed)";
            }
        }

        // --------------------------------------------------------
        //  Core runner — every command funnels through here.
        // --------------------------------------------------------
        private async Task Run_Line_Async(string commandLine)
        {
            try
            {
                Apps_Panel.Enabled = false;
                Logger.Log("Command", commandLine);
                await Command_Runner.Run_Command_Async(commandLine, txtOutput);
            }
            catch (Exception Ex)
            {
                txtOutput.Text = Ex.ToString();
            }
            finally
            {
                Apps_Panel.Enabled = true;
            }
        }

        // Runs "<resolved exe path>" <arguments> for an app command.
        private Task Execute_App_Command_Async(External_App app, string arguments)
        {
            if (!_appPaths.TryGetValue(app, out string exePath))
            {
                MessageBox.Show(this,
                    $"{App_Locator.Display_Name(app)} does not appear to be installed on this machine.",
                    "Remote Access Tools", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return Task.CompletedTask;
            }

            string commandLine = ($"\"{exePath}\" {arguments}").TrimEnd();
            return Run_Line_Async(commandLine);
        }

        // --------------------------------------------------------
        //  Tailscale
        // --------------------------------------------------------
        private async void BtnTsStatus_Click(object Sender, EventArgs e)
            => await Execute_App_Command_Async(External_App.Tailscale, "status");

        private async void BtnTsStatusJson_Click(object Sender, EventArgs e)
            => await Execute_App_Command_Async(External_App.Tailscale, "status --json");

        private async void BtnTsIp_Click(object Sender, EventArgs e)
            => await Execute_App_Command_Async(External_App.Tailscale, "ip -4");

        private async void BtnTsNetcheck_Click(object Sender, EventArgs e)
            => await Execute_App_Command_Async(External_App.Tailscale, "netcheck");

        private async void BtnTsDns_Click(object Sender, EventArgs e)
            => await Execute_App_Command_Async(External_App.Tailscale, "dns status");

        // "debug prefs" prints the current daemon preferences —
        // the closest thing to a Tailscale config dump that does
        // NOT expose the node's private key (that lives in the
        // protected tailscaled.state file, which we never read).
        private async void BtnTsPrefs_Click(object Sender, EventArgs e)
            => await Execute_App_Command_Async(External_App.Tailscale, "debug prefs");

        private async void BtnTsVersion_Click(object Sender, EventArgs e)
            => await Execute_App_Command_Async(External_App.Tailscale, "version");

        // --------------------------------------------------------
        //  RustDesk
        // --------------------------------------------------------
        private async void BtnRdGetId_Click(object Sender, EventArgs e)
            => await Execute_App_Command_Async(External_App.RustDesk, "--get-id");

        private async void BtnRdVersion_Click(object Sender, EventArgs e)
            => await Execute_App_Command_Async(External_App.RustDesk, "--version");

        // Plain shell query — no exe path needed — but it lives in
        // the RustDesk group so it disables when RustDesk is absent.
        private async void BtnRdServiceStatus_Click(object Sender, EventArgs e)
            => await Run_Line_Async("sc query rustdesk");

        // Locally-read config (no shell); secrets are masked.
        private void BtnRdConfig_Click(object Sender, EventArgs e)
            => txtOutput.Text = Get_RustDesk_Config();

        // --------------------------------------------------------
        //  Remote Desktop (RDP) — always available (mstsc ships
        //  with Windows). Opens a picker where the user can select
        //  a discovered PC or type any hostname / IP address.
        // --------------------------------------------------------
        private void BtnRdpConnect_Click(object Sender, EventArgs e)
        {
            using (var dlg = new Remote_Desktop_Dialog())
            {
                dlg.ShowDialog(this);
            }
        }

        // --------------------------------------------------------
        //  RustDesk config reader (secrets masked)
        // --------------------------------------------------------

        // RustDesk stores its .toml config per-profile. For a
        // user-run client it's under %APPDATA%; for the installed
        // service it's under the LocalService / SYSTEM profile.
        private static string[] RustDesk_Config_Dirs()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return new[]
            {
                Path.Combine(appData, @"RustDesk\config"),
                @"C:\Windows\ServiceProfiles\LocalService\AppData\Roaming\RustDesk\config",
                @"C:\Windows\System32\config\systemprofile\AppData\Roaming\RustDesk\config"
            };
        }

        private static readonly string[] _rdConfigFiles = { "RustDesk2.toml", "RustDesk.toml" };

        // Keys whose values are secret and must never be shown.
        // Matched case-insensitively as a substring of the key,
        // so "key_pair" is caught while the (non-secret) server
        // "key" stays visible for triage. Edit this list to taste.
        private static readonly string[] _sensitiveKeys =
        {
            "password", "salt", "enc_id", "key_pair", "secret", "token", "private"
        };

        private static string Get_RustDesk_Config()
        {
            var sb = new StringBuilder();
            bool foundAny = false;

            foreach (string dir in RustDesk_Config_Dirs())
            {
                foreach (string file in _rdConfigFiles)
                {
                    string full = Path.Combine(dir, file);
                    if (!File.Exists(full)) continue;

                    foundAny = true;
                    sb.AppendLine("# ===== " + full + " =====");
                    try
                    {
                        foreach (string line in File.ReadAllLines(full))
                        {
                            sb.AppendLine(Mask_If_Sensitive(line));
                        }
                    }
                    catch (Exception Ex)
                    {
                        sb.AppendLine("  (could not read: " + Ex.Message + ")");
                    }
                    sb.AppendLine();
                }
            }

            if (!foundAny)
            {
                return "No RustDesk config files found in the known locations:\r\n  " +
                       string.Join("\r\n  ", RustDesk_Config_Dirs());
            }

            sb.AppendLine("# Sensitive values (passwords, keys, salts) are masked.");
            return sb.ToString();
        }

        // Masks the value of a "key = value" TOML line when the
        // key matches the sensitive list. Section headers, blank
        // lines, and comments pass through untouched.
        private static string Mask_If_Sensitive(string line)
        {
            int eq = line.IndexOf('=');
            if (eq <= 0) return line;

            string key = line.Substring(0, eq).Trim().ToLowerInvariant();
            foreach (string sensitive in _sensitiveKeys)
            {
                if (key.Contains(sensitive))
                {
                    return line.Substring(0, eq + 1) + " ********";
                }
            }
            return line;
        }

        // --------------------------------------------------------
        //  Output pane buttons
        // --------------------------------------------------------
        private void Btn_Clear_Click(object Sender, EventArgs e)
        {
            txtOutput.Clear();
        }

        private void Btn_Copy_All_Click(object Sender, EventArgs e)
        {
            if (txtOutput.TextLength == 0) return;
            Clipboard.SetText(txtOutput.Text);
        }

        private void Btn_Save_Click(object Sender, EventArgs e)
        {
            if (txtOutput.TextLength == 0) return;

            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                dialog.FileName = Environment.MachineName + "_remote_" +
                                  DateTime.Now.ToString("yyyyMMdd_HHmm") + ".txt";

                if (dialog.ShowDialog(this) != DialogResult.OK) return;

                try
                {
                    File.WriteAllText(dialog.FileName, txtOutput.Text);
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(this, "Could not save the file:\n" + Ex.Message,
                        "Remote Access Tools", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Btn_Close_Click(object Sender, EventArgs e)
        {
            Close();
        }
    }
}