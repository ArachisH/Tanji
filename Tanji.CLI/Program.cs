using System.Runtime.InteropServices;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

using Tanji.Core;
using Tanji.Core.Network;
using Tanji.Core.Services;
using Tanji.Core.Habbo.Canvas;
using Tanji.Core.Configuration;

using Eavesdrop;

namespace Tanji.CLI;

public class Program
{
    #region Application Startup
    private static CancellationTokenSource CTS { get; } = new();
    public static async Task Main(string[] args)
    {
        static void CleanUp(PosixSignalContext context)
        {
            CTS.Cancel();
            context.Cancel = true;
            Eavesdropper.Terminate();
        }

        // These are not flags, and so they can't be combined into a single registration.
        PosixSignalRegistration.Create(PosixSignal.SIGINT, CleanUp);
        PosixSignalRegistration.Create(PosixSignal.SIGHUP, CleanUp);

        var builder = Host.CreateApplicationBuilder(args);
        builder.Services.Configure<TanjiOptions>(builder.Configuration);
        builder.Services.AddSingleton<Program>();
        builder.Services.AddTanjiCore();

        Console.Title = $"Tanji(Core) - Press any key to exit...";
        IHost host = builder.Build();

        Program app = host.Services.GetRequiredService<Program>();
        await app.RunAsync(CTS.Token).ConfigureAwait(false);
    }
    #endregion

    private readonly ILogger<Program> _logger;
    private readonly IInterceptionService _interception;

    public Program(ILogger<Program> logger, IInterceptionService interception)
    {
        _logger = logger;
        _interception = interception;

        _logger.LogDebug($"{nameof(Program)} ctor");
    }

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Intercepting Game Token(s)...");
        do
        {
            string ticket = await _interception.InterceptGameTicketAsync(cancellationToken).ConfigureAwait(false);
            _logger.LogInformation("Game Ticket: {Ticket}", ticket);

            var connection = new HConnection();
            connection.Connected += Connection_Connected;

            await _interception.LaunchInterceptableClientAsync(ticket, connection, HPlatform.Flash, cancellationToken).ConfigureAwait(false);
        }
        while (!cancellationToken.IsCancellationRequested && _interception.IsInterceptingWebTraffic);
    }

    private void Connection_Connected(object? sender, ConnectedEventArgs e)
    {
        var connection = (HConnection?)sender;
        if (connection == null) return;

    }
}