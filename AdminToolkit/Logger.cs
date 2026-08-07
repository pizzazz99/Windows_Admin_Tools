using System;
using System.IO;
using System.Text;

namespace Admin_Tools
{
  /// <summary>
  /// Simple thread-safe file logger.
  ///
  /// Writes timestamped lines to:
  ///     %ProgramData%\AdminToolkit\log-{MachineName}-{yyyy-MM-dd}.txt
  ///
  /// One file per machine per day, so when you bounce between remote PCs
  /// each box keeps its own running record. Logging never throws - any
  /// IO problem is swallowed so it can't take the app down.
  /// </summary>
  internal static class Logger
  {
    private static readonly object _gate = new object();

    private static readonly string _dir = Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.CommonApplicationData ), "AdminToolkit" );

    /// <summary>Full path to today's log file for this machine.</summary>
    public static string CurrentLogPath
    {
      get {
        string name = string.Format( "log-{0}-{1}.txt", Environment.MachineName, DateTime.Now.ToString( "yyyy-MM-dd" ) );
        return Path.Combine( _dir, name );
      }
    }

    /// <summary>
    /// Append one timestamped entry. Category is a short tag such as
    /// "Snapshot", "Launch", "Kill", "App".
    /// </summary>
    public static void Log( string category, string message )
    {
      try
      {
        lock ( _gate )
        {
          Directory.CreateDirectory( _dir );
          string line = string.Format( "{0:yyyy-MM-dd HH:mm:ss}  [{1,-9}]  {2}  ({3}\\{4})", DateTime.Now, category, message, Environment.UserDomainName, Environment.UserName );
          File.AppendAllText( CurrentLogPath, line + Environment.NewLine, Encoding.UTF8 );
        }
      }
      catch
      {
        // Logging must never crash the app - ignore IO errors
        // (disk full, permissions, path locked, etc.)
      }
    }

    /// <summary>Open the log folder in Explorer (for a "View Log" button).</summary>
    public static void OpenLogFolder()
    {
      try
      {
        Directory.CreateDirectory( _dir );
        System.Diagnostics.Process.Start( "explorer.exe", "\"" + _dir + "\"" );
      }
      catch
      {
      }
    }

    public static void LogSessionStart( bool elevated )
    {
      try
      {
        lock ( _gate )
        {
          Directory.CreateDirectory( _dir );
          var sb = new StringBuilder();

          sb.AppendLine();
          sb.AppendLine( new string( '=', 70 ) );
          sb.AppendLine(
            string
              .Format( "SESSION START  {0:yyyy-MM-dd HH:mm:ss}  {1}  ({2}\\{3})  elevated={4}", DateTime.Now, Environment.MachineName, Environment.UserDomainName, Environment.UserName, elevated ) );
          sb.AppendLine( new string( '=', 70 ) );
          File.AppendAllText( CurrentLogPath, sb.ToString(), Encoding.UTF8 );
        }
      }
      catch
      {
      }
    }

    /// <summary>Write a blank separator line (skipped if the file is new/empty).</summary>
    public static void BlankLine()
    {
      try
      {
        lock ( _gate )
        {
          Directory.CreateDirectory( _dir );
          // Only separate if there's already something above.
          if ( File.Exists( CurrentLogPath ) && new FileInfo( CurrentLogPath ).Length > 0 )
          {
            File.AppendAllText( CurrentLogPath, Environment.NewLine, Encoding.UTF8 );
          }
        }
      }
      catch
      {
      }
    }
  }
}
