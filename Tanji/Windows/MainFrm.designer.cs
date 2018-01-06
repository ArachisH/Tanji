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
            this.TanjiTabs = new Tangine.Controls.TangineTabControl();
            this.ConnectionTab = new System.Windows.Forms.TabPage();
            this.ConnectionPg = new Tanji.Services.ConnectionPage();
            this.InjectionTab = new System.Windows.Forms.TabPage();
            this.InjectionTabs = new Tangine.Controls.TangineTabControl();
            this.ConstructerTab = new System.Windows.Forms.TabPage();
            this.ConstructerPg = new Tanji.Services.Injection.ConstructerPage();
            this.SchedulerTab = new System.Windows.Forms.TabPage();
            this.SchedulerPg = new Tanji.Services.Injection.SchedulerPage();
            this.InspectorTab = new System.Windows.Forms.TabPage();
            this.inspectorPage1 = new Tanji.Services.Injection.InspectorPage();
            this.FiltersTab = new System.Windows.Forms.TabPage();
            this.filtersPage1 = new Tanji.Services.Injection.FiltersPage();
            this.ToolboxTab = new System.Windows.Forms.TabPage();
            this.ModulesTab = new System.Windows.Forms.TabPage();
            this.modulesPage1 = new Tanji.Services.ModulesPage();
            this.OptionsTab = new System.Windows.Forms.TabPage();
            this.OptionsTabs = new Tangine.Controls.TangineTabControl();
            this.SettingsTab = new System.Windows.Forms.TabPage();
            this.settingsPage1 = new Tanji.Services.Options.SettingsPage();
            this.AboutTab = new System.Windows.Forms.TabPage();
            this.aboutPage1 = new Tanji.Services.Options.AboutPage();
            this.TanjiStatusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.TanjiTabs.SuspendLayout();
            this.ConnectionTab.SuspendLayout();
            this.InjectionTab.SuspendLayout();
            this.InjectionTabs.SuspendLayout();
            this.ConstructerTab.SuspendLayout();
            this.SchedulerTab.SuspendLayout();
            this.InspectorTab.SuspendLayout();
            this.FiltersTab.SuspendLayout();
            this.ModulesTab.SuspendLayout();
            this.OptionsTab.SuspendLayout();
            this.OptionsTabs.SuspendLayout();
            this.SettingsTab.SuspendLayout();
            this.AboutTab.SuspendLayout();
            this.TanjiStatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // TanjiTabs
            // 
            this.TanjiTabs.Controls.Add(this.ConnectionTab);
            this.TanjiTabs.Controls.Add(this.InjectionTab);
            this.TanjiTabs.Controls.Add(this.ToolboxTab);
            this.TanjiTabs.Controls.Add(this.ModulesTab);
            this.TanjiTabs.Controls.Add(this.OptionsTab);
            this.TanjiTabs.DisplayBoundary = true;
            this.TanjiTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TanjiTabs.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.TanjiTabs.ItemSize = new System.Drawing.Size(98, 24);
            this.TanjiTabs.Location = new System.Drawing.Point(0, 0);
            this.TanjiTabs.Name = "TanjiTabs";
            this.TanjiTabs.SelectedIndex = 0;
            this.TanjiTabs.Size = new System.Drawing.Size(494, 378);
            this.TanjiTabs.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.TanjiTabs.TabIndex = 0;
            // 
            // ConnectionTab
            // 
            this.ConnectionTab.Controls.Add(this.ConnectionPg);
            this.ConnectionTab.Location = new System.Drawing.Point(4, 28);
            this.ConnectionTab.Name = "ConnectionTab";
            this.ConnectionTab.Size = new System.Drawing.Size(486, 346);
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
            this.ConnectionPg.Size = new System.Drawing.Size(486, 346);
            this.ConnectionPg.TabIndex = 0;
            // 
            // InjectionTab
            // 
            this.InjectionTab.Controls.Add(this.InjectionTabs);
            this.InjectionTab.Location = new System.Drawing.Point(4, 28);
            this.InjectionTab.Name = "InjectionTab";
            this.InjectionTab.Size = new System.Drawing.Size(486, 346);
            this.InjectionTab.TabIndex = 1;
            this.InjectionTab.Text = "Injection";
            this.InjectionTab.UseVisualStyleBackColor = true;
            // 
            // InjectionTabs
            // 
            this.InjectionTabs.Alignment = System.Windows.Forms.TabAlignment.Right;
            this.InjectionTabs.Controls.Add(this.ConstructerTab);
            this.InjectionTabs.Controls.Add(this.SchedulerTab);
            this.InjectionTabs.Controls.Add(this.InspectorTab);
            this.InjectionTabs.Controls.Add(this.FiltersTab);
            this.InjectionTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InjectionTabs.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.InjectionTabs.ItemSize = new System.Drawing.Size(25, 65);
            this.InjectionTabs.Location = new System.Drawing.Point(0, 0);
            this.InjectionTabs.Multiline = true;
            this.InjectionTabs.Name = "InjectionTabs";
            this.InjectionTabs.SelectedIndex = 0;
            this.InjectionTabs.Size = new System.Drawing.Size(486, 346);
            this.InjectionTabs.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.InjectionTabs.TabIndex = 0;
            // 
            // ConstructerTab
            // 
            this.ConstructerTab.Controls.Add(this.ConstructerPg);
            this.ConstructerTab.Location = new System.Drawing.Point(4, 4);
            this.ConstructerTab.Name = "ConstructerTab";
            this.ConstructerTab.Size = new System.Drawing.Size(413, 338);
            this.ConstructerTab.TabIndex = 0;
            this.ConstructerTab.Text = "Constructer";
            this.ConstructerTab.UseVisualStyleBackColor = true;
            // 
            // ConstructerPg
            // 
            this.ConstructerPg.BackColor = System.Drawing.Color.White;
            this.ConstructerPg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ConstructerPg.Location = new System.Drawing.Point(0, 0);
            this.ConstructerPg.Name = "ConstructerPg";
            this.ConstructerPg.Size = new System.Drawing.Size(413, 338);
            this.ConstructerPg.TabIndex = 0;
            // 
            // SchedulerTab
            // 
            this.SchedulerTab.Controls.Add(this.SchedulerPg);
            this.SchedulerTab.Location = new System.Drawing.Point(4, 4);
            this.SchedulerTab.Name = "SchedulerTab";
            this.SchedulerTab.Size = new System.Drawing.Size(413, 338);
            this.SchedulerTab.TabIndex = 1;
            this.SchedulerTab.Text = "Scheduler";
            this.SchedulerTab.UseVisualStyleBackColor = true;
            // 
            // SchedulerPg
            // 
            this.SchedulerPg.BackColor = System.Drawing.Color.White;
            this.SchedulerPg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SchedulerPg.Location = new System.Drawing.Point(0, 0);
            this.SchedulerPg.Name = "SchedulerPg";
            this.SchedulerPg.Size = new System.Drawing.Size(413, 338);
            this.SchedulerPg.TabIndex = 0;
            // 
            // InspectorTab
            // 
            this.InspectorTab.Controls.Add(this.inspectorPage1);
            this.InspectorTab.Location = new System.Drawing.Point(4, 4);
            this.InspectorTab.Name = "InspectorTab";
            this.InspectorTab.Size = new System.Drawing.Size(413, 338);
            this.InspectorTab.TabIndex = 2;
            this.InspectorTab.Text = "Inspector";
            this.InspectorTab.UseVisualStyleBackColor = true;
            // 
            // inspectorPage1
            // 
            this.inspectorPage1.BackColor = System.Drawing.Color.White;
            this.inspectorPage1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.inspectorPage1.Location = new System.Drawing.Point(0, 0);
            this.inspectorPage1.Name = "inspectorPage1";
            this.inspectorPage1.Size = new System.Drawing.Size(413, 338);
            this.inspectorPage1.TabIndex = 0;
            // 
            // FiltersTab
            // 
            this.FiltersTab.Controls.Add(this.filtersPage1);
            this.FiltersTab.Location = new System.Drawing.Point(4, 4);
            this.FiltersTab.Name = "FiltersTab";
            this.FiltersTab.Size = new System.Drawing.Size(413, 338);
            this.FiltersTab.TabIndex = 3;
            this.FiltersTab.Text = "Filters";
            this.FiltersTab.UseVisualStyleBackColor = true;
            // 
            // filtersPage1
            // 
            this.filtersPage1.BackColor = System.Drawing.Color.White;
            this.filtersPage1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.filtersPage1.Location = new System.Drawing.Point(0, 0);
            this.filtersPage1.Name = "filtersPage1";
            this.filtersPage1.Size = new System.Drawing.Size(413, 338);
            this.filtersPage1.TabIndex = 0;
            // 
            // ToolboxTab
            // 
            this.ToolboxTab.Location = new System.Drawing.Point(4, 28);
            this.ToolboxTab.Name = "ToolboxTab";
            this.ToolboxTab.Size = new System.Drawing.Size(486, 346);
            this.ToolboxTab.TabIndex = 2;
            this.ToolboxTab.Text = "Toolbox";
            this.ToolboxTab.UseVisualStyleBackColor = true;
            // 
            // ModulesTab
            // 
            this.ModulesTab.Controls.Add(this.modulesPage1);
            this.ModulesTab.Location = new System.Drawing.Point(4, 28);
            this.ModulesTab.Name = "ModulesTab";
            this.ModulesTab.Size = new System.Drawing.Size(486, 346);
            this.ModulesTab.TabIndex = 3;
            this.ModulesTab.Text = "Modules";
            this.ModulesTab.UseVisualStyleBackColor = true;
            // 
            // modulesPage1
            // 
            this.modulesPage1.BackColor = System.Drawing.Color.White;
            this.modulesPage1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modulesPage1.Location = new System.Drawing.Point(0, 0);
            this.modulesPage1.Name = "modulesPage1";
            this.modulesPage1.Size = new System.Drawing.Size(486, 346);
            this.modulesPage1.TabIndex = 0;
            // 
            // OptionsTab
            // 
            this.OptionsTab.Controls.Add(this.OptionsTabs);
            this.OptionsTab.Location = new System.Drawing.Point(4, 28);
            this.OptionsTab.Name = "OptionsTab";
            this.OptionsTab.Size = new System.Drawing.Size(486, 346);
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
            this.OptionsTabs.Size = new System.Drawing.Size(486, 346);
            this.OptionsTabs.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.OptionsTabs.TabIndex = 0;
            // 
            // SettingsTab
            // 
            this.SettingsTab.Controls.Add(this.settingsPage1);
            this.SettingsTab.Location = new System.Drawing.Point(4, 4);
            this.SettingsTab.Name = "SettingsTab";
            this.SettingsTab.Size = new System.Drawing.Size(431, 338);
            this.SettingsTab.TabIndex = 0;
            this.SettingsTab.Text = "Settings";
            this.SettingsTab.UseVisualStyleBackColor = true;
            // 
            // settingsPage1
            // 
            this.settingsPage1.BackColor = System.Drawing.Color.White;
            this.settingsPage1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.settingsPage1.Location = new System.Drawing.Point(0, 0);
            this.settingsPage1.Name = "settingsPage1";
            this.settingsPage1.Size = new System.Drawing.Size(431, 338);
            this.settingsPage1.TabIndex = 0;
            // 
            // AboutTab
            // 
            this.AboutTab.Controls.Add(this.aboutPage1);
            this.AboutTab.Location = new System.Drawing.Point(4, 4);
            this.AboutTab.Name = "AboutTab";
            this.AboutTab.Size = new System.Drawing.Size(431, 338);
            this.AboutTab.TabIndex = 1;
            this.AboutTab.Text = "About";
            this.AboutTab.UseVisualStyleBackColor = true;
            // 
            // aboutPage1
            // 
            this.aboutPage1.BackColor = System.Drawing.Color.White;
            this.aboutPage1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.aboutPage1.Location = new System.Drawing.Point(0, 0);
            this.aboutPage1.Name = "aboutPage1";
            this.aboutPage1.Size = new System.Drawing.Size(431, 338);
            this.aboutPage1.TabIndex = 0;
            // 
            // TanjiStatusStrip
            // 
            this.TanjiStatusStrip.BackColor = System.Drawing.Color.White;
            this.TanjiStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel3,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel4,
            this.toolStripStatusLabel1});
            this.TanjiStatusStrip.Location = new System.Drawing.Point(0, 378);
            this.TanjiStatusStrip.Name = "TanjiStatusStrip";
            this.TanjiStatusStrip.Size = new System.Drawing.Size(494, 24);
            this.TanjiStatusStrip.SizingGrip = false;
            this.TanjiStatusStrip.TabIndex = 1;
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.toolStripStatusLabel3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(50, 19);
            this.toolStripStatusLabel3.Text = "v1.5.0.0";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.toolStripStatusLabel2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(87, 19);
            this.toolStripStatusLabel2.Text = "Schedules: 0/0";
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.toolStripStatusLabel4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(65, 19);
            this.toolStripStatusLabel4.Text = "Filters: 0/0";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.toolStripStatusLabel1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(80, 19);
            this.toolStripStatusLabel1.Text = "Modules: 0/0";
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 402);
            this.Controls.Add(this.TanjiTabs);
            this.Controls.Add(this.TanjiStatusStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainFrm";
            this.Text = "Tanji - Disconnected";
            this.TopMost = true;
            this.TanjiTabs.ResumeLayout(false);
            this.ConnectionTab.ResumeLayout(false);
            this.InjectionTab.ResumeLayout(false);
            this.InjectionTabs.ResumeLayout(false);
            this.ConstructerTab.ResumeLayout(false);
            this.SchedulerTab.ResumeLayout(false);
            this.InspectorTab.ResumeLayout(false);
            this.FiltersTab.ResumeLayout(false);
            this.ModulesTab.ResumeLayout(false);
            this.OptionsTab.ResumeLayout(false);
            this.OptionsTabs.ResumeLayout(false);
            this.SettingsTab.ResumeLayout(false);
            this.AboutTab.ResumeLayout(false);
            this.TanjiStatusStrip.ResumeLayout(false);
            this.TanjiStatusStrip.PerformLayout();
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
        private Tangine.Controls.TangineTabControl InjectionTabs;
        private System.Windows.Forms.TabPage ConstructerTab;
        private System.Windows.Forms.TabPage SchedulerTab;
        private System.Windows.Forms.TabPage InspectorTab;
        private System.Windows.Forms.TabPage FiltersTab;
        private Services.Injection.ConstructerPage ConstructerPg;
        private Services.Injection.SchedulerPage SchedulerPg;
        private Services.Injection.InspectorPage inspectorPage1;
        private Services.Injection.FiltersPage filtersPage1;
        private Services.ConnectionPage ConnectionPg;
        private Services.ModulesPage modulesPage1;
        private Tangine.Controls.TangineTabControl OptionsTabs;
        private System.Windows.Forms.TabPage SettingsTab;
        private Services.Options.SettingsPage settingsPage1;
        private System.Windows.Forms.TabPage AboutTab;
        private Services.Options.AboutPage aboutPage1;
        private System.Windows.Forms.StatusStrip TanjiStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}