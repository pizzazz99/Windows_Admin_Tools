// ============================================================
//  Remote_Desktop_Dialog.cs
//  Small picker for starting a Windows Remote Desktop (RDP)
//  session. The target combo is editable, so the user can
//  EITHER pick a machine discovered on the local network OR
//  type any hostname / IP address (including a Tailscale name).
//
//  Discovered hosts are remembered in Lan_Scanner's in-memory
//  cache for the life of the app, so re-opening this dialog
//  shows the last results instantly. Scan forces a fresh sweep.
//
//  Scan   : sweep the local /24 for hosts with RDP (3389) open.
//  Test   : pre-flight the current target (resolve + 3389 check).
//  Connect: launch the built-in mstsc.exe client.
// ============================================================

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Admin_Tools
{
    public partial class Remote_Desktop_Dialog : Form
    {
        public Remote_Desktop_Dialog()
        {
            InitializeComponent();
        }

        // If a previous scan is remembered, show it immediately
        // (instant, no network activity). Otherwise leave the list
        // empty until the user clicks Scan.
        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (Lan_Scanner.Has_Cache)
                await Populate_Computers(forceRescan: false);
        }

        // The clean target to act on: a picked (un-edited) list
        // item carries one; otherwise use whatever was typed.
        private string Current_Target()
        {
            if (cboTarget.SelectedItem is Rdp_Target picked)
                return picked.ConnectTarget;

            return (cboTarget.Text ?? "").Trim().TrimStart('\\');
        }

        // --------------------------------------------------------
        //  Fill the dropdown from the cache or a fresh scan. The
        //  field stays free-text either way, so manual entry
        //  always works even if discovery finds nothing.
        // --------------------------------------------------------
        private async Task Populate_Computers(bool forceRescan)
        {
            bool willScan = forceRescan || !Lan_Scanner.Has_Cache;

            Scan_Button.Enabled = false;
            string originalHint = lblHint.Text;
            lblHint.Text = willScan
                ? "Scanning local network for Remote Desktop hosts..."
                : "Loading remembered hosts...";

            try
            {
                List<Rdp_Target> targets = await Lan_Scanner.Get_Hosts(forceRescan);

                string selected = cboTarget.Text;      // preserve any typing
                cboTarget.Items.Clear();
                foreach (Rdp_Target t in targets)
                    cboTarget.Items.Add(t);
                cboTarget.Text = selected;

                lblHint.Text = Build_Status(targets.Count);
            }
            catch
            {
                lblHint.Text = originalHint;
            }
            finally
            {
                Scan_Button.Enabled = true;
            }
        }

        private static string Build_Status(int count)
        {
            if (count == 0)
                return "No hosts auto-detected — type a hostname or IP address.";

            string when = Lan_Scanner.Last_Scan_Local.HasValue
                ? " (scanned " + Lan_Scanner.Last_Scan_Local.Value.ToString("h:mm tt") + ")"
                : "";

            return $"{count} host(s) remembered{when} — or type any hostname / IP.";
        }

        private async void Scan_Button_Click(object sender, EventArgs e)
        {
            await Populate_Computers(forceRescan: true);
        }

        // --------------------------------------------------------
        //  Test — resolve the current target and check whether the
        //  RDP port is open, without launching a session.
        // --------------------------------------------------------
        private async void Test_Button_Click(object sender, EventArgs e)
        {
            string target = Current_Target();
            if (string.IsNullOrEmpty(target))
            {
                MessageBox.Show(this,
                    "Enter or pick a computer name or IP address to test.",
                    "Remote Desktop", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Test_Button.Enabled = false;
            string originalHint = lblHint.Text;
            lblHint.Text = "Testing " + target + " ...";

            try
            {
                Host_Test test = await Lan_Scanner.Test_Host(target);
                string where = test.ResolvedIp != null ? $" ({test.ResolvedIp})" : "";

                lblHint.Text = test.PortOpen
                    ? $"Reachable: {target}{where} — RDP port 3389 is open."
                    : $"No response: {target}{where} — RDP port 3389 closed or unreachable.";
            }
            catch
            {
                lblHint.Text = originalHint;
            }
            finally
            {
                Test_Button.Enabled = true;
            }
        }

        // --------------------------------------------------------
        //  Connect — launch mstsc against the current target.
        //  mstsc /v: accepts a hostname or an IP address.
        // --------------------------------------------------------
        private void Connect_Button_Click(object sender, EventArgs e)
        {
            string target = Current_Target();
            if (string.IsNullOrEmpty(target))
            {
                MessageBox.Show(this,
                    "Enter or pick a computer name or IP address.",
                    "Remote Desktop", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                Logger.Log("RDP", target);
                Process.Start(new ProcessStartInfo("mstsc.exe", "/v:" + target)
                {
                    UseShellExecute = true
                });

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,
                    "Could not launch Remote Desktop:\n" + ex.Message,
                    "Remote Desktop", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Close without connecting.
        private void Quit_Button_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}