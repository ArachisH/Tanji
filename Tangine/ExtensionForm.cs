using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading.Tasks;

using Sulakore.Habbo;
using Sulakore.Modules;
using Sulakore.Habbo.Web;
using Sulakore.Protocol;
using Sulakore.Communication;
using Sulakore.Habbo.Messages;

namespace Tangine
{
    public class ExtensionForm : Form, IModule
    {
        private int _initStep;
        private readonly IInstaller _installer;
        private readonly HNode _remoteContractor;
        private readonly TaskCompletionSource<bool> _initializationSource;

        [Browsable(false)]
        public IInstaller Installer { get; set; }

        [Browsable(false)]
        public virtual bool IsRemoteModule { get; }

        [Browsable(false)]
        [Obsolete("Instead, use the (In/Out)DataCapture attributes on methods.", false)]
        public HTriggers Triggers { get; }

        private HGame _game;
        [Browsable(false)]
        public HGame Game => (_game ?? _installer?.Game);

        private HHotel? _hotel;
        [Browsable(false)]
        public HHotel Hotel => (_hotel ?? (_installer?.Hotel ?? HHotel.Com));

        private HGameData _gameData;
        [Browsable(false)]
        public HGameData GameData => (_gameData ?? _installer?.GameData);

        private readonly IHConnection _connection;
        [Browsable(false)]
        public IHConnection Connection => (_connection ?? _installer?.Connection);

        private readonly Incoming _in;
        [Browsable(false)]
        public Incoming In => (_in ?? _installer?.In);

        private readonly Outgoing _out;
        [Browsable(false)]
        public Outgoing Out => (_out ?? _installer?.Out);

        public ExtensionForm()
        {
            _in = new Incoming();
            _out = new Outgoing();
            _installer = Contractor.GetInstaller(GetType());
            if (_installer == null && IsRemoteModule)
            {
                _remoteContractor = GetRemoteContractor();
                if (_remoteContractor != null)
                {
                    _connection = new ContractorProxy(_remoteContractor);
                    _initializationSource = new TaskCompletionSource<bool>();

                    Task receiveRemContDataTask =
                        ReceiveRemoteContractorDataAsync();

                    RequestRemoteContractorData();
                    _initializationSource.Task.Wait();
                    _initializationSource = null;
                }
            }

            Triggers = new HTriggers();
        }

        public virtual void ModifyGame(HGame game)
        { }
        public virtual void ModifyGameData(HGameData gameData)
        { }

        public virtual void HandleOutgoing(DataInterceptedEventArgs e)
        { }
        public virtual void HandleIncoming(DataInterceptedEventArgs e)
        { }

        private void RequestRemoteContractorData()
        {
            _remoteContractor.SendPacketAsync(0); // Hotel
            _remoteContractor.SendPacketAsync(1); // Game
            _remoteContractor.SendPacketAsync(2); // GameData
            _remoteContractor.SendPacketAsync(3); // Connection Info
        }
        private async Task ReceiveRemoteContractorDataAsync()
        {
            try
            {
                HMessage packet = await _remoteContractor.ReceivePacketAsync().ConfigureAwait(false);
                if (packet == null)
                {
                    Environment.Exit(0);
                }
                #region Switch: packet.Header
                switch (packet.Header)
                {
                    case 0:
                    {
                        _initStep++;
                        _hotel = (HHotel)packet.ReadShort();
                        break;
                    }
                    case 1:
                    {
                        _initStep++;
                        string location = packet.ReadString();
                        if (!string.IsNullOrWhiteSpace(location))
                        {
                            _game = new HGame(location);
                            _game.Disassemble();

                            _game.GenerateMessageHashes();

                            if (_initializationSource == null)
                            {
                                ModifyGame(_game);
                            }
                        }
                        if (packet.Readable > 0)
                        {
                            string hashesPath = packet.ReadString();
                            _in.Load(_game, hashesPath);
                            _out.Load(_game, hashesPath);
                        }
                        break;
                    }
                    case 2:
                    {
                        _initStep++;
                        _gameData = new HGameData(packet.ReadString());

                        if (_initializationSource == null)
                            ModifyGameData(_gameData);

                        break;
                    }
                    case 3:
                    {
                        _initStep++;
                        var connection = (ContractorProxy)_connection;
                        connection.Port = packet.ReadShort();
                        connection.Host = packet.ReadString();
                        connection.Address = packet.ReadString();
                        break;
                    }
                    case 4:
                    case 5:
                    {
                        var destination = (HDestination)(packet.Header - 4);

                        string stamp = packet.ReadString();
                        int step = packet.ReadInteger();
                        bool isBlocked = packet.ReadBoolean();
                        int dataLength = packet.ReadInteger();
                        byte[] data = packet.ReadBytes(dataLength);
                        var interPacket = new HMessage(data, destination);

                        var args = new DataInterceptedEventArgs(interPacket, step, null);
                        try
                        {
                            if (destination == HDestination.Client)
                            {
                                HandleIncoming(args);
                                Triggers?.HandleIncoming(args);
                            }
                            else
                            {
                                HandleOutgoing(args);
                                Triggers?.HandleOutgoing(args);
                            }
                        }
                        finally
                        {
                            await SendInterceptedDataResponseAsync(
                                stamp, args).ConfigureAwait(false);
                        }
                        break;
                    }
                }
                #endregion

                if (_initStep == 4 &&
                    _initializationSource != null)
                {
                    _initializationSource.SetResult(true);
                }
            }
            finally
            {
                Task receiveRemContDataTask =
                    ReceiveRemoteContractorDataAsync();
            }
        }

        private HNode GetRemoteContractor()
        {
            HNode remoteContractor = null;
            do
            {
                try
                {
                    remoteContractor = HNode.ConnectNewAsync("localhost", 8055).Result;
                }
                catch
                {
                    DialogResult result = MessageBox.Show($"Unable to establish connection with the remote contractor on port 8055.",
                        "Tangine ~ Warning!", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Warning);

                    switch (result)
                    {
                        case DialogResult.Ignore: return null;
                        case DialogResult.Abort: Environment.Exit(0); break;
                    }
                }
            }
            while (remoteContractor == null);
            return remoteContractor;
        }
        private Task SendInterceptedDataResponseAsync(string stamp, DataInterceptedEventArgs args)
        {
            var interceptedData = new HMessage((ushort)(args.Packet.Destination + 6));
            interceptedData.WriteString(stamp);
            interceptedData.WriteInteger(args.Step);
            interceptedData.WriteBoolean(args.IsBlocked);
            interceptedData.WriteInteger(args.Packet.Length + 4);
            interceptedData.WriteBytes(args.Packet.ToBytes());

            return _remoteContractor.SendPacketAsync(interceptedData);
        }
    }
}