using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Reflection;
using System.Net.Sockets;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

using Tanji.Network;
using Tanji.Controls;

using Sulakore.Modules;
using Sulakore.Network;
using Sulakore.Network.Protocol;

namespace Tanji.Services.Modules
{
    [ToolboxItem(true)]
    [DesignerCategory("UserControl")]
    public partial class ModulesPage : ObservablePage, IHaltable, IReceiver
    {
        private ModuleInfo[] _initializedModules;
        private readonly List<string> _hashBlacklist;
        private ConcurrentQueue<DataInterceptedEventArgs> _packetQueue;
        private static readonly Dictionary<string, ModuleInfo> _moduleCache;

        public DirectoryInfo ModulesDirectory { get; }
        public DirectoryInfo DependenciesDirectory { get; }
        public ObservableCollection<ModuleInfo> Modules { get; }

        private ModuleInfo _selectedModule;
        public ModuleInfo SelectedModule
        {
            get => _selectedModule;
            set
            {
                if (value == null)
                {
                    ModulesLv.SelectedIndices.Clear();
                }
                else
                {
                    value.PhysicalItem.Focused = true;
                    value.PhysicalItem.Selected = true;

                    _selectedModule = value;
                    RaiseOnPropertyChanged();
                }
            }
        }

        static ModulesPage()
        {
            _moduleCache = new Dictionary<string, ModuleInfo>();
        }
        public ModulesPage()
        {
            InitializeComponent();

            _initializedModules = new ModuleInfo[0];
            _hashBlacklist = new List<string>();
            _packetQueue = new ConcurrentQueue<DataInterceptedEventArgs>();

            AppDomain.CurrentDomain.AssemblyResolve += Assembly_Resolve;

            Modules = new ObservableCollection<ModuleInfo>();
            Modules.CollectionChanged += Modules_CollectionChanged;

            if (Master != null)
            {
                ModulesDirectory = Directory.CreateDirectory("Installed Modules");
                DependenciesDirectory = ModulesDirectory.CreateSubdirectory("Dependencies");
                LoadModules();
                try
                {
                    var listener = new TcpListener(IPAddress.Any, TService.DefaultModuleServer.Port);
                    listener.Start();

                    Task captureModulesTask = CaptureModulesAsync(listener);
                }
                catch { Program.Display(null, $"Failed to start module listener on port '{TService.DefaultModuleServer.Port}'."); }
            }
        }

        private void ModulesLv_ItemActivate(object sender, EventArgs e)
        {
            SelectedModule.Initialize();
        }
        private void ModulesLv_ItemSelectionStateChanged(object sender, EventArgs e)
        {
            _selectedModule = (ModuleInfo)ModulesLv.SelectedItem?.Tag;
            UninstallModuleBtn.Enabled = _selectedModule != null;

            if (_selectedModule != null)
            {
                RaiseOnPropertyChanged(nameof(SelectedModule));
            }
        }

        private void InstallModuleBtn_Click(object sender, EventArgs e)
        {
            InstallModuleDlg.FileName = string.Empty;
            if (InstallModuleDlg.ShowDialog() != DialogResult.Cancel)
            {
                Install(InstallModuleDlg.FileName);
            }
        }
        private void UninstallModuleBtn_Click(object sender, EventArgs e)
        {
            if (File.Exists(SelectedModule.Path))
            {
                File.Delete(SelectedModule.Path);
            }

            SelectedModule.Dispose();
            Modules.Remove(SelectedModule);

            SelectedModule = null;
        }

        private void Module_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var module = (ModuleInfo)sender;
            module.PhysicalItem.SubItems[3].Text = module.CurrentState;
            _initializedModules = Modules.Where(m => m.IsInitialized).ToArray();

            if (!Master.Config.IsQueuingModulePackets || !Master.IsConnected || module.Instance == null) return;

            //Task handleDataTask = Task.Factory.StartNew(() => // Simulate all intercepted data being received concurrently as if Continue() was called on each one.
            //{
            //    foreach (DataInterceptedEventArgs data in _packetQueue)
            //    {

            //        if (module.Instance == null) return; // Ensure the module has not been disposed from a different thread.
            //        try
            //        {
            //            if (data.IsOutgoing)
            //            {
            //                module.Instance.HandleOutgoing(data);
            //            }
            //            else module.Instance.HandleIncoming(data);
            //        }
            //        catch (Exception ex)
            //        {
            //            Program.Display(ex);
            //        }
            //    }
            //}, TaskCreationOptions.LongRunning);
        }
        private void Modules_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var module = (ModuleInfo)e.NewItems[0];

                ListViewItem item = ModulesLv.AddItem(module.Name, module.Description, module.Version, module.CurrentState);
                module.PhysicalItem = item;

                item.Tag = module;
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                var module = (ModuleInfo)e.OldItems[0];

                module.Dispose();
                ModulesLv.RemoveItem(module.PhysicalItem);
            }
        }

        private Assembly Assembly_Resolve(object sender, ResolveEventArgs args)
        {
            FileSystemInfo dependencyFile = GetDependencyFile(DependenciesDirectory, args.Name);
            if (dependencyFile == null) return null;

            return Assembly.Load(File.ReadAllBytes(dependencyFile.FullName));
        }

        public void Install(string modulePath)
        {
            // Check if the file was blacklisted based on its MD5 hash, if so, do not attempt to install.
            string hash = GetFileHash(modulePath);
            if (_hashBlacklist.Contains(hash)) return;

            // Check if this module is already installed.
            ModuleInfo module = GetModule(hash);
            if (module != null)
            {
                SelectedModule = module;
                module.FormUI?.BringToFront();
                return;
            }

            // Do not remove from, or empty the module cache.
            // There may be a case where a previously uninstalled module will be be reinstalled in the same session.
            if (!_moduleCache.TryGetValue(hash, out module))
            {
                // Load it through memory, do not feed a local file path/stream(don't want to lock the file).
                module = new ModuleInfo();
                module.Assembly = Assembly.Load(File.ReadAllBytes(modulePath));

                module.Hash = hash;
                module.PropertyChanged += Module_PropertyChanged;

                // Copy the required dependencies, since utilizing 'ExportedTypes' will attempt to load them when enumerating.
                CopyDependencies(modulePath, module.Assembly);
                try
                {
                    foreach (Type type in module.Assembly.ExportedTypes)
                    {
                        if (!typeof(IModule).IsAssignableFrom(type)) continue;

                        var moduleAtt = type.GetCustomAttribute<ModuleAttribute>();
                        if (moduleAtt == null) continue;

                        module.Type = type;
                        module.Name = moduleAtt.Name;
                        module.Description = moduleAtt.Description;
                        module.Version = module.Assembly.GetName().Version;

                        var authorAtts = type.GetCustomAttributes<AuthorAttribute>();
                        module.Authors.AddRange(authorAtts);

                        // Only add it to the cache if this is a valid module.
                        _moduleCache.Add(hash, module);
                        break;
                    }
                    if (module.Type == null) return;
                }
                finally
                {
                    if (module.Type == null)
                    {
                        _hashBlacklist.Add(module.Hash);
                    }
                }
            }

            string installPath = CopyFile(modulePath, hash);
            module.Path = installPath; // This property already might have been set from a previous installation, but it wouldn't hurt to re-set the value.
            Modules.Add(module);
        }
        public ModuleInfo GetModule(string hash)
        {
            return Modules.SingleOrDefault(m => m.Hash == hash);
        }

        private async Task HandleModuleDataAsync(ModuleInfo module)
        {
            try
            {
                while (module.Node.IsConnected)
                {
                    HPacket packet = await module.Node.ReceivePacketAsync().ConfigureAwait(false);
                    if (packet == null) break;

                    switch (packet.Id)
                    {
                        case 1:
                        {
                            string identifier = packet.ReadUTF8();
                            if (module.DataAwaiters.TryGetValue(identifier, out TaskCompletionSource<HPacket> handledDataSource))
                            {
                                handledDataSource.SetResult(packet);
                            }
                            break;
                        }
                        case 2:
                        {
                            byte[] packetData = packet.ReadBytes(packet.ReadInt32(1), 5);
                            if (packet.ReadBoolean()) // IsOutgoing
                            {
                                await Master.Connection.SendToServerAsync(packetData).ConfigureAwait(false);
                            }
                            else
                            {
                                await Master.Connection.SendToClientAsync(packetData).ConfigureAwait(false);
                            }
                            break;
                        }
                    }
                }
            }
            finally { Invoke(new MethodInvoker(() => Modules.Remove(module))); }
        }
        private async Task CaptureModulesAsync(TcpListener listener)
        {
            try
            {
                var moduleNode = new HNode(await listener.AcceptSocketAsync().ConfigureAwait(false));

                HPacket infoPacket = await moduleNode.ReceivePacketAsync().ConfigureAwait(false);
                if (infoPacket == null) return; // Module aborted connection

                var module = new ModuleInfo(moduleNode);
                module.PropertyChanged += Module_PropertyChanged;

                module.Version = Version.Parse(infoPacket.ReadUTF8());
                module.Name = infoPacket.ReadUTF8();
                module.Description = infoPacket.ReadUTF8();

                module.Authors.Capacity = infoPacket.ReadInt32();
                for (int i = 0; i < module.Authors.Capacity; i++)
                {
                    module.Authors.Add(new AuthorAttribute(infoPacket.ReadUTF8()));
                }

                Invoke(new MethodInvoker(() => Modules.Add(module))); // Module must be added to collection before calling Initialize()
                module.Initialize();

                Task handleModuleDataTask = HandleModuleDataAsync(module);
            }
            finally { Task captureModulesAsync = CaptureModulesAsync(listener); }
        }

        private void LoadModules()
        {
            foreach (FileSystemInfo fileSysInfo in ModulesDirectory.EnumerateFiles("*.*"))
            {
                string extension = fileSysInfo.Extension.ToLower();
                if (extension == ".exe" || extension == ".dll")
                {
                    try
                    {
                        Install(fileSysInfo.FullName);
                    }
                    catch (Exception ex)
                    {
                        Program.Display(ex, "Failed to install the assembly as a module.\r\nFile: " + fileSysInfo.Name);
                    }
                }
            }
        }
        private string GetFileHash(string path)
        {
            using (var md5 = MD5.Create())
            using (var fileStream = File.OpenRead(path))
            {
                return BitConverter.ToString(md5.ComputeHash(fileStream))
                    .Replace("-", string.Empty).ToLower();
            }
        }
        private void InvokeModules(Action<IModule> action)
        {
            foreach (ModuleInfo module in _initializedModules)
            {
                if (module.Instance == null) continue;
                action(module.Instance);
            }
        }
        private string CopyFile(string path, string uniqueId)
        {
            path = Path.GetFullPath(path);
            string fileExt = Path.GetExtension(path);
            string fileName = Path.GetFileNameWithoutExtension(path);

            string copiedFilePath = path;
            string fileNameSuffix = $"({uniqueId}){fileExt}";
            if (!path.EndsWith(fileNameSuffix))
            {
                copiedFilePath = Path.Combine(ModulesDirectory.FullName, fileName + fileNameSuffix);
                if (!File.Exists(copiedFilePath))
                {
                    File.Copy(path, copiedFilePath, true);
                }
            }
            return copiedFilePath;
        }
        private void QueueImportantPackets(DataInterceptedEventArgs e)
        {
            if (e.IsOutgoing) return;
            switch (Master.In[e.Packet.Id].Name)
            {
                case nameof(Master.In.RoomHeightMap):
                _packetQueue = new ConcurrentQueue<DataInterceptedEventArgs>();
                break;

                case nameof(Master.In.RoomUsers):
                _packetQueue.Enqueue(e);
                break;

                case nameof(Master.In.RoomWallItems):
                _packetQueue.Enqueue(e);
                break;

                case nameof(Master.In.RoomFloorItems):
                _packetQueue.Enqueue(e);
                break;
            }
        }
        private void CopyDependencies(string path, Assembly assembly)
        {
            AssemblyName[] references = assembly.GetReferencedAssemblies();
            var fileReferences = new Dictionary<string, AssemblyName>(references.Length);
            foreach (AssemblyName reference in references)
            {
                fileReferences[reference.Name] = reference;
            }

            string[] loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Select(a => a.GetName().Name)
                .ToArray();

            var sourceDirectory = new DirectoryInfo(Path.GetDirectoryName(path));
            IEnumerable<string> missingAssemblies = fileReferences.Keys.Except(loadedAssemblies);
            foreach (string missingAssembly in missingAssemblies)
            {
                string assemblyName = fileReferences[missingAssembly].FullName;
                FileSystemInfo dependencyFile = GetDependencyFile(DependenciesDirectory, assemblyName);
                if (dependencyFile == null)
                {
                    dependencyFile = GetDependencyFile(sourceDirectory, assemblyName);
                    if (dependencyFile != null)
                    {
                        string installDependencyPath = Path.Combine(DependenciesDirectory.FullName, dependencyFile.Name);
                        File.Copy(dependencyFile.FullName, installDependencyPath, true);
                    }
                }
            }
        }
        private FileSystemInfo GetDependencyFile(DirectoryInfo directory, string dependencyName)
        {
            FileSystemInfo[] libraries = directory.GetFileSystemInfos("*.dll");
            foreach (FileSystemInfo library in libraries)
            {
                string libraryName = AssemblyName.GetAssemblyName(library.FullName).FullName;
                if (libraryName == dependencyName)
                {
                    return library;
                }
            }
            return null;
        }

        #region IHaltable Implementation
        public void Halt()
        { }
        public void Restore(ConnectedEventArgs e)
        {
            InvokeModules(m => m.OnConnected());
        }
        #endregion
        #region IReceiver Implementation
        public bool IsReceiving => _initializedModules.Length > 0 || Master.Config.IsQueuingModulePackets;

        public void HandleData(DataInterceptedEventArgs e)
        {
            if (Master.Config.IsQueuingModulePackets)
            {
                QueueImportantPackets(e);
            }
            InvokeModules(m =>
            {
                try
                {
                    if (e.IsOutgoing)
                    {
                        m.HandleOutgoing(e);
                    }
                    else m.HandleIncoming(e);
                }
                catch (Exception ex)
                {
                    e.Restore();
                    Task.Factory.StartNew(() => Program.Display(ex));
                }
            });
        }
        public void HandleOutgoing(DataInterceptedEventArgs e) => HandleData(e);
        public void HandleIncoming(DataInterceptedEventArgs e) => HandleData(e);
        #endregion
    }
}