using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;

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
        private readonly IModule _module;
        private readonly HNode _remoteContractor;
        private readonly TaskCompletionSource<bool> _initializationSource;
        private readonly List<DataCaptureAttribute> _unknownDataAttributes;
        private readonly Dictionary<ushort, List<DataCaptureAttribute>> _outDataAttributes, _inDataAttributes;

        private const BindingFlags BINDINGS = (BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);

        [Browsable(false)]
        public IInstaller Installer { get; set; }

        [Browsable(false)]
        public virtual bool IsRemoteModule { get; }

        [Browsable(false)]
        public HTriggers Triggers { get; }

        private HGame _game;
        [Browsable(false)]
        public HGame Game => (_game ?? Installer?.Game);

        private HHotel? _hotel;
        [Browsable(false)]
        public HHotel Hotel => (_hotel ?? (Installer?.Hotel ?? HHotel.Com));

        private HGameData _gameData;
        [Browsable(false)]
        public HGameData GameData => (_gameData ?? Installer?.GameData);

        private readonly IHConnection _connection;
        [Browsable(false)]
        public IHConnection Connection => (_connection ?? Installer?.Connection);

        private Incoming _in;
        [Browsable(false)]
        public Incoming In => (_in ?? Installer?.In);

        private Outgoing _out;
        [Browsable(false)]
        public Outgoing Out => (_out ?? Installer?.Out);

        public ExtensionForm()
        {
            _module = this;
            _unknownDataAttributes = new List<DataCaptureAttribute>();
            _inDataAttributes = new Dictionary<ushort, List<DataCaptureAttribute>>();
            _outDataAttributes = new Dictionary<ushort, List<DataCaptureAttribute>>();

            Triggers = new HTriggers();
            foreach (MethodInfo method in GetAllMethods(GetType()))
            {
                foreach (var dataCaptureAtt in method.GetCustomAttributes<DataCaptureAttribute>())
                {
                    if (dataCaptureAtt == null) continue;

                    dataCaptureAtt.Method = method;
                    if (_unknownDataAttributes.Any(dca => dca.Equals(dataCaptureAtt))) continue;

                    dataCaptureAtt.Target = this;
                    if (dataCaptureAtt.Id != null)
                    {
                        AddCallback(dataCaptureAtt, (ushort)dataCaptureAtt.Id);
                    }
                    else _unknownDataAttributes.Add(dataCaptureAtt);
                }
            }

            if (Installer == null && IsRemoteModule)
            {
                _remoteContractor = GetRemoteContractor();
                if (_remoteContractor != null)
                {
                    _connection = new ContractorProxy(_remoteContractor);
                    _initializationSource = new TaskCompletionSource<bool>();

                    Task receiveRemContDataTask = ReceiveRemoteContractorDataAsync();

                    RequestRemoteContractorData();
                    _initializationSource.Task.Wait();
                    _initializationSource = null;
                }
            }
        }

        void IModule.ModifyGame(HGame game)
        {
            var unresolved = new Dictionary<string, IList<string>>();
            foreach (PropertyInfo property in GetAllProperties(GetType()))
            {
                var messageIdAtt = property.GetCustomAttribute<MessageIdAttribute>();
                if (string.IsNullOrWhiteSpace(messageIdAtt?.Hash)) continue;

                ushort[] ids = game.GetMessageIds(messageIdAtt.Hash);
                if (ids != null)
                {
                    property.SetValue(this, ids[0]);
                }
                else
                {
                    if (!unresolved.TryGetValue(messageIdAtt.Hash, out IList<string> users))
                    {
                        users = new List<string>();
                        unresolved.Add(messageIdAtt.Hash, users);
                    }
                    users.Add("Property: " + property.Name);
                }
            }
            foreach (DataCaptureAttribute dataCaptureAtt in _unknownDataAttributes)
            {
                if (string.IsNullOrWhiteSpace(dataCaptureAtt.Identifier)) continue;

                ushort[] ids = game.GetMessageIds(dataCaptureAtt.Identifier);
                if (ids != null)
                {
                    AddCallback(dataCaptureAtt, ids[0]);
                }
                else
                {
                    var identifiers = (dataCaptureAtt.IsOutgoing ? Out : (Identifiers)In);
                    if (identifiers.TryGetId(dataCaptureAtt.Identifier, out ushort id))
                    {
                        AddCallback(dataCaptureAtt, id);
                    }
                    else
                    {
                        if (!unresolved.TryGetValue(dataCaptureAtt.Identifier, out IList<string> users))
                        {
                            users = new List<string>();
                            unresolved.Add(dataCaptureAtt.Identifier, users);
                        }
                        users.Add(dataCaptureAtt.GetType().Name + ": " + dataCaptureAtt.Method.Name);
                    }
                }
            }
            if (unresolved.Count > 0)
            {
                throw new HashResolvingException(game.Revision, unresolved);
            }
            ModifyGame(game);
        }
        public virtual void ModifyGame(HGame game)
        { }

        void IModule.ModifyGameData(HGameData gameData)
        {
            ModifyGameData(gameData);
        }
        public virtual void ModifyGameData(HGameData gameData)
        { }

        void IModule.HandleOutgoing(DataInterceptedEventArgs e)
        {
            HandleOutgoing(e);
            HandleData(_outDataAttributes, e);
            Triggers?.HandleOutgoing(e);
        }
        public virtual void HandleOutgoing(DataInterceptedEventArgs e)
        { }

        void IModule.HandleIncoming(DataInterceptedEventArgs e)
        {
            HandleIncoming(e);
            HandleData(_inDataAttributes, e);
            Triggers?.HandleIncoming(e);
        }
        public virtual void HandleIncoming(DataInterceptedEventArgs e)
        { }

        private void AddCallback(DataCaptureAttribute attribute, ushort id)
        {
            Dictionary<ushort, List<DataCaptureAttribute>> callbacks =
                (attribute.IsOutgoing ? _outDataAttributes : _inDataAttributes);

            if (!callbacks.TryGetValue(id, out List<DataCaptureAttribute> attributes))
            {
                attributes = new List<DataCaptureAttribute>();
                callbacks.Add(id, attributes);
            }
            attributes.Add(attribute);
        }
        private void HandleData(IDictionary<ushort, List<DataCaptureAttribute>> callbacks, DataInterceptedEventArgs e)
        {
            if (callbacks.TryGetValue(e.Packet.Header, out List<DataCaptureAttribute> attributes))
            {
                foreach (DataCaptureAttribute attribute in attributes)
                {
                    e.Packet.Position = 0;
                    attribute.Invoke(e);
                }
            }
        }

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
                        _in = new Incoming();
                        _out = new Outgoing();

                        string location = packet.ReadString();
                        if (!string.IsNullOrWhiteSpace(location))
                        {
                            _game = new HGame(location);
                            _game.Disassemble();

                            _game.GenerateMessageHashes();
                            if (packet.Readable > 0)
                            {
                                string hashesPath = packet.ReadString();
                                _in.Load(_game, hashesPath);
                                _out.Load(_game, hashesPath);
                            }
                            _module.ModifyGame(_game);
                        }
                        break;
                    }
                    case 2:
                    {
                        _initStep++;
                        _gameData = new HGameData(packet.ReadString());
                        _module.ModifyGameData(_gameData);
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

                        var args = new DataInterceptedEventArgs(interPacket, step, (destination == HDestination.Server));
                        try
                        {
                            if (destination == HDestination.Server)
                            {
                                _module.HandleOutgoing(args);
                            }
                            else
                            {
                                _module.HandleIncoming(args);
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

                if (_initStep == 4)
                {
                    _initializationSource?.SetResult(true);
                }
            }
            finally
            {
                Task receiveRemContDataTask = ReceiveRemoteContractorDataAsync();
            }
        }

        private HNode GetRemoteContractor()
        {
            HNode remoteContractor = null;
            do
            {
                try
                {
                    remoteContractor = HNode.ConnectNewAsync("127.0.0.1", 8055).Result;
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

        private IEnumerable<MethodInfo> GetAllMethods(Type type)
        {
            return Excavate(type, t => t.GetMethods(BINDINGS));
        }
        private IEnumerable<PropertyInfo> GetAllProperties(Type type)
        {
            return Excavate(type, t => t.GetProperties(BINDINGS));
        }
        private IEnumerable<T> Excavate<T>(Type type, Func<Type, IEnumerable<T>> excavator)
        {
            IEnumerable<T> excavated = null;
            while (type != null && type.BaseType != null)
            {
                IEnumerable<T> batch = excavator(type);
                excavated = (excavated?.Concat(batch) ?? batch);
                type = type.BaseType; ;
            }
            return excavated;
        }
    }
}