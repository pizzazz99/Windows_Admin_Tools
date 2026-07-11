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
            this.components = new System.ComponentModel.Container();
            this.btnViewLog = new System.Windows.Forms.Button();
            this.grpTools = new System.Windows.Forms.GroupBox();
            this.btnFirewall = new System.Windows.Forms.Button();
            this.btnLocalUsers = new System.Windows.Forms.Button();
            this.btnDeviceManager = new System.Windows.Forms.Button();
            this.btnResourceMonitor = new System.Windows.Forms.Button();
            this.btnPerfMonitor = new System.Windows.Forms.Button();
            this.btnSystemInfo = new System.Windows.Forms.Button();
            this.btnComputerMgmt = new System.Windows.Forms.Button();
            this.btnDiskManagement = new System.Windows.Forms.Button();
            this.btnServices = new System.Windows.Forms.Button();
            this.btnEventViewer = new System.Windows.Forms.Button();
            this.btnRegistryEditor = new System.Windows.Forms.Button();
            this.btnRestoreWizard = new System.Windows.Forms.Button();
            this.btnSystemProtection = new System.Windows.Forms.Button();
            this.btnTaskScheduler = new System.Windows.Forms.Button();
            this.grpProcesses = new System.Windows.Forms.GroupBox();
            this.btnRemoveClosed = new System.Windows.Forms.Button();
            this.btnKillTool = new System.Windows.Forms.Button();
            this.btnCloseTool = new System.Windows.Forms.Button();
            this.listViewProcesses = new System.Windows.Forms.ListView();
            this.colTool = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colPid = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colStarted = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Email_Settings_Button = new System.Windows.Forms.Button();
            this.Email_Log_Button = new System.Windows.Forms.Button();
            this.Enable_Registry_Backup_Button = new System.Windows.Forms.Button();
            this.Restore_Points_Button = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.timerStatus = new System.Windows.Forms.Timer(this.components);
            this.Snapshot_Operations_Button = new System.Windows.Forms.Button();
            this.Help_Button = new System.Windows.Forms.Button();
            this.View_Log_Button = new System.Windows.Forms.Button();
            this.Quit_Button = new System.Windows.Forms.Button();
            this.grpTools.SuspendLayout();
            this.grpProcesses.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnViewLog
            // 
            this.btnViewLog.Location = new System.Drawing.Point(0, 0);
            this.btnViewLog.Name = "btnViewLog";
            this.btnViewLog.Size = new System.Drawing.Size(75, 23);
            this.btnViewLog.TabIndex = 0;
            // 
            // grpTools
            // 
            this.grpTools.Controls.Add(this.btnFirewall);
            this.grpTools.Controls.Add(this.btnLocalUsers);
            this.grpTools.Controls.Add(this.btnDeviceManager);
            this.grpTools.Controls.Add(this.btnResourceMonitor);
            this.grpTools.Controls.Add(this.btnPerfMonitor);
            this.grpTools.Controls.Add(this.btnSystemInfo);
            this.grpTools.Controls.Add(this.btnComputerMgmt);
            this.grpTools.Controls.Add(this.btnDiskManagement);
            this.grpTools.Controls.Add(this.btnServices);
            this.grpTools.Controls.Add(this.btnEventViewer);
            this.grpTools.Controls.Add(this.btnRegistryEditor);
            this.grpTools.Controls.Add(this.btnRestoreWizard);
            this.grpTools.Controls.Add(this.btnSystemProtection);
            this.grpTools.Controls.Add(this.btnTaskScheduler);
            this.grpTools.Location = new System.Drawing.Point(12, 12);
            this.grpTools.Name = "grpTools";
            this.grpTools.Size = new System.Drawing.Size(325, 290);
            this.grpTools.TabIndex = 1;
            this.grpTools.TabStop = false;
            this.grpTools.Text = "Admin Tools";
            // 
            // btnFirewall
            // 
            this.btnFirewall.Location = new System.Drawing.Point(166, 242);
            this.btnFirewall.Name = "btnFirewall";
            this.btnFirewall.Size = new System.Drawing.Size(140, 30);
            this.btnFirewall.TabIndex = 13;
            this.btnFirewall.Text = "Windows Firewall";
            this.btnFirewall.UseVisualStyleBackColor = true;
            this.btnFirewall.Click += new System.EventHandler(this.btnFirewall_Click);
            // 
            // btnLocalUsers
            // 
            this.btnLocalUsers.Location = new System.Drawing.Point(14, 242);
            this.btnLocalUsers.Name = "btnLocalUsers";
            this.btnLocalUsers.Size = new System.Drawing.Size(140, 30);
            this.btnLocalUsers.TabIndex = 12;
            this.btnLocalUsers.Text = "Local Users && Groups";
            this.btnLocalUsers.UseVisualStyleBackColor = true;
            this.btnLocalUsers.Click += new System.EventHandler(this.btnLocalUsers_Click);
            // 
            // btnDeviceManager
            // 
            this.btnDeviceManager.Location = new System.Drawing.Point(166, 206);
            this.btnDeviceManager.Name = "btnDeviceManager";
            this.btnDeviceManager.Size = new System.Drawing.Size(140, 30);
            this.btnDeviceManager.TabIndex = 11;
            this.btnDeviceManager.Text = "Device Manager";
            this.btnDeviceManager.UseVisualStyleBackColor = true;
            this.btnDeviceManager.Click += new System.EventHandler(this.btnDeviceManager_Click);
            // 
            // btnResourceMonitor
            // 
            this.btnResourceMonitor.Location = new System.Drawing.Point(14, 206);
            this.btnResourceMonitor.Name = "btnResourceMonitor";
            this.btnResourceMonitor.Size = new System.Drawing.Size(140, 30);
            this.btnResourceMonitor.TabIndex = 10;
            this.btnResourceMonitor.Text = "Resource Monitor";
            this.btnResourceMonitor.UseVisualStyleBackColor = true;
            this.btnResourceMonitor.Click += new System.EventHandler(this.btnResourceMonitor_Click);
            // 
            // btnPerfMonitor
            // 
            this.btnPerfMonitor.Location = new System.Drawing.Point(166, 170);
            this.btnPerfMonitor.Name = "btnPerfMonitor";
            this.btnPerfMonitor.Size = new System.Drawing.Size(140, 30);
            this.btnPerfMonitor.TabIndex = 9;
            this.btnPerfMonitor.Text = "Performance Monitor";
            this.btnPerfMonitor.UseVisualStyleBackColor = true;
            this.btnPerfMonitor.Click += new System.EventHandler(this.btnPerfMonitor_Click);
            // 
            // btnSystemInfo
            // 
            this.btnSystemInfo.Location = new System.Drawing.Point(14, 170);
            this.btnSystemInfo.Name = "btnSystemInfo";
            this.btnSystemInfo.Size = new System.Drawing.Size(140, 30);
            this.btnSystemInfo.TabIndex = 8;
            this.btnSystemInfo.Text = "System Info";
            this.btnSystemInfo.UseVisualStyleBackColor = true;
            this.btnSystemInfo.Click += new System.EventHandler(this.btnSystemInfo_Click);
            // 
            // btnComputerMgmt
            // 
            this.btnComputerMgmt.Location = new System.Drawing.Point(166, 134);
            this.btnComputerMgmt.Name = "btnComputerMgmt";
            this.btnComputerMgmt.Size = new System.Drawing.Size(140, 30);
            this.btnComputerMgmt.TabIndex = 7;
            this.btnComputerMgmt.Text = "Computer Mgmt";
            this.btnComputerMgmt.UseVisualStyleBackColor = true;
            this.btnComputerMgmt.Click += new System.EventHandler(this.btnComputerMgmt_Click);
            // 
            // btnDiskManagement
            // 
            this.btnDiskManagement.Location = new System.Drawing.Point(14, 134);
            this.btnDiskManagement.Name = "btnDiskManagement";
            this.btnDiskManagement.Size = new System.Drawing.Size(140, 30);
            this.btnDiskManagement.TabIndex = 6;
            this.btnDiskManagement.Text = "Disk Management";
            this.btnDiskManagement.UseVisualStyleBackColor = true;
            this.btnDiskManagement.Click += new System.EventHandler(this.btnDiskManagement_Click);
            // 
            // btnServices
            // 
            this.btnServices.Location = new System.Drawing.Point(166, 98);
            this.btnServices.Name = "btnServices";
            this.btnServices.Size = new System.Drawing.Size(140, 30);
            this.btnServices.TabIndex = 5;
            this.btnServices.Text = "Services";
            this.btnServices.UseVisualStyleBackColor = true;
            this.btnServices.Click += new System.EventHandler(this.btnServices_Click);
            // 
            // btnEventViewer
            // 
            this.btnEventViewer.Location = new System.Drawing.Point(14, 98);
            this.btnEventViewer.Name = "btnEventViewer";
            this.btnEventViewer.Size = new System.Drawing.Size(140, 30);
            this.btnEventViewer.TabIndex = 4;
            this.btnEventViewer.Text = "Event Viewer";
            this.btnEventViewer.UseVisualStyleBackColor = true;
            this.btnEventViewer.Click += new System.EventHandler(this.btnEventViewer_Click);
            // 
            // btnRegistryEditor
            // 
            this.btnRegistryEditor.Location = new System.Drawing.Point(168, 62);
            this.btnRegistryEditor.Name = "btnRegistryEditor";
            this.btnRegistryEditor.Size = new System.Drawing.Size(140, 30);
            this.btnRegistryEditor.TabIndex = 3;
            this.btnRegistryEditor.Text = "Registry Editor";
            this.btnRegistryEditor.UseVisualStyleBackColor = true;
            this.btnRegistryEditor.Click += new System.EventHandler(this.btnRegistryEditor_Click);
            // 
            // btnRestoreWizard
            // 
            this.btnRestoreWizard.Location = new System.Drawing.Point(16, 62);
            this.btnRestoreWizard.Name = "btnRestoreWizard";
            this.btnRestoreWizard.Size = new System.Drawing.Size(140, 30);
            this.btnRestoreWizard.TabIndex = 2;
            this.btnRestoreWizard.Text = "System Restore";
            this.btnRestoreWizard.UseVisualStyleBackColor = true;
            this.btnRestoreWizard.Click += new System.EventHandler(this.btnRestoreWizard_Click);
            // 
            // btnSystemProtection
            // 
            this.btnSystemProtection.Location = new System.Drawing.Point(166, 26);
            this.btnSystemProtection.Name = "btnSystemProtection";
            this.btnSystemProtection.Size = new System.Drawing.Size(140, 30);
            this.btnSystemProtection.TabIndex = 1;
            this.btnSystemProtection.Text = "System Protection";
            this.btnSystemProtection.UseVisualStyleBackColor = true;
            this.btnSystemProtection.Click += new System.EventHandler(this.btnSystemProtection_Click);
            // 
            // btnTaskScheduler
            // 
            this.btnTaskScheduler.Location = new System.Drawing.Point(14, 26);
            this.btnTaskScheduler.Name = "btnTaskScheduler";
            this.btnTaskScheduler.Size = new System.Drawing.Size(140, 30);
            this.btnTaskScheduler.TabIndex = 0;
            this.btnTaskScheduler.Text = "Task Scheduler";
            this.btnTaskScheduler.UseVisualStyleBackColor = true;
            this.btnTaskScheduler.Click += new System.EventHandler(this.btnTaskScheduler_Click);
            // 
            // grpProcesses
            // 
            this.grpProcesses.Controls.Add(this.btnRemoveClosed);
            this.grpProcesses.Controls.Add(this.btnKillTool);
            this.grpProcesses.Controls.Add(this.btnCloseTool);
            this.grpProcesses.Controls.Add(this.listViewProcesses);
            this.grpProcesses.Location = new System.Drawing.Point(12, 308);
            this.grpProcesses.Name = "grpProcesses";
            this.grpProcesses.Size = new System.Drawing.Size(615, 179);
            this.grpProcesses.TabIndex = 2;
            this.grpProcesses.TabStop = false;
            this.grpProcesses.Text = "Launched Tools";
            // 
            // btnRemoveClosed
            // 
            this.btnRemoveClosed.Location = new System.Drawing.Point(241, 142);
            this.btnRemoveClosed.Name = "btnRemoveClosed";
            this.btnRemoveClosed.Size = new System.Drawing.Size(84, 30);
            this.btnRemoveClosed.TabIndex = 3;
            this.btnRemoveClosed.Text = "Clear Closed";
            this.btnRemoveClosed.UseVisualStyleBackColor = true;
            this.btnRemoveClosed.Click += new System.EventHandler(this.btnRemoveClosed_Click);
            // 
            // btnKillTool
            // 
            this.btnKillTool.Location = new System.Drawing.Point(130, 142);
            this.btnKillTool.Name = "btnKillTool";
            this.btnKillTool.Size = new System.Drawing.Size(105, 30);
            this.btnKillTool.TabIndex = 2;
            this.btnKillTool.Text = "End Task (force)";
            this.btnKillTool.UseVisualStyleBackColor = true;
            this.btnKillTool.Click += new System.EventHandler(this.btnKillTool_Click);
            // 
            // btnCloseTool
            // 
            this.btnCloseTool.Location = new System.Drawing.Point(14, 142);
            this.btnCloseTool.Name = "btnCloseTool";
            this.btnCloseTool.Size = new System.Drawing.Size(110, 30);
            this.btnCloseTool.TabIndex = 1;
            this.btnCloseTool.Text = "Close Selected";
            this.btnCloseTool.UseVisualStyleBackColor = true;
            this.btnCloseTool.Click += new System.EventHandler(this.btnCloseTool_Click);
            // 
            // listViewProcesses
            // 
            this.listViewProcesses.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colTool,
            this.colPid,
            this.colStarted,
            this.colStatus});
            this.listViewProcesses.FullRowSelect = true;
            this.listViewProcesses.GridLines = true;
            this.listViewProcesses.HideSelection = false;
            this.listViewProcesses.Location = new System.Drawing.Point(14, 26);
            this.listViewProcesses.MultiSelect = false;
            this.listViewProcesses.Name = "listViewProcesses";
            this.listViewProcesses.Size = new System.Drawing.Size(584, 110);
            this.listViewProcesses.TabIndex = 0;
            this.listViewProcesses.UseCompatibleStateImageBehavior = false;
            this.listViewProcesses.View = System.Windows.Forms.View.Details;
            // 
            // colTool
            // 
            this.colTool.Text = "Tool";
            this.colTool.Width = 200;
            // 
            // colPid
            // 
            this.colPid.Text = "PID";
            this.colPid.Width = 80;
            // 
            // colStarted
            // 
            this.colStarted.Text = "Started";
            this.colStarted.Width = 100;
            // 
            // colStatus
            // 
            this.colStatus.Text = "Status";
            this.colStatus.Width = 557;
            // 
            // Email_Settings_Button
            // 
            this.Email_Settings_Button.Location = new System.Drawing.Point(642, 93);
            this.Email_Settings_Button.Name = "Email_Settings_Button";
            this.Email_Settings_Button.Size = new System.Drawing.Size(77, 47);
            this.Email_Settings_Button.TabIndex = 7;
            this.Email_Settings_Button.Text = "Email Settings";
            this.Email_Settings_Button.UseVisualStyleBackColor = true;
            this.Email_Settings_Button.Click += new System.EventHandler(this.Email_Settings_Button_Click);
            // 
            // Email_Log_Button
            // 
            this.Email_Log_Button.Location = new System.Drawing.Point(642, 146);
            this.Email_Log_Button.Name = "Email_Log_Button";
            this.Email_Log_Button.Size = new System.Drawing.Size(77, 49);
            this.Email_Log_Button.TabIndex = 6;
            this.Email_Log_Button.Text = "Email Log";
            this.Email_Log_Button.UseVisualStyleBackColor = true;
            this.Email_Log_Button.Click += new System.EventHandler(this.Email_Log_Button_Click);
            // 
            // Enable_Registry_Backup_Button
            // 
            this.Enable_Registry_Backup_Button.Location = new System.Drawing.Point(446, 23);
            this.Enable_Registry_Backup_Button.Name = "Enable_Registry_Backup_Button";
            this.Enable_Registry_Backup_Button.Size = new System.Drawing.Size(76, 45);
            this.Enable_Registry_Backup_Button.TabIndex = 12;
            this.Enable_Registry_Backup_Button.Text = "Registery Operations";
            this.Enable_Registry_Backup_Button.UseVisualStyleBackColor = true;
            this.Enable_Registry_Backup_Button.Click += new System.EventHandler(this.Enable_Registry_Backup_Button_Click);
            // 
            // Restore_Points_Button
            // 
            this.Restore_Points_Button.Location = new System.Drawing.Point(363, 23);
            this.Restore_Points_Button.Name = "Restore_Points_Button";
            this.Restore_Points_Button.Size = new System.Drawing.Size(77, 45);
            this.Restore_Points_Button.TabIndex = 11;
            this.Restore_Points_Button.Text = "Restore Points";
            this.Restore_Points_Button.UseVisualStyleBackColor = true;
            this.Restore_Points_Button.Click += new System.EventHandler(this.Restore_Points_Button_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 507);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(732, 22);
            this.statusStrip.TabIndex = 4;
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(39, 17);
            this.statusLabel.Text = "Ready";
            // 
            // timerStatus
            // 
            this.timerStatus.Enabled = true;
            this.timerStatus.Interval = 3000;
            this.timerStatus.Tick += new System.EventHandler(this.timerStatus_Tick);
            // 
            // Snapshot_Operations_Button
            // 
            this.Snapshot_Operations_Button.Location = new System.Drawing.Point(532, 23);
            this.Snapshot_Operations_Button.Name = "Snapshot_Operations_Button";
            this.Snapshot_Operations_Button.Size = new System.Drawing.Size(76, 45);
            this.Snapshot_Operations_Button.TabIndex = 13;
            this.Snapshot_Operations_Button.Text = "Snapshot Operations";
            this.Snapshot_Operations_Button.UseVisualStyleBackColor = true;
            this.Snapshot_Operations_Button.Click += new System.EventHandler(this.Snapshot_Operations_Button_Click);
            // 
            // Help_Button
            // 
            this.Help_Button.Location = new System.Drawing.Point(642, 423);
            this.Help_Button.Name = "Help_Button";
            this.Help_Button.Size = new System.Drawing.Size(77, 30);
            this.Help_Button.TabIndex = 16;
            this.Help_Button.Text = "Help";
            this.Help_Button.UseVisualStyleBackColor = true;
            this.Help_Button.Click += new System.EventHandler(this.Help_Button_Click);
            // 
            // View_Log_Button
            // 
            this.View_Log_Button.Location = new System.Drawing.Point(642, 387);
            this.View_Log_Button.Name = "View_Log_Button";
            this.View_Log_Button.Size = new System.Drawing.Size(77, 30);
            this.View_Log_Button.TabIndex = 15;
            this.View_Log_Button.Text = "View Log";
            this.View_Log_Button.UseVisualStyleBackColor = true;
            this.View_Log_Button.Click += new System.EventHandler(this.View_Log_Button_Click);
            // 
            // Quit_Button
            // 
            this.Quit_Button.Location = new System.Drawing.Point(642, 459);
            this.Quit_Button.Name = "Quit_Button";
            this.Quit_Button.Size = new System.Drawing.Size(77, 30);
            this.Quit_Button.TabIndex = 14;
            this.Quit_Button.Text = "Quit";
            this.Quit_Button.UseVisualStyleBackColor = true;
            this.Quit_Button.Click += new System.EventHandler(this.Quit_Button_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(732, 529);
            this.Controls.Add(this.Help_Button);
            this.Controls.Add(this.View_Log_Button);
            this.Controls.Add(this.Quit_Button);
            this.Controls.Add(this.Snapshot_Operations_Button);
            this.Controls.Add(this.Email_Log_Button);
            this.Controls.Add(this.Email_Settings_Button);
            this.Controls.Add(this.Enable_Registry_Backup_Button);
            this.Controls.Add(this.Restore_Points_Button);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.grpProcesses);
            this.Controls.Add(this.grpTools);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Admin Toolkit";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.grpTools.ResumeLayout(false);
            this.grpProcesses.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox grpTools;
        private System.Windows.Forms.Button btnViewLog;
        private System.Windows.Forms.Button btnTaskScheduler;
        private System.Windows.Forms.Button btnSystemProtection;
        private System.Windows.Forms.Button btnRestoreWizard;
        private System.Windows.Forms.Button btnRegistryEditor;
        private System.Windows.Forms.Button btnEventViewer;
        private System.Windows.Forms.Button btnServices;
        private System.Windows.Forms.Button btnDiskManagement;
        private System.Windows.Forms.Button btnComputerMgmt;
        private System.Windows.Forms.Button btnSystemInfo;
        private System.Windows.Forms.Button btnPerfMonitor;
        private System.Windows.Forms.Button btnResourceMonitor;
        private System.Windows.Forms.Button btnDeviceManager;
        private System.Windows.Forms.Button btnLocalUsers;
        private System.Windows.Forms.Button btnFirewall;
        private System.Windows.Forms.GroupBox grpProcesses;
        private System.Windows.Forms.ListView listViewProcesses;
        private System.Windows.Forms.ColumnHeader colTool;
        private System.Windows.Forms.ColumnHeader colPid;
        private System.Windows.Forms.ColumnHeader colStarted;
        private System.Windows.Forms.ColumnHeader colStatus;
        private System.Windows.Forms.Button btnCloseTool;
        private System.Windows.Forms.Button btnKillTool;
        private System.Windows.Forms.Button btnRemoveClosed;
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
    }
}
