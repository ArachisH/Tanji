using System;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

using Tanji.Views;
using Tanji.Utilities;

using Tanji.Core;
using Tanji.Core.Configuration;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<TanjiOptions>(builder.Configuration);
builder.Services.AddTanjiCore();

// Views (Windows, Dialogs, Pages)
builder.Services.AddWindowsFormsLifetime<MainView>();
builder.Services.AddTransient<PacketLoggerView>();

var host = builder.Build();

Services = host.Services;

await host.StartAsync();

public partial class Program
{
    // TODO: Try get rid of this
    public static IServiceProvider? Services { get; private set; }
}