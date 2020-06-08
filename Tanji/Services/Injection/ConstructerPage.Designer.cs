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
            this.TransferBelowBtn = new Tangine.Controls.TangineButton();
            this.WriteBooleanBtn = new Tangine.Controls.TangineButton();
            this.WriteStringBtn = new Tangine.Controls.TangineButton();
            this.WriteIntegerBtn = new Tangine.Controls.TangineButton();
            this.ValueTxt = new Tangine.Controls.TangineLabelBox();
            this.ValuesVw = new Tangine.Controls.TangineListView();
            this.TypeCol = new System.Windows.Forms.ColumnHeader();
            this.ValueCol = new System.Windows.Forms.ColumnHeader();
            this.EncodedCol = new System.Windows.Forms.ColumnHeader();
            this.DismantledTxt = new Tangine.Controls.TangineLabelBox();
            this.AmountTxt = new Tangine.Controls.TangineLabelBox();
            this.IDTxt = new Tangine.Controls.TangineLabelBox();
            this.SuspendLayout();
            // 
            // MoveDownBtn
            // 
            this.MoveDownBtn.Enabled = false;
            this.MoveDownBtn.Location = new System.Drawing.Point(322, 256);
            this.MoveDownBtn.Name = "MoveDownBtn";
            this.MoveDownBtn.Size = new System.Drawing.Size(100, 20);
            this.MoveDownBtn.TabIndex = 17;
            this.MoveDownBtn.Text = "Move Down";
            // 
            // MoveUpBtn
            // 
            this.MoveUpBtn.Enabled = false;
            this.MoveUpBtn.Location = new System.Drawing.Point(216, 256);
            this.MoveUpBtn.Name = "MoveUpBtn";
            this.MoveUpBtn.Size = new System.Drawing.Size(100, 20);
            this.MoveUpBtn.TabIndex = 16;
            this.MoveUpBtn.Text = "Move Up";
            // 
            // RemoveBtn
            // 
            this.RemoveBtn.Enabled = false;
            this.RemoveBtn.Location = new System.Drawing.Point(110, 256);
            this.RemoveBtn.Name = "RemoveBtn";
            this.RemoveBtn.Size = new System.Drawing.Size(100, 20);
            this.RemoveBtn.TabIndex = 15;
            this.RemoveBtn.Text = "Remove";
            // 
            // ClearBtn
            // 
            this.ClearBtn.Location = new System.Drawing.Point(322, 230);
            this.ClearBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ClearBtn.Name = "ClearBtn";
            this.ClearBtn.Size = new System.Drawing.Size(100, 20);
            this.ClearBtn.TabIndex = 13;
            this.ClearBtn.Text = "Clear";
            // 
            // TransferBelowBtn
            // 
            this.TransferBelowBtn.Location = new System.Drawing.Point(3, 256);
            this.TransferBelowBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TransferBelowBtn.Name = "TransferBelowBtn";
            this.TransferBelowBtn.Size = new System.Drawing.Size(100, 20);
            this.TransferBelowBtn.TabIndex = 12;
            this.TransferBelowBtn.Text = "Transfer Below";
            // 
            // WriteBooleanBtn
            // 
            this.WriteBooleanBtn.Location = new System.Drawing.Point(219, 30);
            this.WriteBooleanBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.WriteBooleanBtn.Name = "WriteBooleanBtn";
            this.WriteBooleanBtn.Size = new System.Drawing.Size(100, 20);
            this.WriteBooleanBtn.TabIndex = 9;
            this.WriteBooleanBtn.Text = "Write Boolean";
            this.WriteBooleanBtn.Click += new System.EventHandler(this.WriteBooleanBtn_Click);
            // 
            // WriteStringBtn
            // 
            this.WriteStringBtn.Location = new System.Drawing.Point(111, 30);
            this.WriteStringBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.WriteStringBtn.Name = "WriteStringBtn";
            this.WriteStringBtn.Size = new System.Drawing.Size(100, 20);
            this.WriteStringBtn.TabIndex = 8;
            this.WriteStringBtn.Text = "Write String";
            this.WriteStringBtn.Click += new System.EventHandler(this.WriteStringBtn_Click);
            // 
            // WriteIntegerBtn
            // 
            this.WriteIntegerBtn.Location = new System.Drawing.Point(3, 30);
            this.WriteIntegerBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.WriteIntegerBtn.Name = "WriteIntegerBtn";
            this.WriteIntegerBtn.Size = new System.Drawing.Size(100, 20);
            this.WriteIntegerBtn.TabIndex = 2;
            this.WriteIntegerBtn.Text = "Write Integer";
            this.WriteIntegerBtn.Click += new System.EventHandler(this.WriteIntegerBtn_Click);
            // 
            // ValueTxt
            // 
            this.ValueTxt.Location = new System.Drawing.Point(96, 4);
            this.ValueTxt.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ValueTxt.Name = "ValueTxt";
            this.ValueTxt.Size = new System.Drawing.Size(326, 20);
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
            this.ValuesVw.Location = new System.Drawing.Point(3, 56);
            this.ValuesVw.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ValuesVw.MultiSelect = false;
            this.ValuesVw.Name = "ValuesVw";
            this.ValuesVw.ShowItemToolTips = true;
            this.ValuesVw.Size = new System.Drawing.Size(419, 168);
            this.ValuesVw.TabIndex = 0;
            this.ValuesVw.UseCompatibleStateImageBehavior = false;
            this.ValuesVw.View = System.Windows.Forms.View.Details;
            this.ValuesVw.ItemActivate += new System.EventHandler(this.ValuesVw_ItemActivate);
            // 
            // TypeCol
            // 
            this.TypeCol.Name = "TypeCol";
            this.TypeCol.Text = "Type";
            this.TypeCol.Width = 86;
            // 
            // ValueCol
            // 
            this.ValueCol.Name = "ValueCol";
            this.ValueCol.Text = "Value";
            this.ValueCol.Width = 165;
            // 
            // EncodedCol
            // 
            this.EncodedCol.Name = "EncodedCol";
            this.EncodedCol.Text = "Encoded";
            this.EncodedCol.Width = 164;
            // 
            // DismantledTxt
            // 
            this.DismantledTxt.Location = new System.Drawing.Point(3, 230);
            this.DismantledTxt.Name = "DismantledTxt";
            this.DismantledTxt.Size = new System.Drawing.Size(313, 20);
            this.DismantledTxt.TabIndex = 18;
            this.DismantledTxt.Text = "";
            this.DismantledTxt.Title = "Dismantled";
            // 
            // AmountTxt
            // 
            this.AmountTxt.Location = new System.Drawing.Point(326, 30);
            this.AmountTxt.Name = "AmountTxt";
            this.AmountTxt.Size = new System.Drawing.Size(96, 20);
            this.AmountTxt.TabIndex = 19;
            this.AmountTxt.Text = "";
            this.AmountTxt.Title = "Amount";
            // 
            // IDTxt
            // 
            this.IDTxt.Location = new System.Drawing.Point(3, 4);
            this.IDTxt.Name = "IDTxt";
            this.IDTxt.Size = new System.Drawing.Size(86, 20);
            this.IDTxt.TabIndex = 19;
            this.IDTxt.Text = "";
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
            this.Controls.Add(this.TransferBelowBtn);
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
        private System.Windows.Forms.ColumnHeader EncodedCol;
        private Tangine.Controls.TangineButton WriteIntegerBtn;
        private Tangine.Controls.TangineButton WriteStringBtn;
        private Tangine.Controls.TangineButton WriteBooleanBtn;
        private Tangine.Controls.TangineButton TransferBelowBtn;
        private Tangine.Controls.TangineButton ClearBtn;
        private Tangine.Controls.TangineButton MoveUpBtn;
        private Tangine.Controls.TangineButton MoveDownBtn;
        private Tangine.Controls.TangineButton RemoveBtn;
        private Tangine.Controls.TangineLabelBox DismantledTxt;
        private Tangine.Controls.TangineLabelBox AmountTxt;
        private Tangine.Controls.TangineLabelBox IDTxt;
    }
}
