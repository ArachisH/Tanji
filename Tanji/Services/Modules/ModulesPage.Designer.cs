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
            this.Seperator1 = new System.Windows.Forms.Label();
            this.UninstallModuleBtn = new Tangine.Controls.TangineButton();
            this.InstallModuleBtn = new Tangine.Controls.TangineButton();
            this.ModuleServerPortTxt = new Tangine.Controls.TangineLabelBox();
            this.ModulesLv = new Tangine.Controls.TangineListView();
            this.NameCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DescriptionCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.VersionCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.StateCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
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
            this.AvatarPct.Location = new System.Drawing.Point(0, 212);
            this.AvatarPct.Name = "AvatarPct";
            this.AvatarPct.Size = new System.Drawing.Size(65, 103);
            this.AvatarPct.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.AvatarPct.TabIndex = 1;
            this.AvatarPct.TabStop = false;
            // 
            // AuthorsLbl
            // 
            this.AuthorsLbl.AutoSize = true;
            this.AuthorsLbl.Location = new System.Drawing.Point(68, 232);
            this.AuthorsLbl.Name = "AuthorsLbl";
            this.AuthorsLbl.Size = new System.Drawing.Size(49, 13);
            this.AuthorsLbl.TabIndex = 9;
            this.AuthorsLbl.Text = "Author(s)";
            // 
            // AuthorsCmb
            // 
            this.AuthorsCmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AuthorsCmb.Enabled = false;
            this.AuthorsCmb.FormattingEnabled = true;
            this.AuthorsCmb.Location = new System.Drawing.Point(71, 248);
            this.AuthorsCmb.Name = "AuthorsCmb";
            this.AuthorsCmb.Size = new System.Drawing.Size(127, 21);
            this.AuthorsCmb.TabIndex = 8;
            // 
            // Seperator1
            // 
            this.Seperator1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.Seperator1.Location = new System.Drawing.Point(71, 287);
            this.Seperator1.Name = "Seperator1";
            this.Seperator1.Size = new System.Drawing.Size(430, 1);
            this.Seperator1.TabIndex = 18;
            // 
            // UninstallModuleBtn
            // 
            this.UninstallModuleBtn.Enabled = false;
            this.UninstallModuleBtn.Location = new System.Drawing.Point(241, 291);
            this.UninstallModuleBtn.Name = "UninstallModuleBtn";
            this.UninstallModuleBtn.Size = new System.Drawing.Size(127, 20);
            this.UninstallModuleBtn.TabIndex = 4;
            this.UninstallModuleBtn.Text = "Uninstall Module";
            this.UninstallModuleBtn.Click += new System.EventHandler(this.UninstallModuleBtn_Click);
            // 
            // InstallModuleBtn
            // 
            this.InstallModuleBtn.Location = new System.Drawing.Point(374, 291);
            this.InstallModuleBtn.Name = "InstallModuleBtn";
            this.InstallModuleBtn.Size = new System.Drawing.Size(127, 20);
            this.InstallModuleBtn.TabIndex = 3;
            this.InstallModuleBtn.Text = "Install Module";
            this.InstallModuleBtn.Click += new System.EventHandler(this.InstallModuleBtn_Click);
            // 
            // ModuleServerPortTxt
            // 
            this.ModuleServerPortTxt.IsReadOnly = true;
            this.ModuleServerPortTxt.Location = new System.Drawing.Point(71, 291);
            this.ModuleServerPortTxt.Name = "ModuleServerPortTxt";
            this.ModuleServerPortTxt.Size = new System.Drawing.Size(164, 20);
            this.ModuleServerPortTxt.TabIndex = 2;
            this.ModuleServerPortTxt.Text = "8055";
            this.ModuleServerPortTxt.TextPaddingWidth = 0;
            this.ModuleServerPortTxt.Title = "Module Server Port";
            // 
            // ModulesLv
            // 
            this.ModulesLv.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.NameCol,
            this.DescriptionCol,
            this.VersionCol,
            this.StateCol});
            this.ModulesLv.Dock = System.Windows.Forms.DockStyle.Top;
            this.ModulesLv.FullRowSelect = true;
            this.ModulesLv.GridLines = true;
            this.ModulesLv.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.ModulesLv.HideSelection = false;
            this.ModulesLv.Location = new System.Drawing.Point(0, 0);
            this.ModulesLv.MultiSelect = false;
            this.ModulesLv.Name = "ModulesLv";
            this.ModulesLv.ShowItemToolTips = true;
            this.ModulesLv.Size = new System.Drawing.Size(506, 212);
            this.ModulesLv.TabIndex = 0;
            this.ModulesLv.UseCompatibleStateImageBehavior = false;
            this.ModulesLv.View = System.Windows.Forms.View.Details;
            this.ModulesLv.ItemSelectionStateChanged += new System.EventHandler(this.ModulesLv_ItemSelectionStateChanged);
            this.ModulesLv.ItemActivate += new System.EventHandler(this.ModulesLv_ItemActivate);
            // 
            // NameCol
            // 
            this.NameCol.Text = "Name";
            this.NameCol.Width = 100;
            // 
            // DescriptionCol
            // 
            this.DescriptionCol.Text = "Description";
            this.DescriptionCol.Width = 243;
            // 
            // VersionCol
            // 
            this.VersionCol.Text = "Version";
            this.VersionCol.Width = 70;
            // 
            // StateCol
            // 
            this.StateCol.Text = "State";
            this.StateCol.Width = 70;
            // 
            // ModulesPage
            // 
            this.Font = Program.DefaultFont;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Seperator1);
            this.Controls.Add(this.AuthorsLbl);
            this.Controls.Add(this.AuthorsCmb);
            this.Controls.Add(this.UninstallModuleBtn);
            this.Controls.Add(this.InstallModuleBtn);
            this.Controls.Add(this.ModuleServerPortTxt);
            this.Controls.Add(this.AvatarPct);
            this.Controls.Add(this.ModulesLv);
            this.Name = "ModulesPage";
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
        private Tangine.Controls.TangineLabelBox ModuleServerPortTxt;
        private Tangine.Controls.TangineButton InstallModuleBtn;
        private Tangine.Controls.TangineButton UninstallModuleBtn;
        private System.Windows.Forms.Label AuthorsLbl;
        internal System.Windows.Forms.ComboBox AuthorsCmb;
        private System.Windows.Forms.Label Seperator1;
    }
}
