using System;
using System.Windows.Forms;

using Microsoft.Extensions.DependencyInjection;

using WindowsFormsLifetime;

namespace Tanji.Utilities;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Enables Windows Forms support, builds and starts the host, starts the startup <see cref="Form"/>,
    /// then waits for the startup form to close before shutting down.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add the required services to.</param>
    /// <param name="configure">The delegate for configuring the <see cref="WindowsFormsLifetimeOptions"/>.</param>
    /// <param name="preApplicationRunAction">The delegate to execute before the application starts running.</param>
    /// <returns>The same instance of the <see cref="IServiceCollection"/> for chaining.</returns>
    // TODO: Remove when new WindowsFormsLifetime package is published.
    public static IServiceCollection AddWindowsFormsLifetime<TStartForm>(this IServiceCollection services, 
        Action<WindowsFormsLifetimeOptions>? configure = null, 
        Action<IServiceProvider>? preApplicationRunAction = null) 
        where TStartForm : Form 
        => services
            .AddSingleton<TStartForm>()
            .AddSingleton(provider => new ApplicationContext(provider.GetRequiredService<TStartForm>()))
            .AddWindowsFormsLifetime(configure, preApplicationRunAction);
}
