namespace Admin_Tools
{
  partial class Commands_Form
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void                  Dispose( bool disposing )
    {
      if ( disposing && ( components != null ) )
      {
        components.Dispose();
      }
      base.Dispose( disposing );
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      txtOutput = new RichTextBox();
      BtnClear = new Button();
      BtnCopyAll = new Button();
      BtnSave = new Button();
      BtnClose = new Button();
      Commands_Label = new Label();
      Message_Textbox = new TextBox();
      grpSecurity = new GroupBox();
      BtnNetLocalgroup = new Button();
      BtnGpResult = new Button();
      BtnFirewallProfiles = new Button();
      BtnAuditPolicy = new Button();
      grpHealth = new GroupBox();
      BtnSfcVerify = new Button();
      BtnBatteryReport = new Button();
      BtnSystemEvents = new Button();
      BtnUptime = new Button();
      BtnSecurityEvents = new Button();
      BtnDismCheckHealth = new Button();
      grpDisk = new GroupBox();
      BtnFreeSpacebyDrive = new Button();
      BtnChkdsk = new Button();
      BtnVol = new Button();
      BtnFsutilDirty = new Button();
      grpSystem = new GroupBox();
      BtnSystemInfo = new Button();
      BtnHostname = new Button();
      BtnTasklist = new Button();
      BtnTasklistSvc = new Button();
      BtnScQuery = new Button();
      grpNetwork = new GroupBox();
      BtnIpconfig = new Button();
      BtnArp = new Button();
      BtnRoute = new Button();
      BtnNetstat = new Button();
      BtnNslookup = new Button();
      BtnFlushDns = new Button();
      BtnNetstatOwner = new Button();
      BtnDisplayDns = new Button();
      grpIdentity = new GroupBox();
      BtnWhoami = new Button();
      BtnNetUser = new Button();
      BtnLocalAdmins = new Button();
      BtnQueryUser = new Button();
      BtnNetSession = new Button();
      BtnWhoamiPriv = new Button();
      grpSystemConfig = new GroupBox();
      BtnDriverQuery = new Button();
      BtnHotfixes = new Button();
      BtnUpdates = new Button();
      BtnBcdeditEnum = new Button();
      BtnStartupEntries = new Button();
      Commands_Panel = new Panel();
      lvUpdates = new ListView();
      lvHotfixes = new ListView();
      grpSecurity.SuspendLayout();
      grpHealth.SuspendLayout();
      grpDisk.SuspendLayout();
      grpSystem.SuspendLayout();
      grpNetwork.SuspendLayout();
      grpIdentity.SuspendLayout();
      grpSystemConfig.SuspendLayout();
      Commands_Panel.SuspendLayout();
      SuspendLayout();
      // 
      // txtOutput
      // 
      txtOutput.Anchor =  AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      txtOutput.BackColor = Color.FromArgb(   30,   30,   30 );
      txtOutput.BorderStyle = BorderStyle.FixedSingle;
      txtOutput.DetectUrls = false;
      txtOutput.Font = new Font( "Consolas", 9.5F );
      txtOutput.ForeColor = Color.Gainsboro;
      txtOutput.Location = new Point( 586, 26 );
      txtOutput.Name = "txtOutput";
      txtOutput.ReadOnly = true;
      txtOutput.Size = new Size( 715, 511 );
      txtOutput.TabIndex = 6;
      txtOutput.Text = "";
      txtOutput.WordWrap = false;
      // 
      // BtnClear
      // 
      BtnClear.Anchor =  AnchorStyles.Bottom | AnchorStyles.Right;
      BtnClear.Location = new Point( 597, 544 );
      BtnClear.Name = "BtnClear";
      BtnClear.Size = new Size( 90, 28 );
      BtnClear.TabIndex = 7;
      BtnClear.Text = "Clear";
      BtnClear.UseVisualStyleBackColor = true;
      BtnClear.Click += Btn_Clear_Click;
      // 
      // BtnCopyAll
      // 
      BtnCopyAll.Anchor =  AnchorStyles.Bottom | AnchorStyles.Right;
      BtnCopyAll.Location = new Point( 693, 544 );
      BtnCopyAll.Name = "BtnCopyAll";
      BtnCopyAll.Size = new Size( 90, 28 );
      BtnCopyAll.TabIndex = 8;
      BtnCopyAll.Text = "Copy All";
      BtnCopyAll.UseVisualStyleBackColor = true;
      BtnCopyAll.Click += Btn_Copy_All_Click;
      // 
      // BtnSave
      // 
      BtnSave.Anchor =  AnchorStyles.Bottom | AnchorStyles.Right;
      BtnSave.Location = new Point( 789, 544 );
      BtnSave.Name = "BtnSave";
      BtnSave.Size = new Size( 90, 28 );
      BtnSave.TabIndex = 9;
      BtnSave.Text = "Save...";
      BtnSave.UseVisualStyleBackColor = true;
      BtnSave.Click += Btn_Save_Click;
      // 
      // BtnClose
      // 
      BtnClose.Anchor =  AnchorStyles.Bottom | AnchorStyles.Right;
      BtnClose.Location = new Point( 1199, 544 );
      BtnClose.Name = "BtnClose";
      BtnClose.Size = new Size( 90, 28 );
      BtnClose.TabIndex = 10;
      BtnClose.Text = "Close";
      BtnClose.UseVisualStyleBackColor = true;
      BtnClose.Click += Btn_Close_Click;
      // 
      // Commands_Label
      // 
      Commands_Label.Anchor =  AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      Commands_Label.AutoSize = true;
      Commands_Label.Location = new Point( 12, 9 );
      Commands_Label.Name = "Commands_Label";
      Commands_Label.Size = new Size( 72, 15 );
      Commands_Label.TabIndex = 12;
      Commands_Label.Text = "Commands:";
      // 
      // Message_Textbox
      // 
      Message_Textbox.BackColor = SystemColors.Control;
      Message_Textbox.BorderStyle = BorderStyle.None;
      Message_Textbox.Location = new Point( 430, 643 );
      Message_Textbox.Name = "Message_Textbox";
      Message_Textbox.Size = new Size( 598, 16 );
      Message_Textbox.TabIndex = 13;
      // 
      // grpSecurity
      // 
      grpSecurity.Controls.Add( BtnNetLocalgroup );
      grpSecurity.Controls.Add( BtnGpResult );
      grpSecurity.Controls.Add( BtnFirewallProfiles );
      grpSecurity.Controls.Add( BtnAuditPolicy );
      grpSecurity.Location = new Point( 366, 237 );
      grpSecurity.Name = "grpSecurity";
      grpSecurity.Size = new Size( 162, 156 );
      grpSecurity.TabIndex = 11;
      grpSecurity.TabStop = false;
      grpSecurity.Text = "Security && Policy";
      // 
      // BtnNetLocalgroup
      // 
      BtnNetLocalgroup.Location = new Point( 12, 22 );
      BtnNetLocalgroup.Name = "BtnNetLocalgroup";
      BtnNetLocalgroup.Size = new Size( 137, 28 );
      BtnNetLocalgroup.TabIndex = 0;
      BtnNetLocalgroup.Text = "Local Groups (all)";
      BtnNetLocalgroup.UseVisualStyleBackColor = true;
      BtnNetLocalgroup.Click += BtnNetLocalgroup_Click;
      // 
      // BtnGpResult
      // 
      BtnGpResult.Location = new Point( 12, 54 );
      BtnGpResult.Name = "BtnGpResult";
      BtnGpResult.Size = new Size( 137, 28 );
      BtnGpResult.TabIndex = 1;
      BtnGpResult.Text = "Resultant Policy";
      BtnGpResult.UseVisualStyleBackColor = true;
      BtnGpResult.Click += BtnGpResult_Click;
      // 
      // BtnFirewallProfiles
      // 
      BtnFirewallProfiles.Location = new Point( 12, 86 );
      BtnFirewallProfiles.Name = "BtnFirewallProfiles";
      BtnFirewallProfiles.Size = new Size( 137, 28 );
      BtnFirewallProfiles.TabIndex = 2;
      BtnFirewallProfiles.Text = "Firewall Profiles";
      BtnFirewallProfiles.UseVisualStyleBackColor = true;
      BtnFirewallProfiles.Click += BtnFirewallProfiles_Click;
      // 
      // BtnAuditPolicy
      // 
      BtnAuditPolicy.Location = new Point( 12, 118 );
      BtnAuditPolicy.Name = "BtnAuditPolicy";
      BtnAuditPolicy.Size = new Size( 137, 28 );
      BtnAuditPolicy.TabIndex = 3;
      BtnAuditPolicy.Text = "Audit Policy";
      BtnAuditPolicy.UseVisualStyleBackColor = true;
      BtnAuditPolicy.Click += BtnAuditPolicy_Click;
      // 
      // grpHealth
      // 
      grpHealth.Controls.Add( BtnSfcVerify );
      grpHealth.Controls.Add( BtnBatteryReport );
      grpHealth.Controls.Add( BtnSystemEvents );
      grpHealth.Controls.Add( BtnUptime );
      grpHealth.Controls.Add( BtnSecurityEvents );
      grpHealth.Controls.Add( BtnDismCheckHealth );
      grpHealth.Location = new Point( 366, 7 );
      grpHealth.Name = "grpHealth";
      grpHealth.Size = new Size( 174, 220 );
      grpHealth.TabIndex = 9;
      grpHealth.TabStop = false;
      grpHealth.Text = "Health && Logs";
      // 
      // BtnSfcVerify
      // 
      BtnSfcVerify.Location = new Point( 12, 22 );
      BtnSfcVerify.Name = "BtnSfcVerify";
      BtnSfcVerify.Size = new Size( 147, 28 );
      BtnSfcVerify.TabIndex = 0;
      BtnSfcVerify.Text = "Verify System Files (slow)";
      BtnSfcVerify.UseVisualStyleBackColor = true;
      BtnSfcVerify.Click += BtnSfcVerify_Click;
      // 
      // BtnBatteryReport
      // 
      BtnBatteryReport.Location = new Point( 12, 54 );
      BtnBatteryReport.Name = "BtnBatteryReport";
      BtnBatteryReport.Size = new Size( 147, 28 );
      BtnBatteryReport.TabIndex = 1;
      BtnBatteryReport.Text = "Battery Report";
      BtnBatteryReport.UseVisualStyleBackColor = true;
      BtnBatteryReport.Click += BtnBatteryReport_Click;
      // 
      // BtnSystemEvents
      // 
      BtnSystemEvents.Location = new Point( 12, 86 );
      BtnSystemEvents.Name = "BtnSystemEvents";
      BtnSystemEvents.Size = new Size( 147, 28 );
      BtnSystemEvents.TabIndex = 2;
      BtnSystemEvents.Text = "Last 20 System Events";
      BtnSystemEvents.UseVisualStyleBackColor = true;
      BtnSystemEvents.Click += BtnSystemEvents_Click;
      // 
      // BtnUptime
      // 
      BtnUptime.Location = new Point( 12, 118 );
      BtnUptime.Name = "BtnUptime";
      BtnUptime.Size = new Size( 147, 28 );
      BtnUptime.TabIndex = 3;
      BtnUptime.Text = "Uptime / Boot Time";
      BtnUptime.UseVisualStyleBackColor = true;
      BtnUptime.Click += BtnUptime_Click;
      // 
      // BtnSecurityEvents
      // 
      BtnSecurityEvents.Location = new Point( 12, 150 );
      BtnSecurityEvents.Name = "BtnSecurityEvents";
      BtnSecurityEvents.Size = new Size( 147, 28 );
      BtnSecurityEvents.TabIndex = 4;
      BtnSecurityEvents.Text = "Last 20 Security Events";
      BtnSecurityEvents.UseVisualStyleBackColor = true;
      BtnSecurityEvents.Click += BtnSecurityEvents_Click;
      // 
      // BtnDismCheckHealth
      // 
      BtnDismCheckHealth.Location = new Point( 12, 182 );
      BtnDismCheckHealth.Name = "BtnDismCheckHealth";
      BtnDismCheckHealth.Size = new Size( 147, 28 );
      BtnDismCheckHealth.TabIndex = 5;
      BtnDismCheckHealth.Text = "Component Store Health";
      BtnDismCheckHealth.UseVisualStyleBackColor = true;
      BtnDismCheckHealth.Click += BtnDismCheckHealth_Click;
      // 
      // grpDisk
      // 
      grpDisk.Controls.Add( BtnFreeSpacebyDrive );
      grpDisk.Controls.Add( BtnChkdsk );
      grpDisk.Controls.Add( BtnVol );
      grpDisk.Controls.Add( BtnFsutilDirty );
      grpDisk.ForeColor = SystemColors.ControlText;
      grpDisk.Location = new Point( 192, 201 );
      grpDisk.Name = "grpDisk";
      grpDisk.Size = new Size( 162, 156 );
      grpDisk.TabIndex = 8;
      grpDisk.TabStop = false;
      grpDisk.Text = "Disk && Storage";
      // 
      // BtnFreeSpacebyDrive
      // 
      BtnFreeSpacebyDrive.Location = new Point( 12, 22 );
      BtnFreeSpacebyDrive.Name = "BtnFreeSpacebyDrive";
      BtnFreeSpacebyDrive.Size = new Size( 137, 28 );
      BtnFreeSpacebyDrive.TabIndex = 0;
      BtnFreeSpacebyDrive.Text = "Free Space by Drive";
      BtnFreeSpacebyDrive.UseVisualStyleBackColor = true;
      BtnFreeSpacebyDrive.Click += BtnFreeSpacebyDrive_Click;
      // 
      // BtnChkdsk
      // 
      BtnChkdsk.Location = new Point( 12, 54 );
      BtnChkdsk.Name = "BtnChkdsk";
      BtnChkdsk.Size = new Size( 137, 28 );
      BtnChkdsk.TabIndex = 1;
      BtnChkdsk.Text = "Disk Status (slow)";
      BtnChkdsk.UseVisualStyleBackColor = true;
      BtnChkdsk.Click += BtnChkdsk_Click;
      // 
      // BtnVol
      // 
      BtnVol.Location = new Point( 12, 86 );
      BtnVol.Name = "BtnVol";
      BtnVol.Size = new Size( 137, 28 );
      BtnVol.TabIndex = 2;
      BtnVol.Text = "Volume Label && Serial";
      BtnVol.UseVisualStyleBackColor = true;
      BtnVol.Click += BtnVol_Click;
      // 
      // BtnFsutilDirty
      // 
      BtnFsutilDirty.Location = new Point( 12, 118 );
      BtnFsutilDirty.Name = "BtnFsutilDirty";
      BtnFsutilDirty.Size = new Size( 137, 28 );
      BtnFsutilDirty.TabIndex = 3;
      BtnFsutilDirty.Text = "Dirty Bit Check (C:)";
      BtnFsutilDirty.UseVisualStyleBackColor = true;
      BtnFsutilDirty.Click += BtnFsutilDirty_Click;
      // 
      // grpSystem
      // 
      grpSystem.Controls.Add( BtnSystemInfo );
      grpSystem.Controls.Add( BtnHostname );
      grpSystem.Controls.Add( BtnTasklist );
      grpSystem.Controls.Add( BtnTasklistSvc );
      grpSystem.Controls.Add( BtnScQuery );
      grpSystem.Location = new Point( 192, 7 );
      grpSystem.Name = "grpSystem";
      grpSystem.Size = new Size( 162, 188 );
      grpSystem.TabIndex = 7;
      grpSystem.TabStop = false;
      grpSystem.Text = "System State";
      // 
      // BtnSystemInfo
      // 
      BtnSystemInfo.Location = new Point( 12, 22 );
      BtnSystemInfo.Name = "BtnSystemInfo";
      BtnSystemInfo.Size = new Size( 137, 28 );
      BtnSystemInfo.TabIndex = 0;
      BtnSystemInfo.Text = "System Info (slow)";
      BtnSystemInfo.UseVisualStyleBackColor = true;
      BtnSystemInfo.Click += BtnSystemInfo_Click;
      // 
      // BtnHostname
      // 
      BtnHostname.Location = new Point( 12, 54 );
      BtnHostname.Name = "BtnHostname";
      BtnHostname.Size = new Size( 137, 28 );
      BtnHostname.TabIndex = 1;
      BtnHostname.Text = "Hostname";
      BtnHostname.UseVisualStyleBackColor = true;
      BtnHostname.Click += BtnHostname_Click;
      // 
      // BtnTasklist
      // 
      BtnTasklist.Location = new Point( 12, 86 );
      BtnTasklist.Name = "BtnTasklist";
      BtnTasklist.Size = new Size( 137, 28 );
      BtnTasklist.TabIndex = 2;
      BtnTasklist.Text = "Running Processes";
      BtnTasklist.UseVisualStyleBackColor = true;
      BtnTasklist.Click += BtnTasklist_Click;
      // 
      // BtnTasklistSvc
      // 
      BtnTasklistSvc.Location = new Point( 12, 118 );
      BtnTasklistSvc.Name = "BtnTasklistSvc";
      BtnTasklistSvc.Size = new Size( 137, 28 );
      BtnTasklistSvc.TabIndex = 3;
      BtnTasklistSvc.Text = "Processes + Services";
      BtnTasklistSvc.UseVisualStyleBackColor = true;
      BtnTasklistSvc.Click += BtnTasklistSvc_Click;
      // 
      // BtnScQuery
      // 
      BtnScQuery.Location = new Point( 12, 150 );
      BtnScQuery.Name = "BtnScQuery";
      BtnScQuery.Size = new Size( 137, 28 );
      BtnScQuery.TabIndex = 4;
      BtnScQuery.Text = "Running Services";
      BtnScQuery.UseVisualStyleBackColor = true;
      BtnScQuery.Click += BtnScQuery_Click;
      // 
      // grpNetwork
      // 
      grpNetwork.Controls.Add( BtnIpconfig );
      grpNetwork.Controls.Add( BtnArp );
      grpNetwork.Controls.Add( BtnRoute );
      grpNetwork.Controls.Add( BtnNetstat );
      grpNetwork.Controls.Add( BtnNslookup );
      grpNetwork.Controls.Add( BtnFlushDns );
      grpNetwork.Controls.Add( BtnNetstatOwner );
      grpNetwork.Controls.Add( BtnDisplayDns );
      grpNetwork.Location = new Point( 12, 237 );
      grpNetwork.Name = "grpNetwork";
      grpNetwork.Size = new Size( 174, 284 );
      grpNetwork.TabIndex = 6;
      grpNetwork.TabStop = false;
      grpNetwork.Text = "Network";
      // 
      // BtnIpconfig
      // 
      BtnIpconfig.Location = new Point( 12, 22 );
      BtnIpconfig.Name = "BtnIpconfig";
      BtnIpconfig.Size = new Size( 147, 28 );
      BtnIpconfig.TabIndex = 0;
      BtnIpconfig.Text = "IP Configuration (full)";
      BtnIpconfig.UseVisualStyleBackColor = true;
      BtnIpconfig.Click += BtnIpconfig_Click;
      // 
      // BtnArp
      // 
      BtnArp.Location = new Point( 12, 54 );
      BtnArp.Name = "BtnArp";
      BtnArp.Size = new Size( 147, 28 );
      BtnArp.TabIndex = 1;
      BtnArp.Text = "ARP Cache";
      BtnArp.UseVisualStyleBackColor = true;
      BtnArp.Click += BtnArp_Click;
      // 
      // BtnRoute
      // 
      BtnRoute.Location = new Point( 12, 86 );
      BtnRoute.Name = "BtnRoute";
      BtnRoute.Size = new Size( 147, 28 );
      BtnRoute.TabIndex = 2;
      BtnRoute.Text = "Routing Table";
      BtnRoute.UseVisualStyleBackColor = true;
      BtnRoute.Click += BtnRoute_Click;
      // 
      // BtnNetstat
      // 
      BtnNetstat.Location = new Point( 12, 118 );
      BtnNetstat.Name = "BtnNetstat";
      BtnNetstat.Size = new Size( 147, 28 );
      BtnNetstat.TabIndex = 3;
      BtnNetstat.Text = "Connections + PIDs";
      BtnNetstat.UseVisualStyleBackColor = true;
      BtnNetstat.Click += BtnNetstat_Click;
      // 
      // BtnNslookup
      // 
      BtnNslookup.Location = new Point( 12, 150 );
      BtnNslookup.Name = "BtnNslookup";
      BtnNslookup.Size = new Size( 147, 28 );
      BtnNslookup.TabIndex = 4;
      BtnNslookup.Text = "DNS Check";
      BtnNslookup.UseVisualStyleBackColor = true;
      BtnNslookup.Click += BtnNslookup_Click;
      // 
      // BtnFlushDns
      // 
      BtnFlushDns.Location = new Point( 12, 182 );
      BtnFlushDns.Name = "BtnFlushDns";
      BtnFlushDns.Size = new Size( 147, 28 );
      BtnFlushDns.TabIndex = 5;
      BtnFlushDns.Text = "Flush DNS Cache";
      BtnFlushDns.UseVisualStyleBackColor = true;
      BtnFlushDns.Click += BtnFlushDns_Click;
      // 
      // BtnNetstatOwner
      // 
      BtnNetstatOwner.Location = new Point( 12, 214 );
      BtnNetstatOwner.Name = "BtnNetstatOwner";
      BtnNetstatOwner.Size = new Size( 147, 28 );
      BtnNetstatOwner.TabIndex = 6;
      BtnNetstatOwner.Text = "Connections + Owning App";
      BtnNetstatOwner.UseVisualStyleBackColor = true;
      BtnNetstatOwner.Click += BtnNetstatOwner_Click;
      // 
      // BtnDisplayDns
      // 
      BtnDisplayDns.Location = new Point( 12, 246 );
      BtnDisplayDns.Name = "BtnDisplayDns";
      BtnDisplayDns.Size = new Size( 147, 28 );
      BtnDisplayDns.TabIndex = 7;
      BtnDisplayDns.Text = "DNS Cache Contents";
      BtnDisplayDns.UseVisualStyleBackColor = true;
      BtnDisplayDns.Click += BtnDisplayDns_Click;
      // 
      // grpIdentity
      // 
      grpIdentity.Controls.Add( BtnWhoami );
      grpIdentity.Controls.Add( BtnNetUser );
      grpIdentity.Controls.Add( BtnLocalAdmins );
      grpIdentity.Controls.Add( BtnQueryUser );
      grpIdentity.Controls.Add( BtnNetSession );
      grpIdentity.Controls.Add( BtnWhoamiPriv );
      grpIdentity.Location = new Point( 12, 7 );
      grpIdentity.Name = "grpIdentity";
      grpIdentity.Size = new Size( 174, 220 );
      grpIdentity.TabIndex = 5;
      grpIdentity.TabStop = false;
      grpIdentity.Text = "Identity && Sessions";
      // 
      // BtnWhoami
      // 
      BtnWhoami.Location = new Point( 12, 22 );
      BtnWhoami.Name = "BtnWhoami";
      BtnWhoami.Size = new Size( 147, 28 );
      BtnWhoami.TabIndex = 0;
      BtnWhoami.Text = "Who Am I (full)";
      BtnWhoami.UseVisualStyleBackColor = true;
      BtnWhoami.Click += BtnWhoami_Click;
      // 
      // BtnNetUser
      // 
      BtnNetUser.Location = new Point( 12, 54 );
      BtnNetUser.Name = "BtnNetUser";
      BtnNetUser.Size = new Size( 147, 28 );
      BtnNetUser.TabIndex = 1;
      BtnNetUser.Text = "Local Accounts";
      BtnNetUser.UseVisualStyleBackColor = true;
      BtnNetUser.Click += BtnNetUser_Click;
      // 
      // BtnLocalAdmins
      // 
      BtnLocalAdmins.Location = new Point( 12, 86 );
      BtnLocalAdmins.Name = "BtnLocalAdmins";
      BtnLocalAdmins.Size = new Size( 147, 28 );
      BtnLocalAdmins.TabIndex = 2;
      BtnLocalAdmins.Text = "Local Administrators";
      BtnLocalAdmins.UseVisualStyleBackColor = true;
      BtnLocalAdmins.Click += BtnLocalAdmins_Click;
      // 
      // BtnQueryUser
      // 
      BtnQueryUser.Location = new Point( 12, 118 );
      BtnQueryUser.Name = "BtnQueryUser";
      BtnQueryUser.Size = new Size( 147, 28 );
      BtnQueryUser.TabIndex = 3;
      BtnQueryUser.Text = "Logged-On Sessions";
      BtnQueryUser.UseVisualStyleBackColor = true;
      BtnQueryUser.Click += BtnQueryUser_Click;
      // 
      // BtnNetSession
      // 
      BtnNetSession.Location = new Point( 12, 150 );
      BtnNetSession.Name = "BtnNetSession";
      BtnNetSession.Size = new Size( 147, 28 );
      BtnNetSession.TabIndex = 4;
      BtnNetSession.Text = "Inbound SMB Sessions";
      BtnNetSession.UseVisualStyleBackColor = true;
      BtnNetSession.Click += BtnNetSession_Click;
      // 
      // BtnWhoamiPriv
      // 
      BtnWhoamiPriv.Location = new Point( 12, 182 );
      BtnWhoamiPriv.Name = "BtnWhoamiPriv";
      BtnWhoamiPriv.Size = new Size( 147, 28 );
      BtnWhoamiPriv.TabIndex = 5;
      BtnWhoamiPriv.Text = "My Privileges";
      BtnWhoamiPriv.UseVisualStyleBackColor = true;
      BtnWhoamiPriv.Click += BtnWhoamiPriv_Click;
      // 
      // grpSystemConfig
      // 
      grpSystemConfig.Controls.Add( BtnDriverQuery );
      grpSystemConfig.Controls.Add( BtnHotfixes );
      grpSystemConfig.Controls.Add( BtnUpdates );
      grpSystemConfig.Controls.Add( BtnBcdeditEnum );
      grpSystemConfig.Controls.Add( BtnStartupEntries );
      grpSystemConfig.Location = new Point( 192, 363 );
      grpSystemConfig.Name = "grpSystemConfig";
      grpSystemConfig.Size = new Size( 162, 188 );
      grpSystemConfig.TabIndex = 10;
      grpSystemConfig.TabStop = false;
      grpSystemConfig.Text = "System Config";
      // 
      // BtnDriverQuery
      // 
      BtnDriverQuery.Location = new Point( 12, 22 );
      BtnDriverQuery.Name = "BtnDriverQuery";
      BtnDriverQuery.Size = new Size( 137, 28 );
      BtnDriverQuery.TabIndex = 0;
      BtnDriverQuery.Text = "Installed Drivers";
      BtnDriverQuery.UseVisualStyleBackColor = true;
      BtnDriverQuery.Click += BtnDriverQuery_Click;
      // 
      // BtnHotfixes
      // 
      BtnHotfixes.Location = new Point( 12, 54 );
      BtnHotfixes.Name = "BtnHotfixes";
      BtnHotfixes.Size = new Size( 137, 28 );
      BtnHotfixes.TabIndex = 1;
      BtnHotfixes.Text = "Installed Hotfixes";
      BtnHotfixes.UseVisualStyleBackColor = true;
      BtnHotfixes.Click += BtnHotfixes_Click;
      // 
      // BtnUpdates
      // 
      BtnUpdates.Location = new Point( 12, 86 );
      BtnUpdates.Name = "BtnUpdates";
      BtnUpdates.Size = new Size( 137, 28 );
      BtnUpdates.TabIndex = 2;
      BtnUpdates.Text = "Update History";
      BtnUpdates.UseVisualStyleBackColor = true;
      BtnUpdates.Click += BtnUpdates_Click;
      // 
      // BtnBcdeditEnum
      // 
      BtnBcdeditEnum.Location = new Point( 12, 118 );
      BtnBcdeditEnum.Name = "BtnBcdeditEnum";
      BtnBcdeditEnum.Size = new Size( 137, 28 );
      BtnBcdeditEnum.TabIndex = 3;
      BtnBcdeditEnum.Text = "Boot Configuration";
      BtnBcdeditEnum.UseVisualStyleBackColor = true;
      BtnBcdeditEnum.Click += BtnBcdeditEnum_Click;
      // 
      // BtnStartupEntries
      // 
      BtnStartupEntries.Location = new Point( 12, 150 );
      BtnStartupEntries.Name = "BtnStartupEntries";
      BtnStartupEntries.Size = new Size( 137, 28 );
      BtnStartupEntries.TabIndex = 4;
      BtnStartupEntries.Text = "Startup Entries";
      BtnStartupEntries.UseVisualStyleBackColor = true;
      BtnStartupEntries.Click += BtnStartupEntries_Click;
      // 
      // Commands_Panel
      // 
      Commands_Panel.BorderStyle = BorderStyle.FixedSingle;
      Commands_Panel.Controls.Add( grpSystemConfig );
      Commands_Panel.Controls.Add( grpIdentity );
      Commands_Panel.Controls.Add( grpNetwork );
      Commands_Panel.Controls.Add( grpSystem );
      Commands_Panel.Controls.Add( grpDisk );
      Commands_Panel.Controls.Add( grpHealth );
      Commands_Panel.Controls.Add( grpSecurity );
      Commands_Panel.Location = new Point( 12, 27 );
      Commands_Panel.Name = "Commands_Panel";
      Commands_Panel.Size = new Size( 550, 561 );
      Commands_Panel.TabIndex = 11;
      //
      // lvUpdates
      //
      lvUpdates.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      lvUpdates.BackColor = Color.FromArgb( 30, 30, 30 );
      lvUpdates.BorderStyle = BorderStyle.FixedSingle;
      lvUpdates.ForeColor = Color.Gainsboro;
      lvUpdates.FullRowSelect = true;
      lvUpdates.HideSelection = false;
      lvUpdates.Location = new Point( 586, 33 );
      lvUpdates.MultiSelect = false;
      lvUpdates.Name = "lvUpdates";
      lvUpdates.Size = new Size( 715, 500 );
      lvUpdates.TabIndex = 14;
      lvUpdates.UseCompatibleStateImageBehavior = false;
      lvUpdates.View = View.Details;
      lvUpdates.Visible = false;
      lvUpdates.Columns.Add( "Date", 130 );
      lvUpdates.Columns.Add( "Title", 300 );
      lvUpdates.Columns.Add( "KB", 80 );
      lvUpdates.Columns.Add( "Operation", 70 );
      lvUpdates.Columns.Add( "Result", 100 );
      lvUpdates.DoubleClick += LvUpdates_DoubleClick;
      //
      // lvHotfixes
      //
      lvHotfixes.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      lvHotfixes.BackColor = Color.FromArgb( 30, 30, 30 );
      lvHotfixes.BorderStyle = BorderStyle.FixedSingle;
      lvHotfixes.ForeColor = Color.Gainsboro;
      lvHotfixes.FullRowSelect = true;
      lvHotfixes.HideSelection = false;
      lvHotfixes.Location = new Point( 586, 33 );
      lvHotfixes.MultiSelect = false;
      lvHotfixes.Name = "lvHotfixes";
      lvHotfixes.Size = new Size( 715, 500 );
      lvHotfixes.TabIndex = 15;
      lvHotfixes.UseCompatibleStateImageBehavior = false;
      lvHotfixes.View = View.Details;
      lvHotfixes.Visible = false;
      lvHotfixes.Columns.Add( "HotFix ID", 110 );
      lvHotfixes.Columns.Add( "Description", 160 );
      lvHotfixes.Columns.Add( "Installed On", 110 );
      lvHotfixes.Columns.Add( "Installed By", 200 );
      lvHotfixes.DoubleClick += LvHotfixes_DoubleClick;
      //
      // Commands_Form
      //
      AutoScaleDimensions = new SizeF( 7F, 15F );
      AutoScaleMode = AutoScaleMode.Font;
      CancelButton = BtnClose;
      ClientSize = new Size( 1322, 611 );
      Controls.Add( Message_Textbox );
      Controls.Add( Commands_Label );
      Controls.Add( Commands_Panel );
      Controls.Add( BtnClear );
      Controls.Add( BtnCopyAll );
      Controls.Add( BtnSave );
      Controls.Add( BtnClose );
      Controls.Add( txtOutput );
      Controls.Add( lvUpdates );
      Controls.Add( lvHotfixes );
      Font = new Font( "Segoe UI", 9F );
      MinimizeBox = false;
      MinimumSize = new Size( 1016, 650 );
      Name = "Commands_Form";
      ShowInTaskbar = false;
      StartPosition = FormStartPosition.CenterParent;
      Text = "Admin Commands";
      grpSecurity.ResumeLayout( false );
      grpHealth.ResumeLayout( false );
      grpDisk.ResumeLayout( false );
      grpSystem.ResumeLayout( false );
      grpNetwork.ResumeLayout( false );
      grpIdentity.ResumeLayout( false );
      grpSystemConfig.ResumeLayout( false );
      Commands_Panel.ResumeLayout( false );
      ResumeLayout( false );
      PerformLayout();
    }

    #endregion
    private System.Windows.Forms.RichTextBox txtOutput;
    private System.Windows.Forms.Button      BtnClear;
    private System.Windows.Forms.Button      BtnCopyAll;
    private System.Windows.Forms.Button      BtnSave;
    private System.Windows.Forms.Button      BtnClose;
    private Label                            Commands_Label;
    private TextBox                          Message_Textbox;
    private GroupBox grpSecurity;
    private Button BtnNetLocalgroup;
    private Button BtnGpResult;
    private Button BtnFirewallProfiles;
    private Button BtnAuditPolicy;
    private GroupBox grpHealth;
    private Button BtnSfcVerify;
    private Button BtnBatteryReport;
    private Button BtnSystemEvents;
    private Button BtnUptime;
    private Button BtnSecurityEvents;
    private Button BtnDismCheckHealth;
    private GroupBox grpDisk;
    private Button BtnFreeSpacebyDrive;
    private Button BtnChkdsk;
    private Button BtnVol;
    private Button BtnFsutilDirty;
    private GroupBox grpSystem;
    private Button BtnSystemInfo;
    private Button BtnHostname;
    private Button BtnTasklist;
    private Button BtnTasklistSvc;
    private Button BtnScQuery;
    private GroupBox grpNetwork;
    private Button BtnIpconfig;
    private Button BtnArp;
    private Button BtnRoute;
    private Button BtnNetstat;
    private Button BtnNslookup;
    private Button BtnFlushDns;
    private Button BtnNetstatOwner;
    private Button BtnDisplayDns;
    private GroupBox grpIdentity;
    private Button BtnWhoami;
    private Button BtnNetUser;
    private Button BtnLocalAdmins;
    private Button BtnQueryUser;
    private Button BtnNetSession;
    private Button BtnWhoamiPriv;
    private GroupBox grpSystemConfig;
    private Button BtnDriverQuery;
    private Button BtnHotfixes;
    private Button BtnUpdates;
    private Button BtnBcdeditEnum;
    private Button BtnStartupEntries;
    private Panel Commands_Panel;
    private System.Windows.Forms.ListView lvUpdates;
    private System.Windows.Forms.ListView lvHotfixes;
  }
}
