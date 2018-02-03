namespace Tanji.Windows.Dialogs
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
            this.IgnoreBtn = new Sulakore.Components.SKoreButton();
            this.TypeTxt = new System.Windows.Forms.ComboBox();
            this.HeaderTxt = new System.Windows.Forms.NumericUpDown();
            this.HeaderLbl = new System.Windows.Forms.Label();
            this.RemoveBtn = new Sulakore.Components.SKoreButton();
            this.Glow1Pnl = new System.Windows.Forms.Panel();
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
            this.IgnoredVw.Dock = System.Windows.Forms.DockStyle.Top;
            this.IgnoredVw.FullRowSelect = true;
            this.IgnoredVw.GridLines = true;
            this.IgnoredVw.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.IgnoredVw.HideSelection = false;
            this.IgnoredVw.Location = new System.Drawing.Point(0, 0);
            this.IgnoredVw.MultiSelect = false;
            this.IgnoredVw.Name = "IgnoredVw";
            this.IgnoredVw.ShowItemToolTips = true;
            this.IgnoredVw.Size = new System.Drawing.Size(210, 153);
            this.IgnoredVw.TabIndex = 0;
            this.IgnoredVw.UseCompatibleStateImageBehavior = false;
            this.IgnoredVw.View = System.Windows.Forms.View.Details;
            this.IgnoredVw.ItemSelectionStateChanged += new System.EventHandler(this.IgnoredVw_ItemSelectionStateChanged);
            this.IgnoredVw.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.IgnoredVw_ItemChecked);
            // 
            // TypeCol
            // 
            this.TypeCol.Text = "Type";
            this.TypeCol.Width = 85;
            // 
            // HeaderCol
            // 
            this.HeaderCol.Text = "Header";
            this.HeaderCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.HeaderCol.Width = 108;
            // 
            // IgnoreBtn
            // 
            this.IgnoreBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.IgnoreBtn.Location = new System.Drawing.Point(123, 160);
            this.IgnoreBtn.Name = "IgnoreBtn";
            this.IgnoreBtn.Size = new System.Drawing.Size(75, 20);
            this.IgnoreBtn.TabIndex = 1;
            this.IgnoreBtn.Text = "Ignore";
            this.IgnoreBtn.Click += new System.EventHandler(this.IgnoreBtn_Click);
            // 
            // TypeTxt
            // 
            this.TypeTxt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TypeTxt.FormattingEnabled = true;
            this.TypeTxt.ItemHeight = 13;
            this.TypeTxt.Items.AddRange(new object[] {
            "Outgoing",
            "Incoming"});
            this.TypeTxt.Location = new System.Drawing.Point(12, 185);
            this.TypeTxt.Name = "TypeTxt";
            this.TypeTxt.Size = new System.Drawing.Size(105, 21);
            this.TypeTxt.TabIndex = 2;
            // 
            // HeaderTxt
            // 
            this.HeaderTxt.Location = new System.Drawing.Point(57, 160);
            this.HeaderTxt.Maximum = new decimal(new int[] {
            4000,
            0,
            0,
            0});
            this.HeaderTxt.Name = "HeaderTxt";
            this.HeaderTxt.Size = new System.Drawing.Size(60, 20);
            this.HeaderTxt.TabIndex = 50;
            this.HeaderTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.HeaderTxt.Value = new decimal(new int[] {
            4000,
            0,
            0,
            0});
            // 
            // HeaderLbl
            // 
            this.HeaderLbl.AutoSize = true;
            this.HeaderLbl.Location = new System.Drawing.Point(12, 163);
            this.HeaderLbl.Name = "HeaderLbl";
            this.HeaderLbl.Size = new System.Drawing.Size(45, 13);
            this.HeaderLbl.TabIndex = 51;
            this.HeaderLbl.Text = "Header:";
            // 
            // RemoveBtn
            // 
            this.RemoveBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.RemoveBtn.Enabled = false;
            this.RemoveBtn.Location = new System.Drawing.Point(123, 186);
            this.RemoveBtn.Name = "RemoveBtn";
            this.RemoveBtn.Size = new System.Drawing.Size(75, 20);
            this.RemoveBtn.TabIndex = 53;
            this.RemoveBtn.Text = "Remove";
            this.RemoveBtn.Click += new System.EventHandler(this.RemoveBtn_Click);
            // 
            // Glow1Pnl
            // 
            this.Glow1Pnl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.Glow1Pnl.Dock = System.Windows.Forms.DockStyle.Top;
            this.Glow1Pnl.Location = new System.Drawing.Point(0, 153);
            this.Glow1Pnl.Name = "Glow1Pnl";
            this.Glow1Pnl.Size = new System.Drawing.Size(210, 1);
            this.Glow1Pnl.TabIndex = 52;
            // 
            // IgnoreMessagesDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(210, 218);
            this.Controls.Add(this.HeaderLbl);
            this.Controls.Add(this.RemoveBtn);
            this.Controls.Add(this.Glow1Pnl);
            this.Controls.Add(this.HeaderTxt);
            this.Controls.Add(this.TypeTxt);
            this.Controls.Add(this.IgnoreBtn);
            this.Controls.Add(this.IgnoredVw);
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
        private Sulakore.Components.SKoreButton IgnoreBtn;
        private System.Windows.Forms.ComboBox TypeTxt;
        internal System.Windows.Forms.NumericUpDown HeaderTxt;
        private System.Windows.Forms.Label HeaderLbl;
        private Sulakore.Components.SKoreButton RemoveBtn;
        private System.Windows.Forms.Panel Glow1Pnl;
    }
}