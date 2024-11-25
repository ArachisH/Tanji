using System;

using Tanji.Views;
using Tanji.Utilities;
using Tanji.Infrastructure.Services;
using Tanji.Infrastructure.Configuration;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.Configure<TanjiOptions>(builder.Configuration);
builder.Services.AddTanjiCore();

// Views (Windows, Dialogs, Pages)
builder.Services.AddWindowsFormsLifetime<MainView>();
builder.Services.AddSingleton<PacketLoggerView>();

var host = builder.Build();
Services = host.Services;
await host.StartAsync();

public partial class Program
{
    // TODO: Try get rid of this
    public static IServiceProvider? Services { get; private set; }
}