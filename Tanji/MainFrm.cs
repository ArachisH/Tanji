using System;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;

using Tanji.Utilities;
using Tanji.Properties;
using Tanji.Components;
using Tanji.Pages.About;
using Tanji.Applications;
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

using Eavesdrop;

namespace Tanji
{
    [DesignerCategory("Form")]
    public partial class MainFrm : ObservableForm
    {
        private readonly List<IHaltable> _haltables;
        private readonly List<IReceiver> _receivers;
        private readonly Dictionary<Keys, Action> _actions;
        private readonly EventHandler _connected, _disconnected;
        private readonly Dictionary<string, Bitmap> _avatarCache;
        private readonly Dictionary<HHotel, Dictionary<string, HProfile>> _profileCache;

        public KeyboardHook Hook { get; }

        public HGame Game { get; set; }
        public HHotel Hotel { get; set; }
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
            _profileCache = new Dictionary<HHotel, Dictionary<string, HProfile>>();

            GameData = new HGameData();
            Connection = new HConnection();
            Connection.Connected += Connected;
            Connection.Disconnected += Disconnected;
            Connection.DataOutgoing += DataOutgoing;
            Connection.DataIncoming += DataIncoming;

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
        private void TanjiDonateTxt_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=HMYZ4GB5N2PAU");
        }
        private void TanjiVersionTxt_Click(object sender, EventArgs e)
        {
            if (AboutPg.TanjiRepo.LatestRelease != null)
                Process.Start(AboutPg.TanjiRepo.LatestRelease.HtmlUrl);
        }

        private void Hook_HotkeyActivated(object sender, KeyEventArgs e)
        {
            if (_actions.ContainsKey(e.KeyData))
                _actions[e.KeyData]();
        }

        public void AddQuickAction(Keys keyData, Action action)
        {
            if (!_actions.ContainsKey(keyData))
            {
                _actions[keyData] = action;
                Hook.RegisterHotkey(keyData);
            }
        }
        public async Task<Bitmap> GetAvatarAsync(string name, HHotel hotel)
        {
            HProfile profile = await GetProfileAsync(
                name, hotel).ConfigureAwait(false);

            if (profile == null)
                return Resources.Avatar;

            if (!_avatarCache.ContainsKey(profile.User.FigureId))
            {
                Bitmap avatar = await SKore.GetAvatarAsync(
                    profile.User.FigureId, HSize.Medium).ConfigureAwait(false);

                _avatarCache[profile.User.FigureId] = avatar;
            }
            return _avatarCache[profile.User.FigureId];
        }
        public async Task<HProfile> GetProfileAsync(string name, HHotel hotel)
        {
            if (!_profileCache.ContainsKey(hotel))
                _profileCache[hotel] = new Dictionary<string, HProfile>();

            if (!_profileCache[hotel].ContainsKey(name))
            {
                HProfile profile = await SKore.GetProfileAsync(
                    name, hotel).ConfigureAwait(false);

                _profileCache[hotel][name] = profile;
            }
            return _profileCache[hotel][name];
        }

        private void Halt()
        {
            _haltables.ForEach(h => h.Halt());
        }
        private void HandleData(DataInterceptedEventArgs e)
        {
            bool isOutgoing = (e.Packet.Destination == HDestination.Server);
            foreach (IReceiver receiver in _receivers)
            {
                if (!receiver.IsReceiving) continue;

                if (isOutgoing) receiver.HandleOutgoing(e);
                else receiver.HandleIncoming(e);
            }
        }

        private void Connected(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(_connected, sender, e);
                return;
            }

            ConnectionPg.IsReceiving = true;
            Text = $"Tanji ~ Connected[{Connection.Host}:{Connection.Port}]";
            TopMost = PacketLoggerUI.TopMost;

            PacketLoggerUI.RevisionTxt.Text = ("Revision: " + Game.Revision);

            PacketLoggerUI.Show();
            PacketLoggerUI.WindowState = FormWindowState.Normal;

            BringToFront();
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
        private void DataOutgoing(object sender, DataInterceptedEventArgs e) => HandleData(e);
        private void DataIncoming(object sender, DataInterceptedEventArgs e) => HandleData(e);
    }
}