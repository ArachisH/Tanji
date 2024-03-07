using System.Net;
using System.Text;
using System.Runtime.InteropServices;

using Tanji.Core;
using Tanji.Core.Network;
using Tanji.Core.Services;
using Tanji.Core.Habbo.Canvas;
using Tanji.Core.Configuration;
using Tanji.Core.Habbo.Network.Buffers;
using Tanji.Core.Habbo.Network.Formats;

using Eavesdrop;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

using CommunityToolkit.HighPerformance.Buffers;

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
    private readonly IClientHandlerService<CachedGame> _clientHandler;

    public Program(ILogger<Program> logger, IWebInterceptionService webInterception, IClientHandlerService<CachedGame> clientHandler)
    {
        _logger = logger;
        _clientHandler = clientHandler;
        _webInterception = webInterception;

        _logger.LogDebug($"{nameof(Program)} ctor");
    }

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        static IPEndPoint? GetRemoteEndPoint(IHFormat packetFormat, ReadOnlySpan<byte> packetSpan)
        {
            var pktReader = new HPacketReader(packetFormat, packetSpan);
            string hostNameOrAddress = pktReader.ReadUTF8().Split('\0')[0];
            int port = pktReader.Read<int>();

            if (!IPAddress.TryParse(hostNameOrAddress, out IPAddress? address))
            {
                IPAddress[] addresses = Dns.GetHostAddresses(hostNameOrAddress);
                if (addresses.Length > 0) address = addresses[0];
            }

            return address != null ? new IPEndPoint(address, port) : null;
        }

        _logger.LogInformation("Intercepting Game Token(s)...");
        do
        {
            string ticket = await _webInterception.InterceptTicketAsync(cancellationToken).ConfigureAwait(false);
            _logger.LogInformation("Game Ticket: {Ticket}", ticket);

            CachedGame game = _clientHandler.PatchClient(HPlatform.Flash, null);
            _logger.LogInformation("Client Processed : {game.ClientPath}", game.ClientPath);

            var connection = new HConnection();
            var connectionOptions = new HConnectionOptions(game, game.AppliedPatches);

            ValueTask interceptLocalConnectionTask = connection.InterceptLocalConnectionAsync(connectionOptions, cancellationToken);
            _ = _clientHandler.LaunchClient(ticket, HPlatform.Flash, game.ClientPath);

            await interceptLocalConnectionTask.ConfigureAwait(false);
            while (connection.Local!.IsConnected)
            {
                using var writer = new ArrayPoolBufferWriter<byte>(64);
                int written = await connection.Local.ReceivePacketAsync(writer, cancellationToken).ConfigureAwait(false);

                IPEndPoint? remoteEndPoint = GetRemoteEndPoint(connectionOptions.SendPacketFormat, writer.WrittenSpan);
                await connection.EstablishRemoteConnection(connectionOptions, remoteEndPoint!, cancellationToken).ConfigureAwait(false);

                // TODO: Read all data
                break;
            }
        }
        while (!cancellationToken.IsCancellationRequested);
    }
}