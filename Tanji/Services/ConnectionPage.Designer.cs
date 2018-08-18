namespace Tanji.Services
{
    partial class ConnectionPage
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "sso.ticket",
            ""}, -1);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] {
            "furnidata.load.url",
            ""}, -1);
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem(new string[] {
            "productdata.load.url",
            ""}, -1);
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem(new string[] {
            "avatareditor.promohabbos",
            ""}, -1);
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem(new string[] {
            "external.texts.txt",
            ""}, -1);
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem(new string[] {
            "external.variables.txt",
            ""}, -1);
            System.Windows.Forms.ListViewItem listViewItem7 = new System.Windows.Forms.ListViewItem(new string[] {
            "external.figurepartlist.txt",
            ""}, -1);
            System.Windows.Forms.ListViewItem listViewItem8 = new System.Windows.Forms.ListViewItem(new string[] {
            "external.override.texts.txt",
            ""}, -1);
            System.Windows.Forms.ListViewItem listViewItem9 = new System.Windows.Forms.ListViewItem(new string[] {
            "external.override.variables.txt",
            ""}, -1);
            this.ChooseClientDlg = new System.Windows.Forms.OpenFileDialog();
            this.ExportCertificateDlg = new System.Windows.Forms.SaveFileDialog();
            this.ResetBtn = new Tangine.Controls.TangineButton();
            this.UpdateBtn = new Tangine.Controls.TangineButton();
            this.VariableTxt = new Tangine.Controls.TangineLabelBox();
            this.ValueTxt = new Tangine.Controls.TangineLabelBox();
            this.ProxyPortLbl = new Tangine.Controls.TangineLabel();
            this.DestroyCertificatesBtn = new Tangine.Controls.TangineButton();
            this.ExportCertificateAuthorityBtn = new Tangine.Controls.TangineButton();
            this.BrowseBtn = new Tangine.Controls.TangineButton();
            this.CustomClientTxt = new Tangine.Controls.TangineLabelBox();
            this.ConnectBtn = new Tangine.Controls.TangineButton();
            this.StatusTxt = new Tangine.Controls.TangineLabel();
            this.VariablesLv = new Tangine.Controls.TangineListView();
            this.VariableCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ValueCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // ChooseClientDlg
            // 
            this.ChooseClientDlg.Filter = "Shockwave Flash File (*.swf)|*.swf";
            this.ChooseClientDlg.Title = "Tanji - Choose Client";
            // 
            // ExportCertificateDlg
            // 
            this.ExportCertificateDlg.DefaultExt = "cer";
            this.ExportCertificateDlg.FileName = "Tanji Certificate Authority";
            this.ExportCertificateDlg.Filter = "X.509 Certificate (*.cer, *.crt)|*.cer;*.crt";
            this.ExportCertificateDlg.Title = "Tanji - Export Root CA";
            // 
            // ResetBtn
            // 
            this.ResetBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ResetBtn.Enabled = false;
            this.ResetBtn.Location = new System.Drawing.Point(411, 213);
            this.ResetBtn.Name = "ResetBtn";
            this.ResetBtn.Size = new System.Drawing.Size(90, 20);
            this.ResetBtn.TabIndex = 15;
            this.ResetBtn.TabStop = false;
            this.ResetBtn.Text = "Reset";
            this.ResetBtn.Click += new System.EventHandler(this.ResetBtn_Click);
            // 
            // UpdateBtn
            // 
            this.UpdateBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.UpdateBtn.Enabled = false;
            this.UpdateBtn.Location = new System.Drawing.Point(411, 187);
            this.UpdateBtn.Name = "UpdateBtn";
            this.UpdateBtn.Size = new System.Drawing.Size(90, 20);
            this.UpdateBtn.TabIndex = 14;
            this.UpdateBtn.TabStop = false;
            this.UpdateBtn.Text = "Update";
            this.UpdateBtn.Click += new System.EventHandler(this.UpdateBtn_Click);
            // 
            // VariableTxt
            // 
            this.VariableTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.VariableTxt.IsReadOnly = true;
            this.VariableTxt.Location = new System.Drawing.Point(3, 212);
            this.VariableTxt.Name = "VariableTxt";
            this.VariableTxt.Size = new System.Drawing.Size(402, 20);
            this.VariableTxt.TabIndex = 0;
            this.VariableTxt.TabStop = false;
            this.VariableTxt.Text = "";
            this.VariableTxt.TextPaddingWidth = 0;
            this.VariableTxt.Title = "Variable";
            // 
            // ValueTxt
            // 
            this.ValueTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ValueTxt.Location = new System.Drawing.Point(3, 187);
            this.ValueTxt.Name = "ValueTxt";
            this.ValueTxt.Size = new System.Drawing.Size(402, 20);
            this.ValueTxt.TabIndex = 0;
            this.ValueTxt.Text = "";
            this.ValueTxt.TextPaddingWidth = 11;
            this.ValueTxt.Title = "Value";
            this.ValueTxt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ValueTxt_KeyDown);
            // 
            // ProxyPortLbl
            // 
            this.ProxyPortLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ProxyPortLbl.Location = new System.Drawing.Point(411, 238);
            this.ProxyPortLbl.Name = "ProxyPortLbl";
            this.ProxyPortLbl.Size = new System.Drawing.Size(90, 20);
            this.ProxyPortLbl.TabIndex = 12;
            this.ProxyPortLbl.TabStop = false;
            this.ProxyPortLbl.Text = "Proxy Port: 8282";
            // 
            // DestroyCertificatesBtn
            // 
            this.DestroyCertificatesBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.DestroyCertificatesBtn.Location = new System.Drawing.Point(207, 238);
            this.DestroyCertificatesBtn.Name = "DestroyCertificatesBtn";
            this.DestroyCertificatesBtn.Size = new System.Drawing.Size(198, 20);
            this.DestroyCertificatesBtn.TabIndex = 11;
            this.DestroyCertificatesBtn.TabStop = false;
            this.DestroyCertificatesBtn.Text = "Destroy Certificates";
            this.DestroyCertificatesBtn.Click += new System.EventHandler(this.DestroyCertificatesBtn_Click);
            // 
            // ExportCertificateAuthorityBtn
            // 
            this.ExportCertificateAuthorityBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ExportCertificateAuthorityBtn.Location = new System.Drawing.Point(3, 238);
            this.ExportCertificateAuthorityBtn.Name = "ExportCertificateAuthorityBtn";
            this.ExportCertificateAuthorityBtn.Size = new System.Drawing.Size(198, 20);
            this.ExportCertificateAuthorityBtn.TabIndex = 10;
            this.ExportCertificateAuthorityBtn.TabStop = false;
            this.ExportCertificateAuthorityBtn.Text = "Export Certificate Authority";
            this.ExportCertificateAuthorityBtn.Click += new System.EventHandler(this.ExportCertificateAuthorityBtn_Click);
            // 
            // BrowseBtn
            // 
            this.BrowseBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BrowseBtn.Location = new System.Drawing.Point(411, 265);
            this.BrowseBtn.Name = "BrowseBtn";
            this.BrowseBtn.Size = new System.Drawing.Size(90, 20);
            this.BrowseBtn.TabIndex = 0;
            this.BrowseBtn.TabStop = false;
            this.BrowseBtn.Text = "Browse";
            this.BrowseBtn.Click += new System.EventHandler(this.BrowseBtn_Click);
            // 
            // CustomClientTxt
            // 
            this.CustomClientTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CustomClientTxt.Location = new System.Drawing.Point(3, 264);
            this.CustomClientTxt.Name = "CustomClientTxt";
            this.CustomClientTxt.Size = new System.Drawing.Size(402, 20);
            this.CustomClientTxt.TabIndex = 0;
            this.CustomClientTxt.TabStop = false;
            this.CustomClientTxt.Text = "";
            this.CustomClientTxt.TextPaddingWidth = 0;
            this.CustomClientTxt.Title = "Custom Client";
            // 
            // ConnectBtn
            // 
            this.ConnectBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ConnectBtn.Location = new System.Drawing.Point(411, 291);
            this.ConnectBtn.Name = "ConnectBtn";
            this.ConnectBtn.Size = new System.Drawing.Size(90, 20);
            this.ConnectBtn.TabIndex = 0;
            this.ConnectBtn.TabStop = false;
            this.ConnectBtn.Text = "Connect";
            this.ConnectBtn.Click += new System.EventHandler(this.ConnectBtn_Click);
            // 
            // StatusTxt
            // 
            this.StatusTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.StatusTxt.Location = new System.Drawing.Point(3, 290);
            this.StatusTxt.Name = "StatusTxt";
            this.StatusTxt.Size = new System.Drawing.Size(402, 21);
            this.StatusTxt.TabIndex = 4;
            this.StatusTxt.TabStop = false;
            this.StatusTxt.Text = "Standing By...";
            // 
            // VariablesLv
            // 
            this.VariablesLv.CheckBoxes = true;
            this.VariablesLv.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.VariableCol,
            this.ValueCol});
            this.VariablesLv.Dock = System.Windows.Forms.DockStyle.Top;
            this.VariablesLv.FullRowSelect = true;
            this.VariablesLv.GridLines = true;
            this.VariablesLv.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.VariablesLv.HideSelection = false;
            listViewItem1.StateImageIndex = 0;
            listViewItem2.StateImageIndex = 0;
            listViewItem3.StateImageIndex = 0;
            listViewItem4.StateImageIndex = 0;
            listViewItem5.StateImageIndex = 0;
            listViewItem6.StateImageIndex = 0;
            listViewItem7.StateImageIndex = 0;
            listViewItem8.StateImageIndex = 0;
            listViewItem9.StateImageIndex = 0;
            this.VariablesLv.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3,
            listViewItem4,
            listViewItem5,
            listViewItem6,
            listViewItem7,
            listViewItem8,
            listViewItem9});
            this.VariablesLv.Location = new System.Drawing.Point(0, 0);
            this.VariablesLv.MultiSelect = false;
            this.VariablesLv.Name = "VariablesLv";
            this.VariablesLv.ShowItemToolTips = true;
            this.VariablesLv.Size = new System.Drawing.Size(504, 181);
            this.VariablesLv.TabIndex = 3;
            this.VariablesLv.TabStop = false;
            this.VariablesLv.UseCompatibleStateImageBehavior = false;
            this.VariablesLv.View = System.Windows.Forms.View.Details;
            this.VariablesLv.ItemSelectionStateChanged += new System.EventHandler(this.VariablesLv_ItemSelectionStateChanged);
            this.VariablesLv.ItemSelected += new System.EventHandler(this.VariablesLv_ItemSelected);
            // 
            // VariableCol
            // 
            this.VariableCol.Text = "Variable";
            this.VariableCol.Width = 241;
            // 
            // ValueCol
            // 
            this.ValueCol.Text = "Value";
            this.ValueCol.Width = 241;
            // 
            // ConnectionPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ResetBtn);
            this.Controls.Add(this.UpdateBtn);
            this.Controls.Add(this.VariableTxt);
            this.Controls.Add(this.ValueTxt);
            this.Controls.Add(this.ProxyPortLbl);
            this.Controls.Add(this.DestroyCertificatesBtn);
            this.Controls.Add(this.ExportCertificateAuthorityBtn);
            this.Controls.Add(this.BrowseBtn);
            this.Controls.Add(this.CustomClientTxt);
            this.Controls.Add(this.ConnectBtn);
            this.Controls.Add(this.StatusTxt);
            this.Controls.Add(this.VariablesLv);
            this.Name = "ConnectionPage";
            this.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(this.ConnectionPage_PropertyChanged);
            this.ResumeLayout(false);

        }

        #endregion
        private Tangine.Controls.TangineListView VariablesLv;
        private Tangine.Controls.TangineLabel StatusTxt;
        private Tangine.Controls.TangineButton ConnectBtn;
        private Tangine.Controls.TangineLabelBox CustomClientTxt;
        private Tangine.Controls.TangineButton BrowseBtn;
        private Tangine.Controls.TangineButton ExportCertificateAuthorityBtn;
        private Tangine.Controls.TangineButton DestroyCertificatesBtn;
        private Tangine.Controls.TangineLabel ProxyPortLbl;
        private Tangine.Controls.TangineLabelBox ValueTxt;
        private Tangine.Controls.TangineLabelBox VariableTxt;
        private System.Windows.Forms.ColumnHeader VariableCol;
        private System.Windows.Forms.ColumnHeader ValueCol;
        private System.Windows.Forms.OpenFileDialog ChooseClientDlg;
        private System.Windows.Forms.SaveFileDialog ExportCertificateDlg;
        private Tangine.Controls.TangineButton UpdateBtn;
        private Tangine.Controls.TangineButton ResetBtn;
    }
}
