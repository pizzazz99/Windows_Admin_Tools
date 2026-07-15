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
            btnViewLog = new Button();
            grpTools = new GroupBox();
            btnFirewall = new Button();
            btnLocalUsers = new Button();
            btnDeviceManager = new Button();
            btnResourceMonitor = new Button();
            btnPerfMonitor = new Button();
            btnSystemInfo = new Button();
            btnComputerMgmt = new Button();
            btnDiskManagement = new Button();
            btnServices = new Button();
            btnEventViewer = new Button();
            btnRegistryEditor = new Button();
            btnRestoreWizard = new Button();
            btnSystemProtection = new Button();
            btnTaskScheduler = new Button();
            grpProcesses = new GroupBox();
            btnRemoveClosed = new Button();
            btnKillTool = new Button();
            btnCloseTool = new Button();
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
            timerStatus = new System.Windows.Forms.Timer(components);
            Snapshot_Operations_Button = new Button();
            Help_Button = new Button();
            View_Log_Button = new Button();
            Quit_Button = new Button();
            grpTools.SuspendLayout();
            grpProcesses.SuspendLayout();
            statusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // btnViewLog
            // 
            btnViewLog.Location = new Point(0, 0);
            btnViewLog.Name = "btnViewLog";
            btnViewLog.Size = new Size(75, 23);
            btnViewLog.TabIndex = 0;
            // 
            // grpTools
            // 
            grpTools.Controls.Add(btnFirewall);
            grpTools.Controls.Add(btnLocalUsers);
            grpTools.Controls.Add(btnDeviceManager);
            grpTools.Controls.Add(btnResourceMonitor);
            grpTools.Controls.Add(btnPerfMonitor);
            grpTools.Controls.Add(btnSystemInfo);
            grpTools.Controls.Add(btnComputerMgmt);
            grpTools.Controls.Add(btnDiskManagement);
            grpTools.Controls.Add(btnServices);
            grpTools.Controls.Add(btnEventViewer);
            grpTools.Controls.Add(btnRegistryEditor);
            grpTools.Controls.Add(btnRestoreWizard);
            grpTools.Controls.Add(btnSystemProtection);
            grpTools.Controls.Add(btnTaskScheduler);
            grpTools.Location = new Point(12, 12);
            grpTools.Name = "grpTools";
            grpTools.Size = new Size(325, 290);
            grpTools.TabIndex = 1;
            grpTools.TabStop = false;
            grpTools.Text = "Admin Tools";
            // 
            // btnFirewall
            // 
            btnFirewall.Location = new Point(166, 242);
            btnFirewall.Name = "btnFirewall";
            btnFirewall.Size = new Size(140, 30);
            btnFirewall.TabIndex = 13;
            btnFirewall.Text = "Windows Firewall";
            btnFirewall.UseVisualStyleBackColor = true;
            btnFirewall.Click += btnFirewall_Click;
            // 
            // btnLocalUsers
            // 
            btnLocalUsers.Location = new Point(14, 242);
            btnLocalUsers.Name = "btnLocalUsers";
            btnLocalUsers.Size = new Size(140, 30);
            btnLocalUsers.TabIndex = 12;
            btnLocalUsers.Text = "Local Users && Groups";
            btnLocalUsers.UseVisualStyleBackColor = true;
            btnLocalUsers.Click += btnLocalUsers_Click;
            // 
            // btnDeviceManager
            // 
            btnDeviceManager.Location = new Point(166, 206);
            btnDeviceManager.Name = "btnDeviceManager";
            btnDeviceManager.Size = new Size(140, 30);
            btnDeviceManager.TabIndex = 11;
            btnDeviceManager.Text = "Device Manager";
            btnDeviceManager.UseVisualStyleBackColor = true;
            btnDeviceManager.Click += btnDeviceManager_Click;
            // 
            // btnResourceMonitor
            // 
            btnResourceMonitor.Location = new Point(14, 206);
            btnResourceMonitor.Name = "btnResourceMonitor";
            btnResourceMonitor.Size = new Size(140, 30);
            btnResourceMonitor.TabIndex = 10;
            btnResourceMonitor.Text = "Resource Monitor";
            btnResourceMonitor.UseVisualStyleBackColor = true;
            btnResourceMonitor.Click += btnResourceMonitor_Click;
            // 
            // btnPerfMonitor
            // 
            btnPerfMonitor.Location = new Point(166, 170);
            btnPerfMonitor.Name = "btnPerfMonitor";
            btnPerfMonitor.Size = new Size(140, 30);
            btnPerfMonitor.TabIndex = 9;
            btnPerfMonitor.Text = "Performance Monitor";
            btnPerfMonitor.UseVisualStyleBackColor = true;
            btnPerfMonitor.Click += btnPerfMonitor_Click;
            // 
            // btnSystemInfo
            // 
            btnSystemInfo.Location = new Point(14, 170);
            btnSystemInfo.Name = "btnSystemInfo";
            btnSystemInfo.Size = new Size(140, 30);
            btnSystemInfo.TabIndex = 8;
            btnSystemInfo.Text = "System Info";
            btnSystemInfo.UseVisualStyleBackColor = true;
            btnSystemInfo.Click += btnSystemInfo_Click;
            // 
            // btnComputerMgmt
            // 
            btnComputerMgmt.Location = new Point(166, 134);
            btnComputerMgmt.Name = "btnComputerMgmt";
            btnComputerMgmt.Size = new Size(140, 30);
            btnComputerMgmt.TabIndex = 7;
            btnComputerMgmt.Text = "Computer Mgmt";
            btnComputerMgmt.UseVisualStyleBackColor = true;
            btnComputerMgmt.Click += btnComputerMgmt_Click;
            // 
            // btnDiskManagement
            // 
            btnDiskManagement.Location = new Point(14, 134);
            btnDiskManagement.Name = "btnDiskManagement";
            btnDiskManagement.Size = new Size(140, 30);
            btnDiskManagement.TabIndex = 6;
            btnDiskManagement.Text = "Disk Management";
            btnDiskManagement.UseVisualStyleBackColor = true;
            btnDiskManagement.Click += btnDiskManagement_Click;
            // 
            // btnServices
            // 
            btnServices.Location = new Point(166, 98);
            btnServices.Name = "btnServices";
            btnServices.Size = new Size(140, 30);
            btnServices.TabIndex = 5;
            btnServices.Text = "Services";
            btnServices.UseVisualStyleBackColor = true;
            btnServices.Click += btnServices_Click;
            // 
            // btnEventViewer
            // 
            btnEventViewer.Location = new Point(14, 98);
            btnEventViewer.Name = "btnEventViewer";
            btnEventViewer.Size = new Size(140, 30);
            btnEventViewer.TabIndex = 4;
            btnEventViewer.Text = "Event Viewer";
            btnEventViewer.UseVisualStyleBackColor = true;
            btnEventViewer.Click += btnEventViewer_Click;
            // 
            // btnRegistryEditor
            // 
            btnRegistryEditor.Location = new Point(168, 62);
            btnRegistryEditor.Name = "btnRegistryEditor";
            btnRegistryEditor.Size = new Size(140, 30);
            btnRegistryEditor.TabIndex = 3;
            btnRegistryEditor.Text = "Registry Editor";
            btnRegistryEditor.UseVisualStyleBackColor = true;
            btnRegistryEditor.Click += btnRegistryEditor_Click;
            // 
            // btnRestoreWizard
            // 
            btnRestoreWizard.Location = new Point(16, 62);
            btnRestoreWizard.Name = "btnRestoreWizard";
            btnRestoreWizard.Size = new Size(140, 30);
            btnRestoreWizard.TabIndex = 2;
            btnRestoreWizard.Text = "System Restore";
            btnRestoreWizard.UseVisualStyleBackColor = true;
            btnRestoreWizard.Click += btnRestoreWizard_Click;
            // 
            // btnSystemProtection
            // 
            btnSystemProtection.Location = new Point(166, 26);
            btnSystemProtection.Name = "btnSystemProtection";
            btnSystemProtection.Size = new Size(140, 30);
            btnSystemProtection.TabIndex = 1;
            btnSystemProtection.Text = "System Protection";
            btnSystemProtection.UseVisualStyleBackColor = true;
            btnSystemProtection.Click += btnSystemProtection_Click;
            // 
            // btnTaskScheduler
            // 
            btnTaskScheduler.Location = new Point(14, 26);
            btnTaskScheduler.Name = "btnTaskScheduler";
            btnTaskScheduler.Size = new Size(140, 30);
            btnTaskScheduler.TabIndex = 0;
            btnTaskScheduler.Text = "Task Scheduler";
            btnTaskScheduler.UseVisualStyleBackColor = true;
            btnTaskScheduler.Click += btnTaskScheduler_Click;
            // 
            // grpProcesses
            // 
            grpProcesses.Controls.Add(btnRemoveClosed);
            grpProcesses.Controls.Add(btnKillTool);
            grpProcesses.Controls.Add(btnCloseTool);
            grpProcesses.Controls.Add(listViewProcesses);
            grpProcesses.Location = new Point(12, 308);
            grpProcesses.Name = "grpProcesses";
            grpProcesses.Size = new Size(615, 179);
            grpProcesses.TabIndex = 2;
            grpProcesses.TabStop = false;
            grpProcesses.Text = "Launched Tools";
            // 
            // btnRemoveClosed
            // 
            btnRemoveClosed.Location = new Point(241, 142);
            btnRemoveClosed.Name = "btnRemoveClosed";
            btnRemoveClosed.Size = new Size(84, 30);
            btnRemoveClosed.TabIndex = 3;
            btnRemoveClosed.Text = "Clear Closed";
            btnRemoveClosed.UseVisualStyleBackColor = true;
            btnRemoveClosed.Click += btnRemoveClosed_Click;
            // 
            // btnKillTool
            // 
            btnKillTool.Location = new Point(130, 142);
            btnKillTool.Name = "btnKillTool";
            btnKillTool.Size = new Size(105, 30);
            btnKillTool.TabIndex = 2;
            btnKillTool.Text = "End Task (force)";
            btnKillTool.UseVisualStyleBackColor = true;
            btnKillTool.Click += btnKillTool_Click;
            // 
            // btnCloseTool
            // 
            btnCloseTool.Location = new Point(14, 142);
            btnCloseTool.Name = "btnCloseTool";
            btnCloseTool.Size = new Size(110, 30);
            btnCloseTool.TabIndex = 1;
            btnCloseTool.Text = "Close Selected";
            btnCloseTool.UseVisualStyleBackColor = true;
            btnCloseTool.Click += btnCloseTool_Click;
            // 
            // listViewProcesses
            // 
            listViewProcesses.Columns.AddRange(new ColumnHeader[] { colTool, colPid, colStarted, colStatus });
            listViewProcesses.FullRowSelect = true;
            listViewProcesses.GridLines = true;
            listViewProcesses.Location = new Point(14, 26);
            listViewProcesses.MultiSelect = false;
            listViewProcesses.Name = "listViewProcesses";
            listViewProcesses.Size = new Size(584, 110);
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
            Email_Settings_Button.Location = new Point(642, 93);
            Email_Settings_Button.Name = "Email_Settings_Button";
            Email_Settings_Button.Size = new Size(77, 47);
            Email_Settings_Button.TabIndex = 7;
            Email_Settings_Button.Text = "Email Settings";
            Email_Settings_Button.UseVisualStyleBackColor = true;
            Email_Settings_Button.Click += Email_Settings_Button_Click;
            // 
            // Email_Log_Button
            // 
            Email_Log_Button.Location = new Point(642, 146);
            Email_Log_Button.Name = "Email_Log_Button";
            Email_Log_Button.Size = new Size(77, 49);
            Email_Log_Button.TabIndex = 6;
            Email_Log_Button.Text = "Email Log";
            Email_Log_Button.UseVisualStyleBackColor = true;
            Email_Log_Button.Click += Email_Log_Button_Click;
            // 
            // Enable_Registry_Backup_Button
            // 
            Enable_Registry_Backup_Button.Location = new Point(446, 23);
            Enable_Registry_Backup_Button.Name = "Enable_Registry_Backup_Button";
            Enable_Registry_Backup_Button.Size = new Size(76, 59);
            Enable_Registry_Backup_Button.TabIndex = 12;
            Enable_Registry_Backup_Button.Text = "Registery Operations";
            Enable_Registry_Backup_Button.UseVisualStyleBackColor = true;
            Enable_Registry_Backup_Button.Click += Enable_Registry_Backup_Button_Click;
            // 
            // Restore_Points_Button
            // 
            Restore_Points_Button.Location = new Point(363, 23);
            Restore_Points_Button.Name = "Restore_Points_Button";
            Restore_Points_Button.Size = new Size(77, 59);
            Restore_Points_Button.TabIndex = 11;
            Restore_Points_Button.Text = "Restore Point Operations";
            Restore_Points_Button.UseVisualStyleBackColor = true;
            Restore_Points_Button.Click += Restore_Points_Button_Click;
            // 
            // statusStrip
            // 
            statusStrip.Items.AddRange(new ToolStripItem[] { statusLabel });
            statusStrip.Location = new Point(0, 507);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(732, 22);
            statusStrip.TabIndex = 4;
            // 
            // statusLabel
            // 
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(39, 17);
            statusLabel.Text = "Ready";
            // 
            // timerStatus
            // 
            timerStatus.Enabled = true;
            timerStatus.Interval = 3000;
            timerStatus.Tick += timerStatus_Tick;
            // 
            // Snapshot_Operations_Button
            // 
            Snapshot_Operations_Button.Location = new Point(532, 23);
            Snapshot_Operations_Button.Name = "Snapshot_Operations_Button";
            Snapshot_Operations_Button.Size = new Size(76, 59);
            Snapshot_Operations_Button.TabIndex = 13;
            Snapshot_Operations_Button.Text = "Snapshot Operations";
            Snapshot_Operations_Button.UseVisualStyleBackColor = true;
            Snapshot_Operations_Button.Click += Snapshot_Operations_Button_Click;
            // 
            // Help_Button
            // 
            Help_Button.Location = new Point(642, 423);
            Help_Button.Name = "Help_Button";
            Help_Button.Size = new Size(77, 30);
            Help_Button.TabIndex = 16;
            Help_Button.Text = "Help";
            Help_Button.UseVisualStyleBackColor = true;
            Help_Button.Click += Help_Button_Click;
            // 
            // View_Log_Button
            // 
            View_Log_Button.Location = new Point(642, 387);
            View_Log_Button.Name = "View_Log_Button";
            View_Log_Button.Size = new Size(77, 30);
            View_Log_Button.TabIndex = 15;
            View_Log_Button.Text = "View Log";
            View_Log_Button.UseVisualStyleBackColor = true;
            View_Log_Button.Click += View_Log_Button_Click;
            // 
            // Quit_Button
            // 
            Quit_Button.Location = new Point(642, 459);
            Quit_Button.Name = "Quit_Button";
            Quit_Button.Size = new Size(77, 30);
            Quit_Button.TabIndex = 14;
            Quit_Button.Text = "Quit";
            Quit_Button.UseVisualStyleBackColor = true;
            Quit_Button.Click += Quit_Button_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(732, 529);
            Controls.Add(Help_Button);
            Controls.Add(View_Log_Button);
            Controls.Add(Quit_Button);
            Controls.Add(Snapshot_Operations_Button);
            Controls.Add(Email_Log_Button);
            Controls.Add(Email_Settings_Button);
            Controls.Add(Enable_Registry_Backup_Button);
            Controls.Add(Restore_Points_Button);
            Controls.Add(statusStrip);
            Controls.Add(grpProcesses);
            Controls.Add(grpTools);
            Font = new Font("Segoe UI", 9F);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Admin Toolkit";
            Load += MainForm_Load;
            grpTools.ResumeLayout(false);
            grpProcesses.ResumeLayout(false);
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

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
