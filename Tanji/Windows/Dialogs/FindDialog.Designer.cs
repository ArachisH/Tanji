namespace Tanji.Windows.Dialogs
{
    partial class FindDialog
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
            this.FindNextBtn = new Sulakore.Components.SKoreButton();
            this.FindWhatTxt = new System.Windows.Forms.TextBox();
            this.MatchCaseChckbx = new System.Windows.Forms.CheckBox();
            this.MatchWordChckbx = new System.Windows.Forms.CheckBox();
            this.FindWhatLbl = new System.Windows.Forms.Label();
            this.DirectionGrpbx = new System.Windows.Forms.GroupBox();
            this.DownRd = new System.Windows.Forms.RadioButton();
            this.UpRd = new System.Windows.Forms.RadioButton();
            this.ModeGrpbx = new System.Windows.Forms.GroupBox();
            this.RegExRd = new System.Windows.Forms.RadioButton();
            this.NormalRd = new System.Windows.Forms.RadioButton();
            this.WrapAroundChckbx = new System.Windows.Forms.CheckBox();
            this.DirectionGrpbx.SuspendLayout();
            this.ModeGrpbx.SuspendLayout();
            this.SuspendLayout();
            // 
            // FindNextBtn
            // 
            this.FindNextBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FindNextBtn.BackColor = System.Drawing.Color.Transparent;
            this.FindNextBtn.Location = new System.Drawing.Point(252, 10);
            this.FindNextBtn.Name = "FindNextBtn";
            this.FindNextBtn.Size = new System.Drawing.Size(77, 22);
            this.FindNextBtn.Skin = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.FindNextBtn.TabIndex = 4;
            this.FindNextBtn.Text = "Find Next";
            this.FindNextBtn.Click += new System.EventHandler(this.FindNextBtn_Click);
            // 
            // FindWhatTxt
            // 
            this.FindWhatTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FindWhatTxt.Location = new System.Drawing.Point(71, 12);
            this.FindWhatTxt.Name = "FindWhatTxt";
            this.FindWhatTxt.Size = new System.Drawing.Size(175, 20);
            this.FindWhatTxt.TabIndex = 3;
            this.FindWhatTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // MatchCaseChckbx
            // 
            this.MatchCaseChckbx.AutoSize = true;
            this.MatchCaseChckbx.Location = new System.Drawing.Point(8, 44);
            this.MatchCaseChckbx.Name = "MatchCaseChckbx";
            this.MatchCaseChckbx.Size = new System.Drawing.Size(83, 17);
            this.MatchCaseChckbx.TabIndex = 5;
            this.MatchCaseChckbx.Text = "Match Case";
            this.MatchCaseChckbx.UseVisualStyleBackColor = false;
            // 
            // MatchWordChckbx
            // 
            this.MatchWordChckbx.AutoSize = true;
            this.MatchWordChckbx.Location = new System.Drawing.Point(8, 63);
            this.MatchWordChckbx.Name = "MatchWordChckbx";
            this.MatchWordChckbx.Size = new System.Drawing.Size(85, 17);
            this.MatchWordChckbx.TabIndex = 6;
            this.MatchWordChckbx.Text = "Match Word";
            this.MatchWordChckbx.UseVisualStyleBackColor = true;
            // 
            // FindWhatLbl
            // 
            this.FindWhatLbl.AutoSize = true;
            this.FindWhatLbl.Location = new System.Drawing.Point(12, 15);
            this.FindWhatLbl.Name = "FindWhatLbl";
            this.FindWhatLbl.Size = new System.Drawing.Size(59, 13);
            this.FindWhatLbl.TabIndex = 7;
            this.FindWhatLbl.Text = "Find What:";
            // 
            // DirectionGrpbx
            // 
            this.DirectionGrpbx.Controls.Add(this.DownRd);
            this.DirectionGrpbx.Controls.Add(this.UpRd);
            this.DirectionGrpbx.Location = new System.Drawing.Point(252, 38);
            this.DirectionGrpbx.Name = "DirectionGrpbx";
            this.DirectionGrpbx.Size = new System.Drawing.Size(77, 61);
            this.DirectionGrpbx.TabIndex = 8;
            this.DirectionGrpbx.TabStop = false;
            this.DirectionGrpbx.Text = "Direction";
            // 
            // DownRd
            // 
            this.DownRd.AutoSize = true;
            this.DownRd.Checked = true;
            this.DownRd.Location = new System.Drawing.Point(6, 38);
            this.DownRd.Name = "DownRd";
            this.DownRd.Size = new System.Drawing.Size(53, 17);
            this.DownRd.TabIndex = 1;
            this.DownRd.TabStop = true;
            this.DownRd.Text = "Down";
            this.DownRd.UseVisualStyleBackColor = true;
            // 
            // UpRd
            // 
            this.UpRd.AutoSize = true;
            this.UpRd.Location = new System.Drawing.Point(6, 19);
            this.UpRd.Name = "UpRd";
            this.UpRd.Size = new System.Drawing.Size(39, 17);
            this.UpRd.TabIndex = 0;
            this.UpRd.Text = "Up";
            this.UpRd.UseVisualStyleBackColor = true;
            // 
            // ModeGrpbx
            // 
            this.ModeGrpbx.Controls.Add(this.RegExRd);
            this.ModeGrpbx.Controls.Add(this.NormalRd);
            this.ModeGrpbx.Location = new System.Drawing.Point(103, 38);
            this.ModeGrpbx.Name = "ModeGrpbx";
            this.ModeGrpbx.Size = new System.Drawing.Size(143, 61);
            this.ModeGrpbx.TabIndex = 9;
            this.ModeGrpbx.TabStop = false;
            this.ModeGrpbx.Text = "Search Mode";
            // 
            // RegExRd
            // 
            this.RegExRd.AutoSize = true;
            this.RegExRd.Location = new System.Drawing.Point(6, 38);
            this.RegExRd.Name = "RegExRd";
            this.RegExRd.Size = new System.Drawing.Size(116, 17);
            this.RegExRd.TabIndex = 1;
            this.RegExRd.Text = "Regular Expression";
            this.RegExRd.UseVisualStyleBackColor = true;
            // 
            // NormalRd
            // 
            this.NormalRd.AutoSize = true;
            this.NormalRd.Checked = true;
            this.NormalRd.Location = new System.Drawing.Point(6, 19);
            this.NormalRd.Name = "NormalRd";
            this.NormalRd.Size = new System.Drawing.Size(58, 17);
            this.NormalRd.TabIndex = 0;
            this.NormalRd.TabStop = true;
            this.NormalRd.Text = "Normal";
            this.NormalRd.UseVisualStyleBackColor = true;
            // 
            // WrapAroundChckbx
            // 
            this.WrapAroundChckbx.AutoSize = true;
            this.WrapAroundChckbx.Checked = true;
            this.WrapAroundChckbx.CheckState = System.Windows.Forms.CheckState.Checked;
            this.WrapAroundChckbx.Location = new System.Drawing.Point(8, 82);
            this.WrapAroundChckbx.Name = "WrapAroundChckbx";
            this.WrapAroundChckbx.Size = new System.Drawing.Size(89, 17);
            this.WrapAroundChckbx.TabIndex = 10;
            this.WrapAroundChckbx.Text = "Wrap Around";
            this.WrapAroundChckbx.UseVisualStyleBackColor = true;
            // 
            // FindDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(341, 111);
            this.Controls.Add(this.WrapAroundChckbx);
            this.Controls.Add(this.ModeGrpbx);
            this.Controls.Add(this.DirectionGrpbx);
            this.Controls.Add(this.FindWhatLbl);
            this.Controls.Add(this.MatchWordChckbx);
            this.Controls.Add(this.MatchCaseChckbx);
            this.Controls.Add(this.FindNextBtn);
            this.Controls.Add(this.FindWhatTxt);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FindDialog";
            this.ShowInTaskbar = false;
            this.Text = "Tanji ~ Find";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FindTxt_KeyDown);
            this.DirectionGrpbx.ResumeLayout(false);
            this.DirectionGrpbx.PerformLayout();
            this.ModeGrpbx.ResumeLayout(false);
            this.ModeGrpbx.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Sulakore.Components.SKoreButton FindNextBtn;
        private System.Windows.Forms.CheckBox MatchCaseChckbx;
        private System.Windows.Forms.CheckBox MatchWordChckbx;
        private System.Windows.Forms.Label FindWhatLbl;
        private System.Windows.Forms.GroupBox DirectionGrpbx;
        private System.Windows.Forms.RadioButton DownRd;
        private System.Windows.Forms.RadioButton UpRd;
        private System.Windows.Forms.GroupBox ModeGrpbx;
        private System.Windows.Forms.RadioButton RegExRd;
        private System.Windows.Forms.RadioButton NormalRd;
        private System.Windows.Forms.CheckBox WrapAroundChckbx;
        internal System.Windows.Forms.TextBox FindWhatTxt;
    }
}