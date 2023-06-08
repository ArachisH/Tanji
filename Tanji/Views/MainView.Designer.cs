namespace Tanji.Views;

partial class MainView
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
        tabControlMain = new Controls.TanjiTabControl();
        tabConnection = new System.Windows.Forms.TabPage();
        connectionView = new Pages.ConnectionView();
        tabInjection = new System.Windows.Forms.TabPage();
        injectionView = new Pages.InjectionView();
        tabToolbox = new System.Windows.Forms.TabPage();
        toolboxView = new Pages.ToolboxView();
        tabExtensions = new System.Windows.Forms.TabPage();
        tabSettings = new System.Windows.Forms.TabPage();
        statusStripMain = new System.Windows.Forms.StatusStrip();
        extensionsView = new Pages.ExtensionsView();
        settingsViews = new Pages.SettingsViews();
        tabControlMain.SuspendLayout();
        tabConnection.SuspendLayout();
        tabInjection.SuspendLayout();
        tabToolbox.SuspendLayout();
        tabExtensions.SuspendLayout();
        tabSettings.SuspendLayout();
        SuspendLayout();
        // 
        // tabControlMain
        // 
        tabControlMain.Controls.Add(tabConnection);
        tabControlMain.Controls.Add(tabInjection);
        tabControlMain.Controls.Add(tabToolbox);
        tabControlMain.Controls.Add(tabExtensions);
        tabControlMain.Controls.Add(tabSettings);
        tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
        tabControlMain.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
        tabControlMain.HotTrack = true;
        tabControlMain.IsDisplayingBorder = true;
        tabControlMain.ItemSize = new System.Drawing.Size(108, 24);
        tabControlMain.Location = new System.Drawing.Point(0, 0);
        tabControlMain.Name = "tabControlMain";
        tabControlMain.SelectedIndex = 0;
        tabControlMain.Size = new System.Drawing.Size(544, 419);
        tabControlMain.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
        tabControlMain.TabIndex = 0;
        // 
        // tabConnection
        // 
        tabConnection.BackColor = System.Drawing.Color.White;
        tabConnection.Controls.Add(connectionView);
        tabConnection.Location = new System.Drawing.Point(4, 28);
        tabConnection.Name = "tabConnection";
        tabConnection.Size = new System.Drawing.Size(536, 387);
        tabConnection.TabIndex = 1;
        tabConnection.Text = "Connection";
        // 
        // connectionView
        // 
        connectionView.BackColor = System.Drawing.Color.DarkSlateGray;
        connectionView.Dock = System.Windows.Forms.DockStyle.Fill;
        connectionView.Location = new System.Drawing.Point(0, 0);
        connectionView.Name = "connectionView";
        connectionView.Size = new System.Drawing.Size(536, 387);
        connectionView.TabIndex = 0;
        // 
        // tabInjection
        // 
        tabInjection.BackColor = System.Drawing.Color.White;
        tabInjection.Controls.Add(injectionView);
        tabInjection.Location = new System.Drawing.Point(4, 28);
        tabInjection.Name = "tabInjection";
        tabInjection.Size = new System.Drawing.Size(536, 387);
        tabInjection.TabIndex = 2;
        tabInjection.Text = "Injection";
        // 
        // injectionView
        // 
        injectionView.BackColor = System.Drawing.Color.DarkSlateGray;
        injectionView.Dock = System.Windows.Forms.DockStyle.Fill;
        injectionView.Location = new System.Drawing.Point(0, 0);
        injectionView.Name = "injectionView";
        injectionView.Size = new System.Drawing.Size(536, 387);
        injectionView.TabIndex = 0;
        // 
        // tabToolbox
        // 
        tabToolbox.BackColor = System.Drawing.Color.White;
        tabToolbox.Controls.Add(toolboxView);
        tabToolbox.Location = new System.Drawing.Point(4, 28);
        tabToolbox.Name = "tabToolbox";
        tabToolbox.Size = new System.Drawing.Size(536, 387);
        tabToolbox.TabIndex = 3;
        tabToolbox.Text = "Toolbox";
        // 
        // toolboxView
        // 
        toolboxView.BackColor = System.Drawing.Color.DarkSlateGray;
        toolboxView.Dock = System.Windows.Forms.DockStyle.Fill;
        toolboxView.Location = new System.Drawing.Point(0, 0);
        toolboxView.Name = "toolboxView";
        toolboxView.Size = new System.Drawing.Size(536, 387);
        toolboxView.TabIndex = 0;
        // 
        // tabExtensions
        // 
        tabExtensions.BackColor = System.Drawing.Color.White;
        tabExtensions.Controls.Add(extensionsView);
        tabExtensions.Location = new System.Drawing.Point(4, 28);
        tabExtensions.Name = "tabExtensions";
        tabExtensions.Size = new System.Drawing.Size(536, 387);
        tabExtensions.TabIndex = 4;
        tabExtensions.Text = "Extensions";
        // 
        // tabSettings
        // 
        tabSettings.BackColor = System.Drawing.Color.White;
        tabSettings.Controls.Add(settingsViews);
        tabSettings.Location = new System.Drawing.Point(4, 28);
        tabSettings.Name = "tabSettings";
        tabSettings.Size = new System.Drawing.Size(536, 387);
        tabSettings.TabIndex = 0;
        tabSettings.Text = "Settings";
        // 
        // statusStripMain
        // 
        statusStripMain.BackColor = System.Drawing.Color.White;
        statusStripMain.Location = new System.Drawing.Point(0, 419);
        statusStripMain.Name = "statusStripMain";
        statusStripMain.Size = new System.Drawing.Size(544, 22);
        statusStripMain.SizingGrip = false;
        statusStripMain.TabIndex = 1;
        // 
        // extensionsView
        // 
        extensionsView.BackColor = System.Drawing.Color.DarkSlateGray;
        extensionsView.Dock = System.Windows.Forms.DockStyle.Fill;
        extensionsView.Location = new System.Drawing.Point(0, 0);
        extensionsView.Name = "extensionsView";
        extensionsView.Size = new System.Drawing.Size(536, 387);
        extensionsView.TabIndex = 0;
        // 
        // settingsViews
        // 
        settingsViews.BackColor = System.Drawing.Color.DarkSlateGray;
        settingsViews.Dock = System.Windows.Forms.DockStyle.Fill;
        settingsViews.Location = new System.Drawing.Point(0, 0);
        settingsViews.Name = "settingsViews";
        settingsViews.Size = new System.Drawing.Size(536, 387);
        settingsViews.TabIndex = 0;
        // 
        // MainView
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        BackColor = System.Drawing.Color.White;
        ClientSize = new System.Drawing.Size(544, 441);
        Controls.Add(tabControlMain);
        Controls.Add(statusStripMain);
        FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        Name = "MainView";
        StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        Text = "Tanji - Disconnected";
        tabControlMain.ResumeLayout(false);
        tabConnection.ResumeLayout(false);
        tabInjection.ResumeLayout(false);
        tabToolbox.ResumeLayout(false);
        tabExtensions.ResumeLayout(false);
        tabSettings.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Controls.TanjiTabControl tabControlMain;
    private System.Windows.Forms.TabPage tabSettings;
    private System.Windows.Forms.TabPage tabConnection;
    private System.Windows.Forms.TabPage tabInjection;
    private System.Windows.Forms.TabPage tabToolbox;
    private System.Windows.Forms.TabPage tabExtensions;
    private System.Windows.Forms.StatusStrip statusStripMain;
    private Pages.InjectionView injectionView;
    private Pages.ConnectionView connectionView;
    private Pages.ToolboxView toolboxView;
    private Pages.ExtensionsView extensionsView;
    private Pages.SettingsViews settingsViews;
}