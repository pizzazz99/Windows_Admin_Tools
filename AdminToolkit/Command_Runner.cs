using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Admin_Tools
{
  /// <summary>
  /// Runs console commands and appends their output to a text control.
  ///
  /// Usage from your form:
  ///     private void BtnNetUser_Click(object Sender, EventArgs e)
  ///         => Command_Runner.Run_Command("net user", txtOutput);
  /// </summary>
  public static class Command_Runner
  {

    public static void Cursor_Wait()
    {
      Cursor.Current = Cursors.WaitCursor;
    }

    public static void Cursor_Default()
    {
      Cursor.Current = Cursors.Default;
    }

    /// <summary>Runs a console command and appends its output to
    /// the given control. Non-blocking; the UI stays responsive.</summary>
    public static async Task Run_Command_Async( string command, System.Windows.Forms.RichTextBox output )
    {
      string result = await Execute_Async( command );
      output.Text   = result;
    }

    /// <summary>Runs the command via cmd.exe and returns combined
    /// stdout + stderr. Runs on a background thread.</summary>
    private static async Task<string> Execute_Async( string command )
    {
      var psi = new ProcessStartInfo( "cmd.exe", "/c " + command ) { UseShellExecute        = false,
                                                                     RedirectStandardOutput = true,
                   RedirectStandardError  = true,
                                                                     CreateNoWindow         = true,
                                                                     StandardOutputEncoding = Encoding.UTF8,
                                                                     StandardErrorEncoding  = Encoding.UTF8 };

      Cursor_Wait();
      try
      {
        using ( var p = Process.Start( psi ) )
        {
          // --- exit plumbing: subscribe before the process can slip away ---
          p.EnableRaisingEvents  = true;
          var exited             = new TaskCompletionSource<bool>();
          p.Exited += ( s, E ) => exited.TrySetResult( true );
          if ( p.HasExited )
            exited.TrySetResult( true ); // already gone? cover the race

          // --- start both reads before awaiting either ---
          Task<string> outTask = p.StandardOutput.ReadToEndAsync();
          Task<string> errTask = p.StandardError.ReadToEndAsync();

          string outText = await outTask;
          string errText = await errTask;

          await                  exited.Task;

          return errText.Length > 0 ? outText + Environment.NewLine + errText : outText;
        }
      }
      finally
      {
        Cursor_Default();
      }
    }

    /// <summary>Thread-safe append + autoscroll.</summary>
    private static void Append_Output( TextBoxBase output, string text )
    {
      if ( output.IsDisposed )
        return;

      if ( output.InvokeRequired )
      {
        output.BeginInvoke( (Action) ( () => Append_Output( output, text ) ) );
        return;
      }
      output.AppendText( text );
    }
  }
}
