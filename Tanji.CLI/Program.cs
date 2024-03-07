using System.Runtime.InteropServices;

using Tanji.Core;
using Tanji.Core.Network;
using Tanji.Core.Services;
using Tanji.Core.Habbo.Canvas;
using Tanji.Core.Configuration;

using Eavesdrop;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

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
    private readonly IWebInterceptionService _webInterception;
    private readonly IConnectionHandlerService _connectionHandler;
    private readonly IClientHandlerService<CachedGame> _clientHandler;

    public Program(ILogger<Program> logger, IWebInterceptionService webInterception, IClientHandlerService<CachedGame> clientHandler, IConnectionHandlerService connectionHandler)
    {
        _logger = logger;
        _clientHandler = clientHandler;
        _webInterception = webInterception;
        _connectionHandler = connectionHandler;

        _logger.LogDebug($"{nameof(Program)} ctor");
    }

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Intercepting Game Token(s)...");
        do
        {
            string ticket = await _webInterception.InterceptTicketAsync(cancellationToken).ConfigureAwait(false);
            _logger.LogInformation("Game Ticket: {Ticket}", ticket);

            CachedGame game = _clientHandler.PatchClient(HPlatform.Flash, null);
            _logger.LogInformation("Client Processed : {game.ClientPath}", game.Path);

            var connectionContext = new HConnectionContext(game);
            _ = await _connectionHandler.LaunchAndInterceptConnectionAsync(ticket, connectionContext, cancellationToken).ConfigureAwait(false);
        }
        while (!cancellationToken.IsCancellationRequested);
    }
}