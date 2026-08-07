using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Admin_Tools
{
  /// <summary>
  /// Sends email via SMTP (System.Net.Mail - no NuGet packages on .NET 4.8).
  ///
  /// Settings are split so no secret is ever stored in plain text:
  ///   %ProgramData%\AdminToolkit\email.settings  - host/port/user/to (plain, hand-editable)
  ///   %ProgramData%\AdminToolkit\email.secret     - password, DPAPI-encrypted
  ///
  /// Password encryption uses Windows DPAPI (via Dpapi.cs -> crypt32.dll), so
  /// no reference to System.Security.dll is required. At CurrentUser scope the
  /// stored blob only decrypts for the same user account on the same machine.
  /// </summary>
  internal static class Emailer
  {
    // false = CurrentUser scope (tied to this Windows account; safer on
    // shared machines). Set true for LocalMachine scope (any admin on the
    // box can decrypt). Must match between Save and Load.
    private const bool             MachineScope = false;

    private static readonly string _dir = Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.CommonApplicationData ), "AdminToolkit" );

    private static readonly string _settingsPath = Path.Combine( _dir, "email.settings" );
    private static readonly string _secretPath   = Path.Combine( _dir, "email.secret" );

    public static string SettingsPath
    {
      get {
        return _settingsPath;
      }
    }

    public class SmtpConfig
    {
      public string Host      = "";
      public int    Port      = 587;
      public bool   EnableSsl = true;
      public string User      = "";
      public string Password  = ""; // filled from the encrypted store
      public string From      = "";
      public string To        = "";

      /// <summary>Non-secret server fields are present.</summary>
      public bool ServerReady
      {
        get {
          return ! string.IsNullOrEmpty( Host ) && ! string.IsNullOrEmpty( User ) && ! string.IsNullOrEmpty( To );
        }
      }

      /// <summary>Everything, including the password, is ready to send.</summary>
      public bool IsComplete
      {
        get {
          return ServerReady && ! string.IsNullOrEmpty( Password );
        }
      }
    }

    /// <summary>
    /// Read settings (writes a template if the file is missing) and pull the
    /// decrypted password from the secret store.
    /// </summary>
    public static SmtpConfig LoadConfig()
    {
      var cfg = new SmtpConfig();
      try
      {
        if ( ! File.Exists( _settingsPath ) )
        {
          WriteTemplate();
        }
        else
        {
          foreach ( string raw in File.ReadAllLines( _settingsPath ) )
          {
            string line = raw.Trim();
            if ( line.Length == 0 || line.StartsWith( "#" ) )
              continue;

            int eq = line.IndexOf( '=' );
            if ( eq <= 0 )
              continue;

            string key = line.Substring( 0, eq ).Trim().ToLowerInvariant();
            string val = line.Substring( eq + 1 ).Trim();

            switch ( key )
            {
              case "host" :
                cfg.Host = val;
                break;
              case "port" :
                int.TryParse( val, out cfg.Port );
                break;
              case "enablessl" :
                bool.TryParse( val, out cfg.EnableSsl );
                break;
              case "user" :
                cfg.User = val;
                break;
              case "from" :
                cfg.From = val;
                break;
              case "to" :
                cfg.To = val;
                break;
                // NOTE: no "password" here - it lives in the encrypted store.
            }
          }
        }

        if ( string.IsNullOrEmpty( cfg.From ) )
          cfg.From = cfg.User;
        cfg.Password = LoadPassword();
      }
      catch
      { /* return whatever we managed to parse */
      }
      return cfg;
    }

    private static void WriteTemplate()
    {
      try
      {
        Directory.CreateDirectory( _dir );
        var sb = new StringBuilder();
        sb.AppendLine( "# AdminToolkit email settings - fill in and save." );
        sb.AppendLine( "# The password is NOT stored here. It is set through the" );
        sb.AppendLine( "# app's password prompt and saved encrypted (DPAPI)." );
        sb.AppendLine( "#" );
        sb.AppendLine( "# Gmail example:" );
        sb.AppendLine( "#   host=smtp.gmail.com" );
        sb.AppendLine( "#   port=587" );
        sb.AppendLine( "#   enablessl=true" );
        sb.AppendLine( "#   user=you@gmail.com   (use a 16-char App Password when prompted)" );
        sb.AppendLine( "#   to=you@gmail.com" );
        sb.AppendLine();
        sb.AppendLine( "host=" );
        sb.AppendLine( "port=587" );
        sb.AppendLine( "enablessl=true" );
        sb.AppendLine( "user=" );
        sb.AppendLine( "from=" );
        sb.AppendLine( "to=" );
        File.WriteAllText( _settingsPath, sb.ToString() );
      }
      catch
      {
      }
    }

    /// <summary>Open the (non-secret) settings file in Notepad.</summary>
    public static void OpenSettings()
    {
      try
      {
        if ( ! File.Exists( _settingsPath ) )
          WriteTemplate();
        System.Diagnostics.Process.Start( "notepad.exe", "\"" + _settingsPath + "\"" );
      }
      catch
      {
      }
    }

    // ---- encrypted password store (DPAPI via Dpapi.cs) ----------------

    public static bool HasPassword()
    {
      return ! string.IsNullOrEmpty( LoadPassword() );
    }

    public static string LoadPassword()
    {
      try
      {
        if ( ! File.Exists( _secretPath ) )
          return "";
        string b64 = File.ReadAllText( _secretPath ).Trim();
        if ( b64.Length == 0 )
          return "";

        byte[ ] enc = Convert.FromBase64String( b64 );
        byte[ ] dec = Dpapi.Unprotect( enc, MachineScope );
        return Encoding.UTF8.GetString( dec );
      }
      catch
      {
        // Wrong user/machine, tampered, or corrupt - treat as "not set".
        return "";
      }
    }

    public static void SavePassword( string plain )
    {
      Directory.CreateDirectory( _dir );
      byte[ ] enc = Dpapi.Protect( Encoding.UTF8.GetBytes( plain ?? "" ), MachineScope );
      File.WriteAllText( _secretPath, Convert.ToBase64String( enc ) );
    }

    public static void ClearPassword()
    {
      try
      {
        if ( File.Exists( _secretPath ) )
          File.Delete( _secretPath );
      }
      catch
      {
      }
    }

    // ---- send ---------------------------------------------------------

    public static void Send( string subject, string body, string attachmentPath = null )
    {
      SmtpConfig cfg = LoadConfig();
      if ( ! cfg.IsComplete )
      {
        throw new InvalidOperationException( "Email is not fully configured (server details and/or password missing)." );
      }

      using ( var msg = new MailMessage( cfg.From, cfg.To, subject, body ) )
      {
        if ( ! string.IsNullOrEmpty( attachmentPath ) && File.Exists( attachmentPath ) )
          msg.Attachments.Add( new Attachment( attachmentPath ) );

        using ( var client = new SmtpClient( cfg.Host, cfg.Port ) )
        {
          client.EnableSsl   = cfg.EnableSsl;
          client.Credentials = new NetworkCredential( cfg.User, cfg.Password );
          client.Send( msg );
        }
      }
    }
  }
}
