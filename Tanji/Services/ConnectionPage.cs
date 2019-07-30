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

using Sulakore.Crypto;
using Sulakore.Network;
using Sulakore.Habbo.Web;
using Sulakore.Habbo.Messages;

using Eavesdrop;

using Flazzy;
using Flazzy.ABC;
using Flazzy.ABC.AVM2;
using Flazzy.ABC.AVM2.Instructions;

namespace Tanji.Services
{
    [ToolboxItem(true)]
    [DesignerCategory("UserControl")]
    public partial class ConnectionPage : ObservablePage, IHaltable, IReceiver
    {
        private Guid _randomQuery;
        private bool _wasBlacklisted;
        private Dictionary<string, string> _variableReplacements;

        #region Status Constants
        private const string STANDING_BY = "Standing By...";

        private const string INTERCEPTING_CLIENT = "Intercepting Client...";
        private const string INTERCEPTING_CONNECTION = "Intercepting Connection...";
        private const string INTERCEPTING_CLIENT_PAGE = "Intercepting Client Page...";
        private const string INTERCEPTING_CLIENT_REQUEST_RESPONSE = "Intercepting Client Request/Response";

        private const string MODIFYING_CLIENT = "Modifying Client...";
        private const string INJECTING_CLIENT = "Injecting Client...";
        private const string GENERATING_MESSAGE_HASHES = "Generating Message Hashes...";

        private const string ASSEMBLING_CLIENT = "Assembling Client...";
        private const string DISASSEMBLING_CLIENT = "Disassembling Client...";
        #endregion

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
            if (!_wasBlacklisted && !e.Uri.Query.StartsWith("?" + _randomQuery)) return null;

            string clientPath = Path.GetFullPath($"Modified Clients/{e.Uri.Host}/{e.Uri.LocalPath}");
            if (_wasBlacklisted && !File.Exists(clientPath)) return null;

            Eavesdropper.RequestInterceptedAsync -= InjectGameClientAsync;
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
                if (_wasBlacklisted)
                {
                    Eavesdropper.ResponseInterceptedAsync -= InterceptGameClientAsync;
                }

                Status = DISASSEMBLING_CLIENT;
                Master.Game = new HGame(clientPath);
                Master.Game.Disassemble();

                if (Master.Game.IsPostShuffle)
                {
                    Status = GENERATING_MESSAGE_HASHES;
                    Master.Game.GenerateMessageHashes("Hashes.ini");
                }

                TerminateProxy();
                Task interceptConnectionTask = InterceptConnectionAsync();
                e.Request = WebRequest.Create(new Uri(clientPath));
            }
            return null;
        }
        private async Task InterceptGameClientAsync(object sender, ResponseInterceptedEventArgs e)
        {
            if (e.ContentType != "application/x-shockwave-flash") return;
            if (!_wasBlacklisted && !e.Uri.Query.StartsWith("?" + _randomQuery)) return;

            byte[] payload = await e.Content.ReadAsByteArrayAsync().ConfigureAwait(false);

            HGame game = null;
            if (_wasBlacklisted && !IsGameClient(payload, out game)) return;
            Eavesdropper.ResponseInterceptedAsync -= InterceptGameClientAsync;

            string clientPath = Path.GetFullPath($"Modified Clients/{e.Uri.Host}/{e.Uri.LocalPath}");
            string clientDirectory = Path.GetDirectoryName(clientPath);
            Directory.CreateDirectory(clientDirectory);

            if (!_wasBlacklisted)
            {
                Status = DISASSEMBLING_CLIENT;
                game = new HGame(payload);
                game.Location = clientPath;
                game.Disassemble();

                if (game.IsPostShuffle)
                {
                    Status = GENERATING_MESSAGE_HASHES;
                    game.GenerateMessageHashes("Hashes.ini");

                    Status = MODIFYING_CLIENT;
                    game.DisableHostChecks();
                    game.InjectKeyShouter(4001);
                    game.InjectEndPointShouter(4000);
                }
                game.InjectEndPoint("127.0.0.1", Master.Config.GameListenPort);
            }

            Master.Game = game;
            Master.Game.Location = clientPath;

            CompressionKind compression = CompressionKind.ZLIB;
#if DEBUG
            compression = CompressionKind.None;
#endif
            Status = ASSEMBLING_CLIENT;

            byte[] assembled = Master.Game.ToArray(compression);
            e.Headers[HttpResponseHeader.ContentLength] = assembled.Length.ToString();

            e.Content = new ByteArrayContent(assembled);
            using (var clientStream = File.Open(clientPath, FileMode.Create, FileAccess.Write))
            {
                clientStream.Write(assembled, 0, assembled.Length);
            }

            TerminateProxy();
            Task interceptConnectionTask = InterceptConnectionAsync();
        }
        private async Task InterceptClientPageAsync(object sender, ResponseInterceptedEventArgs e)
        {
            if (e.Content == null) return;

            string contentType = e.ContentType.ToLower();
            if (!contentType.Contains("text") && !contentType.Contains("javascript")) return;

            int interceptionTriggersFound = -1;
            string body = await e.Content.ReadAsStringAsync().ConfigureAwait(false);
            foreach (string interceptionTrigger in Master.Config.InterceptionTriggers)
            {
                interceptionTriggersFound += Convert.ToInt32(body.IndexOf(interceptionTrigger, StringComparison.OrdinalIgnoreCase) != -1);
            }
            if (interceptionTriggersFound < 2) return;

            int swfStartIndex = GetSWFStartIndex(body);
            bool isBlacklisted = _wasBlacklisted = Master.Config.CacheBlacklist.Contains(e.Uri.Host);
            if (swfStartIndex == -1 && !isBlacklisted) return;

            Master.GameData.Source = body;
            Eavesdropper.ResponseInterceptedAsync -= InterceptClientPageAsync;

            if (!isBlacklisted)
            {
                do
                {
                    if (body[swfStartIndex++] == ')') continue;
                    var embedSWFEnd = body.IndexOf(',', swfStartIndex);

                    if (embedSWFEnd == -1) break;
                    body = body.Insert(embedSWFEnd, $"+\"?{_randomQuery = Guid.NewGuid()}\"");
                }
                while ((swfStartIndex = GetSWFStartIndex(body, swfStartIndex)) != -1);
                e.Content = new StringContent(body);

                Status = INJECTING_CLIENT;
                Eavesdropper.RequestInterceptedAsync += InjectGameClientAsync;
            }
            else
            {
                Status = INTERCEPTING_CLIENT_REQUEST_RESPONSE;
                Eavesdropper.RequestInterceptedAsync += InjectGameClientAsync;
                Eavesdropper.ResponseInterceptedAsync += InterceptGameClientAsync;
            }
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
#if DEBUG
                FindForm().WindowState = FormWindowState.Minimized;
#endif
                Eavesdropper.ResponseInterceptedAsync += InterceptClientPageAsync;
                Eavesdropper.Initiate(Master.Config.ProxyListenPort);
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
                    bool isBusy = Status != STANDING_BY;
                    ConnectBtn.Text = isBusy ? "Cancel" : "Connect";

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
        private bool HasPingInstructions()
        {
            ASMethod connectMethod = Master.Game.GetManagerConnectMethod();
            if (connectMethod == null) return false;

            ASCode connectCode = connectMethod.Body.ParseCode();
            for (int i = 0, findPropStrictCount = 0; i < connectCode.Count; i++)
            {
                ASInstruction instruction = connectCode[i];
                if (instruction.OP != OPCode.FindPropStrict || ++findPropStrictCount != 3) continue;
                return true;
            }

            return false;
        }
        private async Task InterceptConnectionAsync()
        {
            Status = INTERCEPTING_CONNECTION;
            if (Master.Game.IsPostShuffle && HasPingInstructions())
            {
                Master.Connection.SocketSkip = 2;
            }

            Master.Game.Dispose();
            foreach (HMessage message in Master.Out.Concat(Master.In))
            {
                message.Class = null;
                message.Parser = null;
                message.References.Clear();
            }

            await Master.Connection.InterceptAsync(HotelServer).ConfigureAwait(false);
            Status = STANDING_BY;
        }
        private bool IsGameClient(byte[] data, out HGame game)
        {
            HGame possibleGame = null;
            try
            {
                possibleGame = new HGame(data);
                possibleGame.Disassemble();

                if (possibleGame.IsPostShuffle)
                {
                    possibleGame.GenerateMessageHashes();
                    if (!possibleGame.DisableHostChecks()) return false;
                    if (!possibleGame.InjectKeyShouter(4001)) return false;
                    if (!possibleGame.InjectEndPointShouter(4000)) return false;
                }
                possibleGame.InjectEndPoint("127.0.0.1", Master.Config.GameListenPort); // Does not matter if this returns true/false.
            }
            catch
            {
                possibleGame?.Dispose();
                possibleGame = null;
            }
            finally
            {
                game = possibleGame;
            }
            return game != null;
        }
        private int GetSWFStartIndex(string body, int index = 0)
        {
            int swfStartIndex = body.IndexOf("embedswf(", index, StringComparison.OrdinalIgnoreCase) + 9;
            if (swfStartIndex == 8)
            {
                swfStartIndex = body.IndexOf("swfobject(", index, StringComparison.OrdinalIgnoreCase) + 10;
                if (swfStartIndex == 9) return -1;
            }
            return swfStartIndex;
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
                    sharedKeyHex = "0" + sharedKeyHex;
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