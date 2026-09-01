// ============================================================
//  Restore_Point_Manager.cs   (C# 7.3 / .NET Framework version)
//  Queries System Restore points (root\default : SystemRestore)
//  and cross-links each one to its underlying shadow copy
//  (root\cimv2 : Win32_ShadowCopy) by creation timestamp.
//
//  Requires reference:  System.Management
//    (.NET Framework: Project > Add Reference > Assemblies >
//     System.Management — no NuGet needed)
//  Requires:            elevation (run as Administrator)
//
//  NOTE: adjust the namespace to match your project.
// ============================================================

using System.Management;
using System.Text;
using Trace_Execution_Namespace;
using static Trace_Execution_Namespace.Trace_Execution;

namespace Admin_Tools
{
  public sealed class Restore_Point_Info
  {
    public uint Sequence_Number
    {
      get; set;
    }
    public string Description
    {
      get; set;
    }
    public uint Restore_Point_Type
    {
      get; set;
    }
    public uint Event_Type
    {
      get; set;
    }
    public DateTime Creation_Time
    {
      get; set;
    }

    // Filled in by the shadow-copy correlation pass (may stay null)
    public string Linked_Shadow_Id
    {
      get; set;
    }
    public string Linked_Device
    {
      get; set;
    }

    public string Type_Name
    {
      get
      {
        return Restore_Point_Manager.Type_To_String( Restore_Point_Type );
      }
    }

    public string Event_Name
    {
      get
      {
        return Restore_Point_Manager.Event_To_String( Event_Type );
      }
    }
  }

  public static class Restore_Point_Manager
  {
    // --------------------------------------------------------
    //  Main query — returns all restore points, newest first,
    //  each correlated (where possible) to a shadow copy.
    // --------------------------------------------------------
    public static List<Restore_Point_Info> Get_Restore_Points()
    {
      using var Block = Trace_Block.Start_If_Enabled();
      var Points = new List<Restore_Point_Info>();

      var Scope = new ManagementScope( @"\\.\root\default" );
      Scope.Connect();

      using (var Searcher = new ManagementObjectSearcher( Scope, new ObjectQuery( "SELECT * " +
                                                                                     "FROM " +
                                                                                     "SystemResto" +
                                                                                     "re" ) ))
      {
        foreach (ManagementObject Mo in Searcher.Get())
        {
          var Rp = new Restore_Point_Info();

          Rp.Sequence_Number = (uint) Mo[ "SequenceNumber" ];
          Rp.Description = Mo[ "Description" ] == null ? "" : Mo[ "Description" ].ToString();
          Rp.Restore_Point_Type = (uint) Mo[ "RestorePointType" ];
          Rp.Event_Type = (uint) Mo[ "EventType" ];
          Rp.Creation_Time = ManagementDateTimeConverter.ToDateTime(
            Mo[ "CreationTime" ].ToString() );
          Points.Add( Rp );
        }
      }

      Correlate_With_Shadow_Copies( Points );

      return Points.OrderByDescending( P => P.Creation_Time ).ToList();
    }

    // --------------------------------------------------------
    //  Match each restore point to a shadow copy whose
    //  InstallDate is within a few seconds of the restore
    //  point's CreationTime. Windows creates them in the same
    //  operation, so a tight window is reliable.
    // --------------------------------------------------------
    private static void Correlate_With_Shadow_Copies( List<Restore_Point_Info> Points )
    {
      using var Block = Trace_Block.Start_If_Enabled();
      try
      {
        var Shadows = new List<Shadow_Entry>();

        using (var Searcher = new ManagementObjectSearcher( @"root\cimv2", "SELECT ID, " +
                                                                             "DeviceObject, " +
                                                                             "InstallDate FROM " +
                                                                             "Win32_ShadowCopy" ))
        {
          foreach (ManagementObject Mo in Searcher.Get())
          {
            object Install_Date = Mo[ "InstallDate" ];
            if (Install_Date == null)
              continue;

            var Entry = new Shadow_Entry();
            Entry.Id = Mo[ "ID" ] == null ? "" : Mo[ "ID" ].ToString();
            Entry.Device = Mo[ "DeviceObject" ] == null ? "" : Mo[ "DeviceObject" ].ToString();
            Entry.Created = ManagementDateTimeConverter.ToDateTime( Install_Date.ToString() );
            Shadows.Add( Entry );
          }
        }

        foreach (var Rp in Points)
        {
          Shadow_Entry Best = null;
          double Best_Delta = double.MaxValue;

          foreach (var S in Shadows)
          {
            double Delta = Math.Abs( (S.Created - Rp.Creation_Time).TotalSeconds );
            if (Delta <= 20.0 && Delta < Best_Delta)
            {
              Best = S;
              Best_Delta = Delta;
            }
          }

          if (Best != null)
          {
            Rp.Linked_Shadow_Id = Best.Id;
            Rp.Linked_Device = Best.Device;
          }
        }
      }
      catch
      {
        // Shadow copy correlation is best-effort; restore
        // point data is still valid without it.
      }
    }

    private sealed class Shadow_Entry
    {
      public string Id;
      public string Device;
      public DateTime Created;
    }

    // --------------------------------------------------------
    //  Detail text — same visual style as Snapshot Details
    // --------------------------------------------------------
    public static string Build_Details_Text( Restore_Point_Info Restore_Point )
    {
      var String_Builder = new StringBuilder();
      var Age = DateTime.Now - Restore_Point.Creation_Time;

      String_Builder.AppendLine( "RESTORE POINT DETAILS" );
      String_Builder.AppendLine( new string( '=', 55 ) );
      String_Builder.AppendLine( "Sequence #    : " + Restore_Point.Sequence_Number );
      String_Builder.AppendLine( "Created       : " +
                                  Restore_Point.Creation_Time.ToString( "yyyy-MM-dd HH:mm:ss" ) );
      String_Builder.AppendLine( "Age           : " + Age.TotalDays.ToString( "0.0" ) + " days" );
      String_Builder.AppendLine( "Description   : " + Restore_Point.Description );
      String_Builder.AppendLine();
      String_Builder.AppendLine( "ATTRIBUTION" );
      String_Builder.AppendLine( new string( '-', 55 ) );
      String_Builder.AppendLine( "Type          : " + Restore_Point.Type_Name + "  (" +
                                  Restore_Point.Restore_Point_Type + ")" );
      String_Builder.AppendLine( "Event         : " + Restore_Point.Event_Name + "  (" +
                                  Restore_Point.Event_Type + ")" );
      String_Builder.AppendLine();
      String_Builder.AppendLine( "LINKED SHADOW COPY (matched by timestamp)" );
      String_Builder.AppendLine( new string( '-', 55 ) );

      if (Restore_Point.Linked_Shadow_Id != null)
      {
        String_Builder.AppendLine( "Shadow ID     : " + Restore_Point.Linked_Shadow_Id );
        String_Builder.AppendLine( "Device object : " + Restore_Point.Linked_Device );
      }
      else
      {
        String_Builder.AppendLine( "None found — its shadow copy may have been" );
        String_Builder.AppendLine( "aged out (deleted) while the restore point" );
        String_Builder.AppendLine( "metadata remains." );
      }

      return String_Builder.ToString();
    }

    // --------------------------------------------------------
    //  Enum decoding (values from SrRestorePtApi.h)
    // --------------------------------------------------------
    public static string Type_To_String( uint Restore_Point_Type )
    {
      switch (Restore_Point_Type)
      {
        case 0:
          return "APPLICATION_INSTALL";
        case 1:
          return "APPLICATION_UNINSTALL";
        case 6:
          return "RESTORE";
        case 7:
          return "CHECKPOINT (scheduled/manual)";
        case 8:
          return "WINDOWS_SHUTDOWN";
        case 9:
          return "WINDOWS_BOOT";
        case 10:
          return "DEVICE_DRIVER_INSTALL";
        case 11:
          return "FIRSTRUN";
        case 12:
          return "MODIFY_SETTINGS";
        case 13:
          return "CANCELLED_OPERATION";
        case 14:
          return "BACKUP_RECOVERY";
        default:
          return "UNKNOWN (" + Restore_Point_Type + ")";
      }
    }

    public static string Event_To_String( uint E )
    {
      switch (E)
      {
        case 100:
          return "BEGIN_SYSTEM_CHANGE";
        case 101:
          return "END_SYSTEM_CHANGE";
        case 102:
          return "BEGIN_NESTED_SYSTEM_CHANGE";
        case 103:
          return "END_NESTED_SYSTEM_CHANGE";
        default:
          return "UNKNOWN (" + E + ")";
      }
    }
  }
}
