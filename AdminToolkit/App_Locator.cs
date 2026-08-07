// ============================================================
//  App_Locator.cs
//  Locates externally-installed helper apps (Tailscale,
//  RustDesk) by probing their known install folders and PATH.
//  Shared by any form that offers app-specific commands so the
//  detection logic isn't duplicated.
// ============================================================

using System;
using System.Collections.Generic;
using System.IO;

namespace Admin_Tools
{
  public enum External_App
  {
    Tailscale,
    RustDesk
  }

  internal static class App_Locator
  {
    // Describes how to find one external app.
    private sealed class App_Info
    {
      public string Name { get; }
      public string Exe { get; }
      public string[ ] ProbePaths { get; }

      public App_Info( string name, string exe, string[ ] probePaths )
      {
        Name       = name;
        Exe        = exe;
        ProbePaths = probePaths;
      }
    }

    private static readonly Dictionary<External_App, App_Info> _apps =
      new Dictionary<External_App, App_Info> { { External_App.Tailscale,
                                                 new App_Info( "Tailscale",
                                                               "tailscale.exe",
                                                               new[ ] { @"C:\Program Files\Tailscale\tailscale.exe", @"C:\Program Files (x86)\Tailscale\tailscale.exe" } ) },
                                               { External_App.RustDesk,
                                                 new App_Info( "RustDesk", "rustdesk.exe", new[ ] { @"C:\Program Files\RustDesk\rustdesk.exe", @"C:\Program Files (x86)\RustDesk\rustdesk.exe" } ) } };

    public static string Display_Name( External_App app ) => _apps[ app ].Name;

    // Returns the resolved full exe path, or null if not found.
    public static string Resolve( External_App app )
    {
      App_Info info = _apps[ app ];

      // 1. Known install folders.
      foreach ( string p in info.ProbePaths )
      {
        if ( File.Exists( p ) )
          return p;
      }

      // 2. Anything on PATH.
      string pathVar = Environment.GetEnvironmentVariable( "PATH" ) ?? "";
      foreach ( string dir in pathVar.Split( Path.PathSeparator ) )
      {
        if ( string.IsNullOrWhiteSpace( dir ) )
          continue;
        try
        {
          string candidate = Path.Combine( dir.Trim(), info.Exe );
          if ( File.Exists( candidate ) )
            return candidate;
        }
        catch
        {
          // Ignore malformed PATH entries.
        }
      }
      return null;
    }
  }
}
