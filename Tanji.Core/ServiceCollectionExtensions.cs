using Tanji.Core.Services;
using Tanji.Core.ViewModels;
using Tanji.Core.Configuration;

using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;

namespace Tanji.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTanjiCore(this IServiceCollection services)
    {
        // Add configuration
        services.AddOptions();
        services.AddSingleton<IPostConfigureOptions<TanjiOptions>, PostConfigureTanjiOptions>();

        // Services
        services.AddSingleton<IWebInterceptionService, WebInterceptionService>();
        services.AddSingleton<IClientHandlerService, ClientHandlerService>();

        services.AddSingleton<PacketMiddlemanService>();
        services.AddSingleton<IConnectionHandlerService<PacketMiddlemanService>, ConnectionHandlerService>();

        // Add view-models
        services.AddSingleton<ConnectionViewModel>();
        services.AddSingleton<InjectionViewModel>();
        services.AddSingleton<ToolboxViewModel>();
        services.AddSingleton<ExtensionsViewModel>();
        services.AddSingleton<SettingsViewModel>();

        return services;
    }
}