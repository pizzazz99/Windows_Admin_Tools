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
            txtOutput = new RichTextBox();
            btnClear = new Button();
            btnCopyAll = new Button();
            btnSave = new Button();
            btnClose = new Button();
            Commands_Panel = new Panel();
            grpSystemConfig = new GroupBox();
            btnDriverQuery = new Button();
            btnHotfixes = new Button();
            btnUpdates = new Button();
            grpIdentity = new GroupBox();
            btnWhoami = new Button();
            btnNetUser = new Button();
            btnLocalAdmins = new Button();
            btnQueryUser = new Button();
            btnNetSession = new Button();
            grpNetwork = new GroupBox();
            btnIpconfig = new Button();
            btnArp = new Button();
            btnRoute = new Button();
            btnNetstat = new Button();
            btnNslookup = new Button();
            btnFlushDns = new Button();
            grpSystem = new GroupBox();
            btnSystemInfo = new Button();
            btnHostname = new Button();
            btnTasklist = new Button();
            btnTasklistSvc = new Button();
            btnScQuery = new Button();
            grpDisk = new GroupBox();
            btnFreeSpacebyDrive = new Button();
            btnChkdsk = new Button();
            btnVol = new Button();
            grpHealth = new GroupBox();
            btnSfcVerify = new Button();
            btnBatteryReport = new Button();
            btnSystemEvents = new Button();
            btnUptime = new Button();
            lvUpdates = new ListView();
            Commands_Label = new Label();
            Message_Textbox = new TextBox();
            lvHotfixes = new ListView();
            Commands_Panel.SuspendLayout();
            grpSystemConfig.SuspendLayout();
            grpIdentity.SuspendLayout();
            grpNetwork.SuspendLayout();
            grpSystem.SuspendLayout();
            grpDisk.SuspendLayout();
            grpHealth.SuspendLayout();
            SuspendLayout();
            // 
            // txtOutput
            // 
            txtOutput.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtOutput.BackColor = Color.FromArgb(30, 30, 30);
            txtOutput.BorderStyle = BorderStyle.FixedSingle;
            txtOutput.DetectUrls = false;
            txtOutput.Font = new Font("Consolas", 9.5F);
            txtOutput.ForeColor = Color.Gainsboro;
            txtOutput.Location = new Point(429, 27);
            txtOutput.Name = "txtOutput";
            txtOutput.ReadOnly = true;
            txtOutput.Size = new Size(599, 564);
            txtOutput.TabIndex = 6;
            txtOutput.Text = "";
            txtOutput.WordWrap = false;
            // 
            // btnClear
            // 
            btnClear.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnClear.Location = new Point(428, 606);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(90, 28);
            btnClear.TabIndex = 7;
            btnClear.Text = "Clear";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += Btn_Clear_Click;
            // 
            // btnCopyAll
            // 
            btnCopyAll.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCopyAll.Location = new Point(524, 606);
            btnCopyAll.Name = "btnCopyAll";
            btnCopyAll.Size = new Size(90, 28);
            btnCopyAll.TabIndex = 8;
            btnCopyAll.Text = "Copy All";
            btnCopyAll.UseVisualStyleBackColor = true;
            btnCopyAll.Click += Btn_Copy_All_Click;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSave.Location = new Point(620, 606);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(90, 28);
            btnSave.TabIndex = 9;
            btnSave.Text = "Save...";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += Btn_Save_Click;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnClose.Location = new Point(937, 606);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(90, 28);
            btnClose.TabIndex = 10;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += Btn_Close_Click;
            // 
            // Commands_Panel
            // 
            Commands_Panel.Controls.Add(grpSystemConfig);
            Commands_Panel.Controls.Add(grpIdentity);
            Commands_Panel.Controls.Add(grpNetwork);
            Commands_Panel.Controls.Add(grpSystem);
            Commands_Panel.Controls.Add(grpDisk);
            Commands_Panel.Controls.Add(grpHealth);
            Commands_Panel.Location = new Point(12, 27);
            Commands_Panel.Name = "Commands_Panel";
            Commands_Panel.Size = new Size(402, 594);
            Commands_Panel.TabIndex = 11;
            // 
            // grpSystemConfig
            // 
            grpSystemConfig.Controls.Add(btnDriverQuery);
            grpSystemConfig.Controls.Add(btnHotfixes);
            grpSystemConfig.Controls.Add(btnUpdates);
            grpSystemConfig.Location = new Point(223, 337);
            grpSystemConfig.Name = "grpSystemConfig";
            grpSystemConfig.Size = new Size(162, 129);
            grpSystemConfig.TabIndex = 10;
            grpSystemConfig.TabStop = false;
            grpSystemConfig.Text = "System Config";
            // 
            // btnDriverQuery
            // 
            btnDriverQuery.Location = new Point(12, 26);
            btnDriverQuery.Name = "btnDriverQuery";
            btnDriverQuery.Size = new Size(137, 28);
            btnDriverQuery.TabIndex = 8;
            btnDriverQuery.Text = "Installed Drivers";
            btnDriverQuery.UseVisualStyleBackColor = true;
            btnDriverQuery.Click += btnDriverQuery_Click;
            // 
            // btnHotfixes
            // 
            btnHotfixes.Location = new Point(12, 58);
            btnHotfixes.Name = "btnHotfixes";
            btnHotfixes.Size = new Size(137, 28);
            btnHotfixes.TabIndex = 9;
            btnHotfixes.Text = "Installed Hotfixes";
            btnHotfixes.UseVisualStyleBackColor = true;
            btnHotfixes.Click += btnHotfixes_Click;
            // 
            // btnUpdates
            // 
            btnUpdates.Location = new Point(12, 90);
            btnUpdates.Name = "btnUpdates";
            btnUpdates.Size = new Size(137, 28);
            btnUpdates.TabIndex = 10;
            btnUpdates.Text = "Update History";
            btnUpdates.UseVisualStyleBackColor = true;
            btnUpdates.Click += btnUpdates_Click;
            // 
            // grpIdentity
            // 
            grpIdentity.Controls.Add(btnWhoami);
            grpIdentity.Controls.Add(btnNetUser);
            grpIdentity.Controls.Add(btnLocalAdmins);
            grpIdentity.Controls.Add(btnQueryUser);
            grpIdentity.Controls.Add(btnNetSession);
            grpIdentity.Location = new Point(12, 7);
            grpIdentity.Name = "grpIdentity";
            grpIdentity.Size = new Size(174, 188);
            grpIdentity.TabIndex = 5;
            grpIdentity.TabStop = false;
            grpIdentity.Text = "Identity && Sessions";
            // 
            // btnWhoami
            // 
            btnWhoami.Location = new Point(12, 22);
            btnWhoami.Name = "btnWhoami";
            btnWhoami.Size = new Size(147, 28);
            btnWhoami.TabIndex = 0;
            btnWhoami.Text = "Who Am I (full)";
            btnWhoami.UseVisualStyleBackColor = true;
            btnWhoami.Click += btnWhoami_Click;
            // 
            // btnNetUser
            // 
            btnNetUser.Location = new Point(12, 54);
            btnNetUser.Name = "btnNetUser";
            btnNetUser.Size = new Size(147, 28);
            btnNetUser.TabIndex = 1;
            btnNetUser.Text = "Local Accounts";
            btnNetUser.UseVisualStyleBackColor = true;
            btnNetUser.Click += btnNetUser_Click;
            // 
            // btnLocalAdmins
            // 
            btnLocalAdmins.Location = new Point(12, 86);
            btnLocalAdmins.Name = "btnLocalAdmins";
            btnLocalAdmins.Size = new Size(147, 28);
            btnLocalAdmins.TabIndex = 2;
            btnLocalAdmins.Text = "Local Administrators";
            btnLocalAdmins.UseVisualStyleBackColor = true;
            btnLocalAdmins.Click += btnLocalAdmins_Click;
            // 
            // btnQueryUser
            // 
            btnQueryUser.Location = new Point(12, 118);
            btnQueryUser.Name = "btnQueryUser";
            btnQueryUser.Size = new Size(147, 28);
            btnQueryUser.TabIndex = 3;
            btnQueryUser.Text = "Logged-On Sessions";
            btnQueryUser.UseVisualStyleBackColor = true;
            btnQueryUser.Click += btnQueryUser_Click;
            // 
            // btnNetSession
            // 
            btnNetSession.Location = new Point(12, 150);
            btnNetSession.Name = "btnNetSession";
            btnNetSession.Size = new Size(147, 28);
            btnNetSession.TabIndex = 4;
            btnNetSession.Text = "Inbound SMB Sessions";
            btnNetSession.UseVisualStyleBackColor = true;
            btnNetSession.Click += btnNetSession_Click;
            // 
            // grpNetwork
            // 
            grpNetwork.Controls.Add(btnIpconfig);
            grpNetwork.Controls.Add(btnArp);
            grpNetwork.Controls.Add(btnRoute);
            grpNetwork.Controls.Add(btnNetstat);
            grpNetwork.Controls.Add(btnNslookup);
            grpNetwork.Controls.Add(btnFlushDns);
            grpNetwork.Location = new Point(12, 205);
            grpNetwork.Name = "grpNetwork";
            grpNetwork.Size = new Size(174, 220);
            grpNetwork.TabIndex = 6;
            grpNetwork.TabStop = false;
            grpNetwork.Text = "Network";
            // 
            // btnIpconfig
            // 
            btnIpconfig.Location = new Point(12, 22);
            btnIpconfig.Name = "btnIpconfig";
            btnIpconfig.Size = new Size(147, 28);
            btnIpconfig.TabIndex = 0;
            btnIpconfig.Text = "IP Configuration (full)";
            btnIpconfig.UseVisualStyleBackColor = true;
            btnIpconfig.Click += btnIpconfig_Click;
            // 
            // btnArp
            // 
            btnArp.Location = new Point(12, 54);
            btnArp.Name = "btnArp";
            btnArp.Size = new Size(147, 28);
            btnArp.TabIndex = 1;
            btnArp.Text = "ARP Cache";
            btnArp.UseVisualStyleBackColor = true;
            btnArp.Click += btnArp_Click;
            // 
            // btnRoute
            // 
            btnRoute.Location = new Point(12, 86);
            btnRoute.Name = "btnRoute";
            btnRoute.Size = new Size(147, 28);
            btnRoute.TabIndex = 2;
            btnRoute.Text = "Routing Table";
            btnRoute.UseVisualStyleBackColor = true;
            btnRoute.Click += btnRoute_Click;
            // 
            // btnNetstat
            // 
            btnNetstat.Location = new Point(12, 118);
            btnNetstat.Name = "btnNetstat";
            btnNetstat.Size = new Size(147, 28);
            btnNetstat.TabIndex = 3;
            btnNetstat.Text = "Connections + PIDs";
            btnNetstat.UseVisualStyleBackColor = true;
            btnNetstat.Click += btnNetstat_Click;
            // 
            // btnNslookup
            // 
            btnNslookup.Location = new Point(12, 150);
            btnNslookup.Name = "btnNslookup";
            btnNslookup.Size = new Size(147, 28);
            btnNslookup.TabIndex = 4;
            btnNslookup.Text = "DNS Check";
            btnNslookup.UseVisualStyleBackColor = true;
            btnNslookup.Click += btnNslookup_Click;
            // 
            // btnFlushDns
            // 
            btnFlushDns.Location = new Point(12, 182);
            btnFlushDns.Name = "btnFlushDns";
            btnFlushDns.Size = new Size(147, 28);
            btnFlushDns.TabIndex = 5;
            btnFlushDns.Text = "Flush DNS Cache";
            btnFlushDns.UseVisualStyleBackColor = true;
            btnFlushDns.Click += btnFlushDns_Click;
            // 
            // grpSystem
            // 
            grpSystem.Controls.Add(btnSystemInfo);
            grpSystem.Controls.Add(btnHostname);
            grpSystem.Controls.Add(btnTasklist);
            grpSystem.Controls.Add(btnTasklistSvc);
            grpSystem.Controls.Add(btnScQuery);
            grpSystem.Location = new Point(223, 7);
            grpSystem.Name = "grpSystem";
            grpSystem.Size = new Size(162, 188);
            grpSystem.TabIndex = 7;
            grpSystem.TabStop = false;
            grpSystem.Text = "System State";
            // 
            // btnSystemInfo
            // 
            btnSystemInfo.Location = new Point(12, 22);
            btnSystemInfo.Name = "btnSystemInfo";
            btnSystemInfo.Size = new Size(137, 28);
            btnSystemInfo.TabIndex = 0;
            btnSystemInfo.Text = "System Info (slow)";
            btnSystemInfo.UseVisualStyleBackColor = true;
            btnSystemInfo.Click += btnSystemInfo_Click;
            // 
            // btnHostname
            // 
            btnHostname.Location = new Point(12, 54);
            btnHostname.Name = "btnHostname";
            btnHostname.Size = new Size(137, 28);
            btnHostname.TabIndex = 1;
            btnHostname.Text = "Hostname";
            btnHostname.UseVisualStyleBackColor = true;
            btnHostname.Click += btnHostname_Click;
            // 
            // btnTasklist
            // 
            btnTasklist.Location = new Point(12, 86);
            btnTasklist.Name = "btnTasklist";
            btnTasklist.Size = new Size(137, 28);
            btnTasklist.TabIndex = 2;
            btnTasklist.Text = "Running Processes";
            btnTasklist.UseVisualStyleBackColor = true;
            btnTasklist.Click += btnTasklist_Click;
            // 
            // btnTasklistSvc
            // 
            btnTasklistSvc.Location = new Point(12, 118);
            btnTasklistSvc.Name = "btnTasklistSvc";
            btnTasklistSvc.Size = new Size(137, 28);
            btnTasklistSvc.TabIndex = 3;
            btnTasklistSvc.Text = "Processes + Services";
            btnTasklistSvc.UseVisualStyleBackColor = true;
            btnTasklistSvc.Click += btnTasklistSvc_Click;
            // 
            // btnScQuery
            // 
            btnScQuery.Location = new Point(12, 150);
            btnScQuery.Name = "btnScQuery";
            btnScQuery.Size = new Size(137, 28);
            btnScQuery.TabIndex = 4;
            btnScQuery.Text = "Running Services";
            btnScQuery.UseVisualStyleBackColor = true;
            btnScQuery.Click += btnScQuery_Click;
            // 
            // grpDisk
            // 
            grpDisk.Controls.Add(btnFreeSpacebyDrive);
            grpDisk.Controls.Add(btnChkdsk);
            grpDisk.Controls.Add(btnVol);
            grpDisk.Location = new Point(223, 205);
            grpDisk.Name = "grpDisk";
            grpDisk.Size = new Size(162, 124);
            grpDisk.TabIndex = 8;
            grpDisk.TabStop = false;
            grpDisk.Text = "Disk && Storage";
            // 
            // btnFreeSpacebyDrive
            // 
            btnFreeSpacebyDrive.Location = new Point(12, 22);
            btnFreeSpacebyDrive.Name = "btnFreeSpacebyDrive";
            btnFreeSpacebyDrive.Size = new Size(137, 28);
            btnFreeSpacebyDrive.TabIndex = 0;
            btnFreeSpacebyDrive.Text = "Free Space by Drive";
            btnFreeSpacebyDrive.UseVisualStyleBackColor = true;
            btnFreeSpacebyDrive.Click += btnFreeSpacebyDrive_Click;
            // 
            // btnChkdsk
            // 
            btnChkdsk.Location = new Point(12, 54);
            btnChkdsk.Name = "btnChkdsk";
            btnChkdsk.Size = new Size(137, 28);
            btnChkdsk.TabIndex = 1;
            btnChkdsk.Text = "Disk Status (slow)";
            btnChkdsk.UseVisualStyleBackColor = true;
            btnChkdsk.Click += btnChkdsk_Click;
            // 
            // btnVol
            // 
            btnVol.Location = new Point(12, 86);
            btnVol.Name = "btnVol";
            btnVol.Size = new Size(137, 28);
            btnVol.TabIndex = 2;
            btnVol.Text = "Volume Label && Serial";
            btnVol.UseVisualStyleBackColor = true;
            btnVol.Click += btnVol_Click;
            // 
            // grpHealth
            // 
            grpHealth.Controls.Add(btnSfcVerify);
            grpHealth.Controls.Add(btnBatteryReport);
            grpHealth.Controls.Add(btnSystemEvents);
            grpHealth.Controls.Add(btnUptime);
            grpHealth.Location = new Point(12, 431);
            grpHealth.Name = "grpHealth";
            grpHealth.Size = new Size(174, 156);
            grpHealth.TabIndex = 9;
            grpHealth.TabStop = false;
            grpHealth.Text = "Health && Logs";
            // 
            // btnSfcVerify
            // 
            btnSfcVerify.Location = new Point(12, 22);
            btnSfcVerify.Name = "btnSfcVerify";
            btnSfcVerify.Size = new Size(147, 28);
            btnSfcVerify.TabIndex = 0;
            btnSfcVerify.Text = "Verify System Files (slow)";
            btnSfcVerify.UseVisualStyleBackColor = true;
            btnSfcVerify.Click += btnSfcVerify_Click;
            // 
            // btnBatteryReport
            // 
            btnBatteryReport.Location = new Point(12, 54);
            btnBatteryReport.Name = "btnBatteryReport";
            btnBatteryReport.Size = new Size(147, 28);
            btnBatteryReport.TabIndex = 1;
            btnBatteryReport.Text = "Battery Report";
            btnBatteryReport.UseVisualStyleBackColor = true;
            btnBatteryReport.Click += btnBatteryReport_Click;
            // 
            // btnSystemEvents
            // 
            btnSystemEvents.Location = new Point(12, 86);
            btnSystemEvents.Name = "btnSystemEvents";
            btnSystemEvents.Size = new Size(147, 28);
            btnSystemEvents.TabIndex = 2;
            btnSystemEvents.Text = "Last 20 System Events";
            btnSystemEvents.UseVisualStyleBackColor = true;
            btnSystemEvents.Click += btnSystemEvents_Click;
            // 
            // btnUptime
            // 
            btnUptime.Location = new Point(12, 118);
            btnUptime.Name = "btnUptime";
            btnUptime.Size = new Size(147, 28);
            btnUptime.TabIndex = 3;
            btnUptime.Text = "Uptime / Boot Time";
            btnUptime.UseVisualStyleBackColor = true;
            btnUptime.Click += btnUptime_Click;
            // 
            // lvUpdates
            // 
            lvUpdates.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lvUpdates.BackColor = Color.FromArgb(30, 30, 30);
            lvUpdates.BorderStyle = BorderStyle.FixedSingle;
            lvUpdates.ForeColor = Color.Gainsboro;
            lvUpdates.FullRowSelect = true;
            lvUpdates.HideSelection = false;
            lvUpdates.Location = new Point(429, 34);
            lvUpdates.MultiSelect = false;
            lvUpdates.Name = "lvUpdates";
            lvUpdates.Size = new Size(599, 553);
            lvUpdates.TabIndex = 6;
            lvUpdates.UseCompatibleStateImageBehavior = false;
            lvUpdates.View = View.Details;
            lvUpdates.Visible = false;
            lvUpdates.Columns.Add("Date", 130);
            lvUpdates.Columns.Add("Title", 300);
            lvUpdates.Columns.Add("KB", 80);
            lvUpdates.Columns.Add("Operation", 70);
            lvUpdates.Columns.Add("Result", 100);
            lvUpdates.DoubleClick += lvUpdates_DoubleClick;
            // 
            // Commands_Label
            // 
            Commands_Label.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            Commands_Label.AutoSize = true;
            Commands_Label.Location = new Point(12, 9);
            Commands_Label.Name = "Commands_Label";
            Commands_Label.Size = new Size(72, 15);
            Commands_Label.TabIndex = 12;
            Commands_Label.Text = "Commands:";
            // 
            // Message_Textbox
            // 
            Message_Textbox.BackColor = SystemColors.Control;
            Message_Textbox.BorderStyle = BorderStyle.None;
            Message_Textbox.Location = new Point(430, 643);
            Message_Textbox.Name = "Message_Textbox";
            Message_Textbox.Size = new Size(598, 16);
            Message_Textbox.TabIndex = 13;
            // 
            // lvHotfixes
            // 
            lvHotfixes.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lvHotfixes.BackColor = Color.FromArgb(30, 30, 30);
            lvHotfixes.BorderStyle = BorderStyle.FixedSingle;
            lvHotfixes.ForeColor = Color.Gainsboro;
            lvHotfixes.FullRowSelect = true;
            lvHotfixes.HideSelection = false;
            lvHotfixes.Location = new Point(429, 34);
            lvHotfixes.MultiSelect = false;
            lvHotfixes.Name = "lvHotfixes";
            lvHotfixes.Size = new Size(599, 553);
            lvHotfixes.TabIndex = 6;
            lvHotfixes.UseCompatibleStateImageBehavior = false;
            lvHotfixes.View = View.Details;
            lvHotfixes.Visible = false;
            lvHotfixes.Columns.Add("HotFix ID", 110);
            lvHotfixes.Columns.Add("Description", 160);
            lvHotfixes.Columns.Add("Installed On", 110);
            lvHotfixes.Columns.Add("Installed By", 200);
            lvHotfixes.DoubleClick += lvHotfixes_DoubleClick;
            // 
            // Commands_Form
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnClose;
            ClientSize = new Size(1060, 673);
            Controls.Add(Message_Textbox);
            Controls.Add(Commands_Label);
            Controls.Add(Commands_Panel);
            Controls.Add(btnClear);
            Controls.Add(btnCopyAll);
            Controls.Add(btnSave);
            Controls.Add(btnClose);
            Controls.Add(txtOutput);
            Controls.Add(lvUpdates);
            Controls.Add(lvHotfixes);
            Font = new Font("Segoe UI", 9F);
            MinimizeBox = false;
            MinimumSize = new Size(1016, 650);
            Name = "Commands_Form";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Admin Commands";
            Commands_Panel.ResumeLayout(false);
            grpSystemConfig.ResumeLayout(false);
            grpIdentity.ResumeLayout(false);
            grpNetwork.ResumeLayout(false);
            grpSystem.ResumeLayout(false);
            grpDisk.ResumeLayout(false);
            grpHealth.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.RichTextBox txtOutput;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnCopyAll;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private Panel Commands_Panel;
        private GroupBox grpIdentity;
        private Button btnWhoami;
        private Button btnNetUser;
        private Button btnLocalAdmins;
        private Button btnQueryUser;
        private Button btnNetSession;
        private GroupBox grpNetwork;
        private Button btnIpconfig;
        private Button btnArp;
        private Button btnRoute;
        private Button btnNetstat;
        private Button btnNslookup;
        private Button btnFlushDns;
        private GroupBox grpSystem;
        private Button btnSystemInfo;
        private Button btnHostname;
        private Button btnTasklist;
        private Button btnTasklistSvc;
        private Button btnScQuery;
        private GroupBox grpDisk;
        private Button btnFreeSpacebyDrive;
        private Button btnChkdsk;
        private Button btnVol;
        private GroupBox grpHealth;
        private Button btnSfcVerify;
        private Button btnBatteryReport;
        private Button btnSystemEvents;
        private Button btnUptime;
        private Label Commands_Label;
        private System.Windows.Forms.ListView lvUpdates;
        private System.Windows.Forms.ListView lvHotfixes;
        private GroupBox grpSystemConfig;
        private Button btnDriverQuery;
        private Button btnHotfixes;
        private Button btnUpdates;
        private TextBox Message_Textbox;
    }
}