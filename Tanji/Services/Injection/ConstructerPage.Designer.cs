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
            this.MoveDownBtn = new Tangine.Controls.TangineButton();
            this.MoveUpBtn = new Tangine.Controls.TangineButton();
            this.RemoveBtn = new Tangine.Controls.TangineButton();
            this.ClearBtn = new Tangine.Controls.TangineButton();
            this.CopyBtn = new Tangine.Controls.TangineButton();
            this.WriteBooleanBtn = new Tangine.Controls.TangineButton();
            this.WriteStringBtn = new Tangine.Controls.TangineButton();
            this.WriteIntegerBtn = new Tangine.Controls.TangineButton();
            this.ValueTxt = new Tangine.Controls.TangineLabelBox();
            this.ValuesVw = new Tangine.Controls.TangineListView();
            this.TypeCol = new System.Windows.Forms.ColumnHeader();
            this.ValueCol = new System.Windows.Forms.ColumnHeader();
            this.DismantledTxt = new Tangine.Controls.TangineLabelBox();
            this.AmountTxt = new Tangine.Controls.TangineLabelBox();
            this.IDTxt = new Tangine.Controls.TangineLabelBox();
            this.SuspendLayout();
            // 
            // MoveDownBtn
            // 
            this.MoveDownBtn.Enabled = false;
            this.MoveDownBtn.Location = new System.Drawing.Point(216, 256);
            this.MoveDownBtn.Name = "MoveDownBtn";
            this.MoveDownBtn.Size = new System.Drawing.Size(100, 20);
            this.MoveDownBtn.TabIndex = 17;
            this.MoveDownBtn.Text = "Move Down";
            this.MoveDownBtn.Click += new System.EventHandler(this.MoveDownBtn_Click);
            // 
            // MoveUpBtn
            // 
            this.MoveUpBtn.Enabled = false;
            this.MoveUpBtn.Location = new System.Drawing.Point(322, 256);
            this.MoveUpBtn.Name = "MoveUpBtn";
            this.MoveUpBtn.Size = new System.Drawing.Size(100, 20);
            this.MoveUpBtn.TabIndex = 16;
            this.MoveUpBtn.Text = "Move Up";
            this.MoveUpBtn.Click += new System.EventHandler(this.MoveUpBtn_Click);
            // 
            // RemoveBtn
            // 
            this.RemoveBtn.Enabled = false;
            this.RemoveBtn.Location = new System.Drawing.Point(110, 256);
            this.RemoveBtn.Name = "RemoveBtn";
            this.RemoveBtn.Size = new System.Drawing.Size(100, 20);
            this.RemoveBtn.TabIndex = 15;
            this.RemoveBtn.Text = "Remove";
            this.RemoveBtn.Click += new System.EventHandler(this.RemoveBtn_Click);
            // 
            // ClearBtn
            // 
            this.ClearBtn.Location = new System.Drawing.Point(322, 230);
            this.ClearBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ClearBtn.Name = "ClearBtn";
            this.ClearBtn.Size = new System.Drawing.Size(100, 20);
            this.ClearBtn.TabIndex = 13;
            this.ClearBtn.Text = "Clear";
            this.ClearBtn.Click += new System.EventHandler(this.ClearBtn_Click);
            // 
            // CopyBtn
            // 
            this.CopyBtn.Location = new System.Drawing.Point(3, 256);
            this.CopyBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.CopyBtn.Name = "CopyBtn";
            this.CopyBtn.Size = new System.Drawing.Size(100, 20);
            this.CopyBtn.TabIndex = 12;
            this.CopyBtn.Text = "Copy";
            this.CopyBtn.Click += new System.EventHandler(this.CopyBtn_Click);
            // 
            // WriteBooleanBtn
            // 
            this.WriteBooleanBtn.Location = new System.Drawing.Point(219, 29);
            this.WriteBooleanBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.WriteBooleanBtn.Name = "WriteBooleanBtn";
            this.WriteBooleanBtn.Size = new System.Drawing.Size(100, 20);
            this.WriteBooleanBtn.TabIndex = 9;
            this.WriteBooleanBtn.Text = "Write Boolean";
            this.WriteBooleanBtn.Click += new System.EventHandler(this.WriteBooleanBtn_Click);
            // 
            // WriteStringBtn
            // 
            this.WriteStringBtn.Location = new System.Drawing.Point(111, 29);
            this.WriteStringBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.WriteStringBtn.Name = "WriteStringBtn";
            this.WriteStringBtn.Size = new System.Drawing.Size(100, 20);
            this.WriteStringBtn.TabIndex = 8;
            this.WriteStringBtn.Text = "Write String";
            this.WriteStringBtn.Click += new System.EventHandler(this.WriteStringBtn_Click);
            // 
            // WriteIntegerBtn
            // 
            this.WriteIntegerBtn.Location = new System.Drawing.Point(3, 29);
            this.WriteIntegerBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.WriteIntegerBtn.Name = "WriteIntegerBtn";
            this.WriteIntegerBtn.Size = new System.Drawing.Size(100, 20);
            this.WriteIntegerBtn.TabIndex = 2;
            this.WriteIntegerBtn.Text = "Write Integer";
            this.WriteIntegerBtn.Click += new System.EventHandler(this.WriteIntegerBtn_Click);
            // 
            // ValueTxt
            // 
            this.ValueTxt.Location = new System.Drawing.Point(96, 3);
            this.ValueTxt.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ValueTxt.Name = "ValueTxt";
            this.ValueTxt.Size = new System.Drawing.Size(326, 20);
            this.ValueTxt.TabIndex = 1;
            this.ValueTxt.TabStop = false;
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
            this.ValueCol});
            this.ValuesVw.FullRowSelect = true;
            this.ValuesVw.GridLines = true;
            this.ValuesVw.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.ValuesVw.HideSelection = false;
            this.ValuesVw.Location = new System.Drawing.Point(3, 55);
            this.ValuesVw.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ValuesVw.MultiSelect = false;
            this.ValuesVw.Name = "ValuesVw";
            this.ValuesVw.OwnerDraw = true;
            this.ValuesVw.ShowItemToolTips = true;
            this.ValuesVw.Size = new System.Drawing.Size(419, 169);
            this.ValuesVw.TabIndex = 0;
            this.ValuesVw.TabStop = false;
            this.ValuesVw.UseCompatibleStateImageBehavior = false;
            this.ValuesVw.View = System.Windows.Forms.View.Details;
            this.ValuesVw.ItemActivate += new System.EventHandler(this.ValuesVw_ItemActivate);
            this.ValuesVw.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.ValuesVw_ItemSelectionChanged);
            // 
            // TypeCol
            // 
            this.TypeCol.Name = "TypeCol";
            this.TypeCol.Text = "Type";
            this.TypeCol.Width = 82;
            // 
            // ValueCol
            // 
            this.ValueCol.Name = "ValueCol";
            this.ValueCol.Text = "Value";
            this.ValueCol.Width = 333;
            // 
            // DismantledTxt
            // 
            this.DismantledTxt.IsReadOnly = true;
            this.DismantledTxt.Location = new System.Drawing.Point(3, 230);
            this.DismantledTxt.Name = "DismantledTxt";
            this.DismantledTxt.Size = new System.Drawing.Size(313, 20);
            this.DismantledTxt.TabIndex = 18;
            this.DismantledTxt.TabStop = false;
            this.DismantledTxt.Text = "";
            this.DismantledTxt.Title = "Dismantled";
            // 
            // AmountTxt
            // 
            this.AmountTxt.IsNumbersOnly = true;
            this.AmountTxt.Location = new System.Drawing.Point(326, 29);
            this.AmountTxt.MaxLength = 2;
            this.AmountTxt.Name = "AmountTxt";
            this.AmountTxt.Size = new System.Drawing.Size(96, 20);
            this.AmountTxt.TabIndex = 19;
            this.AmountTxt.TabStop = false;
            this.AmountTxt.Text = "";
            this.AmountTxt.Title = "Amount";
            // 
            // IDTxt
            // 
            this.IDTxt.IsNumbersOnly = true;
            this.IDTxt.Location = new System.Drawing.Point(3, 3);
            this.IDTxt.MaxLength = 4;
            this.IDTxt.Name = "IDTxt";
            this.IDTxt.Size = new System.Drawing.Size(86, 20);
            this.IDTxt.TabIndex = 19;
            this.IDTxt.TabStop = false;
            this.IDTxt.Text = "1";
            this.IDTxt.Title = "ID";
            // 
            // ConstructerPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.IDTxt);
            this.Controls.Add(this.AmountTxt);
            this.Controls.Add(this.DismantledTxt);
            this.Controls.Add(this.MoveDownBtn);
            this.Controls.Add(this.MoveUpBtn);
            this.Controls.Add(this.RemoveBtn);
            this.Controls.Add(this.ClearBtn);
            this.Controls.Add(this.CopyBtn);
            this.Controls.Add(this.WriteIntegerBtn);
            this.Controls.Add(this.WriteStringBtn);
            this.Controls.Add(this.WriteBooleanBtn);
            this.Controls.Add(this.ValueTxt);
            this.Controls.Add(this.ValuesVw);
            this.Name = "ConstructerPage";
            this.Size = new System.Drawing.Size(425, 278);
            this.ResumeLayout(false);

        }

        #endregion

        private Tangine.Controls.TangineListView ValuesVw;
        private System.Windows.Forms.ColumnHeader TypeCol;
        private System.Windows.Forms.ColumnHeader ValueCol;
        private Tangine.Controls.TangineLabelBox ValueTxt;
        private Tangine.Controls.TangineButton WriteIntegerBtn;
        private Tangine.Controls.TangineButton WriteStringBtn;
        private Tangine.Controls.TangineButton WriteBooleanBtn;
        private Tangine.Controls.TangineButton CopyBtn;
        private Tangine.Controls.TangineButton ClearBtn;
        private Tangine.Controls.TangineButton MoveUpBtn;
        private Tangine.Controls.TangineButton MoveDownBtn;
        private Tangine.Controls.TangineButton RemoveBtn;
        private Tangine.Controls.TangineLabelBox DismantledTxt;
        private Tangine.Controls.TangineLabelBox AmountTxt;
        private Tangine.Controls.TangineLabelBox IDTxt;
    }
}
