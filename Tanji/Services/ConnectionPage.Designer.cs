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
            this.ChooseClientDlg = new System.Windows.Forms.OpenFileDialog();
            this.ExportCertificateDlg = new System.Windows.Forms.SaveFileDialog();
            this.DestroyCertificatesBtn = new Tangine.Controls.TangineButton();
            this.ExportCertificateAuthorityBtn = new Tangine.Controls.TangineButton();
            this.BrowseBtn = new Tangine.Controls.TangineButton();
            this.CustomClientTxt = new Tangine.Controls.TangineLabelBox();
            this.ConnectBtn = new Tangine.Controls.TangineButton();
            this.StatusTxt = new Tangine.Controls.TangineLabel();
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
            // DestroyCertificatesBtn
            // 
            this.DestroyCertificatesBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.DestroyCertificatesBtn.Location = new System.Drawing.Point(256, 239);
            this.DestroyCertificatesBtn.Name = "DestroyCertificatesBtn";
            this.DestroyCertificatesBtn.Size = new System.Drawing.Size(247, 20);
            this.DestroyCertificatesBtn.TabIndex = 11;
            this.DestroyCertificatesBtn.TabStop = false;
            this.DestroyCertificatesBtn.Text = "Destroy Certificates";
            this.DestroyCertificatesBtn.Click += new System.EventHandler(this.DestroyCertificatesBtn_Click);
            // 
            // ExportCertificateAuthorityBtn
            // 
            this.ExportCertificateAuthorityBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ExportCertificateAuthorityBtn.Location = new System.Drawing.Point(3, 239);
            this.ExportCertificateAuthorityBtn.Name = "ExportCertificateAuthorityBtn";
            this.ExportCertificateAuthorityBtn.Size = new System.Drawing.Size(247, 20);
            this.ExportCertificateAuthorityBtn.TabIndex = 10;
            this.ExportCertificateAuthorityBtn.TabStop = false;
            this.ExportCertificateAuthorityBtn.Text = "Export Certificate Authority";
            this.ExportCertificateAuthorityBtn.Click += new System.EventHandler(this.ExportCertificateAuthorityBtn_Click);
            // 
            // BrowseBtn
            // 
            this.BrowseBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BrowseBtn.Location = new System.Drawing.Point(413, 265);
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
            this.CustomClientTxt.Location = new System.Drawing.Point(3, 265);
            this.CustomClientTxt.Name = "CustomClientTxt";
            this.CustomClientTxt.Size = new System.Drawing.Size(404, 20);
            this.CustomClientTxt.TabIndex = 0;
            this.CustomClientTxt.TabStop = false;
            this.CustomClientTxt.Text = "";
            this.CustomClientTxt.TextPaddingWidth = 0;
            this.CustomClientTxt.Title = "Custom Client";
            // 
            // ConnectBtn
            // 
            this.ConnectBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ConnectBtn.Location = new System.Drawing.Point(413, 291);
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
            this.StatusTxt.Location = new System.Drawing.Point(3, 291);
            this.StatusTxt.Name = "StatusTxt";
            this.StatusTxt.Size = new System.Drawing.Size(404, 21);
            this.StatusTxt.TabIndex = 4;
            this.StatusTxt.TabStop = false;
            this.StatusTxt.Text = "Standing By...";
            // 
            // ConnectionPage
            // 
            this.Font = Program.DefaultFont;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.DestroyCertificatesBtn);
            this.Controls.Add(this.ExportCertificateAuthorityBtn);
            this.Controls.Add(this.BrowseBtn);
            this.Controls.Add(this.CustomClientTxt);
            this.Controls.Add(this.ConnectBtn);
            this.Controls.Add(this.StatusTxt);
            this.Name = "ConnectionPage";
            this.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(this.ConnectionPage_PropertyChanged);
            this.ResumeLayout(false);

        }

        #endregion
        private Tangine.Controls.TangineLabel StatusTxt;
        private Tangine.Controls.TangineButton ConnectBtn;
        private Tangine.Controls.TangineLabelBox CustomClientTxt;
        private Tangine.Controls.TangineButton BrowseBtn;
        private Tangine.Controls.TangineButton ExportCertificateAuthorityBtn;
        private Tangine.Controls.TangineButton DestroyCertificatesBtn;
        private System.Windows.Forms.OpenFileDialog ChooseClientDlg;
        private System.Windows.Forms.SaveFileDialog ExportCertificateDlg;
    }
}
