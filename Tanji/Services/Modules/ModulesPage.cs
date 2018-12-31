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
        private ModuleInfo[] _safeModules;

        private readonly List<string> _hashBlacklist;
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
                    foreach (ListViewItem item in ModulesLv.Items)
                    {
                        if ((ModuleInfo)item.Tag != value) continue;
                        item.Focused = true;
                        item.Selected = true;
                    }

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

            _safeModules = new ModuleInfo[0];
            _hashBlacklist = new List<string>();

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
            switch (e.PropertyName)
            {
                case nameof(ModuleInfo.Instance):
                {
                    IsReceiving = GetInitializedModules().Count() > 0;
                    break;
                }
            }
        }
        private void Modules_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var module = (ModuleInfo)e.NewItems[0];

                ListViewItem item = ModulesLv.AddItem(module.Name, module.Description, module.Version, module.CurrentState);
                module.PhysicalObject = item;

                item.Tag = module;
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                var module = (ModuleInfo)e.OldItems[0];

                module.Dispose();
                ModulesLv.RemoveItem(module.PhysicalObject);
            }
            _safeModules = Modules.ToArray();
        }

        private Assembly Assembly_Resolve(object sender, ResolveEventArgs args)
        {
            FileSystemInfo dependencyFile = GetDependencyFile(DependenciesDirectory, args.Name);
            if (dependencyFile != null)
            {
                return Assembly.Load(File.ReadAllBytes(dependencyFile.FullName));
            }
            return null;
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
                        module.EntryType = moduleAtt.EntryType;
                        module.Description = moduleAtt.Description;
                        module.PropertyName = moduleAtt.PropertyName;
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
        public IEnumerable<ModuleInfo> GetInitializedModules()
        {
            return Modules.Where(m => m.IsInitialized);
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
                            TaskCompletionSource<HPacket> handledDataSource = null;
                            if (module.DataAwaiters.TryGetValue(identifier, out handledDataSource))
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
                var moduleNode = new HNode(await listener.AcceptSocketAsync());

                moduleNode.InFormat = HFormat.EvaWire;
                moduleNode.OutFormat = HFormat.EvaWire;

                HPacket infoPacket = await moduleNode.ReceivePacketAsync();
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
                Modules.Add(module);

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
            foreach (ModuleInfo module in _safeModules)
            {
                IModule instance = module.Instance;
                if (instance == null) continue;
                action(instance);
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
        public bool IsReceiving { get; private set; }
        public void HandleData(DataInterceptedEventArgs e)
        {
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