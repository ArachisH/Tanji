using System;
using System.Configuration;
using System.Windows.Forms;

using Tanji.Windows;

using Tanji.Core.Services;

namespace Tanji;

public static class Program
{
    public static bool IsParentProcess { get; private set; }
    public static bool HasAdminPrivileges { get; private set; }
    public static ConfigurationService Configuration { get; private set; }

    [STAThread]
    private static void Main(string[] args)
    {
        Configuration = new ConfigurationService(ConfigurationManager.AppSettings);

        ApplicationConfiguration.Initialize();
        Application.Run(new MainFrm());
    }
}