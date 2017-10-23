namespace Tanji.Applications.Dialogs
{
    partial class IgnoreMessagesDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.IgnoredVw = new Sulakore.Components.SKoreListView();
            this.TypeCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.HeaderCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.AddHeaderBtn = new Sulakore.Components.SKoreButton();
            this.TypeTxt = new System.Windows.Forms.ComboBox();
            this.TypeLbl = new System.Windows.Forms.Label();
            this.HeaderTxt = new System.Windows.Forms.NumericUpDown();
            this.HeaderLbl = new System.Windows.Forms.Label();
            this.Glow1Pnl = new System.Windows.Forms.Panel();
            this.RemoveBtn = new Sulakore.Components.SKoreButton();
            ((System.ComponentModel.ISupportInitialize)(this.HeaderTxt)).BeginInit();
            this.SuspendLayout();
            // 
            // IgnoredVw
            // 
            this.IgnoredVw.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.IgnoredVw.CheckBoxes = true;
            this.IgnoredVw.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.TypeCol,
            this.HeaderCol});
            this.IgnoredVw.Dock = System.Windows.Forms.DockStyle.Right;
            this.IgnoredVw.FullRowSelect = true;
            this.IgnoredVw.GridLines = true;
            this.IgnoredVw.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.IgnoredVw.HideSelection = false;
            this.IgnoredVw.Location = new System.Drawing.Point(120, 0);
            this.IgnoredVw.MultiSelect = false;
            this.IgnoredVw.Name = "IgnoredVw";
            this.IgnoredVw.ShowItemToolTips = true;
            this.IgnoredVw.Size = new System.Drawing.Size(151, 153);
            this.IgnoredVw.TabIndex = 0;
            this.IgnoredVw.UseCompatibleStateImageBehavior = false;
            this.IgnoredVw.View = System.Windows.Forms.View.Details;
            // 
            // TypeCol
            // 
            this.TypeCol.Text = "Type";
            this.TypeCol.Width = 77;
            // 
            // HeaderCol
            // 
            this.HeaderCol.Text = "Header";
            this.HeaderCol.Width = 53;
            // 
            // AddHeaderBtn
            // 
            this.AddHeaderBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AddHeaderBtn.BackColor = System.Drawing.Color.Transparent;
            this.AddHeaderBtn.Location = new System.Drawing.Point(12, 91);
            this.AddHeaderBtn.Name = "AddHeaderBtn";
            this.AddHeaderBtn.Size = new System.Drawing.Size(101, 22);
            this.AddHeaderBtn.Skin = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.AddHeaderBtn.TabIndex = 1;
            this.AddHeaderBtn.Text = "Add Header";
            this.AddHeaderBtn.Click += new System.EventHandler(this.AddHeaderBtn_Click);
            // 
            // TypeTxt
            // 
            this.TypeTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TypeTxt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TypeTxt.FormattingEnabled = true;
            this.TypeTxt.Items.AddRange(new object[] {
            "Outgoing",
            "Incoming"});
            this.TypeTxt.Location = new System.Drawing.Point(12, 25);
            this.TypeTxt.Name = "TypeTxt";
            this.TypeTxt.Size = new System.Drawing.Size(101, 21);
            this.TypeTxt.TabIndex = 2;
            // 
            // TypeLbl
            // 
            this.TypeLbl.AutoSize = true;
            this.TypeLbl.Location = new System.Drawing.Point(9, 9);
            this.TypeLbl.Name = "TypeLbl";
            this.TypeLbl.Size = new System.Drawing.Size(31, 13);
            this.TypeLbl.TabIndex = 3;
            this.TypeLbl.Text = "Type";
            // 
            // HeaderTxt
            // 
            this.HeaderTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.HeaderTxt.Location = new System.Drawing.Point(12, 65);
            this.HeaderTxt.Maximum = new decimal(new int[] {
            4000,
            0,
            0,
            0});
            this.HeaderTxt.Name = "HeaderTxt";
            this.HeaderTxt.Size = new System.Drawing.Size(101, 20);
            this.HeaderTxt.TabIndex = 50;
            this.HeaderTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // HeaderLbl
            // 
            this.HeaderLbl.AutoSize = true;
            this.HeaderLbl.Location = new System.Drawing.Point(9, 49);
            this.HeaderLbl.Name = "HeaderLbl";
            this.HeaderLbl.Size = new System.Drawing.Size(42, 13);
            this.HeaderLbl.TabIndex = 51;
            this.HeaderLbl.Text = "Header";
            // 
            // Glow1Pnl
            // 
            this.Glow1Pnl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.Glow1Pnl.Dock = System.Windows.Forms.DockStyle.Right;
            this.Glow1Pnl.Location = new System.Drawing.Point(119, 0);
            this.Glow1Pnl.Name = "Glow1Pnl";
            this.Glow1Pnl.Size = new System.Drawing.Size(1, 153);
            this.Glow1Pnl.TabIndex = 52;
            // 
            // RemoveBtn
            // 
            this.RemoveBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RemoveBtn.BackColor = System.Drawing.Color.Transparent;
            this.RemoveBtn.Enabled = false;
            this.RemoveBtn.Location = new System.Drawing.Point(12, 119);
            this.RemoveBtn.Name = "RemoveBtn";
            this.RemoveBtn.Size = new System.Drawing.Size(101, 22);
            this.RemoveBtn.Skin = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.RemoveBtn.TabIndex = 53;
            this.RemoveBtn.Text = "Remove";
            this.RemoveBtn.Click += new System.EventHandler(this.RemoveBtn_Click);
            // 
            // IgnoreMessagesDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(271, 153);
            this.Controls.Add(this.RemoveBtn);
            this.Controls.Add(this.Glow1Pnl);
            this.Controls.Add(this.HeaderLbl);
            this.Controls.Add(this.HeaderTxt);
            this.Controls.Add(this.TypeLbl);
            this.Controls.Add(this.TypeTxt);
            this.Controls.Add(this.AddHeaderBtn);
            this.Controls.Add(this.IgnoredVw);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "IgnoreMessagesDialog";
            this.Text = "Tanji ~ Ignore Messages";
            ((System.ComponentModel.ISupportInitialize)(this.HeaderTxt)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Sulakore.Components.SKoreListView IgnoredVw;
        private System.Windows.Forms.ColumnHeader TypeCol;
        private System.Windows.Forms.ColumnHeader HeaderCol;
        private Sulakore.Components.SKoreButton AddHeaderBtn;
        private System.Windows.Forms.ComboBox TypeTxt;
        private System.Windows.Forms.Label TypeLbl;
        internal System.Windows.Forms.NumericUpDown HeaderTxt;
        private System.Windows.Forms.Label HeaderLbl;
        private System.Windows.Forms.Panel Glow1Pnl;
        private Sulakore.Components.SKoreButton RemoveBtn;
    }
}