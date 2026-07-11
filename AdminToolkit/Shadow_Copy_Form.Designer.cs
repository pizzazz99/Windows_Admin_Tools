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
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnCreate = new System.Windows.Forms.Button();
            this.lblDrive = new System.Windows.Forms.Label();
            this.cmbDrive = new System.Windows.Forms.ComboBox();
            this.btnDetails = new System.Windows.Forms.Button();
            this.btnVssAdmin = new System.Windows.Forms.Button();
            this.btnDeleteSelected = new System.Windows.Forms.Button();
            this.btnDeleteOlder = new System.Windows.Forms.Button();
            this.btnKeepNewest = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
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
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Location = new System.Drawing.Point(12, 476);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 28);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.Btn_Refresh_Click);
            // 
            // btnCreate
            // 
            this.btnCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCreate.Location = new System.Drawing.Point(118, 476);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(160, 28);
            this.btnCreate.TabIndex = 3;
            this.btnCreate.Text = "Create Snapshot Now";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.Btn_Create_Click);
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
            // btnDetails
            // 
            this.btnDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDetails.Location = new System.Drawing.Point(425, 476);
            this.btnDetails.Name = "btnDetails";
            this.btnDetails.Size = new System.Drawing.Size(130, 28);
            this.btnDetails.TabIndex = 5;
            this.btnDetails.Text = "Snapshot Details";
            this.btnDetails.UseVisualStyleBackColor = true;
            this.btnDetails.Click += new System.EventHandler(this.Btn_Details_Click);
            // 
            // btnVssAdmin
            // 
            this.btnVssAdmin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnVssAdmin.Location = new System.Drawing.Point(561, 476);
            this.btnVssAdmin.Name = "btnVssAdmin";
            this.btnVssAdmin.Size = new System.Drawing.Size(160, 28);
            this.btnVssAdmin.TabIndex = 6;
            this.btnVssAdmin.Text = "VSS Details (vssadmin)";
            this.btnVssAdmin.UseVisualStyleBackColor = true;
            this.btnVssAdmin.Click += new System.EventHandler(this.Btn_VssAdmin_Click);
            // 
            // btnDeleteSelected
            // 
            this.btnDeleteSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeleteSelected.Location = new System.Drawing.Point(12, 514);
            this.btnDeleteSelected.Name = "btnDeleteSelected";
            this.btnDeleteSelected.Size = new System.Drawing.Size(130, 28);
            this.btnDeleteSelected.TabIndex = 7;
            this.btnDeleteSelected.Text = "Delete Selected";
            this.btnDeleteSelected.UseVisualStyleBackColor = true;
            this.btnDeleteSelected.Click += new System.EventHandler(this.Btn_Delete_Selected_Click);
            // 
            // btnDeleteOlder
            // 
            this.btnDeleteOlder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeleteOlder.Location = new System.Drawing.Point(148, 514);
            this.btnDeleteOlder.Name = "btnDeleteOlder";
            this.btnDeleteOlder.Size = new System.Drawing.Size(150, 28);
            this.btnDeleteOlder.TabIndex = 8;
            this.btnDeleteOlder.Text = "Delete Older Than...";
            this.btnDeleteOlder.UseVisualStyleBackColor = true;
            this.btnDeleteOlder.Click += new System.EventHandler(this.Btn_Delete_Older_Click);
            // 
            // btnKeepNewest
            // 
            this.btnKeepNewest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnKeepNewest.Location = new System.Drawing.Point(304, 514);
            this.btnKeepNewest.Name = "btnKeepNewest";
            this.btnKeepNewest.Size = new System.Drawing.Size(130, 28);
            this.btnKeepNewest.TabIndex = 9;
            this.btnKeepNewest.Text = "Keep Newest...";
            this.btnKeepNewest.UseVisualStyleBackColor = true;
            this.btnKeepNewest.Click += new System.EventHandler(this.Btn_Keep_Newest_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(882, 514);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 28);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.Btn_Close_Click);
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
            this.AcceptButton = this.btnClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 554);
            this.Controls.Add(this.Reclaim_Space_Button);
            this.Controls.Add(this.lvSnapshots);
            this.Controls.Add(this.lblSummary);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.lblDrive);
            this.Controls.Add(this.cmbDrive);
            this.Controls.Add(this.btnDetails);
            this.Controls.Add(this.btnVssAdmin);
            this.Controls.Add(this.btnDeleteSelected);
            this.Controls.Add(this.btnDeleteOlder);
            this.Controls.Add(this.btnKeepNewest);
            this.Controls.Add(this.btnClose);
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
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Label lblDrive;
        private System.Windows.Forms.ComboBox cmbDrive;
        private System.Windows.Forms.Button btnDetails;
        private System.Windows.Forms.Button btnVssAdmin;
        private System.Windows.Forms.Button btnDeleteSelected;
        private System.Windows.Forms.Button btnDeleteOlder;
        private System.Windows.Forms.Button btnKeepNewest;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button Reclaim_Space_Button;
    }
}
