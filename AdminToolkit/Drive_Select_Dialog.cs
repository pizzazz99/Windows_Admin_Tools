using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

public class DriveChoice
{
  public string          Name;                  // "C:\"
  public string          Display;               // "C:  Windows  (512 GB)  — already active"
  public bool            Checked;
  public override string ToString() => Display; // what the checklist shows
}

public class DriveSelectDialog : Form
{
  private readonly CheckedListBox _list                                = new CheckedListBox();
  public List<string>             SelectedDrives { get; private set; } = new List<string>();

  public DriveSelectDialog( IEnumerable<DriveChoice> choices, string title = "Select Drives to Shadow", string headerText = "Choose which drives to protect with shadow copies:" )
  {
    Text            = title;
    FormBorderStyle = FormBorderStyle.FixedDialog;
    StartPosition   = FormStartPosition.CenterParent;
    MinimizeBox     = false;
    MaximizeBox     = false;
    ClientSize      = new Size( 440, 300 );

    var header = new Label { Text    = headerText, // was: the hardcoded protect string
                             Dock    = DockStyle.Top,
                             Height  = 34,
                             Padding = new Padding( 12, 12, 12, 0 ) };

    _list.Dock           = DockStyle.Fill;
    _list.CheckOnClick   = true;
    _list.IntegralHeight = false;
    foreach ( var c in choices )
      _list.Items.Add( c, c.Checked );

    var host = new Panel { Dock = DockStyle.Fill, Padding = new Padding( 12, 4, 12, 4 ) };
    host.Controls.Add( _list );

    var ok      = new Button { Text = "OK", DialogResult = DialogResult.OK, Width = 80 };
    var cancel  = new Button { Text = "Cancel", DialogResult = DialogResult.Cancel, Width = 80 };
    ok.Click += ( s, E ) => SelectedDrives = _list.CheckedItems.Cast<DriveChoice>().Select( c => c.Name ).ToList();

    var buttons = new FlowLayoutPanel { Dock = DockStyle.Bottom, FlowDirection = FlowDirection.RightToLeft, Height = 46, Padding = new Padding( 12, 8, 12, 8 ) };
    buttons.Controls.Add( ok );
    buttons.Controls.Add( cancel );

    Controls.Add( host );
    Controls.Add( header );
    Controls.Add( buttons );
    AcceptButton = ok;
    CancelButton = cancel;
  }
}
