using System;

using Tanji.Views;
using Tanji.Core.Infrastructure.Services;
using Tanji.Core.Infrastructure.Configuration;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

// Views (Windows, Dialogs, Pages)
var builder = Host.CreateApplicationBuilder(args);
builder.Services.Configure<TanjiOptions>(builder.Configuration)
    .AddWindowsFormsLifetime<MainView>()
    .AddSingleton<PacketLoggerView>()
    .AddTanjiCore();

var host = builder.Build();
Services = host.Services;
await host.StartAsync();

public partial class Program
{
    // TODO: Try get rid of this
    public static IServiceProvider? Services { get; private set; }
}