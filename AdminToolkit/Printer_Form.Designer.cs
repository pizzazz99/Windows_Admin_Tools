namespace Admin_Tools
{
    partial class Printer_Form
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
      lvPrinters = new ListView();
      colName = new ColumnHeader();
      colStatus = new ColumnHeader();
      colLive = new ColumnHeader();
      colAddress = new ColumnHeader();
      colDefault = new ColumnHeader();
      colType = new ColumnHeader();
      colPort = new ColumnHeader();
      colDriver = new ColumnHeader();
      chkHideVirtual = new CheckBox();
      lblSummary = new Label();
      BtnRefresh = new Button();
      BtnDetails = new Button();
      BtnSupplies = new Button();
      BtnWake = new Button();
      BtnClose = new Button();
      SuspendLayout();
      // 
      // lvPrinters
      // 
      lvPrinters.Anchor =  AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      lvPrinters.Columns.AddRange( new ColumnHeader[] { colName, colStatus, colLive, colAddress, colDefault, colType, colPort, colDriver } );
      lvPrinters.FullRowSelect = true;
      lvPrinters.GridLines = true;
      lvPrinters.Location = new Point( 12, 12 );
      lvPrinters.Name = "lvPrinters";
      lvPrinters.Size = new Size( 853, 356 );
      lvPrinters.TabIndex = 0;
      lvPrinters.UseCompatibleStateImageBehavior = false;
      lvPrinters.View = View.Details;
      lvPrinters.DoubleClick += Btn_Details_Click;
      // 
      // colName
      // 
      colName.Text = "Printer Name";
      colName.Width = 190;
      // 
      // colStatus
      // 
      colStatus.Text = "Status";
      colStatus.Width = 120;
      // 
      // colLive
      // 
      colLive.Text = "Live";
      colLive.Width = 70;
      // 
      // colAddress
      // 
      colAddress.Text = "IP Address";
      colAddress.Width = 110;
      // 
      // colDefault
      // 
      colDefault.Text = "Default";
      colDefault.Width = 55;
      // 
      // colType
      // 
      colType.Text = "Type";
      colType.Width = 65;
      // 
      // colPort
      // 
      colPort.Text = "Port";
      colPort.Width = 110;
      // 
      // colDriver
      // 
      colDriver.Text = "Driver";
      colDriver.Width = 130;
      // 
      // chkHideVirtual
      // 
      chkHideVirtual.Anchor =  AnchorStyles.Bottom | AnchorStyles.Left;
      chkHideVirtual.AutoSize = true;
      chkHideVirtual.Checked = true;
      chkHideVirtual.CheckState = CheckState.Checked;
      chkHideVirtual.Location = new Point( 12, 376 );
      chkHideVirtual.Name = "chkHideVirtual";
      chkHideVirtual.Size = new Size( 320, 19 );
      chkHideVirtual.TabIndex = 7;
      chkHideVirtual.Text = "Hide virtual / software printers (PDF, Fax, OneNote, etc.)";
      chkHideVirtual.UseVisualStyleBackColor = true;
      chkHideVirtual.CheckedChanged += ChkHideVirtual_CheckedChanged;
      // 
      // lblSummary
      // 
      lblSummary.Anchor =  AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      lblSummary.Font = new Font( "Segoe UI", 9F, FontStyle.Bold );
      lblSummary.Location = new Point( 12, 400 );
      lblSummary.Name = "lblSummary";
      lblSummary.Size = new Size( 853, 20 );
      lblSummary.TabIndex = 1;
      lblSummary.Text = "Loading...";
      // 
      // BtnRefresh
      // 
      BtnRefresh.Anchor =  AnchorStyles.Bottom | AnchorStyles.Left;
      BtnRefresh.Location = new Point( 12, 428 );
      BtnRefresh.Name = "BtnRefresh";
      BtnRefresh.Size = new Size( 110, 28 );
      BtnRefresh.TabIndex = 2;
      BtnRefresh.Text = "Refresh";
      BtnRefresh.UseVisualStyleBackColor = true;
      BtnRefresh.Click += Btn_Refresh_Click;
      // 
      // BtnDetails
      // 
      BtnDetails.Anchor =  AnchorStyles.Bottom | AnchorStyles.Left;
      BtnDetails.Location = new Point( 128, 428 );
      BtnDetails.Name = "BtnDetails";
      BtnDetails.Size = new Size( 150, 28 );
      BtnDetails.TabIndex = 3;
      BtnDetails.Text = "Printer Details";
      BtnDetails.UseVisualStyleBackColor = true;
      BtnDetails.Click += Btn_Details_Click;
      // 
      // BtnSupplies
      // 
      BtnSupplies.Anchor =  AnchorStyles.Bottom | AnchorStyles.Left;
      BtnSupplies.Location = new Point( 284, 428 );
      BtnSupplies.Name = "BtnSupplies";
      BtnSupplies.Size = new Size( 190, 28 );
      BtnSupplies.TabIndex = 4;
      BtnSupplies.Text = "Check Ink / Toner Levels";
      BtnSupplies.UseVisualStyleBackColor = true;
      BtnSupplies.Click += Btn_Supplies_Click;
      // 
      // BtnWake
      // 
      BtnWake.Anchor =  AnchorStyles.Bottom | AnchorStyles.Left;
      BtnWake.Location = new Point( 484, 428 );
      BtnWake.Name = "BtnWake";
      BtnWake.Size = new Size( 130, 28 );
      BtnWake.TabIndex = 6;
      BtnWake.Text = "Wake / Retry";
      BtnWake.UseVisualStyleBackColor = true;
      BtnWake.Click += Btn_Wake_Click;
      // 
      // BtnClose
      // 
      BtnClose.Anchor =  AnchorStyles.Bottom | AnchorStyles.Right;
      BtnClose.Location = new Point( 775, 428 );
      BtnClose.Name = "BtnClose";
      BtnClose.Size = new Size( 90, 28 );
      BtnClose.TabIndex = 5;
      BtnClose.Text = "Close";
      BtnClose.UseVisualStyleBackColor = true;
      BtnClose.Click += Btn_Close_Click;
      // 
      // Printer_Form
      // 
      AcceptButton = BtnClose;
      AutoScaleDimensions = new SizeF( 7F, 15F );
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size( 877, 468 );
      Controls.Add( lvPrinters );
      Controls.Add( chkHideVirtual );
      Controls.Add( lblSummary );
      Controls.Add( BtnRefresh );
      Controls.Add( BtnDetails );
      Controls.Add( BtnSupplies );
      Controls.Add( BtnWake );
      Controls.Add( BtnClose );
      Font = new Font( "Segoe UI", 9F );
      MinimizeBox = false;
      MinimumSize = new Size( 700, 400 );
      Name = "Printer_Form";
      ShowInTaskbar = false;
      StartPosition = FormStartPosition.CenterParent;
      Text = "Printers";
      ResumeLayout( false );
      PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ListView lvPrinters;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colStatus;
        private System.Windows.Forms.ColumnHeader colLive;
        private System.Windows.Forms.ColumnHeader colAddress;
        private System.Windows.Forms.ColumnHeader colDefault;
        private System.Windows.Forms.ColumnHeader colType;
        private System.Windows.Forms.ColumnHeader colPort;
        private System.Windows.Forms.ColumnHeader colDriver;
        private System.Windows.Forms.CheckBox chkHideVirtual;
        private System.Windows.Forms.Label lblSummary;
        private System.Windows.Forms.Button BtnRefresh;
        private System.Windows.Forms.Button BtnDetails;
        private System.Windows.Forms.Button BtnSupplies;
        private System.Windows.Forms.Button BtnWake;
        private System.Windows.Forms.Button BtnClose;
    }
}
