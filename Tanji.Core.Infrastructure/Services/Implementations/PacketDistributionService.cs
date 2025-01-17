using System.Text;

using Microsoft.Extensions.Logging;

using Tanji.Core.Net;
using Tanji.Core.Net.Buffers;
using Tanji.Core.Cryptography.Ciphers;
using Tanji.Core.Infrastructure.ViewModels;

namespace Tanji.Core.Infrastructure.Services.Implementations;

public sealed class PacketDistributionService : IPacketDistributionService
{
    private readonly Lock _consoleWriteLock;
    private readonly LoggerViewModel _loggerVM;
    private readonly InjectionViewModel _injectionVM;
    private readonly ExtensionsViewModel _extensionsVM;
    private readonly ILogger<PacketDistributionService> _logger;

    public bool IsHandlingInbound { get; set; } = true;
    public bool IsHandlingOutbound { get; set; } = true;

    public PacketDistributionService(ILogger<PacketDistributionService> logger,
        ExtensionsViewModel extensionsVM, LoggerViewModel loggerVM, InjectionViewModel injectionVM)
    {
        _logger = logger;
        _loggerVM = loggerVM;
        _injectionVM = injectionVM;
        _extensionsVM = extensionsVM;
        _consoleWriteLock = new Lock();
    }

    public async ValueTask<bool> PacketInboundAsync(Memory<byte> buffer, HNode source, HNode destination)
    {
        if (_loggerVM.IsHandlingInbound)
        {
            await _loggerVM.PacketInboundAsync(buffer, source, destination).ConfigureAwait(false);
        }
        return false;
    }
    public ValueTask<bool> PacketOutboundAsync(Memory<byte> buffer, HNode source, HNode destination)
    {
        var pktReader = new HPacketReader(destination.ReceivePacketFormat, buffer.Span);
        if (pktReader.Id == 4002)
        {
            string sharedKeyHex = pktReader.ReadUTF8();
            if (sharedKeyHex.Length % 2 != 0)
            {
                sharedKeyHex = "0" + sharedKeyHex;
            }

            byte[] sharedKeyBytes = Convert.FromHexString(sharedKeyHex);
            destination.EncryptCipher = new RC4(sharedKeyBytes);
        }
        lock (_consoleWriteLock)
        {
            _logger.LogTrace("[Outbound({Id}, {Length})] > {Buffer}\r\n--------", pktReader.Id, pktReader.Length, ToString(buffer.Span));
        }
        return ValueTask.FromResult(false);
    }

    private static string ToString(ReadOnlySpan<byte> bufferSpan)
    {
        string result = Encoding.UTF8.GetString(bufferSpan);
        for (int i = 0; i <= 13; i++)
        {
            result = result.Replace(((char)i).ToString(), "[" + i + "]");
        }
        return result;
    }
}