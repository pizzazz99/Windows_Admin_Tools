# Admin Toolkit

A Windows Forms utility that puts the most-used Windows administration tools, snapshot/restore features, and activity logging in one place. Designed for technicians and power users who bounce between `mmc` consoles, System Restore, and VSS shadow copies all day.

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
- **Restore Points** — view named System Restore points (via WMI `SystemRestore`) with creation time and type, plus raw `vssadmin list shadows` output
- **Snapshot Operations** — dedicated shadow-copy management window
- **Registry Backup** — dedicated registry backup window

### Activity log
Everything the Toolkit does — launches, kills, snapshot operations, emails — is written to a per-machine activity log.

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

## Roadmap

- [ ] Finish the email log feature (SMTP send + settings UI) — scaffolding already in place, contributions welcome
- [ ] Add screenshots to this README

## License

TBD — add your preferred license (MIT is a common choice for utilities like this).
