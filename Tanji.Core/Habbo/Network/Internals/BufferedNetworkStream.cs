using System.Net.Sockets;

namespace Tanji.Core.Habbo.Network;

internal sealed class BufferedNetworkStream : Stream
{
    private const int DEFAULT_BUFFER_SIZE = 4096;

    private readonly NetworkStream _networkStream;
    private readonly BufferedStream _readBuffer, _writeBuffer;

    private bool _disposed;

    public int ReadBufferSize { get; init; }
    public int WriteBufferSize { get; init; }

    public BufferedNetworkStream(Socket socket)
        : this(socket, DEFAULT_BUFFER_SIZE, DEFAULT_BUFFER_SIZE)
    { }
    public BufferedNetworkStream(Socket socket, int readBufferSize, int writeBufferSize)
    {
        ReadBufferSize = readBufferSize;
        WriteBufferSize = writeBufferSize;

        _networkStream = new NetworkStream(socket, true);
        _readBuffer = new BufferedStream(_networkStream, readBufferSize);
        _writeBuffer = new BufferedStream(_networkStream, writeBufferSize);
    }

    public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default) => _readBuffer.ReadAsync(buffer, cancellationToken);
    public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default) => _writeBuffer.WriteAsync(buffer, cancellationToken);

    #region Stream Implementation
    public override long Position
    {
        get => throw new NotSupportedException();
        set => throw new NotSupportedException();
    }
    public override bool CanRead => true;
    public override bool CanSeek => false;
    public override bool CanWrite => true;
    public override long Length => throw new NotSupportedException();

    public override void Flush() => _writeBuffer.Flush();
    public override Task FlushAsync(CancellationToken cancellationToken) => _writeBuffer.FlushAsync(cancellationToken);

    public override int Read(byte[] buffer, int offset, int count) => throw new NotSupportedException();
    public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();

    public override void SetLength(long value) => throw new NotSupportedException();
    public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing && _disposed)
        {
            _networkStream.Dispose();
            _readBuffer.Dispose();
            _writeBuffer.Dispose();
            _disposed = true;
        }
    }
    #endregion
}