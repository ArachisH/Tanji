using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;

using Tanji.Controls;

using Sulakore.Habbo;
using Sulakore.Modules;
using Sulakore.Network;
using Sulakore.Network.Protocol;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.IO;

namespace Tanji.Services.Modules
{
    public class ModuleInfo : ObservableObject
    {
        private static readonly List<Assembly> _loadedStyles;

        private const string DISPOSED_STATE = "Disposed";
        private const string INITIALIZED_STATE = "Initialized";

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
        //?public Window WindowUI { get; set; }
        public bool IsInitialized => (Instance != null);

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

        //?public Command ToggleStateCommand { get; }

        static ModuleInfo()
        {
            _loadedStyles = new List<Assembly>();
        }
        public ModuleInfo()
        {
            Authors = new List<AuthorAttribute>();
            //?ToggleStateCommand = new Command(ToggleState);
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
            //?else if (WindowUI != null)
            //{
            //    WindowUI.Closed -= UserInterface_Closed;
            //    WindowUI.Close();
            //    WindowUI = null;
            //}
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
                //?WindowUI?.Activate();
                FormUI?.BringToFront();
                return;
            }
            try
            {
                object uiInstance = null;
                bool isUninitialized = false;
                if (EntryType != null)
                {
                    //?LoadStyles();
                    uiInstance = Activator.CreateInstance(EntryType);

                    PropertyInfo property = EntryType.GetProperty(PropertyName);
                    if (property == null)
                    {
                        throw new MissingMemberException(EntryType.Name, PropertyName);
                    }
                    Instance = (IModule)property.GetValue(uiInstance);
                }
                else if (Type != null)
                {
                    isUninitialized = true;
                    Instance = (IModule)FormatterServices.GetUninitializedObject(Type);
                    uiInstance = Instance;
                }
                else Instance = new DummyModule(this);

                FormUI = (uiInstance as Form);
                //?WindowUI = (uiInstance as Window);

                Instance.Installer = Master;
                if (isUninitialized)
                {
                    ConstructorInfo moduleConstructor = Type.GetConstructor(Type.EmptyTypes);
                    moduleConstructor.Invoke(Instance, null);
                }

                if (Master.Connection.IsConnected)
                {
                    Instance.Synchronize(Master.Game);
                    Instance.Synchronize(Master.GameData);
                }

                //if (WindowUI != null)
                //{
                //    WindowUI.Topmost = true;
                //    WindowUI.Closed += UserInterface_Closed;
                //    WindowUI.Show();
                //}
                //else 
                if (FormUI != null)
                {
                    FormUI.TopMost = true;
                    FormUI.FormClosed += UserInterface_Closed;
                    FormUI.Show();
                }
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

        //private void LoadStyles()
        //{
        //    if (_loadedStyles.Contains(Assembly)) return;

        //    foreach (Stream resourceStream in Assembly.GetManifestResourceNames()
        //        .Select(n => Assembly.GetManifestResourceStream(n)))
        //    {
        //        using (resourceStream)
        //        using (var reader = new ResourceReader(resourceStream))
        //        {
        //            foreach (DictionaryEntry entry in reader)
        //            {
        //                try
        //                {
        //                    if (!(entry.Key as string).Equals("styles.baml")) continue;

        //                    var bamlStream = (entry.Value as Stream);
        //                    if (bamlStream == null) continue;

        //                    var resource = LoadBaml<ResourceDictionary>(bamlStream);
        //                    System.Windows.Application.Current.Resources.MergedDictionaries.Add(resource);
        //                }
        //                catch { }
        //            }
        //        }
        //    }
        //    _loadedStyles.Add(Assembly);
        //}
        //private T LoadBaml<T>(Stream stream)
        //{
        //    var reader = new Baml2006Reader(stream);
        //    var writer = new XamlObjectWriter(reader.SchemaContext);
        //    while (reader.Read())
        //    {
        //        writer.WriteNode(reader);
        //    }
        //    return (T)writer.Result;
        //}
        private void UserInterface_Closed(object sender, EventArgs e)
        {
            if (FormUI != null)
            {
                FormUI.FormClosed -= UserInterface_Closed;
                FormUI = null;
            }
            //?if (WindowUI != null)
            //{
            //    WindowUI.Closed -= UserInterface_Closed;
            //    WindowUI = null;
            //}
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

            public void Synchronize(HGame game)
            {
                string hashesPath = System.IO.Path.GetFullPath("Hashes.ini");
                _module.Node.SendPacketAsync(3, game.Location, hashesPath);
            }
            public void Synchronize(HGameData gameData)
            {
                _module.Node.SendPacketAsync(4, gameData.Source);
            }

            public void Dispose()
            {
                _module.Node.Dispose();
            }
        }
    }
}