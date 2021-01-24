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
            this.SendToClientBtn = new Tangine.Controls.TangineButton();
            this.SendToServerBtn = new Tangine.Controls.TangineButton();
            this.InjectionTabs = new Tangine.Controls.TangineTabControl();
            this.ConstructerTab = new System.Windows.Forms.TabPage();
            this.ConstructerPg = new Tanji.Services.Injection.ConstructerPage();
            this.SchedulerTab = new System.Windows.Forms.TabPage();
            this.SchedulerPg = new Tanji.Services.Injection.SchedulerPage();
            this.InspectorTab = new System.Windows.Forms.TabPage();
            this.InspectorPg = new Tanji.Services.Injection.InspectorPage();
            this.FiltersTab = new System.Windows.Forms.TabPage();
            this.FiltersPg = new Tanji.Services.Injection.FiltersPage();
            this.PacketTxt = new System.Windows.Forms.ComboBox();
            this.ToolboxTab = new System.Windows.Forms.TabPage();
            this.ToolboxPg = new Tanji.Services.Toolbox.ToolboxPage();
            this.ModulesTab = new System.Windows.Forms.TabPage();
            this.ModulesPg = new Tanji.Services.Modules.ModulesPage();
            this.OptionsTab = new System.Windows.Forms.TabPage();
            this.OptionsTabs = new Tangine.Controls.TangineTabControl();
            this.SettingsTab = new System.Windows.Forms.TabPage();
            this.SettingsPg = new Tanji.Services.Options.SettingsPage();
            this.AboutTab = new System.Windows.Forms.TabPage();
            this.AboutPg = new Tanji.Services.Options.AboutPage();
            this.TanjiStatusStrip.SuspendLayout();
            this.TanjiTabs.SuspendLayout();
            this.ConnectionTab.SuspendLayout();
            this.InjectionTab.SuspendLayout();
            this.InjectionTabs.SuspendLayout();
            this.ConstructerTab.SuspendLayout();
            this.SchedulerTab.SuspendLayout();
            this.InspectorTab.SuspendLayout();
            this.FiltersTab.SuspendLayout();
            this.ToolboxTab.SuspendLayout();
            this.ModulesTab.SuspendLayout();
            this.OptionsTab.SuspendLayout();
            this.OptionsTabs.SuspendLayout();
            this.SettingsTab.SuspendLayout();
            this.AboutTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // TanjiStatusStrip
            // 
            this.TanjiStatusStrip.AllowMerge = false;
            this.TanjiStatusStrip.BackColor = System.Drawing.Color.White;
            this.TanjiStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.VersionLbl,
            this.SchedulesLbl,
            this.FiltersLbl,
            this.ModulesLbl,
            this.GitHubLinkLbl});
            this.TanjiStatusStrip.Location = new System.Drawing.Point(0, 345);
            this.TanjiStatusStrip.Name = "TanjiStatusStrip";
            this.TanjiStatusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 12, 0);
            this.TanjiStatusStrip.Size = new System.Drawing.Size(484, 24);
            this.TanjiStatusStrip.SizingGrip = false;
            this.TanjiStatusStrip.TabIndex = 1;
            // 
            // VersionLbl
            // 
            this.VersionLbl.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.VersionLbl.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.VersionLbl.IsLink = true;
            this.VersionLbl.LinkColor = System.Drawing.SystemColors.HotTrack;
            this.VersionLbl.Name = "VersionLbl";
            this.VersionLbl.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.VersionLbl.Size = new System.Drawing.Size(51, 19);
            this.VersionLbl.Text = "v1.5.0";
            // 
            // SchedulesLbl
            // 
            this.SchedulesLbl.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.SchedulesLbl.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.SchedulesLbl.Name = "SchedulesLbl";
            this.SchedulesLbl.Size = new System.Drawing.Size(87, 19);
            this.SchedulesLbl.Text = "Schedules: 0/0";
            // 
            // FiltersLbl
            // 
            this.FiltersLbl.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.FiltersLbl.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.FiltersLbl.Name = "FiltersLbl";
            this.FiltersLbl.Size = new System.Drawing.Size(65, 19);
            this.FiltersLbl.Text = "Filters: 0/0";
            // 
            // ModulesLbl
            // 
            this.ModulesLbl.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.ModulesLbl.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ModulesLbl.Name = "ModulesLbl";
            this.ModulesLbl.Size = new System.Drawing.Size(80, 19);
            this.ModulesLbl.Text = "Modules: 0/0";
            // 
            // GitHubLinkLbl
            // 
            this.GitHubLinkLbl.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.GitHubLinkLbl.IsLink = true;
            this.GitHubLinkLbl.LinkColor = System.Drawing.SystemColors.HotTrack;
            this.GitHubLinkLbl.Name = "GitHubLinkLbl";
            this.GitHubLinkLbl.Size = new System.Drawing.Size(188, 19);
            this.GitHubLinkLbl.Spring = true;
            this.GitHubLinkLbl.Text = "GitHub - Elbrah/Tanji";
            // 
            // TanjiTabs
            // 
            this.TanjiTabs.AllowDrop = true;
            this.TanjiTabs.Controls.Add(this.ConnectionTab);
            this.TanjiTabs.Controls.Add(this.InjectionTab);
            this.TanjiTabs.Controls.Add(this.ToolboxTab);
            this.TanjiTabs.Controls.Add(this.ModulesTab);
            this.TanjiTabs.Controls.Add(this.OptionsTab);
            this.TanjiTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TanjiTabs.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.TanjiTabs.IsDisplayingBorder = true;
            this.TanjiTabs.ItemSize = new System.Drawing.Size(96, 24);
            this.TanjiTabs.Location = new System.Drawing.Point(0, 0);
            this.TanjiTabs.Name = "TanjiTabs";
            this.TanjiTabs.SelectedIndex = 0;
            this.TanjiTabs.Size = new System.Drawing.Size(484, 345);
            this.TanjiTabs.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.TanjiTabs.TabIndex = 0;
            // 
            // ConnectionTab
            // 
            this.ConnectionTab.Controls.Add(this.ConnectionPg);
            this.ConnectionTab.Location = new System.Drawing.Point(4, 28);
            this.ConnectionTab.Name = "ConnectionTab";
            this.ConnectionTab.Padding = new System.Windows.Forms.Padding(3);
            this.ConnectionTab.Size = new System.Drawing.Size(476, 313);
            this.ConnectionTab.TabIndex = 0;
            this.ConnectionTab.Text = "Connection";
            this.ConnectionTab.UseVisualStyleBackColor = true;
            // 
            // ConnectionPg
            // 
            this.ConnectionPg.BackColor = System.Drawing.Color.Transparent;
            this.ConnectionPg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ConnectionPg.IsReceiving = false;
            this.ConnectionPg.Location = new System.Drawing.Point(0, 0);
            this.ConnectionPg.Margin = new System.Windows.Forms.Padding(0);
            this.ConnectionPg.Name = "ConnectionPg";
            this.ConnectionPg.Size = new System.Drawing.Size(476, 313);
            this.ConnectionPg.TabIndex = 0;
            this.ConnectionPg.TabStop = false;
            // 
            // InjectionTab
            // 
            this.InjectionTab.Controls.Add(this.SendToClientBtn);
            this.InjectionTab.Controls.Add(this.SendToServerBtn);
            this.InjectionTab.Controls.Add(this.InjectionTabs);
            this.InjectionTab.Controls.Add(this.PacketTxt);
            this.InjectionTab.Location = new System.Drawing.Point(4, 28);
            this.InjectionTab.Name = "InjectionTab";
            this.InjectionTab.Padding = new System.Windows.Forms.Padding(3);
            this.InjectionTab.Size = new System.Drawing.Size(476, 313);
            this.InjectionTab.TabIndex = 1;
            this.InjectionTab.Text = "Injection";
            this.InjectionTab.UseVisualStyleBackColor = true;
            // 
            // SendToClientBtn
            // 
            this.SendToClientBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SendToClientBtn.Location = new System.Drawing.Point(262, 287);
            this.SendToClientBtn.Name = "SendToClientBtn";
            this.SendToClientBtn.Size = new System.Drawing.Size(101, 20);
            this.SendToClientBtn.TabIndex = 6;
            this.SendToClientBtn.Text = "Send To Client";
            // 
            // SendToServerBtn
            // 
            this.SendToServerBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SendToServerBtn.Location = new System.Drawing.Point(369, 287);
            this.SendToServerBtn.Name = "SendToServerBtn";
            this.SendToServerBtn.Size = new System.Drawing.Size(101, 20);
            this.SendToServerBtn.TabIndex = 7;
            this.SendToServerBtn.Text = "Send To Server";
            // 
            // InjectionTabs
            // 
            this.InjectionTabs.Alignment = System.Windows.Forms.TabAlignment.Right;
            this.InjectionTabs.Controls.Add(this.ConstructerTab);
            this.InjectionTabs.Controls.Add(this.SchedulerTab);
            this.InjectionTabs.Controls.Add(this.InspectorTab);
            this.InjectionTabs.Controls.Add(this.FiltersTab);
            this.InjectionTabs.Dock = System.Windows.Forms.DockStyle.Top;
            this.InjectionTabs.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.InjectionTabs.IsDisplayingBorder = true;
            this.InjectionTabs.ItemSize = new System.Drawing.Size(24, 65);
            this.InjectionTabs.Location = new System.Drawing.Point(3, 3);
            this.InjectionTabs.Multiline = true;
            this.InjectionTabs.Name = "InjectionTabs";
            this.InjectionTabs.SelectedIndex = 0;
            this.InjectionTabs.Size = new System.Drawing.Size(470, 278);
            this.InjectionTabs.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.InjectionTabs.TabIndex = 4;
            // 
            // ConstructerTab
            // 
            this.ConstructerTab.Controls.Add(this.ConstructerPg);
            this.ConstructerTab.Location = new System.Drawing.Point(4, 4);
            this.ConstructerTab.Name = "ConstructerTab";
            this.ConstructerTab.Padding = new System.Windows.Forms.Padding(3);
            this.ConstructerTab.Size = new System.Drawing.Size(397, 270);
            this.ConstructerTab.TabIndex = 0;
            this.ConstructerTab.Text = "Constructer";
            this.ConstructerTab.UseVisualStyleBackColor = true;
            // 
            // ConstructerPg
            // 
            this.ConstructerPg.Amount = ((ushort)(1));
            this.ConstructerPg.BackColor = System.Drawing.Color.Transparent;
            this.ConstructerPg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ConstructerPg.Id = ((ushort)(1));
            this.ConstructerPg.Location = new System.Drawing.Point(0, 0);
            this.ConstructerPg.Margin = new System.Windows.Forms.Padding(0);
            this.ConstructerPg.Name = "ConstructerPg";
            this.ConstructerPg.Size = new System.Drawing.Size(397, 270);
            this.ConstructerPg.TabIndex = 0;
            this.ConstructerPg.TabStop = false;
            this.ConstructerPg.Value = "";
            // 
            // SchedulerTab
            // 
            this.SchedulerTab.Controls.Add(this.SchedulerPg);
            this.SchedulerTab.Location = new System.Drawing.Point(4, 4);
            this.SchedulerTab.Name = "SchedulerTab";
            this.SchedulerTab.Padding = new System.Windows.Forms.Padding(3);
            this.SchedulerTab.Size = new System.Drawing.Size(397, 270);
            this.SchedulerTab.TabIndex = 1;
            this.SchedulerTab.Text = "Scheduler";
            this.SchedulerTab.UseVisualStyleBackColor = true;
            // 
            // SchedulerPg
            // 
            this.SchedulerPg.BackColor = System.Drawing.Color.Transparent;
            this.SchedulerPg.Cycles = 0;
            this.SchedulerPg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.SchedulerPg.HotkeysText = "";
            this.SchedulerPg.Interval = 250;
            this.SchedulerPg.Location = new System.Drawing.Point(0, 0);
            this.SchedulerPg.Margin = new System.Windows.Forms.Padding(0);
            this.SchedulerPg.Name = "SchedulerPg";
            this.SchedulerPg.PacketText = "";
            this.SchedulerPg.Size = new System.Drawing.Size(397, 270);
            this.SchedulerPg.TabIndex = 1;
            this.SchedulerPg.TabStop = false;
            this.SchedulerPg.ToServer = true;
            // 
            // InspectorTab
            // 
            this.InspectorTab.Controls.Add(this.InspectorPg);
            this.InspectorTab.Location = new System.Drawing.Point(4, 4);
            this.InspectorTab.Name = "InspectorTab";
            this.InspectorTab.Padding = new System.Windows.Forms.Padding(3);
            this.InspectorTab.Size = new System.Drawing.Size(397, 270);
            this.InspectorTab.TabIndex = 2;
            this.InspectorTab.Text = "Inspector";
            this.InspectorTab.UseVisualStyleBackColor = true;
            // 
            // InspectorPg
            // 
            this.InspectorPg.BackColor = System.Drawing.Color.Transparent;
            this.InspectorPg.Location = new System.Drawing.Point(0, 0);
            this.InspectorPg.Margin = new System.Windows.Forms.Padding(0);
            this.InspectorPg.Name = "InspectorPg";
            this.InspectorPg.Size = new System.Drawing.Size(397, 270);
            this.InspectorPg.TabIndex = 0;
            this.InspectorPg.TabStop = false;
            // 
            // FiltersTab
            // 
            this.FiltersTab.Controls.Add(this.FiltersPg);
            this.FiltersTab.Location = new System.Drawing.Point(4, 4);
            this.FiltersTab.Name = "FiltersTab";
            this.FiltersTab.Padding = new System.Windows.Forms.Padding(3);
            this.FiltersTab.Size = new System.Drawing.Size(397, 270);
            this.FiltersTab.TabIndex = 3;
            this.FiltersTab.Text = "Filters";
            this.FiltersTab.UseVisualStyleBackColor = true;
            // 
            // FiltersPg
            // 
            this.FiltersPg.BackColor = System.Drawing.Color.Transparent;
            this.FiltersPg.Location = new System.Drawing.Point(0, 0);
            this.FiltersPg.Margin = new System.Windows.Forms.Padding(0);
            this.FiltersPg.Name = "FiltersPg";
            this.FiltersPg.Size = new System.Drawing.Size(397, 270);
            this.FiltersPg.TabIndex = 0;
            this.FiltersPg.TabStop = false;
            // 
            // PacketTxt
            // 
            this.PacketTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PacketTxt.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.PacketTxt.DropDownWidth = 250;
            this.PacketTxt.FormattingEnabled = true;
            this.PacketTxt.ItemHeight = 14;
            this.PacketTxt.Location = new System.Drawing.Point(6, 287);
            this.PacketTxt.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.PacketTxt.Name = "PacketTxt";
            this.PacketTxt.Size = new System.Drawing.Size(250, 20);
            this.PacketTxt.TabIndex = 1;
            this.PacketTxt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PacketTxt_KeyDown);
            // 
            // ToolboxTab
            // 
            this.ToolboxTab.Controls.Add(this.ToolboxPg);
            this.ToolboxTab.Location = new System.Drawing.Point(4, 28);
            this.ToolboxTab.Name = "ToolboxTab";
            this.ToolboxTab.Padding = new System.Windows.Forms.Padding(3);
            this.ToolboxTab.Size = new System.Drawing.Size(476, 313);
            this.ToolboxTab.TabIndex = 2;
            this.ToolboxTab.Text = "Toolbox";
            this.ToolboxTab.UseVisualStyleBackColor = true;
            // 
            // ToolboxPg
            // 
            this.ToolboxPg.BackColor = System.Drawing.Color.Transparent;
            this.ToolboxPg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ToolboxPg.Location = new System.Drawing.Point(0, 0);
            this.ToolboxPg.Margin = new System.Windows.Forms.Padding(0);
            this.ToolboxPg.Name = "ToolboxPg";
            this.ToolboxPg.Size = new System.Drawing.Size(476, 313);
            this.ToolboxPg.TabIndex = 0;
            this.ToolboxPg.TabStop = false;
            // 
            // ModulesTab
            // 
            this.ModulesTab.AllowDrop = true;
            this.ModulesTab.Controls.Add(this.ModulesPg);
            this.ModulesTab.Location = new System.Drawing.Point(4, 28);
            this.ModulesTab.Name = "ModulesTab";
            this.ModulesTab.Padding = new System.Windows.Forms.Padding(3);
            this.ModulesTab.Size = new System.Drawing.Size(476, 313);
            this.ModulesTab.TabIndex = 3;
            this.ModulesTab.Text = "Modules";
            this.ModulesTab.UseVisualStyleBackColor = true;
            // 
            // ModulesPg
            // 
            this.ModulesPg.BackColor = System.Drawing.Color.Transparent;
            this.ModulesPg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ModulesPg.Location = new System.Drawing.Point(0, 0);
            this.ModulesPg.Margin = new System.Windows.Forms.Padding(0);
            this.ModulesPg.Name = "ModulesPg";
            this.ModulesPg.SelectedModule = null;
            this.ModulesPg.Size = new System.Drawing.Size(476, 313);
            this.ModulesPg.TabIndex = 0;
            this.ModulesPg.TabStop = false;
            // 
            // OptionsTab
            // 
            this.OptionsTab.Controls.Add(this.OptionsTabs);
            this.OptionsTab.Location = new System.Drawing.Point(4, 28);
            this.OptionsTab.Name = "OptionsTab";
            this.OptionsTab.Padding = new System.Windows.Forms.Padding(3);
            this.OptionsTab.Size = new System.Drawing.Size(476, 313);
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
            this.OptionsTabs.ItemSize = new System.Drawing.Size(25, 60);
            this.OptionsTabs.Location = new System.Drawing.Point(3, 3);
            this.OptionsTabs.Multiline = true;
            this.OptionsTabs.Name = "OptionsTabs";
            this.OptionsTabs.SelectedIndex = 0;
            this.OptionsTabs.Size = new System.Drawing.Size(470, 307);
            this.OptionsTabs.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.OptionsTabs.TabIndex = 0;
            // 
            // SettingsTab
            // 
            this.SettingsTab.Controls.Add(this.SettingsPg);
            this.SettingsTab.Location = new System.Drawing.Point(4, 4);
            this.SettingsTab.Name = "SettingsTab";
            this.SettingsTab.Padding = new System.Windows.Forms.Padding(3);
            this.SettingsTab.Size = new System.Drawing.Size(402, 299);
            this.SettingsTab.TabIndex = 0;
            this.SettingsTab.Text = "Settings";
            this.SettingsTab.UseVisualStyleBackColor = true;
            // 
            // SettingsPg
            // 
            this.SettingsPg.BackColor = System.Drawing.Color.Transparent;
            this.SettingsPg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.SettingsPg.Location = new System.Drawing.Point(0, 0);
            this.SettingsPg.Margin = new System.Windows.Forms.Padding(0);
            this.SettingsPg.Name = "SettingsPg";
            this.SettingsPg.Size = new System.Drawing.Size(402, 299);
            this.SettingsPg.TabIndex = 0;
            this.SettingsPg.TabStop = false;
            // 
            // AboutTab
            // 
            this.AboutTab.Controls.Add(this.AboutPg);
            this.AboutTab.Location = new System.Drawing.Point(4, 4);
            this.AboutTab.Name = "AboutTab";
            this.AboutTab.Padding = new System.Windows.Forms.Padding(3);
            this.AboutTab.Size = new System.Drawing.Size(402, 299);
            this.AboutTab.TabIndex = 1;
            this.AboutTab.Text = "About";
            this.AboutTab.UseVisualStyleBackColor = true;
            // 
            // AboutPg
            // 
            this.AboutPg.BackColor = System.Drawing.Color.Transparent;
            this.AboutPg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.AboutPg.Location = new System.Drawing.Point(0, 0);
            this.AboutPg.Margin = new System.Windows.Forms.Padding(0);
            this.AboutPg.Name = "AboutPg";
            this.AboutPg.Size = new System.Drawing.Size(402, 299);
            this.AboutPg.TabIndex = 0;
            this.AboutPg.TabStop = false;
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(484, 369);
            this.Controls.Add(this.TanjiTabs);
            this.Controls.Add(this.TanjiStatusStrip);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "MainFrm";
            this.Text = "Tanji ~ Disconnected";
            this.TopMost = true;
            this.TanjiStatusStrip.ResumeLayout(false);
            this.TanjiStatusStrip.PerformLayout();
            this.TanjiTabs.ResumeLayout(false);
            this.ConnectionTab.ResumeLayout(false);
            this.InjectionTab.ResumeLayout(false);
            this.InjectionTabs.ResumeLayout(false);
            this.ConstructerTab.ResumeLayout(false);
            this.SchedulerTab.ResumeLayout(false);
            this.InspectorTab.ResumeLayout(false);
            this.FiltersTab.ResumeLayout(false);
            this.ToolboxTab.ResumeLayout(false);
            this.ModulesTab.ResumeLayout(false);
            this.OptionsTab.ResumeLayout(false);
            this.OptionsTabs.ResumeLayout(false);
            this.SettingsTab.ResumeLayout(false);
            this.AboutTab.ResumeLayout(false);
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
        private Tangine.Controls.TangineTabControl InjectionTabs;
        private System.Windows.Forms.TabPage ConstructerTab;
        private System.Windows.Forms.TabPage SchedulerTab;
        private System.Windows.Forms.TabPage InspectorTab;
        private System.Windows.Forms.TabPage FiltersTab;
        private Services.Injection.ConstructerPage ConstructerPg;
        private Services.Injection.SchedulerPage SchedulerPg;
        private Services.Injection.InspectorPage InspectorPg;
        private Services.Injection.FiltersPage FiltersPg;
        private Services.ConnectionPage ConnectionPg;
        private Services.Toolbox.ToolboxPage ToolboxPg;
        private Tangine.Controls.TangineButton SendToClientBtn;
        private Tangine.Controls.TangineButton SendToServerBtn;
    }
}