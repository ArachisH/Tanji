namespace Tanji.Services.Injection
{
    partial class SchedulerPage
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
            this.IntervalTxt = new Tangine.Controls.TangineLabelBox();
            this.CyclesTxt = new Tangine.Controls.TangineLabelBox();
            this.PacketTxt = new Tangine.Controls.TangineLabelBox();
            this.HotkeyTxt = new Tangine.Controls.TangineLabelBox();
            this.ToServerChbx = new System.Windows.Forms.CheckBox();
            this.CreateBtn = new Tangine.Controls.TangineButton();
            this.RemoveBtn = new Tangine.Controls.TangineButton();
            this.ClearBtn = new Tangine.Controls.TangineButton();
            this.SchedulesVw = new Tangine.Controls.TangineListView();
            this.PacketCol = new System.Windows.Forms.ColumnHeader();
            this.DestinationCol = new System.Windows.Forms.ColumnHeader();
            this.IntervalCol = new System.Windows.Forms.ColumnHeader();
            this.CyclesCol = new System.Windows.Forms.ColumnHeader();
            this.HotkeyCol = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // IntervalTxt
            // 
            this.IntervalTxt.IsNumbersOnly = true;
            this.IntervalTxt.Location = new System.Drawing.Point(250, 229);
            this.IntervalTxt.MaxLength = 5;
            this.IntervalTxt.Name = "IntervalTxt";
            this.IntervalTxt.Size = new System.Drawing.Size(93, 20);
            this.IntervalTxt.TabIndex = 0;
            this.IntervalTxt.Text = "250";
            this.IntervalTxt.TextPaddingWidth = 0;
            this.IntervalTxt.Title = "Interval";
            // 
            // CyclesTxt
            // 
            this.CyclesTxt.IsNumbersOnly = true;
            this.CyclesTxt.Location = new System.Drawing.Point(158, 255);
            this.CyclesTxt.MaxLength = 5;
            this.CyclesTxt.Name = "CyclesTxt";
            this.CyclesTxt.Size = new System.Drawing.Size(72, 20);
            this.CyclesTxt.TabIndex = 0;
            this.CyclesTxt.Text = "1";
            this.CyclesTxt.TextPaddingWidth = 0;
            this.CyclesTxt.Title = "Cycles";
            // 
            // PacketTxt
            // 
            this.PacketTxt.Location = new System.Drawing.Point(3, 229);
            this.PacketTxt.Name = "PacketTxt";
            this.PacketTxt.Size = new System.Drawing.Size(241, 20);
            this.PacketTxt.TabIndex = 1;
            this.PacketTxt.Text = "";
            this.PacketTxt.TextPaddingWidth = 3;
            this.PacketTxt.Title = "Packet";
            // 
            // HotkeyTxt
            // 
            this.HotkeyTxt.Location = new System.Drawing.Point(3, 255);
            this.HotkeyTxt.Name = "HotkeyTxt";
            this.HotkeyTxt.Size = new System.Drawing.Size(149, 20);
            this.HotkeyTxt.TabIndex = 1;
            this.HotkeyTxt.Text = "Shift, Control+A";
            this.HotkeyTxt.TextPaddingWidth = 0;
            this.HotkeyTxt.Title = "Hotkey";
            this.HotkeyTxt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HotkeyTxt_KeyDown);
            // 
            // ToServerChbx
            // 
            this.ToServerChbx.AutoSize = true;
            this.ToServerChbx.Location = new System.Drawing.Point(349, 230);
            this.ToServerChbx.Name = "ToServerChbx";
            this.ToServerChbx.Size = new System.Drawing.Size(73, 19);
            this.ToServerChbx.TabIndex = 3;
            this.ToServerChbx.Text = "To Server";
            this.ToServerChbx.UseVisualStyleBackColor = true;
            // 
            // CreateBtn
            // 
            this.CreateBtn.Location = new System.Drawing.Point(236, 255);
            this.CreateBtn.Name = "CreateBtn";
            this.CreateBtn.Size = new System.Drawing.Size(58, 20);
            this.CreateBtn.TabIndex = 4;
            this.CreateBtn.Text = "Create";
            this.CreateBtn.Click += new System.EventHandler(this.CreateBtn_Click);
            // 
            // RemoveBtn
            // 
            this.RemoveBtn.Location = new System.Drawing.Point(300, 255);
            this.RemoveBtn.Name = "RemoveBtn";
            this.RemoveBtn.Size = new System.Drawing.Size(58, 20);
            this.RemoveBtn.TabIndex = 4;
            this.RemoveBtn.Text = "Remove";
            this.RemoveBtn.Click += new System.EventHandler(this.RemoveBtn_Click);
            // 
            // ClearBtn
            // 
            this.ClearBtn.Location = new System.Drawing.Point(364, 255);
            this.ClearBtn.Name = "ClearBtn";
            this.ClearBtn.Size = new System.Drawing.Size(58, 20);
            this.ClearBtn.TabIndex = 4;
            this.ClearBtn.Text = "Clear";
            this.ClearBtn.Click += new System.EventHandler(this.ClearBtn_Click);
            // 
            // SchedulesVw
            // 
            this.SchedulesVw.CheckBoxes = true;
            this.SchedulesVw.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.PacketCol,
            this.DestinationCol,
            this.IntervalCol,
            this.CyclesCol,
            this.HotkeyCol});
            this.SchedulesVw.FillColumnIndex = 0;
            this.SchedulesVw.FullRowSelect = true;
            this.SchedulesVw.GridLines = true;
            this.SchedulesVw.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.SchedulesVw.HideSelection = false;
            this.SchedulesVw.Location = new System.Drawing.Point(3, 3);
            this.SchedulesVw.MultiSelect = false;
            this.SchedulesVw.Name = "SchedulesVw";
            this.SchedulesVw.OwnerDraw = true;
            this.SchedulesVw.ShowItemToolTips = true;
            this.SchedulesVw.Size = new System.Drawing.Size(419, 220);
            this.SchedulesVw.TabIndex = 5;
            this.SchedulesVw.UseCompatibleStateImageBehavior = false;
            this.SchedulesVw.View = System.Windows.Forms.View.Details;
            // 
            // PacketCol
            // 
            this.PacketCol.Name = "PacketCol";
            this.PacketCol.Text = "Packet";
            this.PacketCol.Width = 136;
            // 
            // DestinationCol
            // 
            this.DestinationCol.Name = "DestinationCol";
            this.DestinationCol.Text = "Destination";
            this.DestinationCol.Width = 75;
            // 
            // IntervalCol
            // 
            this.IntervalCol.Name = "IntervalCol";
            this.IntervalCol.Text = "Interval";
            this.IntervalCol.Width = 54;
            // 
            // CyclesCol
            // 
            this.CyclesCol.Name = "CyclesCol";
            this.CyclesCol.Text = "Cycles";
            this.CyclesCol.Width = 49;
            // 
            // HotkeyCol
            // 
            this.HotkeyCol.Name = "HotkeyCol";
            this.HotkeyCol.Text = "Hotkey";
            this.HotkeyCol.Width = 101;
            // 
            // SchedulerPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SchedulesVw);
            this.Controls.Add(this.ClearBtn);
            this.Controls.Add(this.RemoveBtn);
            this.Controls.Add(this.CreateBtn);
            this.Controls.Add(this.HotkeyTxt);
            this.Controls.Add(this.PacketTxt);
            this.Controls.Add(this.CyclesTxt);
            this.Controls.Add(this.IntervalTxt);
            this.Controls.Add(this.ToServerChbx);
            this.Name = "SchedulerPage";
            this.Size = new System.Drawing.Size(425, 278);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Tangine.Controls.TangineLabelBox IntervalTxt;
        private Tangine.Controls.TangineLabelBox CyclesTxt;
        private Tangine.Controls.TangineLabelBox PacketTxt;
        private Tangine.Controls.TangineLabelBox HotkeyTxt;
        private System.Windows.Forms.CheckBox ToServerChbx;
        private Tangine.Controls.TangineButton CreateBtn;
        private Tangine.Controls.TangineButton RemoveBtn;
        private Tangine.Controls.TangineButton ClearBtn;
        private Tangine.Controls.TangineListView SchedulesVw;
        private System.Windows.Forms.ColumnHeader PacketCol;
        private System.Windows.Forms.ColumnHeader DestinationCol;
        private System.Windows.Forms.ColumnHeader IntervalCol;
        private System.Windows.Forms.ColumnHeader CyclesCol;
        private System.Windows.Forms.ColumnHeader HotkeyCol;
    }
}
