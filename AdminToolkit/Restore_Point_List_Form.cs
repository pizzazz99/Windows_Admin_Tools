// ============================================================
//  Restore_Point_List_Form.cs   (.NET 10 / WinForms)
//  Designer-based version. Pairs with
//  Restore_Point_List_Form.Designer.cs — all controls live
//  there; this file is logic only.
//
//  Usage from MainForm (modeless):
//      var f = new Restore_Point_List_Form();
//      f.Show(this);
// ============================================================

using System.Diagnostics;
using System.Globalization;
using System.Management;
using System.Text;
using Microsoft.Win32;

namespace Admin_Tools
{
  public partial class Restore_Point_List_Form : Form
  {
    private List<Restore_Point_Info>             _Points = new List<Restore_Point_Info> ();

    // DisplayName -> InstallDate (day precision), read once per Load_Points
    private List<KeyValuePair<string, DateTime>> _Installed_Programs =
      new List<KeyValuePair<string, DateTime>> ();

    // Symlinks created by "Browse Files" — removed when the form closes
    private readonly List<string> _Snapshot_Links = new List<string> ();

    public Restore_Point_List_Form ()
    {
      InitializeComponent ();
      lvPoints.MultiSelect = true;
      Load_Points ();
    }

    // --------------------------------------------------------
    //  Data load — sorted OLDEST first
    // --------------------------------------------------------
    private void Load_Points ()
    {
      Cursor = Cursors.WaitCursor;
      try
      {
        _Points = Restore_Point_Manager.Get_Restore_Points ();

        // Manager returns newest-first; flip to oldest-first
        _Points.Reverse ();

        _Installed_Programs = Load_Installed_Programs ();

        lvPoints.BeginUpdate ();
        lvPoints.Items.Clear ();

        foreach ( var Restore_Point in _Points )
        {
          double Age_Days = ( DateTime.Now - Restore_Point.Creation_Time ).TotalDays;

          var    Item     = new ListViewItem ( Restore_Point.Sequence_Number.ToString () );
          Item.SubItems.Add ( Restore_Point.Creation_Time.ToString ( "yyyy-MM-dd HH:mm:ss" ) );
          Item.SubItems.Add ( Age_Days.ToString ( "0.0" ) );
          Item.SubItems.Add ( Restore_Point.Type_Name );
          Item.SubItems.Add ( Restore_Point.Event_Name );
          Item.SubItems.Add ( Restore_Point.Description );
          Item.SubItems.Add ( Restore_Point.Linked_Shadow_Id != null ? "Linked" : "—" );
          Item.Tag = Restore_Point;

          // Visual grouping hints
          if ( Restore_Point.Restore_Point_Type == 10 ) // driver install
            Item.ForeColor = Color.DarkBlue;
          else if ( Restore_Point.Restore_Point_Type == 0 ||
                    Restore_Point.Restore_Point_Type == 1 )  // app install/uninstall
            Item.ForeColor = Color.DarkGreen;
          else if ( Restore_Point.Restore_Point_Type == 13 ) // cancelled
            Item.ForeColor = Color.Gray;

          lvPoints.Items.Add ( Item );
        }

        lvPoints.EndUpdate ();

        lblSummary.Text = _Points.Count == 0
                            ? "No restore points found (System Protection may be off for the " +
                              "system drive)."
                            : _Points.Count + " restore point(s)   |   oldest: " +
                                _Points[ 0 ].Creation_Time.ToString ( "yyyy-MM-dd HH:mm" ) +
                                "   |   newest: " +
                                _Points[ _Points.Count - 1 ].Creation_Time.ToString ( "yyyy-MM-" +
                                                                                      "dd HH:mm" ) +
                                Deleted_Summary_Text ();

        Update_Status_Line ();
        Clear_Details ();

        // Auto-select newest (last row) so details aren't empty
        if ( lvPoints.Items.Count > 0 )
        {
          var Last      = lvPoints.Items[ lvPoints.Items.Count - 1 ];
          Last.Selected = true;
          Last.EnsureVisible ();
        }
      }
      catch ( Exception Ex )
      {
        lblSummary.Text = "Query failed.";
        Clear_Details ();
        txtNotes.Text = "WMI query failed: " + Ex.Message +
                        "  Make sure the app is running as Administrator.";
      }
      finally
      {
        Cursor = Cursors.Default;
      }
    }

    private void Clear_Details ()
    {
      txtSeq.Text         = "";
      txtCreated.Text     = "";
      txtAge.Text         = "";
      txtDescription.Text = "";
      txtType.Text        = "";
      txtEvent.Text       = "";
      txtShadowId.Text    = "";
      txtDevice.Text      = "";
      txtNotes.Text       = "";
    }

    // --------------------------------------------------------
    //  Selection -> details fields
    // --------------------------------------------------------
    private void Lv_Selection_Changed ( object Sender, EventArgs E )
    {
      if ( lvPoints.SelectedItems.Count == 0 )
        return;

      var Restore_Point = lvPoints.SelectedItems[ 0 ].Tag as Restore_Point_Info;
      if ( Restore_Point == null )
        return;

      var Age             = DateTime.Now - Restore_Point.Creation_Time;

      txtSeq.Text         = Restore_Point.Sequence_Number.ToString ();
      txtCreated.Text     = Restore_Point.Creation_Time.ToString ( "yyyy-MM-dd HH:mm:ss" );
      txtAge.Text         = Age.TotalDays.ToString ( "0.0" );
      txtDescription.Text = Restore_Point.Description;
      txtType.Text  = Restore_Point.Type_Name + "  (" + Restore_Point.Restore_Point_Type + ")";
      txtEvent.Text = Restore_Point.Event_Name + "  (" + Restore_Point.Event_Type + ")";

      if ( Restore_Point.Linked_Shadow_Id != null )
      {
        txtShadowId.Text = Restore_Point.Linked_Shadow_Id;
        txtDevice.Text   = Restore_Point.Linked_Device;
      }
      else
      {
        txtShadowId.Text = "None found";
        txtDevice.Text   = "Shadow copy may have been aged out (deleted) " + "while the restore " +
                                                                           "point metadata " +
                                                                           "remains.";
      }

      // Notes: type hint + gap info + what restoring would remove
      var String_Builder = new StringBuilder ();
      String_Builder.AppendLine ( Type_Hint ( Restore_Point.Restore_Point_Type ) );

      string Gap = Gap_Text ( Restore_Point );
      if ( Gap.Length > 0 )
        String_Builder.AppendLine ( Gap );

      string Removed = Installed_After_Text ( Restore_Point );
      if ( Removed.Length > 0 )
        String_Builder.AppendLine ( Removed );

      txtNotes.Text = String_Builder.ToString ().TrimEnd ();
    }

    private static string Type_Hint ( uint T )
    {
      switch ( T )
      {
        case 0 :
          return "Created by an application installer before making changes. " + "The " +
                                                                                 "description is " +
                                                                                 "whatever name " +
                                                                                 "the installer " +
                                                                                 "passed in.";
        case 1 :
          return "Created by an uninstaller before removing an application.";
        case 6 :
          return "Created automatically before a System Restore operation ran, " + "so the " +
                                                                                   "restore " +
                                                                                   "itself can " +
                                                                                   "be undone.";
        case 7 :
          return "A checkpoint - either the scheduled automatic one or one " + "created manually " +
                                                                               "via System " +
                                                                               "Protection.";
        case 10 :
          return "Created by Windows before installing a device driver.";
        case 12 :
          return "Created before a system settings change.";
        case 13 :
          return "The operation that created this point was cancelled before completing.";
        case 14 :
          return "Created by a backup/recovery operation.";
        default :
          return "No additional notes for this type.";
      }
    }

    // --------------------------------------------------------
    //  Extra data: gaps, installed-after, status line
    // --------------------------------------------------------

    /// <summary>Note when sequence numbers immediately before the
    /// selected point are missing (they were deleted or aged out —
    /// Windows never reuses sequence numbers).</summary>
    private string Gap_Text ( Restore_Point_Info Rp )
    {
      int Idx = _Points.IndexOf ( Rp );
      if ( Idx <= 0 )
        return "";

      uint Prev = _Points[ Idx - 1 ].Sequence_Number;
      uint Cur  = Rp.Sequence_Number;
      if ( Cur - Prev <= 1 )
        return "";

      uint Missing = Cur - Prev - 1;
      return Missing == 1
               ? "Gap: point #" + ( Prev + 1 ) + " no longer exists (deleted or aged out)."
               : "Gap: points #" + ( Prev + 1 ) + " through #" + ( Cur - 1 ) + " (" + Missing +
                   " total) no longer exist (deleted or aged out).";
    }

    private string Deleted_Summary_Text ()
    {
      uint Missing = 0;
      for ( int I = 1; I < _Points.Count; I++ )
        Missing += _Points[ I ].Sequence_Number - _Points[ I - 1 ].Sequence_Number - 1;

      return Missing == 0 ? "" : "   |   " + Missing + " deleted in this range";
    }

    /// <summary>Programs whose registry InstallDate is on/after the
    /// point's date — i.e. what a rollback to this point would remove.
    /// InstallDate is day-granular and some installers omit it, so
    /// this is best-effort.</summary>
    private string Installed_After_Text ( Restore_Point_Info Rp )
    {
      if ( _Installed_Programs.Count == 0 )
        return "";

      var After = _Installed_Programs.Where ( P => P.Value.Date >= Rp.Creation_Time.Date )
                    .OrderBy ( P => P.Value )
                    .ToList ();

      if ( After.Count == 0 )
        return "Restoring to this point would remove no currently installed programs " + "(per " +
                                                                                         "registr" +
                                                                                         "y " +
                                                                                         "install" +
                                                                                         " dates).";

      const int Max_Shown      = 12;
      var       String_Builder = new StringBuilder ();
      String_Builder.Append ( "Restoring to this point would remove (installed on/after its " +
                              "date, day precision): " );
      String_Builder.Append (
        string.Join ( ", ", After.Take ( Max_Shown ).Select ( P => P.Key ) ) );
      if ( After.Count > Max_Shown )
        String_Builder.Append ( ", and " + ( After.Count - Max_Shown ) + " more" );
      String_Builder.Append ( "." );
      return String_Builder.ToString ();
    }

    private static List<KeyValuePair<string, DateTime>> Load_Installed_Programs ()
    {
      var Result = new List<KeyValuePair<string, DateTime>> ();
      try
      {
        Read_Uninstall_Key ( Registry.LocalMachine,
                             @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", Result );
        Read_Uninstall_Key ( Registry.LocalMachine,
                             @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall",
                             Result );
        Read_Uninstall_Key ( Registry.CurrentUser,
                             @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", Result );
      }
      catch
      {
        // best-effort; an unreadable hive just means a shorter list
      }
      return Result;
    }

    private static void Read_Uninstall_Key ( RegistryKey Root, string Path,
                                             List<KeyValuePair<string, DateTime>> Result )
    {
      using ( var Key = Root.OpenSubKey ( Path ) )
      {
        if ( Key == null )
          return;

        foreach ( var Sub_Name in Key.GetSubKeyNames () )
        {
          using ( var App = Key.OpenSubKey ( Sub_Name ) )
          {
            var Name     = App?.GetValue ( "DisplayName" ) as string;
            var Date_Str = App?.GetValue ( "InstallDate" ) as string;
            if ( string.IsNullOrEmpty ( Name ) || string.IsNullOrEmpty ( Date_Str ) )
              continue;

            if ( DateTime.TryParseExact ( Date_Str, "yyyyMMdd", CultureInfo.InvariantCulture,
                                          DateTimeStyles.None, out DateTime Installed ) )
            {
              Result.Add ( new KeyValuePair<string, DateTime> ( Name, Installed ) );
            }
          }
        }
      }
    }

    /// <summary>Second header line: protection / throttle / shadow storage.</summary>
    private void Update_Status_Line ()
    {
      string Protection = Restore_Point_Creator.Is_System_Restore_Enabled () ? "On" : "OFF";

      int    Freq       = Restore_Point_Creator.Get_Creation_Frequency_Minutes ();
      string Throttle   = Freq == 0 ? "disabled (every request honored)"
                                    : ( Freq / 60.0 ).ToString ( "0.#" ) + " hour window";

      lblStatus.Text    = "Protection: " + Protection + "   |   Creation throttle: " + Throttle +
                          "   |   " + Get_Shadow_Storage_Summary ();
    }

    private static string Get_Shadow_Storage_Summary ()
    {
      try
      {
        var Scope = new ManagementScope ( @"\\.\root\cimv2" );
        Scope.Connect ();

        ulong Used = 0, Max = 0;
        bool  Found = false, Unbounded = false;

        using ( var Searcher = new ManagementObjectSearcher ( Scope, new ObjectQuery ( "SELECT " +
                                                                                       "UsedSpace" +
                                                                                       ", " +
                                                                                       "MaxSpace " +
                                                                                       "FROM " +
                                                                                       "Win32_" +
                                                                                       "ShadowSto" +
                                                                                       "rage" ) ) )
        {
          foreach ( ManagementObject Mo in Searcher.Get () )
          {
            Found    = true;
            Used    += (ulong) Mo[ "UsedSpace" ];

            ulong M  = (ulong) Mo[ "MaxSpace" ];
            if ( M == ulong.MaxValue )
              Unbounded = true;
            else
              Max += M;
          }
        }

        if ( ! Found )
          return "Shadow storage: none allocated";

        string Max_Text = Unbounded ? "unbounded" : Format_GB ( Max );
        return "Shadow storage: " + Format_GB ( Used ) + " used / " + Max_Text + " max";
      }
      catch
      {
        return "Shadow storage: unavailable";
      }
    }

    private static string Format_GB ( ulong Bytes )
    {
      return ( Bytes / 1073741824.0 ).ToString ( "0.0" ) + " GB";
    }

    // --------------------------------------------------------
    //  Browse the snapshot's file system (read-only)
    // --------------------------------------------------------
    private void Btn_Browse_Click ( object Sender, EventArgs E )
    {
      if ( lvPoints.SelectedItems.Count == 0 )
      {
        MessageBox.Show ( this, "Select a restore point first.", "Browse Snapshot",
                          MessageBoxButtons.OK, MessageBoxIcon.Information );
        return;
      }

      var Rp = lvPoints.SelectedItems[ 0 ].Tag as Restore_Point_Info;
      if ( Rp == null )
        return;

      if ( Rp.Linked_Device == null )
      {
        MessageBox.Show ( this,
                          "This restore point has no linked shadow copy, so there is " + "no " +
                                                                                         "snapsho" +
                                                                                         "t file " +
                                                                                         "system " +
                                                                                         "to " +
                                                                                         "browse.",
                          "Browse Snapshot", MessageBoxButtons.OK, MessageBoxIcon.Warning );
        return;
      }

      string Link_Path = Path.Combine ( Path.GetTempPath (), "RestorePoint_" + Rp.Sequence_Number );

      try
      {
        if ( ! Directory.Exists ( Link_Path ) )
        {
          // Target must end with a backslash or the link won't browse.
          Directory.CreateSymbolicLink ( Link_Path, Rp.Linked_Device + @"\" );
          _Snapshot_Links.Add ( Link_Path );
        }

        Process.Start ( "explorer.exe", Link_Path );
        lblSummary.Text = "Snapshot #" + Rp.Sequence_Number + " mounted read-only at " + Link_Path +
                          " — the link is removed when this window closes.";
      }
      catch ( Exception Ex )
      {
        MessageBox.Show ( this, "Could not mount the snapshot:\n" + Ex.Message, "Browse Snapshot",
                          MessageBoxButtons.OK, MessageBoxIcon.Error );
      }
    }

    protected override void OnFormClosed ( FormClosedEventArgs E )
    {
      // Remove any snapshot symlinks we created. Deleting a directory
      // symlink removes only the link, never the snapshot behind it.
      foreach ( var Link in _Snapshot_Links )
      {
        try
        {
          if ( Directory.Exists ( Link ) )
            Directory.Delete ( Link, false );
        }
        catch
        {
          // stale link in %TEMP% is harmless; ignore
        }
      }
      base.OnFormClosed ( E );
    }

    // --------------------------------------------------------
    //  Buttons
    // --------------------------------------------------------
    private void Btn_Refresh_Click ( object Sender, EventArgs E )
    {
      Load_Points ();
    }

    private void Btn_Copy_Click ( object Sender, EventArgs E )
    {
      if ( txtSeq.Text.Length == 0 )
        return;

      var Sb = new StringBuilder ();
      Sb.AppendLine ( "RESTORE POINT DETAILS" );
      Sb.AppendLine ( new string ( '=', 55 ) );
      Sb.AppendLine ( "Sequence #    : " + txtSeq.Text );
      Sb.AppendLine ( "Created       : " + txtCreated.Text );
      Sb.AppendLine ( "Age           : " + txtAge.Text + " days" );
      Sb.AppendLine ( "Description   : " + txtDescription.Text );
      Sb.AppendLine ( "Type          : " + txtType.Text );
      Sb.AppendLine ( "Event         : " + txtEvent.Text );
      Sb.AppendLine ( "Shadow ID     : " + txtShadowId.Text );
      Sb.AppendLine ( "Device        : " + txtDevice.Text );
      Sb.AppendLine ( "Notes         : " + txtNotes.Text );
      Sb.AppendLine ( "Status        : " + lblStatus.Text );

      Clipboard.SetText ( Sb.ToString () );
      lblSummary.Text = "Details copied to clipboard.";
    }

    private void Btn_Close_Click ( object Sender, EventArgs E )
    {
      Close ();
    }

    private void Create_Restore_Point_Button_Click ( object Sender, EventArgs E )
    {
      Create_Button_Click ( Sender, E );
    }

    private void Delete_Selected_Restore_Point_Button_Click ( object Sender, EventArgs E )
    {
      Delete_Selected_Button_Click ( Sender, E );
    }
  }
}
