using Tanji.Properties;

namespace Tanji.Applications
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
            this.components = new System.ComponentModel.Container();
            this.LoggerTxt = new System.Windows.Forms.RichTextBox();
            this.PacketLoggerContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.PLCMCopyBtn = new Tanji.Controls.BindableToolStripMenuItem();
            this.PacketLoggerMenu = new System.Windows.Forms.MenuStrip();
            this.FileBtn = new Tanji.Controls.BindableToolStripMenuItem();
            this.FindBtn = new Tanji.Controls.BindableToolStripMenuItem();
            this.FindMessageBtn = new Tanji.Controls.BindableToolStripMenuItem();
            this.FileSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.EmptyLogBtn = new Tanji.Controls.BindableToolStripMenuItem();
            this.ViewBtn = new Tanji.Controls.BindableToolStripMenuItem();
            this.DisplayFiltersBtn = new Tanji.Controls.BindableToolStripMenuItem();
            this.BlockedBtn = new Tanji.Controls.BindableToolStripMenuItem();
            this.ReplacedBtn = new Tanji.Controls.BindableToolStripMenuItem();
            this.DisplaySpecialsBtn = new Tanji.Controls.BindableToolStripMenuItem();
            this.HashBtn = new Tanji.Controls.BindableToolStripMenuItem();
            this.StructureBtn = new Tanji.Controls.BindableToolStripMenuItem();
            this.HexadecimalBtn = new Tanji.Controls.BindableToolStripMenuItem();
            this.ParserNameBtn = new Tanji.Controls.BindableToolStripMenuItem();
            this.MessageName = new Tanji.Controls.BindableToolStripMenuItem();
            this.ViewSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.ViewOutgoingBtn = new Tanji.Controls.BindableToolStripMenuItem();
            this.ViewIncomingBtn = new Tanji.Controls.BindableToolStripMenuItem();
            this.ViewSep2 = new System.Windows.Forms.ToolStripSeparator();
            this.AlwaysOnTopBtn = new Tanji.Controls.BindableToolStripMenuItem();
            this.ToolsBtn = new Tanji.Controls.BindableToolStripMenuItem();
            this.IgnoreMessagesBtn = new Tanji.Controls.BindableToolStripMenuItem();
            this.PacketLoggerStrip = new System.Windows.Forms.StatusStrip();
            this.ViewOutgoingLbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.ViewIncomingLbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.RevisionTxt = new System.Windows.Forms.ToolStripStatusLabel();
            this.PacketLoggerContextMenu.SuspendLayout();
            this.PacketLoggerMenu.SuspendLayout();
            this.PacketLoggerStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // LoggerTxt
            // 
            this.LoggerTxt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(10)))), ((int)(((byte)(10)))));
            this.LoggerTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.LoggerTxt.ContextMenuStrip = this.PacketLoggerContextMenu;
            this.LoggerTxt.DetectUrls = false;
            this.LoggerTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LoggerTxt.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LoggerTxt.ForeColor = System.Drawing.Color.White;
            this.LoggerTxt.HideSelection = false;
            this.LoggerTxt.Location = new System.Drawing.Point(0, 24);
            this.LoggerTxt.Name = "LoggerTxt";
            this.LoggerTxt.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.LoggerTxt.ShowSelectionMargin = true;
            this.LoggerTxt.Size = new System.Drawing.Size(710, 473);
            this.LoggerTxt.TabIndex = 0;
            this.LoggerTxt.Text = "";
            // 
            // PacketLoggerContextMenu
            // 
            this.PacketLoggerContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.PLCMCopyBtn});
            this.PacketLoggerContextMenu.Name = "ConstructMenu";
            this.PacketLoggerContextMenu.Size = new System.Drawing.Size(145, 26);
            // 
            // PLCMCopyBtn
            // 
            this.PLCMCopyBtn.Name = "PLCMCopyBtn";
            this.PLCMCopyBtn.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.PLCMCopyBtn.Size = new System.Drawing.Size(144, 22);
            this.PLCMCopyBtn.Text = "Copy";
            this.PLCMCopyBtn.Click += new System.EventHandler(this.CopyBtn_Click);
            // 
            // PacketLoggerMenu
            // 
            this.PacketLoggerMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileBtn,
            this.ViewBtn,
            this.ToolsBtn});
            this.PacketLoggerMenu.Location = new System.Drawing.Point(0, 0);
            this.PacketLoggerMenu.Name = "PacketLoggerMenu";
            this.PacketLoggerMenu.Size = new System.Drawing.Size(710, 24);
            this.PacketLoggerMenu.TabIndex = 7;
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
            this.FindMessageBtn.Click += new System.EventHandler(this.FindMessageBtn_Click);
            // 
            // FileSep1
            // 
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
            this.DisplaySpecialsBtn,
            this.ViewSep1,
            this.ViewOutgoingBtn,
            this.ViewIncomingBtn,
            this.ViewSep2,
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
            this.BlockedBtn.Size = new System.Drawing.Size(152, 22);
            this.BlockedBtn.Text = "Blocked";
            // 
            // ReplacedBtn
            // 
            this.ReplacedBtn.Checked = true;
            this.ReplacedBtn.CheckOnClick = true;
            this.ReplacedBtn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ReplacedBtn.Name = "ReplacedBtn";
            this.ReplacedBtn.Size = new System.Drawing.Size(152, 22);
            this.ReplacedBtn.Text = "Replaced";
            // 
            // DisplaySpecialsBtn
            // 
            this.DisplaySpecialsBtn.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.HashBtn,
            this.StructureBtn,
            this.HexadecimalBtn,
            this.ParserNameBtn,
            this.MessageName});
            this.DisplaySpecialsBtn.Name = "DisplaySpecialsBtn";
            this.DisplaySpecialsBtn.Size = new System.Drawing.Size(196, 22);
            this.DisplaySpecialsBtn.Text = "Display Details";
            // 
            // HashBtn
            // 
            this.HashBtn.Checked = true;
            this.HashBtn.CheckOnClick = true;
            this.HashBtn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.HashBtn.Name = "HashBtn";
            this.HashBtn.Size = new System.Drawing.Size(155, 22);
            this.HashBtn.Text = "Hash";
            // 
            // StructureBtn
            // 
            this.StructureBtn.Checked = true;
            this.StructureBtn.CheckOnClick = true;
            this.StructureBtn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.StructureBtn.Name = "StructureBtn";
            this.StructureBtn.Size = new System.Drawing.Size(155, 22);
            this.StructureBtn.Text = "Structure";
            // 
            // HexadecimalBtn
            // 
            this.HexadecimalBtn.CheckOnClick = true;
            this.HexadecimalBtn.Name = "HexadecimalBtn";
            this.HexadecimalBtn.Size = new System.Drawing.Size(155, 22);
            this.HexadecimalBtn.Text = "Hexadecimal";
            // 
            // ParserNameBtn
            // 
            this.ParserNameBtn.Checked = true;
            this.ParserNameBtn.CheckOnClick = true;
            this.ParserNameBtn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ParserNameBtn.Name = "ParserNameBtn";
            this.ParserNameBtn.Size = new System.Drawing.Size(155, 22);
            this.ParserNameBtn.Text = "Parser Name";
            // 
            // MessageName
            // 
            this.MessageName.Checked = true;
            this.MessageName.CheckOnClick = true;
            this.MessageName.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MessageName.Name = "MessageName";
            this.MessageName.Size = new System.Drawing.Size(155, 22);
            this.MessageName.Text = "Message Name";
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
            this.IgnoreMessagesBtn.Click += new System.EventHandler(this.IgnoreMessagesBtn_Click);
            // 
            // PacketLoggerStrip
            // 
            this.PacketLoggerStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ViewOutgoingLbl,
            this.ViewIncomingLbl,
            this.RevisionTxt});
            this.PacketLoggerStrip.Location = new System.Drawing.Point(0, 497);
            this.PacketLoggerStrip.Name = "PacketLoggerStrip";
            this.PacketLoggerStrip.Size = new System.Drawing.Size(710, 24);
            this.PacketLoggerStrip.TabIndex = 8;
            // 
            // ViewOutgoingLbl
            // 
            this.ViewOutgoingLbl.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.ViewOutgoingLbl.Name = "ViewOutgoingLbl";
            this.ViewOutgoingLbl.Size = new System.Drawing.Size(119, 19);
            this.ViewOutgoingLbl.Text = "View Outgoing: True";
            // 
            // ViewIncomingLbl
            // 
            this.ViewIncomingLbl.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.ViewIncomingLbl.Name = "ViewIncomingLbl";
            this.ViewIncomingLbl.Size = new System.Drawing.Size(119, 19);
            this.ViewIncomingLbl.Text = "View Incoming: True";
            // 
            // RevisionTxt
            // 
            this.RevisionTxt.Name = "RevisionTxt";
            this.RevisionTxt.Size = new System.Drawing.Size(63, 19);
            this.RevisionTxt.Text = "Revision: 0";
            // 
            // PacketLoggerFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(710, 521);
            this.Controls.Add(this.LoggerTxt);
            this.Controls.Add(this.PacketLoggerStrip);
            this.Controls.Add(this.PacketLoggerMenu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.Name = "PacketLoggerFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tanji ~ Packet Logger";
            this.PacketLoggerContextMenu.ResumeLayout(false);
            this.PacketLoggerMenu.ResumeLayout(false);
            this.PacketLoggerMenu.PerformLayout();
            this.PacketLoggerStrip.ResumeLayout(false);
            this.PacketLoggerStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip PacketLoggerMenu;
        private Tanji.Controls.BindableToolStripMenuItem FileBtn;
        private Tanji.Controls.BindableToolStripMenuItem EmptyLogBtn;
        private Tanji.Controls.BindableToolStripMenuItem ViewBtn;
        private Tanji.Controls.BindableToolStripMenuItem DisplayFiltersBtn;
        private Tanji.Controls.BindableToolStripMenuItem BlockedBtn;
        private Tanji.Controls.BindableToolStripMenuItem ReplacedBtn;
        private System.Windows.Forms.ToolStripSeparator ViewSep2;
        private Tanji.Controls.BindableToolStripMenuItem AlwaysOnTopBtn;
        private System.Windows.Forms.StatusStrip PacketLoggerStrip;
        private System.Windows.Forms.ToolStripStatusLabel ViewOutgoingLbl;
        private System.Windows.Forms.ToolStripStatusLabel ViewIncomingLbl;
        private System.Windows.Forms.ContextMenuStrip PacketLoggerContextMenu;
        public Tanji.Controls.BindableToolStripMenuItem PLCMCopyBtn;
        private Tanji.Controls.BindableToolStripMenuItem DisplaySpecialsBtn;
        private Tanji.Controls.BindableToolStripMenuItem MessageName;
        private Tanji.Controls.BindableToolStripMenuItem ParserNameBtn;
        private Tanji.Controls.BindableToolStripMenuItem HashBtn;
        private Tanji.Controls.BindableToolStripMenuItem FindMessageBtn;
        private System.Windows.Forms.ToolStripSeparator FileSep1;
        private Tanji.Controls.BindableToolStripMenuItem FindBtn;
        internal System.Windows.Forms.RichTextBox LoggerTxt;
        private Tanji.Controls.BindableToolStripMenuItem ViewOutgoingBtn;
        private Tanji.Controls.BindableToolStripMenuItem ViewIncomingBtn;
        private System.Windows.Forms.ToolStripSeparator ViewSep1;
        internal System.Windows.Forms.ToolStripStatusLabel RevisionTxt;
        private Tanji.Controls.BindableToolStripMenuItem ToolsBtn;
        private Tanji.Controls.BindableToolStripMenuItem IgnoreMessagesBtn;
        private Controls.BindableToolStripMenuItem StructureBtn;
        private Controls.BindableToolStripMenuItem HexadecimalBtn;
    }
}