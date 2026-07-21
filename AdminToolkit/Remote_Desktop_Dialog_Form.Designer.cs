namespace Admin_Tools
{
    partial class Remote_Desktop_Dialog
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            lblComputer = new Label();
            cboTarget = new ComboBox();
            lblHint = new Label();
            Scan_Button = new Button();
            Test_Button = new Button();
            Connect_Button = new Button();
            Cancel_Button = new Button();
            Quit_Button = new Button();
            SuspendLayout();
            // 
            // lblComputer
            // 
            lblComputer.AutoSize = true;
            lblComputer.Location = new Point(16, 18);
            lblComputer.Name = "lblComputer";
            lblComputer.Size = new Size(124, 15);
            lblComputer.TabIndex = 0;
            lblComputer.Text = "Computer name or IP:";
            // 
            // cboTarget
            // 
            cboTarget.DropDownStyle = ComboBoxStyle.DropDown;
            cboTarget.FormattingEnabled = true;
            cboTarget.Location = new Point(16, 42);
            cboTarget.Name = "cboTarget";
            cboTarget.Size = new Size(300, 23);
            cboTarget.TabIndex = 1;
            // 
            // lblHint
            // 
            lblHint.AutoSize = true;
            lblHint.ForeColor = SystemColors.GrayText;
            lblHint.Location = new Point(16, 74);
            lblHint.MaximumSize = new Size(400, 0);
            lblHint.Name = "lblHint";
            lblHint.Size = new Size(304, 15);
            lblHint.TabIndex = 2;
            lblHint.Text = "Pick a discovered PC, or type any hostname / IP address.";
            // 
            // Scan_Button
            // 
            Scan_Button.Location = new Point(328, 40);
            Scan_Button.Name = "Scan_Button";
            Scan_Button.Size = new Size(90, 28);
            Scan_Button.TabIndex = 3;
            Scan_Button.Text = "Scan";
            Scan_Button.UseVisualStyleBackColor = true;
            Scan_Button.Click += Scan_Button_Click;
            // 
            // Test_Button
            // 
            Test_Button.Location = new Point(118, 118);
            Test_Button.Name = "Test_Button";
            Test_Button.Size = new Size(90, 28);
            Test_Button.TabIndex = 5;
            Test_Button.Text = "Test";
            Test_Button.UseVisualStyleBackColor = true;
            Test_Button.Click += Test_Button_Click;
            // 
            // Connect_Button
            // 
            Connect_Button.Location = new Point(220, 118);
            Connect_Button.Name = "Connect_Button";
            Connect_Button.Size = new Size(90, 28);
            Connect_Button.TabIndex = 6;
            Connect_Button.Text = "Connect";
            Connect_Button.UseVisualStyleBackColor = true;
            Connect_Button.Click += Connect_Button_Click;
            // 
            // Cancel_Button
            // 
            Cancel_Button.DialogResult = DialogResult.Cancel;
            Cancel_Button.Location = new Point(322, 118);
            Cancel_Button.Name = "Cancel_Button";
            Cancel_Button.Size = new Size(90, 28);
            Cancel_Button.TabIndex = 7;
            Cancel_Button.Text = "Cancel";
            Cancel_Button.UseVisualStyleBackColor = true;
            // 
            // Quit_Button
            // 
            Quit_Button.Location = new Point(16, 118);
            Quit_Button.Name = "Quit_Button";
            Quit_Button.Size = new Size(90, 28);
            Quit_Button.TabIndex = 4;
            Quit_Button.Text = "Quit";
            Quit_Button.UseVisualStyleBackColor = true;
            Quit_Button.Click += Quit_Button_Click;
            // 
            // Remote_Desktop_Dialog
            // 
            AcceptButton = Connect_Button;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = Cancel_Button;
            ClientSize = new Size(430, 165);
            Controls.Add(Quit_Button);
            Controls.Add(Cancel_Button);
            Controls.Add(Connect_Button);
            Controls.Add(Test_Button);
            Controls.Add(Scan_Button);
            Controls.Add(lblComputer);
            Controls.Add(cboTarget);
            Controls.Add(lblHint);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Remote_Desktop_Dialog";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Remote Desktop";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblComputer;
        private System.Windows.Forms.ComboBox cboTarget;
        private System.Windows.Forms.Label lblHint;
        private Button Scan_Button;
        private Button Test_Button;
        private Button Connect_Button;
        private Button Cancel_Button;
        private Button Quit_Button;
    }
}