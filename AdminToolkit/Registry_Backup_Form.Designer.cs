namespace Admin_Tools
{
    partial class Registry_Backup_Form
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
            this.grpRegBack = new System.Windows.Forms.GroupBox();
            this.lvHives = new System.Windows.Forms.ListView();
            this.colHive = new System.Windows.Forms.ColumnHeader();
            this.colSize = new System.Windows.Forms.ColumnHeader();
            this.colModified = new System.Windows.Forms.ColumnHeader();
            this.lblRegBackStatus = new System.Windows.Forms.Label();
            this.txtRegBackStatus = new System.Windows.Forms.TextBox();
            this.lblPeriodic = new System.Windows.Forms.Label();
            this.txtPeriodic = new System.Windows.Forms.TextBox();
            this.lblRegBackPath = new System.Windows.Forms.Label();
            this.txtRegBackPath = new System.Windows.Forms.TextBox();
            this.grpTask = new System.Windows.Forms.GroupBox();
            this.lblTaskLastRun = new System.Windows.Forms.Label();
            this.txtTaskLastRun = new System.Windows.Forms.TextBox();
            this.lblTaskResult = new System.Windows.Forms.Label();
            this.txtTaskResult = new System.Windows.Forms.TextBox();
            this.lblTaskState = new System.Windows.Forms.Label();
            this.txtTaskState = new System.Windows.Forms.TextBox();
            this.lblTaskNextRun = new System.Windows.Forms.Label();
            this.txtTaskNextRun = new System.Windows.Forms.TextBox();
            this.grpSnapshots = new System.Windows.Forms.GroupBox();
            this.lblRpCount = new System.Windows.Forms.Label();
            this.txtRpCount = new System.Windows.Forms.TextBox();
            this.lblRpNewest = new System.Windows.Forms.Label();
            this.txtRpNewest = new System.Windows.Forms.TextBox();
            this.lblScCount = new System.Windows.Forms.Label();
            this.txtScCount = new System.Windows.Forms.TextBox();
            this.lblScNewest = new System.Windows.Forms.Label();
            this.txtScNewest = new System.Windows.Forms.TextBox();
            this.grpVerdict = new System.Windows.Forms.GroupBox();
            this.txtVerdict = new System.Windows.Forms.TextBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnEnableRegBack = new System.Windows.Forms.Button();
            this.btnBackupNow = new System.Windows.Forms.Button();
            this.btnOpenFolder = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.grpRegBack.SuspendLayout();
            this.grpTask.SuspendLayout();
            this.grpSnapshots.SuspendLayout();
            this.grpVerdict.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpRegBack
            // 
            this.grpRegBack.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpRegBack.Controls.Add(this.lvHives);
            this.grpRegBack.Controls.Add(this.lblRegBackStatus);
            this.grpRegBack.Controls.Add(this.txtRegBackStatus);
            this.grpRegBack.Controls.Add(this.lblPeriodic);
            this.grpRegBack.Controls.Add(this.txtPeriodic);
            this.grpRegBack.Controls.Add(this.lblRegBackPath);
            this.grpRegBack.Controls.Add(this.txtRegBackPath);
            this.grpRegBack.Location = new System.Drawing.Point(12, 9);
            this.grpRegBack.Name = "grpRegBack";
            this.grpRegBack.Size = new System.Drawing.Size(860, 212);
            this.grpRegBack.TabIndex = 0;
            this.grpRegBack.TabStop = false;
            this.grpRegBack.Text = "RegBack Folder (built-in periodic registry backup)";
            // 
            // lvHives
            // 
            this.lvHives.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colHive,
            this.colSize,
            this.colModified});
            this.lvHives.FullRowSelect = true;
            this.lvHives.GridLines = true;
            this.lvHives.HideSelection = false;
            this.lvHives.Location = new System.Drawing.Point(12, 24);
            this.lvHives.MultiSelect = false;
            this.lvHives.Name = "lvHives";
            this.lvHives.Size = new System.Drawing.Size(440, 175);
            this.lvHives.TabIndex = 0;
            this.lvHives.UseCompatibleStateImageBehavior = false;
            this.lvHives.View = System.Windows.Forms.View.Details;
            // 
            // columns
            // 
            this.colHive.Text = "Hive File";
            this.colHive.Width = 140;
            this.colSize.Text = "Size";
            this.colSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.colSize.Width = 110;
            this.colModified.Text = "Last Modified";
            this.colModified.Width = 165;
            // 
            // lblRegBackStatus
            // 
            this.lblRegBackStatus.AutoSize = true;
            this.lblRegBackStatus.Location = new System.Drawing.Point(468, 27);
            this.lblRegBackStatus.Name = "lblRegBackStatus";
            this.lblRegBackStatus.Text = "Status:";
            // 
            // txtRegBackStatus
            // 
            this.txtRegBackStatus.BackColor = System.Drawing.Color.White;
            this.txtRegBackStatus.Location = new System.Drawing.Point(560, 24);
            this.txtRegBackStatus.Name = "txtRegBackStatus";
            this.txtRegBackStatus.ReadOnly = true;
            this.txtRegBackStatus.Size = new System.Drawing.Size(285, 23);
            this.txtRegBackStatus.TabStop = false;
            // 
            // lblPeriodic
            // 
            this.lblPeriodic.AutoSize = true;
            this.lblPeriodic.Location = new System.Drawing.Point(468, 59);
            this.lblPeriodic.Name = "lblPeriodic";
            this.lblPeriodic.Text = "Backup setting:";
            // 
            // txtPeriodic
            // 
            this.txtPeriodic.BackColor = System.Drawing.Color.White;
            this.txtPeriodic.Location = new System.Drawing.Point(560, 56);
            this.txtPeriodic.Name = "txtPeriodic";
            this.txtPeriodic.ReadOnly = true;
            this.txtPeriodic.Size = new System.Drawing.Size(285, 23);
            this.txtPeriodic.TabStop = false;
            // 
            // lblRegBackPath
            // 
            this.lblRegBackPath.AutoSize = true;
            this.lblRegBackPath.Location = new System.Drawing.Point(468, 91);
            this.lblRegBackPath.Name = "lblRegBackPath";
            this.lblRegBackPath.Text = "Folder:";
            // 
            // txtRegBackPath
            // 
            this.txtRegBackPath.BackColor = System.Drawing.Color.White;
            this.txtRegBackPath.Location = new System.Drawing.Point(560, 88);
            this.txtRegBackPath.Name = "txtRegBackPath";
            this.txtRegBackPath.ReadOnly = true;
            this.txtRegBackPath.Size = new System.Drawing.Size(285, 23);
            this.txtRegBackPath.TabStop = false;
            // 
            // grpTask
            // 
            this.grpTask.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpTask.Controls.Add(this.lblTaskLastRun);
            this.grpTask.Controls.Add(this.txtTaskLastRun);
            this.grpTask.Controls.Add(this.lblTaskResult);
            this.grpTask.Controls.Add(this.txtTaskResult);
            this.grpTask.Controls.Add(this.lblTaskState);
            this.grpTask.Controls.Add(this.txtTaskState);
            this.grpTask.Controls.Add(this.lblTaskNextRun);
            this.grpTask.Controls.Add(this.txtTaskNextRun);
            this.grpTask.Location = new System.Drawing.Point(12, 229);
            this.grpTask.Name = "grpTask";
            this.grpTask.Size = new System.Drawing.Size(860, 96);
            this.grpTask.TabIndex = 1;
            this.grpTask.TabStop = false;
            this.grpTask.Text = "RegIdleBackup Scheduled Task";
            // 
            // lblTaskLastRun
            // 
            this.lblTaskLastRun.AutoSize = true;
            this.lblTaskLastRun.Location = new System.Drawing.Point(12, 28);
            this.lblTaskLastRun.Name = "lblTaskLastRun";
            this.lblTaskLastRun.Text = "Last run:";
            // 
            // txtTaskLastRun
            // 
            this.txtTaskLastRun.BackColor = System.Drawing.Color.White;
            this.txtTaskLastRun.Location = new System.Drawing.Point(85, 25);
            this.txtTaskLastRun.Name = "txtTaskLastRun";
            this.txtTaskLastRun.ReadOnly = true;
            this.txtTaskLastRun.Size = new System.Drawing.Size(330, 23);
            this.txtTaskLastRun.TabStop = false;
            // 
            // lblTaskResult
            // 
            this.lblTaskResult.AutoSize = true;
            this.lblTaskResult.Location = new System.Drawing.Point(440, 28);
            this.lblTaskResult.Name = "lblTaskResult";
            this.lblTaskResult.Text = "Last result:";
            // 
            // txtTaskResult
            // 
            this.txtTaskResult.BackColor = System.Drawing.Color.White;
            this.txtTaskResult.Location = new System.Drawing.Point(515, 25);
            this.txtTaskResult.Name = "txtTaskResult";
            this.txtTaskResult.ReadOnly = true;
            this.txtTaskResult.Size = new System.Drawing.Size(330, 23);
            this.txtTaskResult.TabStop = false;
            // 
            // lblTaskState
            // 
            this.lblTaskState.AutoSize = true;
            this.lblTaskState.Location = new System.Drawing.Point(12, 60);
            this.lblTaskState.Name = "lblTaskState";
            this.lblTaskState.Text = "State:";
            // 
            // txtTaskState
            // 
            this.txtTaskState.BackColor = System.Drawing.Color.White;
            this.txtTaskState.Location = new System.Drawing.Point(85, 57);
            this.txtTaskState.Name = "txtTaskState";
            this.txtTaskState.ReadOnly = true;
            this.txtTaskState.Size = new System.Drawing.Size(330, 23);
            this.txtTaskState.TabStop = false;
            // 
            // lblTaskNextRun
            // 
            this.lblTaskNextRun.AutoSize = true;
            this.lblTaskNextRun.Location = new System.Drawing.Point(440, 60);
            this.lblTaskNextRun.Name = "lblTaskNextRun";
            this.lblTaskNextRun.Text = "Next run:";
            // 
            // txtTaskNextRun
            // 
            this.txtTaskNextRun.BackColor = System.Drawing.Color.White;
            this.txtTaskNextRun.Location = new System.Drawing.Point(515, 57);
            this.txtTaskNextRun.Name = "txtTaskNextRun";
            this.txtTaskNextRun.ReadOnly = true;
            this.txtTaskNextRun.Size = new System.Drawing.Size(330, 23);
            this.txtTaskNextRun.TabStop = false;
            // 
            // grpSnapshots
            // 
            this.grpSnapshots.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpSnapshots.Controls.Add(this.lblRpCount);
            this.grpSnapshots.Controls.Add(this.txtRpCount);
            this.grpSnapshots.Controls.Add(this.lblRpNewest);
            this.grpSnapshots.Controls.Add(this.txtRpNewest);
            this.grpSnapshots.Controls.Add(this.lblScCount);
            this.grpSnapshots.Controls.Add(this.txtScCount);
            this.grpSnapshots.Controls.Add(this.lblScNewest);
            this.grpSnapshots.Controls.Add(this.txtScNewest);
            this.grpSnapshots.Location = new System.Drawing.Point(12, 333);
            this.grpSnapshots.Name = "grpSnapshots";
            this.grpSnapshots.Size = new System.Drawing.Size(860, 96);
            this.grpSnapshots.TabIndex = 2;
            this.grpSnapshots.TabStop = false;
            this.grpSnapshots.Text = "Snapshot-based Registry Backups (every snapshot contains the hives)";
            // 
            // lblRpCount
            // 
            this.lblRpCount.AutoSize = true;
            this.lblRpCount.Location = new System.Drawing.Point(12, 28);
            this.lblRpCount.Name = "lblRpCount";
            this.lblRpCount.Text = "Restore points:";
            // 
            // txtRpCount
            // 
            this.txtRpCount.BackColor = System.Drawing.Color.White;
            this.txtRpCount.Location = new System.Drawing.Point(115, 25);
            this.txtRpCount.Name = "txtRpCount";
            this.txtRpCount.ReadOnly = true;
            this.txtRpCount.Size = new System.Drawing.Size(70, 23);
            this.txtRpCount.TabStop = false;
            // 
            // lblRpNewest
            // 
            this.lblRpNewest.AutoSize = true;
            this.lblRpNewest.Location = new System.Drawing.Point(210, 28);
            this.lblRpNewest.Name = "lblRpNewest";
            this.lblRpNewest.Text = "Newest:";
            // 
            // txtRpNewest
            // 
            this.txtRpNewest.BackColor = System.Drawing.Color.White;
            this.txtRpNewest.Location = new System.Drawing.Point(270, 25);
            this.txtRpNewest.Name = "txtRpNewest";
            this.txtRpNewest.ReadOnly = true;
            this.txtRpNewest.Size = new System.Drawing.Size(575, 23);
            this.txtRpNewest.TabStop = false;
            // 
            // lblScCount
            // 
            this.lblScCount.AutoSize = true;
            this.lblScCount.Location = new System.Drawing.Point(12, 60);
            this.lblScCount.Name = "lblScCount";
            this.lblScCount.Text = "Shadow copies:";
            // 
            // txtScCount
            // 
            this.txtScCount.BackColor = System.Drawing.Color.White;
            this.txtScCount.Location = new System.Drawing.Point(115, 57);
            this.txtScCount.Name = "txtScCount";
            this.txtScCount.ReadOnly = true;
            this.txtScCount.Size = new System.Drawing.Size(70, 23);
            this.txtScCount.TabStop = false;
            // 
            // lblScNewest
            // 
            this.lblScNewest.AutoSize = true;
            this.lblScNewest.Location = new System.Drawing.Point(210, 60);
            this.lblScNewest.Name = "lblScNewest";
            this.lblScNewest.Text = "Newest:";
            // 
            // txtScNewest
            // 
            this.txtScNewest.BackColor = System.Drawing.Color.White;
            this.txtScNewest.Location = new System.Drawing.Point(270, 57);
            this.txtScNewest.Name = "txtScNewest";
            this.txtScNewest.ReadOnly = true;
            this.txtScNewest.Size = new System.Drawing.Size(575, 23);
            this.txtScNewest.TabStop = false;
            // 
            // grpVerdict
            // 
            this.grpVerdict.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpVerdict.Controls.Add(this.txtVerdict);
            this.grpVerdict.Location = new System.Drawing.Point(12, 437);
            this.grpVerdict.Name = "grpVerdict";
            this.grpVerdict.Size = new System.Drawing.Size(860, 104);
            this.grpVerdict.TabIndex = 3;
            this.grpVerdict.TabStop = false;
            this.grpVerdict.Text = "Verdict";
            // 
            // txtVerdict
            // 
            this.txtVerdict.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtVerdict.BackColor = System.Drawing.SystemColors.Control;
            this.txtVerdict.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtVerdict.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtVerdict.Location = new System.Drawing.Point(12, 24);
            this.txtVerdict.Multiline = true;
            this.txtVerdict.Name = "txtVerdict";
            this.txtVerdict.ReadOnly = true;
            this.txtVerdict.Size = new System.Drawing.Size(836, 68);
            this.txtVerdict.TabStop = false;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Location = new System.Drawing.Point(12, 553);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 28);
            this.btnRefresh.TabIndex = 4;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.Btn_Refresh_Click);
            // 
            // btnEnableRegBack
            // 
            this.btnEnableRegBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEnableRegBack.Location = new System.Drawing.Point(118, 553);
            this.btnEnableRegBack.Name = "btnEnableRegBack";
            this.btnEnableRegBack.Size = new System.Drawing.Size(130, 28);
            this.btnEnableRegBack.TabIndex = 5;
            this.btnEnableRegBack.Text = "Enable RegBack";
            this.btnEnableRegBack.UseVisualStyleBackColor = true;
            this.btnEnableRegBack.Click += new System.EventHandler(this.Btn_Enable_RegBack_Click);
            // 
            // btnBackupNow
            // 
            this.btnBackupNow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnBackupNow.Location = new System.Drawing.Point(254, 553);
            this.btnBackupNow.Name = "btnBackupNow";
            this.btnBackupNow.Size = new System.Drawing.Size(110, 28);
            this.btnBackupNow.TabIndex = 6;
            this.btnBackupNow.Text = "Backup Now";
            this.btnBackupNow.UseVisualStyleBackColor = true;
            this.btnBackupNow.Click += new System.EventHandler(this.Btn_Backup_Now_Click);
            // 
            // btnOpenFolder
            // 
            this.btnOpenFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOpenFolder.Location = new System.Drawing.Point(370, 553);
            this.btnOpenFolder.Name = "btnOpenFolder";
            this.btnOpenFolder.Size = new System.Drawing.Size(110, 28);
            this.btnOpenFolder.TabIndex = 7;
            this.btnOpenFolder.Text = "Open Folder";
            this.btnOpenFolder.UseVisualStyleBackColor = true;
            this.btnOpenFolder.Click += new System.EventHandler(this.Btn_Open_Folder_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(782, 553);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 28);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.Btn_Close_Click);
            // 
            // Registry_Backup_Form
            // 
            this.AcceptButton = this.btnClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 593);
            this.Controls.Add(this.grpRegBack);
            this.Controls.Add(this.grpTask);
            this.Controls.Add(this.grpSnapshots);
            this.Controls.Add(this.grpVerdict);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnEnableRegBack);
            this.Controls.Add(this.btnBackupNow);
            this.Controls.Add(this.btnOpenFolder);
            this.Controls.Add(this.btnClose);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Registry_Backup_Form";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Registry Backups";
            this.grpRegBack.ResumeLayout(false);
            this.grpRegBack.PerformLayout();
            this.grpTask.ResumeLayout(false);
            this.grpTask.PerformLayout();
            this.grpSnapshots.ResumeLayout(false);
            this.grpSnapshots.PerformLayout();
            this.grpVerdict.ResumeLayout(false);
            this.grpVerdict.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.GroupBox grpRegBack;
        private System.Windows.Forms.ListView lvHives;
        private System.Windows.Forms.ColumnHeader colHive;
        private System.Windows.Forms.ColumnHeader colSize;
        private System.Windows.Forms.ColumnHeader colModified;
        private System.Windows.Forms.Label lblRegBackStatus;
        private System.Windows.Forms.TextBox txtRegBackStatus;
        private System.Windows.Forms.Label lblPeriodic;
        private System.Windows.Forms.TextBox txtPeriodic;
        private System.Windows.Forms.Label lblRegBackPath;
        private System.Windows.Forms.TextBox txtRegBackPath;
        private System.Windows.Forms.GroupBox grpTask;
        private System.Windows.Forms.Label lblTaskLastRun;
        private System.Windows.Forms.TextBox txtTaskLastRun;
        private System.Windows.Forms.Label lblTaskResult;
        private System.Windows.Forms.TextBox txtTaskResult;
        private System.Windows.Forms.Label lblTaskState;
        private System.Windows.Forms.TextBox txtTaskState;
        private System.Windows.Forms.Label lblTaskNextRun;
        private System.Windows.Forms.TextBox txtTaskNextRun;
        private System.Windows.Forms.GroupBox grpSnapshots;
        private System.Windows.Forms.Label lblRpCount;
        private System.Windows.Forms.TextBox txtRpCount;
        private System.Windows.Forms.Label lblRpNewest;
        private System.Windows.Forms.TextBox txtRpNewest;
        private System.Windows.Forms.Label lblScCount;
        private System.Windows.Forms.TextBox txtScCount;
        private System.Windows.Forms.Label lblScNewest;
        private System.Windows.Forms.TextBox txtScNewest;
        private System.Windows.Forms.GroupBox grpVerdict;
        private System.Windows.Forms.TextBox txtVerdict;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnEnableRegBack;
        private System.Windows.Forms.Button btnBackupNow;
        private System.Windows.Forms.Button btnOpenFolder;
        private System.Windows.Forms.Button btnClose;
    }
}
