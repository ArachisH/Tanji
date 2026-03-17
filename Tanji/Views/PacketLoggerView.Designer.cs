namespace Tanji.Views;

partial class PacketLoggerView
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
        components = new System.ComponentModel.Container();
        viewModelSrc = new System.Windows.Forms.BindingSource(components);
        ((System.ComponentModel.ISupportInitialize)viewModelSrc).BeginInit();
        SuspendLayout();
        // 
        // PacketLoggerView
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        BackColor = System.Drawing.Color.White;
        ClientSize = new System.Drawing.Size(735, 551);
        Name = "PacketLoggerView";
        StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        Text = "Tanji - Packet Logger";
        ((System.ComponentModel.ISupportInitialize)viewModelSrc).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private System.Windows.Forms.BindingSource viewModelSrc;
}