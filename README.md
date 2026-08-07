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

### Printer management
A dedicated window (**Printers** on the main form) for auditing and troubleshooting every printer Windows knows about — not just what the standard "Printers & Scanners" settings page shows.

- Full list of installed printers pulled from WMI `Win32_Printer`, including virtual/software printers (Print to PDF, XPS Document Writer, Fax, Send to OneNote, remote-session printers like RustDesk) — hideable with a checkbox since they're rarely what you're troubleshooting
- **Printer Details** — a full per-printer dump (driver, port, server, WMI status/state, resolution, capabilities) plus its resolved network address, in one text window
- **IP Address column**, resolved through a layered fallback chain — Windows caches printer addresses inconsistently depending on how each printer was installed:
  1. `MSFT_PrinterPort` (the modern print-port WMI class; covers WSD ports)
  2. Classic `Win32_TCPIPPrinterPort`
  3. Address embedded in the port name itself (`IP_192.168.1.5`, `LAN_192.168.1.5`, etc.)
  4. Address embedded in the printer's `Location` field, which Windows auto-populates for WSD-discovered printers — handles IPv4, IPv6 link-local addresses, and hostnames alike
  5. Last resort for WSD printers Windows never cached an address for: the WSD port's registry-stored device UUID is used to look up the linked `SWD\IPP\<uuid>` PnP device node, whose cached IPP/AirPrint properties hold the live address
- **Live column** — a real reachability check, not just WMI's last-known status. Probes the standard print ports (9100 RAW/JetDirect, 631 IPP, 515 LPR) before falling back to ICMP ping, since many printers and their firewalls block ping while still accepting print jobs. Shown in green (online) / red (offline); resolved in the background so the list appears instantly instead of blocking on network I/O
- **Wake / Retry** — for a printer showing offline, retries the connectivity check with a longer timeout and repeated attempts. A plain connection attempt is often enough to nudge a sleeping printer's network stack awake; true Wake-on-LAN isn't implemented, since most printers' print engines don't actually listen for a magic packet even when their NIC supports it at the hardware level
- **Check Ink / Toner Levels** — queries supply levels over SNMP (Printer-MIB / RFC 3805) for any printer with a resolved network address, using a minimal self-contained SNMPv1 client (no external library, no vendor tooling required). Local/USB printers have no OS-level supply API, so this only works for network-reachable devices that answer SNMP (community `public` by default)

### Multi-window workflow
Tool windows (Restore Points, Snapshot Operations, Registry Backup, Printers) open modeless, so the main window and several tool windows can be used simultaneously. Each launcher button disables while its window is open — a built-in "already open" indicator — and re-enables when the window closes.

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
- Outbound UDP 161 (SNMP) reachable to a printer for the ink/toner supply-level check; outbound TCP 9100/631/515 (or ICMP) for the printer online/offline check

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
- Printer supply levels are read with a hand-rolled SNMPv1 client (`Printer_Support`'s internal `Snmp` class) that does its own BER encode/decode of GET-NEXT requests over UDP — enough to walk the standard Printer-MIB supplies table without pulling in a third-party SNMP library.
- Printer online/offline is a real network probe (`TcpClient.BeginConnect` against ports 9100/631/515, falling back to `Ping`), not a read of WMI's cached `PrinterStatus`/`WorkOffline`, which only reflects Windows' last-known state.
- When no WMI or registry source has a printer's address, the WSD port's device UUID (`HKLM\SYSTEM\CurrentControlSet\Control\Print\Monitors\WSD Port\Ports\<PortName>`) is used to query the linked `SWD\IPP\<uuid>` PnP device node via `Get-PnpDeviceProperty`, since that property store has no WMI or plain-registry equivalent.

## A note on restore points as a safety net

Restore points revert system files, the registry, drivers, and installed programs to a moment in time; personal documents are untouched. They're an excellent *undo* for risky installs and settings changes — checkpoint before each change, roll back to just before the one that went wrong. They are **not** a security boundary: persistent malware can survive a rollback. For genuinely untrusted software, use a VM or sandbox instead.

# The Restore Point Creation Throttle — and What the Toolkit Does About It

This document explains the Windows restore point *creation throttle*, why it makes
"Create Restore Point" buttons silently fail, and exactly what the Admin Toolkit
does to the setting when you create a point — including the guarantee that the
system is left byte-for-byte as it was found.

## The problem: Windows silently skips restore points

By default, Windows will only create **one restore point per 24 hours**. If
anything — an installer, a script, or this Toolkit — asks for a new point while
one younger than 24 hours exists, Windows *skips the creation entirely* but
**still returns success** to the caller. No error, no warning, no new point.

The design intent was reasonable: in the XP/Vista era, installers created so many
restore points that the useful older ones were evicted from the fixed-size
storage within hours. Throttling to one per day preserved history. But for a
deliberate, manual snapshotting workflow — checkpoint, install, checkpoint,
install — it is exactly wrong.

### The registry knob

The throttle window is controlled by one registry value:

```
Key   : HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore
Value : SystemRestorePointCreationFrequency   (REG_DWORD)
```

| State | Meaning |
|---|---|
| **Value absent** (the default) | Windows uses its built-in 24-hour (1440-minute) window |
| **N** (any positive number) | Minimum N minutes between honored creation requests |
| **0** | Throttle disabled — every creation request is honored |

Note that *absent* and *1440* behave identically but are different registry
states. That distinction matters below.

## What the Toolkit does: a per-click toggle

The **Create Restore Point** button does not permanently change this setting.
Instead, each click performs a scoped toggle around the single creation call:

1. **Read** the current throttle. If it is already `0`, nothing is touched —
   steps 2–4 are skipped entirely.
2. **Save** the exact current state of the registry value — including the
   distinction between "absent" and "present with some number."
3. **Set** the value to `0`, lifting the throttle.
4. **Create** the restore point (on a background thread; the UI stays live).
5. **Restore** the saved state — in a `finally` block, so it runs even if the
   creation call throws or fails:
   - If the value was **absent** before, it is **deleted** again (not rewritten
     as 1440 — the registry is returned to its literal prior state).
   - If the value held a number, that exact number is written back.

The throttle is therefore off for only the few seconds the creation takes.
Before and after, the system setting is untouched — `reg query` shows exactly
what it showed before the click.

### Why not a persistent on/off button?

An earlier design used a toggle button ("Force mode on / off") that left the
throttle disabled until turned back off. It worked, but carried a failure mode:
forget to toggle back, and the machine is left permanently unthrottled — a
silent system-wide change surviving the app. The per-click toggle has no state
to forget: the override lives and dies inside one button press. If you want
the throttle permanently disabled anyway, set the registry value to `0`
yourself; the Toolkit will detect that and skip its toggle logic.

## The companion fixes (same button, different landmines)

The throttle is only one of several ways Windows silently declines to create a
restore point while reporting success. The same Create flow also handles:

**The BEGIN/END system change window.** Creation requests are sent with
`EventType = BEGIN_SYSTEM_CHANGE (100)`. Windows treats that as "a system
change is in progress" and suppresses all further creation requests *from the
same process* until a matching `END_SYSTEM_CHANGE (101)` arrives or the
process exits. Without the END, only the first create per app session works.
The Toolkit sends the END immediately after each successful create, so
back-to-back creations in one session all succeed.

**System Protection disabled.** If protection is off for the system drive, the
creation call is a no-op. The Toolkit detects this before creating and offers
to enable protection on the spot.

**Enumeration lag.** A newly created point can take a few seconds to appear in
the WMI `SystemRestore` enumeration. After a successful create, the Toolkit
polls (up to ~15 seconds) until the new sequence number is visible, then
refreshes the list — so the point appears on its own, no manual refresh needed.

## Verifying the behavior

Check the throttle value at any time from an elevated prompt:

```
reg query "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore" /v SystemRestorePointCreationFrequency
```

- Before a create, during normal use, and after a create, this shows your
  ambient setting (typically "unable to find the specified registry value" —
  i.e., absent, the Windows default).
- Only during the brief creation window would it read `0x0`.

Windows also logs every creation attempt — honored or skipped — in
**Event Viewer → Application log, source "System Restore"**, which is the
authoritative record of what Windows actually did.

## Edge cases and honest caveats

- **Hard kill mid-create.** If the process is force-terminated in the seconds
  between lifting and restoring the throttle (task manager kill, power loss),
  the value can be left at `0`. The window is a few seconds per click, so this
  is unlikely — but if it happens, the fix is one command:
  `reg delete "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore" /v SystemRestorePointCreationFrequency /f`
  (restores the Windows default). A normal app crash does not cause this — the
  restore runs in a `finally` block and survives exceptions.
- **Another process racing the window.** While the throttle is briefly `0`,
  an unrelated installer could also get a point created. Harmless — arguably a
  bonus.
- **Registry write requires elevation.** The Toolkit already requires
  Administrator for all VSS/restore features, so this is not an additional
  requirement. If the write fails anyway, the Toolkit warns that Windows may
  skip the point rather than failing silently.
- **The throttle is per-machine, not per-user.** Lifting it affects all
  creation requests system-wide during the window, not just the Toolkit's.

## Summary

| Moment | `SystemRestorePointCreationFrequency` |
|---|---|
| Before clicking Create | Whatever it was (usually absent = 24 h default) |
| During creation (~seconds) | `0` |
| After creation (success *or* failure) | Exactly what it was before — same value, or absent again |

The net effect: **every click of Create Restore Point produces a restore
point, and no click leaves any trace in the system configuration.**


## Roadmap

- [ ] Finish the email log feature (SMTP send + settings UI) — scaffolding already in place, contributions welcome
- [ ] Add screenshots to this README
- [ ] Per-drive shadow storage breakdown / resize (`vssadmin resize shadowstorage`)
- [ ] Optional registry-hive diff between a snapshot and the live system (pairs with the Registry Backup window)

## License

Opensource
