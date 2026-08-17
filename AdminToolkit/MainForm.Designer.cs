namespace Admin_Tools
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      components = new System.ComponentModel.Container();
      BtnViewLog = new Button();
      grpTools = new GroupBox();
      BtnFirewall = new Button();
      BtnLocalUsers = new Button();
      BtnDeviceManager = new Button();
      BtnResourceMonitor = new Button();
      BtnPerfMonitor = new Button();
      BtnSystemInfo = new Button();
      BtnComputerMgmt = new Button();
      BtnDiskManagement = new Button();
      BtnServices = new Button();
      BtnEventViewer = new Button();
      BtnRegistryEditor = new Button();
      BtnRestoreWizard = new Button();
      BtnSystemProtection = new Button();
      BtnTaskScheduler = new Button();
      grpProcesses = new GroupBox();
      BtnRemoveClosed = new Button();
      BtnKillTool = new Button();
      BtnCloseTool = new Button();
      listViewProcesses = new ListView();
      colTool = new ColumnHeader();
      colPid = new ColumnHeader();
      colStarted = new ColumnHeader();
      colStatus = new ColumnHeader();
      Email_Settings_Button = new Button();
      Email_Log_Button = new Button();
      Enable_Registry_Backup_Button = new Button();
      Restore_Points_Button = new Button();
      statusStrip = new StatusStrip();
      statusLabel = new ToolStripStatusLabel();
      timerStatus = new System.Windows.Forms.Timer( components );
      Snapshot_Operations_Button = new Button();
      Help_Button = new Button();
      View_Log_Button = new Button();
      Quit_Button = new Button();
      Admin_Commands_Button = new Button();
      Remote_Tools_Button = new Button();
      Powershell_Button = new Button();
      CMD_Button = new Button();
      Printer_Button = new Button();
      Trace_Button = new Button();
      grpTools.SuspendLayout();
      grpProcesses.SuspendLayout();
      statusStrip.SuspendLayout();
      SuspendLayout();
      // 
      // BtnViewLog
      // 
      BtnViewLog.Location = new Point( 0, 0 );
      BtnViewLog.Name = "BtnViewLog";
      BtnViewLog.Size = new Size( 75, 23 );
      BtnViewLog.TabIndex = 0;
      // 
      // grpTools
      // 
      grpTools.Controls.Add( BtnFirewall );
      grpTools.Controls.Add( BtnLocalUsers );
      grpTools.Controls.Add( BtnDeviceManager );
      grpTools.Controls.Add( BtnResourceMonitor );
      grpTools.Controls.Add( BtnPerfMonitor );
      grpTools.Controls.Add( BtnSystemInfo );
      grpTools.Controls.Add( BtnComputerMgmt );
      grpTools.Controls.Add( BtnDiskManagement );
      grpTools.Controls.Add( BtnServices );
      grpTools.Controls.Add( BtnEventViewer );
      grpTools.Controls.Add( BtnRegistryEditor );
      grpTools.Controls.Add( BtnRestoreWizard );
      grpTools.Controls.Add( BtnSystemProtection );
      grpTools.Controls.Add( BtnTaskScheduler );
      grpTools.Location = new Point( 12, 12 );
      grpTools.Name = "grpTools";
      grpTools.Size = new Size( 325, 290 );
      grpTools.TabIndex = 1;
      grpTools.TabStop = false;
      grpTools.Text = "Admin Tools";
      // 
      // BtnFirewall
      // 
      BtnFirewall.Location = new Point( 166, 242 );
      BtnFirewall.Name = "BtnFirewall";
      BtnFirewall.Size = new Size( 140, 30 );
      BtnFirewall.TabIndex = 13;
      BtnFirewall.Text = "Windows Firewall";
      BtnFirewall.UseVisualStyleBackColor = true;
      BtnFirewall.Click += BtnFirewall_Click;
      // 
      // BtnLocalUsers
      // 
      BtnLocalUsers.Location = new Point( 14, 242 );
      BtnLocalUsers.Name = "BtnLocalUsers";
      BtnLocalUsers.Size = new Size( 140, 30 );
      BtnLocalUsers.TabIndex = 12;
      BtnLocalUsers.Text = "Local Users && Groups";
      BtnLocalUsers.UseVisualStyleBackColor = true;
      BtnLocalUsers.Click += BtnLocalUsers_Click;
      // 
      // BtnDeviceManager
      // 
      BtnDeviceManager.Location = new Point( 166, 206 );
      BtnDeviceManager.Name = "BtnDeviceManager";
      BtnDeviceManager.Size = new Size( 140, 30 );
      BtnDeviceManager.TabIndex = 11;
      BtnDeviceManager.Text = "Device Manager";
      BtnDeviceManager.UseVisualStyleBackColor = true;
      BtnDeviceManager.Click += BtnDeviceManager_Click;
      // 
      // BtnResourceMonitor
      // 
      BtnResourceMonitor.Location = new Point( 14, 206 );
      BtnResourceMonitor.Name = "BtnResourceMonitor";
      BtnResourceMonitor.Size = new Size( 140, 30 );
      BtnResourceMonitor.TabIndex = 10;
      BtnResourceMonitor.Text = "Resource Monitor";
      BtnResourceMonitor.UseVisualStyleBackColor = true;
      BtnResourceMonitor.Click += BtnResourceMonitor_Click;
      // 
      // BtnPerfMonitor
      // 
      BtnPerfMonitor.Location = new Point( 166, 170 );
      BtnPerfMonitor.Name = "BtnPerfMonitor";
      BtnPerfMonitor.Size = new Size( 140, 30 );
      BtnPerfMonitor.TabIndex = 9;
      BtnPerfMonitor.Text = "Performance Monitor";
      BtnPerfMonitor.UseVisualStyleBackColor = true;
      BtnPerfMonitor.Click += BtnPerfMonitor_Click;
      // 
      // BtnSystemInfo
      // 
      BtnSystemInfo.Location = new Point( 14, 170 );
      BtnSystemInfo.Name = "BtnSystemInfo";
      BtnSystemInfo.Size = new Size( 140, 30 );
      BtnSystemInfo.TabIndex = 8;
      BtnSystemInfo.Text = "System Info";
      BtnSystemInfo.UseVisualStyleBackColor = true;
      BtnSystemInfo.Click += BtnSystemInfo_Click;
      // 
      // BtnComputerMgmt
      // 
      BtnComputerMgmt.Location = new Point( 166, 134 );
      BtnComputerMgmt.Name = "BtnComputerMgmt";
      BtnComputerMgmt.Size = new Size( 140, 30 );
      BtnComputerMgmt.TabIndex = 7;
      BtnComputerMgmt.Text = "Computer Mgmt";
      BtnComputerMgmt.UseVisualStyleBackColor = true;
      BtnComputerMgmt.Click += BtnComputerMgmt_Click;
      // 
      // BtnDiskManagement
      // 
      BtnDiskManagement.Location = new Point( 14, 134 );
      BtnDiskManagement.Name = "BtnDiskManagement";
      BtnDiskManagement.Size = new Size( 140, 30 );
      BtnDiskManagement.TabIndex = 6;
      BtnDiskManagement.Text = "Disk Management";
      BtnDiskManagement.UseVisualStyleBackColor = true;
      BtnDiskManagement.Click += BtnDiskManagement_Click;
      // 
      // BtnServices
      // 
      BtnServices.Location = new Point( 166, 98 );
      BtnServices.Name = "BtnServices";
      BtnServices.Size = new Size( 140, 30 );
      BtnServices.TabIndex = 5;
      BtnServices.Text = "Services";
      BtnServices.UseVisualStyleBackColor = true;
      BtnServices.Click += BtnServices_Click;
      // 
      // BtnEventViewer
      // 
      BtnEventViewer.Location = new Point( 14, 98 );
      BtnEventViewer.Name = "BtnEventViewer";
      BtnEventViewer.Size = new Size( 140, 30 );
      BtnEventViewer.TabIndex = 4;
      BtnEventViewer.Text = "Event Viewer";
      BtnEventViewer.UseVisualStyleBackColor = true;
      BtnEventViewer.Click += BtnEventViewer_Click;
      // 
      // BtnRegistryEditor
      // 
      BtnRegistryEditor.Location = new Point( 168, 62 );
      BtnRegistryEditor.Name = "BtnRegistryEditor";
      BtnRegistryEditor.Size = new Size( 140, 30 );
      BtnRegistryEditor.TabIndex = 3;
      BtnRegistryEditor.Text = "Registry Editor";
      BtnRegistryEditor.UseVisualStyleBackColor = true;
      BtnRegistryEditor.Click += BtnRegistryEditor_Click;
      // 
      // BtnRestoreWizard
      // 
      BtnRestoreWizard.Location = new Point( 16, 62 );
      BtnRestoreWizard.Name = "BtnRestoreWizard";
      BtnRestoreWizard.Size = new Size( 140, 30 );
      BtnRestoreWizard.TabIndex = 2;
      BtnRestoreWizard.Text = "System Restore";
      BtnRestoreWizard.UseVisualStyleBackColor = true;
      BtnRestoreWizard.Click += BtnRestoreWizard_Click;
      // 
      // BtnSystemProtection
      // 
      BtnSystemProtection.Location = new Point( 166, 26 );
      BtnSystemProtection.Name = "BtnSystemProtection";
      BtnSystemProtection.Size = new Size( 140, 30 );
      BtnSystemProtection.TabIndex = 1;
      BtnSystemProtection.Text = "System Protection";
      BtnSystemProtection.UseVisualStyleBackColor = true;
      BtnSystemProtection.Click += BtnSystemProtection_Click;
      // 
      // BtnTaskScheduler
      // 
      BtnTaskScheduler.Location = new Point( 14, 26 );
      BtnTaskScheduler.Name = "BtnTaskScheduler";
      BtnTaskScheduler.Size = new Size( 140, 30 );
      BtnTaskScheduler.TabIndex = 0;
      BtnTaskScheduler.Text = "Task Scheduler";
      BtnTaskScheduler.UseVisualStyleBackColor = true;
      BtnTaskScheduler.Click += BtnTaskScheduler_Click;
      // 
      // grpProcesses
      // 
      grpProcesses.Controls.Add( BtnRemoveClosed );
      grpProcesses.Controls.Add( BtnKillTool );
      grpProcesses.Controls.Add( BtnCloseTool );
      grpProcesses.Controls.Add( listViewProcesses );
      grpProcesses.Location = new Point( 12, 308 );
      grpProcesses.Name = "grpProcesses";
      grpProcesses.Size = new Size( 615, 179 );
      grpProcesses.TabIndex = 2;
      grpProcesses.TabStop = false;
      grpProcesses.Text = "Launched Tools";
      // 
      // BtnRemoveClosed
      // 
      BtnRemoveClosed.Location = new Point( 241, 142 );
      BtnRemoveClosed.Name = "BtnRemoveClosed";
      BtnRemoveClosed.Size = new Size( 84, 30 );
      BtnRemoveClosed.TabIndex = 3;
      BtnRemoveClosed.Text = "Clear Closed";
      BtnRemoveClosed.UseVisualStyleBackColor = true;
      BtnRemoveClosed.Click += BtnRemoveClosed_Click;
      // 
      // BtnKillTool
      // 
      BtnKillTool.Location = new Point( 130, 142 );
      BtnKillTool.Name = "BtnKillTool";
      BtnKillTool.Size = new Size( 105, 30 );
      BtnKillTool.TabIndex = 2;
      BtnKillTool.Text = "End Task (force)";
      BtnKillTool.UseVisualStyleBackColor = true;
      BtnKillTool.Click += BtnKillTool_Click;
      // 
      // BtnCloseTool
      // 
      BtnCloseTool.Location = new Point( 14, 142 );
      BtnCloseTool.Name = "BtnCloseTool";
      BtnCloseTool.Size = new Size( 110, 30 );
      BtnCloseTool.TabIndex = 1;
      BtnCloseTool.Text = "Close Selected";
      BtnCloseTool.UseVisualStyleBackColor = true;
      BtnCloseTool.Click += BtnCloseTool_Click;
      // 
      // listViewProcesses
      // 
      listViewProcesses.Columns.AddRange( new ColumnHeader[] { colTool, colPid, colStarted, colStatus } );
      listViewProcesses.FullRowSelect = true;
      listViewProcesses.GridLines = true;
      listViewProcesses.Location = new Point( 14, 26 );
      listViewProcesses.MultiSelect = false;
      listViewProcesses.Name = "listViewProcesses";
      listViewProcesses.Size = new Size( 584, 110 );
      listViewProcesses.TabIndex = 0;
      listViewProcesses.UseCompatibleStateImageBehavior = false;
      listViewProcesses.View = View.Details;
      // 
      // colTool
      // 
      colTool.Text = "Tool";
      colTool.Width = 200;
      // 
      // colPid
      // 
      colPid.Text = "PID";
      colPid.Width = 80;
      // 
      // colStarted
      // 
      colStarted.Text = "Started";
      colStarted.Width = 100;
      // 
      // colStatus
      // 
      colStatus.Text = "Status";
      colStatus.Width = 557;
      // 
      // Email_Settings_Button
      // 
      Email_Settings_Button.Location = new Point( 642, 117 );
      Email_Settings_Button.Name = "Email_Settings_Button";
      Email_Settings_Button.Size = new Size( 77, 59 );
      Email_Settings_Button.TabIndex = 7;
      Email_Settings_Button.Text = "Email Settings";
      Email_Settings_Button.UseVisualStyleBackColor = true;
      Email_Settings_Button.Click += Email_Settings_Button_Click;
      // 
      // Email_Log_Button
      // 
      Email_Log_Button.Location = new Point( 642, 182 );
      Email_Log_Button.Name = "Email_Log_Button";
      Email_Log_Button.Size = new Size( 77, 55 );
      Email_Log_Button.TabIndex = 6;
      Email_Log_Button.Text = "Email Log";
      Email_Log_Button.UseVisualStyleBackColor = true;
      Email_Log_Button.Click += Email_Log_Button_Click;
      // 
      // Enable_Registry_Backup_Button
      // 
      Enable_Registry_Backup_Button.Location = new Point( 446, 23 );
      Enable_Registry_Backup_Button.Name = "Enable_Registry_Backup_Button";
      Enable_Registry_Backup_Button.Size = new Size( 76, 59 );
      Enable_Registry_Backup_Button.TabIndex = 12;
      Enable_Registry_Backup_Button.Text = "Registery Operations";
      Enable_Registry_Backup_Button.UseVisualStyleBackColor = true;
      Enable_Registry_Backup_Button.Click += Enable_Registry_Backup_Button_Click;
      // 
      // Restore_Points_Button
      // 
      Restore_Points_Button.Location = new Point( 363, 23 );
      Restore_Points_Button.Name = "Restore_Points_Button";
      Restore_Points_Button.Size = new Size( 77, 59 );
      Restore_Points_Button.TabIndex = 11;
      Restore_Points_Button.Text = "Restore Point Operations";
      Restore_Points_Button.UseVisualStyleBackColor = true;
      Restore_Points_Button.Click += Restore_Points_Button_Click;
      // 
      // statusStrip
      // 
      statusStrip.Items.AddRange( new ToolStripItem[] { statusLabel } );
      statusStrip.Location = new Point( 0, 507 );
      statusStrip.Name = "statusStrip";
      statusStrip.Size = new Size( 732, 22 );
      statusStrip.TabIndex = 4;
      // 
      // statusLabel
      // 
      statusLabel.Name = "statusLabel";
      statusLabel.Size = new Size( 39, 17 );
      statusLabel.Text = "Ready";
      // 
      // timerStatus
      // 
      timerStatus.Enabled = true;
      timerStatus.Interval = 3000;
      timerStatus.Tick += TimerStatus_Tick;
      // 
      // Snapshot_Operations_Button
      // 
      Snapshot_Operations_Button.Location = new Point( 532, 23 );
      Snapshot_Operations_Button.Name = "Snapshot_Operations_Button";
      Snapshot_Operations_Button.Size = new Size( 76, 59 );
      Snapshot_Operations_Button.TabIndex = 13;
      Snapshot_Operations_Button.Text = "Snapshot Operations";
      Snapshot_Operations_Button.UseVisualStyleBackColor = true;
      Snapshot_Operations_Button.Click += Snapshot_Operations_Button_Click;
      // 
      // Help_Button
      // 
      Help_Button.Location = new Point( 642, 423 );
      Help_Button.Name = "Help_Button";
      Help_Button.Size = new Size( 77, 30 );
      Help_Button.TabIndex = 16;
      Help_Button.Text = "Help";
      Help_Button.UseVisualStyleBackColor = true;
      Help_Button.Click += Help_Button_Click;
      // 
      // View_Log_Button
      // 
      View_Log_Button.Location = new Point( 642, 387 );
      View_Log_Button.Name = "View_Log_Button";
      View_Log_Button.Size = new Size( 77, 30 );
      View_Log_Button.TabIndex = 15;
      View_Log_Button.Text = "View Log";
      View_Log_Button.UseVisualStyleBackColor = true;
      View_Log_Button.Click += View_Log_Button_Click;
      // 
      // Quit_Button
      // 
      Quit_Button.Location = new Point( 642, 459 );
      Quit_Button.Name = "Quit_Button";
      Quit_Button.Size = new Size( 77, 30 );
      Quit_Button.TabIndex = 14;
      Quit_Button.Text = "Quit";
      Quit_Button.UseVisualStyleBackColor = true;
      Quit_Button.Click += Quit_Button_Click;
      // 
      // Admin_Commands_Button
      // 
      Admin_Commands_Button.Location = new Point( 363, 117 );
      Admin_Commands_Button.Name = "Admin_Commands_Button";
      Admin_Commands_Button.Size = new Size( 77, 59 );
      Admin_Commands_Button.TabIndex = 17;
      Admin_Commands_Button.Text = "Admin Commands";
      Admin_Commands_Button.UseVisualStyleBackColor = true;
      Admin_Commands_Button.Click += Admin_Commands_Button_Click;
      // 
      // Remote_Tools_Button
      // 
      Remote_Tools_Button.Location = new Point( 532, 243 );
      Remote_Tools_Button.Name = "Remote_Tools_Button";
      Remote_Tools_Button.Size = new Size( 78, 59 );
      Remote_Tools_Button.TabIndex = 18;
      Remote_Tools_Button.Text = "Remote Tools";
      Remote_Tools_Button.UseVisualStyleBackColor = true;
      Remote_Tools_Button.Click += Remote_Tools_Button_Click;
      // 
      // Powershell_Button
      // 
      Powershell_Button.Location = new Point( 446, 117 );
      Powershell_Button.Name = "Powershell_Button";
      Powershell_Button.Size = new Size( 77, 59 );
      Powershell_Button.TabIndex = 19;
      Powershell_Button.Text = "Powershell";
      Powershell_Button.UseVisualStyleBackColor = true;
      Powershell_Button.Click += Powershell_Button_Click;
      // 
      // CMD_Button
      // 
      CMD_Button.Location = new Point( 532, 117 );
      CMD_Button.Name = "CMD_Button";
      CMD_Button.Size = new Size( 73, 59 );
      CMD_Button.TabIndex = 20;
      CMD_Button.Text = "Cmd";
      CMD_Button.UseVisualStyleBackColor = true;
      CMD_Button.Click += CMD_Button_Click;
      // 
      // Printer_Button
      // 
      Printer_Button.Location = new Point( 642, 23 );
      Printer_Button.Name = "Printer_Button";
      Printer_Button.Size = new Size( 78, 59 );
      Printer_Button.TabIndex = 21;
      Printer_Button.Text = "Printers";
      Printer_Button.UseVisualStyleBackColor = true;
      Printer_Button.Click += Printer_Button_Click;
      // 
      // Trace_Button
      // 
      Trace_Button.Location = new Point( 643, 351 );
      Trace_Button.Name = "Trace_Button";
      Trace_Button.Size = new Size( 77, 30 );
      Trace_Button.TabIndex = 22;
      Trace_Button.Text = "Trace";
      Trace_Button.UseVisualStyleBackColor = true;
      Trace_Button.Click += Trace_Button_Click;
      // 
      // MainForm
      // 
      AutoScaleDimensions = new SizeF( 7F, 15F );
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size( 732, 529 );
      Controls.Add( Trace_Button );
      Controls.Add( Printer_Button );
      Controls.Add( CMD_Button );
      Controls.Add( Powershell_Button );
      Controls.Add( Remote_Tools_Button );
      Controls.Add( Admin_Commands_Button );
      Controls.Add( Help_Button );
      Controls.Add( View_Log_Button );
      Controls.Add( Quit_Button );
      Controls.Add( Snapshot_Operations_Button );
      Controls.Add( Email_Log_Button );
      Controls.Add( Email_Settings_Button );
      Controls.Add( Enable_Registry_Backup_Button );
      Controls.Add( Restore_Points_Button );
      Controls.Add( statusStrip );
      Controls.Add( grpProcesses );
      Controls.Add( grpTools );
      Font = new Font( "Segoe UI", 9F );
      Name = "MainForm";
      StartPosition = FormStartPosition.CenterScreen;
      Text = "Admin Toolkit";
      Load += MainForm_Load;
      grpTools.ResumeLayout( false );
      grpProcesses.ResumeLayout( false );
      statusStrip.ResumeLayout( false );
      statusStrip.PerformLayout();
      ResumeLayout( false );
      PerformLayout();

    }

    #endregion
    private System.Windows.Forms.GroupBox grpTools;
        private System.Windows.Forms.Button BtnViewLog;
        private System.Windows.Forms.Button BtnTaskScheduler;
        private System.Windows.Forms.Button BtnSystemProtection;
        private System.Windows.Forms.Button BtnRestoreWizard;
        private System.Windows.Forms.Button BtnRegistryEditor;
        private System.Windows.Forms.Button BtnEventViewer;
        private System.Windows.Forms.Button BtnServices;
        private System.Windows.Forms.Button BtnDiskManagement;
        private System.Windows.Forms.Button BtnComputerMgmt;
        private System.Windows.Forms.Button BtnSystemInfo;
        private System.Windows.Forms.Button BtnPerfMonitor;
        private System.Windows.Forms.Button BtnResourceMonitor;
        private System.Windows.Forms.Button BtnDeviceManager;
        private System.Windows.Forms.Button BtnLocalUsers;
        private System.Windows.Forms.Button BtnFirewall;
        private System.Windows.Forms.GroupBox grpProcesses;
        private System.Windows.Forms.ListView listViewProcesses;
        private System.Windows.Forms.ColumnHeader colTool;
        private System.Windows.Forms.ColumnHeader colPid;
        private System.Windows.Forms.ColumnHeader colStarted;
        private System.Windows.Forms.ColumnHeader colStatus;
        private System.Windows.Forms.Button BtnCloseTool;
        private System.Windows.Forms.Button BtnKillTool;
        private System.Windows.Forms.Button BtnRemoveClosed;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.Timer timerStatus;
        private System.Windows.Forms.Button Email_Log_Button;
        private System.Windows.Forms.Button Email_Settings_Button;
        private System.Windows.Forms.Button Restore_Points_Button;
        private System.Windows.Forms.Button Enable_Registry_Backup_Button;
        private System.Windows.Forms.Button Snapshot_Operations_Button;
        private System.Windows.Forms.Button Help_Button;
        private System.Windows.Forms.Button View_Log_Button;
        private System.Windows.Forms.Button Quit_Button;
        private Button Admin_Commands_Button;
        private Button Remote_Tools_Button;
    private Button Powershell_Button;
    private Button CMD_Button;
    private Button Printer_Button;
    private Button Trace_Button;
  }
}
