namespace Tanji.Windows
{
    partial class MainFrm
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
            this.TanjiStatusStrip = new System.Windows.Forms.StatusStrip();
            this.VersionLbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.SchedulesLbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.FiltersLbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.ModulesLbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.GitHubLinkLbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.TanjiTabs = new Tangine.Controls.TangineTabControl();
            this.ConnectionTab = new System.Windows.Forms.TabPage();
            this.ConnectionPg = new Tanji.Services.ConnectionPage();
            this.InjectionTab = new System.Windows.Forms.TabPage();
            this.ConstructerTab = new System.Windows.Forms.TabPage();
            this.SchedulerTab = new System.Windows.Forms.TabPage();
            this.InspectorTab = new System.Windows.Forms.TabPage();
            this.FiltersTab = new System.Windows.Forms.TabPage();
            this.ToolboxTab = new System.Windows.Forms.TabPage();
            this.ModulesTab = new System.Windows.Forms.TabPage();
            this.ModulesPg = new Tanji.Services.Modules.ModulesPage();
            this.OptionsTab = new System.Windows.Forms.TabPage();
            this.OptionsTabs = new Tangine.Controls.TangineTabControl();
            this.SettingsTab = new System.Windows.Forms.TabPage();
            this.SettingsPg = new Tanji.Services.Options.SettingsPage();
            this.AboutTab = new System.Windows.Forms.TabPage();
            this.AboutPg = new Tanji.Services.Options.AboutPage();
            this.SendToClientBtn = new Tangine.Controls.TangineButton();
            this.SendToServerBtn = new Tangine.Controls.TangineButton();
            this.PacketTxt = new System.Windows.Forms.ComboBox();
            this.InjectionTabs = new Tangine.Controls.TangineTabControl();
            this.TanjiStatusStrip.SuspendLayout();
            this.TanjiTabs.SuspendLayout();
            this.ConnectionTab.SuspendLayout();
            this.InjectionTab.SuspendLayout();
            this.ModulesTab.SuspendLayout();
            this.OptionsTab.SuspendLayout();
            this.OptionsTabs.SuspendLayout();
            this.SettingsTab.SuspendLayout();
            this.AboutTab.SuspendLayout();
            this.InjectionTabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // TanjiStatusStrip
            // 
            this.TanjiStatusStrip.BackColor = System.Drawing.Color.White;
            this.TanjiStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.VersionLbl,
            this.SchedulesLbl,
            this.FiltersLbl,
            this.ModulesLbl,
            this.GitHubLinkLbl});
            this.TanjiStatusStrip.Location = new System.Drawing.Point(0, 347);
            this.TanjiStatusStrip.Name = "TanjiStatusStrip";
            this.TanjiStatusStrip.Size = new System.Drawing.Size(514, 24);
            this.TanjiStatusStrip.SizingGrip = false;
            this.TanjiStatusStrip.TabIndex = 1;
            // 
            // VersionLbl
            // 
            this.VersionLbl.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.VersionLbl.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.VersionLbl.Name = "VersionLbl";
            this.VersionLbl.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.VersionLbl.Size = new System.Drawing.Size(65, 19);
            this.VersionLbl.Text = "v1.5.0.0";
            // 
            // SchedulesLbl
            // 
            this.SchedulesLbl.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.SchedulesLbl.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.SchedulesLbl.Name = "SchedulesLbl";
            this.SchedulesLbl.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.SchedulesLbl.Size = new System.Drawing.Size(102, 19);
            this.SchedulesLbl.Text = "Schedules: 0/0";
            // 
            // FiltersLbl
            // 
            this.FiltersLbl.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.FiltersLbl.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.FiltersLbl.Name = "FiltersLbl";
            this.FiltersLbl.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.FiltersLbl.Size = new System.Drawing.Size(80, 19);
            this.FiltersLbl.Text = "Filters: 0/0";
            // 
            // ModulesLbl
            // 
            this.ModulesLbl.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.ModulesLbl.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ModulesLbl.Name = "ModulesLbl";
            this.ModulesLbl.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.ModulesLbl.Size = new System.Drawing.Size(95, 19);
            this.ModulesLbl.Text = "Modules: 0/0";
            // 
            // GitHubLinkLbl
            // 
            this.GitHubLinkLbl.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.GitHubLinkLbl.Name = "GitHubLinkLbl";
            this.GitHubLinkLbl.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.GitHubLinkLbl.Size = new System.Drawing.Size(157, 19);
            this.GitHubLinkLbl.Spring = true;
            this.GitHubLinkLbl.Text = "GitHub - ArachisH/Tanji";
            // 
            // TanjiTabs
            // 
            this.TanjiTabs.Controls.Add(this.ConnectionTab);
            this.TanjiTabs.Controls.Add(this.InjectionTab);
            this.TanjiTabs.Controls.Add(this.ToolboxTab);
            this.TanjiTabs.Controls.Add(this.ModulesTab);
            this.TanjiTabs.Controls.Add(this.OptionsTab);
            this.TanjiTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TanjiTabs.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.TanjiTabs.IsDisplayingBorder = true;
            this.TanjiTabs.ItemSize = new System.Drawing.Size(102, 24);
            this.TanjiTabs.Location = new System.Drawing.Point(0, 0);
            this.TanjiTabs.Name = "TanjiTabs";
            this.TanjiTabs.SelectedIndex = 0;
            this.TanjiTabs.Size = new System.Drawing.Size(514, 347);
            this.TanjiTabs.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.TanjiTabs.TabIndex = 0;
            // 
            // ConnectionTab
            // 
            this.ConnectionTab.Controls.Add(this.ConnectionPg);
            this.ConnectionTab.Location = new System.Drawing.Point(4, 28);
            this.ConnectionTab.Name = "ConnectionTab";
            this.ConnectionTab.Size = new System.Drawing.Size(506, 315);
            this.ConnectionTab.TabIndex = 0;
            this.ConnectionTab.Text = "Connection";
            this.ConnectionTab.UseVisualStyleBackColor = true;
            // 
            // ConnectionPg
            // 
            this.ConnectionPg.BackColor = System.Drawing.Color.White;
            this.ConnectionPg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ConnectionPg.IsReceiving = false;
            this.ConnectionPg.Location = new System.Drawing.Point(0, 0);
            this.ConnectionPg.Name = "ConnectionPg";
            this.ConnectionPg.Size = new System.Drawing.Size(506, 315);
            this.ConnectionPg.TabIndex = 0;
            this.ConnectionPg.TabStop = false;
            // 
            // InjectionTab
            // 
            this.InjectionTab.Controls.Add(this.InjectionTabs);
            this.InjectionTab.Controls.Add(this.PacketTxt);
            this.InjectionTab.Controls.Add(this.SendToServerBtn);
            this.InjectionTab.Controls.Add(this.SendToClientBtn);
            this.InjectionTab.Location = new System.Drawing.Point(4, 28);
            this.InjectionTab.Name = "InjectionTab";
            this.InjectionTab.Size = new System.Drawing.Size(506, 315);
            this.InjectionTab.TabIndex = 1;
            this.InjectionTab.Text = "Injection";
            this.InjectionTab.UseVisualStyleBackColor = true;
            // 
            // ToolboxTab
            // 
            this.ToolboxTab.Location = new System.Drawing.Point(4, 28);
            this.ToolboxTab.Name = "ToolboxTab";
            this.ToolboxTab.Size = new System.Drawing.Size(506, 315);
            this.ToolboxTab.TabIndex = 2;
            this.ToolboxTab.Text = "Toolbox";
            this.ToolboxTab.UseVisualStyleBackColor = true;
            // 
            // ModulesTab
            // 
            this.ModulesTab.Controls.Add(this.ModulesPg);
            this.ModulesTab.Location = new System.Drawing.Point(4, 28);
            this.ModulesTab.Name = "ModulesTab";
            this.ModulesTab.Size = new System.Drawing.Size(506, 315);
            this.ModulesTab.TabIndex = 3;
            this.ModulesTab.Text = "Modules";
            this.ModulesTab.UseVisualStyleBackColor = true;
            // 
            // ModulesPg
            // 
            this.ModulesPg.BackColor = System.Drawing.Color.White;
            this.ModulesPg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ModulesPg.Location = new System.Drawing.Point(0, 0);
            this.ModulesPg.Name = "ModulesPg";
            this.ModulesPg.SelectedModule = null;
            this.ModulesPg.Size = new System.Drawing.Size(506, 315);
            this.ModulesPg.TabIndex = 0;
            this.ModulesPg.TabStop = false;
            // 
            // OptionsTab
            // 
            this.OptionsTab.Controls.Add(this.OptionsTabs);
            this.OptionsTab.Location = new System.Drawing.Point(4, 28);
            this.OptionsTab.Name = "OptionsTab";
            this.OptionsTab.Size = new System.Drawing.Size(506, 315);
            this.OptionsTab.TabIndex = 4;
            this.OptionsTab.Text = "Options";
            this.OptionsTab.UseVisualStyleBackColor = true;
            // 
            // OptionsTabs
            // 
            this.OptionsTabs.Alignment = System.Windows.Forms.TabAlignment.Right;
            this.OptionsTabs.Controls.Add(this.SettingsTab);
            this.OptionsTabs.Controls.Add(this.AboutTab);
            this.OptionsTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OptionsTabs.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.OptionsTabs.ItemSize = new System.Drawing.Size(25, 47);
            this.OptionsTabs.Location = new System.Drawing.Point(0, 0);
            this.OptionsTabs.Multiline = true;
            this.OptionsTabs.Name = "OptionsTabs";
            this.OptionsTabs.SelectedIndex = 0;
            this.OptionsTabs.Size = new System.Drawing.Size(506, 315);
            this.OptionsTabs.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.OptionsTabs.TabIndex = 0;
            // 
            // SettingsTab
            // 
            this.SettingsTab.Controls.Add(this.SettingsPg);
            this.SettingsTab.Location = new System.Drawing.Point(4, 4);
            this.SettingsTab.Name = "SettingsTab";
            this.SettingsTab.Size = new System.Drawing.Size(451, 307);
            this.SettingsTab.TabIndex = 0;
            this.SettingsTab.Text = "Settings";
            this.SettingsTab.UseVisualStyleBackColor = true;
            // 
            // SettingsPg
            // 
            this.SettingsPg.BackColor = System.Drawing.Color.White;
            this.SettingsPg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SettingsPg.Location = new System.Drawing.Point(0, 0);
            this.SettingsPg.Name = "SettingsPg";
            this.SettingsPg.Size = new System.Drawing.Size(451, 307);
            this.SettingsPg.TabIndex = 0;
            this.SettingsPg.TabStop = false;
            // 
            // AboutTab
            // 
            this.AboutTab.Controls.Add(this.AboutPg);
            this.AboutTab.Location = new System.Drawing.Point(4, 4);
            this.AboutTab.Name = "AboutTab";
            this.AboutTab.Size = new System.Drawing.Size(451, 307);
            this.AboutTab.TabIndex = 1;
            this.AboutTab.Text = "About";
            this.AboutTab.UseVisualStyleBackColor = true;
            // 
            // AboutPg
            // 
            this.AboutPg.BackColor = System.Drawing.Color.White;
            this.AboutPg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AboutPg.Location = new System.Drawing.Point(0, 0);
            this.AboutPg.Name = "AboutPg";
            this.AboutPg.Size = new System.Drawing.Size(451, 307);
            this.AboutPg.TabIndex = 0;
            this.AboutPg.TabStop = false;
            // 
            // SendToClientBtn
            // 
            this.SendToClientBtn.Location = new System.Drawing.Point(297, 292);
            this.SendToClientBtn.Name = "SendToClientBtn";
            this.SendToClientBtn.Size = new System.Drawing.Size(100, 20);
            this.SendToClientBtn.TabIndex = 2;
            this.SendToClientBtn.Text = "Send To Client";
            // 
            // SendToServerBtn
            // 
            this.SendToServerBtn.Location = new System.Drawing.Point(403, 292);
            this.SendToServerBtn.Name = "SendToServerBtn";
            this.SendToServerBtn.Size = new System.Drawing.Size(100, 20);
            this.SendToServerBtn.TabIndex = 3;
            this.SendToServerBtn.Text = "Send To Server";
            // 
            // PacketTxt
            // 
            this.PacketTxt.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.PacketTxt.FormattingEnabled = true;
            this.PacketTxt.ItemHeight = 14;
            this.PacketTxt.Location = new System.Drawing.Point(3, 292);
            this.PacketTxt.Name = "PacketTxt";
            this.PacketTxt.Size = new System.Drawing.Size(288, 20);
            this.PacketTxt.TabIndex = 1;
            this.PacketTxt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PacketTxt_KeyDown);
            // 
            // InjectionTabs
            // 
            this.InjectionTabs.Alignment = System.Windows.Forms.TabAlignment.Right;
            this.InjectionTabs.Controls.Add(this.ConstructerTab);
            this.InjectionTabs.Controls.Add(this.SchedulerTab);
            this.InjectionTabs.Controls.Add(this.InspectorTab);
            this.InjectionTabs.Controls.Add(this.FiltersTab);
            this.InjectionTabs.Dock = System.Windows.Forms.DockStyle.Top;
            this.InjectionTabs.ItemSize = new System.Drawing.Size(25, 65);
            this.InjectionTabs.Location = new System.Drawing.Point(0, 0);
            this.InjectionTabs.Multiline = true;
            this.InjectionTabs.Name = "InjectionTabs";
            this.InjectionTabs.SelectedIndex = 0;
            this.InjectionTabs.Size = new System.Drawing.Size(506, 286);
            this.InjectionTabs.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.InjectionTabs.TabIndex = 4;
            // 
            // ConstructerTab
            // 
            this.ConstructerTab.Location = new System.Drawing.Point(4, 4);
            this.ConstructerTab.Name = "ConstructerTab";
            this.ConstructerTab.Size = new System.Drawing.Size(433, 278);
            this.ConstructerTab.TabIndex = 0;
            this.ConstructerTab.Text = "Constructer";
            this.ConstructerTab.UseVisualStyleBackColor = true;
            this.ConstructerTab.Controls.Add(new Tanji.Services.Injection.ConstructerPage());
            // 
            // SchedulerTab
            // 
            this.SchedulerTab.Location = new System.Drawing.Point(4, 4);
            this.SchedulerTab.Name = "SchedulerTab";
            this.SchedulerTab.Padding = new System.Windows.Forms.Padding(3);
            this.SchedulerTab.Size = new System.Drawing.Size(433, 278);
            this.SchedulerTab.TabIndex = 1;
            this.SchedulerTab.Text = "Scheduler";
            this.SchedulerTab.UseVisualStyleBackColor = true;
            this.SchedulerTab.Controls.Add(new Tanji.Services.Injection.SchedulerPage());
            // 
            // InspectorTab
            // 
            this.InspectorTab.Location = new System.Drawing.Point(4, 4);
            this.InspectorTab.Name = "InspectorTab";
            this.InspectorTab.Padding = new System.Windows.Forms.Padding(3);
            this.InspectorTab.Size = new System.Drawing.Size(433, 278);
            this.InspectorTab.TabIndex = 2;
            this.InspectorTab.Text = "Inspector";
            this.InspectorTab.UseVisualStyleBackColor = true;
            this.InspectorTab.Controls.Add(new Tanji.Services.Injection.InspectorPage());
            // 
            // FiltersTab
            // 
            this.FiltersTab.Location = new System.Drawing.Point(4, 4);
            this.FiltersTab.Name = "FiltersTab";
            this.FiltersTab.Padding = new System.Windows.Forms.Padding(3);
            this.FiltersTab.Size = new System.Drawing.Size(433, 278);
            this.FiltersTab.TabIndex = 3;
            this.FiltersTab.Text = "Filters";
            this.FiltersTab.UseVisualStyleBackColor = true;
            this.FiltersTab.Controls.Add(new Tanji.Services.Injection.FiltersPage());
            // 
            // MainFrm
            // 
            this.Font = Program.DefaultFont;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(514, 371);
            this.Controls.Add(this.TanjiTabs);
            this.Controls.Add(this.TanjiStatusStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainFrm";
            this.Text = "Tanji - Disconnected";
            this.TopMost = true;
            this.TanjiStatusStrip.ResumeLayout(false);
            this.TanjiStatusStrip.PerformLayout();
            this.TanjiTabs.ResumeLayout(false);
            this.ConnectionTab.ResumeLayout(false);
            this.InjectionTab.ResumeLayout(false);
            this.ModulesTab.ResumeLayout(false);
            this.OptionsTab.ResumeLayout(false);
            this.OptionsTabs.ResumeLayout(false);
            this.SettingsTab.ResumeLayout(false);
            this.AboutTab.ResumeLayout(false);
            this.InjectionTabs.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Tangine.Controls.TangineTabControl TanjiTabs;
        private System.Windows.Forms.TabPage ConnectionTab;
        private System.Windows.Forms.TabPage InjectionTab;
        private System.Windows.Forms.TabPage ToolboxTab;
        private System.Windows.Forms.TabPage ModulesTab;
        private System.Windows.Forms.TabPage OptionsTab;
        private Services.ConnectionPage ConnectionPg;
        private Tangine.Controls.TangineTabControl OptionsTabs;
        private System.Windows.Forms.TabPage SettingsTab;
        private Services.Options.SettingsPage SettingsPg;
        private System.Windows.Forms.TabPage AboutTab;
        private Services.Options.AboutPage AboutPg;
        private System.Windows.Forms.StatusStrip TanjiStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel VersionLbl;
        private System.Windows.Forms.ToolStripStatusLabel SchedulesLbl;
        private System.Windows.Forms.ToolStripStatusLabel FiltersLbl;
        private System.Windows.Forms.ToolStripStatusLabel ModulesLbl;
        private Services.Modules.ModulesPage ModulesPg;
        private System.Windows.Forms.ToolStripStatusLabel GitHubLinkLbl;
        private System.Windows.Forms.ComboBox PacketTxt;
        private Tangine.Controls.TangineButton SendToServerBtn;
        private Tangine.Controls.TangineButton SendToClientBtn;
        private Tangine.Controls.TangineTabControl InjectionTabs;
        private System.Windows.Forms.TabPage ConstructerTab;
        private System.Windows.Forms.TabPage SchedulerTab;
        private System.Windows.Forms.TabPage InspectorTab;
        private System.Windows.Forms.TabPage FiltersTab;
    }
}