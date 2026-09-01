// ============================================================
//  Snapshot_Delete_Manager.cs   (C# 7.3 / .NET Framework)
//  Deletes Volume Shadow Copies via WMI (Win32_ShadowCopy).
//
//  DESTRUCTIVE: a deleted snapshot is gone forever, and any
//  restore point riding on it dies with it. Callers are
//  responsible for confirming with the user first.
//
//  Requires reference:  System.Management
//  Requires:            elevation (run as Administrator)
// ============================================================

using System.Management;
using Trace_Execution_Namespace;
using static Trace_Execution_Namespace.Trace_Execution;

namespace Admin_Tools
{
  public sealed class Snapshot_Delete_Result
  {
    public int          Deleted_Count { get; set; }
    public int          Failed_Count { get; set; }
    public List<string> Errors { get; set; }

    public Snapshot_Delete_Result ()
    {
      Errors = new List<string> ();
    }
  }

  public static class Snapshot_Delete_Manager
  {
    // --------------------------------------------------------
    //  Delete ONE snapshot by its shadow ID.
    //  Accepts the ID with or without braces, any case.
    // --------------------------------------------------------
    public static Snapshot_Delete_Result Delete_By_Id ( string Shadow_Id )
    {
      using var Block = Trace_Block.Start_If_Enabled();
      var    Result     = new Snapshot_Delete_Result ();

      string Normalized = Normalize_Id ( Shadow_Id );
      if ( Normalized == null )
      {
        Result.Failed_Count = 1;
        Result.Errors.Add ( "Invalid shadow ID format: " + Shadow_Id );
        return Result;
      }

      try
      {
        using ( var Searcher = new ManagementObjectSearcher ( @"root\cimv2", "SELECT * FROM " +
                                                                             "Win32_ShadowCopy " +
                                                                             "WHERE ID = '" +
                                                                               Normalized + "'" ) )
        {
          bool Found = false;

          foreach ( ManagementObject Mo in Searcher.Get () )
          {
            Found = true;
            Mo.Delete ();
            Result.Deleted_Count++;
          }

          if ( ! Found )
          {
            Result.Failed_Count = 1;
            Result.Errors.Add ( "No snapshot found with ID " + Normalized +
                                " (already deleted or aged out?)" );
          }
        }
      }
      catch ( Exception Ex )
      {
        Result.Failed_Count = 1;
        Result.Errors.Add ( Normalized + ": " + Ex.Message );
      }

      return Result;
    }

    // --------------------------------------------------------
    //  Delete MANY snapshots by ID (e.g. from a multi-select
    //  list). Keeps going on individual failures.
    // --------------------------------------------------------
    public static Snapshot_Delete_Result Delete_By_Ids ( IEnumerable<string> Shadow_Ids )
    {
      using var Block = Trace_Block.Start_If_Enabled();
      var Total = new Snapshot_Delete_Result ();

      foreach ( var Id in Shadow_Ids )
      {
        var One              = Delete_By_Id ( Id );
        Total.Deleted_Count += One.Deleted_Count;
        Total.Failed_Count  += One.Failed_Count;
        Total.Errors.AddRange ( One.Errors );
      }

      return Total;
    }

    // --------------------------------------------------------
    //  Delete all snapshots OLDER THAN a cutoff date,
    //  optionally restricted to one volume (drive letter,
    //  e.g. "C:"). Pass null for all volumes.
    // --------------------------------------------------------
    public static Snapshot_Delete_Result Delete_Older_Than ( DateTime Cutoff, string Drive_Letter )
    {
      using var Block = Trace_Block.Start_If_Enabled();
      var Result = new Snapshot_Delete_Result ();

      try
      {
        string Volume_Id = null;
        if ( Drive_Letter != null )
        {
          Volume_Id = Get_Volume_Id ( Drive_Letter );
          if ( Volume_Id == null )
          {
            Result.Errors.Add ( "Could not resolve volume for drive " + Drive_Letter );
            return Result;
          }
        }

        using ( var Searcher = new ManagementObjectSearcher ( @"root\cimv2", "SELECT ID, " +
                                                                             "InstallDate, " +
                                                                             "VolumeName FROM " +
                                                                             "Win32_ShadowCopy" ) )
        {
          // Collect first, then delete — deleting while
          // enumerating a WMI collection is unreliable.
          var To_Delete = new List<string> ();

          foreach ( ManagementObject Mo in Searcher.Get () )
          {
            object Install_Date = Mo[ "InstallDate" ];
            if ( Install_Date == null )
              continue;

            DateTime Created = ManagementDateTimeConverter.ToDateTime ( Install_Date.ToString () );
            if ( Created >= Cutoff )
              continue;

            if ( Volume_Id != null )
            {
              string Vol = Mo[ "VolumeName" ] == null ? "" : Mo[ "VolumeName" ].ToString ();
              if ( ! string.Equals ( Vol, Volume_Id, StringComparison.OrdinalIgnoreCase ) )
                continue;
            }

            To_Delete.Add ( Mo[ "ID" ].ToString () );
          }

          var Batch            = Delete_By_Ids ( To_Delete );
          Result.Deleted_Count = Batch.Deleted_Count;
          Result.Failed_Count  = Batch.Failed_Count;
          Result.Errors.AddRange ( Batch.Errors );
        }
      }
      catch ( Exception Ex )
      {
        Result.Errors.Add ( Ex.Message );
      }

      return Result;
    }

    // --------------------------------------------------------
    //  Keep only the NEWEST n snapshots on a volume, delete
    //  the rest. Drive_Letter e.g. "C:", or null for all.
    // --------------------------------------------------------
    public static Snapshot_Delete_Result Keep_Newest ( int Keep_Count, string Drive_Letter )
    {
      using var Block = Trace_Block.Start_If_Enabled();
      var Result = new Snapshot_Delete_Result ();

      try
      {
        string Volume_Id = Drive_Letter == null ? null : Get_Volume_Id ( Drive_Letter );

        var    All       = new List<KeyValuePair<DateTime, string>> ();

        using ( var Searcher = new ManagementObjectSearcher ( @"root\cimv2", "SELECT ID, " +
                                                                             "InstallDate, " +
                                                                             "VolumeName FROM " +
                                                                             "Win32_ShadowCopy" ) )
        {
          foreach ( ManagementObject Mo in Searcher.Get () )
          {
            object Install_Date = Mo[ "InstallDate" ];
            if ( Install_Date == null )
              continue;

            if ( Volume_Id != null )
            {
              string Vol = Mo[ "VolumeName" ] == null ? "" : Mo[ "VolumeName" ].ToString ();
              if ( ! string.Equals ( Vol, Volume_Id, StringComparison.OrdinalIgnoreCase ) )
                continue;
            }

            All.Add ( new KeyValuePair<DateTime, string> ( ManagementDateTimeConverter.ToDateTime (
                                                             Install_Date.ToString () ),
                                                           Mo[ "ID" ].ToString () ) );
          }
        }

        // Oldest first; delete everything except the last keepCount
        All.Sort ( ( A, B ) => A.Key.CompareTo ( B.Key ) );

        var To_Delete = new List<string> ();
        for ( int I = 0; I < All.Count - Keep_Count; I++ )
          To_Delete.Add ( All[ I ].Value );

        var Batch            = Delete_By_Ids ( To_Delete );
        Result.Deleted_Count = Batch.Deleted_Count;
        Result.Failed_Count  = Batch.Failed_Count;
        Result.Errors.AddRange ( Batch.Errors );
      }
      catch ( Exception Ex )
      {
        Result.Errors.Add ( Ex.Message );
      }

      return Result;
    }

    // --------------------------------------------------------
    //  Helpers
    // --------------------------------------------------------

    // Returns the ID in canonical {GUID} form, or null if invalid.
    private static string Normalize_Id ( string Shadow_Id )
    {
      if ( string.IsNullOrEmpty ( Shadow_Id ) )
        return null;

      Guid   G;
      string Trimmed = Shadow_Id.Trim ().Trim ( '{', '}' );
      if ( ! Guid.TryParse ( Trimmed, out G ) )
        return null;

      return "{" + G.ToString ().ToUpperInvariant () + "}";
    }

    // Resolves "C:" to its \\?\Volume{guid}\ name, which is what
    // Win32_ShadowCopy.VolumeName contains.
    private static string Get_Volume_Id ( string Drive_Letter )
    {
      using var Block = Trace_Block.Start_If_Enabled();
      string Letter = Drive_Letter.TrimEnd ( '\\' );
      if ( ! Letter.EndsWith ( ":" ) )
        Letter += ":";

      using ( var Searcher = new ManagementObjectSearcher ( @"root\cimv2", "SELECT DeviceID, " +
                                                                           "DriveLetter FROM " +
                                                                           "Win32_Volume WHERE " +
                                                                           "DriveLetter = '" +
                                                                             Letter + "'" ) )
      {
        foreach ( ManagementObject Mo in Searcher.Get () )
          return Mo[ "DeviceID" ] == null ? null : Mo[ "DeviceID" ].ToString ();
      }

      return null;
    }
  }
}
