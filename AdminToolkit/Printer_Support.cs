// ============================================================
//  Printer_Support.cs   (C# 7.3 / .NET Framework)
//  Static helper for the Printer_Form:
//    - list installed printers + connection state (WMI Win32_Printer)
//    - pull per-printer detail (driver, port, WMI properties)
//    - query ink/toner supply levels over the network via SNMP
//      (Printer-MIB / RFC 3805) for printers on a TCP/IP port
//
//  Local/USB printers have no OS-level supply API — Windows does
//  not expose toner/ink levels for them outside the vendor's own
//  status utility, so Get_Supply_Levels only works for network
//  printers that answer SNMP (community "public" by default).
//
//  Requires reference:  System.Management
// ============================================================

using System.Diagnostics;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace Admin_Tools
{
    public sealed class Printer_Info
    {
        public string Name;
        public string PortName;
        public string DriverName;
        public string Location;
        public string Comment;
        public string ServerName;
        public bool IsDefault;
        public bool IsNetwork;
        public bool IsLocal;
        public bool IsShared;
        public bool WorkOffline;
        public bool IsVirtual;     // PDF/XPS/Fax/OneNote/remote-session printer, not real hardware
        public int PrinterStatus;
        public string StatusText;
        public string IpAddress;   // resolved network address (IPv4/IPv6/hostname), or null
    }

    public sealed class Supply_Level
    {
        public string Description;
        public int? Percent;
        public string RawNote;
    }

    public sealed class Supply_Query_Result
    {
        public bool Success;
        public string Error;
        public List<Supply_Level> Supplies = new List<Supply_Level>();
    }

    public static class Printer_Support
    {
        // --------------------------------------------------------
        //  Installed printers + connection state
        // --------------------------------------------------------
        public static List<Printer_Info> Get_Installed_Printers()
        {
            var list = new List<Printer_Info>();

            using (var searcher = new ManagementObjectSearcher(
                @"root\cimv2", "SELECT * FROM Win32_Printer"))
            {
                foreach (ManagementObject mo in searcher.Get())
                {
                    var info = new Printer_Info();
                    info.Name = Prop(mo, "Name");
                    info.PortName = Prop(mo, "PortName");
                    info.DriverName = Prop(mo, "DriverName");
                    info.Location = Prop(mo, "Location");
                    info.Comment = Prop(mo, "Comment");
                    info.ServerName = Prop(mo, "ServerName");
                    info.IsDefault = Prop(mo, "Default") == "True";
                    info.IsNetwork = Prop(mo, "Network") == "True";
                    info.IsLocal = Prop(mo, "Local") == "True";
                    info.IsShared = Prop(mo, "Shared") == "True";
                    info.WorkOffline = Prop(mo, "WorkOffline") == "True";

                    int status;
                    int.TryParse(Prop(mo, "PrinterStatus"), out status);
                    info.PrinterStatus = status;
                    info.StatusText = Describe_Status(status, info.WorkOffline);

                    int attributes;
                    int.TryParse(Prop(mo, "Attributes"), out attributes);
                    info.IsVirtual = Detect_Virtual(info.Name, info.DriverName, info.PortName, attributes);

                    info.IpAddress = Resolve_Ip(info.PortName, info.Location);

                    list.Add(info);
                }
            }

            list.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase));
            return list;
        }

        // Names/drivers of the well-known software "printers" Windows ships or
        // that common apps install — no physical device behind these, so a
        // supply/connectivity check makes no sense for them.
        private static readonly string[] _VirtualNameHints =
        {
            "Microsoft Print To PDF",
            "Microsoft XPS Document Writer",
            "Microsoft Shared Fax Driver",
            "OneNote",
            "Fax",
            "RustDesk",
            "PDFCreator",
            "Bullzip",
            "CutePDF",
            "doPDF",
            "novaPDF",
            "Journal Note Writer",
            "Adobe PDF",
            "Foxit Reader PDF Printer",
        };

        // Pseudo-ports used by software printers — never a real device.
        private static readonly string[] _VirtualPorts =
        {
            "PORTPROMPT:", "NUL:", "FILE:", "SHRFAX:", "XPSPORT:"
        };

        private const int PRINTER_ATTRIBUTE_FAX = 0x00004000;

        private static bool Detect_Virtual(string name, string driverName, string portName,
            int attributes)
        {
            if ((attributes & PRINTER_ATTRIBUTE_FAX) != 0) return true;

            foreach (var hint in _VirtualNameHints)
            {
                if (name.IndexOf(hint, StringComparison.OrdinalIgnoreCase) >= 0) return true;
                if (driverName.IndexOf(hint, StringComparison.OrdinalIgnoreCase) >= 0) return true;
            }

            string port = (portName ?? "").ToUpperInvariant();
            foreach (var virtualPort in _VirtualPorts)
                if (port == virtualPort) return true;

            return false;
        }

        // --------------------------------------------------------
        //  Live connectivity check — is the printer actually
        //  reachable right now? (WMI's WorkOffline/PrinterStatus
        //  reflect Windows' last-known state, not this instant.)
        //
        //  Tries the standard print ports first (9100 RAW/JetDirect,
        //  631 IPP, 515 LPR) rather than a plain ICMP ping, because
        //  many printers and their firewalls drop ping while still
        //  accepting print jobs — pinging alone would misreport a
        //  perfectly reachable printer as offline. ICMP is only a
        //  last-resort fallback for devices that block all three.
        //
        //  Returns null when there's no address to test (local/USB
        //  or virtual printers) — that's "not applicable", not "down".
        // --------------------------------------------------------
        public static bool? Is_Online(string ipAddress, int timeoutMs = 800)
        {
            if (string.IsNullOrEmpty(ipAddress)) return null;

            int[] ports = { 9100, 631, 515 };
            foreach (int port in ports)
                if (Try_Tcp_Connect(ipAddress, port, timeoutMs))
                    return true;

            return Try_Ping(ipAddress, timeoutMs);
        }

        // --------------------------------------------------------
        //  "Wake" a printer that's gone quiet — there's no real
        //  Wake-on-LAN for print engines (most don't listen for a
        //  magic packet even when their NIC supports WOL; WOL is
        //  fundamentally a PC feature), but repeated connection
        //  attempts with a longer timeout are often what actually
        //  brings a sleeping printer's engine back — the same TCP
        //  connect Is_Online already does is frequently the "poke"
        //  that wakes it, it's just that a light-sleep device can be
        //  slow to answer the very first attempt.
        // --------------------------------------------------------
        public static bool? Wake_And_Check(string ipAddress, int attempts = 3, int timeoutMs = 3000)
        {
            if (string.IsNullOrEmpty(ipAddress)) return null;

            bool? result = null;
            for (int i = 0; i < attempts; i++)
            {
                result = Is_Online(ipAddress, timeoutMs);
                if (result == true) return true;
            }

            return result;
        }

        private static bool Try_Tcp_Connect(string host, int port, int timeoutMs)
        {
            try
            {
                using (var client = new TcpClient())
                {
                    IAsyncResult result = client.BeginConnect(host, port, null, null);
                    bool signaled = result.AsyncWaitHandle.WaitOne(timeoutMs);
                    if (!signaled || !client.Connected) return false;

                    client.EndConnect(result);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        private static bool Try_Ping(string host, int timeoutMs)
        {
            try
            {
                using (var ping = new Ping())
                {
                    PingReply reply = ping.Send(host, timeoutMs);
                    return reply != null && reply.Status == IPStatus.Success;
                }
            }
            catch
            {
                return false;
            }
        }

        private static string Describe_Status(int printerStatus, bool workOffline)
        {
            if (workOffline) return "Offline";

            switch (printerStatus)
            {
                case 3: return "Connected (Idle)";
                case 4: return "Connected (Printing)";
                case 5: return "Connected (Warming Up)";
                case 6: return "Stopped Printing";
                case 7: return "Offline";
                case 1: return "Other";
                default: return "Unknown";
            }
        }

        // --------------------------------------------------------
        //  Per-printer detail dump
        // --------------------------------------------------------
        public static string Get_Printer_Details_Text(string printerName)
        {
            var sb = new StringBuilder();
            sb.AppendLine("PRINTER DETAILS");
            sb.AppendLine(new string('=', 60));

            using (var searcher = new ManagementObjectSearcher(
                @"root\cimv2",
                "SELECT * FROM Win32_Printer WHERE Name = '" + Escape(printerName) + "'"))
            {
                foreach (ManagementObject mo in searcher.Get())
                {
                    sb.AppendLine("Name             : " + Prop(mo, "Name"));
                    sb.AppendLine("Share Name       : " + Prop(mo, "ShareName"));
                    sb.AppendLine("Port             : " + Prop(mo, "PortName"));
                    sb.AppendLine("Driver           : " + Prop(mo, "DriverName"));
                    sb.AppendLine("Server           : " + Prop(mo, "ServerName"));
                    sb.AppendLine("Location         : " + Prop(mo, "Location"));
                    sb.AppendLine("Comment          : " + Prop(mo, "Comment"));
                    sb.AppendLine("Default          : " + Prop(mo, "Default"));
                    sb.AppendLine("Network          : " + Prop(mo, "Network"));
                    sb.AppendLine("Local            : " + Prop(mo, "Local"));
                    sb.AppendLine("Shared           : " + Prop(mo, "Shared"));
                    sb.AppendLine("Work Offline     : " + Prop(mo, "WorkOffline"));
                    sb.AppendLine("Printer Status   : " + Prop(mo, "PrinterStatus"));
                    sb.AppendLine("Printer State    : " + Prop(mo, "PrinterState"));
                    sb.AppendLine("Extended Status  : " + Prop(mo, "ExtendedPrinterStatus"));
                    sb.AppendLine("Detected Error   : " + Prop(mo, "DetectedErrorState"));
                    sb.AppendLine("Horizontal DPI   : " + Prop(mo, "HorizontalResolution"));
                    sb.AppendLine("Vertical DPI     : " + Prop(mo, "VerticalResolution"));
                    sb.AppendLine("Color            : " + Prop(mo, "Capabilities"));
                }
            }

            string driverName = null;
            using (var searcher = new ManagementObjectSearcher(
                @"root\cimv2",
                "SELECT * FROM Win32_Printer WHERE Name = '" + Escape(printerName) + "'"))
            {
                foreach (ManagementObject mo in searcher.Get())
                    driverName = Prop(mo, "DriverName");
            }

            if (!string.IsNullOrEmpty(driverName))
            {
                sb.AppendLine();
                sb.AppendLine("DRIVER");
                sb.AppendLine(new string('-', 60));

                using (var searcher = new ManagementObjectSearcher(
                    @"root\cimv2",
                    "SELECT * FROM Win32_PrinterDriver WHERE Name = '" + Escape(driverName) + "'"))
                {
                    foreach (ManagementObject mo in searcher.Get())
                    {
                        sb.AppendLine("Driver Name      : " + Prop(mo, "Name"));
                        sb.AppendLine("Version          : " + Prop(mo, "Version"));
                        sb.AppendLine("Driver Path      : " + Prop(mo, "DriverPath"));
                        sb.AppendLine("Infname          : " + Prop(mo, "InfName"));
                        sb.AppendLine("Supported Env.   : " + Prop(mo, "SupportedPlatform"));
                    }
                }
            }

            string portName, location;
            Get_Port_And_Location(printerName, out portName, out location);

            string ip = Resolve_Ip(portName, location);
            if (!string.IsNullOrEmpty(ip))
            {
                sb.AppendLine();
                sb.AppendLine("TCP/IP PORT");
                sb.AppendLine(new string('-', 60));
                sb.AppendLine("Host Address     : " + ip);
            }

            return sb.ToString();
        }

        private static void Get_Port_And_Location(
            string printerName, out string portName, out string location)
        {
            portName = null;
            location = null;

            using (var searcher = new ManagementObjectSearcher(
                @"root\cimv2",
                "SELECT PortName, Location FROM Win32_Printer WHERE Name = '" +
                    Escape(printerName) + "'"))
            {
                foreach (ManagementObject mo in searcher.Get())
                {
                    portName = Prop(mo, "PortName");
                    location = Prop(mo, "Location");
                }
            }
        }

        private static string Resolve_Ip(string portName, string location)
        {
            if (string.IsNullOrEmpty(portName)) return null;

            // Modern print-port WMI class (backs PowerShell's Get-PrinterPort).
            // Covers WSD ports (e.g. "WSD-f97a8705-ed62-...") as well as
            // classic TCP/IP ports, since Windows resolves the real host
            // address for both when it manages the port.
            try
            {
                using (var searcher = new ManagementObjectSearcher(
                    @"root\StandardCimv2",
                    "SELECT PrinterHostAddress FROM MSFT_PrinterPort WHERE Name = '" +
                        Escape(portName) + "'"))
                {
                    foreach (ManagementObject mo in searcher.Get())
                    {
                        string host = Prop(mo, "PrinterHostAddress");
                        if (host.Length > 0) return host;
                    }
                }
            }
            catch
            {
                // PrintManagement WMI provider unavailable — fall through.
            }

            try
            {
                using (var searcher = new ManagementObjectSearcher(
                    @"root\cimv2",
                    "SELECT HostAddress FROM Win32_TCPIPPrinterPort WHERE Name = '" +
                        Escape(portName) + "'"))
                {
                    foreach (ManagementObject mo in searcher.Get())
                    {
                        string host = Prop(mo, "HostAddress");
                        if (host.Length > 0) return host;
                    }
                }
            }
            catch
            {
                // not a TCP/IP port (USB, local, redirected, etc.)
            }

            // Fallback: some drivers (Zebra "LAN_", Canon "IP_", etc.) name the
            // port after the address directly instead of registering it as a
            // Win32_TCPIPPrinterPort. Pull an address-looking substring out of it.
            string fromPort = Extract_Address(portName);
            if (fromPort != null) return fromPort;

            // WSD-discovered printers default their Location field to the
            // device's own URL — Windows fills this in automatically at
            // discovery time, it's not a user-typed value. It's not always an
            // IPv4 literal though: it can be IPv6 (link-local addresses are
            // common on WSD/WS-Discovery), or a hostname if that's what the
            // device advertised — Extract_Address handles all three.
            string fromLocation = Extract_Address(location);
            if (fromLocation != null) return fromLocation;

            // Last resort for WSD printers whose address was never cached in
            // WMI or Location at all (common on Windows 11, where the modern
            // IPP-class driver takes over the queue and only leaves a bare
            // device UUID behind). Windows still knows the live address —
            // it's just cached on a different device node.
            return Resolve_Wsd_Device_Address(portName);
        }

        // --------------------------------------------------------
        //  Last-resort WSD address lookup via the PnP device store.
        //
        //  When a printer is installed over WSD, the "WSD Port" monitor
        //  records the device's WSD UUID in the registry
        //  (HKLM\SYSTEM\...\Print\Monitors\WSD Port\Ports\<PortName>,
        //  value "Printer UUID") but NOT its network address. On modern
        //  Windows the actual queue is driven by the IPP class driver via
        //  a linked "SWD\IPP\<uuid>" software device node, and THAT node's
        //  PnP properties cache the printer's live IPP/AirPrint URLs —
        //  including its current IP. There's no WMI or plain-registry path
        //  to that property store; Get-PnpDeviceProperty (backed by
        //  CfgMgr32) is the supported way to read it, so this shells out
        //  for just this one value, only when every cheaper lookup failed.
        // --------------------------------------------------------
        private static string Resolve_Wsd_Device_Address(string portName)
        {
            if (string.IsNullOrEmpty(portName) ||
                !portName.StartsWith("WSD-", StringComparison.OrdinalIgnoreCase))
                return null;

            string uuid = Read_Wsd_Printer_Uuid(portName);
            if (string.IsNullOrEmpty(uuid)) return null;

            return Query_Ipp_Device_Address(uuid);
        }

        private static string Read_Wsd_Printer_Uuid(string portName)
        {
            try
            {
                using (var key = Registry.LocalMachine.OpenSubKey(
                    @"SYSTEM\CurrentControlSet\Control\Print\Monitors\WSD Port\Ports\" + portName))
                {
                    return key?.GetValue("Printer UUID") as string;
                }
            }
            catch
            {
                return null;
            }
        }

        private static string Query_Ipp_Device_Address(string uuid)
        {
            string instanceId = ("SWD\\IPP\\" + uuid).Replace("'", "''");

            // Join each property's Data (scalar or array) into one string per
            // property, then pull the first IPv4-looking substring out of
            // whichever one has it — covers both bare-IP properties and
            // full IPP/AirPrint URL properties in one pass.
            string script =
                "$ErrorActionPreference = 'SilentlyContinue'; " +
                "Get-PnpDeviceProperty -InstanceId '" + instanceId + "' | " +
                "ForEach-Object { $_.Data } | " +
                "ForEach-Object { [string]::Join(',', $_) } | " +
                "Select-String -Pattern '\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}' -AllMatches | " +
                "ForEach-Object { $_.Matches } | " +
                "ForEach-Object { $_.Value } | " +
                "Select-Object -First 1";

            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = "-NoProfile -NonInteractive -Command \"" +
                        script.Replace("\"", "\\\"") + "\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                using (var proc = Process.Start(psi))
                {
                    string output = proc.StandardOutput.ReadToEnd().Trim();
                    proc.WaitForExit(4000);
                    return output.Length > 0 ? output : null;
                }
            }
            catch
            {
                return null;
            }
        }

        // Pulls a usable host out of free-form text: if the text parses as an
        // http(s) URL (the shape WSD-populated Location fields always take,
        // e.g. "http://192.168.1.77:80/WebServices/Device" or
        // "http://[fe80::1234:5678]/WebServices/Device"), the URI's Host is
        // exactly the address we want — IPv4 literal, IPv6 literal (brackets
        // stripped automatically), or a plain hostname, and callers (TCP
        // connect, ping, SNMP) can all resolve a hostname via DNS just fine.
        // Otherwise falls back to regex-hunting for a bare IPv4/IPv6 literal,
        // which covers port names like "IP_192.168.254.49".
        private static string Extract_Address(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;

            Uri uri;
            if (Uri.TryCreate(text, UriKind.Absolute, out uri) &&
                (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
            {
                return uri.Host;
            }

            // No \b anchor here: some vendor port monitors (Zebra's "LAN_",
            // Epson's "USB_", etc.) prefix the address with an underscore,
            // and \b never fires between an underscore and a digit — both
            // count as "word" characters, so there's no boundary to match.
            var v4 = Regex.Match(text, @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");
            if (v4.Success)
            {
                IPAddress parsed;
                if (IPAddress.TryParse(v4.Value, out parsed)) return v4.Value;
            }

            var v6 = Regex.Match(text, @"(?:[0-9a-fA-F]{1,4}:){2,7}[0-9a-fA-F]{1,4}");
            if (v6.Success)
            {
                IPAddress parsed;
                if (IPAddress.TryParse(v6.Value, out parsed)) return v6.Value;
            }

            return null;
        }

        private static string Escape(string value)
        {
            return value == null ? "" : value.Replace("'", "''");
        }

        private static string Prop(ManagementObject mo, string name)
        {
            try
            {
                object v = mo[name];
                return v == null ? "" : v.ToString();
            }
            catch
            {
                return "";
            }
        }

        // --------------------------------------------------------
        //  Ink / toner levels — SNMP Printer-MIB (RFC 3805) walk.
        //  Only works for network printers reachable on UDP 161
        //  with the given community string (default "public").
        // --------------------------------------------------------
        public static Supply_Query_Result Get_Supply_Levels(
            string ipAddress, string community = "public", int timeoutMs = 2000)
        {
            var result = new Supply_Query_Result();

            if (string.IsNullOrWhiteSpace(ipAddress))
            {
                result.Error =
                    "This printer has no resolvable network address (not a TCP/IP port). " +
                    "Supply levels can only be queried over the network via SNMP — local/USB " +
                    "printers require the manufacturer's own status utility.";
                return result;
            }

            const string DescOid = "1.3.6.1.2.1.43.11.1.1.6.1";
            const string LevelOid = "1.3.6.1.2.1.43.11.1.1.9.1";
            const string MaxOid = "1.3.6.1.2.1.43.11.1.1.8.1";

            try
            {
                var descriptions = Snmp.Walk(ipAddress, community, DescOid, timeoutMs);

                if (descriptions.Count == 0)
                {
                    result.Error =
                        "No SNMP response from " + ipAddress + " (UDP 161). The printer may be " +
                        "offline, block SNMP, or use a vendor-private MIB not covered by the " +
                        "standard Printer-MIB.";
                    return result;
                }

                var levels = Snmp.Walk(ipAddress, community, LevelOid, timeoutMs);
                var maxes = Snmp.Walk(ipAddress, community, MaxOid, timeoutMs);

                foreach (var d in descriptions)
                {
                    string index = d.Oid.Substring(d.Oid.LastIndexOf('.') + 1);

                    var levelEntry = levels.Find(v => v.Oid.EndsWith("." + index));
                    var maxEntry = maxes.Find(v => v.Oid.EndsWith("." + index));

                    var supply = new Supply_Level();
                    supply.Description = Snmp.To_Display_String(d);

                    int level = levelEntry != null ? Snmp.To_Int(levelEntry) : int.MinValue;
                    int max = maxEntry != null ? Snmp.To_Int(maxEntry) : int.MinValue;

                    // RFC 3805: -2 = level unavailable, -3 = capacity unknown.
                    if (level == -2)
                        supply.RawNote = "Level unavailable (supply present)";
                    else if (level < 0 || max <= 0)
                        supply.RawNote = "Capacity not reported by this device";
                    else
                        supply.Percent = (int)Math.Round(level * 100.0 / max);

                    result.Supplies.Add(supply);
                }

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Error = "SNMP query failed: " + ex.Message;
            }

            return result;
        }

        // ==========================================================
        //  Minimal SNMPv1 GET-NEXT client (BER encode/decode).
        //  Just enough to walk a MIB subtree — no external library.
        // ==========================================================
        private static class Snmp
        {
            public sealed class Varbind
            {
                public string Oid;
                public byte Tag;
                public byte[] Value;
            }

            private static readonly Random _Rand = new Random();

            public static List<Varbind> Walk(
                string ip, string community, string baseOid, int timeoutMs)
            {
                var results = new List<Varbind>();
                string current = baseOid;

                for (int i = 0; i < 64; i++)   // safety cap — real tables are tiny
                {
                    var vb = Get_Next(ip, community, current, timeoutMs);
                    if (vb == null) break;
                    if (vb.Oid != baseOid && !vb.Oid.StartsWith(baseOid + ".")) break;
                    if (results.Count > 0 && vb.Oid == results[results.Count - 1].Oid) break;

                    results.Add(vb);
                    current = vb.Oid;
                }

                return results;
            }

            public static int To_Int(Varbind vb)
            {
                return vb.Tag == 0x02 ? Decode_Integer(vb.Value) : (int)Decode_Unsigned(vb.Value);
            }

            public static string To_Display_String(Varbind vb)
            {
                if (vb.Tag == 0x04)
                {
                    // Some devices (this HP included) pad OCTET STRING supply
                    // descriptions with a trailing NUL, which isn't printable
                    // ASCII itself but shouldn't disqualify an otherwise clean
                    // string — trim it before judging printability.
                    int length = vb.Value.Length;
                    while (length > 0 && vb.Value[length - 1] == 0x00) length--;

                    bool printable = true;
                    for (int i = 0; i < length; i++)
                    {
                        byte b = vb.Value[i];
                        if (b < 0x20 || b >= 0x7F) { printable = false; break; }
                    }

                    return printable ? Encoding.ASCII.GetString(vb.Value, 0, length)
                                      : BitConverter.ToString(vb.Value);
                }

                return vb.Tag == 0x02
                    ? Decode_Integer(vb.Value).ToString()
                    : Decode_Unsigned(vb.Value).ToString();
            }

            private static Varbind Get_Next(string ip, string community, string oid, int timeoutMs)
            {
                try
                {
                    byte[] request = Build_Get_Next(community, oid, _Rand.Next(1, int.MaxValue));

                    using (var udp = new UdpClient())
                    {
                        udp.Client.ReceiveTimeout = timeoutMs;
                        udp.Connect(ip, 161);
                        udp.Send(request, request.Length);

                        var remote = new IPEndPoint(IPAddress.Any, 0);
                        byte[] response = udp.Receive(ref remote);

                        int pos = 0;
                        var envelope = Read_Tlv(response, ref pos);

                        int innerPos = 0;
                        Read_Tlv(envelope.Value, ref innerPos);            // version
                        Read_Tlv(envelope.Value, ref innerPos);            // community
                        var pdu = Read_Tlv(envelope.Value, ref innerPos);  // GetResponse-PDU

                        int pduPos = 0;
                        Read_Tlv(pdu.Value, ref pduPos);                   // request-id
                        var errStatus = Read_Tlv(pdu.Value, ref pduPos);
                        Read_Tlv(pdu.Value, ref pduPos);                   // error-index

                        if (Decode_Integer(errStatus.Value) != 0)
                            return null;   // noSuchName — end of walkable subtree

                        var varbindList = Read_Tlv(pdu.Value, ref pduPos);
                        int vbPos = 0;
                        var varbind = Read_Tlv(varbindList.Value, ref vbPos);

                        int oPos = 0;
                        var oidTlv = Read_Tlv(varbind.Value, ref oPos);
                        var valTlv = Read_Tlv(varbind.Value, ref oPos);

                        return new Varbind
                        {
                            Oid = Decode_Oid(oidTlv.Value),
                            Tag = valTlv.Tag,
                            Value = valTlv.Value
                        };
                    }
                }
                catch
                {
                    return null;   // timeout / unreachable / malformed reply
                }
            }

            // ---- BER encode ----------------------------------------

            private static byte[] Build_Get_Next(string community, string oid, int requestId)
            {
                byte[] oidTlv = Encode_Oid(oid);
                byte[] varbind = Wrap_Tlv(0x30, Concat(oidTlv, Null_Tlv));
                byte[] varbindList = Wrap_Tlv(0x30, varbind);

                byte[] pduContent = Concat(
                    Encode_Integer(requestId),
                    Encode_Integer(0),
                    Encode_Integer(0),
                    varbindList);
                byte[] pdu = Wrap_Tlv(0xA1, pduContent);   // GetNextRequest-PDU

                byte[] messageContent = Concat(
                    Encode_Integer(0),   // SNMP version 1 -> encoded value 0
                    Encode_Octet_String(community),
                    pdu);

                return Wrap_Tlv(0x30, messageContent);
            }

            private static readonly byte[] Null_Tlv = { 0x05, 0x00 };

            private static byte[] Encode_Oid(string oid)
            {
                string[] parts = oid.Split('.');
                var body = new List<byte>();
                body.Add((byte)(int.Parse(parts[0]) * 40 + int.Parse(parts[1])));

                for (int i = 2; i < parts.Length; i++)
                    body.AddRange(Encode_Base128(int.Parse(parts[i])));

                return Wrap_Tlv(0x06, body.ToArray());
            }

            private static byte[] Encode_Base128(int value)
            {
                if (value == 0) return new byte[] { 0 };

                var stack = new List<byte>();
                while (value > 0)
                {
                    stack.Insert(0, (byte)(value & 0x7F));
                    value >>= 7;
                }
                for (int i = 0; i < stack.Count - 1; i++)
                    stack[i] |= 0x80;

                return stack.ToArray();
            }

            private static byte[] Encode_Integer(int value)
            {
                byte[] bytes = BitConverter.GetBytes(value);
                Array.Reverse(bytes);   // big-endian

                int start = 0;
                while (start < bytes.Length - 1 &&
                       ((bytes[start] == 0x00 && (bytes[start + 1] & 0x80) == 0) ||
                        (bytes[start] == 0xFF && (bytes[start + 1] & 0x80) != 0)))
                    start++;

                byte[] trimmed = new byte[bytes.Length - start];
                Array.Copy(bytes, start, trimmed, 0, trimmed.Length);
                return Wrap_Tlv(0x02, trimmed);
            }

            private static byte[] Encode_Octet_String(string s)
            {
                return Wrap_Tlv(0x04, Encoding.ASCII.GetBytes(s));
            }

            private static byte[] Encode_Length(int length)
            {
                if (length < 0x80) return new byte[] { (byte)length };

                var bytes = new List<byte>();
                int len = length;
                while (len > 0)
                {
                    bytes.Insert(0, (byte)(len & 0xFF));
                    len >>= 8;
                }

                var result = new List<byte> { (byte)(0x80 | bytes.Count) };
                result.AddRange(bytes);
                return result.ToArray();
            }

            private static byte[] Wrap_Tlv(byte tag, byte[] content)
            {
                byte[] lenBytes = Encode_Length(content.Length);
                byte[] result = new byte[1 + lenBytes.Length + content.Length];
                result[0] = tag;
                Array.Copy(lenBytes, 0, result, 1, lenBytes.Length);
                Array.Copy(content, 0, result, 1 + lenBytes.Length, content.Length);
                return result;
            }

            private static byte[] Concat(params byte[][] chunks)
            {
                int total = 0;
                foreach (var c in chunks) total += c.Length;

                byte[] result = new byte[total];
                int offset = 0;
                foreach (var c in chunks)
                {
                    Array.Copy(c, 0, result, offset, c.Length);
                    offset += c.Length;
                }
                return result;
            }

            // ---- BER decode ----------------------------------------

            private struct Tlv
            {
                public byte Tag;
                public byte[] Value;
            }

            private static Tlv Read_Tlv(byte[] buf, ref int pos)
            {
                byte tag = buf[pos++];
                int len = buf[pos++];

                if ((len & 0x80) != 0)
                {
                    int numBytes = len & 0x7F;
                    len = 0;
                    for (int i = 0; i < numBytes; i++)
                        len = (len << 8) | buf[pos++];
                }

                byte[] value = new byte[len];
                Array.Copy(buf, pos, value, 0, len);
                pos += len;

                return new Tlv { Tag = tag, Value = value };
            }

            private static string Decode_Oid(byte[] data)
            {
                var sb = new StringBuilder();
                int first = data[0];
                sb.Append(first / 40).Append('.').Append(first % 40);

                int value = 0;
                for (int i = 1; i < data.Length; i++)
                {
                    value = (value << 7) | (data[i] & 0x7F);
                    if ((data[i] & 0x80) == 0)
                    {
                        sb.Append('.').Append(value);
                        value = 0;
                    }
                }

                return sb.ToString();
            }

            private static int Decode_Integer(byte[] data)
            {
                int value = (sbyte)data[0];
                for (int i = 1; i < data.Length; i++)
                    value = (value << 8) | data[i];
                return value;
            }

            private static long Decode_Unsigned(byte[] data)
            {
                long value = 0;
                foreach (byte b in data)
                    value = (value << 8) | b;
                return value;
            }
        }
    }
}
