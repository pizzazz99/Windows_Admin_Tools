// ============================================================
//  Lan_Scanner.cs
//  Finds Remote-Desktop-capable machines on the local network
//  by probing TCP 3389 across the host's own /24 subnet(s),
//  then resolving names (reverse DNS, then NetBIOS). This works
//  where "net view" fails, because it does not rely on the
//  (legacy, usually-disabled) SMB browse list, and TCP probing
//  is not blocked by the ICMP rules that defeat ping sweeps.
//
//  Also provides Test_Host — a single-target pre-flight check
//  (the equivalent of "Test-NetConnection -Port 3389").
// ============================================================

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Admin_Tools
{
  // A machine we can offer as an RDP target.
  internal sealed class Rdp_Target
  {
    public string          Name { get; set; } // may be null if unresolved
    public string          Ip { get; set; }   // may be null (e.g. from net view)

    // What we actually hand to mstsc. A confirmed-open IP is
    // the most reliable target; fall back to the name.
    public string          ConnectTarget => ! string.IsNullOrEmpty( Ip ) ? Ip : Name;

    public override string ToString()
    {
      if ( ! string.IsNullOrEmpty( Name ) && ! string.IsNullOrEmpty( Ip ) )
        return $"{Name}  ({Ip})";
      return string.IsNullOrEmpty( Name ) ? Ip : Name;
    }
  }

  // Result of a single-target pre-flight test.
  internal sealed class Host_Test
  {
    public string ResolvedIp; // null if the name didn't resolve
    public bool   PortOpen;
  }

  internal static class Lan_Scanner
  {
    private const int               Rdp_Port         = 3389;
    private const int               Probe_Timeout_Ms = 400;
    private const int               Max_Subnets      = 2; // guard against huge scans

    // --------------------------------------------------------
    //  In-memory cache (lives for the life of the process, so
    //  discovered hosts are remembered across dialog opens).
    // --------------------------------------------------------
    private static List<Rdp_Target> _cache;

    public static bool              Has_Cache => _cache != null;
    public static DateTime? Last_Scan_Local { get; private set; }

    // Returns remembered hosts if we have them (unless a rescan
    // is forced); otherwise runs a fresh scan and caches it.
    public static async Task<List<Rdp_Target>> Get_Hosts( bool forceRescan = false )
    {
      if ( ! forceRescan && _cache != null )
        return _cache;

      // Primary: hosts with RDP open on the local subnet.
      List<Rdp_Target> targets = await Scan_Rdp_Hosts();

      // Bonus: names from the legacy browse list that the
      // scan didn't already surface.
      foreach ( string name in await Task.Run( () => Browse_List_Names() ) )
      {
        bool known = targets.Any( t => string.Equals( t.Name, name, StringComparison.OrdinalIgnoreCase ) || string.Equals( t.ConnectTarget, name, StringComparison.OrdinalIgnoreCase ) );

        if ( ! known )
          targets.Add( new Rdp_Target { Name = name } );
      }

      _cache          = targets;
      Last_Scan_Local = DateTime.Now;
      return _cache;
    }

    // Forget the remembered list (next Get_Hosts will rescan).
    public static void Clear_Cache()
    {
      _cache          = null;
      Last_Scan_Local = null;
    }

    // --------------------------------------------------------
    //  Subnet sweep
    // --------------------------------------------------------
    public static async Task<List<Rdp_Target>> Scan_Rdp_Hosts()
    {
      var probes = new List<Task<Rdp_Target>>();

      foreach ( string prefix in Local_Subnets().Take( Max_Subnets ) )
      {
        for ( int host = 1; host <= 254; host++ )
        {
          probes.Add( Probe( prefix + "." + host ) );
        }
      }

      Rdp_Target[ ] results = await Task.WhenAll( probes );

      var                           byIp = new Dictionary<string, Rdp_Target>( StringComparer.OrdinalIgnoreCase );
      foreach ( Rdp_Target r in results )
      {
        if ( r != null && ! byIp.ContainsKey( r.Ip ) )
          byIp[ r.Ip ] = r;
      }

      List<Rdp_Target> list = byIp.Values.ToList();
      list.Sort( ( a, b ) => string.Compare( a.ToString(), b.ToString(), StringComparison.OrdinalIgnoreCase ) );
      return list;
    }

    // Returns a target if TCP 3389 is open on the IP, else null.
    private static async Task<Rdp_Target> Probe( string ip )
    {
      try
      {
        using ( var client = new TcpClient() )
        {
          Task connect  = client.ConnectAsync( ip, Rdp_Port );
          Task finished = await Task.WhenAny( connect, Task.Delay( Probe_Timeout_Ms ) );

          if ( finished != connect )
          {
            Observe( connect );
            return null;
          }

          await connect; // throws if refused
          if ( ! client.Connected )
            return null;

          string name = await Resolve_Name( ip );
          return new Rdp_Target { Ip = ip, Name = name };
        }
      }
      catch
      {
        return null; // closed / unreachable
      }
    }

    // --------------------------------------------------------
    //  Single-target pre-flight test (Test-NetConnection-style)
    // --------------------------------------------------------
    public static async Task<Host_Test> Test_Host( string target, int timeoutMs = 1500 )
    {
      var    result = new Host_Test();

      // Resolve first so we can show the IP a name maps to.
      string connectIp = target;
      try
      {
        IPAddress[ ] addrs                = await Dns.GetHostAddressesAsync( target );
        IPAddress                  v4     = addrs.FirstOrDefault( a => a.AddressFamily == AddressFamily.InterNetwork );
        IPAddress                  chosen = v4 ?? addrs.FirstOrDefault();
        if ( chosen != null )
        {
          result.ResolvedIp = chosen.ToString();
          connectIp         = result.ResolvedIp;
        }
      }
      catch
      {
        // Couldn't resolve; still try connecting to the raw target.
      }

      try
      {
        using ( var client = new TcpClient() )
        {
          Task connect  = client.ConnectAsync( connectIp, Rdp_Port );
          Task finished = await Task.WhenAny( connect, Task.Delay( timeoutMs ) );

          if ( finished == connect )
          {
            await connect;
            result.PortOpen = client.Connected;
          }
          else
          {
            Observe( connect );
          }
        }
      }
      catch
      {
        result.PortOpen = false;
      }

      return result;
    }

    // --------------------------------------------------------
    //  Name resolution: reverse DNS, then NetBIOS fallback
    // --------------------------------------------------------
    private static async Task<string> Resolve_Name( string ip )
    {
      // 1. Reverse DNS (PTR).
      try
      {
        IPHostEntry entry              = await Dns.GetHostEntryAsync( ip );
        string                    host = entry.HostName;
        if ( ! string.IsNullOrEmpty( host ) && host != ip )
        {
          int dot = host.IndexOf( '.' ); // strip DNS suffix
          return dot > 0 ? host.Substring( 0, dot ) : host;
        }
      }
      catch
      {
        // fall through to NetBIOS
      }

      // 2. NetBIOS node-status query — works on many LANs
      //    where no PTR record exists (like "net view" targets).
      return await Task.Run( () => Nbt_Name( ip ) );
    }

    // Parses the computer name from "nbtstat -A <ip>" output:
    // the <00> UNIQUE entry is the machine name.
    private static string Nbt_Name( string ip )
    {
      try
      {
        var psi = new ProcessStartInfo( "nbtstat", "-A " + ip ) { RedirectStandardOutput = true, UseShellExecute = false, CreateNoWindow = true };

        using ( var p = Process.Start( psi ) )
        {
          string output = p.StandardOutput.ReadToEnd();
          if ( ! p.WaitForExit( 2000 ) )
          {
            try
            {
              p.Kill();
            }
            catch
            {
            }
            return null;
          }

          foreach ( string raw in output.Split( '\n' ) )
          {
            string line = raw.Trim();
            if ( ! line.Contains( "<00>" ) )
              continue;
            if ( line.IndexOf( "UNIQUE", StringComparison.OrdinalIgnoreCase ) < 0 )
              continue;

            string name = line.Split( new[ ] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries )[ 0 ];
            if ( name.Length > 0 )
              return name;
          }
        }
      }
      catch
      {
        // best effort
      }
      return null;
    }

    // --------------------------------------------------------
    //  Helpers
    // --------------------------------------------------------

    // Legacy SMB browse list via "net view" (best effort; often
    // empty on modern Windows). Kept only as an extra name source.
    private static List<string> Browse_List_Names()
    {
      var names = new List<string>();
      try
      {
        var psi = new ProcessStartInfo( "net", "view" ) { RedirectStandardOutput = true, UseShellExecute = false, CreateNoWindow = true };

        using ( var p = Process.Start( psi ) )
        {
          string output = p.StandardOutput.ReadToEnd();
          p.WaitForExit( 5000 );

          foreach ( string raw in output.Split( '\n' ) )
          {
            string line = raw.Trim();
            if ( ! line.StartsWith( @"\\" ) )
              continue;

            string name = line.Substring( 2 ).Split( new[ ] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries )[ 0 ];

            if ( name.Length > 0 )
              names.Add( name );
          }
        }
      }
      catch
      {
        // best effort
      }
      return names;
    }

    // Swallow the eventual exception of an abandoned connect
    // task so it doesn't surface as an unobserved exception.
    private static void Observe( Task t )
    {
      _ = t.ContinueWith( x =>
                          { var _ignored = x.Exception; },
                          TaskScheduler.Default );
    }

    // First three octets of each up, non-loopback IPv4 adapter,
    // skipping APIPA and the Tailscale CGNAT range (100.64/10).
    private static IEnumerable<string> Local_Subnets()
    {
      var prefixes = new HashSet<string>();

      foreach ( NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces() )
      {
        if ( ni.OperationalStatus != OperationalStatus.Up )
          continue;
        if ( ni.NetworkInterfaceType == NetworkInterfaceType.Loopback )
          continue;

        foreach ( UnicastIPAddressInformation ua in ni.GetIPProperties().UnicastAddresses )
        {
          if ( ua.Address.AddressFamily != AddressFamily.InterNetwork )
            continue;

          string ip = ua.Address.ToString();
          if ( ip.StartsWith( "169.254." ) )
            continue; // APIPA
          if ( Is_Tailscale_Cgnat( ua.Address ) )
            continue; // 100.64/10

          int last = ip.LastIndexOf( '.' );
          if ( last > 0 )
            prefixes.Add( ip.Substring( 0, last ) );
        }
      }
      return prefixes;
    }

    private static bool Is_Tailscale_Cgnat( IPAddress addr )
    {
      byte[ ] b = addr.GetAddressBytes();
      return b.Length == 4 && b[ 0 ] == 100 && b[ 1 ] >= 64 && b[ 1 ] <= 127;
    }
  }
}
