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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "String",
            "Testing testing 123"}, -1);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("Int32");
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem("Boolean");
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem("Byte");
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem("UInt16");
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem("");
            System.Windows.Forms.ListViewItem listViewItem7 = new System.Windows.Forms.ListViewItem("");
            System.Windows.Forms.ListViewItem listViewItem8 = new System.Windows.Forms.ListViewItem("");
            this.tangineListView1 = new Tangine.Controls.TangineListView();
            this.TypeCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ValueCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ValueTxt = new Tangine.Controls.TangineLabelBox();
            this.SuspendLayout();
            // 
            // tangineListView1
            // 
            this.tangineListView1.CheckBoxes = true;
            this.tangineListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.TypeCol,
            this.ValueCol});
            this.tangineListView1.FullRowSelect = true;
            this.tangineListView1.GridLines = true;
            this.tangineListView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.tangineListView1.HideSelection = false;
            listViewItem1.StateImageIndex = 0;
            listViewItem2.StateImageIndex = 0;
            listViewItem3.StateImageIndex = 0;
            listViewItem4.StateImageIndex = 0;
            listViewItem5.StateImageIndex = 0;
            listViewItem6.StateImageIndex = 0;
            listViewItem7.StateImageIndex = 0;
            listViewItem8.StateImageIndex = 0;
            this.tangineListView1.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3,
            listViewItem4,
            listViewItem5,
            listViewItem6,
            listViewItem7,
            listViewItem8});
            this.tangineListView1.Location = new System.Drawing.Point(0, 0);
            this.tangineListView1.MultiSelect = false;
            this.tangineListView1.Name = "tangineListView1";
            this.tangineListView1.ShowItemToolTips = true;
            this.tangineListView1.Size = new System.Drawing.Size(407, 115);
            this.tangineListView1.TabIndex = 0;
            this.tangineListView1.UseCompatibleStateImageBehavior = false;
            this.tangineListView1.View = System.Windows.Forms.View.Details;
            // 
            // TypeCol
            // 
            this.TypeCol.Text = "Type";
            this.TypeCol.Width = 80;
            // 
            // ValueCol
            // 
            this.ValueCol.Text = "Value";
            this.ValueCol.Width = 306;
            // 
            // ValueTxt
            // 
            this.ValueTxt.Location = new System.Drawing.Point(0, 121);
            this.ValueTxt.Name = "ValueTxt";
            this.ValueTxt.Size = new System.Drawing.Size(200, 20);
            this.ValueTxt.TabIndex = 1;
            this.ValueTxt.Text = "";
            this.ValueTxt.TextPaddingWidth = 0;
            this.ValueTxt.Title = "Value";
            // 
            // ConstructerPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ValueTxt);
            this.Controls.Add(this.tangineListView1);
            this.Name = "ConstructerPage";
            this.Size = new System.Drawing.Size(407, 282);
            this.ResumeLayout(false);

        }

        #endregion

        private Tangine.Controls.TangineListView tangineListView1;
        private System.Windows.Forms.ColumnHeader TypeCol;
        private System.Windows.Forms.ColumnHeader ValueCol;
        private Tangine.Controls.TangineLabelBox ValueTxt;
    }
}
