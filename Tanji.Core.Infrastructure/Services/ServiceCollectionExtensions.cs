using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;

using Tanji.Core.Net;
using Tanji.Core.Infrastructure.ViewModels;
using Tanji.Core.Infrastructure.Configuration;
using Tanji.Core.Infrastructure.Services.Implementations;

namespace Tanji.Core.Infrastructure.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTanjiCore(this IServiceCollection services)
    {

        // Add Configuration
        services.AddOptions();
        services.AddSingleton<IPostConfigureOptions<TanjiOptions>, PostConfigureTanjiOptions>();

        // Add Singleton Services
        services.AddSingleton<IHotelStateService, HotelStateService>();
        services.AddSingleton<IClientHandlerService, ClientHandlerService>();
        services.AddSingleton<IConnectionHandlerService, ConnectionHandlerService>();
        services.AddSingleton<IWebInterceptionService, EavesdropInterceptionService>();
        services.AddSingleton<IRemoteEndPointResolverService<HotelEndPoint>, RemoteHotelEndPointResolverService>();

        // Register View Models
        services.AddSingleton<ConnectionViewModel>();
        services.AddSingleton<InjectionViewModel>();
        services.AddSingleton<ToolboxViewModel>();
        services.AddSingleton<ExtensionsViewModel>();
        services.AddSingleton<SettingsViewModel>();
        services.AddSingleton<LoggerViewModel>();

        // Add Hosted Services
        services.AddHostedService(provider => provider.GetRequiredService<IHotelStateService>());

        return services;
    }
}