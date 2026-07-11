using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

// This is a PARTIAL of your existing MainForm — it merges with MainForm.cs.
// It expects your MainForm to already provide:
//    - a status label named  statusLabel   (Label)
//    - a method             RefreshSnapshots()
//    - a logger              Logger.Log(string category, string message)
// If your namespace isn't "AdminToolkit", change the line below to match.
namespace Admin_Tools
{
    public partial class MainForm : Form
    {
        // ================================================================
        //  SHARED HELPERS
        // ================================================================

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool GetVolumeNameForVolumeMountPoint(
            string lpszVolumeMountPoint, StringBuilder lpszVolumeName, int cchBufferLength);

        /// <summary>Runs a process and captures stdout+stderr and the exit code.</summary>
        private static (int exitCode, string output) RunCapture(string exe, string args)
        {
            try
            {
                var psi = new ProcessStartInfo(exe, args)
                {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };
                using (var p = Process.Start(psi))
                {
                    string outp = p.StandardOutput.ReadToEnd();
                    string err = p.StandardError.ReadToEnd();
                    p.WaitForExit(30000);
                    string combined = (outp + "\n" + err).Trim();
                    return (p.ExitCode, combined);
                }
            }
            catch (Exception ex) { return (-1, ex.Message); }
        }

        /// <summary>Logs a step result line-by-line under [Shadow].</summary>
        private void LogShadow(string step, int exit, string output)
        {
            Logger.Log("Shadow", step + " -> exit " + exit);
            if (!string.IsNullOrWhiteSpace(output))
                foreach (var line in output.Replace("\r\n", "\n").Split('\n'))
                    if (line.Trim().Length > 0)
                        Logger.Log("Shadow", "  " + line.TrimEnd());
        }

        private static string SafeLabel(DriveInfo d)
        {
            try { return d.VolumeLabel; } catch { return ""; }
        }

        /// <summary>True if System Protection is enabled for the given drive ("C:\").</summary>
        private static bool IsShadowingActive(string drive)
        {
            try
            {
                var sb = new StringBuilder(64);
                if (!GetVolumeNameForVolumeMountPoint(drive, sb, sb.Capacity))
                    return false;
                string volumeGuid = sb.ToString();   // \\?\Volume{GUID}\

                using (var key = Registry.LocalMachine.OpenSubKey(
                    @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\SPP\Clients"))
                {
                    var data = key?.GetValue("{09F7EDC5-294E-4180-AF6A-FB0E6A0E9513}") as string[];
                    return data != null &&
                           data.Any(v => v.IndexOf(volumeGuid, StringComparison.OrdinalIgnoreCase) >= 0);
                }
            }
            catch { return false; }
        }

        private static bool TaskExists(string taskName)
        {
            var r = RunCapture("schtasks.exe", "/Query /TN \"" + taskName + "\"");
            return r.exitCode == 0;
        }

        private static List<string> GetFixedNtfsDrives()
        {
            return DriveInfo.GetDrives()
                .Where(d => d.DriveType == DriveType.Fixed && d.IsReady &&
                            string.Equals(d.DriveFormat, "NTFS", StringComparison.OrdinalIgnoreCase))
                .Select(d => d.Name)
                .ToList();
        }

        /// <summary>Builds picker rows for setup: annotates active drives, pre-checks active + C:.</summary>
        private List<DriveChoice> BuildDriveChoices()
        {
            string sysDrive = Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows));
            var choices = new List<DriveChoice>();
            foreach (var d in DriveInfo.GetDrives())
            {
                if (d.DriveType != DriveType.Fixed || !d.IsReady) continue;
                if (!string.Equals(d.DriveFormat, "NTFS", StringComparison.OrdinalIgnoreCase)) continue;

                bool active = IsShadowingActive(d.Name);
                string label = string.IsNullOrWhiteSpace(SafeLabel(d)) ? "Local Disk" : SafeLabel(d);
                double gb = d.TotalSize / 1024.0 / 1024.0 / 1024.0;

                choices.Add(new DriveChoice
                {
                    Name = d.Name,
                    Display = string.Format("{0}  {1}  ({2:0} GB){3}",
                                d.Name.TrimEnd('\\'), label, gb, active ? "   — already active" : ""),
                    Checked = active || d.Name.Equals(sysDrive, StringComparison.OrdinalIgnoreCase)
                });
            }
            return choices;
        }

        /// <summary>(Re)registers AutoShadowCopy to snapshot exactly the given drives.</summary>
        private bool RegisterShadowTask(IReadOnlyList<string> drives)
        {
            const string taskName = "AutoShadowCopy";
            string volumeList = string.Join(",", drives.Select(d => "'" + d + "'"));
            string psInner =
                "foreach($v in " + volumeList + "){Invoke-CimMethod -ClassName Win32_ShadowCopy " +
                "-Namespace root/cimv2 -MethodName Create -Arguments @{Volume=$v;Context='ClientAccessible'}}";
            string psArgs = System.Security.SecurityElement.Escape(
                "-NoProfile -WindowStyle Hidden -Command \"" + psInner + "\"");

            string xml =
"<?xml version=\"1.0\" encoding=\"UTF-16\"?>" +
"<Task version=\"1.2\" xmlns=\"http://schemas.microsoft.com/windows/2004/02/mit/task\">" +
  "<Triggers><CalendarTrigger><StartBoundary>2020-01-01T12:00:00</StartBoundary>" +
    "<Enabled>true</Enabled><ScheduleByDay><DaysInterval>1</DaysInterval></ScheduleByDay>" +
  "</CalendarTrigger></Triggers>" +
  "<Principals><Principal id=\"Author\"><UserId>S-1-5-18</UserId>" +
    "<RunLevel>HighestAvailable</RunLevel></Principal></Principals>" +
  "<Settings><MultipleInstancesPolicy>IgnoreNew</MultipleInstancesPolicy>" +
    "<StartWhenAvailable>true</StartWhenAvailable></Settings>" +
  "<Actions Context=\"Author\"><Exec>" +
    "<Command>powershell.exe</Command><Arguments>" + psArgs + "</Arguments>" +
  "</Exec></Actions>" +
"</Task>";

            string xmlPath = Path.Combine(Path.GetTempPath(), "AutoShadowCopy.xml");
            File.WriteAllText(xmlPath, xml, Encoding.Unicode);   // must be UTF-16
            var t = RunCapture("schtasks.exe",
                "/Create /TN \"" + taskName + "\" /XML \"" + xmlPath + "\" /F");
            LogShadow("schtasks /Create /XML", t.exitCode, t.output);
            try { File.Delete(xmlPath); } catch { }
            return t.exitCode == 0;
        }

        // ================================================================
        //  SETUP  (turn shadowing ON)
        // ================================================================

        private bool CreateShadowTask(IReadOnlyList<string> selected, out string error)
        {
            error = "";
            const string taskName = "AutoShadowCopy";

            // Capture the "before" picture so we know whether anything actually changes.
            var beforeActive = GetFixedNtfsDrives().Where(IsShadowingActive).ToList();
            bool taskExistedBefore = TaskExists(taskName);

            // 1. Enable protection — selected + system drive (Windows requires the system drive)
            var toEnable = new List<string>(selected);
            string sysDrive = Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows));
            if (!toEnable.Any(d => d.Equals(sysDrive, StringComparison.OrdinalIgnoreCase)))
                toEnable.Insert(0, sysDrive);

            string driveArgs = string.Join(",", toEnable.Select(d => "'" + d + "'"));
            var r = RunCapture("powershell.exe",
                "-NoProfile -ExecutionPolicy Bypass -Command " +
                "\"$ErrorActionPreference='Stop'; Enable-ComputerRestore -Drive " + driveArgs + "\"");
            LogShadow("Enable-ComputerRestore " + string.Join(",", toEnable), r.exitCode, r.output);
            if (r.exitCode != 0)
            {
                error = "Enable-ComputerRestore failed (exit " + r.exitCode + ").";
                return false;
            }

            // 2. Scope the task to EVERY currently-protected drive, not just the picker
            //    selection — this is the fix. Fresh scan after enabling so it can't drift.
            var protectedNow = GetFixedNtfsDrives().Where(IsShadowingActive).ToList();
            // Safety: include anything we just enabled, in case the registry read lags.
            foreach (var d in toEnable)
                if (!protectedNow.Any(x => x.Equals(d, StringComparison.OrdinalIgnoreCase)))
                    protectedNow.Add(d);

            if (protectedNow.Count == 0)
            {
                error = "No protected drives to snapshot.";
                return false;
            }

            if (!RegisterShadowTask(protectedNow))
            {
                error = "Task creation failed. See log for details.";
                return false;
            }
            Logger.Log("Shadow", "Task scoped to: " + string.Join(", ", protectedNow));

            // 3. Remove the 24-hour throttle (idempotent, non-fatal)
            try
            {
                using (var key = Registry.LocalMachine.CreateSubKey(
                    @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore"))
                {
                    key?.SetValue("SystemRestorePointCreationFrequency", 0, RegistryValueKind.DWord);
                }
                Logger.Log("Shadow", "Throttle disabled (SystemRestorePointCreationFrequency=0)");
            }
            catch (Exception ex) { Logger.Log("Shadow", "Throttle registry skipped: " + ex.Message); }

            // 4. Run once now — only if a drive was newly enabled or the task didn't exist,
            //    so clicking Setup again on an unchanged machine won't spawn extra snapshots.
            bool newlyEnabled = protectedNow.Any(d =>
                !beforeActive.Any(b => b.Equals(d, StringComparison.OrdinalIgnoreCase)));
            if (newlyEnabled || !taskExistedBefore)
            {
                var run = RunCapture("schtasks.exe", "/Run /TN \"" + taskName + "\"");
                LogShadow("schtasks /Run", run.exitCode, run.output);
            }
            else
            {
                Logger.Log("Shadow", "No new drives enabled; skipped the immediate run.");
            }

            return true;
        }

        private void btnSetupShadowing_Click(object sender, EventArgs e)
        {
            const string taskName = "AutoShadowCopy";

            var choices = BuildDriveChoices();
            if (choices.Count == 0)
            {
                statusLabel.Text = "No fixed NTFS drives found to protect.";
                return;
            }

            List<string> drives;
            using (var dlg = new DriveSelectDialog(choices))
            {
                if (dlg.ShowDialog(this) != DialogResult.OK)
                {
                    statusLabel.Text = "Shadowing setup cancelled.";
                    return;
                }
                drives = dlg.SelectedDrives;
            }

            if (drives.Count == 0)
            {
                statusLabel.Text = "No drives selected — nothing to do.";
                return;
            }

            if (drives.All(d => IsShadowingActive(d)) && TaskExists(taskName))
            {
                statusLabel.Text = "Selected drives already shadowed — nothing to do.";
                return;
            }

            statusLabel.Text = "Setting up shadowing on " + string.Join(", ", drives) + "...";
            Application.DoEvents();

            if (CreateShadowTask(drives, out string error))
            {
                statusLabel.Text = "Shadowing enabled on " + string.Join(", ", drives) +
                    " and daily task created (ran once now).";
              //  RefreshSnapshots();
            }
            else
            {
                MessageBox.Show(
                    "Setup failed.\n\n" + error + "\n\nCheck that you're elevated and that System " +
                    "Restore isn't disabled by group policy. See the log for the full details.",
                    "Setup Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                statusLabel.Text = "Shadowing setup failed.";
            }
        }

        // ================================================================
        //  TEARDOWN  (turn shadowing OFF)
        // ================================================================

        private bool TeardownShadowing(IReadOnlyList<string> offDrives,
                                       IReadOnlyList<string> survivors, out string error)
        {
            const string taskName = "AutoShadowCopy";
            error = "";

            // 1. Disable protection on the off-drives (one call; this wipes their snapshots)
            string driveArgs = string.Join(",", offDrives.Select(d => "'" + d + "'"));
            var r = RunCapture("powershell.exe",
                "-NoProfile -ExecutionPolicy Bypass -Command " +
                "\"$ErrorActionPreference='Stop'; Disable-ComputerRestore -Drive " + driveArgs + "\"");
            LogShadow("Disable-ComputerRestore " + string.Join(",", offDrives), r.exitCode, r.output);
            if (r.exitCode != 0)
            {
                error = "Disable-ComputerRestore failed (exit " + r.exitCode + ").";
                return false;
            }

            // 2. Task: rebuild for survivors, or delete if nothing is protected anymore
            if (survivors.Count > 0)
            {
                if (!RegisterShadowTask(survivors))
                {
                    error = "Protection was disabled, but rebuilding the task failed.";
                    return false;
                }
            }
            else
            {
                var d = RunCapture("schtasks.exe", "/Delete /TN \"" + taskName + "\" /F");
                LogShadow("schtasks /Delete", d.exitCode, d.output);
                // exit 1 just means the task didn't exist — not a failure for teardown.

                // Nothing protected now — undo the throttle override from setup.
                try
                {
                    using (var key = Registry.LocalMachine.OpenSubKey(
                        @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore", writable: true))
                    {
                        if (key?.GetValue("SystemRestorePointCreationFrequency") != null)
                        {
                            key.DeleteValue("SystemRestorePointCreationFrequency", false);
                            Logger.Log("Shadow", "Throttle override removed (back to Windows default)");
                        }
                    }
                }
                catch (Exception ex) { Logger.Log("Shadow", "Throttle cleanup skipped: " + ex.Message); }
            }

            return true;
        }

        private void btnTeardownShadowing_Click(object sender, EventArgs e)
        {
            string sysDrive = Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows));

            // Currently-protected fixed NTFS drives
            var protectedDrives = GetFixedNtfsDrives()
                .Where(name => IsShadowingActive(name))
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
                if (dlg.ShowDialog(this) != DialogResult.OK)
                {
                    statusLabel.Text = "Teardown cancelled.";
                    return;
                }
                offList = dlg.SelectedDrives;
            }

            if (offList.Count == 0)
            {
                statusLabel.Text = "No drives selected — nothing to do.";
                return;
            }

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

            if (confirm != DialogResult.Yes)
            {
                statusLabel.Text = "Teardown cancelled.";
                return;
            }

            statusLabel.Text = "Turning off shadowing on " + willDelete + "...";
            Application.DoEvents();

            if (TeardownShadowing(offList, survivors, out string error))
            {
                statusLabel.Text = "Shadowing turned off on " + willDelete +
                    (survivors.Count > 0
                        ? " — task now covers " + string.Join(", ", survivors) + "."
                        : " — task removed.");
                //  RefreshSnapshots();
            }
            else
            {
                MessageBox.Show("Turn-off failed.\n\n" + error + "\n\nCheck the log for details.",
                    "Teardown Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                statusLabel.Text = "Teardown failed.";
            }
        }

        // ================================================================
        //  HELP
        // ================================================================

        private void btnHelp_Click(object sender, EventArgs e)
        {
            using (var dlg = new HelpDialog())
                dlg.ShowDialog(this);
        }
    }
}
