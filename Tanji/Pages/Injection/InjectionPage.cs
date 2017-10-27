using System;
using System.Windows.Forms;
using System.Threading.Tasks;

using Tanji.Windows;
using Tanji.Manipulators;

using Sulakore.Protocol;
using Sulakore.Communication;

namespace Tanji.Pages.Injection
{
    public class InjectionPage : TanjiPage
    {
        private const string INJECTION_UNAUTHORIZED =
            "Injection functionality is currently disabled, consider connecting before attempting to utilize any of these services.";

        private const string PACKET_LENGTH_INVALID =
            "The packet's head contains an invalid length value that does not match the actual size of the data present.";

        public FiltersPage FiltersPg { get; }
        public PrimitivePage PrimitivePg { get; }
        public SchedulerPage SchedulerPg { get; }
        public ConstructerPage ConstructerPg { get; }

        public InjectionPage(MainFrm ui, TabPage tab)
            : base(ui, tab)
        {
            UI.AddQuickAction(Keys.F1, UI.ITSendToClientBtn.PerformClick);
            UI.AddQuickAction(Keys.F2, UI.ITSendToServerBtn.PerformClick);

            UI.ITSendToClientBtn.Click += ITSendToClientBtn_Click;
            UI.ITSendToServerBtn.Click += ITSendToServerBtn_Click;

            FiltersPg = new FiltersPage(this, UI.FiltersTab);
            PrimitivePg = new PrimitivePage(this, UI.PrimitiveTab);
            SchedulerPg = new SchedulerPage(this, UI.SchedulerTab);
            ConstructerPg = new ConstructerPage(this, UI.ConstructerTab);
        }

        private async void ITSendToClientBtn_Click(object sender, EventArgs e)
        {
            await InjectInputAsync(HDestination.Client)
                .ConfigureAwait(false);
        }
        private async void ITSendToServerBtn_Click(object sender, EventArgs e)
        {
            await InjectInputAsync(HDestination.Server)
                .ConfigureAwait(false);
        }

        public HMessage GetPacket()
        {
            return new HMessage(UI.ITPacketTxt.Text);
        }
        public async Task<int> SendAsync(HMessage packet)
        {
            if (packet.IsCorrupted) return 0;

            bool toServer =
                (packet.Destination == HDestination.Server);

            HNode node = (toServer ?
                UI.Connection.Remote : UI.Connection.Local);

            return await node.SendPacketAsync(packet).ConfigureAwait(false);
        }
        public async Task<int> InjectInputAsync(HDestination destination)
        {
            HMessage packet = GetPacket();
            if (!AuthorizeInjection(packet)) return 0;
            packet.Destination = destination;

            int length = await SendAsync(
                packet).ConfigureAwait(false);

            if (length == (packet.Length + 4))
                AddAutocomplete(packet.ToString());

            return length;
        }

        public bool AuthorizeInjection()
        {
            bool isConnected = UI.Connection.IsConnected;
            if (!isConnected)
            {
                MessageBox.Show(INJECTION_UNAUTHORIZED, "Tanji ~ Alert!",
                    MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            return isConnected;
        }
        public bool ValidatePacket(HMessage packet)
        {
            bool isCorrupted = packet.IsCorrupted;
            if (isCorrupted)
            {
                byte[] data = packet.ToBytes();
                string alertMsg = PACKET_LENGTH_INVALID;

                if (data.Length < 6)
                {
                    alertMsg +=
                        $"\r\n\r\nYou're missing {6 - data.Length:#,##0} byte(s).";
                }
                else
                {
                    int length = (data.Length > 0 ?
                        BigEndian.ToInt32(data, 0) : 0);

                    int realLength = (data.Length - 4);
                    bool tooShort = (length > realLength);
                    int difference = Math.Abs(realLength - length);

                    alertMsg +=
                        $"\r\n\r\nYou're {difference:#,##0} byte(s) too {(tooShort ? "short" : "big")}.";
                }
                MessageBox.Show(alertMsg, "Tanji ~ Alert!",
                    MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            return !isCorrupted;
        }
        public bool AuthorizeInjection(HMessage packet)
        {
            return (AuthorizeInjection() &&
                ValidatePacket(packet));
        }

        private void AddAutocomplete(string value)
        {
            if (!UI.ITPacketTxt.Items.Contains(value))
                UI.ITPacketTxt.Items.Add(value);
        }
    }
}