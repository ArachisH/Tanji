using System;
using System.Linq;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;

using Sulakore.Habbo;
using Sulakore.Modules;
using Sulakore.Protocol;
using Sulakore.Habbo.Web;
using Sulakore.Communication;

using Tangine;
using Tangine.Habbo;

namespace Tanji.Pages.Modules.Handlers
{
    public class ModulesManager : Contractor, ITContext
    {
        private ModuleItem[] _moduleItemsArray;
        private readonly Dictionary<Type, ModuleItem> _moduleItems;

        public delegate void ModuleActionDelegate(Type moduleType, ModuleAction action);

        public HGame Game => UI.Game;
        public override HHotel Hotel => UI.Hotel;
        public override HGameData GameData => UI.GameData;
        public override IHConnection Connection => UI.Connection;

        public MainFrm UI { get; }
        public HNode RemoteModule { get; private set; }
        public ModuleActionDelegate OnModuleAction { get; set; }
        public Dictionary<string, TaskCompletionSource<DataInterceptedEventArgs>> DataAwaiters { get; }

        public ModulesManager(MainFrm ui)
            : base("Installed Modules")
        {
            _moduleItems = new Dictionary<Type, ModuleItem>();

            UI = ui;

            DataAwaiters = new Dictionary<string, TaskCompletionSource<DataInterceptedEventArgs>>();

            Task grabRemoteModuleTask =
                GrabRemoteModuleAsync();
        }

        public ModuleItem[] GetModuleItems()
        {
            return (_moduleItemsArray ??
                (_moduleItemsArray = _moduleItems.Values.ToArray()));
        }
        public ModuleItem GetModuleItem(Type type)
        {
            ModuleItem moduleItem = null;
            _moduleItems.TryGetValue(type, out moduleItem);
            return moduleItem;
        }

        private async Task GrabRemoteModuleAsync()
        {
            RemoteModule = await HNode.AcceptAsync(8055).ConfigureAwait(false);
            Task receiveRemModuDataTask = ReceiveRemoteModuleDataAsync();
        }
        private async Task ReceiveRemoteModuleDataAsync()
        {
            try
            {
                HMessage packet = await RemoteModule.ReceivePacketAsync().ConfigureAwait(false);
                if (packet == null)
                {
                    DataAwaiters.Values.ToList()
                        .ForEach(awaiter => awaiter.SetResult(null));

                    RemoteModule = null;
                    Task grabRemoteModuleTask =
                        GrabRemoteModuleAsync();

                    return;
                }

                var response = new HMessage(packet.Header);
                #region Switch: packet.Header
                switch (packet.Header)
                {
                    default: response = null; break;
                    case 0:
                    {
                        response.WriteShort((ushort)Hotel);
                        break;
                    }
                    case 1:
                    {
                        response.WriteString(Game?.Location);
                        break;
                    }
                    case 2:
                    {
                        response.WriteString(GameData.Source);
                        break;
                    }
                    case 3:
                    {
                        response.WriteShort(Connection.Port);
                        response.WriteString(Connection.Host);
                        response.WriteString(Connection.Address);
                        break;
                    }
                    case 4:
                    {
                        if (Connection != null)
                        {
                            int dataLength = packet.ReadInteger();
                            byte[] data = packet.ReadBytes(dataLength);

                            await Connection.SendToClientAsync(
                                data).ConfigureAwait(false);

                            response = null;
                        }
                        break;
                    }
                    case 5:
                    {
                        if (Connection != null)
                        {
                            int dataLength = packet.ReadInteger();
                            byte[] data = packet.ReadBytes(dataLength);

                            await Connection.SendToServerAsync(
                                data).ConfigureAwait(false);

                            response = null;
                        }
                        break;
                    }
                    case 6:
                    case 7:
                    {
                        string stamp = packet.ReadString();
                        if (DataAwaiters.ContainsKey(stamp))
                        {
                            var destination = (HDestination)(packet.Header - 6);

                            int step = packet.ReadInteger();
                            bool isBlocked = packet.ReadBoolean();
                            int dataLength = packet.ReadInteger();
                            byte[] data = packet.ReadBytes(dataLength);
                            var interPacket = new HMessage(data, destination);

                            var args = new DataInterceptedEventArgs(interPacket, step, null);
                            args.IsBlocked = isBlocked;

                            DataAwaiters[stamp].SetResult(args);
                            response = null;
                        }
                        break;
                    }
                }
                #endregion

                if (response != null)
                {
                    await RemoteModule.SendPacketAsync(response).ConfigureAwait(false);
                }
            }
            finally
            {
                if (RemoteModule != null)
                {
                    Task receiveRemModuDataTask =
                        ReceiveRemoteModuleDataAsync();
                }
            }
        }

        protected override void OnModuleInstalled(Type type)
        {
            if (!_moduleItems.ContainsKey(type))
            {
                _moduleItemsArray = null;
                _moduleItems[type] = new ModuleItem(type, this);
            }
            OnModuleAction?.Invoke(type, ModuleAction.Installed);
        }
        protected override void OnModuleReinstalled(Type type)
        {
            OnModuleAction?.Invoke(type, ModuleAction.Reinstalled);
        }
        protected override void OnModuleUninstalled(Type type)
        {
            OnModuleAction?.Invoke(type, ModuleAction.Uninstalled);
        }

        protected override void OnModuleDisposed(Type type)
        {
            ModuleItem moduleInfo = GetModuleItem(type);
            moduleInfo.IsInitialized = false;

            OnModuleAction?.Invoke(type, ModuleAction.Disposed);
        }
        protected override void OnModuleInitialized(Type type)
        {
            IModule module = GetModule(type);
            ModuleItem moduleInfo = GetModuleItem(type);

            var extension = (module as ITExtension);
            if (extension != null)
            {
                moduleInfo.Extension = extension;

                var extensionForm = (module as Form);
                if (extensionForm != null)
                {
                    extensionForm.Icon = UI.Icon;
                    extensionForm.ShowIcon = true;
                    moduleInfo.ExtensionForm = extensionForm;
                }
            }

            moduleInfo.IsInitialized = true;
            OnModuleAction?.Invoke(type, ModuleAction.Initialized);
        }
    }
}