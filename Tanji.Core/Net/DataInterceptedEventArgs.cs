using Tanji.Core.Net.Buffers;
using Tanji.Core.Net.Formats;
using Tanji.Core.Net.Messages;

namespace Tanji.Core.Net;

/// <summary>
/// Represents an intercepted message that will be returned to the caller with blocking/replacing information.
/// </summary>
public sealed class DataInterceptedEventArgs : EventArgs
{
    private readonly IHFormat _format;
    private readonly Func<Task>? _interceptAsync;
    private readonly Func<ReadOnlyMemory<byte>, CancellationToken, ValueTask>? _sendAsync;

    public DateTime Timestamp { get; }
    public Task WaitUntil { get; set; }

    public int Step { get; init; }
    public HMessage Message { get; init; }
    public ReadOnlyMemory<byte> Buffer { get; init; }

    public bool IsBlocked { get; set; }
    public bool WasRelayed { get; private set; }
    public bool HasContinued { get; private set; }

    public DataInterceptedEventArgs(IHFormat format, Func<Task>? interceptAsync, Func<ReadOnlyMemory<byte>, CancellationToken, ValueTask>? sendAsync)
    {
        _format = format;
        _sendAsync = sendAsync;
        _interceptAsync = interceptAsync;

        Timestamp = DateTime.Now;
    }

    public async void Continue(bool relay = false)
    {
        if (_interceptAsync == null) return;

        // This method should not be awaited
        _ = _interceptAsync();
        if (relay && _sendAsync != null)
        {
            await _sendAsync(Buffer, default).ConfigureAwait(false);
        }
    }
    public HPacketReader GetPacket() => new(_format, Buffer.Span);
}