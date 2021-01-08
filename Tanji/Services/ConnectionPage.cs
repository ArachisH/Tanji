using System;
using System.IO;
using System.Net;
using System.Text;
using System.Buffers;
using System.Net.Http;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;

using Tanji.Habbo;
using Tanji.Network;
using Tanji.Controls;

using Sulakore.Habbo;
using Sulakore.Network;
using Sulakore.Cryptography.Ciphers;

using Eavesdrop;

namespace Tanji.Services
{
    [ToolboxItem(true)]
    [DesignerCategory("UserControl")]
    public partial class ConnectionPage : NotifiablePage, IHaltable, IReceiver
    {
        private static readonly string _sessionId;

        #region Status Constants
        private const string STANDING_BY = "Standing By...";

        private const string INTERCEPTING_RESOURCES = "Intercepting Resources...";
        private const string INTERCEPTING_CONNECTION = "Intercepting Connection...";
        private const string INTERCEPTING_CLIENT_LOADER = "Intercepting Client Loader...";

        private const string PATCHING_CLIENT = "Patching Client...";
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

        static ConnectionPage()
        {
            _sessionId = Convert.ToHexString(Guid.NewGuid().ToByteArray()).ToLowerInvariant();
        }
        public ConnectionPage()
        {
            InitializeComponent();

            Bind(StatusTxt, "Text", nameof(Status));
            Bind(CustomClientTxt, "Text", nameof(CustomClientPath));
        }

        private Task InterceptRequestResourcesAsync(object sender, RequestInterceptedEventArgs e)
        {
            if (!e.Uri.Query.Contains(_sessionId) && CanModifyClientPage(e.Uri.DnsSafeHost)) return Task.CompletedTask;

            string resourceName = e.Uri.Segments[^1];
            string resourcePath = Path.GetFullPath($"Cache/{e.Uri.Host}/{e.Uri.LocalPath}");

            bool isGlobalProdData = resourceName.Equals("habbo2020-global-prod.data.unityweb", StringComparison.InvariantCultureIgnoreCase);
            bool isGlobalProdWasmCode = resourceName.Equals("habbo2020-global-prod.wasm.code.unityweb", StringComparison.InvariantCultureIgnoreCase);
            if (File.Exists(resourcePath))
            {
                if (!isGlobalProdData)
                {
                    if (!string.IsNullOrWhiteSpace(CustomClientPath))
                    {
                        resourcePath = CustomClientPath;
                    }

                    if (!isGlobalProdWasmCode)
                    {
                        var flash = new FlashGame(resourcePath);
                        Status = DISASSEMBLING_CLIENT;
                        flash.Disassemble();

                        if (flash.IsPostShuffle)
                        {
                            Status = GENERATING_MESSAGE_HASHES;
                            flash.GenerateMessageHashes("Messages.ini");
                        }
                        Master.Game = flash;
                        TerminateProxy();
                    }
                    else
                    {
                        var unity = new UnityGame(null, resourcePath, e.Uri.Segments[2][0..^1]);
                        unity.LoadMessagesInformation("Messages.ini");
                        Master.Game = unity;
                    }
                    _ = InterceptConnectionAsync();
                }
                e.Request = WebRequest.Create(new Uri(resourcePath));
            }
            else
            {
                if (isGlobalProdData)
                {
                    HttpWebRequest buraksRequest = StripInterceptionQuery((HttpWebRequest)e.Request, "https://jxz.be"); // Thanks Burak!
                    buraksRequest.Headers["Initiator"] = e.Uri.Host;
                    buraksRequest.Headers["From"] = "Tanji";
                    buraksRequest.Host = "jxz.be";
                    e.Request = buraksRequest;
                }
                else e.Request = StripInterceptionQuery((HttpWebRequest)e.Request);
            }
            return Task.CompletedTask;
        }
        private async Task InterceptClientLoaderAsync(object sender, ResponseInterceptedEventArgs e)
        {
            if (!TryGetContentTypes(e, out bool isText, out bool isJavascript, out _, out _, out _)) return;
            if (!isText && !isJavascript) return;

            byte[] replacement = null;
            bool clientLoaderDetected = false;
            string body = await e.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (!isJavascript && HasTriggers(body, Master.Config.UnityInterceptionTriggers)) // Unity Client
            {
                string globalProdJsonUrl = GetInnerString(body, "UnityLoader.instantiate(\"unityContainer\", \"", "\"");
                string revision = GetInnerString(body, "habbo-webgl-clients/", "/");

                body = body.Replace(globalProdJsonUrl, $"{globalProdJsonUrl}?{revision}_{_sessionId}"); // Force a non-cached version of this client, unless we've already returns this modified file at least once during this session.
                replacement = Encoding.UTF8.GetBytes(body);

                clientLoaderDetected = true;
                HotelServer = HotelEndPoint.Create(e.Uri.DnsSafeHost);
            }
            else if (HasTriggers(body, Master.Config.FlashInterceptionTriggers, 2)) // Flash Client
            {
                int swfStartIndex = GetSWFStartIndex(body, 0);
                if (swfStartIndex == -1) return;

                clientLoaderDetected = true;
                if (CanModifyClientPage(e.Uri.DnsSafeHost))
                {
                    do
                    {
                        if (body[swfStartIndex++] == ')') continue;
                        var embedSWFEnd = body.IndexOf(',', swfStartIndex);

                        if (embedSWFEnd == -1) break;
                        body = body.Insert(embedSWFEnd, $"+\"?{_sessionId}\"");
                    }
                    while ((swfStartIndex = GetSWFStartIndex(body, swfStartIndex)) != -1);
                    replacement = Encoding.UTF8.GetBytes(body);
                }
            }

            if (clientLoaderDetected)
            {
                Eavesdropper.ResponseInterceptedAsync -= InterceptClientLoaderAsync;
                Eavesdropper.RequestInterceptedAsync += InterceptRequestResourcesAsync;
                Eavesdropper.ResponseInterceptedAsync += InterceptResponseResourcesAsync;
                Status = INTERCEPTING_RESOURCES;
            }

            if (replacement != null)
            {
                e.Content = new ByteArrayContent(replacement);
                e.Headers[HttpResponseHeader.ContentLength] = replacement.Length.ToString();
            }
        }
        private async Task InterceptResponseResourcesAsync(object sender, ResponseInterceptedEventArgs e)
        {
            if (!TryGetContentTypes(e, out _, out _, out bool isShockwaveFlash, out bool isJson, out bool isUnityWeb)) return;
            if (!isShockwaveFlash && !isJson && !isUnityWeb) return;

            string revision = null;
            byte[] replacement = null;
            bool isStoringLocally = true;
            string resourceName = e.Uri.Segments[^1];
            string resourcePath = Path.GetFullPath($"Cache/{e.Uri.Host}/{e.Uri.LocalPath}");
            if (isJson && resourceName == "habbo2020-global-prod.json")
            {
                isStoringLocally = false;
                revision = e.Uri.Segments[2][0..^1];
                string body = await e.Content.ReadAsStringAsync().ConfigureAwait(false);

                body = body.Replace(".unityweb", $".unityweb?{revision}_{_sessionId}.unityweb");
                replacement = Encoding.UTF8.GetBytes(body);
            }
            else if (isUnityWeb)
            {
                revision = e.Uri.Segments[2][0..^1];
                switch (resourceName)
                {
                    case "habbo2020-global-prod.data.unityweb":
                    {
                        replacement = await e.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                        if (e.Uri.Host.Equals("jxz.be", StringComparison.InvariantCultureIgnoreCase))
                        {
                            resourcePath = Path.GetFullPath($"Cache/{e.Request.Headers["Initiator"]}/{e.Uri.LocalPath}");
                        }
                        break;
                    }
                    case "habbo2020-global-prod.wasm.code.unityweb":
                    {
                        replacement = await e.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                        var unity = new UnityGame(replacement, resourcePath, revision);

                        Status = DISASSEMBLING_CLIENT;
                        unity.Disassemble();
                        unity.LoadMessagesInformation("Messages.ini");

                        Status = PATCHING_CLIENT;
                        unity.Patch();

                        Status = ASSEMBLING_CLIENT;
                        replacement = unity.ToArray();

                        Master.Game = unity;
                        _ = InterceptConnectionAsync();
                        break;
                    }
                    case "habbo2020-global-prod.wasm.framework.unityweb":
                    {
                        isStoringLocally = false;
                        string body = await e.Content.ReadAsStringAsync().ConfigureAwait(false);
                        body = body.Replace("new WebSocket(instance.url);", $"new WebSocket(\"ws://localhost:{Master.Config.GameListenPort}/websocket\");");
                        replacement = Encoding.UTF8.GetBytes(body);
                        break;
                    }
                }
            }
            else if (isShockwaveFlash)
            {
                replacement = await e.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                var flash = new FlashGame(resourcePath, replacement);

                Status = DISASSEMBLING_CLIENT;
                flash.Disassemble();

                if (flash.IsPostShuffle)
                {
                    Status = GENERATING_MESSAGE_HASHES;
                    flash.GenerateMessageHashes("Messages.ini");
                }

                Status = PATCHING_CLIENT;
                flash.Patch(flash.IsPostShuffle ? 4000 : 206, "127.0.0.1", Master.Config.GameListenPort);

                Flazzy.CompressionKind compression = Flazzy.CompressionKind.ZLIB;
#if DEBUG
                compression = Flazzy.CompressionKind.None;
#endif

                Status = ASSEMBLING_CLIENT;
                replacement = flash.ToArray(compression);

                Master.Game = flash;

                TerminateProxy();
                _ = InterceptConnectionAsync();
            }

            if (replacement != null)
            {
                if (isStoringLocally)
                {
#if DEBUG
                    Directory.CreateDirectory(Path.GetDirectoryName(resourcePath));
                    using FileStream localCacheFileStream = File.Open(resourcePath, FileMode.Create, FileAccess.Write);
                    localCacheFileStream.Write(replacement, 0, replacement.Length);
#endif
                }
                e.Content = new ByteArrayContent(replacement);
                e.Headers[HttpResponseHeader.ContentLength] = replacement.Length.ToString();
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
                Eavesdropper.ResponseInterceptedAsync += InterceptClientLoaderAsync;
                Eavesdropper.Initiate(Master.Config.ProxyListenPort);
                Status = INTERCEPTING_CLIENT_LOADER;
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
            Eavesdropper.ResponseInterceptedAsync -= InterceptClientLoaderAsync;
            Eavesdropper.RequestInterceptedAsync -= InterceptRequestResourcesAsync;
            Eavesdropper.ResponseInterceptedAsync -= InterceptResponseResourcesAsync;
        }
        private async Task InterceptConnectionAsync()
        {
            if (Master.Game.IsPostShuffle && Master.Game.HasPingInstructions)
            {
                Master.Connection.SocketSkip = 2;
            }
            Master.Game.Dispose(); // This will only kill the objects that are no longer needed. (In/Out will stay alive)

            Status = INTERCEPTING_CONNECTION;
            await Master.Connection.InterceptAsync(HotelServer).ConfigureAwait(false);

            TerminateProxy(); // Ensure the local proxy has been terminated.
            Status = STANDING_BY;
        }

        private static bool CanModifyClientPage(string domain)
        {
            return HotelEndPoint.GetHotel(domain) != HHotel.Unknown || Master.Config.IsModifyingRetroClientLoaders;
        }
        private static string GetInnerString(string body, string left, string right)
        {
            int leftStartIndex = body.IndexOf(left);
            if (leftStartIndex == -1) return null;

            int innerStartIndex = leftStartIndex + left.Length;

            int rightStartIndex = body.IndexOf(right, innerStartIndex);
            if (rightStartIndex == -1) return null;

            return body[innerStartIndex..rightStartIndex];
        }
        private static bool TryGetContentTypes(ResponseInterceptedEventArgs e, out bool isText, out bool isJavascript, out bool isShockwaveFlash, out bool isJson, out bool isUnityWeb)
        {
            string contentType = e.ContentType?.ToLower();
            isText = contentType?.Contains("text") ?? false;
            isJson = contentType?.Contains("json") ?? false;
            isUnityWeb = contentType?.Contains("vnd.unity") ?? false;
            isJavascript = contentType?.Contains("javascript") ?? false;
            isShockwaveFlash = contentType?.Contains("x-shockwave-flash") ?? false;
            return e.Content != null;
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
        private static bool HasTriggers(string body, IList<string> triggers, int minimum = 0)
        {
            if (minimum < 1)
            {
                minimum = triggers.Count;
            }

            int triggersFound = 0;
            foreach (string trigger in triggers)
            {
                if (!body.Contains(trigger, StringComparison.InvariantCultureIgnoreCase)) continue;
                triggersFound++;
            }
            return triggersFound >= minimum;
        }
        private static HttpWebRequest StripInterceptionQuery(HttpWebRequest request, string authority = null)
        {
            string strippedUrl = (authority ?? request.RequestUri.GetLeftPart(UriPartial.Authority)) + request.RequestUri.AbsolutePath;
            HttpWebRequest strippedQueryRequest = WebRequest.CreateHttp(strippedUrl);
            strippedQueryRequest.AllowAutoRedirect = request.AllowAutoRedirect;
            strippedQueryRequest.IfModifiedSince = request.IfModifiedSince;
            strippedQueryRequest.ProtocolVersion = request.ProtocolVersion;
            strippedQueryRequest.CookieContainer = request.CookieContainer;
            strippedQueryRequest.ContentType = request.ContentType;
            strippedQueryRequest.KeepAlive = request.KeepAlive;
            strippedQueryRequest.UserAgent = request.UserAgent;
            strippedQueryRequest.Referer = request.Referer;
            strippedQueryRequest.Accept = request.Accept;
            strippedQueryRequest.Method = request.Method;
            strippedQueryRequest.Proxy = request.Proxy;
            strippedQueryRequest.Host = request.Host;

            if (request.ContentLength > 0)
            {
                strippedQueryRequest.ContentLength = request.ContentLength;
            }
            foreach (string header in request.Headers.Keys)
            {
                switch (header.ToLower())
                {
                    case "range":
                    case "expect":
                    case "host":
                    case "accept":
                    case "cookie":
                    case "referer":
                    case "keep-alive":
                    case "connection":
                    case "user-agent":
                    case "content-type":
                    case "content-length":
                    case "proxy-connection":
                    case "if-modified-since": break;
                    default: strippedQueryRequest.Headers[header] = request.Headers[header]; break;
                }
            }
            return strippedQueryRequest;
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
        private byte[] _nonce;
        private Task _initializeStreamCiphersTask;

        [Browsable(false)]
        public bool IsReceiving { get; set; }

        [Browsable(false)]
        public bool IsIncomingEncrypted { get; private set; }

        public void HandleOutgoing(DataInterceptedEventArgs e)
        {
            if (Master.Game.IsUnity)
            {
                if (e.Packet.Id == Master.Out.Hello)
                {
                    string nonce = string.Empty;
                    string hex = e.Packet.ReadUTF8(0);
                    for (var i = 0; i < 8; i++)
                    {
                        nonce += hex.Substring(i * 3, 2);
                    }
                    _nonce = Convert.FromHexString(nonce);
                }
                else if (e.Packet.Id == Master.Out.CompleteDhHandshake)
                {
                    e.WaitUntil = _initializeStreamCiphersTask = InitializeStreamCiphersAsync();
                    IsReceiving = false;
                }
            }
            else if (e.Packet.Id == 4002 && Master.Game.IsPostShuffle)
            {
                string sharedKeyHex = e.Packet.ReadUTF8();
                if (sharedKeyHex.Length % 2 != 0)
                {
                    sharedKeyHex = "0" + sharedKeyHex;
                }
                byte[] sharedKey = Convert.FromHexString(sharedKeyHex);

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
            if (Master.Game.IsUnity && e.Packet.Id == Master.In.DhCompleteHandshake)
            {
                e.WaitUntil = _initializeStreamCiphersTask;
            }
            else if (!Master.Game.IsUnity && (e.Step == 2 || e.Packet.Id == Master.In.CompleteDiffieHandshake) && (Master.Game?.IsPostShuffle ?? false))
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