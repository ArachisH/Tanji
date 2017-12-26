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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("sso.ticket");
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("furnidata.load.url");
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem("productdata.load.url");
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem("avatareditor.promohabbos");
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem("external.texts.txt");
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem("external.variables.txt");
            System.Windows.Forms.ListViewItem listViewItem7 = new System.Windows.Forms.ListViewItem("external.figurepartlist.txt");
            System.Windows.Forms.ListViewItem listViewItem8 = new System.Windows.Forms.ListViewItem("external.override.texts.txt");
            System.Windows.Forms.ListViewItem listViewItem9 = new System.Windows.Forms.ListViewItem("external.override.variables.txt");
            this.HotelServerTxt = new Tangine.Controls.TangineLabelBox();
            this.AutomaticServerExtractionChbx = new System.Windows.Forms.CheckBox();
            this.Seperator1 = new System.Windows.Forms.Label();
            this.VariablesLv = new Tangine.Controls.TangineListView();
            this.VariableCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ValueCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.StatusTxt = new Tangine.Controls.TangineLabel();
            this.ConnectBtn = new Tangine.Controls.TangineButton();
            this.CancelBtn = new Tangine.Controls.TangineButton();
            this.Seperator3 = new System.Windows.Forms.Label();
            this.CustomClientTxt = new Tangine.Controls.TangineLabelBox();
            this.BrowseBtn = new Tangine.Controls.TangineButton();
            this.ExportCertificateAuthorityBtn = new Tangine.Controls.TangineButton();
            this.DestroyCertificatesBtn = new Tangine.Controls.TangineButton();
            this.ProxyPortLbl = new Tangine.Controls.TangineLabel();
            this.Seperator2 = new System.Windows.Forms.Label();
            this.ValueTxt = new Tangine.Controls.TangineLabelBox();
            this.VariableTxt = new Tangine.Controls.TangineLabelBox();
            this.ClearBtn = new Tangine.Controls.TangineButton();
            this.UpdateBtn = new Tangine.Controls.TangineButton();
            this.SuspendLayout();
            // 
            // HotelServerTxt
            // 
            this.HotelServerTxt.IsReadOnly = true;
            this.HotelServerTxt.Location = new System.Drawing.Point(3, 3);
            this.HotelServerTxt.Name = "HotelServerTxt";
            this.HotelServerTxt.Size = new System.Drawing.Size(317, 20);
            this.HotelServerTxt.TabIndex = 0;
            this.HotelServerTxt.Text = "*:*";
            this.HotelServerTxt.Title = "Hotel Server";
            // 
            // AutomaticServerExtractionChbx
            // 
            this.AutomaticServerExtractionChbx.AutoSize = true;
            this.AutomaticServerExtractionChbx.Checked = true;
            this.AutomaticServerExtractionChbx.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AutomaticServerExtractionChbx.Location = new System.Drawing.Point(326, 5);
            this.AutomaticServerExtractionChbx.Name = "AutomaticServerExtractionChbx";
            this.AutomaticServerExtractionChbx.Size = new System.Drawing.Size(157, 17);
            this.AutomaticServerExtractionChbx.TabIndex = 1;
            this.AutomaticServerExtractionChbx.Text = "Automatic Server Extraction";
            this.AutomaticServerExtractionChbx.UseVisualStyleBackColor = true;
            // 
            // Seperator1
            // 
            this.Seperator1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.Seperator1.Location = new System.Drawing.Point(3, 26);
            this.Seperator1.Name = "Seperator1";
            this.Seperator1.Size = new System.Drawing.Size(480, 1);
            this.Seperator1.TabIndex = 2;
            // 
            // VariablesLv
            // 
            this.VariablesLv.CheckBoxes = true;
            this.VariablesLv.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.VariableCol,
            this.ValueCol});
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
            this.VariablesLv.Location = new System.Drawing.Point(3, 30);
            this.VariablesLv.MultiSelect = false;
            this.VariablesLv.Name = "VariablesLv";
            this.VariablesLv.ShowItemToolTips = true;
            this.VariablesLv.Size = new System.Drawing.Size(480, 183);
            this.VariablesLv.TabIndex = 3;
            this.VariablesLv.UseCompatibleStateImageBehavior = false;
            this.VariablesLv.View = System.Windows.Forms.View.Details;
            // 
            // VariableCol
            // 
            this.VariableCol.Text = "Variable";
            this.VariableCol.Width = 229;
            // 
            // ValueCol
            // 
            this.ValueCol.Text = "Value";
            this.ValueCol.Width = 230;
            // 
            // StatusTxt
            // 
            this.StatusTxt.Location = new System.Drawing.Point(3, 325);
            this.StatusTxt.Name = "StatusTxt";
            this.StatusTxt.Size = new System.Drawing.Size(288, 20);
            this.StatusTxt.TabIndex = 4;
            this.StatusTxt.Text = "Standing By...";
            // 
            // ConnectBtn
            // 
            this.ConnectBtn.BackColor = System.Drawing.Color.Transparent;
            this.ConnectBtn.Location = new System.Drawing.Point(393, 325);
            this.ConnectBtn.Name = "ConnectBtn";
            this.ConnectBtn.Size = new System.Drawing.Size(90, 20);
            this.ConnectBtn.TabIndex = 5;
            this.ConnectBtn.Text = "Connect";
            this.ConnectBtn.Click += new System.EventHandler(this.ConnectBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.BackColor = System.Drawing.Color.Transparent;
            this.CancelBtn.Location = new System.Drawing.Point(297, 325);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(90, 20);
            this.CancelBtn.TabIndex = 6;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // Seperator3
            // 
            this.Seperator3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.Seperator3.Location = new System.Drawing.Point(3, 321);
            this.Seperator3.Name = "Seperator3";
            this.Seperator3.Size = new System.Drawing.Size(480, 1);
            this.Seperator3.TabIndex = 7;
            // 
            // CustomClientTxt
            // 
            this.CustomClientTxt.Location = new System.Drawing.Point(3, 298);
            this.CustomClientTxt.Name = "CustomClientTxt";
            this.CustomClientTxt.Size = new System.Drawing.Size(384, 20);
            this.CustomClientTxt.TabIndex = 8;
            this.CustomClientTxt.Text = "";
            this.CustomClientTxt.Title = "Custom Client";
            // 
            // BrowseBtn
            // 
            this.BrowseBtn.BackColor = System.Drawing.Color.Transparent;
            this.BrowseBtn.Location = new System.Drawing.Point(393, 298);
            this.BrowseBtn.Name = "BrowseBtn";
            this.BrowseBtn.Size = new System.Drawing.Size(90, 20);
            this.BrowseBtn.TabIndex = 9;
            this.BrowseBtn.Text = "Browse";
            // 
            // ExportCertificateAuthorityBtn
            // 
            this.ExportCertificateAuthorityBtn.BackColor = System.Drawing.Color.Transparent;
            this.ExportCertificateAuthorityBtn.Location = new System.Drawing.Point(3, 272);
            this.ExportCertificateAuthorityBtn.Name = "ExportCertificateAuthorityBtn";
            this.ExportCertificateAuthorityBtn.Size = new System.Drawing.Size(189, 20);
            this.ExportCertificateAuthorityBtn.TabIndex = 10;
            this.ExportCertificateAuthorityBtn.Text = "Export Certificate Authority";
            this.ExportCertificateAuthorityBtn.Click += new System.EventHandler(this.ExportCertificateAuthorityBtn_Click);
            // 
            // DestroyCertificatesBtn
            // 
            this.DestroyCertificatesBtn.BackColor = System.Drawing.Color.Transparent;
            this.DestroyCertificatesBtn.Location = new System.Drawing.Point(198, 272);
            this.DestroyCertificatesBtn.Name = "DestroyCertificatesBtn";
            this.DestroyCertificatesBtn.Size = new System.Drawing.Size(189, 20);
            this.DestroyCertificatesBtn.TabIndex = 11;
            this.DestroyCertificatesBtn.Text = "Destroy Certificates";
            this.DestroyCertificatesBtn.Click += new System.EventHandler(this.DestroyCertificatesBtn_Click);
            // 
            // ProxyPortLbl
            // 
            this.ProxyPortLbl.Location = new System.Drawing.Point(393, 272);
            this.ProxyPortLbl.Name = "ProxyPortLbl";
            this.ProxyPortLbl.Size = new System.Drawing.Size(90, 20);
            this.ProxyPortLbl.TabIndex = 12;
            this.ProxyPortLbl.Text = "Proxy Port: 8282";
            // 
            // Seperator2
            // 
            this.Seperator2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.Seperator2.Location = new System.Drawing.Point(3, 268);
            this.Seperator2.Name = "Seperator2";
            this.Seperator2.Size = new System.Drawing.Size(480, 1);
            this.Seperator2.TabIndex = 13;
            // 
            // ValueTxt
            // 
            this.ValueTxt.Location = new System.Drawing.Point(3, 219);
            this.ValueTxt.Name = "ValueTxt";
            this.ValueTxt.Size = new System.Drawing.Size(384, 20);
            this.ValueTxt.TabIndex = 14;
            this.ValueTxt.Text = "";
            this.ValueTxt.TextPaddingWidth = 23;
            this.ValueTxt.Title = "Value";
            // 
            // VariableTxt
            // 
            this.VariableTxt.IsReadOnly = true;
            this.VariableTxt.Location = new System.Drawing.Point(3, 245);
            this.VariableTxt.Name = "VariableTxt";
            this.VariableTxt.Size = new System.Drawing.Size(384, 20);
            this.VariableTxt.TabIndex = 15;
            this.VariableTxt.Text = "";
            this.VariableTxt.Title = "Variable";
            // 
            // ClearBtn
            // 
            this.ClearBtn.BackColor = System.Drawing.Color.Transparent;
            this.ClearBtn.Location = new System.Drawing.Point(393, 245);
            this.ClearBtn.Name = "ClearBtn";
            this.ClearBtn.Size = new System.Drawing.Size(90, 20);
            this.ClearBtn.TabIndex = 16;
            this.ClearBtn.Text = "Clear";
            this.ClearBtn.Click += new System.EventHandler(this.ClearBtn_Click);
            // 
            // UpdateBtn
            // 
            this.UpdateBtn.BackColor = System.Drawing.Color.Transparent;
            this.UpdateBtn.Location = new System.Drawing.Point(393, 219);
            this.UpdateBtn.Name = "UpdateBtn";
            this.UpdateBtn.Size = new System.Drawing.Size(90, 20);
            this.UpdateBtn.TabIndex = 17;
            this.UpdateBtn.Text = "Update";
            this.UpdateBtn.Click += new System.EventHandler(this.UpdateBtn_Click);
            // 
            // ConnectionPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.UpdateBtn);
            this.Controls.Add(this.ClearBtn);
            this.Controls.Add(this.VariableTxt);
            this.Controls.Add(this.ValueTxt);
            this.Controls.Add(this.Seperator2);
            this.Controls.Add(this.ProxyPortLbl);
            this.Controls.Add(this.DestroyCertificatesBtn);
            this.Controls.Add(this.ExportCertificateAuthorityBtn);
            this.Controls.Add(this.BrowseBtn);
            this.Controls.Add(this.CustomClientTxt);
            this.Controls.Add(this.Seperator3);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.ConnectBtn);
            this.Controls.Add(this.StatusTxt);
            this.Controls.Add(this.Seperator1);
            this.Controls.Add(this.AutomaticServerExtractionChbx);
            this.Controls.Add(this.HotelServerTxt);
            this.Controls.Add(this.VariablesLv);
            this.Name = "ConnectionPage";
            this.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(this.ConnectionPage_PropertyChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Tangine.Controls.TangineLabelBox HotelServerTxt;
        private System.Windows.Forms.CheckBox AutomaticServerExtractionChbx;
        private System.Windows.Forms.Label Seperator1;
        private Tangine.Controls.TangineListView VariablesLv;
        private Tangine.Controls.TangineLabel StatusTxt;
        private Tangine.Controls.TangineButton ConnectBtn;
        private Tangine.Controls.TangineButton CancelBtn;
        private System.Windows.Forms.Label Seperator3;
        private Tangine.Controls.TangineLabelBox CustomClientTxt;
        private Tangine.Controls.TangineButton BrowseBtn;
        private Tangine.Controls.TangineButton ExportCertificateAuthorityBtn;
        private Tangine.Controls.TangineButton DestroyCertificatesBtn;
        private Tangine.Controls.TangineLabel ProxyPortLbl;
        private System.Windows.Forms.Label Seperator2;
        private Tangine.Controls.TangineLabelBox ValueTxt;
        private Tangine.Controls.TangineLabelBox VariableTxt;
        private System.Windows.Forms.ColumnHeader VariableCol;
        private System.Windows.Forms.ColumnHeader ValueCol;
        private Tangine.Controls.TangineButton ClearBtn;
        private Tangine.Controls.TangineButton UpdateBtn;
    }
}
