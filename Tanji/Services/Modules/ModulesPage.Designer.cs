namespace Tanji.Services.Modules
{
    partial class ModulesPage
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
            this.InstallModuleDlg = new System.Windows.Forms.OpenFileDialog();
            this.AvatarPct = new System.Windows.Forms.PictureBox();
            this.AuthorsLbl = new System.Windows.Forms.Label();
            this.AuthorsCmb = new System.Windows.Forms.ComboBox();
            this.UninstallModuleBtn = new Tangine.Controls.TangineButton();
            this.InstallModuleBtn = new Tangine.Controls.TangineButton();
            this.ModulesLv = new Tangine.Controls.TangineListView();
            this.NameCol = new System.Windows.Forms.ColumnHeader();
            this.DescriptionCol = new System.Windows.Forms.ColumnHeader();
            this.VersionCol = new System.Windows.Forms.ColumnHeader();
            this.StateCol = new System.Windows.Forms.ColumnHeader();
            ((System.ComponentModel.ISupportInitialize)(this.AvatarPct)).BeginInit();
            this.SuspendLayout();
            // 
            // InstallModuleDlg
            // 
            this.InstallModuleDlg.Filter = ".NET Assembly (*.dll, *.exe)|*.dll; *.exe|Dynamic Link Library (*.dll)|*.dll|Exec" +
    "utable (*.exe)|*.exe";
            this.InstallModuleDlg.Title = "Tanji - Install Module";
            // 
            // AvatarPct
            // 
            this.AvatarPct.Dock = System.Windows.Forms.DockStyle.Left;
            this.AvatarPct.ErrorImage = null;
            this.AvatarPct.InitialImage = null;
            this.AvatarPct.Location = new System.Drawing.Point(0, 199);
            this.AvatarPct.Name = "AvatarPct";
            this.AvatarPct.Size = new System.Drawing.Size(65, 114);
            this.AvatarPct.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.AvatarPct.TabIndex = 1;
            this.AvatarPct.TabStop = false;
            // 
            // AuthorsLbl
            // 
            this.AuthorsLbl.AutoSize = true;
            this.AuthorsLbl.Location = new System.Drawing.Point(298, 209);
            this.AuthorsLbl.Name = "AuthorsLbl";
            this.AuthorsLbl.Size = new System.Drawing.Size(57, 15);
            this.AuthorsLbl.TabIndex = 9;
            this.AuthorsLbl.Text = "Author(s)";
            // 
            // AuthorsCmb
            // 
            this.AuthorsCmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AuthorsCmb.Enabled = false;
            this.AuthorsCmb.FormattingEnabled = true;
            this.AuthorsCmb.ItemHeight = 15;
            this.AuthorsCmb.Location = new System.Drawing.Point(298, 227);
            this.AuthorsCmb.Name = "AuthorsCmb";
            this.AuthorsCmb.Size = new System.Drawing.Size(173, 23);
            this.AuthorsCmb.TabIndex = 8;
            // 
            // UninstallModuleBtn
            // 
            this.UninstallModuleBtn.Enabled = false;
            this.UninstallModuleBtn.Location = new System.Drawing.Point(298, 261);
            this.UninstallModuleBtn.Name = "UninstallModuleBtn";
            this.UninstallModuleBtn.Size = new System.Drawing.Size(173, 20);
            this.UninstallModuleBtn.TabIndex = 4;
            this.UninstallModuleBtn.Text = "Uninstall Module";
            this.UninstallModuleBtn.Click += new System.EventHandler(this.UninstallModuleBtn_Click);
            // 
            // InstallModuleBtn
            // 
            this.InstallModuleBtn.Location = new System.Drawing.Point(298, 287);
            this.InstallModuleBtn.Name = "InstallModuleBtn";
            this.InstallModuleBtn.Size = new System.Drawing.Size(173, 20);
            this.InstallModuleBtn.TabIndex = 3;
            this.InstallModuleBtn.Text = "Install Module";
            this.InstallModuleBtn.Click += new System.EventHandler(this.InstallModuleBtn_Click);
            // 
            // ModulesLv
            // 
            this.ModulesLv.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.NameCol,
            this.DescriptionCol,
            this.VersionCol,
            this.StateCol});
            this.ModulesLv.Dock = System.Windows.Forms.DockStyle.Top;
            this.ModulesLv.FillColumnIndex = 1;
            this.ModulesLv.FullRowSelect = true;
            this.ModulesLv.GridLines = true;
            this.ModulesLv.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.ModulesLv.HideSelection = false;
            this.ModulesLv.Location = new System.Drawing.Point(0, 0);
            this.ModulesLv.MultiSelect = false;
            this.ModulesLv.Name = "ModulesLv";
            this.ModulesLv.OwnerDraw = true;
            this.ModulesLv.ShowItemToolTips = true;
            this.ModulesLv.Size = new System.Drawing.Size(476, 199);
            this.ModulesLv.TabIndex = 0;
            this.ModulesLv.UseCompatibleStateImageBehavior = false;
            this.ModulesLv.View = System.Windows.Forms.View.Details;
            this.ModulesLv.ItemSelectionStateChanged += new System.EventHandler(this.ModulesLv_ItemSelectionStateChanged);
            this.ModulesLv.ItemActivate += new System.EventHandler(this.ModulesLv_ItemActivate);
            // 
            // NameCol
            // 
            this.NameCol.Name = "NameCol";
            this.NameCol.Text = "Name";
            this.NameCol.Width = 200;
            // 
            // DescriptionCol
            // 
            this.DescriptionCol.Name = "DescriptionCol";
            this.DescriptionCol.Text = "Description";
            this.DescriptionCol.Width = 171;
            // 
            // VersionCol
            // 
            this.VersionCol.Name = "VersionCol";
            this.VersionCol.Text = "Version";
            this.VersionCol.Width = 66;
            // 
            // StateCol
            // 
            this.StateCol.Name = "StateCol";
            this.StateCol.Text = "State";
            this.StateCol.Width = 65;
            // 
            // ModulesPage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.AuthorsLbl);
            this.Controls.Add(this.AuthorsCmb);
            this.Controls.Add(this.UninstallModuleBtn);
            this.Controls.Add(this.InstallModuleBtn);
            this.Controls.Add(this.AvatarPct);
            this.Controls.Add(this.ModulesLv);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ModulesPage";
            this.Size = new System.Drawing.Size(476, 313);
            ((System.ComponentModel.ISupportInitialize)(this.AvatarPct)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog InstallModuleDlg;
        private Tangine.Controls.TangineListView ModulesLv;
        private System.Windows.Forms.ColumnHeader NameCol;
        private System.Windows.Forms.ColumnHeader DescriptionCol;
        private System.Windows.Forms.ColumnHeader VersionCol;
        private System.Windows.Forms.ColumnHeader StateCol;
        private System.Windows.Forms.PictureBox AvatarPct;
        private Tangine.Controls.TangineButton InstallModuleBtn;
        private Tangine.Controls.TangineButton UninstallModuleBtn;
        private System.Windows.Forms.Label AuthorsLbl;
        internal System.Windows.Forms.ComboBox AuthorsCmb;
    }
}
