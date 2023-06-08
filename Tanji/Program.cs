﻿using System;
using System.Windows.Forms;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

using Tanji.Views;
using Tanji.Services;
using Tanji.Core.Services;
using Tanji.Core.ViewModels;

namespace Tanji;

static class Program
{
    public static bool IsParentProcess { get; private set; }
    public static bool HasAdminPrivileges { get; private set; }

    public static IServiceProvider? ServiceProvider { get; private set; }
    public static IConfigurationService Configuration { get; private set; }

    static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                // ViewModels
                services.AddSingleton<ConnectionViewModel>();
                services.AddSingleton<InjectionViewModel>();
                services.AddSingleton<ToolboxViewModel>();
                services.AddSingleton<ExtensionsViewModel>();
                services.AddSingleton<SettingsViewModel>();

                // Views (Windows, Dialogs, Pages)
                services.AddSingleton<MainView>();

                // Services
                services.AddSingleton<IConfigurationDataProviderService, ConfigurationDataProviderService>();                                                                   // Provides the data
                services.AddSingleton<IConfigurationService, ConfigurationService>(s => new ConfigurationService(s.GetRequiredService<IConfigurationDataProviderService>()));   // Parses the data
            });
    }

    [STAThread]
    static void Main(string[] args)
    {
        IHost host = CreateHostBuilder(args).Build();
        ServiceProvider = host.Services;

        Configuration = ServiceProvider.GetRequiredService<IConfigurationService>();

        ApplicationConfiguration.Initialize();
        Application.Run(ServiceProvider.GetRequiredService<MainView>());
    }
}