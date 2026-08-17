
using Microsoft.VisualBasic.Devices;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using Trace_Execution_Namespace;
using static Trace_Execution_Namespace.Trace_Execution;

namespace Admin_Tools
{
  public partial class MainForm : Form
  {
    // Tracks every tool this program has launched
    private readonly List<LaunchedTool> _Launched = new();

    // The single detached output window (created on demand, reused)

    private bool                        _Is_Elevated;
    private OutputForm                  _LogForm;

    // Proportional resize: original bounds/font sizes captured once at Load,
    // then every control is rescaled by the same factor the window is resized by.
    private readonly Dictionary<Control, Rectangle> _BaseBounds   = new();
    private readonly Dictionary<Control, float> _BaseFontSize     = new();
    private readonly Dictionary<ColumnHeader, int> _BaseColWidths = new();
    private Size                                   _BaseClientSize;
    private bool                                   _ScalingInProgress;

    // Button state colors: green = ready, amber = tool running
    private static readonly System.Drawing.Color _ReadyColor =
      System.Drawing.Color.FromArgb ( 198, 239, 206 );
    private static readonly System.Drawing.Color _RunningColor =
      System.Drawing.Color.FromArgb ( 255, 235, 156 );

    private Restore_Point_List_Form _RestorePointsForm;
    private Registry_Backup_Form    _RegistryBackupForm;
    private Shadow_Copy_Form        _ShadowCopyForm;
    private Printer_Form            _PrinterForm;
    private static readonly Color _OpenColor = Color.FromArgb ( 76, 175, 80 ); // green = tool open

    public bool                   Handoff_Pending;
    public partial class NativeMethods
    {
      [ LibraryImport ( "user32.dll", StringMarshalling = StringMarshalling.Utf16 ) ]
      internal static partial IntPtr FindWindow ( string? Class_Name, string Window_Title );
    }

    [ DllImport ( "user32.dll" ) ]
    private static extern uint GetWindowThreadProcessId ( IntPtr HWnd, out uint Pid );

    // --- P/Invoke, anywhere in the form class ---
    [ DllImport ( "user32.dll" ) ]
    private static extern bool EnumWindows ( EnumWindowsProc Callback, IntPtr LParam );
    private delegate bool      EnumWindowsProc ( IntPtr HWnd, IntPtr LParam );

    [ DllImport ( "user32.dll" ) ]
    private static extern bool IsWindowVisible ( IntPtr HWnd );

    [ DllImport ( "user32.dll" ) ]
    private static extern bool PostMessage ( IntPtr HWnd, uint Msg, IntPtr WParam, IntPtr LParam );

    private const uint         WM_CLOSE = 0x0010;

    private class LaunchedTool
    {
      public string       Name;
      public Process      Process;
      public ListViewItem Item;
      public Button       Button;
      public bool         Handoff_Pending; // true while Track_Handoff rebinds a stub launcher
    }

    public MainForm ()
    {

      InitializeComponent ();
      Trace_Execution.Initialize ( Start_Enabled: true );
      Update_Trace_Button_State ( IsRunning );
      string AppTitle =
        Assembly.GetExecutingAssembly ().GetCustomAttribute<AssemblyTitleAttribute> ()?.Title ??
        "Admin Toolkit";

      this.Text                      = $"{AppTitle} Running on => {Environment.MachineName}";

      // Hide the email buttons until implemented)
      Email_Settings_Button.Enabled  = false;
      Email_Settings_Button.Visible  = false;
      Email_Log_Button.Enabled       = false;
      Email_Log_Button.Visible       = false;

      this.Resize                   += MainForm_Resize;
      listViewProcesses.SelectedIndexChanged += ( S, Args ) => Update_Launched_Tool_Buttons ();
    }

    private void MainForm_Load ( object Sender, EventArgs E )
    {
      using var Block = Trace_Block.Start_If_Enabled ();
      // All launcher buttons start green (ready)
      foreach ( Control C in grpTools.Controls )
      {
        var B = C as Button;
        if ( B != null )
        {
          B.UseVisualStyleBackColor = false;
          B.BackColor               = _ReadyColor;
        }
      }

      _Is_Elevated = IsRunningAsAdmin ();
      Logger.BlankLine ();
      Logger.Log ( "App",
                   "Started on " + Environment.MachineName + "  (elevated=" + _Is_Elevated + ")" );
      if ( ! _Is_Elevated )
      {
        ShowElevationBanner ();
        statusLabel.Text = "WARNING: not elevated - admin features will fail.";
      }

      // Lock the smallest allowed size to the as-loaded layout (banner included,
      // if shown) and remember every control's starting bounds/font for scaling.
      this.MinimumSize = this.Size;
      CaptureScaleBaseline ();

      // Nothing launched yet — nothing for these to act on.
      Update_Launched_Tool_Buttons ();
    }

    /// <summary>Close Selected / End Task act on the selected row, so
    /// they stay disabled until something is selected. Clear Closed
    /// acts on every closed entry regardless of selection, so it only
    /// needs the list to be non-empty.</summary>
    private void Update_Launched_Tool_Buttons ()
    {
      bool HasSelection       = listViewProcesses.SelectedItems.Count > 0;
      BtnCloseTool.Enabled    = HasSelection;
      BtnKillTool.Enabled     = HasSelection;
      BtnRemoveClosed.Enabled = listViewProcesses.Items.Count > 0;
    }

    // ====================================================================
    //  PROPORTIONAL RESIZE / SCALING
    // ====================================================================

    private void CaptureScaleBaseline ()
    {
      using var Block = Trace_Block.Start_If_Enabled ();
      _BaseClientSize = ClientSize;
      CaptureControlBounds ( this );
    }

    private void CaptureControlBounds ( Control Parent )
    {
      using var Block = Trace_Block.Start_If_Enabled ();
      foreach ( Control C in Parent.Controls )
      {
        _BaseBounds[ C ]   = C.Bounds;
        _BaseFontSize[ C ] = C.Font.Size;

        if ( C is ListView Lv )
        {
          foreach ( ColumnHeader Col in Lv.Columns )
            _BaseColWidths[ Col ] = Col.Width;
        }

        if ( C.Controls.Count > 0 )
          CaptureControlBounds ( C );
      }
    }

    private void MainForm_Resize ( object Sender, EventArgs E )
    {
      if ( _ScalingInProgress )
        return;
      if ( WindowState == FormWindowState.Minimized )
        return;
      if ( _BaseClientSize.Width == 0 || _BaseClientSize.Height == 0 )
        return;

      _ScalingInProgress = true;
      try
      {
        float ScaleX = (float) ClientSize.Width / _BaseClientSize.Width;
        float ScaleY = (float) ClientSize.Height / _BaseClientSize.Height;
        ScaleControlsRecursive ( this, ScaleX, ScaleY );
      }
      finally
      {
        _ScalingInProgress = false;
      }
    }

    private void ScaleControlsRecursive ( Control Parent, float ScaleX, float ScaleY )
    {
      using var Block      = Trace_Block.Start_If_Enabled ();
      float     Font_Scale = Math.Min ( ScaleX, ScaleY );

      foreach ( Control C in Parent.Controls )
      {
        // Docked controls (the status strip, the elevation banner) are left to
        // the docking engine - only freely-positioned controls get new bounds.
        if ( C.Dock == DockStyle.None && _BaseBounds.TryGetValue ( C, out Rectangle B ) )
        {
          C.Bounds = new Rectangle ( x: (int) Math.Round ( B.X * ScaleX ),
                                     y: (int) Math.Round ( B.Y * ScaleY ),
                                     width: (int) Math.Round ( B.Width * ScaleX ),
                                     height: (int) Math.Round ( B.Height * ScaleY ) );
        }

        if ( _BaseFontSize.TryGetValue ( C, out float BaseSize ) )
        {
          float NewSize = Math.Max ( 6f, BaseSize * Font_Scale );
          if ( Math.Abs ( C.Font.Size - NewSize ) > 0.25f )
            C.Font = new Font ( C.Font.FontFamily, NewSize, C.Font.Style );
        }

        if ( C is ListView Lv )
        {
          foreach ( ColumnHeader Col in Lv.Columns )
            if ( _BaseColWidths.TryGetValue ( Col, out int W ) )
              Col.Width = (int) Math.Round ( W * ScaleX );
        }

        if ( C.Controls.Count > 0 )
          ScaleControlsRecursive ( C, ScaleX, ScaleY );
      }
    }

    // ====================================================================
    //  SNAPSHOTS / RESTORE POINTS
    // ====================================================================

    // drive is like "C:\\"

    /// <summary>Stub launchers (resmon -> perfmon) exit immediately while
    /// the real UI runs in another process. Wait for the named window to
    /// appear, then rebind this tool's tracking to the process that owns
    /// it.</summary>
    private async void Track_Handoff ( LaunchedTool Tool, string WindowTitle )
    {
      using var Block = Trace_Block.Start_If_Enabled ();
      for ( int I = 0; I < 25; I++ ) // up to ~5 seconds
      {
        IntPtr HWnd = NativeMethods.FindWindow ( null, WindowTitle );
        if ( HWnd != IntPtr.Zero )
        {
          GetWindowThreadProcessId ( HWnd, out uint Pid );
          try
          {
            var Real                      = Process.GetProcessById ( (int) Pid );
            Real.EnableRaisingEvents      = true;

            Tool.Process                  = Real;
            Tool.Item.SubItems[ 1 ].Text  = Real.Id.ToString ();
            Tool.Handoff_Pending          = false;

            Real.Exited                  += ( S, Args ) =>
            {
              try
              {
                BeginInvoke ( () =>
                              {
                                statusLabel.Text = Tool.Name + " closed.";
                                Remove_Launched_Tool ( Tool );
                              } );
              }
              catch
              { /* form closing */
              }
            };
            return;
          }
          catch
          { /* window vanished between calls; keep polling */
          }
        }
        await Task.Delay ( 200 );
      }

      // Window never appeared — give up and remove the row
      Tool.Handoff_Pending = false;
      try
      {
        BeginInvoke ( () => Remove_Launched_Tool ( Tool ) );
      }
      catch
      {
      }
    }

    /// <summary>Posts WM_CLOSE to every visible top-level window owned by
    /// the process — the manual equivalent of clicking its X button.</summary>
    private static bool Close_Tool_Windows ( int Pid )
    {
      using var Block  = Trace_Block.Start_If_Enabled ();
      bool      Posted = false;
      EnumWindows (
        ( HWnd, LParam ) =>
        {
          GetWindowThreadProcessId ( HWnd, out uint WinPid );
          if ( WinPid == (uint) Pid && IsWindowVisible ( HWnd ) )
          {
            PostMessage ( HWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero );
            Posted = true;
          }
          return true; // keep enumerating
        },
        IntPtr.Zero );
      return Posted;
    }

    /// <summary>True if this process is running with Administrator
    /// rights.</summary>
    private static bool IsRunningAsAdmin ()
    {
      using var Block = Trace_Block.Start_If_Enabled ();
      try
      {
        using ( WindowsIdentity Id = WindowsIdentity.GetCurrent () )
        {
          return new WindowsPrincipal ( Id ).IsInRole ( WindowsBuiltInRole.Administrator );
        }
      }
      catch
      {
        return false;
      }
    }

    /// <summary>
    /// Drop a red warning strip across the top when we're NOT elevated.
    /// Pushes the existing groups down and grows the form so nothing overlaps -
    /// no Designer changes needed.
    /// </summary>
    private void ShowElevationBanner ()
    {
      using var Block  = Trace_Block.Start_If_Enabled ();
      const int H      = 30;

      var       Banner = new Panel { Dock = DockStyle.Top, Height = H,
                                     BackColor = System.Drawing.Color.FromArgb ( 192, 0, 0 ) };
      Banner.Controls.Add (
        new Label { Dock = DockStyle.Fill, ForeColor = System.Drawing.Color.White,
                    Font      = new System.Drawing.Font ( "Segoe UI", 9.75F,
                                                          System.Drawing.FontStyle.Bold ),
                    TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                    Text      = "NOT RUNNING AS ADMINISTRATOR  -  snapshot / VSS " +
                                "actions will fail. " +
                                "Close and restart with 'Run as administrator'." } );

      // Move the absolutely-positioned groups down and make room at the top.
      foreach ( Control C in Controls )
      {
        if ( C is StatusStrip )
          continue; // stays docked at the bottom
        C.Top += H;
      }
      Height += H;

      Controls.Add ( Banner );
      Banner.BringToFront ();
    }

    // ====================================================================
    //  ADMIN TOOL LAUNCHER
    // ====================================================================

    private void LaunchTool ( string Display_Name, string File_Name, string Arguments,
                              Button Launcher, string Window_Title = null )
    {
      using var Block = Trace_Block.Start_If_Enabled ();
      try
      {
        var     Process_Start_Info = new ProcessStartInfo ( File_Name,
                                                            Arguments ) { UseShellExecute = true };
        Process P                  = Process.Start ( Process_Start_Info );
        if ( P == null )
        {
          statusLabel.Text = Display_Name + " launched (no process handle).";
          return;
        }

        // Mark the launcher as "open" — stays enabled so a second
        // click can close the tool (see ToggleTool)
        if ( Launcher != null )
        {
          Launcher.BackColor               = _OpenColor;
          Launcher.UseVisualStyleBackColor = false;
        }

        P.EnableRaisingEvents = true;

        var Item              = new ListViewItem ( Display_Name );
        Item.SubItems.Add ( P.Id.ToString () );
        Item.SubItems.Add ( DateTime.Now.ToString ( "HH:mm:ss" ) );
        Item.SubItems.Add ( "Running" );
        listViewProcesses.Items.Add ( Item );
        Update_Launched_Tool_Buttons ();

        var Tool = new LaunchedTool { Name = Display_Name, Process = P, Item = Item,
                                      Button = Launcher };
        _Launched.Add ( Tool );
        Item.Tag = Tool;

        if ( Window_Title == null )
        {
          // Normal tool: the launched process IS the tool — track
          // its exit directly.
          P.Exited += ( S, Args ) =>
          {
            try
            {
              BeginInvoke ( () =>
                            {
                              statusLabel.Text = Tool.Name + " closed.";
                              Remove_Launched_Tool ( Tool );
                            } );
            }
            catch
            { /* form closing */
            }
          };
        }
        else
        {
          // Stub launcher (e.g. resmon -> perfmon): the launched
          // process exits immediately while the real UI runs in
          // another process. Ignore the stub's exit and rebind
          // tracking to whichever process owns the named window.
          Tool.Handoff_Pending = true;
          Track_Handoff ( Tool, Window_Title );
        }

        statusLabel.Text = Display_Name + " launched (PID " + P.Id + ").";
        Logger.Log ( "Launch", Display_Name + " (PID " + P.Id + ")" );
      }
      catch ( Exception Ex )
      {
        // Launch failed - make sure the button is usable again
        if ( Launcher != null )
        {
          Launcher.Enabled   = true;
          Launcher.BackColor = _ReadyColor;
        }
        MessageBox.Show ( text: "Could not launch " + Display_Name + ":\n\n" + Ex.Message,
                          caption: "Launch Failed", buttons: MessageBoxButtons.OK,
                          icon: MessageBoxIcon.Error );
      }
    }

    /// <summary>Return a launcher button to the green/ready state.</summary>
    private void ReleaseButton ( LaunchedTool Tool )
    {
      using var Block = Trace_Block.Start_If_Enabled ();
      if ( Tool.Button != null )
      {
        Tool.Button.Enabled   = true;
        Tool.Button.BackColor = _ReadyColor;
      }
    }

    /// <summary>Removes a launched tool's row entirely once its process has
    /// actually exited — however that happened (X button, Cancel, force-kill,
    /// crash). Must run on the UI thread.</summary>
    private void Remove_Launched_Tool ( LaunchedTool Tool )
    {
      ReleaseButton ( Tool );
      _Launched.Remove ( Tool );
      if ( Tool.Item.ListView != null )
        listViewProcesses.Items.Remove ( Tool.Item );
      Update_Launched_Tool_Buttons ();
    }

    // ------------------------------------------------------------
    //  First click launches (button goes green); second click
    //  politely closes the tool's window.
    // ------------------------------------------------------------
    private void ToggleTool ( string Display_Name, string File_Name, string Arguments,
                              Button Launcher, string Window_Title = null )
    {
      using var Block = Trace_Block.Start_If_Enabled ();
      var Running     = _Launched.FirstOrDefault ( T => T.Button == Launcher && T.Process != null &&
                                                        ! T.Process.HasExited );

      if ( Running == null )
      {
        LaunchTool ( Display_Name, File_Name, Arguments, Launcher, Window_Title );
      }
      else
      {
        try
        {
          Running.Process.Refresh ();

          bool Asked = Running.Process.CloseMainWindow ();
          if ( ! Asked )
            Asked = Close_Tool_Windows ( Running.Process.Id );

          statusLabel.Text = Asked ? "Closing " + Running.Name + "..."
                                   : Running.Name + ( " has no closable window — use End Task " +
                                                      "in" + " t" + "he" + " p" + "ro" + "ce" +
                                                      "ss" + " l" + "is" + "t" + "." );
        }
        catch ( InvalidOperationException )
        {
          // Process exited between the check and the close — the
          // Exited event will clean up momentarily.
        }
      }
    }

    private static string Sys32 ( string File )
    {
      return Path.Combine ( Environment.SystemDirectory, File );
    }

    private void BtnTaskScheduler_Click ( object Sender, EventArgs E )
    {
      using var Block = Trace_Block.Start_If_Enabled ();
      ToggleTool ( Display_Name: "Task Scheduler", File_Name: "mmc.exe",
                   Arguments: Sys32 ( "taskschd.msc" ), Launcher: (Button) Sender );
    }

    private void BtnSystemProtection_Click ( object Sender, EventArgs E )
    {
      using var Block = Trace_Block.Start_If_Enabled ();
      ToggleTool ( Display_Name: "System Protection",
                   File_Name: Sys32 ( "SystemPropertiesProtection.exe" ), Arguments: "",
                   Launcher: (Button) Sender );
    }

    private void BtnRestoreWizard_Click ( object Sender, EventArgs E )
    {
      using var Block = Trace_Block.Start_If_Enabled ();
      ToggleTool ( Display_Name: "System Restore Wizard", File_Name: Sys32 ( "rstrui.exe" ),
                   Arguments: "", Launcher: (Button) Sender );
    }

    private void BtnRegistryEditor_Click ( object Sender, EventArgs E )
    {
      using var Block = Trace_Block.Start_If_Enabled ();
      ToggleTool ( Display_Name: "Registry Editor", File_Name: "regedit.exe", Arguments: "",
                   Launcher: (Button) Sender );
    }

    private void BtnEventViewer_Click ( object Sender, EventArgs E )
    {
      using var Block = Trace_Block.Start_If_Enabled ();
      ToggleTool ( Display_Name: "Event Viewer", File_Name: "mmc.exe",
                   Arguments: Sys32 ( "eventvwr.msc" ), Launcher: (Button) Sender );
    }

    private void BtnServices_Click ( object Sender, EventArgs E )
    {
      using var Block = Trace_Block.Start_If_Enabled ();
      ToggleTool ( Display_Name: "Services", File_Name: "mmc.exe",
                   Arguments: Sys32 ( "services.msc" ), Launcher: (Button) Sender );
    }

    private void BtnDiskManagement_Click ( object Sender, EventArgs E )
    {
      using var Block = Trace_Block.Start_If_Enabled ();
      ToggleTool ( Display_Name: "Disk Management", File_Name: "mmc.exe",
                   Arguments: Sys32 ( "diskmgmt.msc" ), Launcher: (Button) Sender );
    }

    private void BtnComputerMgmt_Click ( object Sender, EventArgs E )
    {
      using var Block = Trace_Block.Start_If_Enabled ();
      ToggleTool ( Display_Name: "Computer Management", File_Name: "mmc.exe",
                   Arguments: Sys32 ( "compmgmt.msc" ), Launcher: (Button) Sender );
    }

    private void BtnSystemInfo_Click ( object Sender, EventArgs E )
    {
      using var Block = Trace_Block.Start_If_Enabled ();
      ToggleTool ( Display_Name: "System Information", File_Name: Sys32 ( "msinfo32.exe" ),
                   Arguments: "", Launcher: (Button) Sender );
    }

    private void BtnPerfMonitor_Click ( object Sender, EventArgs E )
    {
      using var Block = Trace_Block.Start_If_Enabled ();
      ToggleTool ( Display_Name: "Performance Monitor", File_Name: "mmc.exe",
                   Arguments: Sys32 ( "perfmon.msc" ), Launcher: (Button) Sender );
    }

    private void BtnResourceMonitor_Click ( object Sender, EventArgs E )
    {
      using var Block = Trace_Block.Start_If_Enabled ();
      ToggleTool ( Display_Name: "Resource Monitor", File_Name: Sys32 ( "resmon.exe" ),
                   Arguments: "", Launcher: (Button) Sender, Window_Title: "Resource Monitor" );
    }

    private void BtnDeviceManager_Click ( object Sender, EventArgs E )
    {
      using var Block = Trace_Block.Start_If_Enabled ();
      ToggleTool ( Display_Name: "Device Manager", File_Name: "mmc.exe",
                   Arguments: Sys32 ( "devmgmt.msc" ), Launcher: (Button) Sender );
    }

    private void BtnLocalUsers_Click ( object Sender, EventArgs E )
    {
      using var Block = Trace_Block.Start_If_Enabled ();
      ToggleTool ( Display_Name: "Local Users && Groups", File_Name: "mmc.exe",
                   Arguments: Sys32 ( "lusrmgr.msc" ), Launcher: (Button) Sender );
    }

    private void BtnFirewall_Click ( object Sender, EventArgs E )
    {
      using var Block = Trace_Block.Start_If_Enabled ();
      ToggleTool ( Display_Name: "Windows Firewall", File_Name: "mmc.exe",
                   Arguments: Sys32 ( "wf.msc" ), Launcher: (Button) Sender );
    }

    // ====================================================================
    //  PROCESS MONITOR / CONTROL
    // ====================================================================

    private LaunchedTool SelectedTool ()
    {
      using var Block = Trace_Block.Start_If_Enabled ();
      if ( listViewProcesses.SelectedItems.Count == 0 )
        return null;
      return listViewProcesses.SelectedItems[ 0 ].Tag as LaunchedTool;
    }

    private void BtnCloseTool_Click ( object Sender, EventArgs E )
    {
      using var    Block = Trace_Block.Start_If_Enabled ();
      LaunchedTool Tool  = SelectedTool ();
      if ( Tool == null )
      {
        statusLabel.Text = "Select a launched tool first.";
        return;
      }
      try
      {
        if ( ! Tool.Process.HasExited )
        {
          // Polite close - same as clicking the X button
          if ( ! Tool.Process.CloseMainWindow () )
          {
            statusLabel.Text = Tool.Name + " has no main window to close - use End Task.";
          }
        }
        else
        {
          statusLabel.Text = Tool.Name + " is already closed.";
        }
      }
      catch ( Exception Ex )
      {
        statusLabel.Text = "Close failed: " + Ex.Message;
      }
    }

    private void BtnKillTool_Click ( object Sender, EventArgs E )
    {
      using var    Block = Trace_Block.Start_If_Enabled ();
      LaunchedTool Tool  = SelectedTool ();
      if ( Tool == null )
      {
        statusLabel.Text = "Select a launched tool first.";
        return;
      }
      try
      {
        if ( ! Tool.Process.HasExited )
        {
          Tool.Process.Kill ();
          statusLabel.Text = Tool.Name + " terminated.";
          Logger.Log ( "Kill", Tool.Name + " force-terminated." );
        }
        else
        {
          statusLabel.Text = Tool.Name + " is already closed.";
        }
      }
      catch ( Exception Ex )
      {
        statusLabel.Text = "End Task failed: " + Ex.Message;
      }
    }

    private void BtnRemoveClosed_Click ( object Sender, EventArgs E )
    {
      using var Block = Trace_Block.Start_If_Enabled ();
      for ( int I = listViewProcesses.Items.Count - 1; I >= 0; I-- )
      {
        var Tool = listViewProcesses.Items[ I ].Tag as LaunchedTool;
        bool                                        Exited = true;
        try
        {
          Exited = Tool == null || Tool.Process.HasExited;
        }
        catch
        {
        }
        if ( Exited )
        {
          if ( Tool != null )
            _Launched.Remove ( Tool );
          listViewProcesses.Items.RemoveAt ( I );
        }
      }
      Update_Launched_Tool_Buttons ();
    }

    // Safety net: poll status in case Exited didn't fire (e.g. shell handoff)
    private void TimerStatus_Tick ( object Sender, EventArgs E )
    {
      // Collect first — removing from _Launched while iterating it would throw.
      var Exited_Tools = new List<LaunchedTool> ();

      foreach ( LaunchedTool Tool in _Launched )
      {
        if ( Tool.Handoff_Pending )
          continue; // rebinding in progress — hands off

        try
        {
          if ( Tool.Process.HasExited )
            Exited_Tools.Add ( Tool );
        }
        catch
        {
        }
      }

      foreach ( var Tool in Exited_Tools )
        Remove_Launched_Tool ( Tool );
    }

    private void Quit_Button_Click ( object Sender, EventArgs E )
    {
      using var Block = Trace_Block.Start_If_Enabled ();
      Logger.Log ( "App", "Quit - closing all launched tools." );
      for ( int I = Application.OpenForms.Count - 1; I >= 0; I-- )
      {
        if ( Application.OpenForms[ I ] != this )
          Application.OpenForms[ I ].Close ();
      }
      Close ();
    }

    public void Show_Log ()
    {
      using var Block = Trace_Block.Start_If_Enabled ();
      if ( _LogForm == null || _LogForm.IsDisposed )
      {
        _LogForm             = new OutputForm ();
        _LogForm.Width       = 850;
        _LogForm.Location    = new System.Drawing.Point ( Math.Max ( 0, Location.X + 60 ),
                                                          Location.Y + 100 );

        // Re-enable the button when the log window closes.
        _LogForm.FormClosed += ( S, Ev ) =>
        {
          View_Log_Button.Enabled   = true;
          View_Log_Button.BackColor = _ReadyColor;
        };

        _LogForm.Show ( this ); // owned, non-modal
      }

      _LogForm.ShowLiveLog ( "Activity Log - " + Environment.MachineName, Logger.CurrentLogPath );

      if ( _LogForm.WindowState == FormWindowState.Minimized )
        _LogForm.WindowState = FormWindowState.Normal;
      _LogForm.BringToFront ();

      View_Log_Button.Enabled   = false;
      View_Log_Button.BackColor = _RunningColor;
    }
    private void View_Log_Button_Click ( object Sender, EventArgs E )
    {
      using var Block = Trace_Block.Start_If_Enabled ();
      Show_Log ();
    }

    private void Email_Log_Button_Click ( object Sender, EventArgs E )
    {

      if ( ! EnsureEmailConfigured () )
        return;

      try
      {
        string Src = Logger.CurrentLogPath;
        string Tmp = System.IO.Path.Combine ( System.IO.Path.GetTempPath (),
                                              "AdminToolkit-" + Environment.MachineName +
                                                "-log.txt" );

        using (
          var InFs =
            new System.IO
              .FileStream ( Src, System.IO.FileMode.Open, System.IO.FileAccess.Read,
                            System.IO.FileShare
                              .ReadWrite ) ) using ( var OutFs =
                                                       new System.IO
                                                         .FileStream ( Tmp,
                                                                       System.IO.FileMode.Create ) )
        {
          InFs.CopyTo ( OutFs );
        }

        string Subject   = "AdminToolkit log - " + Environment.MachineName + "  " +
                           DateTime.Now.ToString ( "yyyy-MM-dd HH:mm" );
        string Body      = "Activity log attached from " + Environment.MachineName + " (" +
                           Environment.UserName + ").";

        statusLabel.Text = "Sending log...";
        Application.DoEvents ();

        Emailer.Send ( subject: Subject, body: Body, attachmentPath: Tmp );

        statusLabel.Text = "Log emailed.";
        Logger.Log ( "Email", "Log emailed successfully." );
      }
      catch ( Exception Ex )
      {
        statusLabel.Text = "Email failed: " + Ex.Message;
        Logger.Log ( "Email", "FAILED: " + Ex.Message );

        bool LooksLikeAuth = Ex is System.Net.Mail.SmtpException &&
                             Ex.Message.IndexOf ( "auth", StringComparison.OrdinalIgnoreCase ) >= 0;

        if ( LooksLikeAuth &&
             MessageBox.Show ( text: "Could not send the log:\n\n" + Ex.Message +
                                 ( "\n\nThis looks like a login problem. " + "Re" + "-e" + "nt" +
                                   "er" + " t" + "he" + " p" + "as" + "sw" + "or" + "d " + "no" +
                                   "w" + "?" ),
                               caption: "Email Failed", buttons: MessageBoxButtons.YesNo,
                               icon: MessageBoxIcon.Error ) == DialogResult.Yes )
        {
          Emailer.ClearPassword (); // drop the bad one
          EnsureEmailConfigured (); // prompt for a fresh password
        }
        else if ( ! LooksLikeAuth )
        {
          MessageBox.Show ( text: "Could not send the log:\n\n" + Ex.Message,
                            caption: "Email Failed", buttons: MessageBoxButtons.OK,
                            icon: MessageBoxIcon.Error );
        }
      }
    }

    /// <summary>
    /// Ensure email is ready: server fields filled in and an encrypted password
    /// stored. Opens the settings file or prompts for the password as needed.
    /// Returns true only when everything is set.
    /// </summary>
    private bool EnsureEmailConfigured ()
    {
      Emailer.SmtpConfig CFG = Emailer.LoadConfig (); // writes template if missing

      if ( ! CFG.ServerReady )
      {
        if ( MessageBox.Show ( text: "Email isn't set up on this machine yet.\n\n" +
                                 "Open the settings file to fill in the server, " +
                                 "user, and recipient?",
                               caption: "Email Setup Needed", buttons: MessageBoxButtons.YesNo,
                               icon: MessageBoxIcon.Information ) == DialogResult.Yes )
          Emailer.OpenSettings ();

        statusLabel.Text = "Complete the email settings, then try again.";
        return false;
      }

      if ( ! Emailer.HasPassword () )
      {
        string PWD = PasswordBox.Show ( owner: this, title: "Email Password",
                                        prompt: "Enter the SMTP / app password for " +
                                          CFG.User + ".\n\n" +
                                          ( "It's encrypted for this account on this PC - never " +
                                            "stored as " + "plain text." ) );

        if ( string.IsNullOrEmpty ( PWD ) )
        {
          statusLabel.Text = "Email password not set.";
          return false;
        }

        Emailer.SavePassword ( PWD );
        Logger.Log ( "Email", "SMTP password stored (encrypted)." );
      }

      return true;
    }

    private void Email_Settings_Button_Click ( object Sender, EventArgs E )
    {
      Emailer.OpenSettings (); // opens email.settings in Notepad
      statusLabel.Text = "Editing email settings - save Notepad, then send again.";
    }

    private void Enable_Shadowing_Button_Click ( object Sender, EventArgs E )
    {
      var Choices = BuildDriveChoices ();
      if ( Choices.Count == 0 )
      {
        statusLabel.Text = "No fixed NTFS drives found to protect.";
        return;
      }

      List<string> Selected;
      using ( var Dlg = new DriveSelectDialog ( Choices ) )
      {
        if ( Dlg.ShowDialog ( this ) != DialogResult.OK )
        {
          statusLabel.Text = "Shadowing setup cancelled.";
          return;
        }
        Selected = Dlg.SelectedDrives;
      }

      if ( Selected.Count == 0 )
      {
        statusLabel.Text = "No drives selected — nothing to do.";
        return;
      }

      statusLabel.Text = "Setting up shadowing...";
      Application.DoEvents ();

      if ( CreateShadowTask ( Selected, out string Error ) )
      {
        var Covered      = GetFixedNtfsDrives ().Where ( IsShadowingActive ).ToList ();
        statusLabel.Text = "Shadowing active on " + string.Join ( ", ", Covered ) +
                           " — daily task covers all of them.";
      }
      else
      {
        MessageBox.Show ( text: "Setup failed.\n\n" + Error +
                            "\n\nCheck that you're elevated and that System " +
                            ( "Restore isn't disabled by group policy. See the " + "log for " +
                              "details." ),
                          caption: "Setup Failed", buttons: MessageBoxButtons.OK,
                          icon: MessageBoxIcon.Warning );
        statusLabel.Text = "Shadowing setup failed.";
      }
    }

    // (Re)register AutoShadowCopy to snapshot exactly these drives.

    private void Help_Button_Click ( object Sender, EventArgs E )
    {
      using var Block = Trace_Block.Start_If_Enabled ();
      using ( var Dlg = new HelpDialog () ) Dlg.ShowDialog ( this );
    }

    private void Disable_Shadowing_Button_Click ( object Sender, EventArgs E )
    {
      using var Block    = Trace_Block.Start_If_Enabled ();
      string    SysDrive = Path.GetPathRoot (
        Environment.GetFolderPath ( Environment.SpecialFolder.Windows ) );

      // Currently-protected fixed NTFS drives
      var ProtectedDrives = DriveInfo.GetDrives ()
                              .Where ( D => D.DriveType == DriveType.Fixed && D.IsReady &&
                                            string.Equals ( D.DriveFormat, "NTFS",
                                                            StringComparison.OrdinalIgnoreCase ) &&
                                            IsShadowingActive ( D.Name ) )
                              .Select ( D => D.Name )
                              .ToList ();

      if ( ProtectedDrives.Count == 0 )
      {
        statusLabel.Text = "Shadowing isn't active on any drive — nothing to turn off.";
        return;
      }

      // Picker: nothing pre-checked; user checks what to TURN OFF
      var Choices =
        ProtectedDrives
          .Select (
            Name =>
            {
              var    Drive_Info      = new DriveInfo ( Name );
              string Label           = string.IsNullOrWhiteSpace ( SafeLabel ( Drive_Info ) )
                                         ? "Local Disk"
                                         : SafeLabel ( Drive_Info );
              double GB              = Drive_Info.TotalSize / 1024.0 / 1024.0 / 1024.0;
              bool   Is_System_Drive = Name.Equals ( SysDrive, StringComparison.OrdinalIgnoreCase );
              return new DriveChoice { Name    = Name,
                                       Display = string.Format ( "{0}  {1}  ({2:0} " + "GB){3}",
                                                                 Name.TrimEnd ( '\\' ), Label, GB,
                                                                 Is_System_Drive
                                                                   ? " " + " " + " " + "—" + " " +
                                                                       "s" + "y" + "s" + "t" + "e" +
                                                                       "m" + " " + "d" + "r" + "ive"
                                                                   : "" ),
                                       Checked = false };
            } )
          .ToList ();

      List<string> OffList;
      using ( var Dlg = new DriveSelectDialog ( choices: Choices, title: "Turn Off Shadowing",
                                                headerText: ( "Check the drives you want to " +
                                                              "TURN " ) +
                                                  "OFF " + "(this" + " dele" + "tes " +
                                                  "their" + " snap" + "shots" + "):" ) )
      {
        if ( Dlg.ShowDialog ( this ) != DialogResult.OK )
        {
          statusLabel.Text = "Teardown cancelled.";
          return;
        }
        OffList = Dlg.SelectedDrives;
      }

      if ( OffList.Count == 0 )
      {
        statusLabel.Text = "No drives selected — nothing to do.";
        return;
      }

      // Cascade: the system drive can't be turned off while other drives stay
      // protected.
      bool Turn_Off_System = OffList.Any (
        D => D.Equals ( SysDrive, StringComparison.OrdinalIgnoreCase ) );
      var Survivors =
        ProtectedDrives.Except ( OffList, StringComparer.OrdinalIgnoreCase ).ToList ();
      bool Cascaded = false;

      if ( Turn_Off_System && Survivors.Count > 0 )
      {
        OffList = ProtectedDrives.ToList (); // everything comes off
        Survivors.Clear ();
        Cascaded = true;
      }

      string WillDelete  = string.Join ( ", ", OffList );
      string TaskFate    = Survivors.Count > 0
                             ? "The AutoShadowCopy task will be rebuilt to " +
                                 "snapshot only: " + string.Join ( ", ", Survivors ) + "."
                             : "The AutoShadowCopy task will be removed entirely.";
      string CascadeNote = Cascaded ? "\n\nNote: turning off the system drive (" +
                                        SysDrive.TrimEnd ( '\\' ) +
                                        ( ") also turns off the other protected " + "drives — " +
                                          "Windows " + "requires " + "this." )
                                    : "";

      var    Confirm = MessageBox.Show ( text: "This will permanently delete all snapshots on: " +
                                           WillDelete + "." + CascadeNote + "\n\n" + TaskFate +
                                           "\n\nSnapshots cannot be recovered. Continue?",
                                         caption: "Confirm Turn Off Shadowing",
                                         buttons: MessageBoxButtons.YesNo,
                                         icon: MessageBoxIcon.Warning,
                                         defaultButton: MessageBoxDefaultButton.Button2 );

      if ( Confirm != DialogResult.Yes )
      {
        statusLabel.Text = "Teardown cancelled.";
        return;
      }

      statusLabel.Text = "Turning off shadowing on " + WillDelete + "...";
      Application.DoEvents ();

      if ( TeardownShadowing ( offDrives: OffList, survivors: Survivors, error: out string Error ) )
      {
        statusLabel.Text = "Shadowing turned off on " + WillDelete +
                           ( Survivors.Count > 0
                               ? " — task now covers " + string.Join ( ", ", Survivors ) + "."
                               : " — task removed." );
      }
      else
      {
        MessageBox.Show ( text: "Turn-off failed.\n\n" + Error +
                            "\n\nCheck the log for details.",
                          caption: "Teardown Failed", buttons: MessageBoxButtons.OK,
                          icon: MessageBoxIcon.Warning );
        statusLabel.Text = "Teardown failed.";
      }
    }

    private void Restore_Points_Button_Click ( object Sender, EventArgs E )
    {
      using var Block = Trace_Block.Start_If_Enabled ();
      if ( _RestorePointsForm == null || _RestorePointsForm.IsDisposed )
      {
        _RestorePointsForm             = new Restore_Point_List_Form ();
        _RestorePointsForm.FormClosed += ( S, Args ) => Restore_Points_Button.Enabled = true;
        _RestorePointsForm.Show ( this );
        Restore_Points_Button.Enabled = false;
      }
      else
      {
        if ( _RestorePointsForm.WindowState == FormWindowState.Minimized )
          _RestorePointsForm.WindowState = FormWindowState.Normal;
        _RestorePointsForm.BringToFront ();
      }
    }

    private void Enable_Registry_Backup_Button_Click ( object Sender, EventArgs E )
    {
      using var Block = Trace_Block.Start_If_Enabled ();
      if ( _RegistryBackupForm == null || _RegistryBackupForm.IsDisposed )
      {
        _RegistryBackupForm             = new Registry_Backup_Form ();
        _RegistryBackupForm.FormClosed += ( S,
                                            Args ) => Enable_Registry_Backup_Button.Enabled = true;
        _RegistryBackupForm.Show ( this );
        Enable_Registry_Backup_Button.Enabled = false;
      }
      else
      {
        if ( _RegistryBackupForm.WindowState == FormWindowState.Minimized )
          _RegistryBackupForm.WindowState = FormWindowState.Normal;
        _RegistryBackupForm.BringToFront ();
      }
    }

    private void Snapshot_Operations_Button_Click ( object Sender, EventArgs E )
    {
      using var Block = Trace_Block.Start_If_Enabled ();
      if ( _ShadowCopyForm == null || _ShadowCopyForm.IsDisposed )
      {
        _ShadowCopyForm             = new Shadow_Copy_Form ();
        _ShadowCopyForm.FormClosed += ( S, Args ) => Snapshot_Operations_Button.Enabled = true;
        _ShadowCopyForm.Show ( this );
        Snapshot_Operations_Button.Enabled = false;
      }
      else
      {
        if ( _ShadowCopyForm.WindowState == FormWindowState.Minimized )
          _ShadowCopyForm.WindowState = FormWindowState.Normal;
        _ShadowCopyForm.BringToFront ();
      }
    }

    private void Printer_Button_Click ( object Sender, EventArgs E )
    {
      using var Block = Trace_Block.Start_If_Enabled ();
      Cursor.Current  = Cursors.WaitCursor;
      try
      {
        if ( _PrinterForm == null || _PrinterForm.IsDisposed )
        {
          _PrinterForm             = new Printer_Form ();
          _PrinterForm.FormClosed += ( S, Args ) => Printer_Button.Enabled = true;
          _PrinterForm.Show ( this );
          Printer_Button.Enabled = false;
        }
        else
        {
          if ( _PrinterForm.WindowState == FormWindowState.Minimized )
            _PrinterForm.WindowState = FormWindowState.Normal;
          _PrinterForm.BringToFront ();
        }
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    private void Admin_Commands_Button_Click ( object Sender, EventArgs E )
    {
      using var Block         = Trace_Block.Start_If_Enabled ();
      var       Commands_Form = new Commands_Form ();
      Commands_Form.Show ( this );
    }

    private void Remote_Tools_Button_Click ( object Sender, EventArgs E )
    {
      using var Block              = Trace_Block.Start_If_Enabled ();
      var       Remote_Access_Form = new Remote_Access_Form ();
      Remote_Access_Form.Show ( this );
    }

    private void Powershell_Button_Click ( object Sender, EventArgs E )
    {
      using var Block = Trace_Block.Start_If_Enabled ();
      try
      {
        ProcessStartInfo Psi = new ProcessStartInfo {
          FileName        = "powershell.exe",
          UseShellExecute = true // opens in its own window, independent of your app
        };

        Process.Start ( Psi );
      }
      catch ( Exception Ex )
      {
        MessageBox.Show ( $"Failed to launch PowerShell: {Ex.Message}" );
      }
    }

    private void CMD_Button_Click ( object Sender, EventArgs E )
    {
      using var Block = Trace_Block.Start_If_Enabled ();
      try
      {
        ProcessStartInfo Psi = new ProcessStartInfo {
          FileName        = "cmd.exe",
          UseShellExecute = true // opens in its own window, independent of your app
        };

        Process.Start ( Psi );
      }
      catch ( Exception Ex )
      {
        MessageBox.Show ( $"Failed to launch Command Prompt: {Ex.Message}" );
      }
    }

    private void Trace_Button_Click ( object sender, EventArgs e )
    {
      using var Block = Trace_Block.Start_If_Enabled ();
      Trace_Execution.Toggle ();
      Trace_Button.Text      = Trace_Execution.IsRunning ? "Trace OFF" : "Trace ON";
      Trace_Button.BackColor = Trace_Execution.IsRunning ? Color.Yellow : SystemColors.Control;
    }

    private void Update_Trace_Button_State ( bool Trace_Visible )
    {
      using var Block = Trace_Block.Start_If_Enabled ();

      if ( InvokeRequired )
      {
        BeginInvoke ( new Action ( () => Update_Trace_Button_State ( Trace_Visible ) ) );
        return;
      }

      Trace_Button.Text      = Trace_Visible ? "Trace OFF" : "Trace ON";
      Trace_Button.BackColor = Trace_Visible ? Color.Yellow : SystemColors.Control;
    }

    private void Trace_Window_Trace_Visibility_Changed ( bool Visible )
    {
      using var Block = Trace_Block.Start_If_Enabled ();
      if ( InvokeRequired )
      {
        BeginInvoke ( () => Trace_Window_Trace_Visibility_Changed ( Visible ) );
        return;
      }
      Trace_Button.Text      = Visible ? "Trace OFF" : "Trace ON";
      Trace_Button.BackColor = Visible ? Color.Green : SystemColors.Control;
    }
  }
}
