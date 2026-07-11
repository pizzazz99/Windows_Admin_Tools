ADMIN TOOLKIT - C# Windows Forms project
=========================================

WHAT IT IS
A single-window admin console for your PCs:

  * Shadow Copies panel - lists every VSS snapshot (created date, age,
    type, ID) with live storage usage. Buttons to Refresh, Create a
    Snapshot Now, and dump raw "vssadmin list shadows" output.
  * Admin Tools panel - one-click launch of Task Scheduler, System
    Protection, System Restore wizard, Registry Editor, Event Viewer,
    Services, Disk Management, and Computer Management.
  * Launched Tools panel - every tool started from this app is tracked
    with its PID, start time, and live status. Close Selected sends a
    polite window-close; End Task force-kills; Clear Closed tidies the
    list. Status updates automatically when a tool exits.
  * Command Output panel - shows captured console output (vssadmin).

The app requests administrator elevation automatically at startup
(app.manifest), which VSS queries and snapshot creation require.

HOW TO OPEN
1. Install Visual Studio 2022 Community (free) with the
   ".NET desktop development" workload.
2. Double-click AdminToolkit.sln.
3. Open MainForm.cs in the designer (Shift+F7) - every control can be
   moved, resized, and restyled visually.
4. Press F5 to build and run. Approve the UAC prompt.

TARGET FRAMEWORK
.NET Framework 4.8 - already built into Windows 10 (1903+) and all of
Windows 11, so the compiled EXE needs NO runtime install on your PCs.

DEPLOYING TO YOUR 7 PCs
1. Switch the toolbar dropdown from Debug to Release, then
   Build > Build Solution.
2. Copy bin\Release\AdminToolkit.exe to each PC (USB stick or network
   share - avoids the browser "blocked file" tag).
3. That single EXE is the whole app. Run it, approve UAC, done.

EXTENDING IT
To add another tool button: drop a Button on the form in the designer,
double-click it, and add one line in the handler, e.g.

    LaunchTool("Performance Monitor", "mmc.exe", Sys32("perfmon.msc"));

Any .msc snap-in, .exe, or control panel applet works the same way.
