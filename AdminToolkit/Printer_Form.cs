// ============================================================
//  Printer_Form.cs   (C# 7.3 / .NET Framework)
//  Dedicated window for printer management:
//    - list installed printers with live connection status
//    - per-printer detail view (WMI properties + driver + port)
//    - ink/toner supply levels for network printers (SNMP)
//
//  Pairs with Printer_Form.Designer.cs.
//  Requires:  System.Management reference.
//
//  Usage from MainForm:
//      using (var f = new Printer_Form())
//          f.ShowDialog(this);
// ============================================================

using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Trace_Execution_Namespace;
using static Trace_Execution_Namespace.Trace_Execution;

namespace Admin_Tools
{
  public partial class Printer_Form : Form
  {
    public Printer_Form ()
    {
      InitializeComponent ();
      Trace_Execution.Initialize ( Start_Enabled: true );
      Load_Printers ();
    }

    // --------------------------------------------------------
    //  Printer list
    // --------------------------------------------------------
    private async void Load_Printers ()
    {
      using var Block = Trace_Block.Start_If_Enabled ();
      Cursor          = Cursors.WaitCursor;
      try
      {
        // Off the UI thread: WMI enumeration plus, for any WSD printer
        // Windows never cached an address for, a PowerShell shell-out
        // (up to ~4s). Keeping this off the UI thread stops it from
        // freezing the app and lets the trace window paint entries as
        // they actually happen instead of in one burst once this
        // returns.
        var all      = await Task.Run ( () => Printer_Support.Get_Installed_Printers () );
        var printers = chkHideVirtual.Checked ? all.Where ( p => ! p.IsVirtual ).ToList () : all;

        lvPrinters.BeginUpdate ();
        lvPrinters.Items.Clear ();

        foreach ( var p in printers )
        {
          var item = new ListViewItem ( p.Name );
          item.SubItems.Add ( p.StatusText );
          item.SubItems.Add ( p.IpAddress != null ? "Checking..." : "N/A" );
          item.SubItems.Add ( p.IpAddress ?? "" );
          item.SubItems.Add ( p.IsDefault ? "Yes" : "" );
          item.SubItems.Add ( p.IsVirtual
                                ? "Virtual"
                                : ( p.IsNetwork ? "Network" : ( p.IsLocal ? "Local" : "Other" ) ) );
          item.SubItems.Add ( p.PortName );
          item.SubItems.Add ( p.DriverName );
          item.Tag = p;

          lvPrinters.Items.Add ( item );
        }

        lvPrinters.EndUpdate ();

        var def         = printers.FirstOrDefault ( x => x.IsDefault );
        int hiddenCount = all.Count - printers.Count;
        lblSummary.Text = printers.Count + " printer(s) installed" +
                          ( hiddenCount > 0 ? " (" + hiddenCount + " virtual hidden)" : "" ) +
                          "   |   Default: " + ( def != null ? def.Name : "(none)" );
      }
      catch ( Exception Ex )
      {
        lblSummary.Text = "Failed to load printers: " + Ex.Message;
      }
      finally
      {
        Cursor = Cursors.Default;
      }

      Refresh_Live_Status ();
    }

    // --------------------------------------------------------
    //  Live "Online"/"Offline" column — probed in the background
    //  so the list appears instantly and fills in as results
    //  arrive (see Printer_Support.Is_Online for the method).
    // --------------------------------------------------------
    private async void Refresh_Live_Status ()
    {
      using var Block = Trace_Block.Start_If_Enabled ();
      var       items = lvPrinters.Items.Cast<ListViewItem> ()
                          .Where ( i => ( i.Tag as Printer_Info )?.IpAddress != null )
                          .ToArray ();

      if ( items.Length == 0 )
        return;

      bool?[ ] online = await Task.Run (
        () =>
        {
          var result = new bool?[ items.Length ];
          Parallel.For ( 0, items.Length,
                         i =>
                         {
                           var info    = (Printer_Info) items[ i ].Tag;
                           result[ i ] = Printer_Support.Is_Online ( info.IpAddress );
                         } );
          return result;
        } );

      for ( int i = 0; i < items.Length; i++ )
      {
        if ( items[ i ].ListView == null )
          continue; // list was reloaded meanwhile

        var liveCell                       = items[ i ].SubItems[ 2 ];
        items[ i ].UseItemStyleForSubItems = false;

        if ( online[ i ] == true )
        {
          liveCell.Text      = "Online";
          liveCell.ForeColor = Color.Green;
        }
        else
        {
          liveCell.Text      = "Offline";
          liveCell.ForeColor = Color.Firebrick;
        }
      }
    }

    // --------------------------------------------------------
    //  Wake / Retry — repeated, longer-timeout connection
    //  attempts against the selected printer. See
    //  Printer_Support.Wake_And_Check for why this beats a
    //  Wake-on-LAN magic packet for actual printer hardware.
    // --------------------------------------------------------
    private async void Btn_Wake_Click ( object Sender, EventArgs e )
    {
      using var Block = Trace_Block.Start_If_Enabled ();
      var       p     = Selected ();
      if ( p == null )
      {
        MessageBox.Show ( "Select a printer first.", "Wake / Retry", MessageBoxButtons.OK,
                          MessageBoxIcon.Information );
        return;
      }

      if ( string.IsNullOrEmpty ( p.IpAddress ) )
      {
        MessageBox.Show ( p.Name + " has no resolvable network address to probe.",
                          "Wake / Retry", MessageBoxButtons.OK, MessageBoxIcon.Information );
        return;
      }

      var item                     = lvPrinters.SelectedItems[ 0 ];
      var liveCell                 = item.SubItems[ 2 ];
      item.UseItemStyleForSubItems = false;
      liveCell.Text                = "Waking...";
      liveCell.ForeColor           = Color.DarkOrange;

      BtnWake.Enabled              = false;
      Cursor                       = Cursors.WaitCursor;
      try
      {
        bool? online = await Task.Run ( () => Printer_Support.Wake_And_Check ( p.IpAddress ) );

        if ( online == true )
        {
          liveCell.Text      = "Online";
          liveCell.ForeColor = Color.Green;
        }
        else
        {
          liveCell.Text      = "Offline";
          liveCell.ForeColor = Color.Firebrick;
          MessageBox.Show ( p.Name + " did not respond after repeated attempts (~9 seconds). " +
                              ( "It may be fully powered off, on a different subnet/VLAN, or " +
                                "blocking " ) +
                              "every port this tool probes.",
                            "Wake / Retry", MessageBoxButtons.OK, MessageBoxIcon.Warning );
        }
      }
      finally
      {
        BtnWake.Enabled = true;
        Cursor          = Cursors.Default;
      }
    }

    private void ChkHideVirtual_CheckedChanged ( object Sender, EventArgs e )
    {
      using var Block = Trace_Block.Start_If_Enabled ();
      Load_Printers ();
    }

    private Printer_Info Selected ()
    {
      using var Block = Trace_Block.Start_If_Enabled ();
      if ( lvPrinters.SelectedItems.Count == 0 )
        return null;
      return lvPrinters.SelectedItems[ 0 ].Tag as Printer_Info;
    }

    // --------------------------------------------------------
    //  Refresh
    // --------------------------------------------------------
    private void Btn_Refresh_Click ( object Sender, EventArgs e )
    {
      using var Block = Trace_Block.Start_If_Enabled ();
      Load_Printers ();
    }

    // --------------------------------------------------------
    //  Extract info about the selected printer
    // --------------------------------------------------------
    private void Btn_Details_Click ( object Sender, EventArgs e )
    {
      using var Block = Trace_Block.Start_If_Enabled ();
      var       p     = Selected ();
      if ( p == null )
      {
        MessageBox.Show ( "Select a printer first.", "Printer Details", MessageBoxButtons.OK,
                          MessageBoxIcon.Information );
        return;
      }

      Cursor = Cursors.WaitCursor;
      string text;
      try
      {
        text = Printer_Support.Get_Printer_Details_Text ( p.Name );
      }
      catch ( Exception Ex )
      {
        text = "Failed to read printer details: " + Ex.Message;
      }
      finally
      {
        Cursor = Cursors.Default;
      }

      Show_Text_Window ( "Printer Details - " + p.Name, text );
    }

    // --------------------------------------------------------
    //  Ink / toner levels (SNMP — network printers only)
    // --------------------------------------------------------
    private async void Btn_Supplies_Click ( object Sender, EventArgs e )
    {
      using var Block = Trace_Block.Start_If_Enabled ();
      var       p     = Selected ();
      if ( p == null )
      {
        MessageBox.Show ( "Select a printer first.", "Ink / Toner Levels", MessageBoxButtons.OK,
                          MessageBoxIcon.Information );
        return;
      }

      BtnSupplies.Enabled    = false;
      Cursor                 = Cursors.WaitCursor;
      string previousSummary = lblSummary.Text;
      lblSummary.Text        = "Querying " + p.Name + " over SNMP...";

      try
      {
        var result = await Task.Run ( () => Printer_Support.Get_Supply_Levels ( p.IpAddress ) );

        if ( ! result.Success )
        {
          MessageBox.Show ( result.Error, "Ink / Toner Levels", MessageBoxButtons.OK,
                            MessageBoxIcon.Warning );
          return;
        }

        var sb = new StringBuilder ();
        sb.AppendLine ( "SUPPLY LEVELS - " + p.Name );
        sb.AppendLine ( "Queried " + p.IpAddress + " via SNMP (Printer-MIB)" );
        sb.AppendLine ( new string ( '=', 60 ) );

        foreach ( var s in result.Supplies )
        {
          // Separate lines rather than padding onto one — some
          // devices report long descriptions that would run
          // straight into the percentage/status with no gap.
          sb.AppendLine ( s.Description );
          sb.AppendLine ( "    " + ( s.Percent.HasValue ? s.Percent.Value + "%"
                                                        : ( s.RawNote ?? "Unknown" ) ) );
          sb.AppendLine ();
        }

        Show_Text_Window ( "Ink / Toner Levels - " + p.Name, sb.ToString () );
      }
      finally
      {
        BtnSupplies.Enabled = true;
        Cursor              = Cursors.Default;
        lblSummary.Text     = previousSummary;
      }
    }

    // --------------------------------------------------------
    //  Monospace text viewer (details / supply levels)
    // --------------------------------------------------------
    private void Show_Text_Window ( string title, string text )
    {
      using var Block = Trace_Block.Start_If_Enabled ();
      var form = new Form { Text = title,        Width = 700,
                            Height = 560,        StartPosition = FormStartPosition.CenterParent,
                            MinimizeBox = false, ShowInTaskbar = false };

      var txt  = new TextBox { Multiline  = true,
                               ReadOnly   = true,
                               ScrollBars = ScrollBars.Both,
                               WordWrap   = false,
                               Dock       = DockStyle.Fill,
                               Font       = new Font ( "Consolas", 10f ),
                               BackColor  = Color.White,
                               Text       = text };

      var btnCloseViewer    = new Button { Text = "Close", Width = 90 };
      btnCloseViewer.Click += delegate
      {
        form.Close ();
      };

      var panel = new FlowLayoutPanel { Dock          = DockStyle.Bottom,
                                        FlowDirection = FlowDirection.RightToLeft, Height = 45,
                                        Padding = new Padding ( 8 ) };
      panel.Controls.Add ( btnCloseViewer );

      form.Controls.Add ( txt );
      form.Controls.Add ( panel );
      form.AcceptButton  = btnCloseViewer;
      form.Shown        += delegate
      {
        txt.SelectionStart  = 0;
        txt.SelectionLength = 0;
      };

      form.Show ( this );
    }

    private void Btn_Close_Click ( object Sender, EventArgs e )
    {

      using var Block = Trace_Block.Start_If_Enabled ();
      Close ();
    }
  }
}
