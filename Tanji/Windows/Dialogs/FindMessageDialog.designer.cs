namespace Tanji.Windows.Dialogs
{
    partial class FindMessageDialog
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
            this.HashLbl = new System.Windows.Forms.Label();
            this.HashTxt = new System.Windows.Forms.TextBox();
            this.FindBtn = new Sulakore.Components.SKoreButton();
            this.HeadersVw = new Sulakore.Components.SKoreListView();
            this.TypeCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.NameCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ClassNameCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ParserNameCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Glow1Pnl = new System.Windows.Forms.Panel();
            this.IdNum = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.IDCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            ((System.ComponentModel.ISupportInitialize)(this.IdNum)).BeginInit();
            this.SuspendLayout();
            // 
            // HashLbl
            // 
            this.HashLbl.AutoSize = true;
            this.HashLbl.Location = new System.Drawing.Point(100, 15);
            this.HashLbl.Name = "HashLbl";
            this.HashLbl.Size = new System.Drawing.Size(35, 13);
            this.HashLbl.TabIndex = 0;
            this.HashLbl.Text = "Hash:";
            // 
            // HashTxt
            // 
            this.HashTxt.Location = new System.Drawing.Point(135, 12);
            this.HashTxt.Name = "HashTxt";
            this.HashTxt.Size = new System.Drawing.Size(243, 20);
            this.HashTxt.TabIndex = 1;
            this.HashTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FindBtn
            // 
            this.FindBtn.BackColor = System.Drawing.Color.Transparent;
            this.FindBtn.Location = new System.Drawing.Point(384, 12);
            this.FindBtn.Name = "FindBtn";
            this.FindBtn.Size = new System.Drawing.Size(66, 20);
            this.FindBtn.Skin = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.FindBtn.TabIndex = 2;
            this.FindBtn.Text = "Find";
            this.FindBtn.Click += new System.EventHandler(this.FindBtn_Click);
            // 
            // HeadersVw
            // 
            this.HeadersVw.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.HeadersVw.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.TypeCol,
            this.IDCol,
            this.NameCol,
            this.ClassNameCol,
            this.ParserNameCol});
            this.HeadersVw.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.HeadersVw.FullRowSelect = true;
            this.HeadersVw.GridLines = true;
            this.HeadersVw.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.HeadersVw.HideSelection = false;
            this.HeadersVw.Location = new System.Drawing.Point(0, 39);
            this.HeadersVw.MultiSelect = false;
            this.HeadersVw.Name = "HeadersVw";
            this.HeadersVw.ShowItemToolTips = true;
            this.HeadersVw.Size = new System.Drawing.Size(462, 126);
            this.HeadersVw.TabIndex = 3;
            this.HeadersVw.UseCompatibleStateImageBehavior = false;
            this.HeadersVw.View = System.Windows.Forms.View.Details;
            this.HeadersVw.ItemSelected += new System.EventHandler(this.HeadersVw_ItemSelected);
            // 
            // TypeCol
            // 
            this.TypeCol.Text = "Type";
            // 
            // NameCol
            // 
            this.NameCol.Text = "Name";
            this.NameCol.Width = 125;
            // 
            // ClassNameCol
            // 
            this.ClassNameCol.Text = "Class Name";
            this.ClassNameCol.Width = 116;
            // 
            // ParserNameCol
            // 
            this.ParserNameCol.Text = "Parser Name";
            this.ParserNameCol.Width = 116;
            // 
            // Glow1Pnl
            // 
            this.Glow1Pnl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.Glow1Pnl.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Glow1Pnl.Location = new System.Drawing.Point(0, 38);
            this.Glow1Pnl.Name = "Glow1Pnl";
            this.Glow1Pnl.Size = new System.Drawing.Size(462, 1);
            this.Glow1Pnl.TabIndex = 4;
            // 
            // IdNum
            // 
            this.IdNum.Location = new System.Drawing.Point(33, 12);
            this.IdNum.Maximum = new decimal(new int[] {
            4000,
            0,
            0,
            0});
            this.IdNum.Name = "IdNum";
            this.IdNum.Size = new System.Drawing.Size(61, 20);
            this.IdNum.TabIndex = 5;
            this.IdNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "ID:";
            // 
            // IDCol
            // 
            this.IDCol.Text = "ID";
            this.IDCol.Width = 45;
            // 
            // FindMessageDialog
            // 
            this.AcceptButton = this.FindBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 165);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.IdNum);
            this.Controls.Add(this.Glow1Pnl);
            this.Controls.Add(this.HashLbl);
            this.Controls.Add(this.HeadersVw);
            this.Controls.Add(this.FindBtn);
            this.Controls.Add(this.HashTxt);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FindMessageDialog";
            this.ShowInTaskbar = false;
            this.Text = "Tanji ~ Find Message";
            ((System.ComponentModel.ISupportInitialize)(this.IdNum)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label HashLbl;
        private Sulakore.Components.SKoreButton FindBtn;
        private Sulakore.Components.SKoreListView HeadersVw;
        private System.Windows.Forms.ColumnHeader TypeCol;
        private System.Windows.Forms.ColumnHeader ClassNameCol;
        internal System.Windows.Forms.TextBox HashTxt;
        private System.Windows.Forms.Panel Glow1Pnl;
        private System.Windows.Forms.NumericUpDown IdNum;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ColumnHeader NameCol;
        private System.Windows.Forms.ColumnHeader ParserNameCol;
        private System.Windows.Forms.ColumnHeader IDCol;
    }
}