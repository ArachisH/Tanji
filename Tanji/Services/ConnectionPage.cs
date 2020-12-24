using System;
using System.IO;
using System.Net;
using System.Text;
using System.Linq;
using System.Buffers;
using System.Net.Http;
using System.Text.Json;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;

using Tanji.Network;
using Tanji.Controls;

using Sulakore.Network;
using Sulakore.Habbo.Web;
using Sulakore.Habbo.Messages;
using Sulakore.Cryptography.Ciphers;

using Eavesdrop;

using Flazzy;

using Wazzy;
using Wazzy.Types;
using Wazzy.Bytecode;
using Wazzy.Sections.Subsections;
using Wazzy.Bytecode.Instructions.Numeric;
using Wazzy.Bytecode.Instructions.Variable;
using Wazzy.Bytecode.Instructions.Control;
using Wazzy.Bytecode.Instructions.Parametric;

namespace Tanji.Services
{
    [ToolboxItem(true)]
    [DesignerCategory("UserControl")]
    public partial class ConnectionPage : NotifiablePage, IHaltable, IReceiver
    {
        const string USEr_JSON_END = ";window.geoLocation";
        const string USER_JSON_START = "<script>window.session=";

        private static readonly JsonSerializerOptions _userSerializerOptions;

        private byte[] _nonce;
        private Guid _randomQuery;
        private bool _wasBlacklisted;
        private int _unhandledUnityAssets;

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

        public bool IsH2020 => Master.GameData.User != null;

        static ConnectionPage()
        {
            _userSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            _userSerializerOptions.Converters.Add(new Sulakore.Habbo.Web.Json.DateTimeConverter());
        }
        public ConnectionPage()
        {
            InitializeComponent();

            Bind(StatusTxt, "Text", nameof(Status));
            Bind(CustomClientTxt, "Text", nameof(CustomClientPath));
        }

        private Task InjectResourceAsync(object sender, RequestInterceptedEventArgs e)
        {
            if (!_wasBlacklisted && !e.Uri.Query.Contains(_randomQuery.ToString())) return null;

            string resourcePath = Path.GetFullPath($"Cache/{e.Uri.Host}/{e.Uri.LocalPath}");
            if ((_wasBlacklisted || IsH2020) && !File.Exists(resourcePath)) return null;

            if (!IsH2020 && !string.IsNullOrWhiteSpace(CustomClientPath))
            {
                resourcePath = CustomClientPath;
            }

            if (File.Exists(resourcePath))
            {
                if (!IsH2020)
                {
                    Status = DISASSEMBLING_CLIENT;
                    Master.Game = new HGame(resourcePath);
                    Master.Game.Disassemble();

                    if (Master.Game.IsPostShuffle)
                    {
                        Status = GENERATING_MESSAGE_HASHES;
                        Master.Game.GenerateMessageHashes("Hashes.ini");
                    }
                }

                if (!IsH2020 || --_unhandledUnityAssets == 0)
                {
                    TerminateProxy();
                    _ = InterceptConnectionAsync();
                }
                e.Request = WebRequest.Create(new Uri(resourcePath));
            }
            else if (!IsH2020)
            {
                Status = INTERCEPTING_CLIENT;
                Eavesdropper.ResponseInterceptedAsync += InterceptResourceAsync;
            }
            return null;
        }
        private async Task InterceptResourceAsync(object sender, ResponseInterceptedEventArgs e)
        {
            if (!IsH2020 && e.ContentType != "application/x-shockwave-flash") return;
            if (!_wasBlacklisted && !e.Uri.Query.Contains(_randomQuery.ToString())) return;

            string clientPath = Path.GetFullPath($"Cache/{e.Uri.Host}/{e.Uri.LocalPath}");
            string clientDirectory = Path.GetDirectoryName(clientPath);

            if (IsH2020)
            {
                byte[] replacement = null;
                string resourceName = e.Uri.Segments[^1];
                switch (resourceName)
                {
                    case "habbo2020-global-prod.data.unityweb":
                    {
                        // TODO: Download from external location.
                        replacement = await e.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                        break;
                    }
                    case "habbo2020-global-prod.wasm.code.unityweb":
                    {
                        replacement = await e.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                        replacement = InjectKeyShouter(replacement);
                        break;
                    }
                    case "habbo2020-global-prod.wasm.framework.unityweb":
                    {
                        string body = await e.Content.ReadAsStringAsync().ConfigureAwait(false);
                        body = body.Replace("new WebSocket(instance.url);", $"new WebSocket(\"ws://localhost:{Master.Config.GameListenPort}/websocket\");");
                        replacement = Encoding.UTF8.GetBytes(body);
                        break;
                    }
                }

                if (replacement != null)
                {
                    e.Content = new ByteArrayContent(replacement);
                    e.Headers[HttpResponseHeader.ContentLength] = replacement.Length.ToString();
                    using (var cacheStream = File.Open(clientPath, FileMode.Create, FileAccess.Write))
                    {
                        cacheStream.Write(replacement, 0, replacement.Length);
                    }
                }
                --_unhandledUnityAssets;
            }
            else
            {
                byte[] payload = await e.Content.ReadAsByteArrayAsync().ConfigureAwait(false);

                HGame game = null;
                if (_wasBlacklisted && !IsGameClient(payload, out game)) return;
                Directory.CreateDirectory(clientDirectory);

                if (!_wasBlacklisted)
                {
                    Status = DISASSEMBLING_CLIENT;
                    game = new HGame(payload) { Location = clientPath };
                    game.Disassemble();

                    if (game.IsPostShuffle)
                    {
                        Status = GENERATING_MESSAGE_HASHES;
                        game.GenerateMessageHashes("Hashes.ini");

                        Status = MODIFYING_CLIENT;
                        game.DisableHostChecks();
                        game.InjectKeyShouter(4001);
                    }
                    game.InjectEndPointShouter(game.IsPostShuffle ? 4000 : 206);
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
            }

            if (!IsH2020 || _unhandledUnityAssets == 0)
            {
                TerminateProxy();
                Task interceptConnectionTask = InterceptConnectionAsync();
            }
        }
        private async Task InterceptClientPageAsync(object sender, ResponseInterceptedEventArgs e)
        {
            if (e.Content == null) return;
            string contentType = e.ContentType.ToLower();
            bool hasText = contentType.Contains("text");
            bool hasJson = contentType.Contains("json");
            bool hasJavascript = contentType.Contains("javascript");
            if (!hasText && !hasJson && !hasJavascript) return;

            string body = await e.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (!IsH2020 && (hasText || hasJavascript))
            {
                int userTriggersFound = GetTriggersCount(Master.Config.UserInterceptionTriggers, body);
                bool hasUserJson = userTriggersFound == Master.Config.UserInterceptionTriggers.Count;

                if (hasUserJson)
                {
                    int offset = 0;
                    int jsonStart = body.IndexOf(USER_JSON_START) + USER_JSON_START.Length + offset;
                    int jsonLength = body.IndexOf(USEr_JSON_END) - jsonStart - offset;

                    string userJson = body.Substring(jsonStart, jsonLength);
                    Master.GameData = new HGameData(JsonSerializer.Deserialize<HUser>(userJson, _userSerializerOptions));
                }
                if (hasUserJson || GetTriggersCount(Master.Config.FlashInterceptionTriggers, body) < 2) return; // Otherwise, continue executing code below.
            }
            else if (IsH2020 && hasJson && GetTriggersCount(Master.Config.WASMInterceptionTriggers, body) >= 3)
            {
                body = body.Replace(".unityweb", $".unityweb?{_randomQuery = Guid.NewGuid()}.unityweb");

                e.Content = new StringContent(body);
                e.Headers[HttpResponseHeader.ContentLength] = body.Length.ToString();

                _unhandledUnityAssets = 3;
            }
            else return;

            int swfStartIndex = !IsH2020 ? GetSWFStartIndex(body) : -1;
            bool isBlacklisted = _wasBlacklisted = Master.Config.CacheBlacklist.Contains(e.Uri.Host);
            if (!IsH2020 && swfStartIndex == -1 && !isBlacklisted) return;

            Eavesdropper.ResponseInterceptedAsync -= InterceptClientPageAsync;
            Master.GameData.Source = body;

            if (!IsH2020 && !isBlacklisted)
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
                e.Headers[HttpResponseHeader.ContentLength] = body.Length.ToString();

                Status = INJECTING_CLIENT;
                Eavesdropper.RequestInterceptedAsync += InjectResourceAsync;
            }
            else
            {
                Status = INTERCEPTING_CLIENT_REQUEST_RESPONSE;
                Eavesdropper.RequestInterceptedAsync += InjectResourceAsync;
                Eavesdropper.ResponseInterceptedAsync += InterceptResourceAsync;
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
            Eavesdropper.RequestInterceptedAsync -= InjectResourceAsync;
            Eavesdropper.ResponseInterceptedAsync -= InterceptClientPageAsync;
            Eavesdropper.ResponseInterceptedAsync -= InterceptResourceAsync;
            Eavesdropper.Terminate();
        }
        private async Task InterceptConnectionAsync()
        {
            Status = INTERCEPTING_CONNECTION;
            if (!IsH2020 && Master.Game.IsPostShuffle && Master.Game.HasPingInstructions)
            {
                Master.Connection.SocketSkip = 2;
            }

            if (!IsH2020)
            {
                Master.Game.Dispose();
                foreach (HMessage message in Master.Out.Concat(Master.In))
                {
                    message.Class = null;
                    message.Parser = null;
                    message.References.Clear();
                }
            }
            else
            {
                Master.Connection.SocketSkip = 0;
                HotelServer = HotelEndPoint.Parse($"game-{Master.GameData.User.UniqueId.Substring(2, 2)}.habbo.com", 30001);
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
                }
                if (!possibleGame.InjectEndPointShouter(possibleGame.IsPostShuffle ? 4000 : 206)) return false;
                possibleGame.InjectEndPoint("127.0.0.1", Master.Config.GameListenPort);
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

        private static byte[] InjectKeyShouter(byte[] contentBytes)
        {
            var module = new WASMModule(contentBytes);
            module.Disassemble();
            for (int i = 0; i < module.CodeSec.Count; i++)
            {
                // Begin searching for the ChaChaEngine.SetKey method.
                var funcTypeIndex = (int)module.FunctionSec[i];
                FuncType functionType = module.TypeSec[funcTypeIndex];
                CodeSubsection codeSubSec = module.CodeSec[i];

                if (codeSubSec.Locals.Count != 1) continue;
                if (functionType.ParameterTypes.Count != 4) continue;

                bool hasValidParamTypes = true;
                for (int j = 0; j < functionType.ParameterTypes.Count; j++)
                {
                    if (functionType.ParameterTypes[j] == typeof(int)) continue;
                    hasValidParamTypes = false;
                    break;
                }
                if (!hasValidParamTypes) continue; // If all of the parameters are not of type int.

                if (codeSubSec.Expression[0].OP != OPCode.ConstantI32) continue;
                if (codeSubSec.Expression[1].OP != OPCode.LoadI32_8S) continue;
                if (codeSubSec.Expression[2].OP != OPCode.EqualZeroI32) continue;
                if (codeSubSec.Expression[3].OP != OPCode.If) continue;

                // Dig through the block/branching expressions
                var expandedInstructions = WASMInstruction.ConcatNestedExpressions(codeSubSec.Expression).ToArray();
                for (int j = 0, k = expandedInstructions.Length - 2; j < expandedInstructions.Length; j++)
                {
                    WASMInstruction instruction = expandedInstructions[j];
                    if (instruction.OP != OPCode.ConstantI32) continue;

                    var constanti32Ins = (ConstantI32Ins)instruction;
                    if (constanti32Ins.Constant != 12) continue;

                    if (expandedInstructions[++j].OP != OPCode.AddI32) continue;
                    if (expandedInstructions[++j].OP != OPCode.TeeLocal) continue;
                    if (expandedInstructions[++j].OP != OPCode.LoadI32) continue;
                    if (expandedInstructions[++j].OP != OPCode.ConstantI32) continue;
                    if (expandedInstructions[++j].OP != OPCode.SubtractI32) continue;

                    if (expandedInstructions[k--].OP != OPCode.Call) continue;
                    if (expandedInstructions[k--].OP != OPCode.ConstantI32) continue;
                    if (expandedInstructions[k--].OP != OPCode.ConstantI32) continue;
                    if (expandedInstructions[k--].OP != OPCode.ConstantI32) continue;

                    codeSubSec.Expression.InsertRange(0, new WASMInstruction[]
                    {
                        new ConstantI32Ins(0),      // WebSocket Instance Id
                        new GetLocalIns(1),         // Key Pointer
                        new ConstantI32Ins(48),     // Key Length
                        new CallIns(126),           // _WebSocketSend
                        new DropIns(),
                    });
                    return module.ToArray();
                }
            }
            return null;
        }
        private static int GetSWFStartIndex(string body, int index = 0)
        {
            int swfStartIndex = body.IndexOf("embedswf(", index, StringComparison.OrdinalIgnoreCase) + 9;
            if (swfStartIndex == 8)
            {
                swfStartIndex = body.IndexOf("swfobject(", index, StringComparison.OrdinalIgnoreCase) + 10;
                if (swfStartIndex == 9) return -1;
            }
            return swfStartIndex;
        }
        private static int GetTriggersCount(IList<string> triggers, string body)
        {
            int triggersFound = 0;
            foreach (string trigger in triggers)
            {
                if (!body.Contains(trigger, StringComparison.CurrentCulture)) continue;
                triggersFound++;
            }
            return triggersFound;
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
        private Task _initializeStreamCiphersTask;

        [Browsable(false)]
        public bool IsReceiving { get; set; }

        [Browsable(false)]
        public bool IsIncomingEncrypted { get; private set; }

        public void HandleOutgoing(DataInterceptedEventArgs e)
        {
            if (IsH2020)
            {
                if (e.Packet.Id == 4000)
                {
                    string nonce = string.Empty;
                    string hex = e.Packet.ReadUTF8(0);
                    for (var i = 0; i < 8; i++)
                    {
                        nonce += hex.Substring(i * 3, 2);
                    }
                    _nonce = Convert.FromHexString(nonce);
                }
                else if (e.Packet.Id == 208)
                {
                    e.WaitUntil = _initializeStreamCiphersTask = InitializeStreamCiphersAsync();
                }
            }
            else if (e.Packet.Id == 4001 && Master.Game.IsPostShuffle)
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
                if (IsIncomingEncrypted)
                {
                    Master.Connection.Remote.Decrypter = new RC4(sharedKey);
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
            if (IsH2020 && e.Packet.Id == 279)
            {
                e.WaitUntil = _initializeStreamCiphersTask;
            }
            else if ((e.Step == 2 || e.Packet.Id == Master.In?.CompleteDiffieHandshake) && (Master.Game?.IsPostShuffle ?? false))
            {
                e.Packet.ReadUTF8();
                if (e.Packet.ReadableBytes > 0)
                {
                    IsIncomingEncrypted = e.Packet.ReadBoolean();
                    e.Packet.Replace(false, e.Packet.Position - 1);
                }
            }
        }

        private async Task InitializeStreamCiphersAsync()
        {
            Master.Connection.Local.BypassReceiveSecureTunnel = 2;
            using IMemoryOwner<byte> key = MemoryPool<byte>.Shared.Rent(48);

            int received = await Master.Connection.Local.ReceiveAsync(key.Memory).ConfigureAwait(false);
            received = await Master.Connection.Local.ReceiveAsync(key.Memory).ConfigureAwait(false);

            Memory<byte> keyRegion = key.Memory.Slice(16, 32);
            InitializeChaChaInstances(keyRegion.Span);
        }
        private void InitializeChaChaInstances(ReadOnlySpan<byte> key)
        {
            Master.Connection.Local.Encrypter = new ChaCha20(key, _nonce);
            Master.Connection.Local.Decrypter = new ChaCha20(key, _nonce);

            Master.Connection.Remote.Encrypter = new ChaCha20(key, _nonce);
            Master.Connection.Remote.Decrypter = new ChaCha20(key, _nonce);
        }
        #endregion
    }
}