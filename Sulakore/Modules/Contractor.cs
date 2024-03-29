﻿using System.Reflection;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Runtime.Serialization;

using Sulakore.Habbo;
using Sulakore.Habbo.Web;
using Sulakore.Communication;
using Sulakore.Habbo.Messages;

namespace Sulakore.Modules;

public abstract class Contractor : IInstaller
{
    private readonly List<string> _installedAsHashes;
    private readonly List<Assembly> _invalidAssemblies;
    private readonly Dictionary<string, Type> _moduleTypes;
    private readonly Dictionary<Type, string> _modulePaths;
    private readonly Dictionary<Type, IModule> _initializedModules;
    private readonly Dictionary<Type, ModuleAttribute> _moduleAttributes;
    private readonly Dictionary<Type, IEnumerable<AuthorAttribute>> _authorAttributes;

    private static readonly Dictionary<string, Assembly> _cachedFileAsms;

    public DirectoryInfo ModulesDirectory { get; }
    public DirectoryInfo DependenciesDirectory { get; }

    public abstract HGame Game { get; }
    public abstract Incoming In { get; }
    public abstract Outgoing Out { get; }
    public abstract HGameData GameData { get; }
    public abstract IHConnection Connection { get; }
    public HHotel Hotel => GameData?.Hotel ?? HHotel.Unknown;

    static Contractor()
    {
        _cachedFileAsms = new Dictionary<string, Assembly>();
    }
    public Contractor(string installDirectory)
    {
        _installedAsHashes = new List<string>();
        _invalidAssemblies = new List<Assembly>();
        _moduleTypes = new Dictionary<string, Type>();
        _modulePaths = new Dictionary<Type, string>();
        _initializedModules = new Dictionary<Type, IModule>();
        _moduleAttributes = new Dictionary<Type, ModuleAttribute>();
        _authorAttributes = new Dictionary<Type, IEnumerable<AuthorAttribute>>();

        AppDomain.CurrentDomain.AssemblyResolve += Assembly_Resolve;

        ModulesDirectory = Directory.CreateDirectory(installDirectory);
        DependenciesDirectory = ModulesDirectory.CreateSubdirectory("Dependencies");
    }

    public bool InstallModule(string path)
    {
        path = Path.GetFullPath(path);
        if (!File.Exists(path)) return false;

        // Make a copy with a unique name.
        string fileHash = GetFileHash(path);
        string copiedFilePath = CopyFile(path, fileHash);

        Type moduleType = GetModuleType(copiedFilePath);
        if (_installedAsHashes.Contains(fileHash))
        {
            // This assembly/file is still installed.
            OnModuleReinstalled(moduleType);
            return true;
        }

        // Load the assembly, or retreive from the cache.
        Assembly fileAsm = null;
        if (!_cachedFileAsms.ContainsKey(copiedFilePath))
        {
            byte[] fileData = File.ReadAllBytes(path);

            fileAsm = Assembly.Load(fileData);
            _cachedFileAsms.Add(copiedFilePath, fileAsm);
        }
        else fileAsm = _cachedFileAsms[copiedFilePath];

        // Was this assembly marked invalid?
        if (_invalidAssemblies.Contains(fileAsm))
            return false;

        if (moduleType == null)
        {
            CopyDependencies(path, fileAsm);
            foreach (Type type in fileAsm.ExportedTypes)
            {
                var moduleAtt = type.GetCustomAttribute<ModuleAttribute>();
                _moduleAttributes[type] = moduleAtt;

                var authorAtts = type.GetCustomAttributes<AuthorAttribute>();
                _authorAttributes[type] = authorAtts;

                if (moduleAtt != null && typeof(IModule).IsAssignableFrom(type))
                {
                    moduleType = type;
                    _moduleTypes[copiedFilePath] = type;
                    _modulePaths[type] = copiedFilePath;
                    break;
                }
            }
        }

        if (moduleType == null)
        {
            // Do not remove from '_cachedFileAsms', otherwise assemblies will continue
            // to be added to the GAC, even though it's the same file/assembly.

            if (File.Exists(copiedFilePath))
                File.Delete(copiedFilePath);

            // Blacklist this assembly, so an installation re-attempt is not permitted.
            _invalidAssemblies.Add(fileAsm);
            return false;
        }
        else
        {
            _installedAsHashes.Add(fileHash);

            CopyDependencies(path, fileAsm);
            OnModuleInstalled(moduleType);
        }
        return true;
    }
    protected abstract void OnModuleInstalled(Type type);
    protected abstract void OnModuleReinstalled(Type type);

    public void UninstallModule(Type type)
    {
        string filePath = GetModuleFilePath(type);
        string fileHash = GetFileHash(filePath);

        if (File.Exists(filePath))
            File.Delete(filePath);

        if (_installedAsHashes.Contains(fileHash))
            _installedAsHashes.Remove(fileHash);

        DisposeModule(type);
        OnModuleUninstalled(type);
    }
    protected abstract void OnModuleUninstalled(Type type);

    public IModule InitializeModule(Type type)
    {
        // Multiple instances not supported.
        if (_initializedModules.ContainsKey(type))
        {
            return _initializedModules[type];
        }
        IModule module = null;
        try
        {
            module = (IModule)FormatterServices.GetUninitializedObject(type);
            module.Installer = this;

            ConstructorInfo moduleConstructor = type.GetConstructor(Type.EmptyTypes);
            moduleConstructor.Invoke(module, null);

            _initializedModules[type] = module;
            OnModuleInitialized(type);
        }
        catch
        {
            if (module != null)
            {
                _initializedModules.Remove(type);
                DisposeModule(type);
                module = null;
            }
            throw;
        }
        finally
        {
            if (module != null && typeof(IComponent).IsAssignableFrom(type))
            {
                ((IComponent)module).Disposed += ModuleComponent_Disposed;
            }
        }
        return module;
    }
    protected abstract void OnModuleInitialized(Type type);

    public void DisposeModule(Type type)
    {
        IModule module = GetModule(type);
        if (module == null) return;

        module.Dispose();
        if (_initializedModules.ContainsKey(type))
            _initializedModules.Remove(type);

        OnModuleDisposed(type);
    }
    protected abstract void OnModuleDisposed(Type type);

    public int GetInstalledCount()
    {
        return _installedAsHashes.Count;
    }
    public int GetInitializedCount()
    {
        return _initializedModules.Count;
    }
    public IModule GetModule(Type type)
    {
        IModule module = null;
        _initializedModules.TryGetValue(type, out module);
        return module;
    }
    public Type GetModuleType(string path)
    {
        Type moduleType = null;
        _moduleTypes.TryGetValue(path, out moduleType);
        return moduleType;
    }
    public string GetModuleFilePath(Type type)
    {
        string modulePath = string.Empty;
        _modulePaths.TryGetValue(type, out modulePath);
        return modulePath;
    }
    public ModuleAttribute GetModuleAttribute(Type type)
    {
        ModuleAttribute moduleAtt = null;
        _moduleAttributes.TryGetValue(type, out moduleAtt);
        return moduleAtt;
    }
    public IEnumerable<AuthorAttribute> GetAuthorAttributes(Type type)
    {
        IEnumerable<AuthorAttribute> authorAtts = null;
        _authorAttributes.TryGetValue(type, out authorAtts);
        return authorAtts;
    }

    protected string GetFileHash(string path)
    {
        using (var md5 = MD5.Create())
        using (var fileStream = File.OpenRead(path))
        {
            return BitConverter.ToString(
                md5.ComputeHash(fileStream))
                .Replace("-", string.Empty).ToLower();
        }
    }
    protected string CopyFile(string path, string uniqueId)
    {
        path = Path.GetFullPath(path);
        string fileExt = Path.GetExtension(path);
        string fileName = Path.GetFileNameWithoutExtension(path);

        string copiedFilePath = path;
        string fileNameSuffix = $"({uniqueId}){fileExt}";
        if (!path.EndsWith(fileNameSuffix))
        {
            copiedFilePath = Path.Combine(
                ModulesDirectory.FullName, fileName + fileNameSuffix);

            if (!File.Exists(copiedFilePath))
                File.Copy(path, copiedFilePath, true);
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

    private void ModuleComponent_Disposed(object sender, EventArgs e)
    {
        ((IComponent)sender).Disposed -= ModuleComponent_Disposed;
        DisposeModule(sender.GetType()); // It won't hurt to re-dispose, it's better than making another "special" method for it.
    }
    private Assembly Assembly_Resolve(object sender, ResolveEventArgs e)
    {
        FileSystemInfo dependency = GetDependencyFile(DependenciesDirectory, e.Name);
        if (dependency != null)
        {
            return Assembly.Load(File.ReadAllBytes(dependency.FullName));
        }
        return null;
    }
}