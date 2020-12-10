using System;
using System.IO;
using System.Reflection;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

using Sulakore.Modules;
using Sulakore.Network;
using Sulakore.Network.Protocol;

namespace Tanji.Services.Modules
{
    public class ModuleInfo : INotifyPropertyChanged
    {
        public const string DISPOSED_STATE = "Disposed";
        public const string INITIALIZED_STATE = "Initialized";

        public ListViewItem PhysicalItem { get; set; }
        public ConcurrentDictionary<string, TaskCompletionSource<HPacket>> DataAwaiters { get; }

        public Type Type { get; set; }
        public Assembly Assembly { get; set; }
        public Version Version { get; set; } = new Version(0, 0);

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
            DataAwaiters = new ConcurrentDictionary<string, TaskCompletionSource<HPacket>>();
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

            foreach (TaskCompletionSource<HPacket> source in DataAwaiters.Values)
            {
                source.TrySetResult(null);
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
                bool isUninitialized = false;
                if (Type != null)
                {
                    instance = (IModule)FormatterServices.GetUninitializedObject(Type);

                    isUninitialized = true;
                    FormUI = instance as Form;
                }
                else instance = new DummyModule(this);

                instance.Installer = Program.Master;
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

                instance.OnConnected();
                Instance = instance;
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

        private void UserInterface_Closed(object sender, EventArgs e)
        {
            if (FormUI != null)
            {
                FormUI.FormClosed -= UserInterface_Closed;
                FormUI = null;
            }
            Dispose();
        }

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }
        protected void RaiseOnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
        #endregion

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
                string identifier = e.Step + e.IsOutgoing.ToString();
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

                    // TODO: wow never finished this
                    var dataAwaiterSource = new TaskCompletionSource<HPacket>();
                    if (!_module.DataAwaiters.TryAdd(identifier, dataAwaiterSource))
                    { }
                    _module.Node.SendAsync(interceptedData);

                    HPacket handledDataPacket = dataAwaiterSource.Task.GetAwaiter().GetResult();
                    if (handledDataPacket == null) return;
                    // This packet contains the identifier at the start, although we do not read it here.

                    bool isContinuing = handledDataPacket.ReadBoolean();
                    if (isContinuing)
                    {
                        dataAwaiterSource = new TaskCompletionSource<HPacket>();

                        // TODO: Finish this
                        _module.DataAwaiters.TryRemove(identifier, out TaskCompletionSource<HPacket> oldSource);
                        if (!_module.DataAwaiters.TryAdd(identifier, dataAwaiterSource))
                        { }

                        bool wasRelayed = handledDataPacket.ReadBoolean();
                        e.Continue(wasRelayed);

                        if (wasRelayed) return; // We have nothing else to do here, packet has already been sent/relayed.

                        handledDataPacket = dataAwaiterSource.Task.GetAwaiter().GetResult();
                        isContinuing = handledDataPacket.ReadBoolean(); // We can ignore this one.
                    }

                    int newPacketLength = handledDataPacket.ReadInt32();
                    byte[] newPacketData = handledDataPacket.ReadBytes(newPacketLength);

                    e.Packet = e.Packet.Format.CreatePacket(newPacketData);
                    e.IsBlocked = handledDataPacket.ReadBoolean();
                }
                finally { _module.DataAwaiters.TryRemove(identifier, out TaskCompletionSource<HPacket> oldSource); }
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

                string identifier = DateTime.Now.Ticks.ToString();
                onConnectedPacket.Write(identifier);

                _module.Node.SendAsync(onConnectedPacket);
            }

            public void Dispose()
            {
                _module.Node.Dispose();
            }
        }
    }
}