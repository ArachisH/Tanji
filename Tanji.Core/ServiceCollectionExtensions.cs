using Tanji.Core.Services;
using Tanji.Core.ViewModels;
using Tanji.Core.Habbo.Canvas;
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
        services.AddSingleton<IConnectionHandlerService, ConnectionHandlerService>();
        services.AddSingleton<IClientHandlerService<CachedGame>, ClientHandlerService>();

        // Add view-models
        services.AddSingleton<ConnectionViewModel>();
        services.AddSingleton<InjectionViewModel>();
        services.AddSingleton<ToolboxViewModel>();
        services.AddSingleton<ExtensionsViewModel>();
        services.AddSingleton<SettingsViewModel>();

        return services;
    }
}