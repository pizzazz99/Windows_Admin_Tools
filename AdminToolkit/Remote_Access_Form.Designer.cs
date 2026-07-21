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
            btnClear = new Button();
            btnCopyAll = new Button();
            btnSave = new Button();
            btnClose = new Button();
            Apps_Panel = new Panel();
            grpTailscale = new GroupBox();
            btnTsStatus = new Button();
            btnTsStatusJson = new Button();
            btnTsIp = new Button();
            btnTsNetcheck = new Button();
            btnTsDns = new Button();
            btnTsPrefs = new Button();
            btnTsVersion = new Button();
            grpRustDesk = new GroupBox();
            btnRdGetId = new Button();
            btnRdVersion = new Button();
            btnRdServiceStatus = new Button();
            btnRdConfig = new Button();
            grpRdp = new GroupBox();
            btnRdpConnect = new Button();
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
            // btnClear
            // 
            btnClear.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnClear.Location = new Point(494, 545);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(90, 28);
            btnClear.TabIndex = 3;
            btnClear.Text = "Clear";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += Btn_Clear_Click;
            // 
            // btnCopyAll
            // 
            btnCopyAll.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCopyAll.Location = new Point(590, 545);
            btnCopyAll.Name = "btnCopyAll";
            btnCopyAll.Size = new Size(90, 28);
            btnCopyAll.TabIndex = 4;
            btnCopyAll.Text = "Copy All";
            btnCopyAll.UseVisualStyleBackColor = true;
            btnCopyAll.Click += Btn_Copy_All_Click;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSave.Location = new Point(686, 545);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(90, 28);
            btnSave.TabIndex = 5;
            btnSave.Text = "Save...";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += Btn_Save_Click;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnClose.Location = new Point(798, 545);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(90, 28);
            btnClose.TabIndex = 6;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += Btn_Close_Click;
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
            grpTailscale.Controls.Add(btnTsStatus);
            grpTailscale.Controls.Add(btnTsStatusJson);
            grpTailscale.Controls.Add(btnTsIp);
            grpTailscale.Controls.Add(btnTsNetcheck);
            grpTailscale.Controls.Add(btnTsDns);
            grpTailscale.Controls.Add(btnTsPrefs);
            grpTailscale.Controls.Add(btnTsVersion);
            grpTailscale.Location = new Point(12, 7);
            grpTailscale.Name = "grpTailscale";
            grpTailscale.Size = new Size(174, 256);
            grpTailscale.TabIndex = 0;
            grpTailscale.TabStop = false;
            grpTailscale.Text = "Tailscale";
            // 
            // btnTsStatus
            // 
            btnTsStatus.Location = new Point(12, 22);
            btnTsStatus.Name = "btnTsStatus";
            btnTsStatus.Size = new Size(147, 28);
            btnTsStatus.TabIndex = 0;
            btnTsStatus.Text = "Status";
            btnTsStatus.UseVisualStyleBackColor = true;
            btnTsStatus.Click += btnTsStatus_Click;
            // 
            // btnTsStatusJson
            // 
            btnTsStatusJson.Location = new Point(12, 54);
            btnTsStatusJson.Name = "btnTsStatusJson";
            btnTsStatusJson.Size = new Size(147, 28);
            btnTsStatusJson.TabIndex = 1;
            btnTsStatusJson.Text = "Status (JSON)";
            btnTsStatusJson.UseVisualStyleBackColor = true;
            btnTsStatusJson.Click += btnTsStatusJson_Click;
            // 
            // btnTsIp
            // 
            btnTsIp.Location = new Point(12, 86);
            btnTsIp.Name = "btnTsIp";
            btnTsIp.Size = new Size(147, 28);
            btnTsIp.TabIndex = 2;
            btnTsIp.Text = "Tailnet IP";
            btnTsIp.UseVisualStyleBackColor = true;
            btnTsIp.Click += btnTsIp_Click;
            // 
            // btnTsNetcheck
            // 
            btnTsNetcheck.Location = new Point(12, 118);
            btnTsNetcheck.Name = "btnTsNetcheck";
            btnTsNetcheck.Size = new Size(147, 28);
            btnTsNetcheck.TabIndex = 3;
            btnTsNetcheck.Text = "Net Check (slow)";
            btnTsNetcheck.UseVisualStyleBackColor = true;
            btnTsNetcheck.Click += btnTsNetcheck_Click;
            // 
            // btnTsDns
            // 
            btnTsDns.Location = new Point(12, 150);
            btnTsDns.Name = "btnTsDns";
            btnTsDns.Size = new Size(147, 28);
            btnTsDns.TabIndex = 4;
            btnTsDns.Text = "DNS Status";
            btnTsDns.UseVisualStyleBackColor = true;
            btnTsDns.Click += btnTsDns_Click;
            // 
            // btnTsPrefs
            // 
            btnTsPrefs.Location = new Point(12, 182);
            btnTsPrefs.Name = "btnTsPrefs";
            btnTsPrefs.Size = new Size(147, 28);
            btnTsPrefs.TabIndex = 5;
            btnTsPrefs.Text = "Preferences (config)";
            btnTsPrefs.UseVisualStyleBackColor = true;
            btnTsPrefs.Click += btnTsPrefs_Click;
            // 
            // btnTsVersion
            // 
            btnTsVersion.Location = new Point(12, 214);
            btnTsVersion.Name = "btnTsVersion";
            btnTsVersion.Size = new Size(147, 28);
            btnTsVersion.TabIndex = 6;
            btnTsVersion.Text = "Version";
            btnTsVersion.UseVisualStyleBackColor = true;
            btnTsVersion.Click += btnTsVersion_Click;
            // 
            // grpRustDesk
            // 
            grpRustDesk.Controls.Add(btnRdGetId);
            grpRustDesk.Controls.Add(btnRdVersion);
            grpRustDesk.Controls.Add(btnRdServiceStatus);
            grpRustDesk.Controls.Add(btnRdConfig);
            grpRustDesk.Location = new Point(12, 275);
            grpRustDesk.Name = "grpRustDesk";
            grpRustDesk.Size = new Size(174, 156);
            grpRustDesk.TabIndex = 1;
            grpRustDesk.TabStop = false;
            grpRustDesk.Text = "RustDesk";
            // 
            // btnRdGetId
            // 
            btnRdGetId.Location = new Point(12, 22);
            btnRdGetId.Name = "btnRdGetId";
            btnRdGetId.Size = new Size(147, 28);
            btnRdGetId.TabIndex = 0;
            btnRdGetId.Text = "Get ID";
            btnRdGetId.UseVisualStyleBackColor = true;
            btnRdGetId.Click += btnRdGetId_Click;
            // 
            // btnRdVersion
            // 
            btnRdVersion.Location = new Point(12, 54);
            btnRdVersion.Name = "btnRdVersion";
            btnRdVersion.Size = new Size(147, 28);
            btnRdVersion.TabIndex = 1;
            btnRdVersion.Text = "Version";
            btnRdVersion.UseVisualStyleBackColor = true;
            btnRdVersion.Click += btnRdVersion_Click;
            // 
            // btnRdServiceStatus
            // 
            btnRdServiceStatus.Location = new Point(12, 86);
            btnRdServiceStatus.Name = "btnRdServiceStatus";
            btnRdServiceStatus.Size = new Size(147, 28);
            btnRdServiceStatus.TabIndex = 2;
            btnRdServiceStatus.Text = "Service Status";
            btnRdServiceStatus.UseVisualStyleBackColor = true;
            btnRdServiceStatus.Click += btnRdServiceStatus_Click;
            // 
            // btnRdConfig
            // 
            btnRdConfig.Location = new Point(12, 118);
            btnRdConfig.Name = "btnRdConfig";
            btnRdConfig.Size = new Size(147, 28);
            btnRdConfig.TabIndex = 3;
            btnRdConfig.Text = "View Config (redacted)";
            btnRdConfig.UseVisualStyleBackColor = true;
            btnRdConfig.Click += btnRdConfig_Click;
            // 
            // grpRdp
            // 
            grpRdp.Controls.Add(btnRdpConnect);
            grpRdp.Location = new Point(12, 443);
            grpRdp.Name = "grpRdp";
            grpRdp.Size = new Size(174, 64);
            grpRdp.TabIndex = 2;
            grpRdp.TabStop = false;
            grpRdp.Text = "Remote Desktop";
            // 
            // btnRdpConnect
            // 
            btnRdpConnect.Location = new Point(12, 22);
            btnRdpConnect.Name = "btnRdpConnect";
            btnRdpConnect.Size = new Size(147, 28);
            btnRdpConnect.TabIndex = 0;
            btnRdpConnect.Text = "New Session...";
            btnRdpConnect.UseVisualStyleBackColor = true;
            btnRdpConnect.Click += btnRdpConnect_Click;
            // 
            // Remote_Access_Form
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnClose;
            ClientSize = new Size(900, 584);
            Controls.Add(lblApps);
            Controls.Add(Apps_Panel);
            Controls.Add(lblOutput);
            Controls.Add(txtOutput);
            Controls.Add(btnClear);
            Controls.Add(btnCopyAll);
            Controls.Add(btnSave);
            Controls.Add(btnClose);
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
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnCopyAll;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private Panel Apps_Panel;
        private GroupBox grpTailscale;
        private Button btnTsStatus;
        private Button btnTsStatusJson;
        private Button btnTsIp;
        private Button btnTsNetcheck;
        private Button btnTsDns;
        private Button btnTsPrefs;
        private Button btnTsVersion;
        private GroupBox grpRustDesk;
        private Button btnRdGetId;
        private Button btnRdVersion;
        private Button btnRdServiceStatus;
        private Button btnRdConfig;
        private GroupBox grpRdp;
        private Button btnRdpConnect;
    }
}