namespace Tanji.Views.Pages;

partial class ConnectionView
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
        components = new System.ComponentModel.Container();
        viewModelSource = new System.Windows.Forms.BindingSource(components);
        ((System.ComponentModel.ISupportInitialize)viewModelSource).BeginInit();
        SuspendLayout();
        // 
        // viewModelSource
        // 
        viewModelSource.DataSource = typeof(Core.ViewModels.ConnectionViewModel);
        // 
        // ConnectionView
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        Name = "ConnectionView";
        ((System.ComponentModel.ISupportInitialize)viewModelSource).EndInit();
        ResumeLayout(false);
    }

    #endregion
    private System.Windows.Forms.BindingSource viewModelSource;
}
