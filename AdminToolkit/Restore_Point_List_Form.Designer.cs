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
            lblSummary = new Label();
            lblStatus = new Label();
            lvPoints = new ListView();
            colSeq = new ColumnHeader();
            colCreated = new ColumnHeader();
            colAge = new ColumnHeader();
            colType = new ColumnHeader();
            colEvent = new ColumnHeader();
            colDescription = new ColumnHeader();
            colShadow = new ColumnHeader();
            grpRestorePoint = new GroupBox();
            lblSeq = new Label();
            txtSeq = new TextBox();
            lblCreated = new Label();
            txtCreated = new TextBox();
            lblAge = new Label();
            txtAge = new TextBox();
            lblDescription = new Label();
            txtDescription = new TextBox();
            grpAttribution = new GroupBox();
            lblType = new Label();
            txtType = new TextBox();
            lblEvent = new Label();
            txtEvent = new TextBox();
            grpShadow = new GroupBox();
            lblShadowId = new Label();
            txtShadowId = new TextBox();
            lblDevice = new Label();
            txtDevice = new TextBox();
            grpNotes = new GroupBox();
            txtNotes = new TextBox();
            btnRefresh = new Button();
            btnCopy = new Button();
            btnClose = new Button();
            Create_Restore_Point_Button = new Button();
            Delete_Selected_Restore_Point_Button = new Button();
            Browse_Snapshot_Button = new Button();
            grpRestorePoint.SuspendLayout();
            grpAttribution.SuspendLayout();
            grpShadow.SuspendLayout();
            grpNotes.SuspendLayout();
            SuspendLayout();
            // 
            // lblSummary
            // 
            lblSummary.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblSummary.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblSummary.Location = new Point(12, 9);
            lblSummary.Name = "lblSummary";
            lblSummary.Size = new Size(960, 20);
            lblSummary.TabIndex = 0;
            lblSummary.Text = "Loading...";
            // 
            // lblStatus
            // 
            lblStatus.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblStatus.ForeColor = SystemColors.GrayText;
            lblStatus.Location = new Point(12, 29);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(960, 18);
            lblStatus.TabIndex = 12;
            lblStatus.Text = "";
            // 
            // lvPoints
            // 
            lvPoints.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lvPoints.Columns.AddRange(new ColumnHeader[] { colSeq, colCreated, colAge, colType, colEvent, colDescription, colShadow });
            lvPoints.FullRowSelect = true;
            lvPoints.GridLines = true;
            lvPoints.Location = new Point(12, 50);
            lvPoints.MultiSelect = true;
            lvPoints.Name = "lvPoints";
            lvPoints.Size = new Size(960, 249);
            lvPoints.TabIndex = 1;
            lvPoints.UseCompatibleStateImageBehavior = false;
            lvPoints.View = View.Details;
            lvPoints.SelectedIndexChanged += Lv_Selection_Changed;
            // 
            // colSeq
            // 
            colSeq.Text = "Seq #";
            colSeq.TextAlign = HorizontalAlignment.Right;
            // 
            // colCreated
            // 
            colCreated.Text = "Created";
            colCreated.Width = 140;
            // 
            // colAge
            // 
            colAge.Text = "Age (days)";
            colAge.TextAlign = HorizontalAlignment.Right;
            colAge.Width = 80;
            // 
            // colType
            // 
            colType.Text = "Type";
            colType.Width = 200;
            // 
            // colEvent
            // 
            colEvent.Text = "Event";
            colEvent.Width = 170;
            // 
            // colDescription
            // 
            colDescription.Text = "Description";
            colDescription.Width = 230;
            // 
            // colShadow
            // 
            colShadow.Text = "Shadow Copy";
            colShadow.Width = 90;
            // 
            // grpRestorePoint
            // 
            grpRestorePoint.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            grpRestorePoint.Controls.Add(lblSeq);
            grpRestorePoint.Controls.Add(txtSeq);
            grpRestorePoint.Controls.Add(lblCreated);
            grpRestorePoint.Controls.Add(txtCreated);
            grpRestorePoint.Controls.Add(lblAge);
            grpRestorePoint.Controls.Add(txtAge);
            grpRestorePoint.Controls.Add(lblDescription);
            grpRestorePoint.Controls.Add(txtDescription);
            grpRestorePoint.Location = new Point(12, 322);
            grpRestorePoint.Name = "grpRestorePoint";
            grpRestorePoint.Size = new Size(960, 96);
            grpRestorePoint.TabIndex = 2;
            grpRestorePoint.TabStop = false;
            grpRestorePoint.Text = "Restore Point";
            // 
            // lblSeq
            // 
            lblSeq.AutoSize = true;
            lblSeq.Location = new Point(12, 28);
            lblSeq.Name = "lblSeq";
            lblSeq.Size = new Size(71, 15);
            lblSeq.TabIndex = 0;
            lblSeq.Text = "Sequence #:";
            // 
            // txtSeq
            // 
            txtSeq.BackColor = Color.White;
            txtSeq.Location = new Point(95, 25);
            txtSeq.Name = "txtSeq";
            txtSeq.ReadOnly = true;
            txtSeq.Size = new Size(80, 23);
            txtSeq.TabIndex = 1;
            txtSeq.TabStop = false;
            // 
            // lblCreated
            // 
            lblCreated.AutoSize = true;
            lblCreated.Location = new Point(210, 28);
            lblCreated.Name = "lblCreated";
            lblCreated.Size = new Size(51, 15);
            lblCreated.TabIndex = 2;
            lblCreated.Text = "Created:";
            // 
            // txtCreated
            // 
            txtCreated.BackColor = Color.White;
            txtCreated.Location = new Point(270, 25);
            txtCreated.Name = "txtCreated";
            txtCreated.ReadOnly = true;
            txtCreated.Size = new Size(150, 23);
            txtCreated.TabIndex = 3;
            txtCreated.TabStop = false;
            // 
            // lblAge
            // 
            lblAge.AutoSize = true;
            lblAge.Location = new Point(450, 28);
            lblAge.Name = "lblAge";
            lblAge.Size = new Size(66, 15);
            lblAge.TabIndex = 4;
            lblAge.Text = "Age (days):";
            // 
            // txtAge
            // 
            txtAge.BackColor = Color.White;
            txtAge.Location = new Point(525, 25);
            txtAge.Name = "txtAge";
            txtAge.ReadOnly = true;
            txtAge.Size = new Size(70, 23);
            txtAge.TabIndex = 5;
            txtAge.TabStop = false;
            // 
            // lblDescription
            // 
            lblDescription.AutoSize = true;
            lblDescription.Location = new Point(12, 61);
            lblDescription.Name = "lblDescription";
            lblDescription.Size = new Size(70, 15);
            lblDescription.TabIndex = 6;
            lblDescription.Text = "Description:";
            // 
            // txtDescription
            // 
            txtDescription.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtDescription.BackColor = Color.White;
            txtDescription.Location = new Point(95, 58);
            txtDescription.Name = "txtDescription";
            txtDescription.ReadOnly = true;
            txtDescription.Size = new Size(850, 23);
            txtDescription.TabIndex = 7;
            txtDescription.TabStop = false;
            // 
            // grpAttribution
            // 
            grpAttribution.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            grpAttribution.Controls.Add(lblType);
            grpAttribution.Controls.Add(txtType);
            grpAttribution.Controls.Add(lblEvent);
            grpAttribution.Controls.Add(txtEvent);
            grpAttribution.Location = new Point(12, 424);
            grpAttribution.Name = "grpAttribution";
            grpAttribution.Size = new Size(470, 103);
            grpAttribution.TabIndex = 3;
            grpAttribution.TabStop = false;
            grpAttribution.Text = "Attribution";
            // 
            // lblType
            // 
            lblType.AutoSize = true;
            lblType.Location = new Point(12, 28);
            lblType.Name = "lblType";
            lblType.Size = new Size(35, 15);
            lblType.TabIndex = 0;
            lblType.Text = "Type:";
            // 
            // txtType
            // 
            txtType.BackColor = Color.White;
            txtType.Location = new Point(65, 25);
            txtType.Name = "txtType";
            txtType.ReadOnly = true;
            txtType.Size = new Size(390, 23);
            txtType.TabIndex = 1;
            txtType.TabStop = false;
            // 
            // lblEvent
            // 
            lblEvent.AutoSize = true;
            lblEvent.Location = new Point(12, 60);
            lblEvent.Name = "lblEvent";
            lblEvent.Size = new Size(39, 15);
            lblEvent.TabIndex = 2;
            lblEvent.Text = "Event:";
            // 
            // txtEvent
            // 
            txtEvent.BackColor = Color.White;
            txtEvent.Location = new Point(65, 57);
            txtEvent.Name = "txtEvent";
            txtEvent.ReadOnly = true;
            txtEvent.Size = new Size(390, 23);
            txtEvent.TabIndex = 3;
            txtEvent.TabStop = false;
            // 
            // grpShadow
            // 
            grpShadow.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            grpShadow.Controls.Add(lblShadowId);
            grpShadow.Controls.Add(txtShadowId);
            grpShadow.Controls.Add(lblDevice);
            grpShadow.Controls.Add(txtDevice);
            grpShadow.Location = new Point(492, 424);
            grpShadow.Name = "grpShadow";
            grpShadow.Size = new Size(480, 103);
            grpShadow.TabIndex = 4;
            grpShadow.TabStop = false;
            grpShadow.Text = "Linked Shadow Copy (matched by timestamp)";
            // 
            // lblShadowId
            // 
            lblShadowId.AutoSize = true;
            lblShadowId.Location = new Point(12, 28);
            lblShadowId.Name = "lblShadowId";
            lblShadowId.Size = new Size(66, 15);
            lblShadowId.TabIndex = 0;
            lblShadowId.Text = "Shadow ID:";
            // 
            // txtShadowId
            // 
            txtShadowId.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtShadowId.BackColor = Color.White;
            txtShadowId.Location = new Point(95, 25);
            txtShadowId.Name = "txtShadowId";
            txtShadowId.ReadOnly = true;
            txtShadowId.Size = new Size(370, 23);
            txtShadowId.TabIndex = 1;
            txtShadowId.TabStop = false;
            // 
            // lblDevice
            // 
            lblDevice.AutoSize = true;
            lblDevice.Location = new Point(12, 60);
            lblDevice.Name = "lblDevice";
            lblDevice.Size = new Size(45, 15);
            lblDevice.TabIndex = 2;
            lblDevice.Text = "Device:";
            // 
            // txtDevice
            // 
            txtDevice.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtDevice.BackColor = Color.White;
            txtDevice.Location = new Point(95, 57);
            txtDevice.Name = "txtDevice";
            txtDevice.ReadOnly = true;
            txtDevice.Size = new Size(370, 23);
            txtDevice.TabIndex = 3;
            txtDevice.TabStop = false;
            // 
            // grpNotes
            // 
            grpNotes.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            grpNotes.Controls.Add(txtNotes);
            grpNotes.Location = new Point(12, 533);
            grpNotes.Name = "grpNotes";
            grpNotes.Size = new Size(960, 86);
            grpNotes.TabIndex = 5;
            grpNotes.TabStop = false;
            grpNotes.Text = "Notes";
            // 
            // txtNotes
            // 
            txtNotes.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtNotes.BackColor = SystemColors.Control;
            txtNotes.BorderStyle = BorderStyle.None;
            txtNotes.Location = new Point(12, 22);
            txtNotes.Multiline = true;
            txtNotes.Name = "txtNotes";
            txtNotes.ReadOnly = true;
            txtNotes.ScrollBars = ScrollBars.Vertical;
            txtNotes.Size = new Size(936, 54);
            txtNotes.TabIndex = 0;
            txtNotes.TabStop = false;
            // 
            // btnRefresh
            // 
            btnRefresh.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnRefresh.Location = new Point(656, 658);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(100, 28);
            btnRefresh.TabIndex = 6;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += Btn_Refresh_Click;
            // 
            // btnCopy
            // 
            btnCopy.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCopy.Location = new Point(762, 658);
            btnCopy.Name = "btnCopy";
            btnCopy.Size = new Size(110, 28);
            btnCopy.TabIndex = 7;
            btnCopy.Text = "Copy Details";
            btnCopy.UseVisualStyleBackColor = true;
            btnCopy.Click += Btn_Copy_Click;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnClose.Location = new Point(882, 658);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(90, 28);
            btnClose.TabIndex = 8;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += Btn_Close_Click;
            // 
            // Create_Restore_Point_Button
            // 
            Create_Restore_Point_Button.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            Create_Restore_Point_Button.Location = new Point(12, 625);
            Create_Restore_Point_Button.Name = "Create_Restore_Point_Button";
            Create_Restore_Point_Button.Size = new Size(75, 61);
            Create_Restore_Point_Button.TabIndex = 10;
            Create_Restore_Point_Button.Text = "Create Restore Point";
            Create_Restore_Point_Button.UseVisualStyleBackColor = true;
            Create_Restore_Point_Button.Click += Create_Restore_Point_Button_Click;
            // 
            // Delete_Selected_Restore_Point_Button
            // 
            Delete_Selected_Restore_Point_Button.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            Delete_Selected_Restore_Point_Button.BackColor = Color.LightGoldenrodYellow;
            Delete_Selected_Restore_Point_Button.Location = new Point(94, 625);
            Delete_Selected_Restore_Point_Button.Name = "Delete_Selected_Restore_Point_Button";
            Delete_Selected_Restore_Point_Button.Size = new Size(93, 61);
            Delete_Selected_Restore_Point_Button.TabIndex = 11;
            Delete_Selected_Restore_Point_Button.Text = "Delete Selected Restore Point";
            Delete_Selected_Restore_Point_Button.UseVisualStyleBackColor = false;
            Delete_Selected_Restore_Point_Button.Click += Delete_Selected_Restore_Point_Button_Click;
            // 
            // Browse_Snapshot_Button
            // 
            Browse_Snapshot_Button.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            Browse_Snapshot_Button.Location = new Point(194, 625);
            Browse_Snapshot_Button.Name = "Browse_Snapshot_Button";
            Browse_Snapshot_Button.Size = new Size(93, 61);
            Browse_Snapshot_Button.TabIndex = 13;
            Browse_Snapshot_Button.Text = "Browse Files at This Point";
            Browse_Snapshot_Button.UseVisualStyleBackColor = true;
            Browse_Snapshot_Button.Click += Btn_Browse_Click;
            // 
            // Restore_Point_List_Form
            // 
            AcceptButton = btnClose;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(984, 698);
            Controls.Add(Browse_Snapshot_Button);
            Controls.Add(Delete_Selected_Restore_Point_Button);
            Controls.Add(Create_Restore_Point_Button);
            Controls.Add(lblSummary);
            Controls.Add(lblStatus);
            Controls.Add(lvPoints);
            Controls.Add(grpRestorePoint);
            Controls.Add(grpAttribution);
            Controls.Add(grpShadow);
            Controls.Add(grpNotes);
            Controls.Add(btnRefresh);
            Controls.Add(btnCopy);
            Controls.Add(btnClose);
            Font = new Font("Segoe UI", 9F);
            MinimizeBox = false;
            MinimumSize = new Size(1000, 693);
            Name = "Restore_Point_List_Form";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Restore Points";
            grpRestorePoint.ResumeLayout(false);
            grpRestorePoint.PerformLayout();
            grpAttribution.ResumeLayout(false);
            grpAttribution.PerformLayout();
            grpShadow.ResumeLayout(false);
            grpShadow.PerformLayout();
            grpNotes.ResumeLayout(false);
            grpNotes.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblSummary;
        private System.Windows.Forms.Label lblStatus;
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
        private Button Create_Restore_Point_Button;
        private Button Delete_Selected_Restore_Point_Button;
        private Button Browse_Snapshot_Button;
    }
}