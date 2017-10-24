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
            this.PLCMCopyBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.PacketLoggerMenu = new System.Windows.Forms.MenuStrip();
            this.FileBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.FindBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.FindMessageBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.FileSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.EmptyLogBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.DisplayFiltersBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.BlockedBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.ReplacedBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.DisplaySpecialsBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.HashBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.TimestampBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.ClassNameBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.ParserName = new System.Windows.Forms.ToolStripMenuItem();
            this.DisplayStructureBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.ViewOutgoingBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewIncomingBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewSep2 = new System.Windows.Forms.ToolStripSeparator();
            this.AlwaysOnTopBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolsBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.IgnoreMessagesBtn = new System.Windows.Forms.ToolStripMenuItem();
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
            this.DisplayStructureBtn,
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
            // DisplaySpecialsBtn
            // 
            this.DisplaySpecialsBtn.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.HashBtn,
            this.TimestampBtn,
            this.ClassNameBtn,
            this.ParserName});
            this.DisplaySpecialsBtn.Name = "DisplaySpecialsBtn";
            this.DisplaySpecialsBtn.Size = new System.Drawing.Size(196, 22);
            this.DisplaySpecialsBtn.Text = "Display Specials";
            // 
            // HashBtn
            // 
            this.HashBtn.CheckOnClick = true;
            this.HashBtn.Name = "HashBtn";
            this.HashBtn.Size = new System.Drawing.Size(141, 22);
            this.HashBtn.Text = "Hash";
            // 
            // TimestampBtn
            // 
            this.TimestampBtn.CheckOnClick = true;
            this.TimestampBtn.Name = "TimestampBtn";
            this.TimestampBtn.Size = new System.Drawing.Size(141, 22);
            this.TimestampBtn.Text = "Timestamp";
            // 
            // ClassNameBtn
            // 
            this.ClassNameBtn.Checked = true;
            this.ClassNameBtn.CheckOnClick = true;
            this.ClassNameBtn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ClassNameBtn.Name = "ClassNameBtn";
            this.ClassNameBtn.Size = new System.Drawing.Size(141, 22);
            this.ClassNameBtn.Text = "Class Name";
            // 
            // ParserName
            // 
            this.ParserName.Checked = true;
            this.ParserName.CheckOnClick = true;
            this.ParserName.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ParserName.Name = "ParserName";
            this.ParserName.Size = new System.Drawing.Size(141, 22);
            this.ParserName.Text = "Parser Name";
            // 
            // DisplayStructureBtn
            // 
            this.DisplayStructureBtn.Checked = true;
            this.DisplayStructureBtn.CheckOnClick = true;
            this.DisplayStructureBtn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DisplayStructureBtn.Name = "DisplayStructureBtn";
            this.DisplayStructureBtn.Size = new System.Drawing.Size(196, 22);
            this.DisplayStructureBtn.Text = "Display Structure";
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
            this.IgnoreMessagesBtn.Size = new System.Drawing.Size(171, 22);
            this.IgnoreMessagesBtn.Text = "Ignore Messages...";
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
        private System.Windows.Forms.ToolStripMenuItem FileBtn;
        private System.Windows.Forms.ToolStripMenuItem EmptyLogBtn;
        private System.Windows.Forms.ToolStripMenuItem ViewBtn;
        private System.Windows.Forms.ToolStripMenuItem DisplayFiltersBtn;
        private System.Windows.Forms.ToolStripMenuItem BlockedBtn;
        private System.Windows.Forms.ToolStripMenuItem ReplacedBtn;
        private System.Windows.Forms.ToolStripSeparator ViewSep2;
        private System.Windows.Forms.ToolStripMenuItem AlwaysOnTopBtn;
        private System.Windows.Forms.StatusStrip PacketLoggerStrip;
        private System.Windows.Forms.ToolStripStatusLabel ViewOutgoingLbl;
        private System.Windows.Forms.ToolStripStatusLabel ViewIncomingLbl;
        private System.Windows.Forms.ToolStripMenuItem DisplayStructureBtn;
        private System.Windows.Forms.ContextMenuStrip PacketLoggerContextMenu;
        public System.Windows.Forms.ToolStripMenuItem PLCMCopyBtn;
        private System.Windows.Forms.ToolStripMenuItem DisplaySpecialsBtn;
        private System.Windows.Forms.ToolStripMenuItem TimestampBtn;
        private System.Windows.Forms.ToolStripMenuItem ClassNameBtn;
        private System.Windows.Forms.ToolStripMenuItem ParserName;
        private System.Windows.Forms.ToolStripMenuItem HashBtn;
        private System.Windows.Forms.ToolStripMenuItem FindMessageBtn;
        private System.Windows.Forms.ToolStripSeparator FileSep1;
        private System.Windows.Forms.ToolStripMenuItem FindBtn;
        internal System.Windows.Forms.RichTextBox LoggerTxt;
        private System.Windows.Forms.ToolStripMenuItem ViewOutgoingBtn;
        private System.Windows.Forms.ToolStripMenuItem ViewIncomingBtn;
        private System.Windows.Forms.ToolStripSeparator ViewSep1;
        internal System.Windows.Forms.ToolStripStatusLabel RevisionTxt;
        private System.Windows.Forms.ToolStripMenuItem ToolsBtn;
        private System.Windows.Forms.ToolStripMenuItem IgnoreMessagesBtn;
    }
}