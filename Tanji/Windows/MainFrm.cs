using System;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Tanji.Network;
using Tanji.Utilities;
using Tanji.Properties;
using Tanji.Components;
using Tanji.Pages.About;
using Tanji.Manipulators;
using Tanji.Pages.Modules;
using Tanji.Pages.Toolbox;
using Tanji.Pages.Injection;
using Tanji.Pages.Connection;

using Sulakore;
using Sulakore.Habbo;
using Sulakore.Protocol;
using Sulakore.Habbo.Web;
using Sulakore.Communication;
using Sulakore.Habbo.Messages;

using Eavesdrop;

namespace Tanji.Windows
{
    [DesignerCategory("Form")]
    public partial class MainFrm : ObservableForm
    {
        private DateTime _latencyTestStart;
        private readonly List<IHaltable> _haltables;
        private readonly List<IReceiver> _receivers;
        private readonly EventHandler _disconnected;
        private readonly Dictionary<Keys, Action> _actions;
        private readonly Dictionary<string, Bitmap> _avatarCache;
        private readonly EventHandler<ConnectedEventArgs> _connected;
        private readonly Dictionary<HHotel, Dictionary<string, HUser>> _userCache;

        public KeyboardHook Hook { get; }

        public Incoming In { get; set; }
        public Outgoing Out { get; set; }

        public HGame Game { get; set; }
        public HGameData GameData { get; set; }
        public HConnection Connection { get; set; }

        public AboutPage AboutPg { get; }
        public ModulesPage ModulesPg { get; }
        public ToolboxPage ToolboxPg { get; }
        public InjectionPage InjectionPg { get; }
        public ConnectionPage ConnectionPg { get; }

        public PacketLoggerFrm PacketLoggerUI { get; }

        public MainFrm()
        {
            InitializeComponent();

            _connected = Connected;
            _disconnected = Disconnected;
            _haltables = new List<IHaltable>();
            _receivers = new List<IReceiver>();
            _actions = new Dictionary<Keys, Action>();
            _avatarCache = new Dictionary<string, Bitmap>();
            _userCache = new Dictionary<HHotel, Dictionary<string, HUser>>();

            In = new Incoming();
            Out = new Outgoing();
            GameData = new HGameData();
            Connection = new HConnection();
            Connection.Connected += Connected;
            Connection.DataOutgoing += HandleData;
            Connection.DataIncoming += HandleData;
            Connection.Disconnected += Disconnected;

            Hook = new KeyboardHook();
            Hook.HotkeyActivated += Hook_HotkeyActivated;

            ConnectionPg = new ConnectionPage(this, ConnectionTab);
            InjectionPg = new InjectionPage(this, InjectionTab);
            ToolboxPg = new ToolboxPage(this, ToolboxTab);
            ModulesPg = new ModulesPage(this, ModulesTab);
            AboutPg = new AboutPage(this, AboutTab);

            PacketLoggerUI = new PacketLoggerFrm(this);

            _haltables.Add(ModulesPg);
            _haltables.Add(PacketLoggerUI);
            _haltables.Add(InjectionPg.FiltersPg);
            _haltables.Add(InjectionPg.SchedulerPg);

            _receivers.Add(ModulesPg);
            _receivers.Add(InjectionPg.FiltersPg);
            _receivers.Add(ConnectionPg);
            _receivers.Add(PacketLoggerUI);

            Connection.ListenPort = (int)Program.Settings["ConnectionListenPort"];
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {
            Eavesdropper.Terminate();
        }
        private void MainFrm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Eavesdropper.Terminate();
        }

        private void TanjiInfoTxt_Click(object sender, EventArgs e)
        {
            Process.Start("https://GitHub.com/ArachisH/Tanji");
        }

        private void Hook_HotkeyActivated(object sender, KeyEventArgs e)
        {
            if (_actions.ContainsKey(e.KeyData))
            {
                _actions[e.KeyData]();
            }
        }

        public void AddQuickAction(Keys keyData, Action action)
        {
            if (!_actions.ContainsKey(keyData))
            {
                _actions[keyData] = action;
                Hook.RegisterHotkey(keyData);
            }
        }
        public async Task<HUser> GetUserAsync(string name, HHotel hotel)
        {
            if (!_userCache.ContainsKey(hotel))
            {
                _userCache[hotel] = new Dictionary<string, HUser>();
            }
            if (!_userCache[hotel].ContainsKey(name))
            {
                HUser profile = await SKore.GetUserAsync(name, hotel).ConfigureAwait(false);
                _userCache[hotel][name] = profile;
            }
            return _userCache[hotel][name];
        }
        public async Task<Bitmap> GetAvatarAsync(string name, HHotel hotel)
        {
            HUser user = await GetUserAsync(name, hotel).ConfigureAwait(false);
            if (user == null) return Resources.Avatar;

            if (!_avatarCache.ContainsKey(user.FigureId))
            {
                using (Bitmap avatar = await SKore.GetAvatarAsync(user.FigureId, HSize.Medium).ConfigureAwait(false))
                {
                    _avatarCache[user.FigureId] = TrimAvatar(avatar);
                }
            }
            return _avatarCache[user.FigureId];
        }

        private void Halt()
        {
            _haltables.ForEach(h => h.Halt());
        }
        private Bitmap TrimAvatar(Bitmap avatar)
        {
            BitmapData data = null;
            var srcRect = default(Rectangle);
            try
            {
                data = avatar.LockBits(new Rectangle(0, 0, avatar.Width, avatar.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                byte[] buffer = new byte[data.Height * data.Stride];
                Marshal.Copy(data.Scan0, buffer, 0, buffer.Length);
                int xMin = int.MaxValue;
                int xMax = 0;
                int yMin = int.MaxValue;
                int yMax = 0;
                for (int y = 0; y < data.Height; y++)
                {
                    for (int x = 0; x < data.Width; x++)
                    {
                        byte alpha = buffer[y * data.Stride + 4 * x + 3];
                        if (alpha != 0)
                        {
                            if (x < xMin) xMin = x;
                            if (x > xMax) xMax = x;
                            if (y < yMin) yMin = y;
                            if (y > yMax) yMax = y;
                        }
                    }
                }
                if (xMax < xMin || yMax < yMin) return null;
                srcRect = Rectangle.FromLTRB(xMin, yMin, xMax + 1, yMax + 1);
            }
            finally
            {
                if (data != null)
                {
                    avatar.UnlockBits(data);
                }
            }

            var trimmedAvatar = new Bitmap(srcRect.Width, srcRect.Height);
            var destRect = new Rectangle(0, 0, srcRect.Width, srcRect.Height);
            using (var graphics = Graphics.FromImage(trimmedAvatar))
            {
                graphics.DrawImage(avatar, destRect, srcRect, GraphicsUnit.Pixel);
            }
            return trimmedAvatar;
        }

        private void Disconnected(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(_disconnected, sender, e);
                return;
            }

            Halt();
            Game.Dispose();
            Game = null;

            TopMost = true;
            Text = "Tanji ~ Disconnected";
        }
        private async void Connected(object sender, ConnectedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(_connected, sender, e);
                return;
            }

            HMessage remoteEndPointPkt = await Connection.Local.ReceivePacketAsync();

            e.HotelServer = ConnectionPg.HotelServer = HotelEndPoint.Parse(remoteEndPointPkt.ReadString().Split('\0')[0], remoteEndPointPkt.ReadInteger());
            e.IsFakingPolicyRequest = (e.HotelServer.Hotel == HHotel.Unknown);
            e.HotelServerSource.SetResult(e.HotelServer);

            ConnectionPg.IsReceiving = true;
            Text = $"Tanji ~ Connected[{e.HotelServer}]";
            TopMost = PacketLoggerUI.TopMost;

            PacketLoggerUI.RevisionTxt.Text = ("Revision: " + Game.Revision);

            PacketLoggerUI.Show();
            PacketLoggerUI.WindowState = FormWindowState.Normal;

            BringToFront();
        }
        private void HandleData(object sender, DataInterceptedEventArgs e)
        {
            if (e.IsOutgoing && e.Packet.Header == Out.LatencyTest)
            {
                _latencyTestStart = e.Timestamp;
            }
            else if (e.Packet.Header == In.LatencyResponse)
            {
                PacketLoggerUI.Latency = (int)(e.Timestamp - _latencyTestStart).TotalMilliseconds;
            }
            foreach (IReceiver receiver in _receivers)
            {
                if (!receiver.IsReceiving) continue;
                if (e.IsOutgoing)
                {
                    receiver.HandleOutgoing(e);
                }
                else
                {
                    receiver.HandleIncoming(e);
                }
            }
        }
    }
}