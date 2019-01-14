using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Tanji.Controls;

using Sulakore.Modules;
using Sulakore.Network;
using Sulakore.Network.Protocol;

namespace Tanji.Services.Modules
{
    public class ModuleInfo : ObservableObject
    {
        private const string DISPOSED_STATE = "Disposed";
        private const string INITIALIZED_STATE = "Initialized";

        public ListViewItem PhysicalItem { get; set; }
        public Dictionary<string, TaskCompletionSource<HPacket>> DataAwaiters { get; }

        public Type Type { get; set; }
        public Assembly Assembly { get; set; }
        public Version Version { get; set; } = new Version(0, 0);

        public Type EntryType { get; set; }
        public string PropertyName { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public List<AuthorAttribute> Authors { get; }

        public HNode Node { get; set; }
        public string Path { get; set; }
        public string Hash { get; set; }

        public Form FormUI { get; set; }
        public bool IsInitialized => Instance != null;

        private string _currentState = DISPOSED_STATE;
        public string CurrentState
        {
            get => _currentState;
            set
            {
                _currentState = value;
                RaiseOnPropertyChanged();
            }
        }

        private IModule _instance;
        public IModule Instance
        {
            get => _instance;
            set
            {
                _instance = value;
                RaiseOnPropertyChanged();
            }
        }

        public ModuleInfo()
        {
            Authors = new List<AuthorAttribute>();
            DataAwaiters = new Dictionary<string, TaskCompletionSource<HPacket>>();
        }
        public ModuleInfo(HNode node)
            : this()
        {
            Node = node;
        }

        public void Dispose()
        {
            if (FormUI != null)
            {
                FormUI.FormClosed -= UserInterface_Closed;
                FormUI.Close();
                FormUI = null;
            }
            else if (Instance != null)
            {
                Instance.Dispose();
            }

            lock (DataAwaiters)
            {
                foreach (TaskCompletionSource<HPacket> handledDataSource in DataAwaiters.Values.ToArray())
                {
                    handledDataSource.SetResult(null);
                }
            }

            Instance = null;
            CurrentState = DISPOSED_STATE;
        }
        public void Initialize()
        {
            if (Instance != null)
            {
                FormUI?.BringToFront();
                return;
            }
            try
            {
                IModule instance = null; // Make the initialization of the property 'Instance' thread-safe.
                object uiInstance = null;
                bool isUninitialized = false;
                if (EntryType != null)
                {
                    uiInstance = Activator.CreateInstance(EntryType);

                    PropertyInfo property = EntryType.GetProperty(PropertyName);
                    if (property == null)
                    {
                        throw new MissingMemberException(EntryType.Name, PropertyName);
                    }
                    instance = (IModule)property.GetValue(uiInstance);
                }
                else if (Type != null)
                {
                    isUninitialized = true;
                    instance = (IModule)FormatterServices.GetUninitializedObject(Type);
                    uiInstance = instance;
                }
                else instance = new DummyModule(this);

                FormUI = uiInstance as Form;
                instance.Installer = Master;
                if (isUninitialized)
                {
                    ConstructorInfo moduleConstructor = Type.GetConstructor(Type.EmptyTypes);
                    moduleConstructor.Invoke(instance, null);
                }

                if (FormUI != null)
                {
                    FormUI.TopMost = true;
                    FormUI.FormClosed += UserInterface_Closed;
                    FormUI.Show();
                }

                Instance = instance; // At this point, the IModule instance should have been fully initialized.
            }
            catch (Exception ex)
            {
                Program.Display(ex);
                Dispose();
            }
            finally
            {
                if (Instance != null)
                {
                    CurrentState = INITIALIZED_STATE;
                }
            }
        }
        public void ToggleState(object obj)
        {
            switch (CurrentState)
            {
                case INITIALIZED_STATE:
                {
                    Dispose();
                    break;
                }
                case DISPOSED_STATE:
                {
                    Initialize();
                    break;
                }
            }
        }

        private void UserInterface_Closed(object sender, EventArgs e)
        {
            if (FormUI != null)
            {
                FormUI.FormClosed -= UserInterface_Closed;
                FormUI = null;
            }
            Dispose();
        }

        private class DummyModule : IModule
        {
            private readonly ModuleInfo _module;

            public bool IsStandalone => true;
            public IInstaller Installer { get; set; }

            public DummyModule(ModuleInfo module)
            {
                _module = module;
            }

            private void HandleData(DataInterceptedEventArgs e)
            {
                string identifier = e.Timestamp.Ticks.ToString();
                identifier += e.IsOutgoing;
                identifier += e.Step;
                try
                {
                    var interceptedData = new EvaWirePacket(1);
                    interceptedData.Write(identifier);

                    interceptedData.Write(e.Step);
                    interceptedData.Write(e.IsOutgoing);
                    interceptedData.Write(e.Packet.Format.Name);
                    interceptedData.Write(e.IsContinuable && !e.HasContinued);

                    interceptedData.Write(e.GetOriginalData().Length);
                    interceptedData.Write(e.GetOriginalData());

                    interceptedData.Write(e.IsOriginal);
                    if (!e.IsOriginal)
                    {
                        byte[] curPacketData = e.Packet.ToBytes();
                        interceptedData.Write(curPacketData.Length);
                        interceptedData.Write(curPacketData);
                    }

                    var dataAwaiterSource = new TaskCompletionSource<HPacket>();
                    lock (_module.DataAwaiters)
                    {
                        _module.DataAwaiters.Add(identifier, dataAwaiterSource);
                    }
                    _module.Node.SendPacketAsync(interceptedData);

                    HPacket handledDataPacket = dataAwaiterSource.Task.Result;
                    if (handledDataPacket == null) return;
                    // This packet contains the identifier at the start, although we do not read it here.

                    bool isContinuing = handledDataPacket.ReadBoolean();
                    if (isContinuing)
                    {
                        dataAwaiterSource = new TaskCompletionSource<HPacket>();
                        _module.DataAwaiters[identifier] = dataAwaiterSource;

                        bool wasRelayed = handledDataPacket.ReadBoolean();
                        e.Continue(wasRelayed);

                        if (wasRelayed) return; // We have nothing else to do here, packet has already been sent/relayed.

                        handledDataPacket = dataAwaiterSource.Task.Result;
                        isContinuing = handledDataPacket.ReadBoolean(); // We can ignore this one.
                    }

                    int newPacketLength = handledDataPacket.ReadInt32();
                    byte[] newPacketData = handledDataPacket.ReadBytes(newPacketLength);

                    e.Packet = e.Packet.Format.CreatePacket(newPacketData);
                    e.IsBlocked = handledDataPacket.ReadBoolean();
                }
                finally { _module.DataAwaiters.Remove(identifier); }
            }
            public void HandleOutgoing(DataInterceptedEventArgs e) => HandleData(e);
            public void HandleIncoming(DataInterceptedEventArgs e) => HandleData(e);

            public void OnConnected()
            {
                var onConnectedPacket = new EvaWirePacket(2);
                onConnectedPacket.Write(Installer.GameData.Source);

                byte[] gameData = File.ReadAllBytes(Installer.Game.Location);
                onConnectedPacket.Write(gameData.Length);
                onConnectedPacket.Write(gameData);
                onConnectedPacket.Write(Installer.Game.Location);

                byte[] hashesData = File.ReadAllBytes("Hashes.ini");
                onConnectedPacket.Write(hashesData.Length);
                onConnectedPacket.Write(hashesData);

                _module.Node.SendPacketAsync(onConnectedPacket);
                // TODO: Create a special "DataAwaiter" for this message, as it should return AFTER the module is finished with it.
            }

            public void Dispose()
            {
                _module.Node.Dispose();
            }
        }
    }
}