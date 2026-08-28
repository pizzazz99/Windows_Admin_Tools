// ============================================================
//  Admin_Tools_Form.cs   (C# 7.3 / .NET Framework)
//  Detached window holding the "Admin Tools" launcher buttons
//  that used to live in a group box on MainForm. All launch
//  logic (ToggleTool, process tracking, button color state)
//  still lives on MainForm — this form just forwards each
//  button's Click event to the matching MainForm handler, so
//  there is exactly one place that owns "what happens when you
//  click Task Scheduler."
//
//  Pairs with Admin_Tools_Form.Designer.cs.
//
//  Usage from MainForm:
//      _AdminToolsForm = new Admin_Tools_Form(this);
//      _AdminToolsForm.Show(this);
// ============================================================

using System.Windows.Forms;

namespace Admin_Tools
{
  public partial class Admin_Tools_Form : Form
  {
    private readonly MainForm _Owner;

    public Admin_Tools_Form( MainForm owner )
    {
      InitializeComponent();
      _Owner = owner;

      // All launcher buttons start green (ready) — same palette
      // MainForm uses for every other tool launcher.
      foreach (Control control in grpAdminTools.Controls)
      {
        if (control is Button button)
        {
          button.UseVisualStyleBackColor = false;
          button.BackColor = MainForm._ReadyColor;
        }
      }
    }

    private void BtnTaskScheduler_Click( object sender, System.EventArgs e ) =>
        _Owner.BtnTaskScheduler_Click( sender, e );

    private void BtnSystemProtection_Click( object sender, System.EventArgs e ) =>
        _Owner.BtnSystemProtection_Click( sender, e );

    private void Task_Manager_Button_Click( object sender, System.EventArgs e ) =>
        _Owner.Task_Manager_Button_Click( sender, e );

    private void BtnLocalSecurityPolicy_Click( object sender, System.EventArgs e ) =>
        _Owner.BtnLocalSecurityPolicy_Click( sender, e );

    private void BtnRestoreWizard_Click( object sender, System.EventArgs e ) =>
        _Owner.BtnRestoreWizard_Click( sender, e );

    private void BtnRegistryEditor_Click( object sender, System.EventArgs e ) =>
        _Owner.BtnRegistryEditor_Click( sender, e );

    private void BtnEventViewer_Click( object sender, System.EventArgs e ) =>
        _Owner.BtnEventViewer_Click( sender, e );

    private void BtnServices_Click( object sender, System.EventArgs e ) =>
        _Owner.BtnServices_Click( sender, e );

    private void BtnDiskManagement_Click( object sender, System.EventArgs e ) =>
        _Owner.BtnDiskManagement_Click( sender, e );

    private void BtnComputerMgmt_Click( object sender, System.EventArgs e ) =>
        _Owner.BtnComputerMgmt_Click( sender, e );

    private void BtnSystemInfo_Click( object sender, System.EventArgs e ) =>
        _Owner.BtnSystemInfo_Click( sender, e );

    private void BtnPerfMonitor_Click( object sender, System.EventArgs e ) =>
        _Owner.BtnPerfMonitor_Click( sender, e );

    private void BtnResourceMonitor_Click( object sender, System.EventArgs e ) =>
        _Owner.BtnResourceMonitor_Click( sender, e );

    private void BtnDeviceManager_Click( object sender, System.EventArgs e ) =>
        _Owner.BtnDeviceManager_Click( sender, e );

    private void BtnLocalUsers_Click( object sender, System.EventArgs e ) =>
        _Owner.BtnLocalUsers_Click( sender, e );

    private void BtnFirewall_Click( object sender, System.EventArgs e ) =>
        _Owner.BtnFirewall_Click( sender, e );

    private void BtnSharedFolders_Click( object sender, System.EventArgs e ) =>
        _Owner.BtnSharedFolders_Click( sender, e );

    private void BtnCertManager_Click( object sender, System.EventArgs e ) =>
        _Owner.BtnCertManager_Click( sender, e );

    private void BtnWmiControl_Click( object sender, System.EventArgs e ) =>
        _Owner.BtnWmiControl_Click( sender, e );

    private void BtnAdvancedSystemProps_Click( object sender, System.EventArgs e ) =>
        _Owner.BtnAdvancedSystemProps_Click( sender, e );

    private void BtnPerformanceOptions_Click( object sender, System.EventArgs e ) =>
        _Owner.BtnPerformanceOptions_Click( sender, e );

    private void BtnRemoteSettings_Click( object sender, System.EventArgs e ) =>
        _Owner.BtnRemoteSettings_Click( sender, e );

    private void BtnReliabilityMonitor_Click( object sender, System.EventArgs e ) =>
        _Owner.BtnReliabilityMonitor_Click( sender, e );

    private void BtnWindowsFeatures_Click( object sender, System.EventArgs e ) =>
        _Owner.BtnWindowsFeatures_Click( sender, e );

    private void BtnProgramsFeatures_Click( object sender, System.EventArgs e ) =>
        _Owner.BtnProgramsFeatures_Click( sender, e );

    private void BtnSystemConfig_Click( object sender, System.EventArgs e ) =>
        _Owner.BtnSystemConfig_Click( sender, e );

    private void Close_Button_Click( object sender, EventArgs e )
    {
        this.Close();
    }
  }
}
