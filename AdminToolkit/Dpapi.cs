using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Admin_Tools
{
  /// <summary>
  /// Minimal DPAPI wrapper calling crypt32.dll directly, so the project needs
  /// NO reference to System.Security.dll. Encrypts/decrypts bytes tied to the
  /// current user (or the local machine).
  /// </summary>
  internal static class Dpapi
  {
    [ StructLayout( LayoutKind.Sequential ) ]
    private struct DATA_BLOB
    {
      public int    cbData;
      public IntPtr pbData;
    }

    [ DllImport( "crypt32.dll", SetLastError = true, CharSet = CharSet.Unicode ) ]
    [ return:MarshalAs( UnmanagedType.Bool ) ]
    private static extern bool CryptProtectData( ref DATA_BLOB pDataIn, string szDataDescr, IntPtr pOptionalEntropy, IntPtr pvReserved, IntPtr pPromptStruct, int dwFlags, ref DATA_BLOB pDataOut );

    [ DllImport( "crypt32.dll", SetLastError = true, CharSet = CharSet.Unicode ) ]
    [ return:MarshalAs( UnmanagedType.Bool ) ]
    private static extern bool CryptUnprotectData( ref DATA_BLOB pDataIn, IntPtr ppszDataDescr, IntPtr pOptionalEntropy, IntPtr pvReserved, IntPtr pPromptStruct, int dwFlags, ref DATA_BLOB pDataOut );

    [ DllImport( "kernel32.dll" ) ]
    private static extern IntPtr LocalFree( IntPtr hMem );

    private const int            CRYPTPROTECT_UI_FORBIDDEN  = 0x1;
    private const int            CRYPTPROTECT_LOCAL_MACHINE = 0x4;

    /// <summary>Encrypt. machineScope=false ties it to the current user.</summary>
    public static byte[ ] Protect( byte[ ] data, bool machineScope )
    {
      DATA_BLOB inBlob  = new DATA_BLOB();
      DATA_BLOB outBlob = new DATA_BLOB();
      try
      {
        inBlob.cbData = data.Length;
        inBlob.pbData = Marshal.AllocHGlobal( Math.Max( 1, data.Length ) );
        Marshal.Copy( data, 0, inBlob.pbData, data.Length );

        int flags = CRYPTPROTECT_UI_FORBIDDEN | ( machineScope ? CRYPTPROTECT_LOCAL_MACHINE : 0 );

        if ( ! CryptProtectData( ref inBlob, null, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, flags, ref outBlob ) )
          throw new Win32Exception( Marshal.GetLastWin32Error() );

        byte[ ] result = new byte[ outBlob.cbData ];
        Marshal.Copy( outBlob.pbData, result, 0, outBlob.cbData );
        return result;
      }
      finally
      {
        if ( inBlob.pbData != IntPtr.Zero )
          Marshal.FreeHGlobal( inBlob.pbData );
        if ( outBlob.pbData != IntPtr.Zero )
          LocalFree( outBlob.pbData );
      }
    }

    /// <summary>Decrypt. machineScope must match what was used to Protect.</summary>
    public static byte[ ] Unprotect( byte[ ] data, bool machineScope )
    {
      DATA_BLOB inBlob  = new DATA_BLOB();
      DATA_BLOB outBlob = new DATA_BLOB();
      try
      {
        inBlob.cbData = data.Length;
        inBlob.pbData = Marshal.AllocHGlobal( Math.Max( 1, data.Length ) );
        Marshal.Copy( data, 0, inBlob.pbData, data.Length );

        int flags = CRYPTPROTECT_UI_FORBIDDEN | ( machineScope ? CRYPTPROTECT_LOCAL_MACHINE : 0 );

        if ( ! CryptUnprotectData( ref inBlob, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, flags, ref outBlob ) )
          throw new Win32Exception( Marshal.GetLastWin32Error() );

        byte[ ] result = new byte[ outBlob.cbData ];
        Marshal.Copy( outBlob.pbData, result, 0, outBlob.cbData );
        return result;
      }
      finally
      {
        if ( inBlob.pbData != IntPtr.Zero )
          Marshal.FreeHGlobal( inBlob.pbData );
        if ( outBlob.pbData != IntPtr.Zero )
          LocalFree( outBlob.pbData );
      }
    }
  }
}
