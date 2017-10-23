using System;
using System.IO;
using System.Windows.Forms;

using Tanji.Manipulators;

using Sulakore.Protocol;

namespace Tanji.Pages.Injection
{
    public class PrimitivePage : TanjiSubPage<InjectionPage>, IRetrievable
    {
        public PrimitivePage(InjectionPage parent, TabPage tab)
            : base(parent, tab)
        {
            UI.PTSaveAsBtn.Click += PTSaveAsBtn_Click;
        }

        private void PTSaveAsBtn_Click(object sender, EventArgs e)
        {
            UI.SavePacketDlg.FileName = string.Empty;
            if (UI.SavePacketDlg.ShowDialog() != DialogResult.OK) return;

            File.WriteAllBytes(UI.SavePacketDlg.FileName,
                GetPacket().ToBytes());
        }
        private void PTPacketTxt_TextChanged(object sender, EventArgs e)
        {
            HMessage packet = GetPacket();
            UI.ITPacketTxt.Text = packet.ToString();

            UI.PTHeaderTxt.Value = packet.Header.ToString();
            UI.PTLengthTxt.Value = packet.Length.ToString("n0");

            UI.PTCorruptedTxt.Value =
                packet.IsCorrupted.ToString();

            UI.PTCorruptedTxt.BackColor = (packet.IsCorrupted ?
                UI.PacketLoggerUI.IncomingHighlight :
                UI.PacketLoggerUI.OutgoingHighlight);
        }

        public HMessage GetPacket()
        {
            return new HMessage(UI.PTPacketTxt.Text);
        }

        protected override void OnTabSelecting(TabControlCancelEventArgs e)
        {
            UI.ITPacketTxt.Enabled = false;
            UI.PTPacketTxt.TextChanged += PTPacketTxt_TextChanged;

            UI.PTPacketTxt.Text = UI.ITPacketTxt.Text;
            UI.InjectionMenu.InputBox = UI.PTPacketTxt;

            base.OnTabSelecting(e);
        }
        protected override void OnTabDeselecting(TabControlCancelEventArgs e)
        {
            UI.ITPacketTxt.Enabled = true;
            UI.PTPacketTxt.TextChanged -= PTPacketTxt_TextChanged;

            base.OnTabDeselecting(e);
        }
    }
}