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
            components = new System.ComponentModel.Container();
            var listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] { "furnidata.load.url", "" }, -1);
            var listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] { "productdata.load.url", "" }, -1);
            var listViewItem3 = new System.Windows.Forms.ListViewItem(new string[] { "avatareditor.promohabbos", "" }, -1);
            var listViewItem4 = new System.Windows.Forms.ListViewItem(new string[] { "external.texts.txt", "" }, -1);
            var listViewItem5 = new System.Windows.Forms.ListViewItem(new string[] { "external.variables.txt", "" }, -1);
            var listViewItem6 = new System.Windows.Forms.ListViewItem(new string[] { "external.figurepartlist.txt", "" }, -1);
            var listViewItem7 = new System.Windows.Forms.ListViewItem(new string[] { "external.override.texts.txt", "" }, -1);
            var listViewItem8 = new System.Windows.Forms.ListViewItem(new string[] { "external.override.variables.txt", "" }, -1);
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFrm));
            CustomClientDlg = new System.Windows.Forms.OpenFileDialog();
            InstallModuleDlg = new System.Windows.Forms.OpenFileDialog();
            TanjiTabs = new Tanji.Controls.TanjiTabControl();
            ConnectionTab = new System.Windows.Forms.TabPage();
            label2 = new System.Windows.Forms.Label();
            CoTProxyPortLbl = new Tanji.Controls.TanjiLabel();
            label1 = new System.Windows.Forms.Label();
            CoTVariableTxt = new Tanji.Controls.TanjiLabelBox();
            CoTValueTxt = new Tanji.Controls.TanjiLabelBox();
            CoTCustomClientTxt = new Tanji.Controls.TanjiLabelBox();
            CoTBrowseBtn = new Tanji.Controls.TanjiButton();
            CoTExportCertificateAuthorityBtn = new Tanji.Controls.TanjiButton();
            CoTDestroyCertificatesBtn = new Tanji.Controls.TanjiButton();
            CoTResetBtn = new Tanji.Controls.TanjiButton();
            CoTUpdateBtn = new Tanji.Controls.TanjiButton();
            CoTStatusTxt = new Tanji.Controls.TanjiLabel();
            CoTConnectBtn = new Tanji.Controls.TanjiButton();
            CoTVariablesVw = new Tanji.Controls.TanjiListView();
            CoTVariableCol = new System.Windows.Forms.ColumnHeader();
            CoTValueCol = new System.Windows.Forms.ColumnHeader();
            InjectionTab = new System.Windows.Forms.TabPage();
            ITSendToClientBtn = new Tanji.Controls.TanjiButton();
            ITSendToServerBtn = new Tanji.Controls.TanjiButton();
            InjectionTabs = new Tanji.Controls.TanjiTabControl();
            ConstructerTab = new System.Windows.Forms.TabPage();
            CTValueTxt = new System.Windows.Forms.ComboBox();
            CTHeaderTxt = new System.Windows.Forms.NumericUpDown();
            CTHeaderLbl = new System.Windows.Forms.Label();
            CTAmountLbl = new System.Windows.Forms.Label();
            CTStructureTxt = new System.Windows.Forms.TextBox();
            CTTransferBelowBtn = new Tanji.Controls.TanjiButton();
            CTValueCountLbl = new Tanji.Controls.TanjiLabel();
            CTAmountTxt = new System.Windows.Forms.NumericUpDown();
            CTRemoveBtn = new Tanji.Controls.TanjiButton();
            CTMoveDownBtn = new Tanji.Controls.TanjiButton();
            CTMoveUpBtn = new Tanji.Controls.TanjiButton();
            CTClearBtn = new Tanji.Controls.TanjiButton();
            CTWriteBooleanBtn = new Tanji.Controls.TanjiButton();
            CTWriteStringBtn = new Tanji.Controls.TanjiButton();
            CTWriteIntegerBtn = new Tanji.Controls.TanjiButton();
            CTValueLbl = new System.Windows.Forms.Label();
            CTConstructerVw = new Tanji.Controls.SKoreConstructView();
            CTTypeCol = new System.Windows.Forms.ColumnHeader();
            CTValueCol = new System.Windows.Forms.ColumnHeader();
            CTEncodedCol = new System.Windows.Forms.ColumnHeader();
            SchedulerTab = new System.Windows.Forms.TabPage();
            STAutoStartChckbx = new System.Windows.Forms.CheckBox();
            STDestinationTxt = new System.Windows.Forms.ComboBox();
            STPacketTxt = new System.Windows.Forms.TextBox();
            STHotkeyTxt = new Tanji.Controls.TanjiLabelBox();
            STRemoveBtn = new Tanji.Controls.TanjiButton();
            STDestinationLbl = new System.Windows.Forms.Label();
            STCyclesLbl = new System.Windows.Forms.Label();
            STCyclesTxt = new System.Windows.Forms.NumericUpDown();
            STIntervalLbl = new System.Windows.Forms.Label();
            STClearBtn = new Tanji.Controls.TanjiButton();
            STCreateBtn = new Tanji.Controls.TanjiButton();
            STIntervalTxt = new System.Windows.Forms.NumericUpDown();
            STPacketLbl = new System.Windows.Forms.Label();
            STSchedulerVw = new Tanji.Controls.SKoreScheduleView();
            STPacketCol = new System.Windows.Forms.ColumnHeader();
            STDestinationCol = new System.Windows.Forms.ColumnHeader();
            STIntervalCol = new System.Windows.Forms.ColumnHeader();
            STCyclesCol = new System.Windows.Forms.ColumnHeader();
            STHotkeyCol = new System.Windows.Forms.ColumnHeader();
            PrimitiveTab = new System.Windows.Forms.TabPage();
            PTSaveAsBtn = new Tanji.Controls.TanjiButton();
            PTLengthTxt = new Tanji.Controls.TanjiLabelBox();
            PTHeaderTxt = new Tanji.Controls.TanjiLabelBox();
            PTCorruptedTxt = new Tanji.Controls.TanjiLabelBox();
            PTPacketTxt = new System.Windows.Forms.TextBox();
            FiltersTab = new System.Windows.Forms.TabPage();
            FTReplacementTxt = new Tanji.Controls.TanjiLabelBox();
            FTDestinationLbl = new System.Windows.Forms.Label();
            FTDestinationTxt = new System.Windows.Forms.ComboBox();
            FTHeaderTxt = new System.Windows.Forms.NumericUpDown();
            FTHeaderLbl = new System.Windows.Forms.Label();
            FTRemoveBtn = new Tanji.Controls.TanjiButton();
            FTCreateBtn = new Tanji.Controls.TanjiButton();
            FTActionLbl = new System.Windows.Forms.Label();
            FTActionTxt = new System.Windows.Forms.ComboBox();
            FTFiltersVw = new Tanji.Controls.TanjiListView();
            FTHeaderCol = new System.Windows.Forms.ColumnHeader();
            FTDestinationCol = new System.Windows.Forms.ColumnHeader();
            FTActionCol = new System.Windows.Forms.ColumnHeader();
            FTReplacementCol = new System.Windows.Forms.ColumnHeader();
            ITPacketTxt = new System.Windows.Forms.ComboBox();
            ToolboxTab = new System.Windows.Forms.TabPage();
            TT16BitInputLbl = new System.Windows.Forms.Label();
            TT32BitInputLbl = new System.Windows.Forms.Label();
            TTIntInputTxt = new System.Windows.Forms.NumericUpDown();
            TTIntOutputTxt = new System.Windows.Forms.TextBox();
            TTUShortOutputTxt = new System.Windows.Forms.TextBox();
            TTDecodeIntBtn = new Tanji.Controls.TanjiButton();
            TTDecodeUShortBtn = new Tanji.Controls.TanjiButton();
            TTUShortInputTxt = new System.Windows.Forms.NumericUpDown();
            ModulesTab = new System.Windows.Forms.TabPage();
            MTSeperator1 = new System.Windows.Forms.Label();
            sKoreLabelBox1 = new Tanji.Controls.TanjiLabelBox();
            MTResourceBtn = new Tanji.Controls.TanjiButton();
            MTHabboNameTxt = new System.Windows.Forms.TextBox();
            MTHabboNameLbl = new System.Windows.Forms.Label();
            MTAuthorsLbl = new System.Windows.Forms.Label();
            MTAuthorsTxt = new System.Windows.Forms.ComboBox();
            MTUninstallModuleBtn = new Tanji.Controls.TanjiButton();
            MTInstallModuleBtn = new Tanji.Controls.TanjiButton();
            MTAuthorPctbx = new System.Windows.Forms.PictureBox();
            MTModulesVw = new Tanji.Controls.TanjiListView();
            MTNameCol = new System.Windows.Forms.ColumnHeader();
            MTDescriptionCol = new System.Windows.Forms.ColumnHeader();
            MTVersionCol = new System.Windows.Forms.ColumnHeader();
            MTStateCol = new System.Windows.Forms.ColumnHeader();
            AboutTab = new System.Windows.Forms.TabPage();
            Sellout2Btn = new Tanji.Controls.TanjiButton();
            SelloutBtn = new Tanji.Controls.TanjiButton();
            HarbleDiscordBtn = new Tanji.Controls.TanjiButton();
            DonateBtn = new Tanji.Controls.TanjiButton();
            SpeaqerBtn = new Tanji.Controls.TanjiButton();
            SNGButton = new Tanji.Controls.TanjiButton();
            DarkboxBtn = new Tanji.Controls.TanjiButton();
            DarkStarBtn = new Tanji.Controls.TanjiButton();
            ArachisBtn = new Tanji.Controls.TanjiButton();
            InjectionMenu = new Tanji.Controls.SKoreInjectionMenu(components);
            SavePacketDlg = new System.Windows.Forms.SaveFileDialog();
            TanjiVersionTxt = new System.Windows.Forms.ToolStripStatusLabel();
            SchedulesTxt = new System.Windows.Forms.ToolStripStatusLabel();
            FiltersTxt = new System.Windows.Forms.ToolStripStatusLabel();
            ModulesTxt = new System.Windows.Forms.ToolStripStatusLabel();
            TanjiInfoTxt = new System.Windows.Forms.ToolStripStatusLabel();
            TanjiStrip = new System.Windows.Forms.StatusStrip();
            TanjiTabs.SuspendLayout();
            ConnectionTab.SuspendLayout();
            InjectionTab.SuspendLayout();
            InjectionTabs.SuspendLayout();
            ConstructerTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)CTHeaderTxt).BeginInit();
            ((System.ComponentModel.ISupportInitialize)CTAmountTxt).BeginInit();
            SchedulerTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)STCyclesTxt).BeginInit();
            ((System.ComponentModel.ISupportInitialize)STIntervalTxt).BeginInit();
            PrimitiveTab.SuspendLayout();
            FiltersTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)FTHeaderTxt).BeginInit();
            ToolboxTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)TTIntInputTxt).BeginInit();
            ((System.ComponentModel.ISupportInitialize)TTUShortInputTxt).BeginInit();
            ModulesTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)MTAuthorPctbx).BeginInit();
            AboutTab.SuspendLayout();
            TanjiStrip.SuspendLayout();
            SuspendLayout();
            // 
            // CustomClientDlg
            // 
            CustomClientDlg.DefaultExt = "swf";
            CustomClientDlg.Filter = "Shockwave Flash File (*.swf)|*.swf";
            CustomClientDlg.Title = "Tanji ~ Custom Client";
            // 
            // InstallModuleDlg
            // 
            InstallModuleDlg.DefaultExt = "dll";
            InstallModuleDlg.Filter = ".NET Assembly (*.dll, *.exe)|*.dll; *.exe|Dynamic Link Library (*.dll)|*.dll|Executable (*.exe)|*.exe";
            InstallModuleDlg.Title = "Tanji ~ Install Module";
            // 
            // TanjiTabs
            // 
            TanjiTabs.AllowDrop = true;
            TanjiTabs.Controls.Add(ConnectionTab);
            TanjiTabs.Controls.Add(InjectionTab);
            TanjiTabs.Controls.Add(ToolboxTab);
            TanjiTabs.Controls.Add(ModulesTab);
            TanjiTabs.Controls.Add(AboutTab);
            TanjiTabs.DisplayBoundary = true;
            TanjiTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            TanjiTabs.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            TanjiTabs.IsDisplayingBorder = true;
            TanjiTabs.ItemSize = new System.Drawing.Size(96, 24);
            TanjiTabs.Location = new System.Drawing.Point(0, 0);
            TanjiTabs.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TanjiTabs.Name = "TanjiTabs";
            TanjiTabs.SelectedIndex = 0;
            TanjiTabs.Size = new System.Drawing.Size(565, 402);
            TanjiTabs.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            TanjiTabs.TabIndex = 6;
            // 
            // ConnectionTab
            // 
            ConnectionTab.BackColor = System.Drawing.Color.White;
            ConnectionTab.Controls.Add(label2);
            ConnectionTab.Controls.Add(CoTProxyPortLbl);
            ConnectionTab.Controls.Add(label1);
            ConnectionTab.Controls.Add(CoTVariableTxt);
            ConnectionTab.Controls.Add(CoTValueTxt);
            ConnectionTab.Controls.Add(CoTCustomClientTxt);
            ConnectionTab.Controls.Add(CoTBrowseBtn);
            ConnectionTab.Controls.Add(CoTExportCertificateAuthorityBtn);
            ConnectionTab.Controls.Add(CoTDestroyCertificatesBtn);
            ConnectionTab.Controls.Add(CoTResetBtn);
            ConnectionTab.Controls.Add(CoTUpdateBtn);
            ConnectionTab.Controls.Add(CoTStatusTxt);
            ConnectionTab.Controls.Add(CoTConnectBtn);
            ConnectionTab.Controls.Add(CoTVariablesVw);
            ConnectionTab.Location = new System.Drawing.Point(4, 28);
            ConnectionTab.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ConnectionTab.Name = "ConnectionTab";
            ConnectionTab.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ConnectionTab.Size = new System.Drawing.Size(557, 370);
            ConnectionTab.TabIndex = 4;
            ConnectionTab.Text = "Connection";
            // 
            // label2
            // 
            label2.BackColor = System.Drawing.Color.FromArgb(243, 63, 63);
            label2.Location = new System.Drawing.Point(7, 265);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(541, 1);
            label2.TabIndex = 111;
            // 
            // CoTProxyPortLbl
            // 
            CoTProxyPortLbl.AnimationInterval = 0;
            CoTProxyPortLbl.DisplayBoundary = true;
            CoTProxyPortLbl.Location = new System.Drawing.Point(432, 270);
            CoTProxyPortLbl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CoTProxyPortLbl.Name = "CoTProxyPortLbl";
            CoTProxyPortLbl.Size = new System.Drawing.Size(117, 23);
            CoTProxyPortLbl.TabIndex = 110;
            CoTProxyPortLbl.Text = "Proxy Port: 0";
            // 
            // label1
            // 
            label1.BackColor = System.Drawing.Color.FromArgb(243, 63, 63);
            label1.Location = new System.Drawing.Point(7, 327);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(541, 1);
            label1.TabIndex = 109;
            // 
            // CoTVariableTxt
            // 
            CoTVariableTxt.BackColor = System.Drawing.Color.FromArgb(243, 63, 63);
            CoTVariableTxt.IsReadOnly = true;
            CoTVariableTxt.Location = new System.Drawing.Point(7, 239);
            CoTVariableTxt.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CoTVariableTxt.Name = "CoTVariableTxt";
            CoTVariableTxt.Size = new System.Drawing.Size(418, 20);
            CoTVariableTxt.TabIndex = 107;
            CoTVariableTxt.Text = "";
            CoTVariableTxt.TextPaddingWidth = 0;
            CoTVariableTxt.Title = "Variable";
            CoTVariableTxt.Value = "";
            CoTVariableTxt.ValueAlign = System.Windows.Forms.HorizontalAlignment.Left;
            CoTVariableTxt.ValueReadOnly = true;
            // 
            // CoTValueTxt
            // 
            CoTValueTxt.BackColor = System.Drawing.Color.FromArgb(243, 63, 63);
            CoTValueTxt.Location = new System.Drawing.Point(7, 209);
            CoTValueTxt.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CoTValueTxt.Name = "CoTValueTxt";
            CoTValueTxt.Size = new System.Drawing.Size(418, 20);
            CoTValueTxt.TabIndex = 106;
            CoTValueTxt.Text = "";
            CoTValueTxt.Title = "Value";
            CoTValueTxt.Value = "";
            CoTValueTxt.ValueAlign = System.Windows.Forms.HorizontalAlignment.Left;
            CoTValueTxt.ValueReadOnly = false;
            // 
            // CoTCustomClientTxt
            // 
            CoTCustomClientTxt.BackColor = System.Drawing.Color.FromArgb(243, 63, 63);
            CoTCustomClientTxt.Location = new System.Drawing.Point(7, 300);
            CoTCustomClientTxt.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CoTCustomClientTxt.Name = "CoTCustomClientTxt";
            CoTCustomClientTxt.Size = new System.Drawing.Size(414, 20);
            CoTCustomClientTxt.TabIndex = 105;
            CoTCustomClientTxt.Text = "";
            CoTCustomClientTxt.TextPaddingWidth = 0;
            CoTCustomClientTxt.Title = "Custom Client";
            CoTCustomClientTxt.Value = "";
            CoTCustomClientTxt.ValueAlign = System.Windows.Forms.HorizontalAlignment.Left;
            CoTCustomClientTxt.ValueReadOnly = false;
            // 
            // CoTBrowseBtn
            // 
            CoTBrowseBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            CoTBrowseBtn.Location = new System.Drawing.Point(432, 304);
            CoTBrowseBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CoTBrowseBtn.Name = "CoTBrowseBtn";
            CoTBrowseBtn.Size = new System.Drawing.Size(117, 23);
            CoTBrowseBtn.TabIndex = 2;
            CoTBrowseBtn.Text = "Browse";
            // 
            // CoTExportCertificateAuthorityBtn
            // 
            CoTExportCertificateAuthorityBtn.BackColor = System.Drawing.Color.FromArgb(243, 63, 63);
            CoTExportCertificateAuthorityBtn.Location = new System.Drawing.Point(7, 270);
            CoTExportCertificateAuthorityBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CoTExportCertificateAuthorityBtn.Name = "CoTExportCertificateAuthorityBtn";
            CoTExportCertificateAuthorityBtn.Size = new System.Drawing.Size(205, 23);
            CoTExportCertificateAuthorityBtn.TabIndex = 3;
            CoTExportCertificateAuthorityBtn.Text = "Export Certificate Authority";
            // 
            // CoTDestroyCertificatesBtn
            // 
            CoTDestroyCertificatesBtn.BackColor = System.Drawing.Color.FromArgb(243, 63, 63);
            CoTDestroyCertificatesBtn.Location = new System.Drawing.Point(219, 270);
            CoTDestroyCertificatesBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CoTDestroyCertificatesBtn.Name = "CoTDestroyCertificatesBtn";
            CoTDestroyCertificatesBtn.Size = new System.Drawing.Size(205, 23);
            CoTDestroyCertificatesBtn.TabIndex = 2;
            CoTDestroyCertificatesBtn.Text = "Destroy Certificates";
            // 
            // CoTResetBtn
            // 
            CoTResetBtn.BackColor = System.Drawing.Color.FromArgb(243, 63, 63);
            CoTResetBtn.Enabled = false;
            CoTResetBtn.Location = new System.Drawing.Point(432, 239);
            CoTResetBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CoTResetBtn.Name = "CoTResetBtn";
            CoTResetBtn.Size = new System.Drawing.Size(117, 23);
            CoTResetBtn.TabIndex = 95;
            CoTResetBtn.Text = "Reset";
            // 
            // CoTUpdateBtn
            // 
            CoTUpdateBtn.BackColor = System.Drawing.Color.FromArgb(243, 63, 63);
            CoTUpdateBtn.Enabled = false;
            CoTUpdateBtn.Location = new System.Drawing.Point(432, 209);
            CoTUpdateBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CoTUpdateBtn.Name = "CoTUpdateBtn";
            CoTUpdateBtn.Size = new System.Drawing.Size(117, 23);
            CoTUpdateBtn.TabIndex = 94;
            CoTUpdateBtn.Text = "Update";
            // 
            // CoTStatusTxt
            // 
            CoTStatusTxt.AnimationInterval = 0;
            CoTStatusTxt.DisplayBoundary = true;
            CoTStatusTxt.Location = new System.Drawing.Point(7, 331);
            CoTStatusTxt.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CoTStatusTxt.Name = "CoTStatusTxt";
            CoTStatusTxt.Size = new System.Drawing.Size(418, 23);
            CoTStatusTxt.TabIndex = 2;
            CoTStatusTxt.Text = "Standing By...";
            // 
            // CoTConnectBtn
            // 
            CoTConnectBtn.BackColor = System.Drawing.Color.FromArgb(243, 63, 63);
            CoTConnectBtn.Location = new System.Drawing.Point(432, 331);
            CoTConnectBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CoTConnectBtn.Name = "CoTConnectBtn";
            CoTConnectBtn.Size = new System.Drawing.Size(117, 23);
            CoTConnectBtn.TabIndex = 1;
            CoTConnectBtn.Text = "Connect";
            // 
            // CoTVariablesVw
            // 
            CoTVariablesVw.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            CoTVariablesVw.CheckBoxes = true;
            CoTVariablesVw.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { CoTVariableCol, CoTValueCol });
            CoTVariablesVw.FullRowSelect = true;
            CoTVariablesVw.GridLines = true;
            CoTVariablesVw.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            listViewItem1.StateImageIndex = 0;
            listViewItem2.StateImageIndex = 0;
            listViewItem3.StateImageIndex = 0;
            listViewItem4.StateImageIndex = 0;
            listViewItem5.StateImageIndex = 0;
            listViewItem6.StateImageIndex = 0;
            listViewItem7.StateImageIndex = 0;
            listViewItem8.StateImageIndex = 0;
            CoTVariablesVw.Items.AddRange(new System.Windows.Forms.ListViewItem[] { listViewItem1, listViewItem2, listViewItem3, listViewItem4, listViewItem5, listViewItem6, listViewItem7, listViewItem8 });
            CoTVariablesVw.Location = new System.Drawing.Point(7, 7);
            CoTVariablesVw.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CoTVariablesVw.MultiSelect = false;
            CoTVariablesVw.Name = "CoTVariablesVw";
            CoTVariablesVw.ShowItemToolTips = true;
            CoTVariablesVw.Size = new System.Drawing.Size(541, 194);
            CoTVariablesVw.TabIndex = 101;
            CoTVariablesVw.UseCompatibleStateImageBehavior = false;
            CoTVariablesVw.View = System.Windows.Forms.View.Details;
            // 
            // CoTVariableCol
            // 
            CoTVariableCol.Text = "Variable";
            CoTVariableCol.Width = 221;
            // 
            // CoTValueCol
            // 
            CoTValueCol.Text = "Value";
            CoTValueCol.Width = 222;
            // 
            // InjectionTab
            // 
            InjectionTab.Controls.Add(ITSendToClientBtn);
            InjectionTab.Controls.Add(ITSendToServerBtn);
            InjectionTab.Controls.Add(InjectionTabs);
            InjectionTab.Controls.Add(ITPacketTxt);
            InjectionTab.Location = new System.Drawing.Point(4, 28);
            InjectionTab.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            InjectionTab.Name = "InjectionTab";
            InjectionTab.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            InjectionTab.Size = new System.Drawing.Size(557, 370);
            InjectionTab.TabIndex = 2;
            InjectionTab.Text = "Injection";
            InjectionTab.UseVisualStyleBackColor = true;
            // 
            // ITSendToClientBtn
            // 
            ITSendToClientBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            ITSendToClientBtn.BackColor = System.Drawing.Color.FromArgb(243, 63, 63);
            ITSendToClientBtn.Location = new System.Drawing.Point(306, 335);
            ITSendToClientBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ITSendToClientBtn.Name = "ITSendToClientBtn";
            ITSendToClientBtn.Size = new System.Drawing.Size(118, 23);
            ITSendToClientBtn.TabIndex = 7;
            ITSendToClientBtn.Text = "Send To Client";
            // 
            // ITSendToServerBtn
            // 
            ITSendToServerBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            ITSendToServerBtn.BackColor = System.Drawing.Color.FromArgb(243, 63, 63);
            ITSendToServerBtn.Location = new System.Drawing.Point(430, 335);
            ITSendToServerBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ITSendToServerBtn.Name = "ITSendToServerBtn";
            ITSendToServerBtn.Size = new System.Drawing.Size(118, 23);
            ITSendToServerBtn.TabIndex = 6;
            ITSendToServerBtn.Text = "Send To Server";
            // 
            // InjectionTabs
            // 
            InjectionTabs.Alignment = System.Windows.Forms.TabAlignment.Right;
            InjectionTabs.Controls.Add(ConstructerTab);
            InjectionTabs.Controls.Add(SchedulerTab);
            InjectionTabs.Controls.Add(PrimitiveTab);
            InjectionTabs.Controls.Add(FiltersTab);
            InjectionTabs.DisplayBoundary = true;
            InjectionTabs.Dock = System.Windows.Forms.DockStyle.Top;
            InjectionTabs.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            InjectionTabs.IsDisplayingBorder = true;
            InjectionTabs.ItemSize = new System.Drawing.Size(24, 65);
            InjectionTabs.Location = new System.Drawing.Point(4, 3);
            InjectionTabs.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            InjectionTabs.Multiline = true;
            InjectionTabs.Name = "InjectionTabs";
            InjectionTabs.SelectedIndex = 0;
            InjectionTabs.Size = new System.Drawing.Size(549, 321);
            InjectionTabs.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            InjectionTabs.TabIndex = 0;
            // 
            // ConstructerTab
            // 
            ConstructerTab.Controls.Add(CTValueTxt);
            ConstructerTab.Controls.Add(CTHeaderTxt);
            ConstructerTab.Controls.Add(CTHeaderLbl);
            ConstructerTab.Controls.Add(CTAmountLbl);
            ConstructerTab.Controls.Add(CTStructureTxt);
            ConstructerTab.Controls.Add(CTTransferBelowBtn);
            ConstructerTab.Controls.Add(CTValueCountLbl);
            ConstructerTab.Controls.Add(CTAmountTxt);
            ConstructerTab.Controls.Add(CTRemoveBtn);
            ConstructerTab.Controls.Add(CTMoveDownBtn);
            ConstructerTab.Controls.Add(CTMoveUpBtn);
            ConstructerTab.Controls.Add(CTClearBtn);
            ConstructerTab.Controls.Add(CTWriteBooleanBtn);
            ConstructerTab.Controls.Add(CTWriteStringBtn);
            ConstructerTab.Controls.Add(CTWriteIntegerBtn);
            ConstructerTab.Controls.Add(CTValueLbl);
            ConstructerTab.Controls.Add(CTConstructerVw);
            ConstructerTab.Location = new System.Drawing.Point(4, 4);
            ConstructerTab.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ConstructerTab.Name = "ConstructerTab";
            ConstructerTab.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ConstructerTab.Size = new System.Drawing.Size(476, 313);
            ConstructerTab.TabIndex = 2;
            ConstructerTab.Text = "Constructer";
            ConstructerTab.UseVisualStyleBackColor = true;
            // 
            // CTValueTxt
            // 
            CTValueTxt.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            CTValueTxt.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            CTValueTxt.IntegralHeight = false;
            CTValueTxt.ItemHeight = 15;
            CTValueTxt.Location = new System.Drawing.Point(79, 22);
            CTValueTxt.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CTValueTxt.Name = "CTValueTxt";
            CTValueTxt.Size = new System.Drawing.Size(304, 23);
            CTValueTxt.TabIndex = 28;
            // 
            // CTHeaderTxt
            // 
            CTHeaderTxt.Location = new System.Drawing.Point(7, 23);
            CTHeaderTxt.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CTHeaderTxt.Maximum = new decimal(new int[] { 4000, 0, 0, 0 });
            CTHeaderTxt.Name = "CTHeaderTxt";
            CTHeaderTxt.Size = new System.Drawing.Size(65, 23);
            CTHeaderTxt.TabIndex = 49;
            CTHeaderTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // CTHeaderLbl
            // 
            CTHeaderLbl.AutoSize = true;
            CTHeaderLbl.Location = new System.Drawing.Point(4, 3);
            CTHeaderLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 1);
            CTHeaderLbl.Name = "CTHeaderLbl";
            CTHeaderLbl.Size = new System.Drawing.Size(45, 15);
            CTHeaderLbl.TabIndex = 31;
            CTHeaderLbl.Text = "Header";
            // 
            // CTAmountLbl
            // 
            CTAmountLbl.AutoSize = true;
            CTAmountLbl.Location = new System.Drawing.Point(387, 3);
            CTAmountLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 1);
            CTAmountLbl.Name = "CTAmountLbl";
            CTAmountLbl.Size = new System.Drawing.Size(51, 15);
            CTAmountLbl.TabIndex = 39;
            CTAmountLbl.Text = "Amount";
            // 
            // CTStructureTxt
            // 
            CTStructureTxt.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            CTStructureTxt.Location = new System.Drawing.Point(131, 248);
            CTStructureTxt.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CTStructureTxt.Name = "CTStructureTxt";
            CTStructureTxt.ReadOnly = true;
            CTStructureTxt.Size = new System.Drawing.Size(326, 23);
            CTStructureTxt.TabIndex = 48;
            CTStructureTxt.Text = "{l}{u:0}";
            CTStructureTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // CTTransferBelowBtn
            // 
            CTTransferBelowBtn.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            CTTransferBelowBtn.BackColor = System.Drawing.Color.FromArgb(243, 63, 63);
            CTTransferBelowBtn.Location = new System.Drawing.Point(8, 278);
            CTTransferBelowBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CTTransferBelowBtn.Name = "CTTransferBelowBtn";
            CTTransferBelowBtn.Size = new System.Drawing.Size(117, 23);
            CTTransferBelowBtn.TabIndex = 46;
            CTTransferBelowBtn.Text = "Transfer Below";
            // 
            // CTValueCountLbl
            // 
            CTValueCountLbl.AnimationInterval = 0;
            CTValueCountLbl.DisplayBoundary = true;
            CTValueCountLbl.Location = new System.Drawing.Point(7, 248);
            CTValueCountLbl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CTValueCountLbl.Name = "CTValueCountLbl";
            CTValueCountLbl.Size = new System.Drawing.Size(117, 23);
            CTValueCountLbl.TabIndex = 45;
            CTValueCountLbl.Text = "Value Count: 0";
            // 
            // CTAmountTxt
            // 
            CTAmountTxt.Location = new System.Drawing.Point(391, 23);
            CTAmountTxt.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CTAmountTxt.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            CTAmountTxt.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            CTAmountTxt.Name = "CTAmountTxt";
            CTAmountTxt.Size = new System.Drawing.Size(65, 23);
            CTAmountTxt.TabIndex = 40;
            CTAmountTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            CTAmountTxt.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // CTRemoveBtn
            // 
            CTRemoveBtn.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            CTRemoveBtn.BackColor = System.Drawing.Color.FromArgb(243, 63, 63);
            CTRemoveBtn.Enabled = false;
            CTRemoveBtn.Location = new System.Drawing.Point(132, 278);
            CTRemoveBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CTRemoveBtn.Name = "CTRemoveBtn";
            CTRemoveBtn.Size = new System.Drawing.Size(104, 23);
            CTRemoveBtn.TabIndex = 38;
            CTRemoveBtn.Text = "Remove";
            // 
            // CTMoveDownBtn
            // 
            CTMoveDownBtn.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            CTMoveDownBtn.BackColor = System.Drawing.Color.FromArgb(243, 63, 63);
            CTMoveDownBtn.Enabled = false;
            CTMoveDownBtn.Location = new System.Drawing.Point(353, 278);
            CTMoveDownBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CTMoveDownBtn.Name = "CTMoveDownBtn";
            CTMoveDownBtn.Size = new System.Drawing.Size(104, 23);
            CTMoveDownBtn.TabIndex = 37;
            CTMoveDownBtn.Text = "Move Down";
            // 
            // CTMoveUpBtn
            // 
            CTMoveUpBtn.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            CTMoveUpBtn.BackColor = System.Drawing.Color.FromArgb(243, 63, 63);
            CTMoveUpBtn.Enabled = false;
            CTMoveUpBtn.Location = new System.Drawing.Point(242, 278);
            CTMoveUpBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CTMoveUpBtn.Name = "CTMoveUpBtn";
            CTMoveUpBtn.Size = new System.Drawing.Size(104, 23);
            CTMoveUpBtn.TabIndex = 36;
            CTMoveUpBtn.Text = "Move Up";
            // 
            // CTClearBtn
            // 
            CTClearBtn.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            CTClearBtn.BackColor = System.Drawing.Color.FromArgb(243, 63, 63);
            CTClearBtn.Location = new System.Drawing.Point(8, 53);
            CTClearBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CTClearBtn.Name = "CTClearBtn";
            CTClearBtn.Size = new System.Drawing.Size(85, 23);
            CTClearBtn.TabIndex = 35;
            CTClearBtn.Text = "Clear";
            // 
            // CTWriteBooleanBtn
            // 
            CTWriteBooleanBtn.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            CTWriteBooleanBtn.BackColor = System.Drawing.Color.FromArgb(243, 63, 63);
            CTWriteBooleanBtn.Location = new System.Drawing.Point(343, 53);
            CTWriteBooleanBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CTWriteBooleanBtn.Name = "CTWriteBooleanBtn";
            CTWriteBooleanBtn.Size = new System.Drawing.Size(114, 23);
            CTWriteBooleanBtn.TabIndex = 34;
            CTWriteBooleanBtn.Text = "Write Boolean";
            // 
            // CTWriteStringBtn
            // 
            CTWriteStringBtn.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            CTWriteStringBtn.BackColor = System.Drawing.Color.FromArgb(243, 63, 63);
            CTWriteStringBtn.Location = new System.Drawing.Point(221, 53);
            CTWriteStringBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CTWriteStringBtn.Name = "CTWriteStringBtn";
            CTWriteStringBtn.Size = new System.Drawing.Size(114, 23);
            CTWriteStringBtn.TabIndex = 33;
            CTWriteStringBtn.Text = "Write String";
            // 
            // CTWriteIntegerBtn
            // 
            CTWriteIntegerBtn.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            CTWriteIntegerBtn.BackColor = System.Drawing.Color.FromArgb(243, 63, 63);
            CTWriteIntegerBtn.Location = new System.Drawing.Point(100, 53);
            CTWriteIntegerBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CTWriteIntegerBtn.Name = "CTWriteIntegerBtn";
            CTWriteIntegerBtn.Size = new System.Drawing.Size(114, 23);
            CTWriteIntegerBtn.TabIndex = 32;
            CTWriteIntegerBtn.Text = "Write Integer";
            // 
            // CTValueLbl
            // 
            CTValueLbl.AutoSize = true;
            CTValueLbl.Location = new System.Drawing.Point(76, 3);
            CTValueLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            CTValueLbl.Name = "CTValueLbl";
            CTValueLbl.Size = new System.Drawing.Size(35, 15);
            CTValueLbl.TabIndex = 29;
            CTValueLbl.Text = "Value";
            // 
            // CTConstructerVw
            // 
            CTConstructerVw.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            CTConstructerVw.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { CTTypeCol, CTValueCol, CTEncodedCol });
            CTConstructerVw.FullRowSelect = true;
            CTConstructerVw.GridLines = true;
            CTConstructerVw.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            CTConstructerVw.Location = new System.Drawing.Point(7, 85);
            CTConstructerVw.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CTConstructerVw.MultiSelect = false;
            CTConstructerVw.Name = "CTConstructerVw";
            CTConstructerVw.ShowItemToolTips = true;
            CTConstructerVw.Size = new System.Drawing.Size(449, 159);
            CTConstructerVw.TabIndex = 44;
            CTConstructerVw.UseCompatibleStateImageBehavior = false;
            CTConstructerVw.View = System.Windows.Forms.View.Details;
            // 
            // CTTypeCol
            // 
            CTTypeCol.Text = "Type";
            CTTypeCol.Width = 66;
            // 
            // CTValueCol
            // 
            CTValueCol.Text = "Value";
            CTValueCol.Width = 149;
            // 
            // CTEncodedCol
            // 
            CTEncodedCol.Text = "Encoded";
            CTEncodedCol.Width = 149;
            // 
            // SchedulerTab
            // 
            SchedulerTab.Controls.Add(STAutoStartChckbx);
            SchedulerTab.Controls.Add(STDestinationTxt);
            SchedulerTab.Controls.Add(STPacketTxt);
            SchedulerTab.Controls.Add(STHotkeyTxt);
            SchedulerTab.Controls.Add(STRemoveBtn);
            SchedulerTab.Controls.Add(STDestinationLbl);
            SchedulerTab.Controls.Add(STCyclesLbl);
            SchedulerTab.Controls.Add(STCyclesTxt);
            SchedulerTab.Controls.Add(STIntervalLbl);
            SchedulerTab.Controls.Add(STClearBtn);
            SchedulerTab.Controls.Add(STCreateBtn);
            SchedulerTab.Controls.Add(STIntervalTxt);
            SchedulerTab.Controls.Add(STPacketLbl);
            SchedulerTab.Controls.Add(STSchedulerVw);
            SchedulerTab.Location = new System.Drawing.Point(4, 4);
            SchedulerTab.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            SchedulerTab.Name = "SchedulerTab";
            SchedulerTab.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            SchedulerTab.Size = new System.Drawing.Size(476, 313);
            SchedulerTab.TabIndex = 0;
            SchedulerTab.Text = "Scheduler";
            SchedulerTab.UseVisualStyleBackColor = true;
            // 
            // STAutoStartChckbx
            // 
            STAutoStartChckbx.AutoSize = true;
            STAutoStartChckbx.Checked = true;
            STAutoStartChckbx.CheckState = System.Windows.Forms.CheckState.Checked;
            STAutoStartChckbx.Location = new System.Drawing.Point(149, 228);
            STAutoStartChckbx.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            STAutoStartChckbx.Name = "STAutoStartChckbx";
            STAutoStartChckbx.Size = new System.Drawing.Size(79, 19);
            STAutoStartChckbx.TabIndex = 53;
            STAutoStartChckbx.Text = "Auto Start";
            STAutoStartChckbx.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            STAutoStartChckbx.UseVisualStyleBackColor = true;
            // 
            // STDestinationTxt
            // 
            STDestinationTxt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            STDestinationTxt.FormattingEnabled = true;
            STDestinationTxt.Location = new System.Drawing.Point(234, 247);
            STDestinationTxt.Margin = new System.Windows.Forms.Padding(4, 2, 4, 3);
            STDestinationTxt.Name = "STDestinationTxt";
            STDestinationTxt.Size = new System.Drawing.Size(76, 23);
            STDestinationTxt.TabIndex = 47;
            // 
            // STPacketTxt
            // 
            STPacketTxt.Location = new System.Drawing.Point(7, 248);
            STPacketTxt.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            STPacketTxt.MaxLength = int.MaxValue;
            STPacketTxt.Name = "STPacketTxt";
            STPacketTxt.Size = new System.Drawing.Size(220, 23);
            STPacketTxt.TabIndex = 45;
            // 
            // STHotkeyTxt
            // 
            STHotkeyTxt.BackColor = System.Drawing.Color.FromArgb(243, 63, 63);
            STHotkeyTxt.IsReadOnly = true;
            STHotkeyTxt.Location = new System.Drawing.Point(234, 278);
            STHotkeyTxt.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            STHotkeyTxt.Name = "STHotkeyTxt";
            STHotkeyTxt.Size = new System.Drawing.Size(222, 20);
            STHotkeyTxt.TabIndex = 63;
            STHotkeyTxt.Text = "";
            STHotkeyTxt.TextPaddingWidth = 0;
            STHotkeyTxt.Title = "Hotkey";
            STHotkeyTxt.Value = "";
            STHotkeyTxt.ValueAlign = System.Windows.Forms.HorizontalAlignment.Left;
            STHotkeyTxt.ValueReadOnly = true;
            // 
            // STRemoveBtn
            // 
            STRemoveBtn.BackColor = System.Drawing.Color.FromArgb(243, 63, 63);
            STRemoveBtn.Enabled = false;
            STRemoveBtn.Location = new System.Drawing.Point(83, 278);
            STRemoveBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            STRemoveBtn.Name = "STRemoveBtn";
            STRemoveBtn.Size = new System.Drawing.Size(69, 23);
            STRemoveBtn.TabIndex = 61;
            STRemoveBtn.Text = "Remove";
            // 
            // STDestinationLbl
            // 
            STDestinationLbl.AutoSize = true;
            STDestinationLbl.Location = new System.Drawing.Point(231, 230);
            STDestinationLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            STDestinationLbl.Name = "STDestinationLbl";
            STDestinationLbl.Size = new System.Drawing.Size(67, 15);
            STDestinationLbl.TabIndex = 48;
            STDestinationLbl.Text = "Destination";
            // 
            // STCyclesLbl
            // 
            STCyclesLbl.AutoSize = true;
            STCyclesLbl.Location = new System.Drawing.Point(387, 230);
            STCyclesLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            STCyclesLbl.Name = "STCyclesLbl";
            STCyclesLbl.Size = new System.Drawing.Size(41, 15);
            STCyclesLbl.TabIndex = 60;
            STCyclesLbl.Text = "Cycles";
            // 
            // STCyclesTxt
            // 
            STCyclesTxt.Location = new System.Drawing.Point(391, 248);
            STCyclesTxt.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            STCyclesTxt.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            STCyclesTxt.Name = "STCyclesTxt";
            STCyclesTxt.Size = new System.Drawing.Size(65, 23);
            STCyclesTxt.TabIndex = 59;
            STCyclesTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // STIntervalLbl
            // 
            STIntervalLbl.AutoSize = true;
            STIntervalLbl.Location = new System.Drawing.Point(315, 230);
            STIntervalLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            STIntervalLbl.Name = "STIntervalLbl";
            STIntervalLbl.Size = new System.Drawing.Size(46, 15);
            STIntervalLbl.TabIndex = 50;
            STIntervalLbl.Text = "Interval";
            // 
            // STClearBtn
            // 
            STClearBtn.BackColor = System.Drawing.Color.FromArgb(243, 63, 63);
            STClearBtn.Location = new System.Drawing.Point(159, 278);
            STClearBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            STClearBtn.Name = "STClearBtn";
            STClearBtn.Size = new System.Drawing.Size(69, 23);
            STClearBtn.TabIndex = 58;
            STClearBtn.Text = "Clear";
            // 
            // STCreateBtn
            // 
            STCreateBtn.BackColor = System.Drawing.Color.FromArgb(243, 63, 63);
            STCreateBtn.Location = new System.Drawing.Point(7, 278);
            STCreateBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            STCreateBtn.Name = "STCreateBtn";
            STCreateBtn.Size = new System.Drawing.Size(69, 23);
            STCreateBtn.TabIndex = 54;
            STCreateBtn.Text = "Create";
            // 
            // STIntervalTxt
            // 
            STIntervalTxt.Location = new System.Drawing.Point(318, 248);
            STIntervalTxt.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            STIntervalTxt.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            STIntervalTxt.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            STIntervalTxt.Name = "STIntervalTxt";
            STIntervalTxt.Size = new System.Drawing.Size(65, 23);
            STIntervalTxt.TabIndex = 49;
            STIntervalTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            STIntervalTxt.Value = new decimal(new int[] { 100, 0, 0, 0 });
            // 
            // STPacketLbl
            // 
            STPacketLbl.AutoSize = true;
            STPacketLbl.Location = new System.Drawing.Point(4, 230);
            STPacketLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            STPacketLbl.Name = "STPacketLbl";
            STPacketLbl.Size = new System.Drawing.Size(42, 15);
            STPacketLbl.TabIndex = 46;
            STPacketLbl.Text = "Packet";
            // 
            // STSchedulerVw
            // 
            STSchedulerVw.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            STSchedulerVw.CheckBoxes = true;
            STSchedulerVw.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { STPacketCol, STDestinationCol, STIntervalCol, STCyclesCol, STHotkeyCol });
            STSchedulerVw.FullRowSelect = true;
            STSchedulerVw.GridLines = true;
            STSchedulerVw.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            STSchedulerVw.Location = new System.Drawing.Point(7, 7);
            STSchedulerVw.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            STSchedulerVw.MultiSelect = false;
            STSchedulerVw.Name = "STSchedulerVw";
            STSchedulerVw.ShowItemToolTips = true;
            STSchedulerVw.Size = new System.Drawing.Size(449, 219);
            STSchedulerVw.TabIndex = 0;
            STSchedulerVw.UseCompatibleStateImageBehavior = false;
            STSchedulerVw.View = System.Windows.Forms.View.Details;
            // 
            // STPacketCol
            // 
            STPacketCol.Text = "Packet";
            STPacketCol.Width = 120;
            // 
            // STDestinationCol
            // 
            STDestinationCol.Text = "Destination";
            STDestinationCol.Width = 73;
            // 
            // STIntervalCol
            // 
            STIntervalCol.Text = "Interval";
            STIntervalCol.Width = 53;
            // 
            // STCyclesCol
            // 
            STCyclesCol.Text = "Cycles";
            STCyclesCol.Width = 53;
            // 
            // STHotkeyCol
            // 
            STHotkeyCol.Text = "Hotkey";
            STHotkeyCol.Width = 65;
            // 
            // PrimitiveTab
            // 
            PrimitiveTab.Controls.Add(PTSaveAsBtn);
            PrimitiveTab.Controls.Add(PTLengthTxt);
            PrimitiveTab.Controls.Add(PTHeaderTxt);
            PrimitiveTab.Controls.Add(PTCorruptedTxt);
            PrimitiveTab.Controls.Add(PTPacketTxt);
            PrimitiveTab.Location = new System.Drawing.Point(4, 4);
            PrimitiveTab.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            PrimitiveTab.Name = "PrimitiveTab";
            PrimitiveTab.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            PrimitiveTab.Size = new System.Drawing.Size(476, 313);
            PrimitiveTab.TabIndex = 1;
            PrimitiveTab.Text = "Primitive";
            PrimitiveTab.UseVisualStyleBackColor = true;
            // 
            // PTSaveAsBtn
            // 
            PTSaveAsBtn.BackColor = System.Drawing.Color.FromArgb(243, 63, 63);
            PTSaveAsBtn.Location = new System.Drawing.Point(355, 278);
            PTSaveAsBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            PTSaveAsBtn.Name = "PTSaveAsBtn";
            PTSaveAsBtn.Size = new System.Drawing.Size(102, 23);
            PTSaveAsBtn.TabIndex = 11;
            PTSaveAsBtn.Text = "Save As";
            // 
            // PTLengthTxt
            // 
            PTLengthTxt.BackColor = System.Drawing.Color.FromArgb(243, 63, 63);
            PTLengthTxt.IsReadOnly = true;
            PTLengthTxt.Location = new System.Drawing.Point(120, 278);
            PTLengthTxt.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            PTLengthTxt.Name = "PTLengthTxt";
            PTLengthTxt.Size = new System.Drawing.Size(106, 20);
            PTLengthTxt.TabIndex = 10;
            PTLengthTxt.Text = "0";
            PTLengthTxt.TextPaddingWidth = 0;
            PTLengthTxt.Title = "Length";
            PTLengthTxt.Value = "0";
            PTLengthTxt.ValueAlign = System.Windows.Forms.HorizontalAlignment.Left;
            PTLengthTxt.ValueReadOnly = true;
            // 
            // PTHeaderTxt
            // 
            PTHeaderTxt.BackColor = System.Drawing.Color.FromArgb(243, 63, 63);
            PTHeaderTxt.IsReadOnly = true;
            PTHeaderTxt.Location = new System.Drawing.Point(7, 278);
            PTHeaderTxt.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            PTHeaderTxt.Name = "PTHeaderTxt";
            PTHeaderTxt.Size = new System.Drawing.Size(106, 20);
            PTHeaderTxt.TabIndex = 9;
            PTHeaderTxt.Text = "0";
            PTHeaderTxt.TextPaddingWidth = 0;
            PTHeaderTxt.Title = "Header";
            PTHeaderTxt.Value = "0";
            PTHeaderTxt.ValueAlign = System.Windows.Forms.HorizontalAlignment.Left;
            PTHeaderTxt.ValueReadOnly = true;
            // 
            // PTCorruptedTxt
            // 
            PTCorruptedTxt.BackColor = System.Drawing.Color.Firebrick;
            PTCorruptedTxt.IsReadOnly = true;
            PTCorruptedTxt.Location = new System.Drawing.Point(233, 278);
            PTCorruptedTxt.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            PTCorruptedTxt.Name = "PTCorruptedTxt";
            PTCorruptedTxt.Size = new System.Drawing.Size(114, 20);
            PTCorruptedTxt.TabIndex = 8;
            PTCorruptedTxt.Text = "True";
            PTCorruptedTxt.TextPaddingWidth = 0;
            PTCorruptedTxt.Title = "Corrupted";
            PTCorruptedTxt.Value = "True";
            PTCorruptedTxt.ValueAlign = System.Windows.Forms.HorizontalAlignment.Left;
            PTCorruptedTxt.ValueReadOnly = true;
            // 
            // PTPacketTxt
            // 
            PTPacketTxt.Location = new System.Drawing.Point(7, 7);
            PTPacketTxt.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            PTPacketTxt.MaxLength = int.MaxValue;
            PTPacketTxt.Multiline = true;
            PTPacketTxt.Name = "PTPacketTxt";
            PTPacketTxt.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            PTPacketTxt.Size = new System.Drawing.Size(448, 264);
            PTPacketTxt.TabIndex = 3;
            // 
            // FiltersTab
            // 
            FiltersTab.Controls.Add(FTReplacementTxt);
            FiltersTab.Controls.Add(FTDestinationLbl);
            FiltersTab.Controls.Add(FTDestinationTxt);
            FiltersTab.Controls.Add(FTHeaderTxt);
            FiltersTab.Controls.Add(FTHeaderLbl);
            FiltersTab.Controls.Add(FTRemoveBtn);
            FiltersTab.Controls.Add(FTCreateBtn);
            FiltersTab.Controls.Add(FTActionLbl);
            FiltersTab.Controls.Add(FTActionTxt);
            FiltersTab.Controls.Add(FTFiltersVw);
            FiltersTab.Location = new System.Drawing.Point(4, 4);
            FiltersTab.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            FiltersTab.Name = "FiltersTab";
            FiltersTab.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            FiltersTab.Size = new System.Drawing.Size(476, 313);
            FiltersTab.TabIndex = 3;
            FiltersTab.Text = "Filters";
            FiltersTab.UseVisualStyleBackColor = true;
            // 
            // FTReplacementTxt
            // 
            FTReplacementTxt.BackColor = System.Drawing.Color.FromArgb(243, 63, 63);
            FTReplacementTxt.IsReadOnly = true;
            FTReplacementTxt.Location = new System.Drawing.Point(7, 278);
            FTReplacementTxt.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            FTReplacementTxt.Name = "FTReplacementTxt";
            FTReplacementTxt.Size = new System.Drawing.Size(449, 20);
            FTReplacementTxt.TabIndex = 54;
            FTReplacementTxt.Text = "";
            FTReplacementTxt.TextPaddingWidth = 0;
            FTReplacementTxt.Title = "Replacement";
            FTReplacementTxt.Value = "";
            FTReplacementTxt.ValueAlign = System.Windows.Forms.HorizontalAlignment.Left;
            FTReplacementTxt.ValueReadOnly = true;
            // 
            // FTDestinationLbl
            // 
            FTDestinationLbl.AutoSize = true;
            FTDestinationLbl.Location = new System.Drawing.Point(77, 228);
            FTDestinationLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            FTDestinationLbl.Name = "FTDestinationLbl";
            FTDestinationLbl.Size = new System.Drawing.Size(67, 15);
            FTDestinationLbl.TabIndex = 53;
            FTDestinationLbl.Text = "Destination";
            // 
            // FTDestinationTxt
            // 
            FTDestinationTxt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            FTDestinationTxt.FormattingEnabled = true;
            FTDestinationTxt.Location = new System.Drawing.Point(80, 247);
            FTDestinationTxt.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            FTDestinationTxt.Name = "FTDestinationTxt";
            FTDestinationTxt.Size = new System.Drawing.Size(76, 23);
            FTDestinationTxt.TabIndex = 52;
            // 
            // FTHeaderTxt
            // 
            FTHeaderTxt.Location = new System.Drawing.Point(7, 248);
            FTHeaderTxt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 3);
            FTHeaderTxt.Maximum = new decimal(new int[] { 4000, 0, 0, 0 });
            FTHeaderTxt.Name = "FTHeaderTxt";
            FTHeaderTxt.Size = new System.Drawing.Size(66, 23);
            FTHeaderTxt.TabIndex = 51;
            FTHeaderTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FTHeaderLbl
            // 
            FTHeaderLbl.AutoSize = true;
            FTHeaderLbl.Location = new System.Drawing.Point(4, 228);
            FTHeaderLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            FTHeaderLbl.Name = "FTHeaderLbl";
            FTHeaderLbl.Size = new System.Drawing.Size(45, 15);
            FTHeaderLbl.TabIndex = 50;
            FTHeaderLbl.Text = "Header";
            // 
            // FTRemoveBtn
            // 
            FTRemoveBtn.BackColor = System.Drawing.Color.FromArgb(243, 63, 63);
            FTRemoveBtn.Enabled = false;
            FTRemoveBtn.Location = new System.Drawing.Point(356, 247);
            FTRemoveBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            FTRemoveBtn.Name = "FTRemoveBtn";
            FTRemoveBtn.Size = new System.Drawing.Size(100, 23);
            FTRemoveBtn.TabIndex = 8;
            FTRemoveBtn.Text = "Remove";
            // 
            // FTCreateBtn
            // 
            FTCreateBtn.BackColor = System.Drawing.Color.FromArgb(243, 63, 63);
            FTCreateBtn.Location = new System.Drawing.Point(248, 248);
            FTCreateBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            FTCreateBtn.Name = "FTCreateBtn";
            FTCreateBtn.Size = new System.Drawing.Size(100, 23);
            FTCreateBtn.TabIndex = 7;
            FTCreateBtn.Text = "Create";
            // 
            // FTActionLbl
            // 
            FTActionLbl.AutoSize = true;
            FTActionLbl.Location = new System.Drawing.Point(161, 228);
            FTActionLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            FTActionLbl.Name = "FTActionLbl";
            FTActionLbl.Size = new System.Drawing.Size(42, 15);
            FTActionLbl.TabIndex = 2;
            FTActionLbl.Text = "Action";
            // 
            // FTActionTxt
            // 
            FTActionTxt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            FTActionTxt.FormattingEnabled = true;
            FTActionTxt.Items.AddRange(new object[] { "Block", "Replace", "Execute" });
            FTActionTxt.Location = new System.Drawing.Point(164, 247);
            FTActionTxt.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            FTActionTxt.Name = "FTActionTxt";
            FTActionTxt.Size = new System.Drawing.Size(76, 23);
            FTActionTxt.TabIndex = 1;
            // 
            // FTFiltersVw
            // 
            FTFiltersVw.CheckBoxes = true;
            FTFiltersVw.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { FTHeaderCol, FTDestinationCol, FTActionCol, FTReplacementCol });
            FTFiltersVw.FullRowSelect = true;
            FTFiltersVw.GridLines = true;
            FTFiltersVw.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            FTFiltersVw.Location = new System.Drawing.Point(7, 7);
            FTFiltersVw.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            FTFiltersVw.MultiSelect = false;
            FTFiltersVw.Name = "FTFiltersVw";
            FTFiltersVw.ShowItemToolTips = true;
            FTFiltersVw.Size = new System.Drawing.Size(448, 217);
            FTFiltersVw.TabIndex = 0;
            FTFiltersVw.UseCompatibleStateImageBehavior = false;
            FTFiltersVw.View = System.Windows.Forms.View.Details;
            // 
            // FTHeaderCol
            // 
            FTHeaderCol.Text = "Header";
            FTHeaderCol.Width = 55;
            // 
            // FTDestinationCol
            // 
            FTDestinationCol.Text = "Destination";
            FTDestinationCol.Width = 68;
            // 
            // FTActionCol
            // 
            FTActionCol.Text = "Action";
            FTActionCol.Width = 68;
            // 
            // FTReplacementCol
            // 
            FTReplacementCol.Text = "Replacement";
            FTReplacementCol.Width = 176;
            // 
            // ITPacketTxt
            // 
            ITPacketTxt.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            ITPacketTxt.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            ITPacketTxt.ItemHeight = 14;
            ITPacketTxt.Location = new System.Drawing.Point(7, 335);
            ITPacketTxt.Margin = new System.Windows.Forms.Padding(4, 6, 4, 3);
            ITPacketTxt.Name = "ITPacketTxt";
            ITPacketTxt.Size = new System.Drawing.Size(291, 20);
            ITPacketTxt.TabIndex = 8;
            // 
            // ToolboxTab
            // 
            ToolboxTab.Controls.Add(TT16BitInputLbl);
            ToolboxTab.Controls.Add(TT32BitInputLbl);
            ToolboxTab.Controls.Add(TTIntInputTxt);
            ToolboxTab.Controls.Add(TTIntOutputTxt);
            ToolboxTab.Controls.Add(TTUShortOutputTxt);
            ToolboxTab.Controls.Add(TTDecodeIntBtn);
            ToolboxTab.Controls.Add(TTDecodeUShortBtn);
            ToolboxTab.Controls.Add(TTUShortInputTxt);
            ToolboxTab.Location = new System.Drawing.Point(4, 28);
            ToolboxTab.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ToolboxTab.Name = "ToolboxTab";
            ToolboxTab.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ToolboxTab.Size = new System.Drawing.Size(557, 370);
            ToolboxTab.TabIndex = 3;
            ToolboxTab.Text = "Toolbox";
            ToolboxTab.UseVisualStyleBackColor = true;
            // 
            // TT16BitInputLbl
            // 
            TT16BitInputLbl.AutoSize = true;
            TT16BitInputLbl.Location = new System.Drawing.Point(7, 300);
            TT16BitInputLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            TT16BitInputLbl.Name = "TT16BitInputLbl";
            TT16BitInputLbl.Size = new System.Drawing.Size(112, 15);
            TT16BitInputLbl.TabIndex = 50;
            TT16BitInputLbl.Text = "16-Bit Integer Input:";
            // 
            // TT32BitInputLbl
            // 
            TT32BitInputLbl.AutoSize = true;
            TT32BitInputLbl.Location = new System.Drawing.Point(7, 332);
            TT32BitInputLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            TT32BitInputLbl.Name = "TT32BitInputLbl";
            TT32BitInputLbl.Size = new System.Drawing.Size(112, 15);
            TT32BitInputLbl.TabIndex = 0;
            TT32BitInputLbl.Text = "32-Bit Integer Input:";
            // 
            // TTIntInputTxt
            // 
            TTIntInputTxt.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            TTIntInputTxt.Location = new System.Drawing.Point(126, 329);
            TTIntInputTxt.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TTIntInputTxt.Maximum = new decimal(new int[] { int.MaxValue, 0, 0, 0 });
            TTIntInputTxt.Minimum = new decimal(new int[] { int.MinValue, 0, 0, int.MinValue });
            TTIntInputTxt.Name = "TTIntInputTxt";
            TTIntInputTxt.Size = new System.Drawing.Size(147, 22);
            TTIntInputTxt.TabIndex = 0;
            TTIntInputTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TTIntOutputTxt
            // 
            TTIntOutputTxt.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            TTIntOutputTxt.Location = new System.Drawing.Point(280, 329);
            TTIntOutputTxt.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TTIntOutputTxt.Name = "TTIntOutputTxt";
            TTIntOutputTxt.Size = new System.Drawing.Size(146, 22);
            TTIntOutputTxt.TabIndex = 37;
            TTIntOutputTxt.Text = "[0][0][0][0]";
            TTIntOutputTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TTUShortOutputTxt
            // 
            TTUShortOutputTxt.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            TTUShortOutputTxt.Location = new System.Drawing.Point(280, 297);
            TTUShortOutputTxt.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TTUShortOutputTxt.Name = "TTUShortOutputTxt";
            TTUShortOutputTxt.Size = new System.Drawing.Size(146, 22);
            TTUShortOutputTxt.TabIndex = 38;
            TTUShortOutputTxt.Text = "[0][0]";
            TTUShortOutputTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TTDecodeIntBtn
            // 
            TTDecodeIntBtn.BackColor = System.Drawing.Color.FromArgb(243, 63, 63);
            TTDecodeIntBtn.Location = new System.Drawing.Point(434, 331);
            TTDecodeIntBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TTDecodeIntBtn.Name = "TTDecodeIntBtn";
            TTDecodeIntBtn.Size = new System.Drawing.Size(114, 23);
            TTDecodeIntBtn.TabIndex = 34;
            TTDecodeIntBtn.Text = "Decode Int32";
            // 
            // TTDecodeUShortBtn
            // 
            TTDecodeUShortBtn.BackColor = System.Drawing.Color.FromArgb(243, 63, 63);
            TTDecodeUShortBtn.Location = new System.Drawing.Point(434, 299);
            TTDecodeUShortBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TTDecodeUShortBtn.Name = "TTDecodeUShortBtn";
            TTDecodeUShortBtn.Size = new System.Drawing.Size(114, 23);
            TTDecodeUShortBtn.TabIndex = 35;
            TTDecodeUShortBtn.Text = "Decode UInt16";
            // 
            // TTUShortInputTxt
            // 
            TTUShortInputTxt.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            TTUShortInputTxt.Location = new System.Drawing.Point(126, 297);
            TTUShortInputTxt.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TTUShortInputTxt.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            TTUShortInputTxt.Name = "TTUShortInputTxt";
            TTUShortInputTxt.Size = new System.Drawing.Size(147, 22);
            TTUShortInputTxt.TabIndex = 49;
            TTUShortInputTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ModulesTab
            // 
            ModulesTab.AllowDrop = true;
            ModulesTab.Controls.Add(MTSeperator1);
            ModulesTab.Controls.Add(sKoreLabelBox1);
            ModulesTab.Controls.Add(MTResourceBtn);
            ModulesTab.Controls.Add(MTHabboNameTxt);
            ModulesTab.Controls.Add(MTHabboNameLbl);
            ModulesTab.Controls.Add(MTAuthorsLbl);
            ModulesTab.Controls.Add(MTAuthorsTxt);
            ModulesTab.Controls.Add(MTUninstallModuleBtn);
            ModulesTab.Controls.Add(MTInstallModuleBtn);
            ModulesTab.Controls.Add(MTAuthorPctbx);
            ModulesTab.Controls.Add(MTModulesVw);
            ModulesTab.Location = new System.Drawing.Point(4, 28);
            ModulesTab.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ModulesTab.Name = "ModulesTab";
            ModulesTab.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ModulesTab.Size = new System.Drawing.Size(557, 370);
            ModulesTab.TabIndex = 1;
            ModulesTab.Text = "Modules";
            ModulesTab.UseVisualStyleBackColor = true;
            // 
            // MTSeperator1
            // 
            MTSeperator1.BackColor = System.Drawing.Color.FromArgb(243, 63, 63);
            MTSeperator1.Location = new System.Drawing.Point(90, 305);
            MTSeperator1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            MTSeperator1.Name = "MTSeperator1";
            MTSeperator1.Size = new System.Drawing.Size(458, 1);
            MTSeperator1.TabIndex = 17;
            // 
            // sKoreLabelBox1
            // 
            sKoreLabelBox1.BackColor = System.Drawing.Color.FromArgb(243, 63, 63);
            sKoreLabelBox1.IsReadOnly = true;
            sKoreLabelBox1.Location = new System.Drawing.Point(90, 309);
            sKoreLabelBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            sKoreLabelBox1.Name = "sKoreLabelBox1";
            sKoreLabelBox1.Size = new System.Drawing.Size(148, 20);
            sKoreLabelBox1.TabIndex = 16;
            sKoreLabelBox1.Text = "8055";
            sKoreLabelBox1.TextPaddingWidth = 0;
            sKoreLabelBox1.Title = "Install Port";
            sKoreLabelBox1.Value = "8055";
            sKoreLabelBox1.ValueAlign = System.Windows.Forms.HorizontalAlignment.Left;
            sKoreLabelBox1.ValueReadOnly = true;
            // 
            // MTResourceBtn
            // 
            MTResourceBtn.BackColor = System.Drawing.Color.FromArgb(243, 63, 63);
            MTResourceBtn.Enabled = false;
            MTResourceBtn.Location = new System.Drawing.Point(400, 278);
            MTResourceBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MTResourceBtn.Name = "MTResourceBtn";
            MTResourceBtn.Size = new System.Drawing.Size(148, 23);
            MTResourceBtn.TabIndex = 13;
            // 
            // MTHabboNameTxt
            // 
            MTHabboNameTxt.Location = new System.Drawing.Point(245, 278);
            MTHabboNameTxt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 3);
            MTHabboNameTxt.Name = "MTHabboNameTxt";
            MTHabboNameTxt.ReadOnly = true;
            MTHabboNameTxt.Size = new System.Drawing.Size(148, 23);
            MTHabboNameTxt.TabIndex = 9;
            MTHabboNameTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // MTHabboNameLbl
            // 
            MTHabboNameLbl.AutoSize = true;
            MTHabboNameLbl.Location = new System.Drawing.Point(241, 258);
            MTHabboNameLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            MTHabboNameLbl.Name = "MTHabboNameLbl";
            MTHabboNameLbl.Size = new System.Drawing.Size(78, 15);
            MTHabboNameLbl.TabIndex = 8;
            MTHabboNameLbl.Text = "Habbo Name";
            // 
            // MTAuthorsLbl
            // 
            MTAuthorsLbl.AutoSize = true;
            MTAuthorsLbl.Location = new System.Drawing.Point(86, 258);
            MTAuthorsLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            MTAuthorsLbl.Name = "MTAuthorsLbl";
            MTAuthorsLbl.Size = new System.Drawing.Size(57, 15);
            MTAuthorsLbl.TabIndex = 7;
            MTAuthorsLbl.Text = "Author(s)";
            // 
            // MTAuthorsTxt
            // 
            MTAuthorsTxt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            MTAuthorsTxt.Enabled = false;
            MTAuthorsTxt.FormattingEnabled = true;
            MTAuthorsTxt.Location = new System.Drawing.Point(90, 277);
            MTAuthorsTxt.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MTAuthorsTxt.Name = "MTAuthorsTxt";
            MTAuthorsTxt.Size = new System.Drawing.Size(148, 23);
            MTAuthorsTxt.TabIndex = 6;
            // 
            // MTUninstallModuleBtn
            // 
            MTUninstallModuleBtn.BackColor = System.Drawing.Color.FromArgb(243, 63, 63);
            MTUninstallModuleBtn.Enabled = false;
            MTUninstallModuleBtn.Location = new System.Drawing.Point(245, 309);
            MTUninstallModuleBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MTUninstallModuleBtn.Name = "MTUninstallModuleBtn";
            MTUninstallModuleBtn.Size = new System.Drawing.Size(148, 23);
            MTUninstallModuleBtn.TabIndex = 3;
            MTUninstallModuleBtn.Text = "Uninstall Module";
            // 
            // MTInstallModuleBtn
            // 
            MTInstallModuleBtn.BackColor = System.Drawing.Color.FromArgb(243, 63, 63);
            MTInstallModuleBtn.Location = new System.Drawing.Point(400, 309);
            MTInstallModuleBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MTInstallModuleBtn.Name = "MTInstallModuleBtn";
            MTInstallModuleBtn.Size = new System.Drawing.Size(148, 23);
            MTInstallModuleBtn.TabIndex = 2;
            MTInstallModuleBtn.Text = "Install Module";
            // 
            // MTAuthorPctbx
            // 
            MTAuthorPctbx.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            MTAuthorPctbx.BackColor = System.Drawing.Color.Transparent;
            MTAuthorPctbx.Enabled = false;
            MTAuthorPctbx.ErrorImage = null;
            MTAuthorPctbx.Image = (System.Drawing.Image)resources.GetObject("MTAuthorPctbx.Image");
            MTAuthorPctbx.InitialImage = null;
            MTAuthorPctbx.Location = new System.Drawing.Point(7, 240);
            MTAuthorPctbx.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            MTAuthorPctbx.Name = "MTAuthorPctbx";
            MTAuthorPctbx.Size = new System.Drawing.Size(76, 118);
            MTAuthorPctbx.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            MTAuthorPctbx.TabIndex = 1;
            MTAuthorPctbx.TabStop = false;
            // 
            // MTModulesVw
            // 
            MTModulesVw.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { MTNameCol, MTDescriptionCol, MTVersionCol, MTStateCol });
            MTModulesVw.FullRowSelect = true;
            MTModulesVw.GridLines = true;
            MTModulesVw.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            MTModulesVw.Location = new System.Drawing.Point(7, 7);
            MTModulesVw.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MTModulesVw.MultiSelect = false;
            MTModulesVw.Name = "MTModulesVw";
            MTModulesVw.ShowItemToolTips = true;
            MTModulesVw.Size = new System.Drawing.Size(541, 229);
            MTModulesVw.TabIndex = 0;
            MTModulesVw.UseCompatibleStateImageBehavior = false;
            MTModulesVw.View = System.Windows.Forms.View.Details;
            // 
            // MTNameCol
            // 
            MTNameCol.Text = "Name";
            MTNameCol.Width = 98;
            // 
            // MTDescriptionCol
            // 
            MTDescriptionCol.Text = "Description";
            MTDescriptionCol.Width = 215;
            // 
            // MTVersionCol
            // 
            MTVersionCol.Text = "Version";
            MTVersionCol.Width = 68;
            // 
            // MTStateCol
            // 
            MTStateCol.Text = "State";
            MTStateCol.Width = 68;
            // 
            // AboutTab
            // 
            AboutTab.Controls.Add(Sellout2Btn);
            AboutTab.Controls.Add(SelloutBtn);
            AboutTab.Controls.Add(HarbleDiscordBtn);
            AboutTab.Controls.Add(DonateBtn);
            AboutTab.Controls.Add(SpeaqerBtn);
            AboutTab.Controls.Add(SNGButton);
            AboutTab.Controls.Add(DarkboxBtn);
            AboutTab.Controls.Add(DarkStarBtn);
            AboutTab.Controls.Add(ArachisBtn);
            AboutTab.Location = new System.Drawing.Point(4, 28);
            AboutTab.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            AboutTab.Name = "AboutTab";
            AboutTab.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            AboutTab.Size = new System.Drawing.Size(557, 370);
            AboutTab.TabIndex = 5;
            AboutTab.Text = "About";
            AboutTab.UseVisualStyleBackColor = true;
            // 
            // Sellout2Btn
            // 
            Sellout2Btn.BackColor = System.Drawing.Color.FromArgb(243, 63, 63);
            Sellout2Btn.Location = new System.Drawing.Point(111, 243);
            Sellout2Btn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Sellout2Btn.Name = "Sellout2Btn";
            Sellout2Btn.Size = new System.Drawing.Size(334, 23);
            Sellout2Btn.TabIndex = 16;
            Sellout2Btn.Text = "HabboInterceptor | Niewiarowski's Packet Logger";
            // 
            // SelloutBtn
            // 
            SelloutBtn.BackColor = System.Drawing.Color.FromArgb(243, 63, 63);
            SelloutBtn.Location = new System.Drawing.Point(111, 213);
            SelloutBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            SelloutBtn.Name = "SelloutBtn";
            SelloutBtn.Size = new System.Drawing.Size(334, 23);
            SelloutBtn.TabIndex = 15;
            SelloutBtn.Text = "G-Earth | Cross-Platform Packet Logger by sirjonasxx";
            // 
            // HarbleDiscordBtn
            // 
            HarbleDiscordBtn.BackColor = System.Drawing.Color.FromArgb(114, 137, 218);
            HarbleDiscordBtn.Location = new System.Drawing.Point(148, 183);
            HarbleDiscordBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            HarbleDiscordBtn.Name = "HarbleDiscordBtn";
            HarbleDiscordBtn.Size = new System.Drawing.Size(260, 23);
            HarbleDiscordBtn.Skin = System.Drawing.Color.FromArgb(114, 137, 218);
            HarbleDiscordBtn.TabIndex = 14;
            HarbleDiscordBtn.Text = "Join our Discord! - Elbrah";
            // 
            // DonateBtn
            // 
            DonateBtn.BackColor = System.Drawing.Color.Green;
            DonateBtn.Location = new System.Drawing.Point(272, 93);
            DonateBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            DonateBtn.Name = "DonateBtn";
            DonateBtn.Size = new System.Drawing.Size(136, 23);
            DonateBtn.Skin = System.Drawing.Color.Green;
            DonateBtn.TabIndex = 13;
            DonateBtn.Text = "Donate";
            // 
            // SpeaqerBtn
            // 
            SpeaqerBtn.BackColor = System.Drawing.Color.FromArgb(50, 106, 218);
            SpeaqerBtn.Location = new System.Drawing.Point(148, 153);
            SpeaqerBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            SpeaqerBtn.Name = "SpeaqerBtn";
            SpeaqerBtn.Size = new System.Drawing.Size(117, 23);
            SpeaqerBtn.Skin = System.Drawing.Color.FromArgb(50, 106, 218);
            SpeaqerBtn.TabIndex = 8;
            SpeaqerBtn.Text = "@SpeaqerDev";
            // 
            // SNGButton
            // 
            SNGButton.BackColor = System.Drawing.Color.FromArgb(243, 63, 63);
            SNGButton.Location = new System.Drawing.Point(272, 153);
            SNGButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            SNGButton.Name = "SNGButton";
            SNGButton.Size = new System.Drawing.Size(136, 23);
            SNGButton.TabIndex = 6;
            SNGButton.Text = "SnGForum.info";
            // 
            // DarkboxBtn
            // 
            DarkboxBtn.BackColor = System.Drawing.Color.FromArgb(243, 63, 63);
            DarkboxBtn.Location = new System.Drawing.Point(272, 123);
            DarkboxBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            DarkboxBtn.Name = "DarkboxBtn";
            DarkboxBtn.Size = new System.Drawing.Size(136, 23);
            DarkboxBtn.TabIndex = 5;
            DarkboxBtn.Text = "Darkbox.nl";
            // 
            // DarkStarBtn
            // 
            DarkStarBtn.BackColor = System.Drawing.Color.FromArgb(50, 106, 218);
            DarkStarBtn.Location = new System.Drawing.Point(148, 123);
            DarkStarBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            DarkStarBtn.Name = "DarkStarBtn";
            DarkStarBtn.Size = new System.Drawing.Size(117, 23);
            DarkStarBtn.Skin = System.Drawing.Color.FromArgb(50, 106, 218);
            DarkStarBtn.TabIndex = 2;
            DarkStarBtn.Text = "@DarkStar851";
            // 
            // ArachisBtn
            // 
            ArachisBtn.BackColor = System.Drawing.Color.Purple;
            ArachisBtn.Location = new System.Drawing.Point(148, 93);
            ArachisBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ArachisBtn.Name = "ArachisBtn";
            ArachisBtn.Size = new System.Drawing.Size(117, 23);
            ArachisBtn.Skin = System.Drawing.Color.Purple;
            ArachisBtn.TabIndex = 1;
            ArachisBtn.Text = "@ArachisH";
            // 
            // InjectionMenu
            // 
            InjectionMenu.InputBox = null;
            InjectionMenu.Name = "InjectionMenu";
            InjectionMenu.Size = new System.Drawing.Size(174, 170);
            // 
            // SavePacketDlg
            // 
            SavePacketDlg.DefaultExt = "pkt";
            SavePacketDlg.Filter = "Packet (*.pkt)|*.pkt";
            SavePacketDlg.Title = "Tanji ~ Save Packet";
            // 
            // TanjiVersionTxt
            // 
            TanjiVersionTxt.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            TanjiVersionTxt.IsLink = true;
            TanjiVersionTxt.LinkColor = System.Drawing.SystemColors.HotTrack;
            TanjiVersionTxt.Name = "TanjiVersionTxt";
            TanjiVersionTxt.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            TanjiVersionTxt.Size = new System.Drawing.Size(51, 19);
            TanjiVersionTxt.Text = "v0.0.0";
            TanjiVersionTxt.VisitedLinkColor = System.Drawing.Color.FromArgb(243, 63, 63);
            // 
            // SchedulesTxt
            // 
            SchedulesTxt.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            SchedulesTxt.Name = "SchedulesTxt";
            SchedulesTxt.Size = new System.Drawing.Size(87, 19);
            SchedulesTxt.Text = "Schedules: 0/0";
            // 
            // FiltersTxt
            // 
            FiltersTxt.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            FiltersTxt.Name = "FiltersTxt";
            FiltersTxt.Size = new System.Drawing.Size(65, 19);
            FiltersTxt.Text = "Filters: 0/0";
            // 
            // ModulesTxt
            // 
            ModulesTxt.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            ModulesTxt.Name = "ModulesTxt";
            ModulesTxt.Size = new System.Drawing.Size(80, 19);
            ModulesTxt.Text = "Modules: 0/0";
            // 
            // TanjiInfoTxt
            // 
            TanjiInfoTxt.IsLink = true;
            TanjiInfoTxt.LinkColor = System.Drawing.SystemColors.HotTrack;
            TanjiInfoTxt.Name = "TanjiInfoTxt";
            TanjiInfoTxt.Size = new System.Drawing.Size(265, 19);
            TanjiInfoTxt.Spring = true;
            TanjiInfoTxt.Text = "Github - ArachisH/Tanji";
            TanjiInfoTxt.VisitedLinkColor = System.Drawing.Color.FromArgb(243, 63, 63);
            TanjiInfoTxt.Click += TanjiInfoTxt_Click;
            // 
            // TanjiStrip
            // 
            TanjiStrip.AllowMerge = false;
            TanjiStrip.BackColor = System.Drawing.Color.White;
            TanjiStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { TanjiVersionTxt, SchedulesTxt, FiltersTxt, ModulesTxt, TanjiInfoTxt });
            TanjiStrip.Location = new System.Drawing.Point(0, 402);
            TanjiStrip.Name = "TanjiStrip";
            TanjiStrip.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            TanjiStrip.Size = new System.Drawing.Size(565, 24);
            TanjiStrip.SizingGrip = false;
            TanjiStrip.TabIndex = 5;
            TanjiStrip.Text = "TanjiStrip";
            // 
            // MainFrm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            ClientSize = new System.Drawing.Size(565, 426);
            Controls.Add(TanjiTabs);
            Controls.Add(TanjiStrip);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximizeBox = false;
            Name = "MainFrm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Tanji ~ Disconnected";
            TopMost = true;
            FormClosed += MainFrm_FormClosed;
            Load += MainFrm_Load;
            TanjiTabs.ResumeLayout(false);
            ConnectionTab.ResumeLayout(false);
            InjectionTab.ResumeLayout(false);
            InjectionTabs.ResumeLayout(false);
            ConstructerTab.ResumeLayout(false);
            ConstructerTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)CTHeaderTxt).EndInit();
            ((System.ComponentModel.ISupportInitialize)CTAmountTxt).EndInit();
            SchedulerTab.ResumeLayout(false);
            SchedulerTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)STCyclesTxt).EndInit();
            ((System.ComponentModel.ISupportInitialize)STIntervalTxt).EndInit();
            PrimitiveTab.ResumeLayout(false);
            PrimitiveTab.PerformLayout();
            FiltersTab.ResumeLayout(false);
            FiltersTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)FTHeaderTxt).EndInit();
            ToolboxTab.ResumeLayout(false);
            ToolboxTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)TTIntInputTxt).EndInit();
            ((System.ComponentModel.ISupportInitialize)TTUShortInputTxt).EndInit();
            ModulesTab.ResumeLayout(false);
            ModulesTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)MTAuthorPctbx).EndInit();
            AboutTab.ResumeLayout(false);
            TanjiStrip.ResumeLayout(false);
            TanjiStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        public Tanji.Controls.SKoreInjectionMenu InjectionMenu;
        private System.Windows.Forms.TabPage AboutTab;
        private System.Windows.Forms.TabPage ModulesTab;
        private System.Windows.Forms.TabPage ToolboxTab;
        private System.Windows.Forms.Label TT16BitInputLbl;
        private System.Windows.Forms.Label TT32BitInputLbl;
        internal System.Windows.Forms.NumericUpDown TTIntInputTxt;
        internal System.Windows.Forms.TextBox TTIntOutputTxt;
        internal System.Windows.Forms.TextBox TTUShortOutputTxt;
        internal Tanji.Controls.TanjiButton TTDecodeIntBtn;
        internal Tanji.Controls.TanjiButton TTDecodeUShortBtn;
        internal System.Windows.Forms.NumericUpDown TTUShortInputTxt;
        private System.Windows.Forms.TabPage InjectionTab;
        internal Tanji.Controls.TanjiButton ITSendToClientBtn;
        internal Tanji.Controls.TanjiButton ITSendToServerBtn;
        internal Tanji.Controls.TanjiTabControl InjectionTabs;
        internal System.Windows.Forms.TabPage ConstructerTab;
        internal System.Windows.Forms.ComboBox CTValueTxt;
        internal System.Windows.Forms.NumericUpDown CTHeaderTxt;
        private System.Windows.Forms.Label CTHeaderLbl;
        private System.Windows.Forms.Label CTAmountLbl;
        internal System.Windows.Forms.TextBox CTStructureTxt;
        internal Tanji.Controls.TanjiButton CTTransferBelowBtn;
        internal Tanji.Controls.TanjiLabel CTValueCountLbl;
        internal System.Windows.Forms.NumericUpDown CTAmountTxt;
        internal Tanji.Controls.TanjiButton CTRemoveBtn;
        internal Tanji.Controls.TanjiButton CTMoveDownBtn;
        internal Tanji.Controls.TanjiButton CTMoveUpBtn;
        internal Tanji.Controls.TanjiButton CTClearBtn;
        internal Tanji.Controls.TanjiButton CTWriteBooleanBtn;
        internal Tanji.Controls.TanjiButton CTWriteStringBtn;
        internal Tanji.Controls.TanjiButton CTWriteIntegerBtn;
        private System.Windows.Forms.Label CTValueLbl;
        internal Tanji.Controls.SKoreConstructView CTConstructerVw;
        private System.Windows.Forms.ColumnHeader CTTypeCol;
        private System.Windows.Forms.ColumnHeader CTValueCol;
        private System.Windows.Forms.ColumnHeader CTEncodedCol;
        internal System.Windows.Forms.TabPage SchedulerTab;
        internal Tanji.Controls.TanjiButton STRemoveBtn;
        internal System.Windows.Forms.CheckBox STAutoStartChckbx;
        private System.Windows.Forms.Label STDestinationLbl;
        private System.Windows.Forms.Label STCyclesLbl;
        internal System.Windows.Forms.NumericUpDown STCyclesTxt;
        private System.Windows.Forms.Label STIntervalLbl;
        internal Tanji.Controls.TanjiButton STClearBtn;
        internal Tanji.Controls.TanjiButton STCreateBtn;
        internal System.Windows.Forms.NumericUpDown STIntervalTxt;
        internal System.Windows.Forms.ComboBox STDestinationTxt;
        private System.Windows.Forms.Label STPacketLbl;
        internal System.Windows.Forms.TextBox STPacketTxt;
        internal Tanji.Controls.SKoreScheduleView STSchedulerVw;
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
        internal Tanji.Controls.TanjiButton FTRemoveBtn;
        internal Tanji.Controls.TanjiButton FTCreateBtn;
        private System.Windows.Forms.Label FTActionLbl;
        internal System.Windows.Forms.ComboBox FTActionTxt;
        internal Tanji.Controls.TanjiListView FTFiltersVw;
        private System.Windows.Forms.ColumnHeader FTHeaderCol;
        private System.Windows.Forms.ColumnHeader FTDestinationCol;
        private System.Windows.Forms.ColumnHeader FTActionCol;
        private System.Windows.Forms.ColumnHeader FTReplacementCol;
        internal System.Windows.Forms.ComboBox ITPacketTxt;
        private System.Windows.Forms.TabPage ConnectionTab;
        internal Tanji.Controls.TanjiButton CoTBrowseBtn;
        internal Tanji.Controls.TanjiButton CoTExportCertificateAuthorityBtn;
        internal Tanji.Controls.TanjiButton CoTDestroyCertificatesBtn;
        internal Tanji.Controls.TanjiButton CoTResetBtn;
        internal Tanji.Controls.TanjiButton CoTUpdateBtn;
        internal Tanji.Controls.TanjiLabel CoTStatusTxt;
        internal Tanji.Controls.TanjiButton CoTConnectBtn;
        internal Tanji.Controls.TanjiListView CoTVariablesVw;
        private System.Windows.Forms.ColumnHeader CoTVariableCol;
        private System.Windows.Forms.ColumnHeader CoTValueCol;
        internal Tanji.Controls.TanjiTabControl TanjiTabs;
        internal System.Windows.Forms.OpenFileDialog CustomClientDlg;
        internal System.Windows.Forms.OpenFileDialog InstallModuleDlg;
        internal Tanji.Controls.TanjiListView MTModulesVw;
        private System.Windows.Forms.ColumnHeader MTNameCol;
        private System.Windows.Forms.ColumnHeader MTVersionCol;
        internal System.Windows.Forms.PictureBox MTAuthorPctbx;
        internal Tanji.Controls.TanjiButton MTUninstallModuleBtn;
        internal Tanji.Controls.TanjiButton MTInstallModuleBtn;
        private System.Windows.Forms.ColumnHeader MTDescriptionCol;
        private System.Windows.Forms.ColumnHeader MTStateCol;
        private System.Windows.Forms.Label MTAuthorsLbl;
        internal System.Windows.Forms.ComboBox MTAuthorsTxt;
        internal Tanji.Controls.TanjiButton MTResourceBtn;
        internal System.Windows.Forms.TextBox MTHabboNameTxt;
        private System.Windows.Forms.ColumnHeader STHotkeyCol;
        internal Tanji.Controls.TanjiLabelBox CoTCustomClientTxt;
        internal Tanji.Controls.TanjiLabelBox CoTVariableTxt;
        internal Tanji.Controls.TanjiLabelBox CoTValueTxt;
        internal Tanji.Controls.TanjiLabelBox PTCorruptedTxt;
        internal Tanji.Controls.TanjiLabelBox PTLengthTxt;
        internal Tanji.Controls.TanjiLabelBox PTHeaderTxt;
        internal Tanji.Controls.TanjiButton PTSaveAsBtn;
        internal System.Windows.Forms.SaveFileDialog SavePacketDlg;
        internal Tanji.Controls.TanjiLabelBox FTReplacementTxt;
        internal Tanji.Controls.TanjiLabelBox STHotkeyTxt;
        private Tanji.Controls.TanjiLabelBox sKoreLabelBox1;
        internal System.Windows.Forms.Label MTHabboNameLbl;
        internal Tanji.Controls.TanjiButton DarkStarBtn;
        internal Tanji.Controls.TanjiButton ArachisBtn;
        internal Tanji.Controls.TanjiButton SpeaqerBtn;
        internal Tanji.Controls.TanjiButton SNGButton;
        internal Tanji.Controls.TanjiButton DarkboxBtn;
        internal Tanji.Controls.TanjiButton DonateBtn;
        internal System.Windows.Forms.ToolStripStatusLabel TanjiVersionTxt;
        internal System.Windows.Forms.ToolStripStatusLabel SchedulesTxt;
        internal System.Windows.Forms.ToolStripStatusLabel FiltersTxt;
        internal System.Windows.Forms.ToolStripStatusLabel ModulesTxt;
        private System.Windows.Forms.ToolStripStatusLabel TanjiInfoTxt;
        private System.Windows.Forms.StatusStrip TanjiStrip;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        internal Tanji.Controls.TanjiButton HarbleDiscordBtn;
        private System.Windows.Forms.Label MTSeperator1;
        internal Tanji.Controls.TanjiLabel CoTProxyPortLbl;
        internal Tanji.Controls.TanjiButton SelloutBtn;
        internal Tanji.Controls.TanjiButton Sellout2Btn;
    }
}