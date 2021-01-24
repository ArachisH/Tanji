namespace Tanji.Services.Toolbox
{
    partial class ToolboxPage
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
            this.Bit16InputLbl = new System.Windows.Forms.Label();
            this.Bit32InputLbl = new System.Windows.Forms.Label();
            this.IntOutputTxt = new System.Windows.Forms.TextBox();
            this.IntInputTxt = new System.Windows.Forms.NumericUpDown();
            this.UShortOutputTxt = new System.Windows.Forms.TextBox();
            this.DecodeIntBtn = new Tangine.Controls.TangineButton();
            this.DecodeUShortBtn = new Tangine.Controls.TangineButton();
            this.UShortInputTxt = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.IntInputTxt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UShortInputTxt)).BeginInit();
            this.SuspendLayout();
            // 
            // Bit16InputLbl
            // 
            this.Bit16InputLbl.AutoSize = true;
            this.Bit16InputLbl.BackColor = System.Drawing.Color.Transparent;
            this.Bit16InputLbl.Location = new System.Drawing.Point(3, 260);
            this.Bit16InputLbl.Name = "Bit16InputLbl";
            this.Bit16InputLbl.Size = new System.Drawing.Size(112, 15);
            this.Bit16InputLbl.TabIndex = 0;
            this.Bit16InputLbl.Text = "16-Bit Integer Input:";
            // 
            // Bit32InputLbl
            // 
            this.Bit32InputLbl.AutoSize = true;
            this.Bit32InputLbl.BackColor = System.Drawing.Color.Transparent;
            this.Bit32InputLbl.Location = new System.Drawing.Point(3, 288);
            this.Bit32InputLbl.Name = "Bit32InputLbl";
            this.Bit32InputLbl.Size = new System.Drawing.Size(112, 15);
            this.Bit32InputLbl.TabIndex = 1;
            this.Bit32InputLbl.Text = "32-Bit Integer Input:";
            // 
            // IntOutputTxt
            // 
            this.IntOutputTxt.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.IntOutputTxt.Location = new System.Drawing.Point(240, 285);
            this.IntOutputTxt.Name = "IntOutputTxt";
            this.IntOutputTxt.Size = new System.Drawing.Size(126, 23);
            this.IntOutputTxt.TabIndex = 15;
            this.IntOutputTxt.Text = "[0][0][0][0]";
            this.IntOutputTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // IntInputTxt
            // 
            this.IntInputTxt.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.IntInputTxt.Location = new System.Drawing.Point(116, 285);
            this.IntInputTxt.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.IntInputTxt.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.IntInputTxt.Name = "IntInputTxt";
            this.IntInputTxt.Size = new System.Drawing.Size(118, 23);
            this.IntInputTxt.TabIndex = 16;
            this.IntInputTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // UShortOutputTxt
            // 
            this.UShortOutputTxt.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.UShortOutputTxt.Location = new System.Drawing.Point(240, 257);
            this.UShortOutputTxt.Name = "UShortOutputTxt";
            this.UShortOutputTxt.Size = new System.Drawing.Size(126, 23);
            this.UShortOutputTxt.TabIndex = 17;
            this.UShortOutputTxt.Text = "[0][0]";
            this.UShortOutputTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // DecodeIntBtn
            // 
            this.DecodeIntBtn.Location = new System.Drawing.Point(372, 285);
            this.DecodeIntBtn.Name = "DecodeIntBtn";
            this.DecodeIntBtn.Size = new System.Drawing.Size(98, 23);
            this.DecodeIntBtn.TabIndex = 19;
            this.DecodeIntBtn.Text = "Decode Int32";
            this.DecodeIntBtn.Click += new System.EventHandler(this.DecodeIntBtn_Click);
            // 
            // DecodeUShortBtn
            // 
            this.DecodeUShortBtn.Location = new System.Drawing.Point(372, 257);
            this.DecodeUShortBtn.Name = "DecodeUShortBtn";
            this.DecodeUShortBtn.Size = new System.Drawing.Size(98, 23);
            this.DecodeUShortBtn.TabIndex = 20;
            this.DecodeUShortBtn.Text = "Decode UInt16";
            this.DecodeUShortBtn.Click += new System.EventHandler(this.DecodeUShortBtn_Click);
            // 
            // UShortInputTxt
            // 
            this.UShortInputTxt.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.UShortInputTxt.Location = new System.Drawing.Point(116, 257);
            this.UShortInputTxt.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.UShortInputTxt.Name = "UShortInputTxt";
            this.UShortInputTxt.Size = new System.Drawing.Size(118, 23);
            this.UShortInputTxt.TabIndex = 21;
            this.UShortInputTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ToolboxPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.UShortInputTxt);
            this.Controls.Add(this.Bit16InputLbl);
            this.Controls.Add(this.Bit32InputLbl);
            this.Controls.Add(this.IntInputTxt);
            this.Controls.Add(this.IntOutputTxt);
            this.Controls.Add(this.UShortOutputTxt);
            this.Controls.Add(this.DecodeIntBtn);
            this.Controls.Add(this.DecodeUShortBtn);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ToolboxPage";
            this.Size = new System.Drawing.Size(476, 313);
            ((System.ComponentModel.ISupportInitialize)(this.IntInputTxt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UShortInputTxt)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Bit16InputLbl;
        private System.Windows.Forms.Label Bit32InputLbl;
        private System.Windows.Forms.TextBox IntOutputTxt;
        private System.Windows.Forms.NumericUpDown IntInputTxt;
        private System.Windows.Forms.TextBox UShortOutputTxt;
        private Tangine.Controls.TangineButton DecodeIntBtn;
        private Tangine.Controls.TangineButton DecodeUShortBtn;
        private System.Windows.Forms.NumericUpDown UShortInputTxt;
    }
}
