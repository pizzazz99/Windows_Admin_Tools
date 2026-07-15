using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Admin_Tools
{
    /// <summary>
    /// Read-only, styled help panel describing the app and when to use each
    /// function. Section headers are detected by the dashed rule beneath them,
    /// so if you edit HelpText keep the ---- underline under any new heading.
    /// </summary>
    public class HelpDialog : Form
    {
        public HelpDialog()
        {
            Text = "Admin Toolkit — Help & Reference";
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(700, 640);
            MinimumSize = new Size(520, 400);

            var box = new RichTextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                BorderStyle = BorderStyle.None,
                Font = new Font("Consolas", 9.5f),
                WordWrap = false,          // text is pre-wrapped; keeps offsets stable
                DetectUrls = false,
                Text = HelpText
            };
            ApplyStyles(box);
            box.Select(0, 0);

            var host = new Panel { Dock = DockStyle.Fill, Padding = new Padding(30, 14, 8, 0) };
            host.Controls.Add(box);

            var close = new Button { Text = "Close", DialogResult = DialogResult.OK, Width = 90, Height = 30 };
            var bottom = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                FlowDirection = FlowDirection.RightToLeft,
                Height = 48,
                Padding = new Padding(10)
            };
            bottom.Controls.Add(close);

            Controls.Add(host);
            Controls.Add(bottom);
            AcceptButton = close;
            CancelButton = close;
        }

        private static void ApplyStyles(RichTextBox box)
        {
            Color accent = Color.FromArgb(0, 90, 158);     // section headers
            Color separator = Color.FromArgb(180, 180, 180);  // ==== / ---- rules
            Color warn = Color.FromArgb(176, 0, 32);      // caveat block
            Color subhead = Color.FromArgb(70, 70, 70);      // inline sub-labels

            Font baseFont = box.Font;
            Font boldFont = new Font(baseFont, FontStyle.Bold);
            Font titleFont = new Font(baseFont.FontFamily, baseFont.Size + 4f, FontStyle.Bold);

            string[] subheadLabels =
            {
                "WHEN TO USE", "MULTI-DRIVE NOTE", "SYSTEM-DRIVE RULE",
                "RECOVERY NOTE", "INSTALL WORKFLOW", "THE CATCH"
            };

            string[] lines = box.Lines;
            bool inCaveats = false;

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                int start = box.GetFirstCharIndexFromLine(i);
                if (start < 0) continue;

                string trimmed = line.Trim();
                bool isRule = trimmed.Length > 0 &&
                              trimmed.All(c => c == '-' || c == '=');
                bool nextIsRule = i + 1 < lines.Length &&
                                  lines[i + 1].Trim().Length > 0 &&
                                  lines[i + 1].Trim().All(c => c == '-' || c == '=');

                if (trimmed.StartsWith("IMPORTANT CAVEATS")) inCaveats = true;
                else if (nextIsRule && i != 0) inCaveats = false;

                box.Select(start, line.Length);

                if (i == 0)                                   // main title
                {
                    box.SelectionFont = titleFont;
                    box.SelectionColor = accent;
                }
                else if (isRule)                              // separator lines
                {
                    box.SelectionColor = separator;
                }
                else if (nextIsRule)                          // section headers
                {
                    box.SelectionFont = boldFont;
                    box.SelectionColor = accent;
                }
                else if (inCaveats && trimmed.Length > 0)     // whole caveat block
                {
                    box.SelectionColor = warn;
                }
                else if (subheadLabels.Any(s => trimmed.StartsWith(s)))
                {
                    box.SelectionFont = boldFont;
                    box.SelectionColor = subhead;
                }
            }

            box.Select(0, 0);
            box.SelectionColor = box.ForeColor;   // reset caret color
        }

        private const string HelpText =
@"ADMIN TOOLKIT — HELP & REFERENCE
================================================================

OVERVIEW
----------------------------------------------------------------
Admin Toolkit is a single-window utility for two jobs:

  1. Managing Volume Shadow Copies — the technology behind the
     Windows ""Previous Versions"" feature, which lets you restore
     earlier versions of files without a full backup program —
     plus the System Restore points built on top of it.

  2. Launching the built-in Windows administrative tools from one
     place, and tracking the ones you launch.

The app runs elevated (it requests administrator rights at
startup). It needs this because querying VSS, creating snapshots
and restore points, and enabling System Protection all require
admin.

Tool windows (Restore Points, Snapshot Operations, Registry
Backups) open alongside the main window, so several can be used
at once. Each launcher button disables while its window is open
— a built-in ""already open"" indicator — and re-enables when the
window closes.


SHADOWING vs SNAPSHOTS — WHAT THE TERMS MEAN
----------------------------------------------------------------
  SHADOWING is the capability. Turning it on means enabling
  System Protection for a drive (which allocates disk space for
  copies) and scheduling something to fire regularly.

  A SNAPSHOT (a shadow copy) is one frozen point in time that the
  capability produces. You enable shadowing once; it then creates
  many snapshots over time.

  A RESTORE POINT is a named, system-level snapshot: a shadow
  copy plus metadata (description, type, sequence number) that
  the System Restore wizard can roll the whole machine back to.
  Every restore point contains a complete copy of the registry
  and system files as of its timestamp.


SHADOW COPIES PANEL
----------------------------------------------------------------
Lists every snapshot on the machine with its created date, age,
type, and ID, plus a live storage readout (how much of the
allowed space is in use).

  Refresh
    Re-reads the snapshot list. Use after creating one, or to
    confirm a scheduled run produced a new copy.

  Create Snapshot Now
    Makes ONE snapshot of C: on demand — the manual equivalent of
    one scheduled run. Use before risky changes (installs, edits,
    registry work) so you have a fresh restore point.

  VSS Details
    Shows raw ""vssadmin list shadows"" output — filtered to the
    SELECTED snapshot (via /shadow={ID}) so it can be compared
    side by side with the details window, or all snapshots when
    nothing is selected.


RESTORE POINTS (button)
----------------------------------------------------------------
A full manager for System Restore points, going beyond what the
built-in System Protection dialog shows. Every point is listed
with its sequence number, creation time, age, type, event,
description, and whether its underlying shadow copy still exists.

A status line shows System Protection state, the creation
throttle setting, and shadow storage usage at a glance.

Selecting a point fills the details pane: attribution codes with
plain-English explanations, the linked shadow copy's ID and
device path (matched by timestamp), and two computed extras:

  * Gap detection — Windows never reuses sequence numbers, so
    missing numbers between listed points are flagged as deleted
    or aged-out points.

  * ""What would restoring remove?"" — programs whose registry
    install date falls on/after the point's date are listed, i.e.
    what a rollback to that point would uninstall. Best-effort
    (install dates are day-granular and some installers omit
    them), but it answers the question that matters.

  Create Restore Point
    Prompts for a description, then guarantees creation. Windows
    normally SKIPS creating a point — while still reporting
    success — if one younger than 24 hours exists. The app lifts
    that throttle for the duration of the single creation call
    and restores the previous setting afterward, byte-for-byte,
    even on failure. If System Protection is off, it offers to
    enable it first. The list refreshes itself when the new
    point becomes visible (creation can take a few seconds).

  Delete Selected
    Supports multi-select (Ctrl+click / Shift+click / Ctrl+A).
    The confirmation lists exactly which points will go. Deleting
    a point also deletes its underlying shadow copy. No undo.

  Browse Files at This Point
    Mounts the selected point's snapshot as a read-only folder
    (a temporary symlink in %TEMP%) and opens it in Explorer —
    the entire drive exactly as it was at that moment. Grab a
    config file as it existed before an install, or compare old
    vs. current. Links are removed automatically when the window
    closes; the snapshot itself is never modified.

  Refresh / Copy Details / Close
    Refresh re-queries everything; Copy Details puts the full
    detail text (including status and notes) on the clipboard.

  INSTALL WORKFLOW: installing several apps in one session?
  Create a point BEFORE each install (""Before VendorApp"" etc.).
  If one app turns out bad, restore to the point just before it —
  you land on a machine with only the good ones. Points mark the
  state BEFORE a change, so the baseline before the first install
  is the most important one.

  RECOVERY NOTE: rolling back to a point is done with the System
  Restore wizard (launchable from the Admin Tools panel). It
  reverts system files, the registry, drivers, and installed
  programs to that moment; personal documents are untouched.


SETUP SHADOWING (button)
----------------------------------------------------------------
The one-time setup that turns the whole capability on. When
clicked it:

  1. Checks whether shadowing is already active on each drive.
  2. Shows a dialog listing your fixed NTFS drives with a check
     box beside each — pick which to protect.
  3. Enables System Protection on the selected drives (the system
     drive is always included — Windows requires it).
  4. Creates a daily scheduled task, AutoShadowCopy, that snapshots
     the selected drives.
  5. Removes the 24-hour throttle so every scheduled run counts.
  6. Runs the task once immediately so a first snapshot exists.

  WHEN TO USE: once per PC, during setup. If shadowing is already
  active on the chosen drives, it reports that and does nothing.

  MULTI-DRIVE NOTE: shadow storage is allocated per drive and
  comes out of each drive's own space. Protecting a large data
  drive can reserve a lot more room than the system drive does.


TURN OFF SHADOWING (button)
----------------------------------------------------------------
The teardown. Lists the currently-protected drives and lets you
check which to turn off. For each drive turned off it deletes that
drive's snapshots and disables System Protection, then:

  * If any drives remain protected, the AutoShadowCopy task is
    rebuilt to snapshot only those survivors.
  * If nothing remains protected, the task is removed entirely and
    the throttle override is undone.

  SYSTEM-DRIVE RULE: Windows won't keep a data drive protected
  while the system drive is unprotected. So if you turn off the
  system drive while others are still on, the app cascades — it
  turns them all off too — and warns you before doing it.

  WHEN TO USE: rarely. Good reasons are reclaiming space on a full
  drive that's backed up elsewhere, a scratch/temp drive that
  doesn't need history, or resetting VSS while troubleshooting.
  Turning it off to ""save space"" with no backup trades recovery
  for a few GB — think twice.


ADMIN TOOLS PANEL
----------------------------------------------------------------
One-click launchers for standard Windows consoles. Each opens the
real Microsoft tool — nothing is reimplemented here.

  Task Scheduler ....... View/edit scheduled tasks, incl.
                         AutoShadowCopy. Right-click a task > Run
                         to fire it now; check Last Run Result
                         (0x0 = success).
  System Protection .... The dialog for turning protection on/off
                         and setting each drive's space slider.
  System Restore ....... The wizard for rolling the whole system
                         back to an earlier restore point.
  Registry Editor ...... Direct registry editing. Use with care.
  Event Viewer ......... System/Application logs — first stop when
                         diagnosing crashes, restarts, or VSS
                         errors. Restore point activity is logged
                         under Application, source ""System
                         Restore"".
  Services ............. Start/stop/configure Windows services
                         (e.g. Volume Shadow Copy service).
  Disk Management ...... Partitions, drive letters, volume status.
  Computer Management .. Umbrella console (Event Viewer, Services,
                         Disk Management, and more in one window).
  System Info .......... Full hardware/OS inventory (msinfo32).


LAUNCHED TOOLS PANEL
----------------------------------------------------------------
Every tool started from this app is tracked here with its PID,
start time, and live status (Running / Exited).

  Close Selected .. Politely asks the tool's window to close.
  End Task ........ Force-kills the process if it won't close.
  Clear Closed .... Removes exited entries to tidy the list.


COMMAND OUTPUT / LOG
----------------------------------------------------------------
Captured output and a timestamped activity log. Setup and teardown
steps are logged line-by-line under the [Shadow] category, each
with its exit code, so you can see exactly what succeeded or failed.



REGISTRY BACKUPS (button)
----------------------------------------------------------------
Answers one question: ""is my registry backed up?"" — by checking
every place a registry backup can exist, then giving a verdict.

  RegBack Folder
    Windows has a built-in mechanism that periodically copies the
    registry hives (DEFAULT, SAM, SECURITY, SOFTWARE, SYSTEM) to
    C:\Windows\System32\config\RegBack. The panel lists each hive
    file with its size and date. THE CATCH: since Windows 10
    v1803 this is disabled by default — the files exist but are
    0 bytes (shown grayed). Real sizes mean it's working.

  Backup setting
    The EnablePeriodicBackup registry value that controls the
    mechanism above. ""Not set"" = the Windows default = disabled.

  RegIdleBackup Scheduled Task
    The task that actually fills the RegBack folder. Shows its
    last run time, last result (0x0 = success), state, and next
    run. It fires during automatic maintenance, so ""Never"" or an
    old date is normal on a machine where the setting is off.

  Snapshot-based Backups
    The safety net you already have: every restore point and every
    shadow copy contains a complete copy of the registry hives as
    of its timestamp. This panel counts both and shows the newest
    of each. On a machine with shadowing set up, THIS is the real
    registry backup story — even with RegBack disabled.

  Verdict
    Plain-English summary combining all of the above.

  Enable RegBack ... Sets EnablePeriodicBackup = 1 so Windows
                     resumes filling RegBack during maintenance.
                     Costs roughly 100-300 MB of disk space.
  Backup Now ....... Runs the RegIdleBackup task immediately.
                     Writes nothing unless the setting is enabled
                     first — enable, run, then Refresh to watch
                     the hive files jump from 0 bytes to real
                     sizes.
  Open Folder ...... Opens RegBack in Explorer to eyeball it.
  Refresh .......... Re-checks everything.

  WHEN TO USE: before registry-heavy work (manual edits, cleanup
  tools, risky installs) to confirm a recovery path exists — or
  once, out of curiosity, to learn what your machine's registry
  safety net actually is. Creating a snapshot from the Shadow
  Copies panel is itself a registry backup, so ""Create Snapshot
  Now"" + this panel's verdict covers you.

  RECOVERY NOTE: restoring a hive from RegBack or from a snapshot
  is an advanced, offline operation (recovery environment or
  manual copy). For normal rollbacks, System Restore is the
  supported path — it restores the registry as part of the
  restore point.

IMPORTANT CAVEATS
----------------------------------------------------------------
  * Snapshots are NOT a backup. They live on the same drive, so
    they protect against bad edits and accidental deletion, but
    NOT against drive failure. Keep a real backup elsewhere too.

  * Turning shadowing OFF for a drive permanently deletes all of
    that drive's existing snapshots. There is no undo.

  * Deleting a restore point also deletes its underlying shadow
    copy — and the file history that snapshot held. No undo.

  * Restoring to a restore point removes programs installed after
    it (see the ""would remove"" list in the details pane before
    you roll back). Personal documents are untouched.

  * A restore point is an UNDO for system changes, not a security
    boundary. Persistent malware can survive a rollback. For
    genuinely untrusted software, use a VM or sandbox instead.

  * When storage fills, Windows silently deletes the oldest
    snapshots to make room. A shorter history is normal, not an
    error.

  * Enabling protection on any non-system drive forces protection
    on the system drive as well — a rule of the Windows cmdlet,
    not this app.

  * RegBack backups live on the same drive as Windows itself, so
    like snapshots they protect against corruption and bad edits,
    NOT against drive failure.
";
    }
}