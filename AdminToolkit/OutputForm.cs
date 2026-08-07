using System.Drawing.Printing;
using System.Text;

namespace Admin_Tools
{
  public partial class OutputForm : Form
  {
    // Print state
    private string[ ] _PrintLines;
    private int                        _PrintLineIndex;

    // Live log-tail state
    private System.Windows.Forms.Timer _TailTimer;
    private string                     _TailPath;
    private long                       _TailPos;

    public OutputForm ()
    {
      InitializeComponent ();
    }

    /// <summary>Replace the window content and title (static, one-shot).</summary>
    public void ShowOutput ( string Title, string Content )
    {
      StopLive (); // leaving live mode if we were in it
      Text                      = Title;
      txtOutput.Text            = Content;
      txtOutput.SelectionStart  = 0;
      txtOutput.SelectionLength = 0;
    }

    /// <summary>
    /// Live mode: show the given file and keep appending new lines as they
    /// are written. Safe to call again to re-point at a different file.
    /// </summary>
    public void ShowLiveLog ( string Title, string Path )
    {
      Text      = Title;
      _TailPath = Path;
      _TailPos  = 0;
      txtOutput.Clear ();

      ReadNewText (); // initial fill

      if ( _TailTimer == null )
      {
        _TailTimer           = new System.Windows.Forms.Timer ();
        _TailTimer.Interval  = 1000; // poll once a second
        _TailTimer.Tick += ( S, E ) => ReadNewText ();
      }
      _TailTimer.Start ();
    }

    /// <summary>Read whatever has been appended since the last read.</summary>
    private void ReadNewText ()
    {
      if ( string.IsNullOrEmpty ( _TailPath ) )
        return;
      try
      {
        if ( ! File.Exists ( _TailPath ) )
          return;

        // ReadWrite share so we never block the Logger from writing.
        using ( var File_Stream = new FileStream ( _TailPath, FileMode.Open, FileAccess.Read,
                                                   FileShare.ReadWrite ) )
        {
          if ( File_Stream.Length < _TailPos )
            _TailPos = 0; // file rotated/shrank
          File_Stream.Seek ( _TailPos, SeekOrigin.Begin );

          using ( var Stream_Reader = new StreamReader ( File_Stream, Encoding.UTF8 ) )
          {
            string Chunk = Stream_Reader.ReadToEnd ();
            _TailPos     = File_Stream.Length;
            if ( ! string.IsNullOrEmpty ( Chunk ) )
              txtOutput.AppendText ( Chunk ); // AppendText auto-scrolls
          }
        }
      }
      catch
      {
        // File briefly locked mid-write - just try again next tick.
      }
    }

    private void StopLive ()
    {
      if ( _TailTimer != null )
        _TailTimer.Stop ();
      _TailPath = null;
    }

    protected override void OnFormClosed ( FormClosedEventArgs E )
    {
      if ( _TailTimer != null )
      {
        _TailTimer.Stop ();
        _TailTimer.Dispose ();
        _TailTimer = null;
      }
      base.OnFormClosed ( E );
    }

    private void BtnClose_Click ( object Sender, EventArgs E )
    {
      Close ();
    }

    private void BtnPrint_Click ( object Sender, EventArgs E )
    {
      using ( var PrintDoc = new PrintDocument () ) using ( var Dialog = new PrintDialog () )
      {
        PrintDoc.DocumentName  = Text;
        PrintDoc.PrintPage    += PrintDoc_PrintPage;
        Dialog.Document        = PrintDoc;
        Dialog.UseEXDialog     = true;

        if ( Dialog.ShowDialog ( this ) == DialogResult.OK )
        {
          _PrintLines     = txtOutput.Text.Replace ( "\r\n", "\n" ).Split ( '\n' );
          _PrintLineIndex = 0;
          try
          {
            PrintDoc.Print ();
          }
          catch ( Exception Ex )
          {
            MessageBox.Show ( this, "Print failed:\n\n" + Ex.Message, "Print", MessageBoxButtons.OK,
                              MessageBoxIcon.Error );
          }
        }
      }
    }

    private void PrintDoc_PrintPage ( object Sender, PrintPageEventArgs E )
    {
      using ( var Font = new Font ( "Consolas", 9f ) )
      {
        float Line_Height = Font.GetHeight ( E.Graphics );
        float Y          = E.MarginBounds.Top;

        while ( _PrintLineIndex < _PrintLines.Length && Y + Line_Height <= E.MarginBounds.Bottom )
        {
          E.Graphics.DrawString ( _PrintLines[ _PrintLineIndex ], Font, Brushes.Black,
                                  E.MarginBounds.Left, Y );
          Y += Line_Height;
          _PrintLineIndex++;
        }

        E.HasMorePages = _PrintLineIndex < _PrintLines.Length;
      }
    }

    private void Purge_Button_Click ( object Sender, EventArgs E )
    {

      if ( string.IsNullOrEmpty ( _TailPath ) || ! File.Exists ( _TailPath ) )
        return;

      var Answer = MessageBox.Show ( this, "Clear the entire log file?\n\nThis cannot be undone.",
                                     "Purge Log", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                                     MessageBoxDefaultButton.Button2 ); // default = No

      if ( Answer != DialogResult.Yes )
        return;

      try
      {
        // Truncate to zero bytes. ReadWrite share matches how the
        // tail reads, so the Logger can keep its handle open.
        using ( var File_Stream = new FileStream ( _TailPath, FileMode.Open, FileAccess.Write,
                                          FileShare.ReadWrite ) )
        {
          File_Stream.SetLength ( 0 );
        }

        txtOutput.Clear ();
        _TailPos = 0; // tail restarts from the top of the (empty) file
      }
      catch ( IOException Ex )
      {
        MessageBox.Show ( this, "Could not purge the log file:\n\n" + Ex.Message, "Purge",
                          MessageBoxButtons.OK, MessageBoxIcon.Error );
      }
    }
  }
}
