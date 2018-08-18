using Sulakore.Modules;
using Sulakore.Network;
using Sulakore.Network.Protocol;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tanji.Controls;
using Tanji.Network;

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
                _selectedModule = value;
                RaiseOnPropertyChanged();
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

            Modules = new ObservableCollection<ModuleInfo>();
            Modules.CollectionChanged += Modules_CollectionChanged;

            if (Master != null)
            {
                ModulesDirectory = Directory.CreateDirectory("Installed Modules");
                DependenciesDirectory = ModulesDirectory.CreateSubdirectory("Dependencies");
                LoadModules();
                try
                {
                    var listener = new TcpListener(IPAddress.Any, TService.REMOTE_MODULE_PORT);
                    listener.Start();

                    Task captureModulesTask = CaptureModulesAsync(listener);
                }
                catch { Program.Display(null, $"Failed to start module listener on port '{TService.REMOTE_MODULE_PORT}'."); }
            }

            //?AppDomain.CurrentDomain.AssemblyResolve += Assembly_Resolve;
        }

        private async Task HandleModuleDataAsync(ModuleInfo module)
        {
            try
            {
                while (module.Node.IsConnected)
                {
                    HPacket packet = await module.Node.ReceivePacketAsync().ConfigureAwait(false);
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
            finally
            {
                Invoke(new MethodInvoker(() =>
                {
                    module.Dispose();
                    Modules.Remove(module);
                }));
            }
        }
        private async Task CaptureModulesAsync(TcpListener listener)
        {
            try
            {
                var moduleNode = new HNode(await listener.AcceptSocketAsync());

                moduleNode.InFormat = HFormat.EvaWire;
                moduleNode.OutFormat = HFormat.EvaWire;
                HPacket infoPacket = await moduleNode.ReceivePacketAsync();

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

        public ModuleInfo GetModule(string hash)
        {
            return Modules.SingleOrDefault(m => m.Hash == hash);
        }
        public IEnumerable<ModuleInfo> GetInitializedModules()
        {
            return Modules.Where(m => m.IsInitialized);
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
                        //?Install(fileSysInfo.FullName);
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
                        string installDependencyPath = Path.Combine(
                           DependenciesDirectory.FullName, dependencyFile.Name);

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

        private void Module_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ModuleInfo.Instance):
                {
                    IsReceiving = (GetInitializedModules().Count() > 0);
                    break;
                }
            }
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
        private void Modules_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            _safeModules = Modules.ToArray();
        }

        #region IHaltable Implementation
        public void Halt()
        { }
        public void Restore(ConnectedEventArgs e)
        { }
        #endregion
        #region IReceiver Implementation
        public bool IsReceiving { get; set; }
        public void HandleData(DataInterceptedEventArgs e)
        {
            foreach (ModuleInfo module in _safeModules)
            {
                IModule instance = module.Instance;
                if (instance == null) continue;
                try
                {
                    if (e.IsOutgoing)
                    {
                        instance.HandleOutgoing(e);
                    }
                    else instance.HandleIncoming(e);
                }
                catch (Exception ex)
                {
                    e.Restore();
                    Task.Factory.StartNew(() => Program.Display(ex));
                }
            }
        }
        public void HandleOutgoing(DataInterceptedEventArgs e) => HandleData(e);
        public void HandleIncoming(DataInterceptedEventArgs e) => HandleData(e);
        #endregion
    }
}