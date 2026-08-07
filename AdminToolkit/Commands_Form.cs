// ============================================================
//  Commands_Form.cs
//  Remote-triage command board (Windows built-ins only).
//  Each command button has a dedicated click handler that
//  passes a Triage_Command enum value to Execute_Command_Async.
//  Adding a command means: adding an enum member, a case in
//  Get_Command_Line, a button, and a one-line click handler.
//
//  RustDesk / Tailscale diagnostics live in Remote_Access_Form.
//
//  Usage from MainForm (modeless):
//      var f = new Commands_Form();
//      f.Show(this);
// ============================================================

using System.Management;
using System.Text;
using System.Text.RegularExpressions;

namespace Admin_Tools
{
  public enum Triage_Command
  {
    // Identity & Sessions
    Whoami,
    Net_User,
    Local_Admins,
    Query_User,
    Net_Session,

    // Network
    Ipconfig,
    Arp,
    Route,
    Netstat,
    Nslookup,
    Flush_Dns,

    // System State
    System_Info,
    Hostname,
    Tasklist,
    Tasklist_Svc,
    Sc_Query,

    // Disk & Storage
    Chkdsk,

    // Health & Logs
    Sfc_Verify,
    Battery_Report,
    System_Events,
    Uptime,

    // System Config
    Driver_Query

  }

  public partial class Commands_Form : Form
  {
    public Commands_Form ()
    {
      InitializeComponent ();
      Message_Textbox.ReadOnly = true; // you may already want this since it's a status line
      Message_Textbox.TabStop  = false;
      Message_Textbox.Cursor =
        Cursors.Default; // removes the I-beam hint that invites clicking/typing
      Clear_Message ();
    }

    protected override void OnFormClosing ( FormClosingEventArgs E )
    {
      if ( ! Commands_Panel.Enabled ) // a command is running
      {
        E.Cancel = true;
        return;
      }
      base.OnFormClosing ( E );
    }

    // --------------------------------------------------------
    //  Enum -> command line
    // --------------------------------------------------------
    private static string Get_Command_Line ( Triage_Command Command )
    {
      switch ( Command )
      {
        // Identity & Sessions
        case Triage_Command.Whoami :
          return "whoami /all";
        case Triage_Command.Net_User :
          return "net user";
        case Triage_Command.Local_Admins :
          return "net localgroup administrators";
        case Triage_Command.Query_User :
          return "query user";
        case Triage_Command.Net_Session :
          return "net session";

        // Network
        case Triage_Command.Ipconfig :
          return "ipconfig /all";
        case Triage_Command.Arp :
          return "arp -a";
        case Triage_Command.Route :
          return "route print";
        case Triage_Command.Netstat :
          return "netstat -ano";
        case Triage_Command.Nslookup :
          return "nslookup google.com";
        case Triage_Command.Flush_Dns :
          return "ipconfig /flushdns";

        // System State
        case Triage_Command.System_Info :
          return "systeminfo";
        case Triage_Command.Hostname :
          return "hostname";
        case Triage_Command.Tasklist :
          return "tasklist";
        case Triage_Command.Tasklist_Svc :
          return "tasklist /svc";
        case Triage_Command.Sc_Query :
          return "sc query";
        case Triage_Command.Driver_Query :
          return "driverquery";

        // Disk & Storage
        case Triage_Command.Chkdsk :
          return "chkdsk";

        // Health & Logs
        case Triage_Command.Sfc_Verify :
          return "sfc /verifyonly";
        case Triage_Command.Battery_Report :
          return "powercfg /batteryreport";
        case Triage_Command.System_Events :
          return "wevtutil qe System /c:20 /rd:true /f:text";
        case Triage_Command.Uptime :
          return "net statistics workstation";

        default :
          throw new ArgumentOutOfRangeException ( nameof ( Command ), Command,
                                                  "No command line is defined for this " + "Triag" +
                                                    "e_" + "Comma" + "nd " + "value" + "." );
      }
    }

    // --------------------------------------------------------
    //  Single execute method — every command handler funnels
    //  through here.
    // --------------------------------------------------------
    private async Task Execute_Command_Async ( Triage_Command Command )
    {
      string Command_Line = Get_Command_Line ( Command );

      try
      {
        Show_Text_Output ();
        Commands_Panel.Enabled = false;
        Logger.Log ( "Command", Command_Line );
        await Command_Runner.Run_Command_Async ( Command_Line, txtOutput );
      }
      catch ( Exception Ex )
      {
        txtOutput.Text = Ex.ToString ();
      }
      finally
      {
        Commands_Panel.Enabled = true;
      }
    }

    // --------------------------------------------------------
    //  Identity & Sessions
    // --------------------------------------------------------
    private async void BtnWhoami_Click ( object Sender, EventArgs E )
    {
      Clear_Message ();
      await Execute_Command_Async ( Triage_Command.Whoami );
    }
    private async void BtnNetUser_Click ( object Sender, EventArgs E )
    {
      Clear_Message ();
      await Execute_Command_Async ( Triage_Command.Net_User );
    }

    private async void BtnLocalAdmins_Click ( object Sender, EventArgs E )
    {
      Clear_Message ();
      await Execute_Command_Async ( Triage_Command.Local_Admins );
    }

    private async void BtnQueryUser_Click ( object Sender, EventArgs E )
    {
      Clear_Message ();
      await Execute_Command_Async ( Triage_Command.Query_User );
    }

    private async void BtnNetSession_Click ( object Sender, EventArgs E )
    {
      Clear_Message ();
      await Execute_Command_Async ( Triage_Command.Net_Session );
    }

    // --------------------------------------------------------
    //  Network
    // --------------------------------------------------------
    private async void BtnIpconfig_Click ( object Sender, EventArgs E )
    {
      Clear_Message ();
      await Execute_Command_Async ( Triage_Command.Ipconfig );
    }
    private async void BtnArp_Click ( object Sender, EventArgs E )
    {
      Clear_Message ();
      await Execute_Command_Async ( Triage_Command.Arp );
    }

    private async void BtnRoute_Click ( object Sender, EventArgs E )
    {
      Clear_Message ();
      await Execute_Command_Async ( Triage_Command.Route );
    }

    private async void BtnNetstat_Click ( object Sender, EventArgs E )
    {
      Clear_Message ();
      await Execute_Command_Async ( Triage_Command.Netstat );
    }

    private async void BtnNslookup_Click ( object Sender, EventArgs E )
    {
      Clear_Message ();
      await Execute_Command_Async ( Triage_Command.Nslookup );
    }

    private async void BtnFlushDns_Click ( object Sender, EventArgs E )
    {
      Clear_Message ();
      await Execute_Command_Async ( Triage_Command.Flush_Dns );
    }

    // --------------------------------------------------------
    //  System State
    // --------------------------------------------------------
    private async void BtnSystemInfo_Click ( object Sender, EventArgs E )
    {
      Clear_Message ();
      await Execute_Command_Async ( Triage_Command.System_Info );
    }

    private async void BtnHostname_Click ( object Sender, EventArgs E )
    {
      Clear_Message ();
      await Execute_Command_Async ( Triage_Command.Hostname );
    }

    private async void BtnTasklist_Click ( object Sender, EventArgs E )
    {
      Clear_Message ();
      await Execute_Command_Async ( Triage_Command.Tasklist );
    }

    private async void BtnTasklistSvc_Click ( object Sender, EventArgs E )
    {
      Clear_Message ();
      await Execute_Command_Async ( Triage_Command.Tasklist_Svc );
    }

    private async void BtnScQuery_Click ( object Sender, EventArgs E )
    {
      Clear_Message ();
      await Execute_Command_Async ( Triage_Command.Sc_Query );
    }

    private async void BtnDriverQuery_Click ( object Sender, EventArgs E )
    {
      Clear_Message ();
      await Execute_Command_Async ( Triage_Command.Driver_Query );
    }

    // --------------------------------------------------------
    //  Disk & Storage
    // --------------------------------------------------------
    private async void BtnChkdsk_Click ( object Sender, EventArgs E )
    {
      Clear_Message ();
      await Execute_Command_Async ( Triage_Command.Chkdsk );
    }

    // BtnVol_Click is a locally-computed command — see the
    // "Locally-computed commands" section below.

    // --------------------------------------------------------
    //  Health & Logs
    // --------------------------------------------------------
    private async void BtnSfcVerify_Click ( object Sender, EventArgs E )
    {
      Clear_Message ();
      await Execute_Command_Async ( Triage_Command.Sfc_Verify );
    }

    private async void BtnBatteryReport_Click ( object Sender, EventArgs E )
    {
      Clear_Message ();
      await Execute_Command_Async ( Triage_Command.Battery_Report );
    }

    private async void BtnSystemEvents_Click ( object Sender, EventArgs E )
    {
      Clear_Message ();
      await Execute_Command_Async ( Triage_Command.System_Events );
    }

    private async void BtnUptime_Click ( object Sender, EventArgs E )
    {
      Clear_Message ();
      await Execute_Command_Async ( Triage_Command.Uptime );
    }

    // --------------------------------------------------------
    //  Output pane buttons
    // --------------------------------------------------------
    private void Btn_Clear_Click ( object Sender, EventArgs E )
    {
      Clear_Message ();
      txtOutput.Clear ();
    }

    private void Btn_Copy_All_Click ( object Sender, EventArgs E )
    {
      Clear_Message ();
      if ( txtOutput.TextLength == 0 )
        return;
      Clipboard.SetText ( txtOutput.Text );
    }

    private void Btn_Save_Click ( object Sender, EventArgs E )
    {
      Clear_Message ();
      if ( txtOutput.TextLength == 0 )
        return;

      using ( var Dialog = new SaveFileDialog () )
      {
        Dialog.Filter   = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
        Dialog.FileName = Environment.MachineName + "_triage_" +
                          DateTime.Now.ToString ( "yyyyMMdd_HHmm" ) + ".txt";

        if ( Dialog.ShowDialog ( this ) != DialogResult.OK )
          return;

        try
        {
          File.WriteAllText ( Dialog.FileName, txtOutput.Text );
        }
        catch ( Exception Ex )
        {
          MessageBox.Show ( this, "Could not save the file:\n" + Ex.Message, "Admin Commands",
                            MessageBoxButtons.OK, MessageBoxIcon.Error );
        }
      }
    }

    private void Btn_Close_Click ( object Sender, EventArgs E )
    {
      Close ();
    }

    // --------------------------------------------------------
    //  Locally-computed commands (no shell)
    // --------------------------------------------------------
    private static string Get_Drives ()
    {
      var String_Builder = new StringBuilder ();
      String_Builder.AppendLine ( "Drive  Label                 Total (GB)   Free (GB)   Used %" );
      String_Builder.AppendLine ( "-----  --------------------  ----------  ----------  ------" );

      foreach ( var Drive in DriveInfo.GetDrives () )
      {
        if ( ! Drive.IsReady )
        {
          String_Builder.AppendLine ( $"{Drive.Name,-5}  (not ready)" );
          continue;
        }

        double Total_GB        = Drive.TotalSize / 1073741824.0;
        double Free_GB         = Drive.TotalFreeSpace / 1073741824.0;
        double Used_Percentage = 100.0 * ( Drive.TotalSize - Drive.TotalFreeSpace ) /
                                 Drive.TotalSize;

        String_Builder.AppendLine (
          $"{Drive.Name,-5}  {Drive.VolumeLabel,-20}  {Total_GB,10:N1}  {Free_GB,10:N1}  {Used_Percentage,5:N1}%" );
      }
      return String_Builder.ToString ();
    }

    private void BtnFreeSpacebyDrive_Click ( object Sender, EventArgs E )
    {
      Clear_Message ();
      Show_Text_Output ();

      txtOutput.Text = Get_Drives ();
    }

    private static string Get_Volumes ()
    {

      var String_Builder = new StringBuilder ();
      String_Builder.AppendLine ( "Drive  Label                 Serial Number   File System" );
      String_Builder.AppendLine ( "-----  --------------------  --------------  -----------" );

      // Win32_LogicalDisk exposes the label and serial that
      // the shell "vol" command only reports for one drive.
      var Scope = new ManagementObjectSearcher ( "SELECT DeviceID, VolumeName, " +
                                                 "VolumeSerialNumber, FileSystem FROM " +
                                                 "Win32_LogicalDisk" );

      foreach ( ManagementObject Management_Object in Scope.Get () )
      {
        string Drive       = Management_Object[ "DeviceID" ] as string ?? ""; // e.g. "C:"
        string Label       = Management_Object[ "VolumeName" ] as string ?? "";
        string File_System = Management_Object[ "FileSystem" ] as string ?? "";
        string Serial      = Format_Serial ( Management_Object[ "VolumeSerialNumber" ] as string );

        String_Builder.AppendLine ( $"{Drive,-5}  {Label,-20}  {Serial,-14}  {File_System}" );
      }
      return String_Builder.ToString ();
    }

    // WMI returns the serial as 8 hex chars with no separator;
    // "vol" shows it as XXXX-XXXX, so match that formatting.
    private static string Format_Serial ( string Raw )
    {
      if ( string.IsNullOrEmpty ( Raw ) )
        return "(none)";
      if ( Raw.Length == 8 )
        return Raw.Substring ( 0, 4 ) + "-" + Raw.Substring ( 4, 4 );
      return Raw;
    }

    private void BtnVol_Click ( object Sender, EventArgs E )
    {
      Clear_Message ();
      Show_Text_Output ();

      txtOutput.Text = Get_Volumes ();
    }

    // --------------------------------------------------------
    //  Output-pane switching (text vs. list view)
    // --------------------------------------------------------
    private void Show_Text_Output ()
    {
      lvUpdates.Visible  = false;
      lvHotfixes.Visible = false;
      txtOutput.Visible  = true;
    }

    private void Show_Updates_Output ()
    {
      txtOutput.Visible  = false;
      lvHotfixes.Visible = false;
      lvUpdates.Visible  = true;
    }

    private void Show_Hotfixes_Output ()
    {
      txtOutput.Visible  = false;
      lvUpdates.Visible  = false;
      lvHotfixes.Visible = true;
    }

    private void Show_List_Output ()
    {
      txtOutput.Visible = false;
      lvUpdates.Visible = true;
    }

    private sealed class Hotfix_Record
    {
      public string HotFixID;
      public string Description;
      public string InstalledOn;
      public string InstalledBy;
      public string Caption;
      public string FixComments;
      public string Status;
    }

    private static List<Hotfix_Record> Get_Hotfixes ()
    {
      var List  = new List<Hotfix_Record> ();

      var Scope = new ManagementObjectSearcher ( "SELECT HotFixID, Description, InstalledOn, " +
                                                 "InstalledBy, Caption, FixComments, Status FROM " +
                                                 "Win32_QuickFixEngineering" );

      foreach ( ManagementObject Management_Object in Scope.Get () )
      {
        List.Add ( new Hotfix_Record { HotFixID    = Management_Object[ "HotFixID" ] as string ?? "",
                                       Description = Management_Object[ "Description" ] as string ?? "",
                                       InstalledOn = Management_Object[ "InstalledOn" ] as string ?? "",
                                       InstalledBy = Management_Object[ "InstalledBy" ] as string ?? "",
                                       Caption     = Management_Object[ "Caption" ] as string ?? "",
                                       FixComments = Management_Object[ "FixComments" ] as string ?? "",
                                       Status      = Management_Object[ "Status" ] as string ?? "" } );
      }

      List.Sort (
        ( A, B ) => string.Compare ( B.InstalledOn, A.InstalledOn, StringComparison.Ordinal ) );
      return List;
    }

    private void BtnHotfixes_Click ( object Sender, EventArgs E )
    {
      Set_Message ( "Double-click an entry for details" );
      Show_Hotfixes_Output ();

      var Hot_Fixes = Get_Hotfixes ();

      lvHotfixes.Items.Clear ();
      foreach ( var Hot_Fix in Hot_Fixes )
      {
        var Item = new ListViewItem ( Hot_Fix.HotFixID );
        Item.SubItems.Add ( Hot_Fix.Description );
        Item.SubItems.Add ( Hot_Fix.InstalledOn );
        Item.SubItems.Add ( Hot_Fix.InstalledBy );
        Item.Tag = Hot_Fix;
        lvHotfixes.Items.Add ( Item );
      }
    }

    // --------------------------------------------------------
    //  Windows Update history
    // --------------------------------------------------------
    private sealed class Update_Record
    {
      public string   Title;
      public string   KB;
      public DateTime Date;
      public string   Operation;
      public string   ResultText;
      public string   Description;
      public string   SupportUrl;
    }

    private static readonly Regex _Kb_Pattern = new Regex ( @"KB\d{6,7}", RegexOptions.IgnoreCase );

    private static string         Extract_KB ( string Title, string Description )
    {
      var Match = _Kb_Pattern.Match ( Title ?? "" );
      if ( ! Match.Success )
        Match = _Kb_Pattern.Match ( Description ?? "" );
      return Match.Success ? Match.Value.ToUpperInvariant () : "";
    }

    private static string Get_Operation_Text ( int Operation )
    {
      switch ( Operation )
      {
        case 1 :
          return "Install";
        case 2 :
          return "Uninstall";
        default :
          return "Unknown";
      }
    }

    private static string Get_Result_Text ( int Code )
    {
      switch ( Code )
      {
        case 2 :
          return "Succeeded";
        case 3 :
          return "Succeeded (errors)";
        case 4 :
          return "Failed";
        case 5 :
          return "Aborted";
        case 1 :
          return "In progress";
        default :
          return "Not started";
      }
    }

    // Uses the Windows Update Agent API via late-bound COM
    // (Microsoft.Update.Session) — Windows built-in, no project reference needed.
    private static List<Update_Record> Get_Update_History ()
    {
      var     List        = new List<Update_Record> ();

      Type    Session_Type   = Type.GetTypeFromProgID ( "Microsoft.Update.Session" );
      dynamic Session     = Activator.CreateInstance ( Session_Type );
      dynamic Searcher    = Session.CreateUpdateSearcher ();

      int     Count       = Searcher.GetTotalHistoryCount ();
      if ( Count == 0 )
        return List;

      dynamic History = Searcher.QueryHistory ( 0, Count );

      foreach ( dynamic Entry in History )
      {
        string Title       = Entry.Title as string ?? "";
        string Description = Entry.Description as string ?? "";

        List.Add ( new Update_Record { Title = Title, Date = Entry.Date,
                                       Operation   = Get_Operation_Text ( (int) Entry.Operation ),
                                       ResultText  = Get_Result_Text ( (int) Entry.ResultCode ),
                                       Description = Description,
                                       SupportUrl  = Entry.SupportUrl as string ?? "",
                                       KB          = Extract_KB ( Title, Description ) } );
      }

      List.Sort ( ( A, B ) => B.Date.CompareTo ( A.Date ) );
      return List;
    }

    private async void BtnUpdates_Click ( object Sender, EventArgs E )
    {

      Show_List_Output ();
      Commands_Panel.Enabled = false;
      Cursor                 = Cursors.WaitCursor;
      try
      {
        Set_Message ( "Double-click an entry for details" );

        var Updates = await Task.Run ( () => Get_Update_History () );

        lvUpdates.Items.Clear ();
        foreach ( var Update in Updates )
        {
          var Item = new ListViewItem ( Update.Date.ToString ( "yyyy-MM-dd HH:mm" ) );
          Item.SubItems.Add ( Update.Title );
          Item.SubItems.Add ( Update.KB );
          Item.SubItems.Add ( Update.Operation );
          Item.SubItems.Add ( Update.ResultText );
          Item.Tag = Update;
          lvUpdates.Items.Add ( Item );
        }
      }
      catch ( Exception Ex )
      {
        MessageBox.Show ( this, "Could not retrieve update history:\n" + Ex.Message,
                          "Admin Commands", MessageBoxButtons.OK, MessageBoxIcon.Error );
      }
      finally
      {
        Commands_Panel.Enabled = true;
        Cursor                 = Cursors.Default;
      }
    }

    private void LvUpdates_DoubleClick ( object Sender, EventArgs E )
    {
      if ( lvUpdates.SelectedItems.Count == 0 )
        return;
      if ( ! ( lvUpdates.SelectedItems[ 0 ].Tag is Update_Record Rec ) )
        return;

      string Details = $"Title: {Rec.Title}\r\n" +
                       $"KB: {(string.IsNullOrEmpty(Rec.KB) ? "(none found)" : Rec.KB)}\r\n" +
                       $"Date: {Rec.Date}\r\n" + $"Operation: {Rec.Operation}\r\n" +
                       $"Result: {Rec.ResultText}\r\n" + $"Support URL: {Rec.SupportUrl}\r\n\r\n" +
                       $"Description:\r\n{Rec.Description}";

      MessageBox.Show ( this, Details, "Update Details", MessageBoxButtons.OK,
                        MessageBoxIcon.Information );
    }

    private void LvHotfixes_DoubleClick ( object Sender, EventArgs E )
    {
      if ( lvHotfixes.SelectedItems.Count == 0 )
        return;
      if ( ! ( lvHotfixes.SelectedItems[ 0 ].Tag is Hotfix_Record Rec ) )
        return;

      string Details =
        $"HotFix ID: {Rec.HotFixID}\r\n" + $"Description: {Rec.Description}\r\n" +
        $"Installed On: {Rec.InstalledOn}\r\n" +
        $"Installed By: {(string.IsNullOrEmpty(Rec.InstalledBy) ? "(unknown)" : Rec.InstalledBy)}\r\n" +
        $"Status: {(string.IsNullOrEmpty(Rec.Status) ? "(none)" : Rec.Status)}\r\n" +
        $"Reference: {(string.IsNullOrEmpty(Rec.Caption) ? "(none)" : Rec.Caption)}\r\n\r\n" +
        $"Comments:\r\n{(string.IsNullOrEmpty(Rec.FixComments) ? "(none)" : Rec.FixComments)}";

      MessageBox.Show ( this, Details, "Hotfix Details", MessageBoxButtons.OK,
                        MessageBoxIcon.Information );
    }
    private void Set_Message ( string Text )
    {
      Message_Textbox.Text = Text;
    }
    private void Clear_Message ()
    {
      Message_Textbox.Text = "";
    }
  }
}
