namespace Tanji.Services.Injection
{
    partial class FiltersPage
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
            this.ReplacementTxt = new Tangine.Controls.TangineLabelBox();
            this.DestinationLbl = new System.Windows.Forms.Label();
            this.DestinationTxt = new System.Windows.Forms.ComboBox();
            this.HeaderLbl = new System.Windows.Forms.Label();
            this.HeaderTxt = new System.Windows.Forms.NumericUpDown();
            this.RemoveBtn = new Tangine.Controls.TangineButton();
            this.CreateBtn = new Tangine.Controls.TangineButton();
            this.ActionLbl = new System.Windows.Forms.Label();
            this.ActionTxt = new System.Windows.Forms.ComboBox();
            this.FiltersVw = new Tangine.Controls.TangineListView();
            this.HeaderCol = new System.Windows.Forms.ColumnHeader();
            this.DestinationCol = new System.Windows.Forms.ColumnHeader();
            this.ActionCol = new System.Windows.Forms.ColumnHeader();
            this.ReplacementCol = new System.Windows.Forms.ColumnHeader();
            ((System.ComponentModel.ISupportInitialize)(this.HeaderTxt)).BeginInit();
            this.SuspendLayout();
            // 
            // ReplacementTxt
            // 
            this.ReplacementTxt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.ReplacementTxt.IsReadOnly = true;
            this.ReplacementTxt.Location = new System.Drawing.Point(6, 241);
            this.ReplacementTxt.Name = "ReplacementTxt";
            this.ReplacementTxt.Size = new System.Drawing.Size(385, 20);
            this.ReplacementTxt.TabIndex = 0;
            this.ReplacementTxt.Text = "";
            this.ReplacementTxt.TextPaddingWidth = 0;
            this.ReplacementTxt.Title = "Replacement";
            // 
            // DestinationLbl
            // 
            this.DestinationLbl.AutoSize = true;
            this.DestinationLbl.BackColor = System.Drawing.Color.Transparent;
            this.DestinationLbl.Location = new System.Drawing.Point(66, 198);
            this.DestinationLbl.Name = "DestinationLbl";
            this.DestinationLbl.Size = new System.Drawing.Size(60, 13);
            this.DestinationLbl.TabIndex = 2;
            this.DestinationLbl.Text = "Destination";
            // 
            // DestinationTxt
            // 
            this.DestinationTxt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DestinationTxt.DropDownWidth = 66;
            this.DestinationTxt.FormattingEnabled = true;
            this.DestinationTxt.Location = new System.Drawing.Point(69, 214);
            this.DestinationTxt.Name = "DestinationTxt";
            this.DestinationTxt.Size = new System.Drawing.Size(66, 21);
            this.DestinationTxt.TabIndex = 4;
            // 
            // HeaderLbl
            // 
            this.HeaderLbl.AutoSize = true;
            this.HeaderLbl.BackColor = System.Drawing.Color.Transparent;
            this.HeaderLbl.Location = new System.Drawing.Point(3, 198);
            this.HeaderLbl.Name = "HeaderLbl";
            this.HeaderLbl.Size = new System.Drawing.Size(42, 13);
            this.HeaderLbl.TabIndex = 10;
            this.HeaderLbl.Text = "Header";
            // 
            // HeaderTxt
            // 
            this.HeaderTxt.Location = new System.Drawing.Point(6, 215);
            this.HeaderTxt.Margin = new System.Windows.Forms.Padding(3, 4, 3, 3);
            this.HeaderTxt.Maximum = new decimal(new int[] {
            4000,
            0,
            0,
            0});
            this.HeaderTxt.Name = "HeaderTxt";
            this.HeaderTxt.Size = new System.Drawing.Size(57, 20);
            this.HeaderTxt.TabIndex = 11;
            this.HeaderTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // RemoveBtn
            // 
            this.RemoveBtn.Enabled = false;
            this.RemoveBtn.Location = new System.Drawing.Point(305, 214);
            this.RemoveBtn.Name = "RemoveBtn";
            this.RemoveBtn.Size = new System.Drawing.Size(86, 20);
            this.RemoveBtn.TabIndex = 13;
            this.RemoveBtn.Text = "Remove";
            this.RemoveBtn.Click += new System.EventHandler(this.RemoveBtn_Click);
            // 
            // CreateBtn
            // 
            this.CreateBtn.Location = new System.Drawing.Point(213, 214);
            this.CreateBtn.Name = "CreateBtn";
            this.CreateBtn.Size = new System.Drawing.Size(86, 20);
            this.CreateBtn.TabIndex = 14;
            this.CreateBtn.Text = "Create";
            this.CreateBtn.Click += new System.EventHandler(this.CreateBtn_Click);
            // 
            // ActionLbl
            // 
            this.ActionLbl.AutoSize = true;
            this.ActionLbl.BackColor = System.Drawing.Color.Transparent;
            this.ActionLbl.Location = new System.Drawing.Point(138, 198);
            this.ActionLbl.Name = "ActionLbl";
            this.ActionLbl.Size = new System.Drawing.Size(37, 13);
            this.ActionLbl.TabIndex = 16;
            this.ActionLbl.Text = "Action";
            // 
            // ActionTxt
            // 
            this.ActionTxt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ActionTxt.DropDownWidth = 66;
            this.ActionTxt.FormattingEnabled = true;
            this.ActionTxt.Location = new System.Drawing.Point(141, 214);
            this.ActionTxt.Name = "ActionTxt";
            this.ActionTxt.Size = new System.Drawing.Size(66, 21);
            this.ActionTxt.TabIndex = 18;
            // 
            // FiltersVw
            // 
            this.FiltersVw.CheckBoxes = true;
            this.FiltersVw.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.HeaderCol,
            this.DestinationCol,
            this.ActionCol,
            this.ReplacementCol});
            this.FiltersVw.FullRowSelect = true;
            this.FiltersVw.GridLines = true;
            this.FiltersVw.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.FiltersVw.HideSelection = false;
            this.FiltersVw.Location = new System.Drawing.Point(6, 6);
            this.FiltersVw.MultiSelect = false;
            this.FiltersVw.Name = "FiltersVw";
            this.FiltersVw.ShowItemToolTips = true;
            this.FiltersVw.Size = new System.Drawing.Size(385, 189);
            this.FiltersVw.TabIndex = 20;
            this.FiltersVw.UseCompatibleStateImageBehavior = false;
            this.FiltersVw.View = System.Windows.Forms.View.Details;
            // 
            // HeaderCol
            // 
            this.HeaderCol.Name = "HeaderCol";
            this.HeaderCol.Text = "Header";
            this.HeaderCol.Width = 55;
            // 
            // DestinationCol
            // 
            this.DestinationCol.Name = "DestinationCol";
            this.DestinationCol.Text = "Destination";
            this.DestinationCol.Width = 68;
            // 
            // ActionCol
            // 
            this.ActionCol.Name = "ActionCol";
            this.ActionCol.Text = "Action";
            this.ActionCol.Width = 68;
            // 
            // ReplacementCol
            // 
            this.ReplacementCol.Name = "ReplacementCol";
            this.ReplacementCol.Text = "Replacement";
            this.ReplacementCol.Width = 176;
            // 
            // FiltersPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ReplacementTxt);
            this.Controls.Add(this.DestinationLbl);
            this.Controls.Add(this.DestinationTxt);
            this.Controls.Add(this.HeaderTxt);
            this.Controls.Add(this.HeaderLbl);
            this.Controls.Add(this.RemoveBtn);
            this.Controls.Add(this.CreateBtn);
            this.Controls.Add(this.ActionLbl);
            this.Controls.Add(this.ActionTxt);
            this.Controls.Add(this.FiltersVw);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "FiltersPage";
            this.Size = new System.Drawing.Size(397, 270);
            ((System.ComponentModel.ISupportInitialize)(this.HeaderTxt)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Tangine.Controls.TangineLabelBox ReplacementTxt;
        private System.Windows.Forms.Label DestinationLbl;
        private System.Windows.Forms.ComboBox DestinationTxt;
        private System.Windows.Forms.Label HeaderLbl;
        private System.Windows.Forms.NumericUpDown HeaderTxt;
        private Tangine.Controls.TangineButton RemoveBtn;
        private Tangine.Controls.TangineButton CreateBtn;
        private System.Windows.Forms.Label ActionLbl;
        private System.Windows.Forms.ComboBox ActionTxt;
        private Tangine.Controls.TangineListView FiltersVw;
        private System.Windows.Forms.ColumnHeader HeaderCol;
        private System.Windows.Forms.ColumnHeader DestinationCol;
        private System.Windows.Forms.ColumnHeader ActionCol;
        private System.Windows.Forms.ColumnHeader ReplacementCol;
    }
}
