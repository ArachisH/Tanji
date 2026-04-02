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
    public void WritePacketLog_IncrementsRepetitionsForRepeatedPackets(IEnumerable<(bool IsOutgoing, byte[] Buffer, int Repetitions)> packets)
    {
        PacketLogHandlerService service = CreateService(true);
        foreach ((bool IsOutgoing, byte[] Buffer, int Repetitions) expected in packets)
        {
            PacketLog? actual = service.WritePacketLog(expected.Buffer, expected.IsOutgoing, IHFormat.EvaWire, string.Empty);

            Assert.NotNull(actual);
            Assert.Equal(expected.Repetitions, actual.Repetitions);
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

    public static TheoryData<IEnumerable<(bool IsOutgoing, byte[] Packet, int Repetitions)>> PacketBuffersWithRepeats => new()
    {
        new (bool, byte[], int)[]
        {
            (true, [0, 0, 0, 2, 0, 5], 1),
            (true, [0, 0, 0, 2, 0, 5], 2),
            (true, [0, 0, 0, 2, 0, 5], 3),
            (true, [0, 0, 0, 2, 0, 6], 1),
            (false, [0, 0, 0, 30, 0, 7, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99], 1),
            (false, [0, 0, 0, 2, 0, 8], 1),
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