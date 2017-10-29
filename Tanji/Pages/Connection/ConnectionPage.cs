using System;
using System.IO;
using System.Net;
using System.Text;
using System.Linq;
using System.Drawing;
using System.Net.Http;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;

using Tanji.Windows;
using Tanji.Manipulators;

using Sulakore;
using Sulakore.Habbo;
using Sulakore.Crypto;
using Sulakore.Communication;

using Flazzy;

using Eavesdrop;

namespace Tanji.Pages.Connection
{
    public enum TanjiState
    {
        StandingBy = 0,
        ExtractingGameData = 1,
        InjectingClient = 2,
        InterceptingClient = 3,
        DecompressingClient = 4,
        CompressingClient = 5,
        DisassemblingClient = 6,
        ModifyingClient = 7,
        AssemblingClient = 8,
        InterceptingConnection = 9,
        ReplacingResources = 10,
        GeneratingMessageHashes = 11
    }

    public class ConnectionPage : TanjiPage, IReceiver
    {
        private const ushort EAVESDROP_PROXY_PORT = 8282;
        private const string EAVESDROP_ROOT_CERTIFICATE_NAME = "EavesdropRoot.cer";

        private readonly Action<TanjiState> _setState;
        private readonly DirectoryInfo _modifiedClientsDir;
        private readonly Action<Task> _connectTaskCompleted;

        private TanjiState _state;
        public TanjiState State
        {
            get => _state;
            set
            {
                _state = value;
                RaiseOnPropertyChanged(nameof(State));
            }
        }

        private string _customClientPath;
        public string CustomClientPath
        {
            get => _customClientPath;
            set
            {
                _customClientPath = value;
                RaiseOnPropertyChanged(nameof(CustomClientPath));
            }
        }

        public Dictionary<string, string> VariableReplacements { get; }

        public ConnectionPage(MainFrm ui, TabPage tab)
            : base(ui, tab)
        {
            _setState = SetState;
            _connectTaskCompleted = ConnectTaskCompleted;

            Tab.Paint += Tab_Paint;

            UI.CoTVariablesVw.AddItem("productdata.load.url", string.Empty);
            UI.CoTVariablesVw.AddItem("external.texts.txt", string.Empty);
            UI.CoTVariablesVw.AddItem("external.variables.txt", string.Empty);
            UI.CoTVariablesVw.AddItem("external.override.texts.txt", string.Empty);
            UI.CoTVariablesVw.AddItem("external.figurepartlist.txt", string.Empty);
            UI.CoTVariablesVw.AddItem("external.override.variables.txt", string.Empty);

            VariableReplacements = new Dictionary<string, string>(
                UI.CoTVariablesVw.Items.Count);

            UI.CoTCustomClientTxt.DataBindings.Add("Value", this,
                nameof(CustomClientPath), false, DataSourceUpdateMode.OnPropertyChanged);

            UI.CoTBrowseBtn.Click += CoTBrowseBtn_Click;
            UI.CoTConnectBtn.Click += CoTConnectBtn_Click;

            UI.CoTDestroyCertificatesBtn.Click += CoTDestroyCertificatesBtn_Click;
            UI.CoTExportRootCertificateBtn.Click += CoTExportRootCertificateBtn_Click;

            UI.CoTClearVariableBtn.Click += CoTClearVariableBtn_Click;
            UI.CoTUpdateVariableBtn.Click += CoTUpdateVariableBtn_Click;

            UI.CoTVariablesVw.ItemChecked += CoTVariablesVw_ItemChecked;
            UI.CoTVariablesVw.ItemSelected += CoTVariablesVw_ItemSelected;
            UI.CoTVariablesVw.ItemSelectionStateChanged += CoTVariablesVw_ItemSelectionStateChanged;
        }

        private void Tab_Paint(object sender, PaintEventArgs e)
        {
            using (var skin = new Pen(Color.FromArgb(243, 63, 63)))
            {
                e.Graphics.DrawLine(skin, 6, 218, 469, 218);
                e.Graphics.DrawLine(skin, 6, 277, 469, 277);
            }
        }

        private void CoTBrowseBtn_Click(object sender, EventArgs e)
        {
            UI.CustomClientDlg.FileName = string.Empty;
            if (UI.CustomClientDlg.ShowDialog() != DialogResult.OK) return;
            CustomClientPath = UI.CustomClientDlg.FileName;
        }
        private void CoTConnectBtn_Click(object sender, EventArgs e)
        {
            if (State != TanjiState.StandingBy)
            {
                // We only want to cancel the resource replacing at this point,
                // since a connection has already been established.
                if (State == TanjiState.ReplacingResources)
                {
                    Halt();
                    DisableReplacements();
                    SetState(TanjiState.StandingBy);
                }
                else Cancel();
            }
            else
            {
                if (UI.Connection.IsConnected)
                {
                    DialogResult result = MessageBox.Show(
                        "Are you sure you want to disconnect from the current session?\r\nDon't worry, all of your current options/settings will still be intact.",
                        "Tanji ~ Alert!", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);

                    if (result == DialogResult.No)
                    {
                        return;
                    }
                }

                UI.Connection.Disconnect();
                Connect();
            }
        }

        private void CoTClearVariableBtn_Click(object sender, EventArgs e)
        {
            ListViewItem item =
                UI.CoTVariablesVw.SelectedItem;

            item.SubItems[1].Text = string.Empty;
            UI.CoTClearVariableBtn.Enabled = false;
            UI.CoTValueTxt.Value = string.Empty;
            item.Checked = false;
        }
        private void CoTUpdateVariableBtn_Click(object sender, EventArgs e)
        {
            ListViewItem item =
                UI.CoTVariablesVw.SelectedItem;

            item.SubItems[1].Text =
                UI.CoTValueTxt.Value;

            ToggleClearVariableButton(item);

            if (!item.Checked) item.Checked = true;
            else CoTVariablesVw_ItemChecked(this, new ItemCheckedEventArgs(item));
        }

        private void CoTDestroyCertificatesBtn_Click(object sender, EventArgs e)
        {
            DestroySignedCertificates();
        }
        private void CoTExportRootCertificateBtn_Click(object sender, EventArgs e)
        {
            ExportTrustedRootCertificate();
        }

        private void CoTVariablesVw_ItemSelected(object sender, EventArgs e)
        {
            ListViewItem item = UI.CoTVariablesVw.SelectedItem;

            ToggleClearVariableButton(item);
            UI.CoTUpdateVariableBtn.Enabled = true;

            UI.CoTVariableTxt.Value = item.Text;
            UI.CoTValueTxt.Value = item.SubItems[1].Text;
        }
        private void CoTVariablesVw_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            string name = e.Item.Text;
            string value = e.Item.SubItems[1].Text;
            bool updateValue = (e.Item.Checked && !string.IsNullOrWhiteSpace(value));

            if (updateValue) VariableReplacements[name] = value;
            else if (VariableReplacements.ContainsKey(name)) VariableReplacements.Remove(name);
        }
        private void CoTVariablesVw_ItemSelectionStateChanged(object sender, EventArgs e)
        {
            if (!UI.CoTVariablesVw.HasSelectedItem)
            {
                UI.CoTUpdateVariableBtn.Enabled =
                    (UI.CoTClearVariableBtn.Enabled = false);

                UI.CoTVariableTxt.Value =
                   (UI.CoTValueTxt.Value = string.Empty);
            }
        }

        private Task InjectGameClientAsync(object sender, RequestInterceptedEventArgs e)
        {
            if (!e.Uri.Query.StartsWith("?Tanji-")) return null;
            Eavesdropper.RequestInterceptedAsync -= InjectGameClientAsync;

            Uri remoteUrl = e.Request.RequestUri;
            string clientPath = Path.GetFullPath($"Modified Clients/{remoteUrl.Host}/{remoteUrl.LocalPath}");
            if (!string.IsNullOrWhiteSpace(CustomClientPath))
            {
                clientPath = CustomClientPath;
            }
            if (!File.Exists(clientPath))
            {
                SetState(TanjiState.InterceptingClient);
                Eavesdropper.ResponseInterceptedAsync += InterceptGameClientAsync;
            }
            else
            {
                SetState(TanjiState.DisassemblingClient);
                UI.Game = new HGame(clientPath);
                UI.Game.Disassemble();

                if (UI.Game.IsPostShuffle)
                {
                    SetState(TanjiState.GeneratingMessageHashes);
                    UI.Game.GenerateMessageHashes();
                }

                UI.In.Load(UI.Game, "Hashes.ini");
                UI.Out.Load(UI.Game, "Hashes.ini");
                UI.ModulesPg.ModifyGame(UI.Game);

                if (VariableReplacements.Count > 0)
                {
                    Eavesdropper.ResponseInterceptedAsync += ReplaceResourcesAsync;
                }
                else Halt();

                Task interceptConnectionTask = InterceptConnectionAsync();
                e.Request = WebRequest.Create(new Uri(clientPath));
            }
            return null;
        }
        private async Task ReplaceResourcesAsync(object sender, ResponseInterceptedEventArgs e)
        {
            string absoluteUri = e.Response.ResponseUri.AbsoluteUri;
            if (VariableReplacements.ContainsKey(absoluteUri))
            {
                var httpResponse = (HttpWebResponse)e.Response;
                string replacementUrl = VariableReplacements[absoluteUri];

                if (httpResponse.StatusCode == HttpStatusCode.TemporaryRedirect)
                {
                    VariableReplacements.Remove(absoluteUri);
                    absoluteUri = httpResponse.Headers[HttpResponseHeader.Location];
                    VariableReplacements[absoluteUri] = replacementUrl;
                    return;
                }

                if (replacementUrl.StartsWith("http"))
                {
                    using (var webClient = new WebClient())
                    {
                        e.Content = new ByteArrayContent(await webClient.DownloadDataTaskAsync(replacementUrl).ConfigureAwait(false));
                    }
                }
                else e.Content = new ByteArrayContent(File.ReadAllBytes(replacementUrl));

                VariableReplacements.Remove(absoluteUri);
                if (VariableReplacements.Count < 1)
                {
                    Halt();
                    SetState(TanjiState.StandingBy);
                }
            }
        }
        private async Task InterceptGameClientAsync(object sender, ResponseInterceptedEventArgs e)
        {
            if (!e.Uri.Query.StartsWith("?Tanji-")) return;
            if (e.ContentType != "application/x-shockwave-flash") return;
            Eavesdropper.ResponseInterceptedAsync -= InterceptGameClientAsync;

            string clientPath = Path.GetFullPath($"Modified Clients/{e.Uri.Host}/{e.Uri.LocalPath}");
            string clientDirectory = Path.GetDirectoryName(clientPath);
            Directory.CreateDirectory(clientDirectory);

            SetState(TanjiState.DisassemblingClient);
            UI.Game = new HGame(await e.Content.ReadAsByteArrayAsync());
            UI.Game.Location = clientPath;
            UI.Game.Disassemble();

            if (UI.Game.IsPostShuffle)
            {
                SetState(TanjiState.GeneratingMessageHashes);
                UI.Game.GenerateMessageHashes();

                SetState(TanjiState.ModifyingClient);
                UI.Game.DisableHostChecks();
                UI.Game.InjectKeyShouter(4001);
            }

            if (UI.Hotel == HHotel.Unknown)
            {
                ushort infoPort = ushort.Parse(UI.GameData.InfoPort.Split(',')[0]);

                SetState(TanjiState.ModifyingClient);
                UI.Game.InjectEndPoint("127.0.0.1", infoPort);
            }

            UI.In.Load(UI.Game, "Hashes.ini");
            UI.Out.Load(UI.Game, "Hashes.ini");

            // Synchronize Game
            UI.ModulesPg.ModifyGame(UI.Game);

            CompressionKind compression = CompressionKind.ZLIB;
#if DEBUG
            compression = CompressionKind.None;
#endif

            SetState(TanjiState.AssemblingClient);
            byte[] payload = UI.Game.ToArray(compression);

            e.Content = new ByteArrayContent(payload);
            using (var clientStream = File.Open(clientPath, FileMode.Create, FileAccess.Write))
            {
                clientStream.Write(payload, 0, payload.Length);
            }

            if (VariableReplacements.Count > 0)
            {
                Eavesdropper.ResponseInterceptedAsync += ReplaceResourcesAsync;
            }
            else Halt();
            Task interceptConnectionTask = InterceptConnectionAsync();
        }
        private async Task InterceptClientPageAsync(object sender, ResponseInterceptedEventArgs e)
        {
            if (e.Content == null) return;
            if (!e.ContentType.StartsWith("text")) return;

            string body = await e.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (!body.Contains("info.host") && !body.Contains("info.port")) return;

            if (State != TanjiState.ExtractingGameData) return;
            Eavesdropper.ResponseInterceptedAsync -= InterceptClientPageAsync;

            if (e.Response.ContentType != "text/html") return;
            if (State != TanjiState.ExtractingGameData) return;

            byte[] replacementData = Encoding.UTF8.GetBytes(body);
            Eavesdropper.ResponseInterceptedAsync -= InterceptClientPageAsync;

            try
            {
                UI.GameData.Update(body);
                UI.Hotel = SKore.ToHotel(UI.GameData.InfoHost);

                UI.ModulesPg.ModifyGameData(UI.GameData);
                body = UI.GameData.Source;

                body = body.Replace(UI.GameData.InfoHost, "127.0.0.1");
                body = body.Replace(".swf", $".swf?Tanji-{DateTime.Now.Ticks}");

                string[] resourceKeys = VariableReplacements.Keys.ToArray();
                foreach (string variable in resourceKeys)
                {
                    string fakeValue = VariableReplacements[variable];
                    string realValue = UI.GameData[variable].Replace("\\/", "/");

                    VariableReplacements.Remove(variable);
                    VariableReplacements[realValue] = fakeValue;
                }

                e.Content = new StringContent(body);
                SetState(TanjiState.InjectingClient);
                Eavesdropper.RequestInterceptedAsync += InjectGameClientAsync;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Intercepted game data is not recognized as coming from a valid Habbo Hotel site.",
                    "Tanji ~ Alert!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                WriteLog(ex);
                Reset();
            }
        }

        private async Task InterceptConnectionAsync()
        {
            SetState(TanjiState.InterceptingConnection);

            ushort infoPort = ushort.Parse(UI.GameData.InfoPort.Split(',')[0]);
            await UI.Connection.InterceptAsync(UI.GameData.InfoHost, infoPort).ConfigureAwait(false);

            if (UI.Connection.IsConnected)
            {
                if (VariableReplacements.Count > 0)
                {
                    SetState(TanjiState.ReplacingResources);
                }
                else
                {
                    SetState(TanjiState.StandingBy);
                    Halt();
                }
            }
            else Reset();
        }

        public void Halt()
        {
            Eavesdropper.Terminate();
            Eavesdropper.RequestInterceptedAsync -= InjectGameClientAsync;
            Eavesdropper.ResponseInterceptedAsync -= InterceptGameClientAsync;
            Eavesdropper.ResponseInterceptedAsync -= InterceptClientPageAsync;
            Eavesdropper.ResponseInterceptedAsync -= ReplaceResourcesAsync;
        }
        public void Reset()
        {
            Halt();
            DisableReplacements();
            UI.Connection.Disconnect();

            if (UI.Game != null)
            {
                UI.Game.Dispose();
                UI.Game = null;
            }
        }
        public void Cancel()
        {
            Reset();
            SetState(TanjiState.StandingBy);
        }
        public void Connect()
        {
            Eavesdropper.Certifier.CreateTrustedRootCertificate();
            Eavesdropper.ResponseInterceptedAsync += InterceptClientPageAsync;
            Eavesdropper.Initiate(EAVESDROP_PROXY_PORT);

            SetState(TanjiState.ExtractingGameData);
        }
        public void SetState(TanjiState state)
        {
            if (UI.InvokeRequired)
            {
                UI.Invoke(_setState, state);
                return;
            }

            UI.CoTConnectBtn.Text =
                (state == TanjiState.StandingBy ?
                "Connect" : "Cancel");

            #region Switch: state
            switch (State = state)
            {
                case TanjiState.StandingBy:
                UI.CoTStatusTxt.StopDotAnimation("Standing By...");
                break;

                case TanjiState.ExtractingGameData:
                UI.CoTStatusTxt.SetDotAnimation("Extracting Game Data");
                break;

                case TanjiState.InjectingClient:
                UI.CoTStatusTxt.SetDotAnimation("Injecting Client");
                break;

                case TanjiState.InterceptingClient:
                UI.CoTStatusTxt.SetDotAnimation("Intercepting Client");
                break;

                case TanjiState.DecompressingClient:
                UI.CoTStatusTxt.SetDotAnimation("Decompressing Client");
                break;

                case TanjiState.CompressingClient:
                UI.CoTStatusTxt.SetDotAnimation("Compressing Client");
                break;

                case TanjiState.DisassemblingClient:
                UI.CoTStatusTxt.SetDotAnimation("Disassembling Client");
                break;

                case TanjiState.ModifyingClient:
                UI.CoTStatusTxt.SetDotAnimation("Modifying Client");
                break;

                case TanjiState.AssemblingClient:
                UI.CoTStatusTxt.SetDotAnimation("Assembling Client");
                break;

                case TanjiState.InterceptingConnection:
                UI.CoTStatusTxt.SetDotAnimation("Intercepting Connection");
                break;

                case TanjiState.ReplacingResources:
                UI.CoTStatusTxt.SetDotAnimation("Replacing Resources");
                break;

                case TanjiState.GeneratingMessageHashes:
                UI.CoTStatusTxt.SetDotAnimation("Generating Message Hashes");
                break;
            }
            #endregion
        }

        public void DestroySignedCertificates()
        {
            Eavesdropper.Certifier.DestroyCertificates();
            CreateTrustedRootCertificate();
        }
        public void ExportTrustedRootCertificate()
        {
            string certificatePath =
                Path.GetFullPath(EAVESDROP_ROOT_CERTIFICATE_NAME);

            bool exportSuccess = Eavesdropper.Certifier
                .ExportTrustedRootCertificate(certificatePath);

            string message = (exportSuccess
                ? $"Successfully exported '{EAVESDROP_ROOT_CERTIFICATE_NAME}' to:\r\n\r\n" + certificatePath
                : $"Failed to export '{EAVESDROP_ROOT_CERTIFICATE_NAME}' root certificate.");

            MessageBox.Show(message,
                "Tanji ~ Alert!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        public void CreateTrustedRootCertificate()
        {
            UI.BringToFront();
            while (!Eavesdropper.Certifier.CreateTrustedRootCertificate())
            {
                var result = MessageBox.Show(
                    "Eavesdrop requires a self-signed certificate in the root store to intercept HTTPS traffic.\r\n\r\nWould you like to retry the process?",
                    "Tanji ~ Alert!", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);

                if (result != DialogResult.Yes)
                {
                    UI.Close();
                    return;
                }
            }
            UI.BringToFront();
        }

        protected void DisableReplacements()
        {
            foreach (ListViewItem item in UI.CoTVariablesVw.Items)
            {
                item.Checked = false;
            }
        }
        protected void ToggleClearVariableButton(ListViewItem item)
        {
            UI.CoTClearVariableBtn.Enabled = (!string.IsNullOrWhiteSpace(item.SubItems[1].Text));
        }
        protected virtual void ConnectTaskCompleted(Task connectTask)
        {
            if (UI.Connection.IsConnected)
            {
                if (VariableReplacements.Count > 0)
                {
                    SetState(TanjiState.ReplacingResources);
                    return;
                }
            }
            Halt();
            SetState(TanjiState.StandingBy);
        }

        protected override void OnTabSelecting(TabControlCancelEventArgs e)
        {
            if (!UI.Connection.IsConnected)
            {
                UI.TopMost = true;
            }
            base.OnTabSelecting(e);
        }
        protected override void OnTabDeselecting(TabControlCancelEventArgs e)
        {
            UI.TopMost = UI.PacketLoggerUI.TopMost;
            base.OnTabDeselecting(e);
        }

        #region IReceiver Implementation
        public bool IsReceiving { get; set; }
        public void HandleOutgoing(DataInterceptedEventArgs e)
        {
            if (e.Packet.Header == 4001)
            {
                string sharedKeyHex = e.Packet.ReadString();
                if (sharedKeyHex.Length % 2 != 0)
                {
                    sharedKeyHex = ("0" + sharedKeyHex);
                }

                byte[] sharedKey = Enumerable.Range(0, sharedKeyHex.Length / 2)
                    .Select(x => Convert.ToByte(sharedKeyHex.Substring(x * 2, 2), 16))
                    .ToArray();

                UI.Connection.Remote.Encrypter = new RC4(sharedKey);
                UI.Connection.Remote.IsEncrypting = true;

                e.IsBlocked = true;
                IsReceiving = false;
            }
            else if (e.Step >= 10)
            {
                IsReceiving = false;
            }
        }
        public void HandleIncoming(DataInterceptedEventArgs e)
        { }
        #endregion
    }
}