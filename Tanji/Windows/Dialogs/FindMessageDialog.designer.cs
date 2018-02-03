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
            this.FindBtn = new Sulakore.Components.SKoreButton();
            this.HeadersVw = new Sulakore.Components.SKoreListView();
            this.TypeCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.IDCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.NameCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ClassNameCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ParserNameCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Glow1Pnl = new System.Windows.Forms.Panel();
            this.HashTxt = new Sulakore.Components.SKoreLabelBox();
            this.SuspendLayout();
            // 
            // FindBtn
            // 
            this.FindBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.FindBtn.Location = new System.Drawing.Point(384, 12);
            this.FindBtn.Name = "FindBtn";
            this.FindBtn.Size = new System.Drawing.Size(66, 20);
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
            // IDCol
            // 
            this.IDCol.Text = "ID";
            this.IDCol.Width = 45;
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
            // HashTxt
            // 
            this.HashTxt.Location = new System.Drawing.Point(12, 12);
            this.HashTxt.Name = "HashTxt";
            this.HashTxt.Size = new System.Drawing.Size(366, 20);
            this.HashTxt.TabIndex = 7;
            this.HashTxt.Text = "";
            this.HashTxt.Title = "Hash";
            this.HashTxt.Value = "";
            this.HashTxt.ValueAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.HashTxt.ValueReadOnly = false;
            // 
            // FindMessageDialog
            // 
            this.AcceptButton = this.FindBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 165);
            this.Controls.Add(this.HashTxt);
            this.Controls.Add(this.Glow1Pnl);
            this.Controls.Add(this.HeadersVw);
            this.Controls.Add(this.FindBtn);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FindMessageDialog";
            this.ShowInTaskbar = false;
            this.Text = "Tanji ~ Find Message";
            this.ResumeLayout(false);

        }

        #endregion
        private Sulakore.Components.SKoreButton FindBtn;
        private Sulakore.Components.SKoreListView HeadersVw;
        private System.Windows.Forms.ColumnHeader TypeCol;
        private System.Windows.Forms.ColumnHeader ClassNameCol;
        private System.Windows.Forms.Panel Glow1Pnl;
        private System.Windows.Forms.ColumnHeader NameCol;
        private System.Windows.Forms.ColumnHeader ParserNameCol;
        private System.Windows.Forms.ColumnHeader IDCol;
        private Sulakore.Components.SKoreLabelBox HashTxt;
    }
}