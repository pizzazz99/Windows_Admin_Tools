namespace Admin_Tools
{
    partial class Restore_Point_List_Form
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
            this.lblSummary = new System.Windows.Forms.Label();
            this.lvPoints = new System.Windows.Forms.ListView();
            this.colSeq = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colCreated = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAge = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colEvent = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDescription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colShadow = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.grpRestorePoint = new System.Windows.Forms.GroupBox();
            this.lblSeq = new System.Windows.Forms.Label();
            this.txtSeq = new System.Windows.Forms.TextBox();
            this.lblCreated = new System.Windows.Forms.Label();
            this.txtCreated = new System.Windows.Forms.TextBox();
            this.lblAge = new System.Windows.Forms.Label();
            this.txtAge = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.grpAttribution = new System.Windows.Forms.GroupBox();
            this.lblType = new System.Windows.Forms.Label();
            this.txtType = new System.Windows.Forms.TextBox();
            this.lblEvent = new System.Windows.Forms.Label();
            this.txtEvent = new System.Windows.Forms.TextBox();
            this.grpShadow = new System.Windows.Forms.GroupBox();
            this.lblShadowId = new System.Windows.Forms.Label();
            this.txtShadowId = new System.Windows.Forms.TextBox();
            this.lblDevice = new System.Windows.Forms.Label();
            this.txtDevice = new System.Windows.Forms.TextBox();
            this.grpNotes = new System.Windows.Forms.GroupBox();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.Delete_Button = new System.Windows.Forms.Button();
            this.grpRestorePoint.SuspendLayout();
            this.grpAttribution.SuspendLayout();
            this.grpShadow.SuspendLayout();
            this.grpNotes.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblSummary
            // 
            this.lblSummary.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSummary.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblSummary.Location = new System.Drawing.Point(12, 9);
            this.lblSummary.Name = "lblSummary";
            this.lblSummary.Size = new System.Drawing.Size(960, 20);
            this.lblSummary.TabIndex = 0;
            this.lblSummary.Text = "Loading...";
            // 
            // lvPoints
            // 
            this.lvPoints.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvPoints.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colSeq,
            this.colCreated,
            this.colAge,
            this.colType,
            this.colEvent,
            this.colDescription,
            this.colShadow});
            this.lvPoints.FullRowSelect = true;
            this.lvPoints.GridLines = true;
            this.lvPoints.HideSelection = false;
            this.lvPoints.Location = new System.Drawing.Point(12, 32);
            this.lvPoints.MultiSelect = false;
            this.lvPoints.Name = "lvPoints";
            this.lvPoints.Size = new System.Drawing.Size(960, 223);
            this.lvPoints.TabIndex = 1;
            this.lvPoints.UseCompatibleStateImageBehavior = false;
            this.lvPoints.View = System.Windows.Forms.View.Details;
            this.lvPoints.SelectedIndexChanged += new System.EventHandler(this.Lv_Selection_Changed);
            // 
            // colSeq
            // 
            this.colSeq.Text = "Seq #";
            this.colSeq.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // colCreated
            // 
            this.colCreated.Text = "Created";
            this.colCreated.Width = 140;
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
            this.colType.Width = 200;
            // 
            // colEvent
            // 
            this.colEvent.Text = "Event";
            this.colEvent.Width = 170;
            // 
            // colDescription
            // 
            this.colDescription.Text = "Description";
            this.colDescription.Width = 230;
            // 
            // colShadow
            // 
            this.colShadow.Text = "Shadow Copy";
            this.colShadow.Width = 90;
            // 
            // grpRestorePoint
            // 
            this.grpRestorePoint.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpRestorePoint.Controls.Add(this.lblSeq);
            this.grpRestorePoint.Controls.Add(this.txtSeq);
            this.grpRestorePoint.Controls.Add(this.lblCreated);
            this.grpRestorePoint.Controls.Add(this.txtCreated);
            this.grpRestorePoint.Controls.Add(this.lblAge);
            this.grpRestorePoint.Controls.Add(this.txtAge);
            this.grpRestorePoint.Controls.Add(this.lblDescription);
            this.grpRestorePoint.Controls.Add(this.txtDescription);
            this.grpRestorePoint.Location = new System.Drawing.Point(12, 278);
            this.grpRestorePoint.Name = "grpRestorePoint";
            this.grpRestorePoint.Size = new System.Drawing.Size(960, 96);
            this.grpRestorePoint.TabIndex = 2;
            this.grpRestorePoint.TabStop = false;
            this.grpRestorePoint.Text = "Restore Point";
            // 
            // lblSeq
            // 
            this.lblSeq.AutoSize = true;
            this.lblSeq.Location = new System.Drawing.Point(12, 28);
            this.lblSeq.Name = "lblSeq";
            this.lblSeq.Size = new System.Drawing.Size(71, 15);
            this.lblSeq.TabIndex = 0;
            this.lblSeq.Text = "Sequence #:";
            // 
            // txtSeq
            // 
            this.txtSeq.BackColor = System.Drawing.Color.White;
            this.txtSeq.Location = new System.Drawing.Point(95, 25);
            this.txtSeq.Name = "txtSeq";
            this.txtSeq.ReadOnly = true;
            this.txtSeq.Size = new System.Drawing.Size(80, 23);
            this.txtSeq.TabIndex = 1;
            this.txtSeq.TabStop = false;
            // 
            // lblCreated
            // 
            this.lblCreated.AutoSize = true;
            this.lblCreated.Location = new System.Drawing.Point(210, 28);
            this.lblCreated.Name = "lblCreated";
            this.lblCreated.Size = new System.Drawing.Size(51, 15);
            this.lblCreated.TabIndex = 2;
            this.lblCreated.Text = "Created:";
            // 
            // txtCreated
            // 
            this.txtCreated.BackColor = System.Drawing.Color.White;
            this.txtCreated.Location = new System.Drawing.Point(270, 25);
            this.txtCreated.Name = "txtCreated";
            this.txtCreated.ReadOnly = true;
            this.txtCreated.Size = new System.Drawing.Size(150, 23);
            this.txtCreated.TabIndex = 3;
            this.txtCreated.TabStop = false;
            // 
            // lblAge
            // 
            this.lblAge.AutoSize = true;
            this.lblAge.Location = new System.Drawing.Point(450, 28);
            this.lblAge.Name = "lblAge";
            this.lblAge.Size = new System.Drawing.Size(66, 15);
            this.lblAge.TabIndex = 4;
            this.lblAge.Text = "Age (days):";
            // 
            // txtAge
            // 
            this.txtAge.BackColor = System.Drawing.Color.White;
            this.txtAge.Location = new System.Drawing.Point(525, 25);
            this.txtAge.Name = "txtAge";
            this.txtAge.ReadOnly = true;
            this.txtAge.Size = new System.Drawing.Size(70, 23);
            this.txtAge.TabIndex = 5;
            this.txtAge.TabStop = false;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(12, 61);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(70, 15);
            this.lblDescription.TabIndex = 6;
            this.lblDescription.Text = "Description:";
            // 
            // txtDescription
            // 
            this.txtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDescription.BackColor = System.Drawing.Color.White;
            this.txtDescription.Location = new System.Drawing.Point(95, 58);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ReadOnly = true;
            this.txtDescription.Size = new System.Drawing.Size(850, 23);
            this.txtDescription.TabIndex = 7;
            this.txtDescription.TabStop = false;
            // 
            // grpAttribution
            // 
            this.grpAttribution.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.grpAttribution.Controls.Add(this.lblType);
            this.grpAttribution.Controls.Add(this.txtType);
            this.grpAttribution.Controls.Add(this.lblEvent);
            this.grpAttribution.Controls.Add(this.txtEvent);
            this.grpAttribution.Location = new System.Drawing.Point(12, 380);
            this.grpAttribution.Name = "grpAttribution";
            this.grpAttribution.Size = new System.Drawing.Size(470, 103);
            this.grpAttribution.TabIndex = 3;
            this.grpAttribution.TabStop = false;
            this.grpAttribution.Text = "Attribution";
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(12, 28);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(35, 15);
            this.lblType.TabIndex = 0;
            this.lblType.Text = "Type:";
            // 
            // txtType
            // 
            this.txtType.BackColor = System.Drawing.Color.White;
            this.txtType.Location = new System.Drawing.Point(65, 25);
            this.txtType.Name = "txtType";
            this.txtType.ReadOnly = true;
            this.txtType.Size = new System.Drawing.Size(390, 23);
            this.txtType.TabIndex = 1;
            this.txtType.TabStop = false;
            // 
            // lblEvent
            // 
            this.lblEvent.AutoSize = true;
            this.lblEvent.Location = new System.Drawing.Point(12, 60);
            this.lblEvent.Name = "lblEvent";
            this.lblEvent.Size = new System.Drawing.Size(39, 15);
            this.lblEvent.TabIndex = 2;
            this.lblEvent.Text = "Event:";
            // 
            // txtEvent
            // 
            this.txtEvent.BackColor = System.Drawing.Color.White;
            this.txtEvent.Location = new System.Drawing.Point(65, 57);
            this.txtEvent.Name = "txtEvent";
            this.txtEvent.ReadOnly = true;
            this.txtEvent.Size = new System.Drawing.Size(390, 23);
            this.txtEvent.TabIndex = 3;
            this.txtEvent.TabStop = false;
            // 
            // grpShadow
            // 
            this.grpShadow.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpShadow.Controls.Add(this.lblShadowId);
            this.grpShadow.Controls.Add(this.txtShadowId);
            this.grpShadow.Controls.Add(this.lblDevice);
            this.grpShadow.Controls.Add(this.txtDevice);
            this.grpShadow.Location = new System.Drawing.Point(492, 380);
            this.grpShadow.Name = "grpShadow";
            this.grpShadow.Size = new System.Drawing.Size(480, 103);
            this.grpShadow.TabIndex = 4;
            this.grpShadow.TabStop = false;
            this.grpShadow.Text = "Linked Shadow Copy (matched by timestamp)";
            // 
            // lblShadowId
            // 
            this.lblShadowId.AutoSize = true;
            this.lblShadowId.Location = new System.Drawing.Point(12, 28);
            this.lblShadowId.Name = "lblShadowId";
            this.lblShadowId.Size = new System.Drawing.Size(66, 15);
            this.lblShadowId.TabIndex = 0;
            this.lblShadowId.Text = "Shadow ID:";
            // 
            // txtShadowId
            // 
            this.txtShadowId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtShadowId.BackColor = System.Drawing.Color.White;
            this.txtShadowId.Location = new System.Drawing.Point(95, 25);
            this.txtShadowId.Name = "txtShadowId";
            this.txtShadowId.ReadOnly = true;
            this.txtShadowId.Size = new System.Drawing.Size(370, 23);
            this.txtShadowId.TabIndex = 1;
            this.txtShadowId.TabStop = false;
            // 
            // lblDevice
            // 
            this.lblDevice.AutoSize = true;
            this.lblDevice.Location = new System.Drawing.Point(12, 60);
            this.lblDevice.Name = "lblDevice";
            this.lblDevice.Size = new System.Drawing.Size(45, 15);
            this.lblDevice.TabIndex = 2;
            this.lblDevice.Text = "Device:";
            // 
            // txtDevice
            // 
            this.txtDevice.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDevice.BackColor = System.Drawing.Color.White;
            this.txtDevice.Location = new System.Drawing.Point(95, 57);
            this.txtDevice.Name = "txtDevice";
            this.txtDevice.ReadOnly = true;
            this.txtDevice.Size = new System.Drawing.Size(370, 23);
            this.txtDevice.TabIndex = 3;
            this.txtDevice.TabStop = false;
            // 
            // grpNotes
            // 
            this.grpNotes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpNotes.Controls.Add(this.txtNotes);
            this.grpNotes.Location = new System.Drawing.Point(12, 489);
            this.grpNotes.Name = "grpNotes";
            this.grpNotes.Size = new System.Drawing.Size(960, 115);
            this.grpNotes.TabIndex = 5;
            this.grpNotes.TabStop = false;
            this.grpNotes.Text = "Notes";
            // 
            // txtNotes
            // 
            this.txtNotes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNotes.BackColor = System.Drawing.SystemColors.Control;
            this.txtNotes.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtNotes.Location = new System.Drawing.Point(12, 22);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.ReadOnly = true;
            this.txtNotes.Size = new System.Drawing.Size(936, 83);
            this.txtNotes.TabIndex = 0;
            this.txtNotes.TabStop = false;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.Location = new System.Drawing.Point(656, 614);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 28);
            this.btnRefresh.TabIndex = 6;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.Btn_Refresh_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCopy.Location = new System.Drawing.Point(762, 614);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(110, 28);
            this.btnCopy.TabIndex = 7;
            this.btnCopy.Text = "Copy Details";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.Btn_Copy_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(882, 614);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 28);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.Btn_Close_Click);
            // 
            // Delete_Button
            // 
            this.Delete_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Delete_Button.Location = new System.Drawing.Point(15, 614);
            this.Delete_Button.Name = "Delete_Button";
            this.Delete_Button.Size = new System.Drawing.Size(100, 28);
            this.Delete_Button.TabIndex = 9;
            this.Delete_Button.Text = "Delete";
            this.Delete_Button.UseVisualStyleBackColor = true;
            this.Delete_Button.Click += new System.EventHandler(this.Delete_Button_Click);
            // 
            // Restore_Point_List_Form
            // 
            this.AcceptButton = this.btnClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 654);
            this.Controls.Add(this.Delete_Button);
            this.Controls.Add(this.lblSummary);
            this.Controls.Add(this.lvPoints);
            this.Controls.Add(this.grpRestorePoint);
            this.Controls.Add(this.grpAttribution);
            this.Controls.Add(this.grpShadow);
            this.Controls.Add(this.grpNotes);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.btnClose);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1000, 693);
            this.Name = "Restore_Point_List_Form";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Restore Points";
            this.grpRestorePoint.ResumeLayout(false);
            this.grpRestorePoint.PerformLayout();
            this.grpAttribution.ResumeLayout(false);
            this.grpAttribution.PerformLayout();
            this.grpShadow.ResumeLayout(false);
            this.grpShadow.PerformLayout();
            this.grpNotes.ResumeLayout(false);
            this.grpNotes.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblSummary;
        private System.Windows.Forms.ListView lvPoints;
        private System.Windows.Forms.ColumnHeader colSeq;
        private System.Windows.Forms.ColumnHeader colCreated;
        private System.Windows.Forms.ColumnHeader colAge;
        private System.Windows.Forms.ColumnHeader colType;
        private System.Windows.Forms.ColumnHeader colEvent;
        private System.Windows.Forms.ColumnHeader colDescription;
        private System.Windows.Forms.ColumnHeader colShadow;
        private System.Windows.Forms.GroupBox grpRestorePoint;
        private System.Windows.Forms.Label lblSeq;
        private System.Windows.Forms.TextBox txtSeq;
        private System.Windows.Forms.Label lblCreated;
        private System.Windows.Forms.TextBox txtCreated;
        private System.Windows.Forms.Label lblAge;
        private System.Windows.Forms.TextBox txtAge;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.GroupBox grpAttribution;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.TextBox txtType;
        private System.Windows.Forms.Label lblEvent;
        private System.Windows.Forms.TextBox txtEvent;
        private System.Windows.Forms.GroupBox grpShadow;
        private System.Windows.Forms.Label lblShadowId;
        private System.Windows.Forms.TextBox txtShadowId;
        private System.Windows.Forms.Label lblDevice;
        private System.Windows.Forms.TextBox txtDevice;
        private System.Windows.Forms.GroupBox grpNotes;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button Delete_Button;
    }
}
