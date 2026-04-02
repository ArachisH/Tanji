using System.Drawing;
using System.Diagnostics;

using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging.Abstractions;

using Tanji.Core.Canvas;
using Tanji.Core.Net.Formats;
using Tanji.Core.Net.Messages;
using Tanji.Core.Infrastructure.Models;
using Tanji.Core.Infrastructure.Services;
using Tanji.Core.Infrastructure.Configuration;
using Tanji.Core.Infrastructure.Services.Implementations;

namespace Tanji.Core.Infrastructure.Tests;

public sealed class PacketLogBuilderServiceTests
{
    [Theory]
    [MemberData(nameof(PacketBuffersWithRepeats))]
    public void Build_PacketLogs_ReturnsNullOnRepetitions(IEnumerable<(bool IsOutgoing, byte[] Buffer, bool IsNull)> packets)
    {
        PacketLogHandlerService service = CreateService(true);
        foreach ((bool IsOutgoing, byte[] Buffer, bool IsNull) packet in packets)
        {
            PacketLog? pLog = service.WritePacketLog(packet.Buffer, packet.IsOutgoing, IHFormat.EvaWire, string.Empty);
            if (packet.IsNull)
            {
                Assert.Null(pLog);
            }
            else Assert.NotNull(pLog);
        }
    }

    private static ProxyProvider CreateEmptyProxyProvider() => new()
    {
        Address = null,
        Username = null,
        Password = null
    };
    private static PacketLogHandlerService CreateService(bool compactRepetitions)
    {
        var options = Options.Create(new TanjiOptions
        {
            UnityInterceptionTriggers = [],
            FlashInterceptionTriggers = [],
            HttpSystemProxy = CreateEmptyProxyProvider(),
            SOCKS5ClientProxy = CreateEmptyProxyProvider(),
            GameListenPort = 0,
            ProxyListenPort = 0,
            ModulesListenPort = 0,
            UIScheme = Color.Empty,
            IsCheckingForUpdates = false,
            LauncherPath = string.Empty,
            ProxyOverrides = [],
            IsUsingAirDebugLauncher = false,
            PacketLoggingOptions = new PacketLoggingOptions
            {
                IsCompactingRepetitions = compactRepetitions,
                IsLoggingStructure = false,
                IsLoggingMessageName = false,
                IsLoggingMessageHash = false
            }
        });

        return new PacketLogHandlerService(
            NullLogger<PacketLogHandlerService>.Instance,
            options,
            new FakeClientHandlerService());
    }

    public static TheoryData<IEnumerable<(bool IsOutgoing, byte[] Packet, bool IsNull)>> PacketBuffersWithRepeats => new()
    {
        new (bool, byte[], bool)[]
        {
            (true, [0, 0, 0, 2, 0, 5], false),
            (true, [0, 0, 0, 2, 0, 5], true),
            (true, [0, 0, 0, 2, 0, 5], true),
            (true, [0, 0, 0, 2, 0, 6], false),
            (false, [0, 0, 0, 30, 0, 7, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99], false),
            (false, [0, 0, 0, 2, 0, 8], false),
        },
    };

    private sealed class FakeClientHandlerService : IClientHandlerService
    {
        public DirectoryInfo MessagesDirectory { get; } = new(".");
        public DirectoryInfo PatchedClientsDirectory { get; } = new(".");

        public Task<IGame> PatchClientAsync(HPlatform platform, string? clientPath = null)
            => throw new NotSupportedException();

        public Task<Process> LaunchClientAsync(HPlatform platform, string ticket, string? clientPath = null)
            => throw new NotSupportedException();

        public bool TryGetIdentifiers(string? revision, out Outgoing? outgoing, out Incoming? incoming)
        {
            outgoing = null;
            incoming = null;
            return false;
        }
    }
}