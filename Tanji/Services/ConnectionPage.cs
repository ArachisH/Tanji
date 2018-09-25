using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Net.Http;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;

using Tanji.Network;
using Tanji.Controls;

using Sulakore.Habbo;
using Sulakore.Crypto;
using Sulakore.Network;

using Eavesdrop;

using Flazzy;

namespace Tanji.Services
{
    [ToolboxItem(true)]
    [DesignerCategory("UserControl")]
    public partial class ConnectionPage : ObservablePage, IHaltable, IReceiver
    {
        private Guid _randomQuery;
        private Dictionary<string, string> _variableReplacements;

        #region Status Constants
        private const string STANDING_BY = "Standing By...";

        private const string INTERCEPTING_CLIENT = "Intercepting Client...";
        private const string INTERCEPTING_CONNECTION = "Intercepting Connection...";
        private const string INTERCEPTING_CLIENT_PAGE = "Intercepting Client Page...";

        private const string MODIFYING_CLIENT = "Modifying Client...";
        private const string INJECTING_CLIENT = "Injecting Client...";
        private const string GENERATING_MESSAGE_HASHES = "Generating Message Hashes...";

        private const string ASSEMBLING_CLIENT = "Assembling Client...";
        private const string DISASSEMBLING_CLIENT = "Disassembling Client...";
        #endregion

        private const ushort PROXY_PORT = 8282;

        private string _customClientPath = null;
        [DefaultValue(null)]
        public string CustomClientPath
        {
            get => _customClientPath;
            set
            {
                _customClientPath = value;
                RaiseOnPropertyChanged();
            }
        }

        private HotelEndPoint _hotelServer = null;
        [Browsable(false)]
        [DefaultValue(null)]
        public HotelEndPoint HotelServer
        {
            get => _hotelServer;
            set
            {
                _hotelServer = value;
                RaiseOnPropertyChanged();
            }
        }

        private string _status = STANDING_BY;
        [Browsable(false)]
        [DefaultValue(STANDING_BY)]
        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                RaiseOnPropertyChanged();
            }
        }

        public ConnectionPage()
        {
            InitializeComponent();

            Bind(StatusTxt, "Text", nameof(Status));
            Bind(CustomClientTxt, "Text", nameof(CustomClientPath));
        }

        private Task InjectGameClientAsync(object sender, RequestInterceptedEventArgs e)
        {
            if (!e.Uri.Query.StartsWith("?" + _randomQuery)) return null;
            Eavesdropper.RequestInterceptedAsync -= InjectGameClientAsync;

            Uri remoteUrl = e.Request.RequestUri;
            string clientPath = Path.GetFullPath($"Modified Clients/{remoteUrl.Host}/{remoteUrl.LocalPath}");
            if (!string.IsNullOrWhiteSpace(CustomClientPath))
            {
                clientPath = CustomClientPath;
            }
            if (!File.Exists(clientPath))
            {
                Status = INTERCEPTING_CLIENT;
                Eavesdropper.ResponseInterceptedAsync += InterceptGameClientAsync;
            }
            else
            {
                Status = DISASSEMBLING_CLIENT;
                Master.Game = new HGame(clientPath);
                Master.Game.Disassemble();

                if (Master.Game.IsPostShuffle)
                {
                    Status = GENERATING_MESSAGE_HASHES;
                    Master.Game.GenerateMessageHashes();
                }
                Program.Master.In.Load(Master.Game, "Hashes.ini");
                Program.Master.Out.Load(Master.Game, "Hashes.ini");

                TerminateProxy();
                Task interceptConnectionTask = InterceptConnectionAsync();
                e.Request = WebRequest.Create(new Uri(clientPath));
            }
            return null;
        }
        private async Task InterceptGameClientAsync(object sender, ResponseInterceptedEventArgs e)
        {
            if (!e.Uri.Query.StartsWith("?" + _randomQuery)) return;
            if (e.ContentType != "application/x-shockwave-flash") return;
            Eavesdropper.ResponseInterceptedAsync -= InterceptGameClientAsync;

            string clientPath = Path.GetFullPath($"Modified Clients/{e.Uri.Host}/{e.Uri.LocalPath}");
            string clientDirectory = Path.GetDirectoryName(clientPath);
            Directory.CreateDirectory(clientDirectory);

            Status = DISASSEMBLING_CLIENT;
            Master.Game = new HGame(await e.Content.ReadAsStreamAsync().ConfigureAwait(false));
            Master.Game.Location = clientPath;
            Master.Game.Disassemble();

            if (Master.Game.IsPostShuffle)
            {
                Status = GENERATING_MESSAGE_HASHES;
                Master.Game.GenerateMessageHashes();

                Status = MODIFYING_CLIENT;
                Master.Game.DisableHostChecks();
                Master.Game.InjectKeyShouter(4001);
                Master.Game.InjectEndPointShouter(4000);
            }
            Program.Master.In.Load(Master.Game, "Hashes.ini");
            Program.Master.Out.Load(Master.Game, "Hashes.ini");
            Master.Game.InjectEndPoint("127.0.0.1", Master.Connection.ListenPort);

            CompressionKind compression = CompressionKind.ZLIB;
#if DEBUG
            compression = CompressionKind.None;
#endif
            Status = ASSEMBLING_CLIENT;

            byte[] payload = Master.Game.ToArray(compression);
            e.Headers[HttpResponseHeader.ContentLength] = payload.Length.ToString();

            e.Content = new ByteArrayContent(payload);
            using (var clientStream = File.Open(clientPath, FileMode.Create, FileAccess.Write))
            {
                clientStream.Write(payload, 0, payload.Length);
            }

            TerminateProxy();
            Task interceptConnectionTask = InterceptConnectionAsync();
        }
        private async Task InterceptClientPageAsync(object sender, ResponseInterceptedEventArgs e)
        {
            if (e.Content == null) return;
            if (!e.ContentType.Contains("text")) return;

            string body = await e.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (!body.Contains("info.host") && !body.Contains("info.port")) return;

            Eavesdropper.ResponseInterceptedAsync -= InterceptClientPageAsync;
            Master.GameData.Source = body;

            body = body.Replace(".swf", $".swf?{_randomQuery = Guid.NewGuid()}");
            e.Content = new StringContent(body);

            Status = INJECTING_CLIENT;
            Eavesdropper.RequestInterceptedAsync += InjectGameClientAsync;
        }

        private void BrowseBtn_Click(object sender, EventArgs e)
        {
            ChooseClientDlg.FileName = null;
            if (ChooseClientDlg.ShowDialog() == DialogResult.OK)
            {
                CustomClientPath = ChooseClientDlg.FileName;
            }
        }
        private void DestroyCertificatesBtn_Click(object sender, EventArgs e)
        {
            Eavesdropper.Certifier.DestroyCertificates();
        }
        private void ExportCertificateAuthorityBtn_Click(object sender, EventArgs e)
        {
            if (!Eavesdropper.Certifier.CreateTrustedRootCertificate()) return;
            if (ExportCertificateDlg.ShowDialog() == DialogResult.OK)
            {
                Eavesdropper.Certifier.ExportTrustedRootCertificate(ExportCertificateDlg.FileName);
            }
        }

        private void ConnectBtn_Click(object sender, EventArgs e)
        {
            if (Status != STANDING_BY)
            {
                TerminateProxy();
                Master.Connection.Disconnect();

                Status = STANDING_BY;
                return;
            }

            if (Master.IsConnected)
            {
                if (MessageBox.Show("Are you sure you want to disconnect from the current session?", "Tanji - Alert!",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Asterisk) == DialogResult.Yes)
                {
                    Master.Connection.Disconnect();
                }
                else return;
            }

            _variableReplacements = VariablesLv.CheckedItems
                .Cast<ListViewItem>()
                .Where(i => !string.IsNullOrWhiteSpace(i.SubItems[1].Text))
                .ToDictionary(i => i.Text, i => i.SubItems[1].Text);

            if (Eavesdropper.Certifier.CreateTrustedRootCertificate())
            {
                Eavesdropper.ResponseInterceptedAsync += InterceptClientPageAsync;
                Eavesdropper.Initiate(PROXY_PORT);
                Status = INTERCEPTING_CLIENT_PAGE;
            }
        }

        private void ResetBtn_Click(object sender, EventArgs e)
        {
            ValueTxt.Text = string.Empty;
            VariablesLv.SelectedItem.Checked = false;
            VariablesLv.SelectedItem.SubItems[1].Text = string.Empty;
        }
        private void UpdateBtn_Click(object sender, EventArgs e)
        {
            VariablesLv.SelectedItem.SubItems[1].Text = ValueTxt.Text;
            if (!string.IsNullOrWhiteSpace(ValueTxt.Text))
            {
                VariablesLv.SelectedItem.Checked = true;
            }
        }
        private void ValueTxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                if (UpdateBtn.Enabled)
                {
                    UpdateBtn_Click(sender, e);
                }
            }
        }

        private void VariablesLv_ItemSelected(object sender, EventArgs e)
        {
            VariableTxt.Text = VariablesLv.SelectedItem.Text;
            if (ValueTxt.Text == VariablesLv.SelectedItem.SubItems[1].Text)
            {
                ValueTxt.Text = VariablesLv.SelectedItem.SubItems[1].Text;
            }
        }
        private void VariablesLv_ItemSelectionStateChanged(object sender, EventArgs e)
        {
            UpdateBtn.Enabled = ResetBtn.Enabled = VariablesLv.HasSelectedItem;
            if (!VariablesLv.HasSelectedItem)
            {
                VariableTxt.Text = ValueTxt.Text = string.Empty;
            }
        }

        private void ConnectionPage_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Status):
                {
                    bool isBusy = (Status != STANDING_BY);
                    ConnectBtn.Text = (isBusy ? "Cancel" : "Connect");

                    BrowseBtn.Enabled = !isBusy;
                    CustomClientTxt.IsReadOnly = isBusy;
                    DestroyCertificatesBtn.Enabled = !isBusy;
                    break;
                }
            }
        }

        private void TerminateProxy()
        {
            Eavesdropper.Terminate();
            Eavesdropper.RequestInterceptedAsync -= InjectGameClientAsync;
            Eavesdropper.ResponseInterceptedAsync -= InterceptClientPageAsync;
            Eavesdropper.ResponseInterceptedAsync -= InterceptGameClientAsync;
        }
        private async Task InterceptConnectionAsync()
        {
            Status = INTERCEPTING_CONNECTION;
            Master.Connection.SocketSkip = (Master.Game.IsPostShuffle ? 2 : 0);

            await Master.Connection.InterceptAsync(HotelServer).ConfigureAwait(false);
            Status = STANDING_BY;
        }

        #region IHaltable Implementation
        public void Halt()
        { }
        public void Restore(ConnectedEventArgs e)
        {
            IsReceiving = true;
        }
        #endregion
        #region IReceiver Implementation
        [Browsable(false)]
        public bool IsReceiving { get; set; }

        [Browsable(false)]
        public bool IsIncomingEncrypted { get; private set; }

        public void HandleOutgoing(DataInterceptedEventArgs e)
        {
            if (e.Packet.Id == 4001)
            {
                string sharedKeyHex = e.Packet.ReadUTF8();
                if (sharedKeyHex.Length % 2 != 0)
                {
                    sharedKeyHex = ("0" + sharedKeyHex);
                }

                byte[] sharedKey = Enumerable.Range(0, sharedKeyHex.Length / 2)
                    .Select(x => Convert.ToByte(sharedKeyHex.Substring(x * 2, 2), 16))
                    .ToArray();

                Master.Connection.Remote.Encrypter = new RC4(sharedKey);
                Master.Connection.Remote.IsEncrypting = true;

                if (IsIncomingEncrypted)
                {
                    Master.Connection.Remote.Decrypter = new RC4(sharedKey);
                    Master.Connection.Remote.IsDecrypting = true;
                }

                e.IsBlocked = true;
                IsReceiving = false;
            }
            else if (e.Step >= 10)
            {
                IsReceiving = false;
            }
        }
        public void HandleIncoming(DataInterceptedEventArgs e)
        {
            if (e.Step == 2)
            {
                e.Packet.ReadUTF8();
                IsIncomingEncrypted = e.Packet.ReadBoolean();
                e.Packet.Replace(false, e.Packet.Position - 1);
            }
        }
        #endregion
    }
}