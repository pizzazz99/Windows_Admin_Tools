namespace Admin_Tools
{
    partial class Remote_Access_Form
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
            lblApps = new Label();
            lblOutput = new Label();
            txtOutput = new RichTextBox();
            BtnClear = new Button();
            BtnCopyAll = new Button();
            BtnSave = new Button();
            BtnClose = new Button();
            Apps_Panel = new Panel();
            grpTailscale = new GroupBox();
            BtnTsStatus = new Button();
            BtnTsStatusJson = new Button();
            BtnTsIp = new Button();
            BtnTsNetcheck = new Button();
            BtnTsDns = new Button();
            BtnTsPrefs = new Button();
            BtnTsVersion = new Button();
            grpRustDesk = new GroupBox();
            BtnRdGetId = new Button();
            BtnRdVersion = new Button();
            BtnRdServiceStatus = new Button();
            BtnRdConfig = new Button();
            grpRdp = new GroupBox();
            BtnRdpConnect = new Button();
            Apps_Panel.SuspendLayout();
            grpTailscale.SuspendLayout();
            grpRustDesk.SuspendLayout();
            grpRdp.SuspendLayout();
            SuspendLayout();
            // 
            // lblApps
            // 
            lblApps.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            lblApps.AutoSize = true;
            lblApps.Location = new Point(12, 9);
            lblApps.Name = "lblApps";
            lblApps.Size = new Size(76, 15);
            lblApps.TabIndex = 0;
            lblApps.Text = "Diagnostics:";
            // 
            // lblOutput
            // 
            lblOutput.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblOutput.AutoSize = true;
            lblOutput.Location = new Point(230, 9);
            lblOutput.Name = "lblOutput";
            lblOutput.Size = new Size(48, 15);
            lblOutput.TabIndex = 1;
            lblOutput.Text = "Output:";
            // 
            // txtOutput
            // 
            txtOutput.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtOutput.BackColor = Color.FromArgb(30, 30, 30);
            txtOutput.BorderStyle = BorderStyle.FixedSingle;
            txtOutput.DetectUrls = false;
            txtOutput.Font = new Font("Consolas", 9.5F);
            txtOutput.ForeColor = Color.Gainsboro;
            txtOutput.Location = new Point(230, 34);
            txtOutput.Name = "txtOutput";
            txtOutput.ReadOnly = true;
            txtOutput.Size = new Size(658, 503);
            txtOutput.TabIndex = 2;
            txtOutput.Text = "";
            txtOutput.WordWrap = false;
            // 
            // BtnClear
            // 
            BtnClear.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnClear.Location = new Point(494, 545);
            BtnClear.Name = "BtnClear";
            BtnClear.Size = new Size(90, 28);
            BtnClear.TabIndex = 3;
            BtnClear.Text = "Clear";
            BtnClear.UseVisualStyleBackColor = true;
            BtnClear.Click += Btn_Clear_Click;
            // 
            // BtnCopyAll
            // 
            BtnCopyAll.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnCopyAll.Location = new Point(590, 545);
            BtnCopyAll.Name = "BtnCopyAll";
            BtnCopyAll.Size = new Size(90, 28);
            BtnCopyAll.TabIndex = 4;
            BtnCopyAll.Text = "Copy All";
            BtnCopyAll.UseVisualStyleBackColor = true;
            BtnCopyAll.Click += Btn_Copy_All_Click;
            // 
            // BtnSave
            // 
            BtnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnSave.Location = new Point(686, 545);
            BtnSave.Name = "BtnSave";
            BtnSave.Size = new Size(90, 28);
            BtnSave.TabIndex = 5;
            BtnSave.Text = "Save...";
            BtnSave.UseVisualStyleBackColor = true;
            BtnSave.Click += Btn_Save_Click;
            // 
            // BtnClose
            // 
            BtnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnClose.Location = new Point(798, 545);
            BtnClose.Name = "BtnClose";
            BtnClose.Size = new Size(90, 28);
            BtnClose.TabIndex = 6;
            BtnClose.Text = "Close";
            BtnClose.UseVisualStyleBackColor = true;
            BtnClose.Click += Btn_Close_Click;
            // 
            // Apps_Panel
            // 
            Apps_Panel.Controls.Add(grpTailscale);
            Apps_Panel.Controls.Add(grpRustDesk);
            Apps_Panel.Controls.Add(grpRdp);
            Apps_Panel.Location = new Point(12, 27);
            Apps_Panel.Name = "Apps_Panel";
            Apps_Panel.Size = new Size(200, 510);
            Apps_Panel.TabIndex = 7;
            // 
            // grpTailscale
            // 
            grpTailscale.Controls.Add(BtnTsStatus);
            grpTailscale.Controls.Add(BtnTsStatusJson);
            grpTailscale.Controls.Add(BtnTsIp);
            grpTailscale.Controls.Add(BtnTsNetcheck);
            grpTailscale.Controls.Add(BtnTsDns);
            grpTailscale.Controls.Add(BtnTsPrefs);
            grpTailscale.Controls.Add(BtnTsVersion);
            grpTailscale.Location = new Point(12, 7);
            grpTailscale.Name = "grpTailscale";
            grpTailscale.Size = new Size(174, 256);
            grpTailscale.TabIndex = 0;
            grpTailscale.TabStop = false;
            grpTailscale.Text = "Tailscale";
            // 
            // BtnTsStatus
            // 
            BtnTsStatus.Location = new Point(12, 22);
            BtnTsStatus.Name = "BtnTsStatus";
            BtnTsStatus.Size = new Size(147, 28);
            BtnTsStatus.TabIndex = 0;
            BtnTsStatus.Text = "Status";
            BtnTsStatus.UseVisualStyleBackColor = true;
            BtnTsStatus.Click += BtnTsStatus_Click;
            // 
            // BtnTsStatusJson
            // 
            BtnTsStatusJson.Location = new Point(12, 54);
            BtnTsStatusJson.Name = "BtnTsStatusJson";
            BtnTsStatusJson.Size = new Size(147, 28);
            BtnTsStatusJson.TabIndex = 1;
            BtnTsStatusJson.Text = "Status (JSON)";
            BtnTsStatusJson.UseVisualStyleBackColor = true;
            BtnTsStatusJson.Click += BtnTsStatusJson_Click;
            // 
            // BtnTsIp
            // 
            BtnTsIp.Location = new Point(12, 86);
            BtnTsIp.Name = "BtnTsIp";
            BtnTsIp.Size = new Size(147, 28);
            BtnTsIp.TabIndex = 2;
            BtnTsIp.Text = "Tailnet IP";
            BtnTsIp.UseVisualStyleBackColor = true;
            BtnTsIp.Click += BtnTsIp_Click;
            // 
            // BtnTsNetcheck
            // 
            BtnTsNetcheck.Location = new Point(12, 118);
            BtnTsNetcheck.Name = "BtnTsNetcheck";
            BtnTsNetcheck.Size = new Size(147, 28);
            BtnTsNetcheck.TabIndex = 3;
            BtnTsNetcheck.Text = "Net Check (slow)";
            BtnTsNetcheck.UseVisualStyleBackColor = true;
            BtnTsNetcheck.Click += BtnTsNetcheck_Click;
            // 
            // BtnTsDns
            // 
            BtnTsDns.Location = new Point(12, 150);
            BtnTsDns.Name = "BtnTsDns";
            BtnTsDns.Size = new Size(147, 28);
            BtnTsDns.TabIndex = 4;
            BtnTsDns.Text = "DNS Status";
            BtnTsDns.UseVisualStyleBackColor = true;
            BtnTsDns.Click += BtnTsDns_Click;
            // 
            // BtnTsPrefs
            // 
            BtnTsPrefs.Location = new Point(12, 182);
            BtnTsPrefs.Name = "BtnTsPrefs";
            BtnTsPrefs.Size = new Size(147, 28);
            BtnTsPrefs.TabIndex = 5;
            BtnTsPrefs.Text = "Preferences (config)";
            BtnTsPrefs.UseVisualStyleBackColor = true;
            BtnTsPrefs.Click += BtnTsPrefs_Click;
            // 
            // BtnTsVersion
            // 
            BtnTsVersion.Location = new Point(12, 214);
            BtnTsVersion.Name = "BtnTsVersion";
            BtnTsVersion.Size = new Size(147, 28);
            BtnTsVersion.TabIndex = 6;
            BtnTsVersion.Text = "Version";
            BtnTsVersion.UseVisualStyleBackColor = true;
            BtnTsVersion.Click += BtnTsVersion_Click;
            // 
            // grpRustDesk
            // 
            grpRustDesk.Controls.Add(BtnRdGetId);
            grpRustDesk.Controls.Add(BtnRdVersion);
            grpRustDesk.Controls.Add(BtnRdServiceStatus);
            grpRustDesk.Controls.Add(BtnRdConfig);
            grpRustDesk.Location = new Point(12, 275);
            grpRustDesk.Name = "grpRustDesk";
            grpRustDesk.Size = new Size(174, 156);
            grpRustDesk.TabIndex = 1;
            grpRustDesk.TabStop = false;
            grpRustDesk.Text = "RustDesk";
            // 
            // BtnRdGetId
            // 
            BtnRdGetId.Location = new Point(12, 22);
            BtnRdGetId.Name = "BtnRdGetId";
            BtnRdGetId.Size = new Size(147, 28);
            BtnRdGetId.TabIndex = 0;
            BtnRdGetId.Text = "Get ID";
            BtnRdGetId.UseVisualStyleBackColor = true;
            BtnRdGetId.Click += BtnRdGetId_Click;
            // 
            // BtnRdVersion
            // 
            BtnRdVersion.Location = new Point(12, 54);
            BtnRdVersion.Name = "BtnRdVersion";
            BtnRdVersion.Size = new Size(147, 28);
            BtnRdVersion.TabIndex = 1;
            BtnRdVersion.Text = "Version";
            BtnRdVersion.UseVisualStyleBackColor = true;
            BtnRdVersion.Click += BtnRdVersion_Click;
            // 
            // BtnRdServiceStatus
            // 
            BtnRdServiceStatus.Location = new Point(12, 86);
            BtnRdServiceStatus.Name = "BtnRdServiceStatus";
            BtnRdServiceStatus.Size = new Size(147, 28);
            BtnRdServiceStatus.TabIndex = 2;
            BtnRdServiceStatus.Text = "Service Status";
            BtnRdServiceStatus.UseVisualStyleBackColor = true;
            BtnRdServiceStatus.Click += BtnRdServiceStatus_Click;
            // 
            // BtnRdConfig
            // 
            BtnRdConfig.Location = new Point(12, 118);
            BtnRdConfig.Name = "BtnRdConfig";
            BtnRdConfig.Size = new Size(147, 28);
            BtnRdConfig.TabIndex = 3;
            BtnRdConfig.Text = "View Config (redacted)";
            BtnRdConfig.UseVisualStyleBackColor = true;
            BtnRdConfig.Click += BtnRdConfig_Click;
            // 
            // grpRdp
            // 
            grpRdp.Controls.Add(BtnRdpConnect);
            grpRdp.Location = new Point(12, 443);
            grpRdp.Name = "grpRdp";
            grpRdp.Size = new Size(174, 64);
            grpRdp.TabIndex = 2;
            grpRdp.TabStop = false;
            grpRdp.Text = "Remote Desktop";
            // 
            // BtnRdpConnect
            // 
            BtnRdpConnect.Location = new Point(12, 22);
            BtnRdpConnect.Name = "BtnRdpConnect";
            BtnRdpConnect.Size = new Size(147, 28);
            BtnRdpConnect.TabIndex = 0;
            BtnRdpConnect.Text = "New Session...";
            BtnRdpConnect.UseVisualStyleBackColor = true;
            BtnRdpConnect.Click += BtnRdpConnect_Click;
            // 
            // Remote_Access_Form
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = BtnClose;
            ClientSize = new Size(900, 584);
            Controls.Add(lblApps);
            Controls.Add(Apps_Panel);
            Controls.Add(lblOutput);
            Controls.Add(txtOutput);
            Controls.Add(BtnClear);
            Controls.Add(BtnCopyAll);
            Controls.Add(BtnSave);
            Controls.Add(BtnClose);
            Font = new Font("Segoe UI", 9F);
            MinimizeBox = false;
            MinimumSize = new Size(860, 594);
            Name = "Remote_Access_Form";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Remote Access Tools";
            Apps_Panel.ResumeLayout(false);
            grpTailscale.ResumeLayout(false);
            grpRustDesk.ResumeLayout(false);
            grpRdp.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblApps;
        private System.Windows.Forms.Label lblOutput;
        private System.Windows.Forms.RichTextBox txtOutput;
        private System.Windows.Forms.Button BtnClear;
        private System.Windows.Forms.Button BtnCopyAll;
        private System.Windows.Forms.Button BtnSave;
        private System.Windows.Forms.Button BtnClose;
        private Panel Apps_Panel;
        private GroupBox grpTailscale;
        private Button BtnTsStatus;
        private Button BtnTsStatusJson;
        private Button BtnTsIp;
        private Button BtnTsNetcheck;
        private Button BtnTsDns;
        private Button BtnTsPrefs;
        private Button BtnTsVersion;
        private GroupBox grpRustDesk;
        private Button BtnRdGetId;
        private Button BtnRdVersion;
        private Button BtnRdServiceStatus;
        private Button BtnRdConfig;
        private GroupBox grpRdp;
        private Button BtnRdpConnect;
    }
}