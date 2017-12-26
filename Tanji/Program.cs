using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;

using Tanji.Network;
using Tanji.Windows;
using Tanji.Services;
using Tanji.Utilities;
using Tanji.Services.Injection;

using Sulakore.Habbo;
using Sulakore.Modules;
using Sulakore.Network;
using Sulakore.Habbo.Messages;

using Eavesdrop;

namespace Tanji
{
    public class Program : IInstaller
    {
        private readonly List<IHaltable> _haltables;
        private readonly SortedList<int, IReceiver> _receivers;
        private readonly Dictionary<Keys, Action> _hotkeyActions;

        public Incoming In { get; }
        public Outgoing Out { get; }
        public KeyboardHook Hook { get; }
        public HGameData GameData { get; }
        public bool IsConnected => Connection.IsConnected;

        public HGame Game { get; set; }
        HGame IInstaller.Game => Game;

        public HConnection Connection { get; }
        IHConnection IInstaller.Connection => Connection;

        public static Program Master { get; private set; }

        [STAThread]
        public static void Main()
        {
            Eavesdropper.Certifier = new CertificateManager("Tanji", "Tanji Certificate Authority");
            AppDomain.CurrentDomain.UnhandledException += UnhandledException;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

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

            In = new Incoming();
            Out = new Outgoing();
            GameData = new HGameData();

            Connection = new HConnection();
            Connection.Connected += Connected;
            Connection.DataOutgoing += HandleData;
            Connection.DataIncoming += HandleData;
            Connection.Disconnected += Disconnected;

            Hook = new KeyboardHook();
            Hook.HotkeyPressed += HotkeyPressed;
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
            switch (receiver.GetType().Name)
            {
                case nameof(ModulesPage): rank = 0; break;
                case nameof(FiltersPage): rank = 1; break;
                case nameof(ConnectionPage): rank = 2; break;
                case nameof(PacketLoggerFrm): rank = 3; break;

                default:
                throw new ArgumentException("Unrecognized receiver: " + receiver, nameof(receiver));
            }

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

        private void Connected(object sender, EventArgs e)
        {
            foreach (IHaltable haltable in _haltables)
            {
                if (haltable.InvokeRequired)
                {
                    haltable.Invoke((MethodInvoker)haltable.Restore, null);
                }
                else haltable.Restore();
            }
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
    }
}