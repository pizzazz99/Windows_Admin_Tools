namespace Admin_Tools
{
    partial class Shadow_Copy_Form
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
            this.lvSnapshots = new System.Windows.Forms.ListView();
            this.colCreated = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDrive = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAge = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colShadowId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblSummary = new System.Windows.Forms.Label();
            this.BtnRefresh = new System.Windows.Forms.Button();
            this.BtnCreate = new System.Windows.Forms.Button();
            this.lblDrive = new System.Windows.Forms.Label();
            this.cmbDrive = new System.Windows.Forms.ComboBox();
            this.BtnDetails = new System.Windows.Forms.Button();
            this.BtnVssAdmin = new System.Windows.Forms.Button();
            this.BtnDeleteSelected = new System.Windows.Forms.Button();
            this.BtnDeleteOlder = new System.Windows.Forms.Button();
            this.BtnKeepNewest = new System.Windows.Forms.Button();
            this.BtnClose = new System.Windows.Forms.Button();
            this.Reclaim_Space_Button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lvSnapshots
            // 
            this.lvSnapshots.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvSnapshots.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colCreated,
            this.colDrive,
            this.colAge,
            this.colType,
            this.colShadowId});
            this.lvSnapshots.FullRowSelect = true;
            this.lvSnapshots.GridLines = true;
            this.lvSnapshots.HideSelection = false;
            this.lvSnapshots.Location = new System.Drawing.Point(12, 12);
            this.lvSnapshots.Name = "lvSnapshots";
            this.lvSnapshots.Size = new System.Drawing.Size(960, 428);
            this.lvSnapshots.TabIndex = 0;
            this.lvSnapshots.UseCompatibleStateImageBehavior = false;
            this.lvSnapshots.View = System.Windows.Forms.View.Details;
            this.lvSnapshots.DoubleClick += new System.EventHandler(this.Btn_Details_Click);
            // 
            // colCreated
            // 
            this.colCreated.Text = "Created";
            this.colCreated.Width = 145;
            // 
            // colDrive
            // 
            this.colDrive.Text = "Drive";
            // 
            // colAge
            // 
            this.colAge.Text = "Age (days)";
            this.colAge.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.colAge.Width = 80;
            // 
            // colType
            // 
            this.colType.Text = "Type";
            this.colType.Width = 190;
            // 
            // colShadowId
            // 
            this.colShadowId.Text = "Shadow ID";
            this.colShadowId.Width = 300;
            // 
            // lblSummary
            // 
            this.lblSummary.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSummary.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblSummary.Location = new System.Drawing.Point(12, 448);
            this.lblSummary.Name = "lblSummary";
            this.lblSummary.Size = new System.Drawing.Size(960, 20);
            this.lblSummary.TabIndex = 1;
            this.lblSummary.Text = "Loading...";
            // 
            // BtnRefresh
            // 
            this.BtnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BtnRefresh.Location = new System.Drawing.Point(12, 476);
            this.BtnRefresh.Name = "BtnRefresh";
            this.BtnRefresh.Size = new System.Drawing.Size(100, 28);
            this.BtnRefresh.TabIndex = 2;
            this.BtnRefresh.Text = "Refresh";
            this.BtnRefresh.UseVisualStyleBackColor = true;
            this.BtnRefresh.Click += new System.EventHandler(this.Btn_Refresh_Click);
            // 
            // BtnCreate
            // 
            this.BtnCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BtnCreate.Location = new System.Drawing.Point(118, 476);
            this.BtnCreate.Name = "BtnCreate";
            this.BtnCreate.Size = new System.Drawing.Size(160, 28);
            this.BtnCreate.TabIndex = 3;
            this.BtnCreate.Text = "Create Snapshot Now";
            this.BtnCreate.UseVisualStyleBackColor = true;
            this.BtnCreate.Click += new System.EventHandler(this.Btn_Create_Click);
            // 
            // lblDrive
            // 
            this.lblDrive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblDrive.AutoSize = true;
            this.lblDrive.Location = new System.Drawing.Point(288, 482);
            this.lblDrive.Name = "lblDrive";
            this.lblDrive.Size = new System.Drawing.Size(50, 15);
            this.lblDrive.TabIndex = 4;
            this.lblDrive.Text = "of drive:";
            // 
            // cmbDrive
            // 
            this.cmbDrive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmbDrive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDrive.Location = new System.Drawing.Point(345, 478);
            this.cmbDrive.Name = "cmbDrive";
            this.cmbDrive.Size = new System.Drawing.Size(60, 23);
            this.cmbDrive.TabIndex = 4;
            // 
            // BtnDetails
            // 
            this.BtnDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BtnDetails.Location = new System.Drawing.Point(425, 476);
            this.BtnDetails.Name = "BtnDetails";
            this.BtnDetails.Size = new System.Drawing.Size(130, 28);
            this.BtnDetails.TabIndex = 5;
            this.BtnDetails.Text = "Snapshot Details";
            this.BtnDetails.UseVisualStyleBackColor = true;
            this.BtnDetails.Click += new System.EventHandler(this.Btn_Details_Click);
            // 
            // BtnVssAdmin
            // 
            this.BtnVssAdmin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BtnVssAdmin.Location = new System.Drawing.Point(561, 476);
            this.BtnVssAdmin.Name = "BtnVssAdmin";
            this.BtnVssAdmin.Size = new System.Drawing.Size(160, 28);
            this.BtnVssAdmin.TabIndex = 6;
            this.BtnVssAdmin.Text = "VSS Details (vssadmin)";
            this.BtnVssAdmin.UseVisualStyleBackColor = true;
            this.BtnVssAdmin.Click += new System.EventHandler(this.Btn_VssAdmin_Click);
            // 
            // BtnDeleteSelected
            // 
            this.BtnDeleteSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BtnDeleteSelected.Location = new System.Drawing.Point(12, 514);
            this.BtnDeleteSelected.Name = "BtnDeleteSelected";
            this.BtnDeleteSelected.Size = new System.Drawing.Size(130, 28);
            this.BtnDeleteSelected.TabIndex = 7;
            this.BtnDeleteSelected.Text = "Delete Selected";
            this.BtnDeleteSelected.UseVisualStyleBackColor = true;
            this.BtnDeleteSelected.Click += new System.EventHandler(this.Btn_Delete_Selected_Click);
            // 
            // BtnDeleteOlder
            // 
            this.BtnDeleteOlder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BtnDeleteOlder.Location = new System.Drawing.Point(148, 514);
            this.BtnDeleteOlder.Name = "BtnDeleteOlder";
            this.BtnDeleteOlder.Size = new System.Drawing.Size(150, 28);
            this.BtnDeleteOlder.TabIndex = 8;
            this.BtnDeleteOlder.Text = "Delete Older Than...";
            this.BtnDeleteOlder.UseVisualStyleBackColor = true;
            this.BtnDeleteOlder.Click += new System.EventHandler(this.Btn_Delete_Older_Click);
            // 
            // BtnKeepNewest
            // 
            this.BtnKeepNewest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BtnKeepNewest.Location = new System.Drawing.Point(304, 514);
            this.BtnKeepNewest.Name = "BtnKeepNewest";
            this.BtnKeepNewest.Size = new System.Drawing.Size(130, 28);
            this.BtnKeepNewest.TabIndex = 9;
            this.BtnKeepNewest.Text = "Keep Newest...";
            this.BtnKeepNewest.UseVisualStyleBackColor = true;
            this.BtnKeepNewest.Click += new System.EventHandler(this.Btn_Keep_Newest_Click);
            // 
            // BtnClose
            // 
            this.BtnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnClose.Location = new System.Drawing.Point(882, 514);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(90, 28);
            this.BtnClose.TabIndex = 10;
            this.BtnClose.Text = "Close";
            this.BtnClose.UseVisualStyleBackColor = true;
            this.BtnClose.Click += new System.EventHandler(this.Btn_Close_Click);
            // 
            // Reclaim_Space_Button
            // 
            this.Reclaim_Space_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Reclaim_Space_Button.Location = new System.Drawing.Point(561, 510);
            this.Reclaim_Space_Button.Name = "Reclaim_Space_Button";
            this.Reclaim_Space_Button.Size = new System.Drawing.Size(160, 28);
            this.Reclaim_Space_Button.TabIndex = 11;
            this.Reclaim_Space_Button.Text = "Reclaim Space";
            this.Reclaim_Space_Button.UseVisualStyleBackColor = true;
            this.Reclaim_Space_Button.Click += new System.EventHandler(this.Reclaim_Space_Button_Click);
            // 
            // Shadow_Copy_Form
            // 
            this.AcceptButton = this.BtnClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 554);
            this.Controls.Add(this.Reclaim_Space_Button);
            this.Controls.Add(this.lvSnapshots);
            this.Controls.Add(this.lblSummary);
            this.Controls.Add(this.BtnRefresh);
            this.Controls.Add(this.BtnCreate);
            this.Controls.Add(this.lblDrive);
            this.Controls.Add(this.cmbDrive);
            this.Controls.Add(this.BtnDetails);
            this.Controls.Add(this.BtnVssAdmin);
            this.Controls.Add(this.BtnDeleteSelected);
            this.Controls.Add(this.BtnDeleteOlder);
            this.Controls.Add(this.BtnKeepNewest);
            this.Controls.Add(this.BtnClose);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1000, 593);
            this.Name = "Shadow_Copy_Form";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Volume Shadow Copies";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvSnapshots;
        private System.Windows.Forms.ColumnHeader colCreated;
        private System.Windows.Forms.ColumnHeader colDrive;
        private System.Windows.Forms.ColumnHeader colAge;
        private System.Windows.Forms.ColumnHeader colType;
        private System.Windows.Forms.ColumnHeader colShadowId;
        private System.Windows.Forms.Label lblSummary;
        private System.Windows.Forms.Button BtnRefresh;
        private System.Windows.Forms.Button BtnCreate;
        private System.Windows.Forms.Label lblDrive;
        private System.Windows.Forms.ComboBox cmbDrive;
        private System.Windows.Forms.Button BtnDetails;
        private System.Windows.Forms.Button BtnVssAdmin;
        private System.Windows.Forms.Button BtnDeleteSelected;
        private System.Windows.Forms.Button BtnDeleteOlder;
        private System.Windows.Forms.Button BtnKeepNewest;
        private System.Windows.Forms.Button BtnClose;
        private System.Windows.Forms.Button Reclaim_Space_Button;
    }
}
