﻿using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;

using Sulakore.Modules;

namespace Tanji.Pages.Modules.Handlers;

public class ModuleItem
{
    public Type Type { get; }
    public string Path { get; }
    public List<AuthorAttribute> Authors { get; }

    public Version Version { get; }
    public FileVersionInfo VersionInfo { get; }

    public string Name { get; }
    public string Description { get; }
    public ListViewItem ListItem { get; }

    public IModule Instance { get; set; }
    public Form ExtensionForm { get; set; }
    public bool IsInitialized { get; set; }

    public ModuleItem(Type type, Contractor contractor)
    {
        Type = type;
        Path = contractor.GetModuleFilePath(type);

        VersionInfo = FileVersionInfo.GetVersionInfo(Path);
        Version = new Version(VersionInfo.FileVersion);

        Authors = new List<AuthorAttribute>(contractor.GetAuthorAttributes(type));

        var moduleAtt = contractor.GetModuleAttribute(type);
        Description = moduleAtt.Description;
        Name = moduleAtt.Name;

        ListItem = new ListViewItem(new string[] {
            Name, Description, VersionInfo.FileVersion, "Uninitialized" });

        ListItem.Tag = this;
    }
}