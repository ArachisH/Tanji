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
            this.components = new System.ComponentModel.Container();
            this.CustomClientDlg = new System.Windows.Forms.OpenFileDialog();
            this.InstallModuleDlg = new System.Windows.Forms.OpenFileDialog();
            this.TanjiTabs = new Sulakore.Components.SKoreTabControl();
            this.ConnectionTab = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.ProxyPortLbl = new Sulakore.Components.SKoreLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.CoTVariableTxt = new Sulakore.Components.SKoreLabelBox();
            this.CoTValueTxt = new Sulakore.Components.SKoreLabelBox();
            this.CoTCustomClientTxt = new Sulakore.Components.SKoreLabelBox();
            this.CoTBrowseBtn = new Sulakore.Components.SKoreButton();
            this.CoTExportCertificateAuthorityBtn = new Sulakore.Components.SKoreButton();
            this.CoTDestroyCertificatesBtn = new Sulakore.Components.SKoreButton();
            this.CoTResetBtn = new Sulakore.Components.SKoreButton();
            this.CoTUpdateBtn = new Sulakore.Components.SKoreButton();
            this.CoTStatusTxt = new Sulakore.Components.SKoreLabel();
            this.CoTConnectBtn = new Sulakore.Components.SKoreButton();
            this.CoTVariablesVw = new Sulakore.Components.SKoreListView();
            this.CoTVariableCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CoTValueCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.InjectionTab = new System.Windows.Forms.TabPage();
            this.ITSendToClientBtn = new Sulakore.Components.SKoreButton();
            this.ITSendToServerBtn = new Sulakore.Components.SKoreButton();
            this.InjectionTabs = new Sulakore.Components.SKoreTabControl();
            this.ConstructerTab = new System.Windows.Forms.TabPage();
            this.CTValueTxt = new System.Windows.Forms.ComboBox();
            this.CTHeaderTxt = new System.Windows.Forms.NumericUpDown();
            this.CTHeaderLbl = new System.Windows.Forms.Label();
            this.CTAmountLbl = new System.Windows.Forms.Label();
            this.CTStructureTxt = new System.Windows.Forms.TextBox();
            this.CTTransferBelowBtn = new Sulakore.Components.SKoreButton();
            this.CTValueCountLbl = new Sulakore.Components.SKoreLabel();
            this.CTAmountTxt = new System.Windows.Forms.NumericUpDown();
            this.CTRemoveBtn = new Sulakore.Components.SKoreButton();
            this.CTMoveDownBtn = new Sulakore.Components.SKoreButton();
            this.CTMoveUpBtn = new Sulakore.Components.SKoreButton();
            this.CTClearBtn = new Sulakore.Components.SKoreButton();
            this.CTWriteBooleanBtn = new Sulakore.Components.SKoreButton();
            this.CTWriteStringBtn = new Sulakore.Components.SKoreButton();
            this.CTWriteIntegerBtn = new Sulakore.Components.SKoreButton();
            this.CTValueLbl = new System.Windows.Forms.Label();
            this.CTConstructerVw = new Sulakore.Components.SKoreConstructView();
            this.CTTypeCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CTValueCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CTEncodedCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SchedulerTab = new System.Windows.Forms.TabPage();
            this.STAutoStartChckbx = new System.Windows.Forms.CheckBox();
            this.STDestinationTxt = new System.Windows.Forms.ComboBox();
            this.STPacketTxt = new System.Windows.Forms.TextBox();
            this.STHotkeyTxt = new Sulakore.Components.SKoreLabelBox();
            this.STRemoveBtn = new Sulakore.Components.SKoreButton();
            this.STDestinationLbl = new System.Windows.Forms.Label();
            this.STCyclesLbl = new System.Windows.Forms.Label();
            this.STCyclesTxt = new System.Windows.Forms.NumericUpDown();
            this.STIntervalLbl = new System.Windows.Forms.Label();
            this.STClearBtn = new Sulakore.Components.SKoreButton();
            this.STCreateBtn = new Sulakore.Components.SKoreButton();
            this.STIntervalTxt = new System.Windows.Forms.NumericUpDown();
            this.STPacketLbl = new System.Windows.Forms.Label();
            this.STSchedulerVw = new Sulakore.Components.SKoreScheduleView();
            this.STPacketCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.STDestinationCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.STIntervalCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.STCyclesCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.STHotkeyCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.PrimitiveTab = new System.Windows.Forms.TabPage();
            this.PTSaveAsBtn = new Sulakore.Components.SKoreButton();
            this.PTLengthTxt = new Sulakore.Components.SKoreLabelBox();
            this.PTHeaderTxt = new Sulakore.Components.SKoreLabelBox();
            this.PTCorruptedTxt = new Sulakore.Components.SKoreLabelBox();
            this.PTPacketTxt = new System.Windows.Forms.TextBox();
            this.FiltersTab = new System.Windows.Forms.TabPage();
            this.FTReplacementTxt = new Sulakore.Components.SKoreLabelBox();
            this.FTDestinationLbl = new System.Windows.Forms.Label();
            this.FTDestinationTxt = new System.Windows.Forms.ComboBox();
            this.FTHeaderTxt = new System.Windows.Forms.NumericUpDown();
            this.FTHeaderLbl = new System.Windows.Forms.Label();
            this.FTRemoveBtn = new Sulakore.Components.SKoreButton();
            this.FTCreateBtn = new Sulakore.Components.SKoreButton();
            this.FTActionLbl = new System.Windows.Forms.Label();
            this.FTActionTxt = new System.Windows.Forms.ComboBox();
            this.FTFiltersVw = new Sulakore.Components.SKoreListView();
            this.FTHeaderCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.FTDestinationCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.FTActionCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.FTReplacementCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ITPacketTxt = new System.Windows.Forms.ComboBox();
            this.ToolboxTab = new System.Windows.Forms.TabPage();
            this.TT16BitInputLbl = new System.Windows.Forms.Label();
            this.TT32BitInputLbl = new System.Windows.Forms.Label();
            this.TTIntInputTxt = new System.Windows.Forms.NumericUpDown();
            this.TTIntOutputTxt = new System.Windows.Forms.TextBox();
            this.TTUShortOutputTxt = new System.Windows.Forms.TextBox();
            this.TTDecodeIntBtn = new Sulakore.Components.SKoreButton();
            this.TTDecodeUShortBtn = new Sulakore.Components.SKoreButton();
            this.TTUShortInputTxt = new System.Windows.Forms.NumericUpDown();
            this.ModulesTab = new System.Windows.Forms.TabPage();
            this.sKoreLabelBox1 = new Sulakore.Components.SKoreLabelBox();
            this.MTResourceBtn = new Sulakore.Components.SKoreButton();
            this.MTHabboNameTxt = new System.Windows.Forms.TextBox();
            this.MTHabboNameLbl = new System.Windows.Forms.Label();
            this.MTAuthorsLbl = new System.Windows.Forms.Label();
            this.MTAuthorsTxt = new System.Windows.Forms.ComboBox();
            this.MTUninstallModuleBtn = new Sulakore.Components.SKoreButton();
            this.MTInstallModuleBtn = new Sulakore.Components.SKoreButton();
            this.MTAuthorPctbx = new System.Windows.Forms.PictureBox();
            this.MTModulesVw = new Sulakore.Components.SKoreListView();
            this.MTNameCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.MTDescriptionCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.MTVersionCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.MTStateCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.AboutTab = new System.Windows.Forms.TabPage();
            this.DonateBtn = new Sulakore.Components.SKoreButton();
            this.SpeaqerBtn = new Sulakore.Components.SKoreButton();
            this.SNGButton = new Sulakore.Components.SKoreButton();
            this.DarkboxBtn = new Sulakore.Components.SKoreButton();
            this.DarkStarBtn = new Sulakore.Components.SKoreButton();
            this.ArachisBtn = new Sulakore.Components.SKoreButton();
            this.InjectionMenu = new Sulakore.Components.SKoreInjectionMenu(this.components);
            this.SavePacketDlg = new System.Windows.Forms.SaveFileDialog();
            this.TanjiVersionTxt = new System.Windows.Forms.ToolStripStatusLabel();
            this.SchedulesTxt = new System.Windows.Forms.ToolStripStatusLabel();
            this.FiltersTxt = new System.Windows.Forms.ToolStripStatusLabel();
            this.ModulesTxt = new System.Windows.Forms.ToolStripStatusLabel();
            this.TanjiInfoTxt = new System.Windows.Forms.ToolStripStatusLabel();
            this.TanjiStrip = new System.Windows.Forms.StatusStrip();
            this.TanjiTabs.SuspendLayout();
            this.ConnectionTab.SuspendLayout();
            this.InjectionTab.SuspendLayout();
            this.InjectionTabs.SuspendLayout();
            this.ConstructerTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CTHeaderTxt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CTAmountTxt)).BeginInit();
            this.SchedulerTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.STCyclesTxt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.STIntervalTxt)).BeginInit();
            this.PrimitiveTab.SuspendLayout();
            this.FiltersTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FTHeaderTxt)).BeginInit();
            this.ToolboxTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TTIntInputTxt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TTUShortInputTxt)).BeginInit();
            this.ModulesTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MTAuthorPctbx)).BeginInit();
            this.AboutTab.SuspendLayout();
            this.TanjiStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // CustomClientDlg
            // 
            this.CustomClientDlg.DefaultExt = "swf";
            this.CustomClientDlg.Filter = "Shockwave Flash File (*.swf)|*.swf";
            this.CustomClientDlg.Title = "Tanji ~ Custom Client";
            // 
            // InstallModuleDlg
            // 
            this.InstallModuleDlg.DefaultExt = "dll";
            this.InstallModuleDlg.Filter = ".NET Assembly (*.dll, *.exe)|*.dll; *.exe|Dynamic Link Library (*.dll)|*.dll|Exec" +
    "utable (*.exe)|*.exe";
            this.InstallModuleDlg.Title = "Tanji ~ Install Module";
            // 
            // TanjiTabs
            // 
            this.TanjiTabs.AllowDrop = true;
            this.TanjiTabs.Controls.Add(this.ConnectionTab);
            this.TanjiTabs.Controls.Add(this.InjectionTab);
            this.TanjiTabs.Controls.Add(this.ToolboxTab);
            this.TanjiTabs.Controls.Add(this.ModulesTab);
            this.TanjiTabs.Controls.Add(this.AboutTab);
            this.TanjiTabs.DisplayBoundary = true;
            this.TanjiTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TanjiTabs.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.TanjiTabs.IsDisplayingBorder = true;
            this.TanjiTabs.ItemSize = new System.Drawing.Size(96, 24);
            this.TanjiTabs.Location = new System.Drawing.Point(0, 0);
            this.TanjiTabs.Name = "TanjiTabs";
            this.TanjiTabs.SelectedIndex = 0;
            this.TanjiTabs.Size = new System.Drawing.Size(484, 345);
            this.TanjiTabs.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.TanjiTabs.TabIndex = 6;
            // 
            // ConnectionTab
            // 
            this.ConnectionTab.Controls.Add(this.label2);
            this.ConnectionTab.Controls.Add(this.ProxyPortLbl);
            this.ConnectionTab.Controls.Add(this.label1);
            this.ConnectionTab.Controls.Add(this.CoTVariableTxt);
            this.ConnectionTab.Controls.Add(this.CoTValueTxt);
            this.ConnectionTab.Controls.Add(this.CoTCustomClientTxt);
            this.ConnectionTab.Controls.Add(this.CoTBrowseBtn);
            this.ConnectionTab.Controls.Add(this.CoTExportCertificateAuthorityBtn);
            this.ConnectionTab.Controls.Add(this.CoTDestroyCertificatesBtn);
            this.ConnectionTab.Controls.Add(this.CoTResetBtn);
            this.ConnectionTab.Controls.Add(this.CoTUpdateBtn);
            this.ConnectionTab.Controls.Add(this.CoTStatusTxt);
            this.ConnectionTab.Controls.Add(this.CoTConnectBtn);
            this.ConnectionTab.Controls.Add(this.CoTVariablesVw);
            this.ConnectionTab.Location = new System.Drawing.Point(4, 28);
            this.ConnectionTab.Name = "ConnectionTab";
            this.ConnectionTab.Padding = new System.Windows.Forms.Padding(3);
            this.ConnectionTab.Size = new System.Drawing.Size(476, 313);
            this.ConnectionTab.TabIndex = 4;
            this.ConnectionTab.Text = "Connection";
            this.ConnectionTab.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.label2.Location = new System.Drawing.Point(6, 230);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(464, 1);
            this.label2.TabIndex = 111;
            // 
            // ProxyPortLbl
            // 
            this.ProxyPortLbl.AnimationInterval = 0;
            this.ProxyPortLbl.DisplayBoundary = true;
            this.ProxyPortLbl.Location = new System.Drawing.Point(370, 234);
            this.ProxyPortLbl.Name = "ProxyPortLbl";
            this.ProxyPortLbl.Size = new System.Drawing.Size(100, 20);
            this.ProxyPortLbl.TabIndex = 110;
            this.ProxyPortLbl.Text = "Proxy Port: 8282";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.label1.Location = new System.Drawing.Point(6, 283);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(464, 1);
            this.label1.TabIndex = 109;
            // 
            // CoTVariableTxt
            // 
            this.CoTVariableTxt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.CoTVariableTxt.IsReadOnly = true;
            this.CoTVariableTxt.Location = new System.Drawing.Point(6, 207);
            this.CoTVariableTxt.Name = "CoTVariableTxt";
            this.CoTVariableTxt.Size = new System.Drawing.Size(358, 20);
            this.CoTVariableTxt.TabIndex = 107;
            this.CoTVariableTxt.Text = "";
            this.CoTVariableTxt.TextPaddingWidth = 0;
            this.CoTVariableTxt.Title = "Variable";
            this.CoTVariableTxt.Value = "";
            this.CoTVariableTxt.ValueAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.CoTVariableTxt.ValueReadOnly = true;
            // 
            // CoTValueTxt
            // 
            this.CoTValueTxt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.CoTValueTxt.Location = new System.Drawing.Point(6, 181);
            this.CoTValueTxt.Name = "CoTValueTxt";
            this.CoTValueTxt.Size = new System.Drawing.Size(358, 20);
            this.CoTValueTxt.TabIndex = 106;
            this.CoTValueTxt.Text = "";
            this.CoTValueTxt.Title = "Value";
            this.CoTValueTxt.Value = "";
            this.CoTValueTxt.ValueAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.CoTValueTxt.ValueReadOnly = false;
            // 
            // CoTCustomClientTxt
            // 
            this.CoTCustomClientTxt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.CoTCustomClientTxt.Location = new System.Drawing.Point(6, 260);
            this.CoTCustomClientTxt.Name = "CoTCustomClientTxt";
            this.CoTCustomClientTxt.Size = new System.Drawing.Size(355, 20);
            this.CoTCustomClientTxt.TabIndex = 105;
            this.CoTCustomClientTxt.Text = "";
            this.CoTCustomClientTxt.TextPaddingWidth = 0;
            this.CoTCustomClientTxt.Title = "Custom Client";
            this.CoTCustomClientTxt.Value = "";
            this.CoTCustomClientTxt.ValueAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.CoTCustomClientTxt.ValueReadOnly = false;
            // 
            // CoTBrowseBtn
            // 
            this.CoTBrowseBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CoTBrowseBtn.Location = new System.Drawing.Point(370, 260);
            this.CoTBrowseBtn.Name = "CoTBrowseBtn";
            this.CoTBrowseBtn.Size = new System.Drawing.Size(100, 20);
            this.CoTBrowseBtn.TabIndex = 2;
            this.CoTBrowseBtn.Text = "Browse";
            // 
            // CoTExportCertificateAuthorityBtn
            // 
            this.CoTExportCertificateAuthorityBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.CoTExportCertificateAuthorityBtn.Location = new System.Drawing.Point(6, 234);
            this.CoTExportCertificateAuthorityBtn.Name = "CoTExportCertificateAuthorityBtn";
            this.CoTExportCertificateAuthorityBtn.Size = new System.Drawing.Size(176, 20);
            this.CoTExportCertificateAuthorityBtn.TabIndex = 3;
            this.CoTExportCertificateAuthorityBtn.Text = "Export Certificate Authority";
            // 
            // CoTDestroyCertificatesBtn
            // 
            this.CoTDestroyCertificatesBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.CoTDestroyCertificatesBtn.Location = new System.Drawing.Point(188, 234);
            this.CoTDestroyCertificatesBtn.Name = "CoTDestroyCertificatesBtn";
            this.CoTDestroyCertificatesBtn.Size = new System.Drawing.Size(176, 20);
            this.CoTDestroyCertificatesBtn.TabIndex = 2;
            this.CoTDestroyCertificatesBtn.Text = "Destroy Certificates";
            // 
            // CoTResetBtn
            // 
            this.CoTResetBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.CoTResetBtn.Enabled = false;
            this.CoTResetBtn.Location = new System.Drawing.Point(370, 207);
            this.CoTResetBtn.Name = "CoTResetBtn";
            this.CoTResetBtn.Size = new System.Drawing.Size(100, 20);
            this.CoTResetBtn.TabIndex = 95;
            this.CoTResetBtn.Text = "Reset";
            // 
            // CoTUpdateBtn
            // 
            this.CoTUpdateBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.CoTUpdateBtn.Enabled = false;
            this.CoTUpdateBtn.Location = new System.Drawing.Point(370, 181);
            this.CoTUpdateBtn.Name = "CoTUpdateBtn";
            this.CoTUpdateBtn.Size = new System.Drawing.Size(100, 20);
            this.CoTUpdateBtn.TabIndex = 94;
            this.CoTUpdateBtn.Text = "Update";
            // 
            // CoTStatusTxt
            // 
            this.CoTStatusTxt.AnimationInterval = 0;
            this.CoTStatusTxt.DisplayBoundary = true;
            this.CoTStatusTxt.Location = new System.Drawing.Point(6, 287);
            this.CoTStatusTxt.Name = "CoTStatusTxt";
            this.CoTStatusTxt.Size = new System.Drawing.Size(358, 20);
            this.CoTStatusTxt.TabIndex = 2;
            this.CoTStatusTxt.Text = "Standing By...";
            // 
            // CoTConnectBtn
            // 
            this.CoTConnectBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.CoTConnectBtn.Location = new System.Drawing.Point(370, 287);
            this.CoTConnectBtn.Name = "CoTConnectBtn";
            this.CoTConnectBtn.Size = new System.Drawing.Size(100, 20);
            this.CoTConnectBtn.TabIndex = 1;
            this.CoTConnectBtn.Text = "Connect";
            // 
            // CoTVariablesVw
            // 
            this.CoTVariablesVw.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CoTVariablesVw.CheckBoxes = true;
            this.CoTVariablesVw.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.CoTVariableCol,
            this.CoTValueCol});
            this.CoTVariablesVw.FullRowSelect = true;
            this.CoTVariablesVw.GridLines = true;
            this.CoTVariablesVw.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.CoTVariablesVw.HideSelection = false;
            this.CoTVariablesVw.Location = new System.Drawing.Point(6, 6);
            this.CoTVariablesVw.MultiSelect = false;
            this.CoTVariablesVw.Name = "CoTVariablesVw";
            this.CoTVariablesVw.ShowItemToolTips = true;
            this.CoTVariablesVw.Size = new System.Drawing.Size(464, 169);
            this.CoTVariablesVw.TabIndex = 101;
            this.CoTVariablesVw.UseCompatibleStateImageBehavior = false;
            this.CoTVariablesVw.View = System.Windows.Forms.View.Details;
            // 
            // CoTVariableCol
            // 
            this.CoTVariableCol.Text = "Variable";
            this.CoTVariableCol.Width = 221;
            // 
            // CoTValueCol
            // 
            this.CoTValueCol.Text = "Value";
            this.CoTValueCol.Width = 222;
            // 
            // InjectionTab
            // 
            this.InjectionTab.Controls.Add(this.ITSendToClientBtn);
            this.InjectionTab.Controls.Add(this.ITSendToServerBtn);
            this.InjectionTab.Controls.Add(this.InjectionTabs);
            this.InjectionTab.Controls.Add(this.ITPacketTxt);
            this.InjectionTab.Location = new System.Drawing.Point(4, 28);
            this.InjectionTab.Name = "InjectionTab";
            this.InjectionTab.Padding = new System.Windows.Forms.Padding(3);
            this.InjectionTab.Size = new System.Drawing.Size(476, 313);
            this.InjectionTab.TabIndex = 2;
            this.InjectionTab.Text = "Injection";
            this.InjectionTab.UseVisualStyleBackColor = true;
            // 
            // ITSendToClientBtn
            // 
            this.ITSendToClientBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ITSendToClientBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.ITSendToClientBtn.Location = new System.Drawing.Point(262, 287);
            this.ITSendToClientBtn.Name = "ITSendToClientBtn";
            this.ITSendToClientBtn.Size = new System.Drawing.Size(101, 20);
            this.ITSendToClientBtn.TabIndex = 7;
            this.ITSendToClientBtn.Text = "Send To Client";
            // 
            // ITSendToServerBtn
            // 
            this.ITSendToServerBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ITSendToServerBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.ITSendToServerBtn.Location = new System.Drawing.Point(369, 287);
            this.ITSendToServerBtn.Name = "ITSendToServerBtn";
            this.ITSendToServerBtn.Size = new System.Drawing.Size(101, 20);
            this.ITSendToServerBtn.TabIndex = 6;
            this.ITSendToServerBtn.Text = "Send To Server";
            // 
            // InjectionTabs
            // 
            this.InjectionTabs.Alignment = System.Windows.Forms.TabAlignment.Right;
            this.InjectionTabs.Controls.Add(this.ConstructerTab);
            this.InjectionTabs.Controls.Add(this.SchedulerTab);
            this.InjectionTabs.Controls.Add(this.PrimitiveTab);
            this.InjectionTabs.Controls.Add(this.FiltersTab);
            this.InjectionTabs.DisplayBoundary = true;
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
            this.InjectionTabs.TabIndex = 0;
            // 
            // ConstructerTab
            // 
            this.ConstructerTab.Controls.Add(this.CTValueTxt);
            this.ConstructerTab.Controls.Add(this.CTHeaderTxt);
            this.ConstructerTab.Controls.Add(this.CTHeaderLbl);
            this.ConstructerTab.Controls.Add(this.CTAmountLbl);
            this.ConstructerTab.Controls.Add(this.CTStructureTxt);
            this.ConstructerTab.Controls.Add(this.CTTransferBelowBtn);
            this.ConstructerTab.Controls.Add(this.CTValueCountLbl);
            this.ConstructerTab.Controls.Add(this.CTAmountTxt);
            this.ConstructerTab.Controls.Add(this.CTRemoveBtn);
            this.ConstructerTab.Controls.Add(this.CTMoveDownBtn);
            this.ConstructerTab.Controls.Add(this.CTMoveUpBtn);
            this.ConstructerTab.Controls.Add(this.CTClearBtn);
            this.ConstructerTab.Controls.Add(this.CTWriteBooleanBtn);
            this.ConstructerTab.Controls.Add(this.CTWriteStringBtn);
            this.ConstructerTab.Controls.Add(this.CTWriteIntegerBtn);
            this.ConstructerTab.Controls.Add(this.CTValueLbl);
            this.ConstructerTab.Controls.Add(this.CTConstructerVw);
            this.ConstructerTab.Location = new System.Drawing.Point(4, 4);
            this.ConstructerTab.Name = "ConstructerTab";
            this.ConstructerTab.Padding = new System.Windows.Forms.Padding(3);
            this.ConstructerTab.Size = new System.Drawing.Size(397, 270);
            this.ConstructerTab.TabIndex = 2;
            this.ConstructerTab.Text = "Constructer";
            this.ConstructerTab.UseVisualStyleBackColor = true;
            // 
            // CTValueTxt
            // 
            this.CTValueTxt.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.CTValueTxt.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CTValueTxt.IntegralHeight = false;
            this.CTValueTxt.ItemHeight = 13;
            this.CTValueTxt.Location = new System.Drawing.Point(68, 19);
            this.CTValueTxt.Name = "CTValueTxt";
            this.CTValueTxt.Size = new System.Drawing.Size(261, 21);
            this.CTValueTxt.TabIndex = 28;
            // 
            // CTHeaderTxt
            // 
            this.CTHeaderTxt.Location = new System.Drawing.Point(6, 20);
            this.CTHeaderTxt.Maximum = new decimal(new int[] {
            4000,
            0,
            0,
            0});
            this.CTHeaderTxt.Name = "CTHeaderTxt";
            this.CTHeaderTxt.Size = new System.Drawing.Size(56, 20);
            this.CTHeaderTxt.TabIndex = 49;
            this.CTHeaderTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // CTHeaderLbl
            // 
            this.CTHeaderLbl.AutoSize = true;
            this.CTHeaderLbl.Location = new System.Drawing.Point(3, 3);
            this.CTHeaderLbl.Margin = new System.Windows.Forms.Padding(3, 0, 3, 1);
            this.CTHeaderLbl.Name = "CTHeaderLbl";
            this.CTHeaderLbl.Size = new System.Drawing.Size(42, 13);
            this.CTHeaderLbl.TabIndex = 31;
            this.CTHeaderLbl.Text = "Header";
            // 
            // CTAmountLbl
            // 
            this.CTAmountLbl.AutoSize = true;
            this.CTAmountLbl.Location = new System.Drawing.Point(332, 3);
            this.CTAmountLbl.Margin = new System.Windows.Forms.Padding(3, 0, 3, 1);
            this.CTAmountLbl.Name = "CTAmountLbl";
            this.CTAmountLbl.Size = new System.Drawing.Size(43, 13);
            this.CTAmountLbl.TabIndex = 39;
            this.CTAmountLbl.Text = "Amount";
            // 
            // CTStructureTxt
            // 
            this.CTStructureTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CTStructureTxt.Location = new System.Drawing.Point(112, 215);
            this.CTStructureTxt.Name = "CTStructureTxt";
            this.CTStructureTxt.ReadOnly = true;
            this.CTStructureTxt.Size = new System.Drawing.Size(279, 20);
            this.CTStructureTxt.TabIndex = 48;
            this.CTStructureTxt.Text = "{l}{u:0}";
            this.CTStructureTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // CTTransferBelowBtn
            // 
            this.CTTransferBelowBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CTTransferBelowBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.CTTransferBelowBtn.Location = new System.Drawing.Point(6, 241);
            this.CTTransferBelowBtn.Name = "CTTransferBelowBtn";
            this.CTTransferBelowBtn.Size = new System.Drawing.Size(100, 20);
            this.CTTransferBelowBtn.TabIndex = 46;
            this.CTTransferBelowBtn.Text = "Transfer Below";
            // 
            // CTValueCountLbl
            // 
            this.CTValueCountLbl.AnimationInterval = 0;
            this.CTValueCountLbl.DisplayBoundary = true;
            this.CTValueCountLbl.Location = new System.Drawing.Point(6, 215);
            this.CTValueCountLbl.Name = "CTValueCountLbl";
            this.CTValueCountLbl.Size = new System.Drawing.Size(100, 20);
            this.CTValueCountLbl.TabIndex = 45;
            this.CTValueCountLbl.Text = "Value Count: 0";
            // 
            // CTAmountTxt
            // 
            this.CTAmountTxt.Location = new System.Drawing.Point(335, 20);
            this.CTAmountTxt.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.CTAmountTxt.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.CTAmountTxt.Name = "CTAmountTxt";
            this.CTAmountTxt.Size = new System.Drawing.Size(56, 20);
            this.CTAmountTxt.TabIndex = 40;
            this.CTAmountTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.CTAmountTxt.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // CTRemoveBtn
            // 
            this.CTRemoveBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CTRemoveBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.CTRemoveBtn.Enabled = false;
            this.CTRemoveBtn.Location = new System.Drawing.Point(112, 241);
            this.CTRemoveBtn.Name = "CTRemoveBtn";
            this.CTRemoveBtn.Size = new System.Drawing.Size(89, 20);
            this.CTRemoveBtn.TabIndex = 38;
            this.CTRemoveBtn.Text = "Remove";
            // 
            // CTMoveDownBtn
            // 
            this.CTMoveDownBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CTMoveDownBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.CTMoveDownBtn.Enabled = false;
            this.CTMoveDownBtn.Location = new System.Drawing.Point(302, 241);
            this.CTMoveDownBtn.Name = "CTMoveDownBtn";
            this.CTMoveDownBtn.Size = new System.Drawing.Size(89, 20);
            this.CTMoveDownBtn.TabIndex = 37;
            this.CTMoveDownBtn.Text = "Move Down";
            // 
            // CTMoveUpBtn
            // 
            this.CTMoveUpBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CTMoveUpBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.CTMoveUpBtn.Enabled = false;
            this.CTMoveUpBtn.Location = new System.Drawing.Point(207, 241);
            this.CTMoveUpBtn.Name = "CTMoveUpBtn";
            this.CTMoveUpBtn.Size = new System.Drawing.Size(89, 20);
            this.CTMoveUpBtn.TabIndex = 36;
            this.CTMoveUpBtn.Text = "Move Up";
            // 
            // CTClearBtn
            // 
            this.CTClearBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CTClearBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.CTClearBtn.Location = new System.Drawing.Point(6, 46);
            this.CTClearBtn.Name = "CTClearBtn";
            this.CTClearBtn.Size = new System.Drawing.Size(73, 20);
            this.CTClearBtn.TabIndex = 35;
            this.CTClearBtn.Text = "Clear";
            // 
            // CTWriteBooleanBtn
            // 
            this.CTWriteBooleanBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CTWriteBooleanBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.CTWriteBooleanBtn.Location = new System.Drawing.Point(293, 46);
            this.CTWriteBooleanBtn.Name = "CTWriteBooleanBtn";
            this.CTWriteBooleanBtn.Size = new System.Drawing.Size(98, 20);
            this.CTWriteBooleanBtn.TabIndex = 34;
            this.CTWriteBooleanBtn.Text = "Write Boolean";
            // 
            // CTWriteStringBtn
            // 
            this.CTWriteStringBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CTWriteStringBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.CTWriteStringBtn.Location = new System.Drawing.Point(189, 46);
            this.CTWriteStringBtn.Name = "CTWriteStringBtn";
            this.CTWriteStringBtn.Size = new System.Drawing.Size(98, 20);
            this.CTWriteStringBtn.TabIndex = 33;
            this.CTWriteStringBtn.Text = "Write String";
            // 
            // CTWriteIntegerBtn
            // 
            this.CTWriteIntegerBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CTWriteIntegerBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.CTWriteIntegerBtn.Location = new System.Drawing.Point(85, 46);
            this.CTWriteIntegerBtn.Name = "CTWriteIntegerBtn";
            this.CTWriteIntegerBtn.Size = new System.Drawing.Size(98, 20);
            this.CTWriteIntegerBtn.TabIndex = 32;
            this.CTWriteIntegerBtn.Text = "Write Integer";
            // 
            // CTValueLbl
            // 
            this.CTValueLbl.AutoSize = true;
            this.CTValueLbl.Location = new System.Drawing.Point(65, 3);
            this.CTValueLbl.Name = "CTValueLbl";
            this.CTValueLbl.Size = new System.Drawing.Size(34, 13);
            this.CTValueLbl.TabIndex = 29;
            this.CTValueLbl.Text = "Value";
            // 
            // CTConstructerVw
            // 
            this.CTConstructerVw.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CTConstructerVw.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.CTTypeCol,
            this.CTValueCol,
            this.CTEncodedCol});
            this.CTConstructerVw.FullRowSelect = true;
            this.CTConstructerVw.GridLines = true;
            this.CTConstructerVw.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.CTConstructerVw.HideSelection = false;
            this.CTConstructerVw.Location = new System.Drawing.Point(6, 74);
            this.CTConstructerVw.MultiSelect = false;
            this.CTConstructerVw.Name = "CTConstructerVw";
            this.CTConstructerVw.ShowItemToolTips = true;
            this.CTConstructerVw.Size = new System.Drawing.Size(385, 138);
            this.CTConstructerVw.TabIndex = 44;
            this.CTConstructerVw.UseCompatibleStateImageBehavior = false;
            this.CTConstructerVw.View = System.Windows.Forms.View.Details;
            // 
            // CTTypeCol
            // 
            this.CTTypeCol.Text = "Type";
            this.CTTypeCol.Width = 66;
            // 
            // CTValueCol
            // 
            this.CTValueCol.Text = "Value";
            this.CTValueCol.Width = 149;
            // 
            // CTEncodedCol
            // 
            this.CTEncodedCol.Text = "Encoded";
            this.CTEncodedCol.Width = 149;
            // 
            // SchedulerTab
            // 
            this.SchedulerTab.Controls.Add(this.STAutoStartChckbx);
            this.SchedulerTab.Controls.Add(this.STDestinationTxt);
            this.SchedulerTab.Controls.Add(this.STPacketTxt);
            this.SchedulerTab.Controls.Add(this.STHotkeyTxt);
            this.SchedulerTab.Controls.Add(this.STRemoveBtn);
            this.SchedulerTab.Controls.Add(this.STDestinationLbl);
            this.SchedulerTab.Controls.Add(this.STCyclesLbl);
            this.SchedulerTab.Controls.Add(this.STCyclesTxt);
            this.SchedulerTab.Controls.Add(this.STIntervalLbl);
            this.SchedulerTab.Controls.Add(this.STClearBtn);
            this.SchedulerTab.Controls.Add(this.STCreateBtn);
            this.SchedulerTab.Controls.Add(this.STIntervalTxt);
            this.SchedulerTab.Controls.Add(this.STPacketLbl);
            this.SchedulerTab.Controls.Add(this.STSchedulerVw);
            this.SchedulerTab.Location = new System.Drawing.Point(4, 4);
            this.SchedulerTab.Name = "SchedulerTab";
            this.SchedulerTab.Padding = new System.Windows.Forms.Padding(3);
            this.SchedulerTab.Size = new System.Drawing.Size(397, 270);
            this.SchedulerTab.TabIndex = 0;
            this.SchedulerTab.Text = "Scheduler";
            this.SchedulerTab.UseVisualStyleBackColor = true;
            // 
            // STAutoStartChckbx
            // 
            this.STAutoStartChckbx.AutoSize = true;
            this.STAutoStartChckbx.Checked = true;
            this.STAutoStartChckbx.CheckState = System.Windows.Forms.CheckState.Checked;
            this.STAutoStartChckbx.Location = new System.Drawing.Point(128, 198);
            this.STAutoStartChckbx.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.STAutoStartChckbx.Name = "STAutoStartChckbx";
            this.STAutoStartChckbx.Size = new System.Drawing.Size(73, 17);
            this.STAutoStartChckbx.TabIndex = 53;
            this.STAutoStartChckbx.Text = "Auto Start";
            this.STAutoStartChckbx.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.STAutoStartChckbx.UseVisualStyleBackColor = true;
            // 
            // STDestinationTxt
            // 
            this.STDestinationTxt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.STDestinationTxt.FormattingEnabled = true;
            this.STDestinationTxt.Location = new System.Drawing.Point(201, 214);
            this.STDestinationTxt.Margin = new System.Windows.Forms.Padding(3, 2, 3, 3);
            this.STDestinationTxt.Name = "STDestinationTxt";
            this.STDestinationTxt.Size = new System.Drawing.Size(66, 21);
            this.STDestinationTxt.TabIndex = 47;
            // 
            // STPacketTxt
            // 
            this.STPacketTxt.Location = new System.Drawing.Point(6, 215);
            this.STPacketTxt.MaxLength = 2147483647;
            this.STPacketTxt.Name = "STPacketTxt";
            this.STPacketTxt.Size = new System.Drawing.Size(189, 20);
            this.STPacketTxt.TabIndex = 45;
            // 
            // STHotkeyTxt
            // 
            this.STHotkeyTxt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.STHotkeyTxt.IsReadOnly = true;
            this.STHotkeyTxt.Location = new System.Drawing.Point(201, 241);
            this.STHotkeyTxt.Name = "STHotkeyTxt";
            this.STHotkeyTxt.Size = new System.Drawing.Size(190, 20);
            this.STHotkeyTxt.TabIndex = 63;
            this.STHotkeyTxt.Text = "";
            this.STHotkeyTxt.TextPaddingWidth = 0;
            this.STHotkeyTxt.Title = "Hotkey";
            this.STHotkeyTxt.Value = "";
            this.STHotkeyTxt.ValueAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.STHotkeyTxt.ValueReadOnly = true;
            // 
            // STRemoveBtn
            // 
            this.STRemoveBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.STRemoveBtn.Enabled = false;
            this.STRemoveBtn.Location = new System.Drawing.Point(71, 241);
            this.STRemoveBtn.Name = "STRemoveBtn";
            this.STRemoveBtn.Size = new System.Drawing.Size(59, 20);
            this.STRemoveBtn.TabIndex = 61;
            this.STRemoveBtn.Text = "Remove";
            // 
            // STDestinationLbl
            // 
            this.STDestinationLbl.AutoSize = true;
            this.STDestinationLbl.Location = new System.Drawing.Point(198, 199);
            this.STDestinationLbl.Name = "STDestinationLbl";
            this.STDestinationLbl.Size = new System.Drawing.Size(60, 13);
            this.STDestinationLbl.TabIndex = 48;
            this.STDestinationLbl.Text = "Destination";
            // 
            // STCyclesLbl
            // 
            this.STCyclesLbl.AutoSize = true;
            this.STCyclesLbl.Location = new System.Drawing.Point(332, 199);
            this.STCyclesLbl.Name = "STCyclesLbl";
            this.STCyclesLbl.Size = new System.Drawing.Size(38, 13);
            this.STCyclesLbl.TabIndex = 60;
            this.STCyclesLbl.Text = "Cycles";
            // 
            // STCyclesTxt
            // 
            this.STCyclesTxt.Location = new System.Drawing.Point(335, 215);
            this.STCyclesTxt.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.STCyclesTxt.Name = "STCyclesTxt";
            this.STCyclesTxt.Size = new System.Drawing.Size(56, 20);
            this.STCyclesTxt.TabIndex = 59;
            this.STCyclesTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // STIntervalLbl
            // 
            this.STIntervalLbl.AutoSize = true;
            this.STIntervalLbl.Location = new System.Drawing.Point(270, 199);
            this.STIntervalLbl.Name = "STIntervalLbl";
            this.STIntervalLbl.Size = new System.Drawing.Size(42, 13);
            this.STIntervalLbl.TabIndex = 50;
            this.STIntervalLbl.Text = "Interval";
            // 
            // STClearBtn
            // 
            this.STClearBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.STClearBtn.Location = new System.Drawing.Point(136, 241);
            this.STClearBtn.Name = "STClearBtn";
            this.STClearBtn.Size = new System.Drawing.Size(59, 20);
            this.STClearBtn.TabIndex = 58;
            this.STClearBtn.Text = "Clear";
            // 
            // STCreateBtn
            // 
            this.STCreateBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.STCreateBtn.Location = new System.Drawing.Point(6, 241);
            this.STCreateBtn.Name = "STCreateBtn";
            this.STCreateBtn.Size = new System.Drawing.Size(59, 20);
            this.STCreateBtn.TabIndex = 54;
            this.STCreateBtn.Text = "Create";
            // 
            // STIntervalTxt
            // 
            this.STIntervalTxt.Location = new System.Drawing.Point(273, 215);
            this.STIntervalTxt.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.STIntervalTxt.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.STIntervalTxt.Name = "STIntervalTxt";
            this.STIntervalTxt.Size = new System.Drawing.Size(56, 20);
            this.STIntervalTxt.TabIndex = 49;
            this.STIntervalTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.STIntervalTxt.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // STPacketLbl
            // 
            this.STPacketLbl.AutoSize = true;
            this.STPacketLbl.Location = new System.Drawing.Point(3, 199);
            this.STPacketLbl.Name = "STPacketLbl";
            this.STPacketLbl.Size = new System.Drawing.Size(41, 13);
            this.STPacketLbl.TabIndex = 46;
            this.STPacketLbl.Text = "Packet";
            // 
            // STSchedulerVw
            // 
            this.STSchedulerVw.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.STSchedulerVw.CheckBoxes = true;
            this.STSchedulerVw.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.STPacketCol,
            this.STDestinationCol,
            this.STIntervalCol,
            this.STCyclesCol,
            this.STHotkeyCol});
            this.STSchedulerVw.FullRowSelect = true;
            this.STSchedulerVw.GridLines = true;
            this.STSchedulerVw.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.STSchedulerVw.HideSelection = false;
            this.STSchedulerVw.Location = new System.Drawing.Point(6, 6);
            this.STSchedulerVw.MultiSelect = false;
            this.STSchedulerVw.Name = "STSchedulerVw";
            this.STSchedulerVw.ShowItemToolTips = true;
            this.STSchedulerVw.Size = new System.Drawing.Size(385, 190);
            this.STSchedulerVw.TabIndex = 0;
            this.STSchedulerVw.UseCompatibleStateImageBehavior = false;
            this.STSchedulerVw.View = System.Windows.Forms.View.Details;
            // 
            // STPacketCol
            // 
            this.STPacketCol.Text = "Packet";
            this.STPacketCol.Width = 120;
            // 
            // STDestinationCol
            // 
            this.STDestinationCol.Text = "Destination";
            this.STDestinationCol.Width = 73;
            // 
            // STIntervalCol
            // 
            this.STIntervalCol.Text = "Interval";
            this.STIntervalCol.Width = 53;
            // 
            // STCyclesCol
            // 
            this.STCyclesCol.Text = "Cycles";
            this.STCyclesCol.Width = 53;
            // 
            // STHotkeyCol
            // 
            this.STHotkeyCol.Text = "Hotkey";
            this.STHotkeyCol.Width = 65;
            // 
            // PrimitiveTab
            // 
            this.PrimitiveTab.Controls.Add(this.PTSaveAsBtn);
            this.PrimitiveTab.Controls.Add(this.PTLengthTxt);
            this.PrimitiveTab.Controls.Add(this.PTHeaderTxt);
            this.PrimitiveTab.Controls.Add(this.PTCorruptedTxt);
            this.PrimitiveTab.Controls.Add(this.PTPacketTxt);
            this.PrimitiveTab.Location = new System.Drawing.Point(4, 4);
            this.PrimitiveTab.Name = "PrimitiveTab";
            this.PrimitiveTab.Padding = new System.Windows.Forms.Padding(3);
            this.PrimitiveTab.Size = new System.Drawing.Size(397, 270);
            this.PrimitiveTab.TabIndex = 1;
            this.PrimitiveTab.Text = "Primitive";
            this.PrimitiveTab.UseVisualStyleBackColor = true;
            // 
            // PTSaveAsBtn
            // 
            this.PTSaveAsBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.PTSaveAsBtn.Location = new System.Drawing.Point(304, 241);
            this.PTSaveAsBtn.Name = "PTSaveAsBtn";
            this.PTSaveAsBtn.Size = new System.Drawing.Size(87, 20);
            this.PTSaveAsBtn.TabIndex = 11;
            this.PTSaveAsBtn.Text = "Save As";
            // 
            // PTLengthTxt
            // 
            this.PTLengthTxt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.PTLengthTxt.IsReadOnly = true;
            this.PTLengthTxt.Location = new System.Drawing.Point(103, 241);
            this.PTLengthTxt.Name = "PTLengthTxt";
            this.PTLengthTxt.Size = new System.Drawing.Size(91, 20);
            this.PTLengthTxt.TabIndex = 10;
            this.PTLengthTxt.Text = "0";
            this.PTLengthTxt.TextPaddingWidth = 0;
            this.PTLengthTxt.Title = "Length";
            this.PTLengthTxt.Value = "0";
            this.PTLengthTxt.ValueAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.PTLengthTxt.ValueReadOnly = true;
            // 
            // PTHeaderTxt
            // 
            this.PTHeaderTxt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.PTHeaderTxt.IsReadOnly = true;
            this.PTHeaderTxt.Location = new System.Drawing.Point(6, 241);
            this.PTHeaderTxt.Name = "PTHeaderTxt";
            this.PTHeaderTxt.Size = new System.Drawing.Size(91, 20);
            this.PTHeaderTxt.TabIndex = 9;
            this.PTHeaderTxt.Text = "0";
            this.PTHeaderTxt.TextPaddingWidth = 0;
            this.PTHeaderTxt.Title = "Header";
            this.PTHeaderTxt.Value = "0";
            this.PTHeaderTxt.ValueAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.PTHeaderTxt.ValueReadOnly = true;
            // 
            // PTCorruptedTxt
            // 
            this.PTCorruptedTxt.BackColor = System.Drawing.Color.Firebrick;
            this.PTCorruptedTxt.IsReadOnly = true;
            this.PTCorruptedTxt.Location = new System.Drawing.Point(200, 241);
            this.PTCorruptedTxt.Name = "PTCorruptedTxt";
            this.PTCorruptedTxt.Size = new System.Drawing.Size(98, 20);
            this.PTCorruptedTxt.TabIndex = 8;
            this.PTCorruptedTxt.Text = "True";
            this.PTCorruptedTxt.TextPaddingWidth = 0;
            this.PTCorruptedTxt.Title = "Corrupted";
            this.PTCorruptedTxt.Value = "True";
            this.PTCorruptedTxt.ValueAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.PTCorruptedTxt.ValueReadOnly = true;
            // 
            // PTPacketTxt
            // 
            this.PTPacketTxt.Location = new System.Drawing.Point(6, 6);
            this.PTPacketTxt.MaxLength = 2147483647;
            this.PTPacketTxt.Multiline = true;
            this.PTPacketTxt.Name = "PTPacketTxt";
            this.PTPacketTxt.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.PTPacketTxt.Size = new System.Drawing.Size(385, 229);
            this.PTPacketTxt.TabIndex = 3;
            // 
            // FiltersTab
            // 
            this.FiltersTab.Controls.Add(this.FTReplacementTxt);
            this.FiltersTab.Controls.Add(this.FTDestinationLbl);
            this.FiltersTab.Controls.Add(this.FTDestinationTxt);
            this.FiltersTab.Controls.Add(this.FTHeaderTxt);
            this.FiltersTab.Controls.Add(this.FTHeaderLbl);
            this.FiltersTab.Controls.Add(this.FTRemoveBtn);
            this.FiltersTab.Controls.Add(this.FTCreateBtn);
            this.FiltersTab.Controls.Add(this.FTActionLbl);
            this.FiltersTab.Controls.Add(this.FTActionTxt);
            this.FiltersTab.Controls.Add(this.FTFiltersVw);
            this.FiltersTab.Location = new System.Drawing.Point(4, 4);
            this.FiltersTab.Name = "FiltersTab";
            this.FiltersTab.Padding = new System.Windows.Forms.Padding(3);
            this.FiltersTab.Size = new System.Drawing.Size(397, 270);
            this.FiltersTab.TabIndex = 3;
            this.FiltersTab.Text = "Filters";
            this.FiltersTab.UseVisualStyleBackColor = true;
            // 
            // FTReplacementTxt
            // 
            this.FTReplacementTxt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.FTReplacementTxt.IsReadOnly = true;
            this.FTReplacementTxt.Location = new System.Drawing.Point(6, 241);
            this.FTReplacementTxt.Name = "FTReplacementTxt";
            this.FTReplacementTxt.Size = new System.Drawing.Size(385, 20);
            this.FTReplacementTxt.TabIndex = 54;
            this.FTReplacementTxt.Text = "";
            this.FTReplacementTxt.TextPaddingWidth = 0;
            this.FTReplacementTxt.Title = "Replacement";
            this.FTReplacementTxt.Value = "";
            this.FTReplacementTxt.ValueAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.FTReplacementTxt.ValueReadOnly = true;
            // 
            // FTDestinationLbl
            // 
            this.FTDestinationLbl.AutoSize = true;
            this.FTDestinationLbl.Location = new System.Drawing.Point(66, 198);
            this.FTDestinationLbl.Name = "FTDestinationLbl";
            this.FTDestinationLbl.Size = new System.Drawing.Size(60, 13);
            this.FTDestinationLbl.TabIndex = 53;
            this.FTDestinationLbl.Text = "Destination";
            // 
            // FTDestinationTxt
            // 
            this.FTDestinationTxt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FTDestinationTxt.FormattingEnabled = true;
            this.FTDestinationTxt.Location = new System.Drawing.Point(69, 214);
            this.FTDestinationTxt.Name = "FTDestinationTxt";
            this.FTDestinationTxt.Size = new System.Drawing.Size(66, 21);
            this.FTDestinationTxt.TabIndex = 52;
            // 
            // FTHeaderTxt
            // 
            this.FTHeaderTxt.Location = new System.Drawing.Point(6, 215);
            this.FTHeaderTxt.Margin = new System.Windows.Forms.Padding(3, 4, 3, 3);
            this.FTHeaderTxt.Maximum = new decimal(new int[] {
            4000,
            0,
            0,
            0});
            this.FTHeaderTxt.Name = "FTHeaderTxt";
            this.FTHeaderTxt.Size = new System.Drawing.Size(57, 20);
            this.FTHeaderTxt.TabIndex = 51;
            this.FTHeaderTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FTHeaderLbl
            // 
            this.FTHeaderLbl.AutoSize = true;
            this.FTHeaderLbl.Location = new System.Drawing.Point(3, 198);
            this.FTHeaderLbl.Name = "FTHeaderLbl";
            this.FTHeaderLbl.Size = new System.Drawing.Size(42, 13);
            this.FTHeaderLbl.TabIndex = 50;
            this.FTHeaderLbl.Text = "Header";
            // 
            // FTRemoveBtn
            // 
            this.FTRemoveBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.FTRemoveBtn.Enabled = false;
            this.FTRemoveBtn.Location = new System.Drawing.Point(305, 214);
            this.FTRemoveBtn.Name = "FTRemoveBtn";
            this.FTRemoveBtn.Size = new System.Drawing.Size(86, 20);
            this.FTRemoveBtn.TabIndex = 8;
            this.FTRemoveBtn.Text = "Remove";
            // 
            // FTCreateBtn
            // 
            this.FTCreateBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.FTCreateBtn.Location = new System.Drawing.Point(213, 215);
            this.FTCreateBtn.Name = "FTCreateBtn";
            this.FTCreateBtn.Size = new System.Drawing.Size(86, 20);
            this.FTCreateBtn.TabIndex = 7;
            this.FTCreateBtn.Text = "Create";
            // 
            // FTActionLbl
            // 
            this.FTActionLbl.AutoSize = true;
            this.FTActionLbl.Location = new System.Drawing.Point(138, 198);
            this.FTActionLbl.Name = "FTActionLbl";
            this.FTActionLbl.Size = new System.Drawing.Size(37, 13);
            this.FTActionLbl.TabIndex = 2;
            this.FTActionLbl.Text = "Action";
            // 
            // FTActionTxt
            // 
            this.FTActionTxt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FTActionTxt.FormattingEnabled = true;
            this.FTActionTxt.Items.AddRange(new object[] {
            "Block",
            "Replace",
            "Execute"});
            this.FTActionTxt.Location = new System.Drawing.Point(141, 214);
            this.FTActionTxt.Name = "FTActionTxt";
            this.FTActionTxt.Size = new System.Drawing.Size(66, 21);
            this.FTActionTxt.TabIndex = 1;
            // 
            // FTFiltersVw
            // 
            this.FTFiltersVw.CheckBoxes = true;
            this.FTFiltersVw.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.FTHeaderCol,
            this.FTDestinationCol,
            this.FTActionCol,
            this.FTReplacementCol});
            this.FTFiltersVw.FullRowSelect = true;
            this.FTFiltersVw.GridLines = true;
            this.FTFiltersVw.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.FTFiltersVw.HideSelection = false;
            this.FTFiltersVw.Location = new System.Drawing.Point(6, 6);
            this.FTFiltersVw.MultiSelect = false;
            this.FTFiltersVw.Name = "FTFiltersVw";
            this.FTFiltersVw.ShowItemToolTips = true;
            this.FTFiltersVw.Size = new System.Drawing.Size(385, 189);
            this.FTFiltersVw.TabIndex = 0;
            this.FTFiltersVw.UseCompatibleStateImageBehavior = false;
            this.FTFiltersVw.View = System.Windows.Forms.View.Details;
            // 
            // FTHeaderCol
            // 
            this.FTHeaderCol.Text = "Header";
            this.FTHeaderCol.Width = 55;
            // 
            // FTDestinationCol
            // 
            this.FTDestinationCol.Text = "Destination";
            this.FTDestinationCol.Width = 68;
            // 
            // FTActionCol
            // 
            this.FTActionCol.Text = "Action";
            this.FTActionCol.Width = 68;
            // 
            // FTReplacementCol
            // 
            this.FTReplacementCol.Text = "Replacement";
            this.FTReplacementCol.Width = 176;
            // 
            // ITPacketTxt
            // 
            this.ITPacketTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ITPacketTxt.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.ITPacketTxt.ItemHeight = 14;
            this.ITPacketTxt.Location = new System.Drawing.Point(6, 287);
            this.ITPacketTxt.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.ITPacketTxt.Name = "ITPacketTxt";
            this.ITPacketTxt.Size = new System.Drawing.Size(250, 20);
            this.ITPacketTxt.TabIndex = 8;
            // 
            // ToolboxTab
            // 
            this.ToolboxTab.Controls.Add(this.TT16BitInputLbl);
            this.ToolboxTab.Controls.Add(this.TT32BitInputLbl);
            this.ToolboxTab.Controls.Add(this.TTIntInputTxt);
            this.ToolboxTab.Controls.Add(this.TTIntOutputTxt);
            this.ToolboxTab.Controls.Add(this.TTUShortOutputTxt);
            this.ToolboxTab.Controls.Add(this.TTDecodeIntBtn);
            this.ToolboxTab.Controls.Add(this.TTDecodeUShortBtn);
            this.ToolboxTab.Controls.Add(this.TTUShortInputTxt);
            this.ToolboxTab.Location = new System.Drawing.Point(4, 28);
            this.ToolboxTab.Name = "ToolboxTab";
            this.ToolboxTab.Padding = new System.Windows.Forms.Padding(3);
            this.ToolboxTab.Size = new System.Drawing.Size(476, 313);
            this.ToolboxTab.TabIndex = 3;
            this.ToolboxTab.Text = "Toolbox";
            this.ToolboxTab.UseVisualStyleBackColor = true;
            // 
            // TT16BitInputLbl
            // 
            this.TT16BitInputLbl.AutoSize = true;
            this.TT16BitInputLbl.Location = new System.Drawing.Point(6, 260);
            this.TT16BitInputLbl.Name = "TT16BitInputLbl";
            this.TT16BitInputLbl.Size = new System.Drawing.Size(100, 13);
            this.TT16BitInputLbl.TabIndex = 50;
            this.TT16BitInputLbl.Text = "16-Bit Integer Input:";
            // 
            // TT32BitInputLbl
            // 
            this.TT32BitInputLbl.AutoSize = true;
            this.TT32BitInputLbl.Location = new System.Drawing.Point(6, 288);
            this.TT32BitInputLbl.Name = "TT32BitInputLbl";
            this.TT32BitInputLbl.Size = new System.Drawing.Size(100, 13);
            this.TT32BitInputLbl.TabIndex = 0;
            this.TT32BitInputLbl.Text = "32-Bit Integer Input:";
            // 
            // TTIntInputTxt
            // 
            this.TTIntInputTxt.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.TTIntInputTxt.Location = new System.Drawing.Point(108, 285);
            this.TTIntInputTxt.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.TTIntInputTxt.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.TTIntInputTxt.Name = "TTIntInputTxt";
            this.TTIntInputTxt.Size = new System.Drawing.Size(126, 22);
            this.TTIntInputTxt.TabIndex = 0;
            this.TTIntInputTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TTIntOutputTxt
            // 
            this.TTIntOutputTxt.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.TTIntOutputTxt.Location = new System.Drawing.Point(240, 285);
            this.TTIntOutputTxt.Name = "TTIntOutputTxt";
            this.TTIntOutputTxt.Size = new System.Drawing.Size(126, 22);
            this.TTIntOutputTxt.TabIndex = 37;
            this.TTIntOutputTxt.Text = "[0][0][0][0]";
            this.TTIntOutputTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TTUShortOutputTxt
            // 
            this.TTUShortOutputTxt.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TTUShortOutputTxt.Location = new System.Drawing.Point(240, 257);
            this.TTUShortOutputTxt.Name = "TTUShortOutputTxt";
            this.TTUShortOutputTxt.Size = new System.Drawing.Size(126, 22);
            this.TTUShortOutputTxt.TabIndex = 38;
            this.TTUShortOutputTxt.Text = "[0][0]";
            this.TTUShortOutputTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TTDecodeIntBtn
            // 
            this.TTDecodeIntBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.TTDecodeIntBtn.Location = new System.Drawing.Point(372, 287);
            this.TTDecodeIntBtn.Name = "TTDecodeIntBtn";
            this.TTDecodeIntBtn.Size = new System.Drawing.Size(98, 20);
            this.TTDecodeIntBtn.TabIndex = 34;
            this.TTDecodeIntBtn.Text = "Decode Int32";
            // 
            // TTDecodeUShortBtn
            // 
            this.TTDecodeUShortBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.TTDecodeUShortBtn.Location = new System.Drawing.Point(372, 259);
            this.TTDecodeUShortBtn.Name = "TTDecodeUShortBtn";
            this.TTDecodeUShortBtn.Size = new System.Drawing.Size(98, 20);
            this.TTDecodeUShortBtn.TabIndex = 35;
            this.TTDecodeUShortBtn.Text = "Decode UInt16";
            // 
            // TTUShortInputTxt
            // 
            this.TTUShortInputTxt.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.TTUShortInputTxt.Location = new System.Drawing.Point(108, 257);
            this.TTUShortInputTxt.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.TTUShortInputTxt.Name = "TTUShortInputTxt";
            this.TTUShortInputTxt.Size = new System.Drawing.Size(126, 22);
            this.TTUShortInputTxt.TabIndex = 49;
            this.TTUShortInputTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ModulesTab
            // 
            this.ModulesTab.AllowDrop = true;
            this.ModulesTab.Controls.Add(this.sKoreLabelBox1);
            this.ModulesTab.Controls.Add(this.MTResourceBtn);
            this.ModulesTab.Controls.Add(this.MTHabboNameTxt);
            this.ModulesTab.Controls.Add(this.MTHabboNameLbl);
            this.ModulesTab.Controls.Add(this.MTAuthorsLbl);
            this.ModulesTab.Controls.Add(this.MTAuthorsTxt);
            this.ModulesTab.Controls.Add(this.MTUninstallModuleBtn);
            this.ModulesTab.Controls.Add(this.MTInstallModuleBtn);
            this.ModulesTab.Controls.Add(this.MTAuthorPctbx);
            this.ModulesTab.Controls.Add(this.MTModulesVw);
            this.ModulesTab.Location = new System.Drawing.Point(4, 28);
            this.ModulesTab.Name = "ModulesTab";
            this.ModulesTab.Padding = new System.Windows.Forms.Padding(3);
            this.ModulesTab.Size = new System.Drawing.Size(476, 313);
            this.ModulesTab.TabIndex = 1;
            this.ModulesTab.Text = "Modules";
            this.ModulesTab.UseVisualStyleBackColor = true;
            // 
            // sKoreLabelBox1
            // 
            this.sKoreLabelBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.sKoreLabelBox1.IsReadOnly = true;
            this.sKoreLabelBox1.Location = new System.Drawing.Point(77, 287);
            this.sKoreLabelBox1.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.sKoreLabelBox1.Name = "sKoreLabelBox1";
            this.sKoreLabelBox1.Size = new System.Drawing.Size(127, 20);
            this.sKoreLabelBox1.TabIndex = 16;
            this.sKoreLabelBox1.Text = "8055";
            this.sKoreLabelBox1.TextPaddingWidth = 0;
            this.sKoreLabelBox1.Title = "Install Port";
            this.sKoreLabelBox1.Value = "8055";
            this.sKoreLabelBox1.ValueAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.sKoreLabelBox1.ValueReadOnly = true;
            // 
            // MTResourceBtn
            // 
            this.MTResourceBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.MTResourceBtn.Enabled = false;
            this.MTResourceBtn.Location = new System.Drawing.Point(77, 254);
            this.MTResourceBtn.Name = "MTResourceBtn";
            this.MTResourceBtn.Size = new System.Drawing.Size(127, 20);
            this.MTResourceBtn.TabIndex = 13;
            // 
            // MTHabboNameTxt
            // 
            this.MTHabboNameTxt.Location = new System.Drawing.Point(210, 228);
            this.MTHabboNameTxt.Margin = new System.Windows.Forms.Padding(3, 4, 3, 3);
            this.MTHabboNameTxt.Name = "MTHabboNameTxt";
            this.MTHabboNameTxt.ReadOnly = true;
            this.MTHabboNameTxt.Size = new System.Drawing.Size(127, 20);
            this.MTHabboNameTxt.TabIndex = 9;
            this.MTHabboNameTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // MTHabboNameLbl
            // 
            this.MTHabboNameLbl.AutoSize = true;
            this.MTHabboNameLbl.Location = new System.Drawing.Point(207, 211);
            this.MTHabboNameLbl.Name = "MTHabboNameLbl";
            this.MTHabboNameLbl.Size = new System.Drawing.Size(70, 13);
            this.MTHabboNameLbl.TabIndex = 8;
            this.MTHabboNameLbl.Text = "Habbo Name";
            // 
            // MTAuthorsLbl
            // 
            this.MTAuthorsLbl.AutoSize = true;
            this.MTAuthorsLbl.Location = new System.Drawing.Point(74, 211);
            this.MTAuthorsLbl.Name = "MTAuthorsLbl";
            this.MTAuthorsLbl.Size = new System.Drawing.Size(49, 13);
            this.MTAuthorsLbl.TabIndex = 7;
            this.MTAuthorsLbl.Text = "Author(s)";
            // 
            // MTAuthorsTxt
            // 
            this.MTAuthorsTxt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MTAuthorsTxt.Enabled = false;
            this.MTAuthorsTxt.FormattingEnabled = true;
            this.MTAuthorsTxt.Location = new System.Drawing.Point(77, 227);
            this.MTAuthorsTxt.Name = "MTAuthorsTxt";
            this.MTAuthorsTxt.Size = new System.Drawing.Size(127, 21);
            this.MTAuthorsTxt.TabIndex = 6;
            // 
            // MTUninstallModuleBtn
            // 
            this.MTUninstallModuleBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.MTUninstallModuleBtn.Enabled = false;
            this.MTUninstallModuleBtn.Location = new System.Drawing.Point(210, 287);
            this.MTUninstallModuleBtn.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.MTUninstallModuleBtn.Name = "MTUninstallModuleBtn";
            this.MTUninstallModuleBtn.Size = new System.Drawing.Size(127, 20);
            this.MTUninstallModuleBtn.TabIndex = 3;
            this.MTUninstallModuleBtn.Text = "Uninstall Module";
            // 
            // MTInstallModuleBtn
            // 
            this.MTInstallModuleBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.MTInstallModuleBtn.Location = new System.Drawing.Point(343, 287);
            this.MTInstallModuleBtn.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.MTInstallModuleBtn.Name = "MTInstallModuleBtn";
            this.MTInstallModuleBtn.Size = new System.Drawing.Size(127, 20);
            this.MTInstallModuleBtn.TabIndex = 2;
            this.MTInstallModuleBtn.Text = "Install Module";
            // 
            // MTAuthorPctbx
            // 
            this.MTAuthorPctbx.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.MTAuthorPctbx.BackColor = System.Drawing.Color.Transparent;
            this.MTAuthorPctbx.Enabled = false;
            this.MTAuthorPctbx.ErrorImage = null;
            this.MTAuthorPctbx.Image = global::Tanji.Properties.Resources.Avatar;
            this.MTAuthorPctbx.InitialImage = null;
            this.MTAuthorPctbx.Location = new System.Drawing.Point(6, 205);
            this.MTAuthorPctbx.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.MTAuthorPctbx.Name = "MTAuthorPctbx";
            this.MTAuthorPctbx.Size = new System.Drawing.Size(65, 108);
            this.MTAuthorPctbx.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.MTAuthorPctbx.TabIndex = 1;
            this.MTAuthorPctbx.TabStop = false;
            // 
            // MTModulesVw
            // 
            this.MTModulesVw.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.MTNameCol,
            this.MTDescriptionCol,
            this.MTVersionCol,
            this.MTStateCol});
            this.MTModulesVw.FullRowSelect = true;
            this.MTModulesVw.GridLines = true;
            this.MTModulesVw.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.MTModulesVw.HideSelection = false;
            this.MTModulesVw.Location = new System.Drawing.Point(6, 6);
            this.MTModulesVw.MultiSelect = false;
            this.MTModulesVw.Name = "MTModulesVw";
            this.MTModulesVw.ShowItemToolTips = true;
            this.MTModulesVw.Size = new System.Drawing.Size(464, 199);
            this.MTModulesVw.TabIndex = 0;
            this.MTModulesVw.UseCompatibleStateImageBehavior = false;
            this.MTModulesVw.View = System.Windows.Forms.View.Details;
            // 
            // MTNameCol
            // 
            this.MTNameCol.Text = "Name";
            this.MTNameCol.Width = 98;
            // 
            // MTDescriptionCol
            // 
            this.MTDescriptionCol.Text = "Description";
            this.MTDescriptionCol.Width = 215;
            // 
            // MTVersionCol
            // 
            this.MTVersionCol.Text = "Version";
            this.MTVersionCol.Width = 68;
            // 
            // MTStateCol
            // 
            this.MTStateCol.Text = "State";
            this.MTStateCol.Width = 68;
            // 
            // AboutTab
            // 
            this.AboutTab.Controls.Add(this.DonateBtn);
            this.AboutTab.Controls.Add(this.SpeaqerBtn);
            this.AboutTab.Controls.Add(this.SNGButton);
            this.AboutTab.Controls.Add(this.DarkboxBtn);
            this.AboutTab.Controls.Add(this.DarkStarBtn);
            this.AboutTab.Controls.Add(this.ArachisBtn);
            this.AboutTab.Location = new System.Drawing.Point(4, 28);
            this.AboutTab.Name = "AboutTab";
            this.AboutTab.Padding = new System.Windows.Forms.Padding(3);
            this.AboutTab.Size = new System.Drawing.Size(476, 313);
            this.AboutTab.TabIndex = 5;
            this.AboutTab.Text = "About";
            this.AboutTab.UseVisualStyleBackColor = true;
            // 
            // DonateBtn
            // 
            this.DonateBtn.BackColor = System.Drawing.Color.Green;
            this.DonateBtn.Location = new System.Drawing.Point(203, 120);
            this.DonateBtn.Name = "DonateBtn";
            this.DonateBtn.Size = new System.Drawing.Size(53, 20);
            this.DonateBtn.Skin = System.Drawing.Color.Green;
            this.DonateBtn.TabIndex = 13;
            this.DonateBtn.Text = "Donate";
            // 
            // SpeaqerBtn
            // 
            this.SpeaqerBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(106)))), ((int)(((byte)(218)))));
            this.SpeaqerBtn.Location = new System.Drawing.Point(97, 172);
            this.SpeaqerBtn.Name = "SpeaqerBtn";
            this.SpeaqerBtn.Size = new System.Drawing.Size(100, 20);
            this.SpeaqerBtn.Skin = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(106)))), ((int)(((byte)(218)))));
            this.SpeaqerBtn.TabIndex = 8;
            this.SpeaqerBtn.Text = "@SpeaqerDev";
            // 
            // SNGButton
            // 
            this.SNGButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.SNGButton.Location = new System.Drawing.Point(262, 172);
            this.SNGButton.Name = "SNGButton";
            this.SNGButton.Size = new System.Drawing.Size(117, 20);
            this.SNGButton.TabIndex = 6;
            this.SNGButton.Text = "SnGForum.info";
            // 
            // DarkboxBtn
            // 
            this.DarkboxBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.DarkboxBtn.Location = new System.Drawing.Point(262, 120);
            this.DarkboxBtn.Name = "DarkboxBtn";
            this.DarkboxBtn.Size = new System.Drawing.Size(117, 20);
            this.DarkboxBtn.TabIndex = 5;
            this.DarkboxBtn.Text = "Darkbox.nl";
            // 
            // DarkStarBtn
            // 
            this.DarkStarBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(106)))), ((int)(((byte)(218)))));
            this.DarkStarBtn.Location = new System.Drawing.Point(97, 146);
            this.DarkStarBtn.Name = "DarkStarBtn";
            this.DarkStarBtn.Size = new System.Drawing.Size(100, 20);
            this.DarkStarBtn.Skin = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(106)))), ((int)(((byte)(218)))));
            this.DarkStarBtn.TabIndex = 2;
            this.DarkStarBtn.Text = "@DarkStar851";
            // 
            // ArachisBtn
            // 
            this.ArachisBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(106)))), ((int)(((byte)(218)))));
            this.ArachisBtn.Location = new System.Drawing.Point(97, 120);
            this.ArachisBtn.Name = "ArachisBtn";
            this.ArachisBtn.Size = new System.Drawing.Size(100, 20);
            this.ArachisBtn.Skin = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(106)))), ((int)(((byte)(218)))));
            this.ArachisBtn.TabIndex = 1;
            this.ArachisBtn.Text = "@ArachisH";
            // 
            // InjectionMenu
            // 
            this.InjectionMenu.InputBox = null;
            this.InjectionMenu.Name = "InjectionMenu";
            this.InjectionMenu.Size = new System.Drawing.Size(174, 170);
            // 
            // SavePacketDlg
            // 
            this.SavePacketDlg.DefaultExt = "pkt";
            this.SavePacketDlg.Filter = "Packet (*.pkt)|*.pkt";
            this.SavePacketDlg.Title = "Tanji ~ Save Packet";
            // 
            // TanjiVersionTxt
            // 
            this.TanjiVersionTxt.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.TanjiVersionTxt.IsLink = true;
            this.TanjiVersionTxt.LinkColor = System.Drawing.SystemColors.HotTrack;
            this.TanjiVersionTxt.Name = "TanjiVersionTxt";
            this.TanjiVersionTxt.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.TanjiVersionTxt.Size = new System.Drawing.Size(51, 19);
            this.TanjiVersionTxt.Text = "v0.0.0";
            this.TanjiVersionTxt.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.TanjiVersionTxt.Click += new System.EventHandler(this.TanjiVersionTxt_Click);
            // 
            // SchedulesTxt
            // 
            this.SchedulesTxt.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.SchedulesTxt.Name = "SchedulesTxt";
            this.SchedulesTxt.Size = new System.Drawing.Size(87, 19);
            this.SchedulesTxt.Text = "Schedules: 0/0";
            // 
            // FiltersTxt
            // 
            this.FiltersTxt.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.FiltersTxt.Name = "FiltersTxt";
            this.FiltersTxt.Size = new System.Drawing.Size(65, 19);
            this.FiltersTxt.Text = "Filters: 0/0";
            // 
            // ModulesTxt
            // 
            this.ModulesTxt.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.ModulesTxt.Name = "ModulesTxt";
            this.ModulesTxt.Size = new System.Drawing.Size(80, 19);
            this.ModulesTxt.Text = "Modules: 0/0";
            // 
            // TanjiInfoTxt
            // 
            this.TanjiInfoTxt.IsLink = true;
            this.TanjiInfoTxt.LinkColor = System.Drawing.SystemColors.HotTrack;
            this.TanjiInfoTxt.Name = "TanjiInfoTxt";
            this.TanjiInfoTxt.Size = new System.Drawing.Size(176, 19);
            this.TanjiInfoTxt.Spring = true;
            this.TanjiInfoTxt.Text = "Github - ArachisH/Tanji";
            this.TanjiInfoTxt.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.TanjiInfoTxt.Click += new System.EventHandler(this.TanjiInfoTxt_Click);
            // 
            // TanjiStrip
            // 
            this.TanjiStrip.AllowMerge = false;
            this.TanjiStrip.BackColor = System.Drawing.Color.White;
            this.TanjiStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TanjiVersionTxt,
            this.SchedulesTxt,
            this.FiltersTxt,
            this.ModulesTxt,
            this.TanjiInfoTxt});
            this.TanjiStrip.Location = new System.Drawing.Point(0, 345);
            this.TanjiStrip.Name = "TanjiStrip";
            this.TanjiStrip.Size = new System.Drawing.Size(484, 24);
            this.TanjiStrip.SizingGrip = false;
            this.TanjiStrip.TabIndex = 5;
            this.TanjiStrip.Text = "TanjiStrip";
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(484, 369);
            this.Controls.Add(this.TanjiTabs);
            this.Controls.Add(this.TanjiStrip);
            this.MaximizeBox = false;
            this.Name = "MainFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tanji ~ Disconnected";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainFrm_FormClosed);
            this.Load += new System.EventHandler(this.MainFrm_Load);
            this.TanjiTabs.ResumeLayout(false);
            this.ConnectionTab.ResumeLayout(false);
            this.InjectionTab.ResumeLayout(false);
            this.InjectionTabs.ResumeLayout(false);
            this.ConstructerTab.ResumeLayout(false);
            this.ConstructerTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CTHeaderTxt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CTAmountTxt)).EndInit();
            this.SchedulerTab.ResumeLayout(false);
            this.SchedulerTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.STCyclesTxt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.STIntervalTxt)).EndInit();
            this.PrimitiveTab.ResumeLayout(false);
            this.PrimitiveTab.PerformLayout();
            this.FiltersTab.ResumeLayout(false);
            this.FiltersTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FTHeaderTxt)).EndInit();
            this.ToolboxTab.ResumeLayout(false);
            this.ToolboxTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TTIntInputTxt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TTUShortInputTxt)).EndInit();
            this.ModulesTab.ResumeLayout(false);
            this.ModulesTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MTAuthorPctbx)).EndInit();
            this.AboutTab.ResumeLayout(false);
            this.TanjiStrip.ResumeLayout(false);
            this.TanjiStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public Sulakore.Components.SKoreInjectionMenu InjectionMenu;
        private System.Windows.Forms.TabPage AboutTab;
        private System.Windows.Forms.TabPage ModulesTab;
        private System.Windows.Forms.TabPage ToolboxTab;
        private System.Windows.Forms.Label TT16BitInputLbl;
        private System.Windows.Forms.Label TT32BitInputLbl;
        internal System.Windows.Forms.NumericUpDown TTIntInputTxt;
        internal System.Windows.Forms.TextBox TTIntOutputTxt;
        internal System.Windows.Forms.TextBox TTUShortOutputTxt;
        internal Sulakore.Components.SKoreButton TTDecodeIntBtn;
        internal Sulakore.Components.SKoreButton TTDecodeUShortBtn;
        internal System.Windows.Forms.NumericUpDown TTUShortInputTxt;
        private System.Windows.Forms.TabPage InjectionTab;
        internal Sulakore.Components.SKoreButton ITSendToClientBtn;
        internal Sulakore.Components.SKoreButton ITSendToServerBtn;
        internal Sulakore.Components.SKoreTabControl InjectionTabs;
        internal System.Windows.Forms.TabPage ConstructerTab;
        internal System.Windows.Forms.ComboBox CTValueTxt;
        internal System.Windows.Forms.NumericUpDown CTHeaderTxt;
        private System.Windows.Forms.Label CTHeaderLbl;
        private System.Windows.Forms.Label CTAmountLbl;
        internal System.Windows.Forms.TextBox CTStructureTxt;
        internal Sulakore.Components.SKoreButton CTTransferBelowBtn;
        internal Sulakore.Components.SKoreLabel CTValueCountLbl;
        internal System.Windows.Forms.NumericUpDown CTAmountTxt;
        internal Sulakore.Components.SKoreButton CTRemoveBtn;
        internal Sulakore.Components.SKoreButton CTMoveDownBtn;
        internal Sulakore.Components.SKoreButton CTMoveUpBtn;
        internal Sulakore.Components.SKoreButton CTClearBtn;
        internal Sulakore.Components.SKoreButton CTWriteBooleanBtn;
        internal Sulakore.Components.SKoreButton CTWriteStringBtn;
        internal Sulakore.Components.SKoreButton CTWriteIntegerBtn;
        private System.Windows.Forms.Label CTValueLbl;
        internal Sulakore.Components.SKoreConstructView CTConstructerVw;
        private System.Windows.Forms.ColumnHeader CTTypeCol;
        private System.Windows.Forms.ColumnHeader CTValueCol;
        private System.Windows.Forms.ColumnHeader CTEncodedCol;
        internal System.Windows.Forms.TabPage SchedulerTab;
        internal Sulakore.Components.SKoreButton STRemoveBtn;
        internal System.Windows.Forms.CheckBox STAutoStartChckbx;
        private System.Windows.Forms.Label STDestinationLbl;
        private System.Windows.Forms.Label STCyclesLbl;
        internal System.Windows.Forms.NumericUpDown STCyclesTxt;
        private System.Windows.Forms.Label STIntervalLbl;
        internal Sulakore.Components.SKoreButton STClearBtn;
        internal Sulakore.Components.SKoreButton STCreateBtn;
        internal System.Windows.Forms.NumericUpDown STIntervalTxt;
        internal System.Windows.Forms.ComboBox STDestinationTxt;
        private System.Windows.Forms.Label STPacketLbl;
        internal System.Windows.Forms.TextBox STPacketTxt;
        internal Sulakore.Components.SKoreScheduleView STSchedulerVw;
        private System.Windows.Forms.ColumnHeader STPacketCol;
        private System.Windows.Forms.ColumnHeader STDestinationCol;
        private System.Windows.Forms.ColumnHeader STIntervalCol;
        private System.Windows.Forms.ColumnHeader STCyclesCol;
        internal System.Windows.Forms.TabPage PrimitiveTab;
        internal System.Windows.Forms.TextBox PTPacketTxt;
        internal System.Windows.Forms.TabPage FiltersTab;
        private System.Windows.Forms.Label FTDestinationLbl;
        internal System.Windows.Forms.ComboBox FTDestinationTxt;
        internal System.Windows.Forms.NumericUpDown FTHeaderTxt;
        private System.Windows.Forms.Label FTHeaderLbl;
        internal Sulakore.Components.SKoreButton FTRemoveBtn;
        internal Sulakore.Components.SKoreButton FTCreateBtn;
        private System.Windows.Forms.Label FTActionLbl;
        internal System.Windows.Forms.ComboBox FTActionTxt;
        internal Sulakore.Components.SKoreListView FTFiltersVw;
        private System.Windows.Forms.ColumnHeader FTHeaderCol;
        private System.Windows.Forms.ColumnHeader FTDestinationCol;
        private System.Windows.Forms.ColumnHeader FTActionCol;
        private System.Windows.Forms.ColumnHeader FTReplacementCol;
        internal System.Windows.Forms.ComboBox ITPacketTxt;
        private System.Windows.Forms.TabPage ConnectionTab;
        internal Sulakore.Components.SKoreButton CoTBrowseBtn;
        internal Sulakore.Components.SKoreButton CoTExportCertificateAuthorityBtn;
        internal Sulakore.Components.SKoreButton CoTDestroyCertificatesBtn;
        internal Sulakore.Components.SKoreButton CoTResetBtn;
        internal Sulakore.Components.SKoreButton CoTUpdateBtn;
        internal Sulakore.Components.SKoreLabel CoTStatusTxt;
        internal Sulakore.Components.SKoreButton CoTConnectBtn;
        internal Sulakore.Components.SKoreListView CoTVariablesVw;
        private System.Windows.Forms.ColumnHeader CoTVariableCol;
        private System.Windows.Forms.ColumnHeader CoTValueCol;
        internal Sulakore.Components.SKoreTabControl TanjiTabs;
        internal System.Windows.Forms.OpenFileDialog CustomClientDlg;
        internal System.Windows.Forms.OpenFileDialog InstallModuleDlg;
        internal Sulakore.Components.SKoreListView MTModulesVw;
        private System.Windows.Forms.ColumnHeader MTNameCol;
        private System.Windows.Forms.ColumnHeader MTVersionCol;
        internal System.Windows.Forms.PictureBox MTAuthorPctbx;
        internal Sulakore.Components.SKoreButton MTUninstallModuleBtn;
        internal Sulakore.Components.SKoreButton MTInstallModuleBtn;
        private System.Windows.Forms.ColumnHeader MTDescriptionCol;
        private System.Windows.Forms.ColumnHeader MTStateCol;
        private System.Windows.Forms.Label MTAuthorsLbl;
        internal System.Windows.Forms.ComboBox MTAuthorsTxt;
        internal Sulakore.Components.SKoreButton MTResourceBtn;
        internal System.Windows.Forms.TextBox MTHabboNameTxt;
        private System.Windows.Forms.ColumnHeader STHotkeyCol;
        internal Sulakore.Components.SKoreLabelBox CoTCustomClientTxt;
        internal Sulakore.Components.SKoreLabelBox CoTVariableTxt;
        internal Sulakore.Components.SKoreLabelBox CoTValueTxt;
        internal Sulakore.Components.SKoreLabelBox PTCorruptedTxt;
        internal Sulakore.Components.SKoreLabelBox PTLengthTxt;
        internal Sulakore.Components.SKoreLabelBox PTHeaderTxt;
        internal Sulakore.Components.SKoreButton PTSaveAsBtn;
        internal System.Windows.Forms.SaveFileDialog SavePacketDlg;
        internal Sulakore.Components.SKoreLabelBox FTReplacementTxt;
        internal Sulakore.Components.SKoreLabelBox STHotkeyTxt;
        private Sulakore.Components.SKoreLabelBox sKoreLabelBox1;
        internal System.Windows.Forms.Label MTHabboNameLbl;
        internal Sulakore.Components.SKoreButton DarkStarBtn;
        internal Sulakore.Components.SKoreButton ArachisBtn;
        internal Sulakore.Components.SKoreButton SpeaqerBtn;
        internal Sulakore.Components.SKoreButton SNGButton;
        internal Sulakore.Components.SKoreButton DarkboxBtn;
        internal Sulakore.Components.SKoreButton DonateBtn;
        internal System.Windows.Forms.ToolStripStatusLabel TanjiVersionTxt;
        internal System.Windows.Forms.ToolStripStatusLabel SchedulesTxt;
        internal System.Windows.Forms.ToolStripStatusLabel FiltersTxt;
        internal System.Windows.Forms.ToolStripStatusLabel ModulesTxt;
        private System.Windows.Forms.ToolStripStatusLabel TanjiInfoTxt;
        private System.Windows.Forms.StatusStrip TanjiStrip;
        private System.Windows.Forms.Label label1;
        private Sulakore.Components.SKoreLabel ProxyPortLbl;
        private System.Windows.Forms.Label label2;
    }
}