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
            this.FindWhatTxt = new Tangine.Controls.TangineLabelBox();
            this.FindBtn = new Tangine.Controls.TangineButton();
            this.SearchModeGrbx = new System.Windows.Forms.GroupBox();
            this.RegularExpressionRd = new System.Windows.Forms.RadioButton();
            this.NormalRd = new System.Windows.Forms.RadioButton();
            this.WrapAroundChbx = new System.Windows.Forms.CheckBox();
            this.MatchWordChbx = new System.Windows.Forms.CheckBox();
            this.MatchCaseChbx = new System.Windows.Forms.CheckBox();
            this.DirectionGrpbx = new System.Windows.Forms.GroupBox();
            this.DownRd = new System.Windows.Forms.RadioButton();
            this.UpRd = new System.Windows.Forms.RadioButton();
            this.SearchModeGrbx.SuspendLayout();
            this.DirectionGrpbx.SuspendLayout();
            this.SuspendLayout();
            // 
            // FindWhatTxt
            // 
            this.FindWhatTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FindWhatTxt.Location = new System.Drawing.Point(12, 12);
            this.FindWhatTxt.Name = "FindWhatTxt";
            this.FindWhatTxt.Size = new System.Drawing.Size(305, 20);
            this.FindWhatTxt.TabIndex = 0;
            this.FindWhatTxt.Text = "";
            this.FindWhatTxt.TextPaddingWidth = 0;
            this.FindWhatTxt.Title = "Find What";
            this.FindWhatTxt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FindWhatTxt_KeyDown);
            // 
            // FindBtn
            // 
            this.FindBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FindBtn.Location = new System.Drawing.Point(323, 12);
            this.FindBtn.Name = "FindBtn";
            this.FindBtn.Size = new System.Drawing.Size(70, 20);
            this.FindBtn.TabIndex = 2;
            this.FindBtn.Text = "Find";
            this.FindBtn.Click += new System.EventHandler(this.FindBtn_Click);
            // 
            // SearchModeGrbx
            // 
            this.SearchModeGrbx.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SearchModeGrbx.Controls.Add(this.RegularExpressionRd);
            this.SearchModeGrbx.Controls.Add(this.NormalRd);
            this.SearchModeGrbx.Location = new System.Drawing.Point(109, 38);
            this.SearchModeGrbx.Name = "SearchModeGrbx";
            this.SearchModeGrbx.Size = new System.Drawing.Size(198, 65);
            this.SearchModeGrbx.TabIndex = 13;
            this.SearchModeGrbx.TabStop = false;
            this.SearchModeGrbx.Text = "Search Mode";
            // 
            // RegularExpressionRd
            // 
            this.RegularExpressionRd.AutoSize = true;
            this.RegularExpressionRd.Location = new System.Drawing.Point(6, 40);
            this.RegularExpressionRd.Name = "RegularExpressionRd";
            this.RegularExpressionRd.Size = new System.Drawing.Size(124, 19);
            this.RegularExpressionRd.TabIndex = 1;
            this.RegularExpressionRd.Text = "Regular Expression";
            this.RegularExpressionRd.UseVisualStyleBackColor = true;
            // 
            // NormalRd
            // 
            this.NormalRd.AutoSize = true;
            this.NormalRd.Checked = true;
            this.NormalRd.Location = new System.Drawing.Point(7, 20);
            this.NormalRd.Name = "NormalRd";
            this.NormalRd.Size = new System.Drawing.Size(65, 19);
            this.NormalRd.TabIndex = 0;
            this.NormalRd.TabStop = true;
            this.NormalRd.Text = "Normal";
            this.NormalRd.UseVisualStyleBackColor = true;
            // 
            // WrapAroundChbx
            // 
            this.WrapAroundChbx.AutoSize = true;
            this.WrapAroundChbx.Checked = true;
            this.WrapAroundChbx.CheckState = System.Windows.Forms.CheckState.Checked;
            this.WrapAroundChbx.Location = new System.Drawing.Point(12, 83);
            this.WrapAroundChbx.Name = "WrapAroundChbx";
            this.WrapAroundChbx.Size = new System.Drawing.Size(97, 19);
            this.WrapAroundChbx.TabIndex = 16;
            this.WrapAroundChbx.Text = "Wrap Around";
            this.WrapAroundChbx.UseVisualStyleBackColor = true;
            // 
            // MatchWordChbx
            // 
            this.MatchWordChbx.AutoSize = true;
            this.MatchWordChbx.Location = new System.Drawing.Point(12, 63);
            this.MatchWordChbx.Name = "MatchWordChbx";
            this.MatchWordChbx.Size = new System.Drawing.Size(92, 19);
            this.MatchWordChbx.TabIndex = 15;
            this.MatchWordChbx.Text = "Match Word";
            this.MatchWordChbx.UseVisualStyleBackColor = true;
            // 
            // MatchCaseChbx
            // 
            this.MatchCaseChbx.AutoSize = true;
            this.MatchCaseChbx.Location = new System.Drawing.Point(12, 43);
            this.MatchCaseChbx.Name = "MatchCaseChbx";
            this.MatchCaseChbx.Size = new System.Drawing.Size(88, 19);
            this.MatchCaseChbx.TabIndex = 14;
            this.MatchCaseChbx.Text = "Match Case";
            this.MatchCaseChbx.UseVisualStyleBackColor = false;
            // 
            // DirectionGrpbx
            // 
            this.DirectionGrpbx.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DirectionGrpbx.Controls.Add(this.DownRd);
            this.DirectionGrpbx.Controls.Add(this.UpRd);
            this.DirectionGrpbx.Location = new System.Drawing.Point(313, 38);
            this.DirectionGrpbx.Name = "DirectionGrpbx";
            this.DirectionGrpbx.Size = new System.Drawing.Size(80, 65);
            this.DirectionGrpbx.TabIndex = 17;
            this.DirectionGrpbx.TabStop = false;
            this.DirectionGrpbx.Text = "Direction";
            // 
            // DownRd
            // 
            this.DownRd.AutoSize = true;
            this.DownRd.Checked = true;
            this.DownRd.Location = new System.Drawing.Point(7, 40);
            this.DownRd.Name = "DownRd";
            this.DownRd.Size = new System.Drawing.Size(56, 19);
            this.DownRd.TabIndex = 1;
            this.DownRd.TabStop = true;
            this.DownRd.Text = "Down";
            this.DownRd.UseVisualStyleBackColor = true;
            // 
            // UpRd
            // 
            this.UpRd.AutoSize = true;
            this.UpRd.Location = new System.Drawing.Point(7, 20);
            this.UpRd.Name = "UpRd";
            this.UpRd.Size = new System.Drawing.Size(40, 19);
            this.UpRd.TabIndex = 0;
            this.UpRd.Text = "Up";
            this.UpRd.UseVisualStyleBackColor = true;
            // 
            // FindDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(405, 115);
            this.Controls.Add(this.MatchWordChbx);
            this.Controls.Add(this.MatchCaseChbx);
            this.Controls.Add(this.DirectionGrpbx);
            this.Controls.Add(this.WrapAroundChbx);
            this.Controls.Add(this.SearchModeGrbx);
            this.Controls.Add(this.FindBtn);
            this.Controls.Add(this.FindWhatTxt);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FindDialog";
            this.Text = "Tanji - Find";
            this.SearchModeGrbx.ResumeLayout(false);
            this.SearchModeGrbx.PerformLayout();
            this.DirectionGrpbx.ResumeLayout(false);
            this.DirectionGrpbx.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Tangine.Controls.TangineLabelBox FindWhatTxt;
        private Tangine.Controls.TangineButton FindBtn;
        private System.Windows.Forms.GroupBox SearchModeGrbx;
        private System.Windows.Forms.RadioButton RegularExpressionRd;
        private System.Windows.Forms.RadioButton NormalRd;
        private System.Windows.Forms.CheckBox WrapAroundChbx;
        private System.Windows.Forms.CheckBox MatchWordChbx;
        private System.Windows.Forms.CheckBox MatchCaseChbx;
        private System.Windows.Forms.GroupBox DirectionGrpbx;
        private System.Windows.Forms.RadioButton DownRd;
        private System.Windows.Forms.RadioButton UpRd;
    }
}