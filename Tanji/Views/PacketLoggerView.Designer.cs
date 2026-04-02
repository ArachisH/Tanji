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
        loggerVw = new Tanji.Controls.TanjiPacketViewer();
        mainMenuStrp = new System.Windows.Forms.MenuStrip();
        fileBtn = new System.Windows.Forms.ToolStripMenuItem();
        viewBtn = new System.Windows.Forms.ToolStripMenuItem();
        toolsBtn = new System.Windows.Forms.ToolStripMenuItem();
        mainStatusStrp = new System.Windows.Forms.StatusStrip();
        outgoingPSLbl = new System.Windows.Forms.ToolStripStatusLabel();
        viewModelSrc = new System.Windows.Forms.BindingSource(components);
        incomingPSLbl = new System.Windows.Forms.ToolStripStatusLabel();
        mainMenuStrp.SuspendLayout();
        mainStatusStrp.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)viewModelSrc).BeginInit();
        SuspendLayout();
        // 
        // loggerVw
        // 
        loggerVw.BackColor = System.Drawing.Color.FromArgb(22, 22, 22);
        loggerVw.BorderStyle = System.Windows.Forms.BorderStyle.None;
        loggerVw.Dock = System.Windows.Forms.DockStyle.Fill;
        loggerVw.Font = new System.Drawing.Font("Cascadia Code", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
        loggerVw.ForeColor = System.Drawing.Color.White;
        loggerVw.Location = new System.Drawing.Point(0, 24);
        loggerVw.Name = "loggerVw";
        loggerVw.ReadOnly = true;
        loggerVw.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
        loggerVw.ShowSelectionMargin = true;
        loggerVw.Size = new System.Drawing.Size(1281, 825);
        loggerVw.TabIndex = 1;
        loggerVw.Text = "";
        // 
        // mainMenuStrp
        // 
        mainMenuStrp.BackColor = System.Drawing.Color.White;
        mainMenuStrp.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { fileBtn, viewBtn, toolsBtn });
        mainMenuStrp.Location = new System.Drawing.Point(0, 0);
        mainMenuStrp.Name = "mainMenuStrp";
        mainMenuStrp.Size = new System.Drawing.Size(1281, 24);
        mainMenuStrp.TabIndex = 2;
        // 
        // fileBtn
        // 
        fileBtn.Name = "fileBtn";
        fileBtn.Size = new System.Drawing.Size(37, 20);
        fileBtn.Text = "File";
        // 
        // viewBtn
        // 
        viewBtn.Name = "viewBtn";
        viewBtn.Size = new System.Drawing.Size(44, 20);
        viewBtn.Text = "View";
        // 
        // toolsBtn
        // 
        toolsBtn.Name = "toolsBtn";
        toolsBtn.Size = new System.Drawing.Size(47, 20);
        toolsBtn.Text = "Tools";
        // 
        // mainStatusStrp
        // 
        mainStatusStrp.BackColor = System.Drawing.Color.White;
        mainStatusStrp.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { outgoingPSLbl, incomingPSLbl });
        mainStatusStrp.Location = new System.Drawing.Point(0, 825);
        mainStatusStrp.Name = "mainStatusStrp";
        mainStatusStrp.Size = new System.Drawing.Size(1281, 24);
        mainStatusStrp.TabIndex = 3;
        // 
        // outgoingPSLbl
        // 
        outgoingPSLbl.DataBindings.Add(new System.Windows.Forms.Binding("Text", viewModelSrc, "OutgoingPerSecond", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged, null, "Outgoing 0/Ps"));
        outgoingPSLbl.Name = "outgoingPSLbl";
        outgoingPSLbl.Size = new System.Drawing.Size(84, 19);
        outgoingPSLbl.Text = "Outgoing 0/Ps";
        // 
        // viewModelSrc
        // 
        viewModelSrc.DataSource = typeof(Core.Infrastructure.ViewModels.PacketLoggerViewModel);
        // 
        // incomingPSLbl
        // 
        incomingPSLbl.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
        incomingPSLbl.DataBindings.Add(new System.Windows.Forms.Binding("Text", viewModelSrc, "IncomingPerSecond", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged, null, "Incoming 0/Ps"));
        incomingPSLbl.Name = "incomingPSLbl";
        incomingPSLbl.Size = new System.Drawing.Size(88, 19);
        incomingPSLbl.Text = "Incoming 0/Ps";
        // 
        // PacketLoggerView
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        BackColor = System.Drawing.Color.White;
        ClientSize = new System.Drawing.Size(1281, 849);
        Controls.Add(mainStatusStrp);
        Controls.Add(loggerVw);
        Controls.Add(mainMenuStrp);
        MainMenuStrip = mainMenuStrp;
        Name = "PacketLoggerView";
        StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        Text = "Tanji - Packet Logger";
        mainMenuStrp.ResumeLayout(false);
        mainMenuStrp.PerformLayout();
        mainStatusStrp.ResumeLayout(false);
        mainStatusStrp.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)viewModelSrc).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
    private Controls.TanjiPacketViewer loggerVw;
    private System.Windows.Forms.MenuStrip mainMenuStrp;
    private System.Windows.Forms.ToolStripMenuItem fileBtn;
    private System.Windows.Forms.ToolStripMenuItem viewBtn;
    private System.Windows.Forms.ToolStripMenuItem toolsBtn;
    private System.Windows.Forms.StatusStrip mainStatusStrp;
    private System.Windows.Forms.ToolStripStatusLabel incomingPSLbl;
    private System.Windows.Forms.BindingSource viewModelSrc;
    private System.Windows.Forms.ToolStripStatusLabel outgoingPSLbl;
}