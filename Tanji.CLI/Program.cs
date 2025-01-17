using System.Runtime.InteropServices;

using Tanji.Core.Canvas;
using Tanji.Core.Net.Interception;
using Tanji.Core.Infrastructure.Services;
using Tanji.Core.Infrastructure.Configuration;

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
        builder.Services.Configure<TanjiOptions>(builder.Configuration)
            .AddSingleton<Program>()
            .AddTanjiCore();

        Console.Title = $"Tanji(Core) - Press any key to exit...";
        IHost host = builder.Build();

        Program app = host.Services.GetRequiredService<Program>();
        await app.RunAsync(CTS.Token).ConfigureAwait(false);
    }
    #endregion

    private readonly ILogger<Program> _logger;
    private readonly IClientHandlerService _clientHandler;
    private readonly IWebInterceptionService _webInterception;
    private readonly IConnectionHandlerService _connectionHandler;

    public Program(ILogger<Program> logger,
        IWebInterceptionService webInterception,
        IClientHandlerService clientHandler,
        IConnectionHandlerService connectionHandler)
    {
        _logger = logger;
        _clientHandler = clientHandler;
        _webInterception = webInterception;
        _connectionHandler = connectionHandler;

        _logger.LogDebug($"{nameof(Program)} ctor");
    }

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        _webInterception.Start();
        _logger.LogInformation("Intercepting Game Token(s)...");
        do
        {
            string ticket = true ? "hhus.ABC.v4" : await _webInterception.InterceptTicketAsync(cancellationToken).ConfigureAwait(false);
            _logger.LogInformation("Ticket Acquired: {ticket}", ticket);
            _webInterception.Stop();

            IGame game = await _clientHandler.PatchClientAsync(HPlatform.Flash).ConfigureAwait(false);
            var context = new HConnectionContext(game);

            HConnection connection = await _connectionHandler.LaunchAndInterceptConnectionAsync(ticket, context, cancellationToken);
            await connection.AttachNodesAsync(cancellationToken).ConfigureAwait(false);
        }
        while (!cancellationToken.IsCancellationRequested);
    }
}