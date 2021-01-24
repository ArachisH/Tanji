namespace Tanji.Services.Injection
{
    partial class InspectorPage
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
            this.SaveAsBtn = new Tangine.Controls.TangineButton();
            this.LengthTxt = new Tangine.Controls.TangineLabelBox();
            this.HeaderTxt = new Tangine.Controls.TangineLabelBox();
            this.CorruptedTxt = new Tangine.Controls.TangineLabelBox();
            this.PacketTxt = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // SaveAsBtn
            // 
            this.SaveAsBtn.Location = new System.Drawing.Point(304, 241);
            this.SaveAsBtn.Name = "SaveAsBtn";
            this.SaveAsBtn.Size = new System.Drawing.Size(87, 20);
            this.SaveAsBtn.TabIndex = 0;
            this.SaveAsBtn.Text = "Save As";
            this.SaveAsBtn.Click += new System.EventHandler(this.SaveAsBtn_Click);
            // 
            // LengthTxt
            // 
            this.LengthTxt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.LengthTxt.IsReadOnly = true;
            this.LengthTxt.Location = new System.Drawing.Point(103, 241);
            this.LengthTxt.Name = "LengthTxt";
            this.LengthTxt.Size = new System.Drawing.Size(91, 20);
            this.LengthTxt.TabIndex = 2;
            this.LengthTxt.Text = "0";
            this.LengthTxt.TextPaddingWidth = 0;
            this.LengthTxt.Title = "Length";
            // 
            // HeaderTxt
            // 
            this.HeaderTxt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.HeaderTxt.IsReadOnly = true;
            this.HeaderTxt.Location = new System.Drawing.Point(6, 241);
            this.HeaderTxt.Name = "HeaderTxt";
            this.HeaderTxt.Size = new System.Drawing.Size(91, 20);
            this.HeaderTxt.TabIndex = 3;
            this.HeaderTxt.Text = "0";
            this.HeaderTxt.TextPaddingWidth = 0;
            this.HeaderTxt.Title = "Header";
            // 
            // CorruptedTxt
            // 
            this.CorruptedTxt.BackColor = System.Drawing.Color.Firebrick;
            this.CorruptedTxt.IsReadOnly = true;
            this.CorruptedTxt.Location = new System.Drawing.Point(200, 241);
            this.CorruptedTxt.Name = "CorruptedTxt";
            this.CorruptedTxt.Size = new System.Drawing.Size(98, 20);
            this.CorruptedTxt.TabIndex = 4;
            this.CorruptedTxt.Text = "True";
            this.CorruptedTxt.TextPaddingWidth = 0;
            this.CorruptedTxt.Title = "Corrupted";
            // 
            // PacketTxt
            // 
            this.PacketTxt.Location = new System.Drawing.Point(6, 6);
            this.PacketTxt.MaxLength = 2147483647;
            this.PacketTxt.Multiline = true;
            this.PacketTxt.Name = "PacketTxt";
            this.PacketTxt.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.PacketTxt.Size = new System.Drawing.Size(385, 229);
            this.PacketTxt.TabIndex = 6;
            // 
            // InspectorPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SaveAsBtn);
            this.Controls.Add(this.LengthTxt);
            this.Controls.Add(this.HeaderTxt);
            this.Controls.Add(this.CorruptedTxt);
            this.Controls.Add(this.PacketTxt);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "InspectorPage";
            this.Size = new System.Drawing.Size(397, 270);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Tangine.Controls.TangineButton SaveAsBtn;
        private Tangine.Controls.TangineLabelBox LengthTxt;
        private Tangine.Controls.TangineLabelBox HeaderTxt;
        private Tangine.Controls.TangineLabelBox CorruptedTxt;
        private System.Windows.Forms.TextBox PacketTxt;
    }
}
