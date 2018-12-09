namespace Tanji.Windows
{
    partial class PacketLoggerFrm
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
            this.PacketLoggerMs = new System.Windows.Forms.MenuStrip();
            this.FileBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.FindBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.FindMessageBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.FileSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.EmptyLogBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.DisplayFiltersBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.BlockedBtn = new Tanji.Controls.BindableToolStripMenuItem();
            this.ReplacedBtn = new Tanji.Controls.BindableToolStripMenuItem();
            this.DisplayDetailsBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.DismantledBtn = new Tanji.Controls.BindableToolStripMenuItem();
            this.HashNameBtn = new Tanji.Controls.BindableToolStripMenuItem();
            this.HexadecimalBtn = new Tanji.Controls.BindableToolStripMenuItem();
            this.ParserNameBtn = new Tanji.Controls.BindableToolStripMenuItem();
            this.ClassNameBtn = new Tanji.Controls.BindableToolStripMenuItem();
            this.ViewSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.ViewOutgoingBtn = new Tanji.Controls.BindableToolStripMenuItem();
            this.ViewIncomingBtn = new Tanji.Controls.BindableToolStripMenuItem();
            this.ViewSep2 = new System.Windows.Forms.ToolStripSeparator();
            this.AutoScrollingBtn = new Tanji.Controls.BindableToolStripMenuItem();
            this.AlwaysOnTopBtn = new Tanji.Controls.BindableToolStripMenuItem();
            this.ToolsBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.IgnoreMessagesBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.LogTxt = new Tanji.Controls.RichLogBox();
            this.ViewOutgoingLbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.ViewIncomingLbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.LatencyLbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.RevisionLbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.PacketLoggerSs = new System.Windows.Forms.StatusStrip();
            this.PacketLoggerMs.SuspendLayout();
            this.PacketLoggerSs.SuspendLayout();
            this.SuspendLayout();
            // 
            // PacketLoggerMs
            // 
            this.PacketLoggerMs.BackColor = System.Drawing.SystemColors.Control;
            this.PacketLoggerMs.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileBtn,
            this.ViewBtn,
            this.ToolsBtn});
            this.PacketLoggerMs.Location = new System.Drawing.Point(0, 0);
            this.PacketLoggerMs.Name = "PacketLoggerMs";
            this.PacketLoggerMs.Size = new System.Drawing.Size(834, 24);
            this.PacketLoggerMs.TabIndex = 0;
            // 
            // FileBtn
            // 
            this.FileBtn.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FindBtn,
            this.FindMessageBtn,
            this.FileSep1,
            this.EmptyLogBtn});
            this.FileBtn.Name = "FileBtn";
            this.FileBtn.Size = new System.Drawing.Size(37, 20);
            this.FileBtn.Text = "File";
            // 
            // FindBtn
            // 
            this.FindBtn.Name = "FindBtn";
            this.FindBtn.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.FindBtn.Size = new System.Drawing.Size(191, 22);
            this.FindBtn.Text = "Find";
            this.FindBtn.Click += new System.EventHandler(this.FindBtn_Click);
            // 
            // FindMessageBtn
            // 
            this.FindMessageBtn.Name = "FindMessageBtn";
            this.FindMessageBtn.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.FindMessageBtn.Size = new System.Drawing.Size(191, 22);
            this.FindMessageBtn.Text = "Find Message";
            // 
            // FileSep1
            // 
            this.FileSep1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FileSep1.Name = "FileSep1";
            this.FileSep1.Size = new System.Drawing.Size(188, 6);
            // 
            // EmptyLogBtn
            // 
            this.EmptyLogBtn.Name = "EmptyLogBtn";
            this.EmptyLogBtn.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.EmptyLogBtn.Size = new System.Drawing.Size(191, 22);
            this.EmptyLogBtn.Text = "Empty Log";
            this.EmptyLogBtn.Click += new System.EventHandler(this.EmptyLogBtn_Click);
            // 
            // ViewBtn
            // 
            this.ViewBtn.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DisplayFiltersBtn,
            this.DisplayDetailsBtn,
            this.ViewSep1,
            this.ViewOutgoingBtn,
            this.ViewIncomingBtn,
            this.ViewSep2,
            this.AutoScrollingBtn,
            this.AlwaysOnTopBtn});
            this.ViewBtn.Name = "ViewBtn";
            this.ViewBtn.Size = new System.Drawing.Size(44, 20);
            this.ViewBtn.Text = "View";
            // 
            // DisplayFiltersBtn
            // 
            this.DisplayFiltersBtn.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BlockedBtn,
            this.ReplacedBtn});
            this.DisplayFiltersBtn.Name = "DisplayFiltersBtn";
            this.DisplayFiltersBtn.Size = new System.Drawing.Size(196, 22);
            this.DisplayFiltersBtn.Text = "Display Filters";
            // 
            // BlockedBtn
            // 
            this.BlockedBtn.Checked = true;
            this.BlockedBtn.CheckOnClick = true;
            this.BlockedBtn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.BlockedBtn.Name = "BlockedBtn";
            this.BlockedBtn.Size = new System.Drawing.Size(122, 22);
            this.BlockedBtn.Text = "Blocked";
            // 
            // ReplacedBtn
            // 
            this.ReplacedBtn.Checked = true;
            this.ReplacedBtn.CheckOnClick = true;
            this.ReplacedBtn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ReplacedBtn.Name = "ReplacedBtn";
            this.ReplacedBtn.Size = new System.Drawing.Size(122, 22);
            this.ReplacedBtn.Text = "Replaced";
            // 
            // DisplayDetailsBtn
            // 
            this.DisplayDetailsBtn.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DismantledBtn,
            this.HashNameBtn,
            this.HexadecimalBtn,
            this.ParserNameBtn,
            this.ClassNameBtn});
            this.DisplayDetailsBtn.Name = "DisplayDetailsBtn";
            this.DisplayDetailsBtn.Size = new System.Drawing.Size(196, 22);
            this.DisplayDetailsBtn.Text = "Display Details";
            // 
            // DismantledBtn
            // 
            this.DismantledBtn.Checked = true;
            this.DismantledBtn.CheckOnClick = true;
            this.DismantledBtn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DismantledBtn.Name = "DismantledBtn";
            this.DismantledBtn.Size = new System.Drawing.Size(142, 22);
            this.DismantledBtn.Text = "Dismantled";
            // 
            // HashNameBtn
            // 
            this.HashNameBtn.Checked = true;
            this.HashNameBtn.CheckOnClick = true;
            this.HashNameBtn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.HashNameBtn.Name = "HashNameBtn";
            this.HashNameBtn.Size = new System.Drawing.Size(142, 22);
            this.HashNameBtn.Text = "Hash/Name";
            // 
            // HexadecimalBtn
            // 
            this.HexadecimalBtn.CheckOnClick = true;
            this.HexadecimalBtn.Name = "HexadecimalBtn";
            this.HexadecimalBtn.Size = new System.Drawing.Size(142, 22);
            this.HexadecimalBtn.Text = "Hexadecimal";
            // 
            // ParserNameBtn
            // 
            this.ParserNameBtn.Checked = true;
            this.ParserNameBtn.CheckOnClick = true;
            this.ParserNameBtn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ParserNameBtn.Name = "ParserNameBtn";
            this.ParserNameBtn.Size = new System.Drawing.Size(142, 22);
            this.ParserNameBtn.Text = "Parser Name";
            // 
            // ClassNameBtn
            // 
            this.ClassNameBtn.Checked = true;
            this.ClassNameBtn.CheckOnClick = true;
            this.ClassNameBtn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ClassNameBtn.Name = "ClassNameBtn";
            this.ClassNameBtn.Size = new System.Drawing.Size(142, 22);
            this.ClassNameBtn.Text = "Class Name";
            // 
            // ViewSep1
            // 
            this.ViewSep1.Name = "ViewSep1";
            this.ViewSep1.Size = new System.Drawing.Size(193, 6);
            // 
            // ViewOutgoingBtn
            // 
            this.ViewOutgoingBtn.Checked = true;
            this.ViewOutgoingBtn.CheckOnClick = true;
            this.ViewOutgoingBtn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ViewOutgoingBtn.Name = "ViewOutgoingBtn";
            this.ViewOutgoingBtn.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.ViewOutgoingBtn.Size = new System.Drawing.Size(196, 22);
            this.ViewOutgoingBtn.Text = "View Outgoing";
            // 
            // ViewIncomingBtn
            // 
            this.ViewIncomingBtn.Checked = true;
            this.ViewIncomingBtn.CheckOnClick = true;
            this.ViewIncomingBtn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ViewIncomingBtn.Name = "ViewIncomingBtn";
            this.ViewIncomingBtn.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.ViewIncomingBtn.Size = new System.Drawing.Size(196, 22);
            this.ViewIncomingBtn.Text = "View Incoming";
            // 
            // ViewSep2
            // 
            this.ViewSep2.Name = "ViewSep2";
            this.ViewSep2.Size = new System.Drawing.Size(193, 6);
            // 
            // AutoScrollingBtn
            // 
            this.AutoScrollingBtn.Checked = true;
            this.AutoScrollingBtn.CheckOnClick = true;
            this.AutoScrollingBtn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AutoScrollingBtn.Name = "AutoScrollingBtn";
            this.AutoScrollingBtn.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.AutoScrollingBtn.Size = new System.Drawing.Size(196, 22);
            this.AutoScrollingBtn.Text = "Auto-Scrolling";
            // 
            // AlwaysOnTopBtn
            // 
            this.AlwaysOnTopBtn.CheckOnClick = true;
            this.AlwaysOnTopBtn.Name = "AlwaysOnTopBtn";
            this.AlwaysOnTopBtn.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
            this.AlwaysOnTopBtn.Size = new System.Drawing.Size(196, 22);
            this.AlwaysOnTopBtn.Text = "Always On Top";
            // 
            // ToolsBtn
            // 
            this.ToolsBtn.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.IgnoreMessagesBtn});
            this.ToolsBtn.Name = "ToolsBtn";
            this.ToolsBtn.Size = new System.Drawing.Size(47, 20);
            this.ToolsBtn.Text = "Tools";
            // 
            // IgnoreMessagesBtn
            // 
            this.IgnoreMessagesBtn.Name = "IgnoreMessagesBtn";
            this.IgnoreMessagesBtn.Size = new System.Drawing.Size(162, 22);
            this.IgnoreMessagesBtn.Text = "Ignore Messages";
            // 
            // LogTxt
            // 
            this.LogTxt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.LogTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.LogTxt.DetectUrls = false;
            this.LogTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LogTxt.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LogTxt.HideSelection = false;
            this.LogTxt.Location = new System.Drawing.Point(0, 24);
            this.LogTxt.Name = "LogTxt";
            this.LogTxt.ReadOnly = true;
            this.LogTxt.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.LogTxt.ShowSelectionMargin = true;
            this.LogTxt.Size = new System.Drawing.Size(834, 463);
            this.LogTxt.TabIndex = 1;
            this.LogTxt.TabStop = false;
            this.LogTxt.Text = "";
            // 
            // ViewOutgoingLbl
            // 
            this.ViewOutgoingLbl.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.ViewOutgoingLbl.BorderStyle = System.Windows.Forms.Border3DStyle.RaisedOuter;
            this.ViewOutgoingLbl.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ViewOutgoingLbl.ForeColor = System.Drawing.Color.Black;
            this.ViewOutgoingLbl.Name = "ViewOutgoingLbl";
            this.ViewOutgoingLbl.Size = new System.Drawing.Size(119, 19);
            this.ViewOutgoingLbl.Text = "View Outgoing: True";
            // 
            // ViewIncomingLbl
            // 
            this.ViewIncomingLbl.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.ViewIncomingLbl.BorderStyle = System.Windows.Forms.Border3DStyle.RaisedOuter;
            this.ViewIncomingLbl.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ViewIncomingLbl.ForeColor = System.Drawing.Color.Black;
            this.ViewIncomingLbl.Name = "ViewIncomingLbl";
            this.ViewIncomingLbl.Size = new System.Drawing.Size(119, 19);
            this.ViewIncomingLbl.Text = "View Incoming: True";
            // 
            // LatencyLbl
            // 
            this.LatencyLbl.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.LatencyLbl.BorderStyle = System.Windows.Forms.Border3DStyle.RaisedOuter;
            this.LatencyLbl.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.LatencyLbl.ForeColor = System.Drawing.Color.Black;
            this.LatencyLbl.Name = "LatencyLbl";
            this.LatencyLbl.Size = new System.Drawing.Size(80, 19);
            this.LatencyLbl.Text = "Latency: 0ms";
            // 
            // RevisionLbl
            // 
            this.RevisionLbl.BorderStyle = System.Windows.Forms.Border3DStyle.RaisedOuter;
            this.RevisionLbl.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.RevisionLbl.ForeColor = System.Drawing.Color.Black;
            this.RevisionLbl.Name = "RevisionLbl";
            this.RevisionLbl.Size = new System.Drawing.Size(268, 19);
            this.RevisionLbl.Text = "Revision: PRODUCTION-000000000000-000000000";
            // 
            // PacketLoggerSs
            // 
            this.PacketLoggerSs.BackColor = System.Drawing.SystemColors.Control;
            this.PacketLoggerSs.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ViewOutgoingLbl,
            this.ViewIncomingLbl,
            this.LatencyLbl,
            this.RevisionLbl});
            this.PacketLoggerSs.Location = new System.Drawing.Point(0, 487);
            this.PacketLoggerSs.Name = "PacketLoggerSs";
            this.PacketLoggerSs.Size = new System.Drawing.Size(834, 24);
            this.PacketLoggerSs.SizingGrip = false;
            this.PacketLoggerSs.TabIndex = 2;
            // 
            // PacketLoggerFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 511);
            this.Controls.Add(this.LogTxt);
            this.Controls.Add(this.PacketLoggerSs);
            this.Controls.Add(this.PacketLoggerMs);
            this.MainMenuStrip = this.PacketLoggerMs;
            this.Name = "PacketLoggerFrm";
            this.Text = "Tanji - Packet Logger";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PacketLoggerFrm_FormClosing);
            this.PacketLoggerMs.ResumeLayout(false);
            this.PacketLoggerMs.PerformLayout();
            this.PacketLoggerSs.ResumeLayout(false);
            this.PacketLoggerSs.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip PacketLoggerMs;
        private System.Windows.Forms.ToolStripMenuItem FileBtn;
        private System.Windows.Forms.ToolStripMenuItem ViewBtn;
        private System.Windows.Forms.ToolStripMenuItem ToolsBtn;
        private System.Windows.Forms.ToolStripMenuItem FindBtn;
        private System.Windows.Forms.ToolStripMenuItem FindMessageBtn;
        private System.Windows.Forms.ToolStripSeparator FileSep1;
        private System.Windows.Forms.ToolStripMenuItem EmptyLogBtn;
        private System.Windows.Forms.ToolStripMenuItem DisplayFiltersBtn;
        private Controls.BindableToolStripMenuItem BlockedBtn;
        private Controls.BindableToolStripMenuItem ReplacedBtn;
        private System.Windows.Forms.ToolStripMenuItem DisplayDetailsBtn;
        private Controls.BindableToolStripMenuItem HashNameBtn;
        private Controls.BindableToolStripMenuItem DismantledBtn;
        private Controls.BindableToolStripMenuItem HexadecimalBtn;
        private Controls.BindableToolStripMenuItem ParserNameBtn;
        private Controls.BindableToolStripMenuItem ClassNameBtn;
        private System.Windows.Forms.ToolStripSeparator ViewSep1;
        private Controls.BindableToolStripMenuItem ViewOutgoingBtn;
        private Controls.BindableToolStripMenuItem ViewIncomingBtn;
        private System.Windows.Forms.ToolStripSeparator ViewSep2;
        private Controls.BindableToolStripMenuItem AlwaysOnTopBtn;
        private System.Windows.Forms.ToolStripMenuItem IgnoreMessagesBtn;
        private Controls.RichLogBox LogTxt;
        private System.Windows.Forms.ToolStripStatusLabel ViewOutgoingLbl;
        private System.Windows.Forms.ToolStripStatusLabel ViewIncomingLbl;
        private System.Windows.Forms.ToolStripStatusLabel LatencyLbl;
        private System.Windows.Forms.ToolStripStatusLabel RevisionLbl;
        private System.Windows.Forms.StatusStrip PacketLoggerSs;
        private Controls.BindableToolStripMenuItem AutoScrollingBtn;
    }
}
