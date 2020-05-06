using System;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;

using Tanji.Network;
using Tanji.Windows;
using Tanji.Services;
using Tanji.Utilities;
using Tanji.Services.Modules;
using Tanji.Services.Injection;

using Sulakore.Modules;
using Sulakore.Network;
using Sulakore.Habbo.Web;
using Sulakore.Habbo.Messages;

using Eavesdrop;

namespace Tanji
{
    public class Program : IInstaller
    {
        public static Font DefaultFont { get; } = new Font("Microsoft Sans Serif", 8f);

        private Action<ConnectedEventArgs> _restore;

        private readonly List<IHaltable> _haltables;
        private readonly SortedList<int, IReceiver> _receivers;
        private readonly Dictionary<Keys, Action> _hotkeyActions;

        public Incoming In => Game.In;
        public Outgoing Out => Game.Out;
        public KeyboardHook Hook { get; }
        public HGameData GameData { get; }

        public TConfiguration Config { get; }
        public bool IsConnected => Connection.IsConnected;

        public HGame Game { get; set; }
        HGame IInstaller.Game => Game;

        public HConnection Connection { get; }
        IHConnection IInstaller.Connection => Connection;

        public static Program Master { get; private set; }

        [STAThread]
        public static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledException;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // TODO: .NET 5 - Application.SetDefaultFont(DefaultFont);
            
            Master = new Program();
            Application.Run(new MainFrm());
        }
        private static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached) return;

            var exception = (Exception)e.ExceptionObject;
            MessageBox.Show($"Message: {exception.Message}\r\n\r\n{exception.StackTrace.Trim()}",
                "Tanji - Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (e.IsTerminating)
            {
                Eavesdropper.Terminate();
            }
        }

        public Program()
        {
            _haltables = new List<IHaltable>();
            _receivers = new SortedList<int, IReceiver>();
            _hotkeyActions = new Dictionary<Keys, Action>();

            GameData = new HGameData();

            Connection = new HConnection();
            Connection.Connected += Connected;
            Connection.DataOutgoing += HandleData;
            Connection.DataIncoming += HandleData;
            Connection.Disconnected += Disconnected;

            Hook = new KeyboardHook();
            Hook.HotkeyPressed += HotkeyPressed;

            Config = new TConfiguration();

            Eavesdropper.Terminate();
            Eavesdropper.Certifier = new CertificateManager("Tanji", "Tanji Certificate Authority");
            Eavesdropper.Overrides.AddRange(Config.ProxyOverrides);
        }

        public void AddHaltable(IHaltable haltable)
        {
            if (_haltables.Contains(haltable))
            {
                throw new ArgumentException("Haltable object has already been added.", nameof(haltable));
            }
            _haltables.Add(haltable);
        }
        public void AddReceiver(IReceiver receiver)
        {
            /* 
             * Low = Lowest Importance
             * Will be invoked first, therefore it will NOT have final judgement on the data being passed.
             */

            int rank = -1;
            rank = receiver.GetType().Name switch
            {
                nameof(ModulesPage) => 0,
                nameof(FiltersPage) => 1,
                nameof(ConnectionPage) => 2,
                nameof(PacketLoggerFrm) => 3,
                _ => throw new ArgumentException("Unrecognized receiver: " + receiver, nameof(receiver)),
            };

            if (_receivers.ContainsKey(rank))
            {
                throw new ArgumentException("This rank is already being occupied by a receiver: " + rank);
            }
            else if (_receivers.ContainsValue(receiver))
            {
                throw new ArgumentException("This receiver has already been added: " + receiver);
            }
            else _receivers.Add(rank, receiver);
        }

        private void Disconnected(object sender, EventArgs e)
        {
            foreach (IHaltable haltable in _haltables)
            {
                if (haltable.InvokeRequired)
                {
                    haltable.Invoke((MethodInvoker)haltable.Halt, null);
                }
                else haltable.Halt();
            }
        }
        private void Connected(object sender, ConnectedEventArgs e)
        {
            foreach (IHaltable haltable in _haltables)
            {
                if (haltable.InvokeRequired)
                {
                    _restore = haltable.Restore;
                    haltable.Invoke(_restore, new object[] { e });
                }
                else haltable.Restore(e);
            }
            GC.Collect();
        }
        private void HandleData(object sender, DataInterceptedEventArgs e)
        {
            if (_receivers.Count == 0) return;
            foreach (IReceiver receiver in _receivers.Values)
            {
                if (!receiver.IsReceiving) continue;
                if (e.IsOutgoing)
                {
                    receiver.HandleOutgoing(e);
                }
                else receiver.HandleIncoming(e);
            }
        }

        private void HotkeyPressed(object sender, KeyEventArgs e)
        {
            if (_hotkeyActions.TryGetValue(e.KeyData, out Action action))
            {
                action();
            }
        }

        public static void Display(Exception exception, string header = null)
        {
            string messsage = header;
            if (!string.IsNullOrWhiteSpace(messsage) && exception != null)
            {
                messsage += "\r\n\r\nException: ";
            }
            MessageBox.Show(messsage + exception?.ToString(), "Tanji - Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}