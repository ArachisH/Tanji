namespace Tanji.Services.Injection
{
    partial class ConstructerPage
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
            this.IdentifierTxt = new System.Windows.Forms.NumericUpDown();
            this.AmountTxt = new System.Windows.Forms.NumericUpDown();
            this.StructureTxt = new System.Windows.Forms.TextBox();
            this.MoveDownBtn = new Tangine.Controls.TangineButton();
            this.MoveUpBtn = new Tangine.Controls.TangineButton();
            this.RemoveBtn = new Tangine.Controls.TangineButton();
            this.ClearBtn = new Tangine.Controls.TangineButton();
            this.TransferBelowBtn = new Tangine.Controls.TangineButton();
            this.ValueCountLbl = new Tangine.Controls.TangineLabel();
            this.WriteBooleanBtn = new Tangine.Controls.TangineButton();
            this.WriteStringBtn = new Tangine.Controls.TangineButton();
            this.AmountLbl = new Tangine.Controls.TangineLabel();
            this.IdentifierLbl = new Tangine.Controls.TangineLabel();
            this.WriteIntegerBtn = new Tangine.Controls.TangineButton();
            this.ValueTxt = new Tangine.Controls.TangineLabelBox();
            this.ValuesVw = new Tangine.Controls.TangineListView();
            this.TypeCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ValueCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.EncodedCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            ((System.ComponentModel.ISupportInitialize)(this.IdentifierTxt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AmountTxt)).BeginInit();
            this.SuspendLayout();
            // 
            // IdentifierTxt
            // 
            this.IdentifierTxt.Location = new System.Drawing.Point(27, 0);
            this.IdentifierTxt.Maximum = new decimal(new int[] {
            4000,
            0,
            0,
            0});
            this.IdentifierTxt.Name = "IdentifierTxt";
            this.IdentifierTxt.Size = new System.Drawing.Size(49, 20);
            this.IdentifierTxt.TabIndex = 5;
            this.IdentifierTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.IdentifierTxt.Value = new decimal(new int[] {
            4000,
            0,
            0,
            0});
            // 
            // AmountTxt
            // 
            this.AmountTxt.Location = new System.Drawing.Point(378, 0);
            this.AmountTxt.Maximum = new decimal(new int[] {
            424224,
            0,
            0,
            0});
            this.AmountTxt.Name = "AmountTxt";
            this.AmountTxt.Size = new System.Drawing.Size(49, 20);
            this.AmountTxt.TabIndex = 7;
            this.AmountTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.AmountTxt.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // StructureTxt
            // 
            this.StructureTxt.Location = new System.Drawing.Point(0, 231);
            this.StructureTxt.Name = "StructureTxt";
            this.StructureTxt.ReadOnly = true;
            this.StructureTxt.Size = new System.Drawing.Size(323, 20);
            this.StructureTxt.TabIndex = 11;
            this.StructureTxt.Text = "{id:0}";
            this.StructureTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // MoveDownBtn
            // 
            this.MoveDownBtn.Enabled = false;
            this.MoveDownBtn.Location = new System.Drawing.Point(329, 257);
            this.MoveDownBtn.Name = "MoveDownBtn";
            this.MoveDownBtn.Size = new System.Drawing.Size(98, 20);
            this.MoveDownBtn.TabIndex = 17;
            this.MoveDownBtn.Text = "Move Down";
            // 
            // MoveUpBtn
            // 
            this.MoveUpBtn.Enabled = false;
            this.MoveUpBtn.Location = new System.Drawing.Point(225, 257);
            this.MoveUpBtn.Name = "MoveUpBtn";
            this.MoveUpBtn.Size = new System.Drawing.Size(98, 20);
            this.MoveUpBtn.TabIndex = 16;
            this.MoveUpBtn.Text = "Move Up";
            // 
            // RemoveBtn
            // 
            this.RemoveBtn.Enabled = false;
            this.RemoveBtn.Location = new System.Drawing.Point(117, 257);
            this.RemoveBtn.Name = "RemoveBtn";
            this.RemoveBtn.Size = new System.Drawing.Size(102, 20);
            this.RemoveBtn.TabIndex = 15;
            this.RemoveBtn.Text = "Remove";
            // 
            // ClearBtn
            // 
            this.ClearBtn.Location = new System.Drawing.Point(0, 26);
            this.ClearBtn.Name = "ClearBtn";
            this.ClearBtn.Size = new System.Drawing.Size(76, 20);
            this.ClearBtn.TabIndex = 13;
            this.ClearBtn.Text = "Clear";
            // 
            // TransferBelowBtn
            // 
            this.TransferBelowBtn.Location = new System.Drawing.Point(0, 257);
            this.TransferBelowBtn.Name = "TransferBelowBtn";
            this.TransferBelowBtn.Size = new System.Drawing.Size(111, 20);
            this.TransferBelowBtn.TabIndex = 12;
            this.TransferBelowBtn.Text = "Transfer Below";
            // 
            // ValueCountLbl
            // 
            this.ValueCountLbl.Location = new System.Drawing.Point(329, 231);
            this.ValueCountLbl.Name = "ValueCountLbl";
            this.ValueCountLbl.Size = new System.Drawing.Size(98, 21);
            this.ValueCountLbl.TabIndex = 10;
            this.ValueCountLbl.Text = "Value Count: 0";
            // 
            // AmountLbl
            // 
            this.AmountLbl.BorderMode = Tangine.Controls.TangineLabel.LabelBorderMode.Right;
            this.AmountLbl.Location = new System.Drawing.Point(317, 0);
            this.AmountLbl.Name = "AmountLbl";
            this.AmountLbl.Size = new System.Drawing.Size(56, 20);
            this.AmountLbl.TabIndex = 6;
            this.AmountLbl.Text = "Amount";
            // 
            // IdentifierLbl
            // 
            this.IdentifierLbl.BorderMode = Tangine.Controls.TangineLabel.LabelBorderMode.Right;
            this.IdentifierLbl.Location = new System.Drawing.Point(-10, 0);
            this.IdentifierLbl.Name = "IdentifierLbl";
            this.IdentifierLbl.Size = new System.Drawing.Size(31, 21);
            this.IdentifierLbl.TabIndex = 4;
            this.IdentifierLbl.Text = "ID";
            // 
            // WriteIntegerBtn
            // 
            this.WriteIntegerBtn.Location = new System.Drawing.Point(83, 26);
            this.WriteIntegerBtn.Name = "WriteIntegerBtn";
            this.WriteIntegerBtn.Size = new System.Drawing.Size(110, 20);
            this.WriteIntegerBtn.TabIndex = 2;
            this.WriteIntegerBtn.Text = "Write Integer";
            this.WriteIntegerBtn.Click += new System.EventHandler(this.WriteIntegerBtn_Click);
            // 
            // WriteStringBtn
            // 
            this.WriteStringBtn.Location = new System.Drawing.Point(200, 26);
            this.WriteStringBtn.Name = "WriteStringBtn";
            this.WriteStringBtn.Size = new System.Drawing.Size(110, 20);
            this.WriteStringBtn.TabIndex = 8;
            this.WriteStringBtn.Text = "Write String";
            this.WriteStringBtn.Click += new System.EventHandler(this.WriteStringBtn_Click);
            // 
            // WriteBooleanBtn
            // 
            this.WriteBooleanBtn.Location = new System.Drawing.Point(317, 26);
            this.WriteBooleanBtn.Name = "WriteBooleanBtn";
            this.WriteBooleanBtn.Size = new System.Drawing.Size(110, 20);
            this.WriteBooleanBtn.TabIndex = 9;
            this.WriteBooleanBtn.Text = "Write Boolean";
            this.WriteBooleanBtn.Click += new System.EventHandler(this.WriteBooleanBtn_Click);
            // 
            // ValueTxt
            // 
            this.ValueTxt.Location = new System.Drawing.Point(82, 0);
            this.ValueTxt.Name = "ValueTxt";
            this.ValueTxt.Size = new System.Drawing.Size(228, 20);
            this.ValueTxt.TabIndex = 1;
            this.ValueTxt.Text = "";
            this.ValueTxt.TextPaddingWidth = 0;
            this.ValueTxt.Title = "Value";
            this.ValueTxt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ValueTxt_KeyDown);
            // 
            // ValuesVw
            // 
            this.ValuesVw.CheckBoxes = true;
            this.ValuesVw.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.TypeCol,
            this.ValueCol,
            this.EncodedCol});
            this.ValuesVw.FullRowSelect = true;
            this.ValuesVw.GridLines = true;
            this.ValuesVw.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.ValuesVw.HideSelection = false;
            this.ValuesVw.Location = new System.Drawing.Point(0, 52);
            this.ValuesVw.MultiSelect = false;
            this.ValuesVw.Name = "ValuesVw";
            this.ValuesVw.ShowItemToolTips = true;
            this.ValuesVw.Size = new System.Drawing.Size(427, 173);
            this.ValuesVw.TabIndex = 0;
            this.ValuesVw.UseCompatibleStateImageBehavior = false;
            this.ValuesVw.View = System.Windows.Forms.View.Details;
            this.ValuesVw.ItemActivate += new System.EventHandler(this.ValuesVw_ItemActivate);
            // 
            // TypeCol
            // 
            this.TypeCol.Text = "Type";
            this.TypeCol.Width = 86;
            // 
            // ValueCol
            // 
            this.ValueCol.Text = "Value";
            this.ValueCol.Width = 165;
            // 
            // EncodedCol
            // 
            this.EncodedCol.Text = "Encoded";
            this.EncodedCol.Width = 172;
            // 
            // ConstructerPage
            // 
            this.Font = Program.DefaultFont;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MoveDownBtn);
            this.Controls.Add(this.MoveUpBtn);
            this.Controls.Add(this.RemoveBtn);
            this.Controls.Add(this.ClearBtn);
            this.Controls.Add(this.TransferBelowBtn);
            this.Controls.Add(this.StructureTxt);
            this.Controls.Add(this.ValueCountLbl);
            this.Controls.Add(this.WriteIntegerBtn);
            this.Controls.Add(this.WriteStringBtn);
            this.Controls.Add(this.WriteBooleanBtn);
            this.Controls.Add(this.AmountTxt);
            this.Controls.Add(this.AmountLbl);
            this.Controls.Add(this.IdentifierTxt);
            this.Controls.Add(this.IdentifierLbl);
            this.Controls.Add(this.ValueTxt);
            this.Controls.Add(this.ValuesVw);
            this.Name = "ConstructerPage";
            this.Size = new System.Drawing.Size(433, 278);
            ((System.ComponentModel.ISupportInitialize)(this.IdentifierTxt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AmountTxt)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Tangine.Controls.TangineListView ValuesVw;
        private System.Windows.Forms.ColumnHeader TypeCol;
        private System.Windows.Forms.ColumnHeader ValueCol;
        private Tangine.Controls.TangineLabelBox ValueTxt;
        private Tangine.Controls.TangineLabel IdentifierLbl;
        private System.Windows.Forms.NumericUpDown IdentifierTxt;
        private System.Windows.Forms.NumericUpDown AmountTxt;
        private Tangine.Controls.TangineLabel AmountLbl;
        private System.Windows.Forms.ColumnHeader EncodedCol;
        private Tangine.Controls.TangineButton WriteIntegerBtn;
        private Tangine.Controls.TangineButton WriteStringBtn;
        private Tangine.Controls.TangineButton WriteBooleanBtn;
        private Tangine.Controls.TangineLabel ValueCountLbl;
        private System.Windows.Forms.TextBox StructureTxt;
        private Tangine.Controls.TangineButton TransferBelowBtn;
        private Tangine.Controls.TangineButton ClearBtn;
        private Tangine.Controls.TangineButton MoveUpBtn;
        private Tangine.Controls.TangineButton MoveDownBtn;
        private Tangine.Controls.TangineButton RemoveBtn;
    }
}
