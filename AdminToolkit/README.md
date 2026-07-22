# Admin Toolkit

A Windows Forms utility that puts the most-used Windows administration tools, snapshot/restore features, remote-access diagnostics, and activity logging in one place. Designed for technicians and power users who bounce between `mmc` consoles, System Restore, VSS shadow copies, and remote-support tools all day.

![Windows](https://img.shields.io/badge/platform-Windows-blue)
![.NET](https://img.shields.io/badge/.NET-Windows%20Forms-512BD4)
![License](https://img.shields.io/badge/license-TBD-lightgrey)

<!-- Screenshot: replace with a real capture of the main window -->
<!-- ![Main window](docs/screenshot-main.png) -->

## Features

### One-click admin tool launcher
Launch the standard Windows administration tools without hunting through menus:

| Tool | Target |
|---|---|
| Task Scheduler | `taskschd.msc` |
| System Protection | `SystemPropertiesProtection.exe` |
| System Restore Wizard | `rstrui.exe` |
| Registry Editor | `regedit.exe` |
| Event Viewer | `eventvwr.msc` |
| Services | `services.msc` |
| Disk Management | `diskmgmt.msc` |
| Computer Management | `compmgmt.msc` |
| System Information | `msinfo32.exe` |
| Performance Monitor | `perfmon.msc` |
| Resource Monitor | `resmon.exe` |
| Device Manager | `devmgmt.msc` |
| Local Users & Groups | `lusrmgr.msc` |
| Windows Firewall | `wf.msc` |

Each launcher button is color-coded: **green** = ready, **amber** = that tool is currently running. Buttons re-enable automatically when the tool closes.

### Launched-process monitor
Every tool started from the Toolkit appears in a process list showing its name, PID, launch time, and status. From there you can:

- **Close** — polite shutdown (equivalent to clicking the tool's X button)
- **End Task** — force-terminate a hung tool
- **Remove Closed** — clear finished entries from the list

A background timer catches processes that exit without raising an event (e.g., shell handoffs), so the status column stays accurate.

### Snapshots & shadow copies
- **Enable Shadowing** — pick which fixed NTFS drives to protect; registers a daily `AutoShadowCopy` scheduled task covering the selected drives
- **Disable Shadowing** — selectively turn protection off per drive, with a clear warning that snapshots will be deleted. Handles the Windows rule that disabling the system drive cascades to all other protected drives, and rebuilds or removes the scheduled task accordingly
- **Restore Points** — a full System Restore point manager built on the WMI `SystemRestore` class: sortable list with sequence number, creation time, age, type, event, description, and linked shadow copy; gap detection for missing sequence numbers; "what would restoring remove" cross-referenced against installed-programs registry keys; throttle-aware creation (Windows silently skips a point if one was made in the last 24 hours — the Toolkit detects this and offers to disable the throttle); multi-select delete with per-point failure reporting; and a **Browse Files at This Point** option that mounts a selected snapshot as a read-only directory in `%TEMP%`
- **Snapshot Operations** — dedicated shadow-copy management window
- **Registry Backups** — answers "is my registry backed up?" by checking every place a backup can exist and giving a plain-English verdict: the `RegBack` folder (`C:\Windows\System32\config\RegBack`, disabled by default since Windows 10 v1803 — files show 0 bytes when inactive), the `EnablePeriodicBackup` setting controlling it, the `RegIdleBackup` scheduled task's last run/result, and a count of registry-bearing restore points and shadow copies with the newest of each. **Enable RegBack** flips the setting on, **Backup Now** fires the task immediately, **Open Folder** and **Refresh** round it out

### Remote access
A diagnostics window (`Remote_Access_Form`) for the remote-support stack, laid out the same way as the admin tool launchers: one group per app, auto-enabled only when that app is actually detected on the machine (`App_Locator.Resolve`).

- **Tailscale** — status, status (JSON), current Tailscale IP (`ip -4`), `netcheck`, DNS status, daemon preferences (`debug prefs` — deliberately avoids reading the protected `tailscaled.state` file, so the node's private key is never exposed), and version. All commands run through the resolved `tailscale.exe` path and stream output to a shared results pane
- **RustDesk** — get this machine's RustDesk ID, version, and Windows service status (`sc query rustdesk`); a **Config** button reads the local `RustDesk2.toml` / `RustDesk.toml` directly from disk (checking the per-user, LocalService, and SYSTEM profile locations) with passwords, keys, salts, and tokens automatically masked before display
- **Remote Desktop (RDP)** — always available since `mstsc` ships with Windows. Opens a picker (`Remote_Desktop_Dialog`) with an editable target field: choose a host discovered on the local network or type any hostname, IP, or Tailscale name. **Scan** sweeps the local /24 for hosts with port 3389 open and caches the results for the life of the app; **Test** pre-flights the current target (resolve + port check) without connecting; **Connect** launches `mstsc.exe /v:<target>`
- Every command run and every RDP connection attempt is written to the shared activity log
- Results pane includes **Clear**, **Copy All**, and **Save** (to a timestamped `.txt` file) for grabbing diagnostic output to send along with a support request

### Help panel
A built-in reference (`HelpDialog`) documenting the app and when to use each function, opened from a **Help** button rather than sending anyone to an external doc. It's a plain-text panel with lightweight, automatic styling rather than a static wall of text:

- The title, section headers (any line immediately followed by a `----`/`====` rule), and a handful of inline sub-labels (`WHEN TO USE`, `MULTI-DRIVE NOTE`, `SYSTEM-DRIVE RULE`, `RECOVERY NOTE`, `INSTALL WORKFLOW`, `THE CATCH`) are auto-detected and bolded/colored at render time — no manual markup needed beyond the underline convention
- The **Important Caveats** block is colored as a whole to flag it as must-read (snapshots aren't a real backup, deleting a restore point has no undo, restoring removes newer programs, etc.)
- Content walks through every panel in the app — Shadow Copies, Restore Points, Setup/Turn Off Shadowing, Admin Tools, Launched Tools, Registry Backups, and the command output/log — explaining not just what each button does but when you'd reach for it
- Read-only, single window, closes with **Close** or Esc/Enter

### Activity log
Everything the Toolkit does — launches, kills, snapshot operations, remote-access commands, emails — is written to a per-machine activity log.

- **View Log** — opens a live, auto-tailing log window that updates once per second while the app runs
- **Purge** — clear the log file from the viewer (with confirmation)
- **Print** — send the current log contents to any installed printer

### Email log (not yet implemented)
Groundwork exists in the code for emailing the activity log as an attachment via SMTP — including a settings-file template and DPAPI-encrypted password storage (per-user, per-machine, never stored as plain text). The **Email Log** and **Email Settings** buttons are currently hidden in the UI pending completion. Contributions welcome — see the Roadmap below.

### Elevation awareness
On startup the app checks whether it's running as Administrator. If not, a red banner appears across the top of the window and the status bar warns that snapshot/VSS actions will fail. The tool launcher still works un-elevated for tools that don't need it.

## Requirements

- Windows 10 or Windows 11
- .NET (Windows Forms) — build with Visual Studio 2022
- **Administrator rights** for snapshot, VSS, and scheduled-task features (the app runs without elevation, but those features will fail)
- NTFS fixed drives for shadow-copy protection
- Optional, for the Remote Access panel: [Tailscale](https://tailscale.com/) and/or [RustDesk](https://rustdesk.com/) installed — each group is simply disabled if its app isn't found. Remote Desktop requires the target machine to be running Windows Pro/Enterprise/Education (Windows Home cannot host incoming RDP sessions)

## Getting Started

1. Clone the repository:
   ```
   git clone https://github.com/<your-user>/admin-toolkit.git
   ```
2. Open the solution in Visual Studio and build (`Ctrl+Shift+B`).
3. Run the executable **as Administrator** (right-click → *Run as administrator*) to unlock all features.

## How It Works (implementation notes)

- Launched tools are tracked in a `LaunchedTool` list tying together the `Process`, its `ListViewItem`, and the launcher `Button`, so UI state stays in sync via the process `Exited` event plus a polling safety net.
- The live log viewer tails the log file with `FileShare.ReadWrite` reads, so it never blocks the logger from writing, and automatically recovers if the file is rotated or purged.
- Shadow-copy scheduling is done through `schtasks.exe`; restore-point enumeration uses the WMI `root\default\SystemRestore` class.
- The Remote Access panel follows the same "disable while a command runs" pattern as the rest of the app: `Execute_App_Command_Async` resolves the app's exe path via `App_Locator`, and `Run_Line_Async` locks the panel, logs the command, and streams output through `Command_Runner` until it completes.
- `Lan_Scanner` caches its RDP host-discovery results in memory for the life of the app, so reopening the Remote Desktop dialog shows the last scan instantly; a manual **Scan** forces a fresh sweep.
- RustDesk config values are masked by matching key names against a small sensitive-key list (`password`, `salt`, `enc_id`, `key_pair`, `secret`, `token`, `private`) rather than parsing full TOML, keeping the reader simple and dependency-free.

## Roadmap

- [ ] Finish the email log feature (SMTP send + settings UI) — scaffolding already in place, contributions welcome
- [ ] Add screenshots to this README

## License

TBD — add your preferred license (MIT is a common choice for utilities like this).