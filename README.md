# Admin Toolkit

A Windows Forms utility that puts the most-used Windows administration tools, snapshot/restore features, and activity logging in one place. Designed for technicians and power users who bounce between `mmc` consoles, System Restore, and VSS shadow copies all day.

![Windows](https://img.shields.io/badge/platform-Windows-blue)
![.NET](https://img.shields.io/badge/.NET-10%20%7C%20Windows%20Forms-512BD4)
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

### Restore Points

A full System Restore point manager built on the WMI `SystemRestore` class, going well beyond what the built-in System Protection dialog shows.

**Viewing & inspection**

- Sortable list of all restore points with sequence number, creation time, age, type, event, description, and whether a linked shadow copy still exists
- Selecting a point shows full details: attribution (type/event codes with plain-English explanations), the linked shadow copy's ID and device path (matched by timestamp), and contextual notes
- **Gap detection** — Windows never reuses sequence numbers, so missing numbers between listed points are flagged ("point #31 no longer exists — deleted or aged out"), both in the summary line and per-selection
- **"What would restoring remove?"** — for the selected point, the Toolkit cross-references the registry's installed-programs list (`Uninstall` keys, both 64/32-bit hives plus per-user) and lists every program installed on or after that point's date. Best-effort (install dates are day-granular and some installers omit them), but it answers the question that matters before a rollback
- A status line shows System Protection state, the current creation-throttle setting, and shadow storage usage (`used / max`) at a glance

**Creating points**

- **Create Restore Point** button with a description prompt
- If System Protection is off, the Toolkit offers to enable it on the spot (WMI `SystemRestore.Enable`)
- **Throttle-aware:** by default Windows silently skips creating a restore point if one was made in the last 24 hours — the API even reports success. The Toolkit predicts this before calling, warns you, and offers to disable the throttle (`SystemRestorePointCreationFrequency = 0`) so every request is honored. Ideal for the install-several-apps workflow: checkpoint before each install, roll back to exactly the right moment if one goes bad
- Creation runs on a background thread so the UI stays responsive

**Deleting points**

- **Delete Selected** supports multi-select (Ctrl+click / Shift+click / Ctrl+A) with a confirmation listing exactly which points — and their underlying shadow copies — will be removed. Failures are collected and reported per-point rather than aborting the batch

**Browsing a snapshot's files**

- **Browse Files at This Point** mounts the selected point's shadow copy as a read-only directory symlink in `%TEMP%` and opens it in Explorer — the entire volume exactly as it existed at that moment. Recover a config file as it was before an install, or diff old vs. current. Links are cleaned up automatically when the window closes; the snapshot itself is never modified

### Snapshots & shadow copies
- **Enable Shadowing** — pick which fixed NTFS drives to protect; registers a daily `AutoShadowCopy` scheduled task covering the selected drives
- **Disable Shadowing** — selectively turn protection off per drive, with a clear warning that snapshots will be deleted. Handles the Windows rule that disabling the system drive cascades to all other protected drives, and rebuilds or removes the scheduled task accordingly
- **Snapshot Operations** — dedicated shadow-copy management window with per-snapshot detail view (flags, provider, device object, persistence attributes)
- **Raw vssadmin view** — one click shows `vssadmin list shadows` output filtered to the selected snapshot (`/shadow={ID}`), or all snapshots when nothing is selected. Detail windows are modeless, so the WMI-based details and the vssadmin output for the same snapshot can be compared side by side
- **Registry Backup** — dedicated registry backup window

### Multi-window workflow
Tool windows (Restore Points, Snapshot Operations, Registry Backup) open modeless, so the main window and several tool windows can be used simultaneously. Each launcher button disables while its window is open — a built-in "already open" indicator — and re-enables when the window closes.

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

- Windows 10 or Windows 11 (x64)
- **To run the published release:** nothing — it's a self-contained single-file executable with the .NET runtime bundled in
- **To build:** Visual Studio 2022 or 2026 with the .NET 10 SDK (the project uses the SDK-style format and targets `net10.0-windows`)
- **Administrator rights** for snapshot, VSS, restore point, and scheduled-task features (the app runs without elevation, but those features will fail)
- NTFS fixed drives for shadow-copy protection

## Getting Started

1. Clone the repository:
   ```
   git clone https://github.com/<your-user>/admin-toolkit.git
   ```
2. Open the solution in Visual Studio and build (`Ctrl+Shift+B`).
3. Run the executable **as Administrator** (right-click → *Run as administrator*) to unlock all features.

## Building a standalone executable

The project publishes as a self-contained, single-file binary that runs on any 64-bit Windows machine with no .NET installation:

```
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

Or in Visual Studio: right-click the project → **Publish** → Folder target, with **Self-contained**, **win-x64**, and **Produce single file** selected. The output lands in `bin\Release\net10.0-windows\win-x64\publish\`. Expect roughly 70 MB — the entire runtime is inside the exe.

## How It Works (implementation notes)

- Launched tools are tracked in a `LaunchedTool` list tying together the `Process`, its `ListViewItem`, and the launcher `Button`, so UI state stays in sync via the process `Exited` event plus a polling safety net.
- The live log viewer tails the log file with `FileShare.ReadWrite` reads, so it never blocks the logger from writing, and automatically recovers if the file is rotated or purged.
- Shadow-copy scheduling is done through `schtasks.exe`; restore-point enumeration uses the WMI `root\default\SystemRestore` class, correlated with `Win32_ShadowCopy` instances by timestamp.
- Restore point creation calls `SystemRestore.CreateRestorePoint` via WMI; deletion uses `SRRemoveRestorePoint` from `srclient.dll`. The creation throttle is read from and written to `HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore\SystemRestorePointCreationFrequency`.
- Snapshot browsing creates a directory symlink (`Directory.CreateSymbolicLink`, .NET 6+) pointing at the shadow copy's `\\?\GLOBALROOT\Device\HarddiskVolumeShadowCopyNN` device object. Deleting the symlink removes only the link — the snapshot is untouched.
- Shadow storage figures come from the `Win32_ShadowStorage` WMI class (equivalent to `vssadmin list shadowstorage`).

## A note on restore points as a safety net

Restore points revert system files, the registry, drivers, and installed programs to a moment in time; personal documents are untouched. They're an excellent *undo* for risky installs and settings changes — checkpoint before each change, roll back to just before the one that went wrong. They are **not** a security boundary: persistent malware can survive a rollback. For genuinely untrusted software, use a VM or sandbox instead.

## Roadmap

- [ ] Finish the email log feature (SMTP send + settings UI) — scaffolding already in place, contributions welcome
- [ ] Add screenshots to this README
- [ ] Per-drive shadow storage breakdown / resize (`vssadmin resize shadowstorage`)
- [ ] Optional registry-hive diff between a snapshot and the live system (pairs with the Registry Backup window)

## License

TBD — add your preferred license (MIT is a common choice for utilities like this).
