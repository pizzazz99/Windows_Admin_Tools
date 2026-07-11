namespace Admin_Tools
{
    partial class Storage_Reclaim_Form
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
            this.lvStorage = new System.Windows.Forms.ListView();
            this.colFor = new System.Windows.Forms.ColumnHeader();
            this.colOn = new System.Windows.Forms.ColumnHeader();
            this.colUsed = new System.Windows.Forms.ColumnHeader();
            this.colAllocated = new System.Windows.Forms.ColumnHeader();
            this.colMax = new System.Windows.Forms.ColumnHeader();
            this.grpResize = new System.Windows.Forms.GroupBox();
            this.lblNewMax = new System.Windows.Forms.Label();
            this.numMax = new System.Windows.Forms.NumericUpDown();
            this.cmbUnit = new System.Windows.Forms.ComboBox();
            this.btnApply = new System.Windows.Forms.Button();
            this.lblHint = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.grpResize.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMax)).BeginInit();
            this.SuspendLayout();
            // 
            // lvStorage
            // 
            this.lvStorage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvStorage.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colFor,
            this.colOn,
            this.colUsed,
            this.colAllocated,
            this.colMax});
            this.lvStorage.FullRowSelect = true;
            this.lvStorage.GridLines = true;
            this.lvStorage.HideSelection = false;
            this.lvStorage.Location = new System.Drawing.Point(12, 12);
            this.lvStorage.MultiSelect = false;
            this.lvStorage.Name = "lvStorage";
            this.lvStorage.Size = new System.Drawing.Size(760, 200);
            this.lvStorage.TabIndex = 0;
            this.lvStorage.UseCompatibleStateImageBehavior = false;
            this.lvStorage.View = System.Windows.Forms.View.Details;
            // 
            // columns
            // 
            this.colFor.Text = "Snapshots of";
            this.colFor.Width = 110;
            this.colOn.Text = "Stored on";
            this.colOn.Width = 110;
            this.colUsed.Text = "Used";
            this.colUsed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.colUsed.Width = 110;
            this.colAllocated.Text = "Allocated";
            this.colAllocated.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.colAllocated.Width = 110;
            this.colMax.Text = "Maximum";
            this.colMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.colMax.Width = 130;
            // 
            // grpResize
            // 
            this.grpResize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpResize.Controls.Add(this.lblNewMax);
            this.grpResize.Controls.Add(this.numMax);
            this.grpResize.Controls.Add(this.cmbUnit);
            this.grpResize.Controls.Add(this.btnApply);
            this.grpResize.Controls.Add(this.lblHint);
            this.grpResize.Location = new System.Drawing.Point(12, 222);
            this.grpResize.Name = "grpResize";
            this.grpResize.Size = new System.Drawing.Size(760, 128);
            this.grpResize.TabIndex = 1;
            this.grpResize.TabStop = false;
            this.grpResize.Text = "Resize maximum for the selected row";
            // 
            // lblNewMax
            // 
            this.lblNewMax.AutoSize = true;
            this.lblNewMax.Location = new System.Drawing.Point(12, 30);
            this.lblNewMax.Name = "lblNewMax";
            this.lblNewMax.Text = "New maximum:";
            // 
            // numMax
            // 
            this.numMax.Location = new System.Drawing.Point(110, 27);
            this.numMax.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            this.numMax.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numMax.Name = "numMax";
            this.numMax.Size = new System.Drawing.Size(80, 23);
            this.numMax.TabIndex = 1;
            this.numMax.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // cmbUnit
            // 
            this.cmbUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUnit.Items.AddRange(new object[] { "GB", "%", "Unbounded" });
            this.cmbUnit.Location = new System.Drawing.Point(196, 27);
            this.cmbUnit.Name = "cmbUnit";
            this.cmbUnit.Size = new System.Drawing.Size(100, 23);
            this.cmbUnit.TabIndex = 2;
            this.cmbUnit.SelectedIndexChanged += new System.EventHandler(this.Cmb_Unit_Changed);
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(312, 25);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(120, 28);
            this.btnApply.TabIndex = 3;
            this.btnApply.Text = "Apply Resize";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.Btn_Apply_Click);
            // 
            // lblHint
            // 
            this.lblHint.ForeColor = System.Drawing.Color.FromArgb(176, 0, 32);
            this.lblHint.Location = new System.Drawing.Point(12, 62);
            this.lblHint.Name = "lblHint";
            this.lblHint.Size = new System.Drawing.Size(736, 56);
            this.lblHint.Text = "Shrinking the maximum below the currently USED amount makes Windows " +
                "delete the OLDEST snapshots immediately to fit, and releases the freed " +
                "allocation back to the drive. This is the supported way to reclaim disk " +
                "space from shadow storage. There is no undo for the deleted snapshots.";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Location = new System.Drawing.Point(12, 360);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 28);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.Btn_Refresh_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(682, 360);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 28);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.Btn_Close_Click);
            // 
            // Storage_Reclaim_Form
            // 
            this.AcceptButton = this.btnClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 400);
            this.Controls.Add(this.lvStorage);
            this.Controls.Add(this.grpResize);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnClose);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(800, 439);
            this.Name = "Storage_Reclaim_Form";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Shadow Storage - Reclaim Space";
            this.grpResize.ResumeLayout(false);
            this.grpResize.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMax)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ListView lvStorage;
        private System.Windows.Forms.ColumnHeader colFor;
        private System.Windows.Forms.ColumnHeader colOn;
        private System.Windows.Forms.ColumnHeader colUsed;
        private System.Windows.Forms.ColumnHeader colAllocated;
        private System.Windows.Forms.ColumnHeader colMax;
        private System.Windows.Forms.GroupBox grpResize;
        private System.Windows.Forms.Label lblNewMax;
        private System.Windows.Forms.NumericUpDown numMax;
        private System.Windows.Forms.ComboBox cmbUnit;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Label lblHint;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnClose;
    }
}
