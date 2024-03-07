using System.Net;
using System.Text;
using System.Buffers;
using System.Net.Sockets;
using System.Buffers.Text;
using System.Net.Security;
using System.Security.Cryptography;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;

using Tanji.Core.Cryptography.Ciphers;
using Tanji.Core.Habbo.Network.Formats;

using CommunityToolkit.HighPerformance.Buffers;

namespace Tanji.Core.Habbo.Network;

public sealed class HNode : IDisposable
{
    private static ReadOnlySpan<byte> _okBytes => "OK"u8;
    private static ReadOnlySpan<byte> _startTLSBytes => "StartTLS"u8;
    private static ReadOnlySpan<byte> _rfc6455GuidBytes => "258EAFA5-E914-47DA-95CA-C5AB0DC85B11"u8;
    private static ReadOnlySpan<byte> _secWebSocketKeyBytes => "Sec-WebSocket-Key: "u8;
    private static ReadOnlySpan<byte> _upgradeWebSocketResponseBytes => "HTTP/1.1 101 Switching Protocols\r\nConnection: Upgrade\r\nUpgrade: websocket\r\nSec-WebSocket-Accept: "u8;

    private readonly Socket _socket;
    private readonly SemaphoreSlim _sendSemaphore, _receiveSemaphore;

    private bool _disposed;
    private Stream _socketStream;
    private Stream? _webSocketStream;

    public EndPoint? RemoteEndPoint { get; }
    public int BypassReceiveSecureTunnel { get; set; }

    public bool IsUpgraded { get; private set; }
    public bool IsWebSocket => _webSocketStream != null;
    public bool IsConnected => !_disposed && _socket.Connected;

    public IStreamCipher? EncryptCipher { get; set; }
    public IStreamCipher? DecryptCipher { get; set; }

    /// <summary>
    /// The format used by the client when receiving packets from the server.
    /// Alternatively, the format used by the server when sending packets to the client.
    /// </summary>
    public required IHFormat ReceivePacketFormat { get; init; }

    [SetsRequiredMembers]
    public HNode(Socket socket, IHFormat receivePacketFormat)
    {
        socket.NoDelay = true;
        socket.LingerState = new LingerOption(false, 0);

        _socket = socket;
        _socketStream = new BufferedNetworkStream(socket);

        _sendSemaphore = new SemaphoreSlim(1, 1);
        _receiveSemaphore = new SemaphoreSlim(1, 1);

        RemoteEndPoint = socket.RemoteEndPoint;
        ReceivePacketFormat = receivePacketFormat;
    }

    public async ValueTask SendAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
    {
        if (!IsConnected) return;
        if (buffer.Length == 0) return;

        await _sendSemaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            await _socketStream.WriteAsync(buffer, cancellationToken).ConfigureAwait(false);
            await _socketStream.FlushAsync(cancellationToken).ConfigureAwait(false);
            return;
        }
        finally { _sendSemaphore.Release(); }
    }
    public async ValueTask<int> ReceiveAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
    {
        if (!IsConnected) return -1;
        if (buffer.Length == 0) return 0;

        await _receiveSemaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
        try { return await ReadFromStreamAsync(buffer, cancellationToken).ConfigureAwait(false); }
        catch { return -1; }
        finally { _receiveSemaphore.Release(); }
    }

    /// <summary>
    /// Sends an encrypted packet by overwriting the original buffer space.
    /// This is a destructive process to the original buffer being passed.
    /// </summary>
    /// <param name="buffer">The buffer to overwrite by encryption.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    public async ValueTask SendPacketAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
    {
        if (!IsConnected) return;
        if (buffer.Length == 0) return;

        await _sendSemaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            if (EncryptCipher != null)
            {
                Encipher(EncryptCipher, buffer, IsWebSocket);
            }
            await _socketStream.WriteAsync(buffer, cancellationToken).ConfigureAwait(false);
            await _socketStream.FlushAsync(cancellationToken).ConfigureAwait(false);
        }
        finally { _sendSemaphore.Release(); }
    }
    /// <summary>
    /// Sends a packet by encrypting a copy of the original buffer.
    /// </summary>
    /// <param name="buffer">The read-only buffer to copy and encrypt.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async ValueTask SendPacketAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
    {
        if (!IsConnected) return;
        if (buffer.Length == 0) return;

        MemoryOwner<byte>? tempBufferOwner = null;
        await _sendSemaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            if (EncryptCipher != null)
            {
                tempBufferOwner = MemoryOwner<byte>.Allocate(buffer.Length);
                Encipher(EncryptCipher, buffer.Span, tempBufferOwner.Span, IsWebSocket);

                buffer = tempBufferOwner.Memory;
            }
            await _socketStream.WriteAsync(buffer, cancellationToken).ConfigureAwait(false);
            await _socketStream.FlushAsync(cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            _sendSemaphore.Release();
            tempBufferOwner?.Dispose();
        }
    }

    public async ValueTask<int> ReceivePacketAsync(IBufferWriter<byte> writer, CancellationToken cancellationToken = default)
    {
        if (!IsConnected) return -1;

        int totalReceived = 0;
        await _receiveSemaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            Memory<byte> buffer = writer.GetMemory(ReceivePacketFormat.MinBufferSize);
            Memory<byte> header = buffer.Slice(0, ReceivePacketFormat.MinBufferSize);

            totalReceived = await ReadFromStreamAsync(header, cancellationToken).ConfigureAwait(false);
            if (!ReceivePacketFormat.TryReadHeader(header.Span, out int length, out short id, out _)) return -1;

            if (length == ReceivePacketFormat.MinPacketLength)
            {
                if (DecryptCipher != null)
                {
                    Encipher(DecryptCipher, header, IsWebSocket);
                }
                writer.Advance(totalReceived);
                return totalReceived;
            }

            int bodyReceived = 0;
            int bodyAvailable = length - ReceivePacketFormat.MinPacketLength;
            while (bodyAvailable > 0)
            {
                int bufferAvailable = buffer.Length - totalReceived;
                if (bufferAvailable == 0)
                { }
                else if (bufferAvailable < 0)
                { }

                bodyReceived = await ReadFromStreamAsync(buffer.Slice(totalReceived, Math.Min(bodyAvailable, bufferAvailable)), cancellationToken).ConfigureAwait(false);
                bodyAvailable -= bodyReceived;
                totalReceived += bodyReceived;
            }

            if (DecryptCipher != null)
            {
                Encipher(DecryptCipher, buffer.Slice(0, totalReceived), IsWebSocket);
            }
            writer.Advance(totalReceived);
        }
        finally { _receiveSemaphore.Release(); }
        return totalReceived;
    }

    public async Task<bool> UpgradeToWebSocketClientAsync(CancellationToken cancellationToken = default)
    {
        static string GenerateWebSocketKey()
        {
            Span<byte> keyGenerationBuffer = stackalloc byte[24];
            RandomNumberGenerator.Fill(keyGenerationBuffer.Slice(0, 16));

            Base64.EncodeToUtf8InPlace(keyGenerationBuffer, 16, out int encodedSize);
            return Encoding.UTF8.GetString(keyGenerationBuffer.Slice(0, encodedSize));
        }
        static bool IsTLSAccepted(ReadOnlySpan<byte> message) => message.SequenceEqual(_okBytes);
        const string requestHeaders = "Connection: Upgrade\r\n"
                                      + "Pragma: no-cache\r\n"
                                      + "Cache-Control: no-cache\r\n"
                                      + "User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.67 Safari/537.36 Edg/87.0.664.52\r\n"
                                      + "Upgrade: websocket\r\n"
                                      + "Origin: https://images.habbogroup.com\r\n"
                                      + "Sec-WebSocket-Version: 13\r\n"
                                      + "Accept-Encoding: gzip, deflate, br\r\n"
                                      + "Accept-Language: en-US,en;q=0.9";

        // Initialize the top-most secure tunnel where ALL data will be read/written from/to.
        var secureSocketStream = new SslStream(_socketStream, false, ValidateRemoteCertificate);
        _socketStream = secureSocketStream; // Take ownership of the main input/output stream, and override the field with an SslStream instance that wraps around it.

        if (RemoteEndPoint is not IPEndPoint remoteIPEndPoint)
        {
            throw new NullReferenceException("Unable to determine the remote IP address of the socket.");
        }

        var sslClientAuthOptions = new SslClientAuthenticationOptions()
        {
            TargetHost = remoteIPEndPoint.Address.ToString()
        };

        await secureSocketStream.AuthenticateAsClientAsync(sslClientAuthOptions, cancellationToken).ConfigureAwait(false);
        if (!secureSocketStream.IsAuthenticated) return false;

        string webRequest = $"GET /websocket HTTP/1.1\r\nHost: {RemoteEndPoint}\r\n{requestHeaders}\r\nSec-WebSocket-Key: {GenerateWebSocketKey()}\r\n\r\n";
        await SendAsync(Encoding.UTF8.GetBytes(webRequest), cancellationToken).ConfigureAwait(false);

        using MemoryOwner<byte> receiveOwner = MemoryOwner<byte>.Allocate(256);
        int received = await ReceiveAsync(receiveOwner.Memory, cancellationToken).ConfigureAwait(false);

        // Create the mask that will be used for the WebSocket payloads.
        //RandomNumberGenerator.Fill(_mask);

        IsUpgraded = true;
        _socketStream = _webSocketStream = new WebSocketStream(_socketStream, true, null, false); // Anything now being sent or received through the stream will be parsed using the WebSocket protocol.

        await SendAsync(_startTLSBytes.ToArray(), cancellationToken).ConfigureAwait(false);
        received = await ReceiveAsync(receiveOwner.Memory, cancellationToken).ConfigureAwait(false);
        if (!IsTLSAccepted(receiveOwner.Span.Slice(0, received))) return false;

        // Initialize the second secure tunnel layer where ONLY the WebSocket payload data will be read/written from/to.
        secureSocketStream = new SslStream(_socketStream, false, ValidateRemoteCertificate);

        _socketStream = secureSocketStream; // This stream layer will decrypt/encrypt the payload using the WebSocket protocol.
        await secureSocketStream.AuthenticateAsClientAsync(sslClientAuthOptions, cancellationToken).ConfigureAwait(false);
        return IsUpgraded;
    }
    public async Task<bool> UpgradeToWebSocketServerAsync(X509Certificate certificate, CancellationToken cancellationToken = default)
    {
        static bool IsTLSRequested(ReadOnlySpan<byte> message) => message.SequenceEqual(_startTLSBytes);
        static void FillWebResponse(ReadOnlySpan<byte> webRequest, Span<byte> webResponse, out int responseWritten)
        {
            int keyStart = webRequest.LastIndexOf(_secWebSocketKeyBytes) + _secWebSocketKeyBytes.Length;
            int keySize = webRequest.Slice(keyStart).IndexOf((byte)13); // Carriage Return

            Span<byte> mergedKey = stackalloc byte[keySize + _rfc6455GuidBytes.Length];
            webRequest.Slice(keyStart, keySize).CopyTo(mergedKey);
            _rfc6455GuidBytes.CopyTo(mergedKey.Slice(keySize));

            _upgradeWebSocketResponseBytes.CopyTo(webResponse);
            responseWritten = _upgradeWebSocketResponseBytes.Length;

            int keyHashedSize = SHA1.HashData(mergedKey, webResponse.Slice(_upgradeWebSocketResponseBytes.Length));
            Base64.EncodeToUtf8InPlace(webResponse.Slice(responseWritten), keyHashedSize, out int keyEncodedSize);
            responseWritten += keyEncodedSize;

            Span<byte> eof = webResponse.Slice(responseWritten);
            eof[0] = eof[2] = 13; // Carriage Return
            eof[1] = eof[3] = 10; // New Line
            responseWritten += 4; // \r\n\r\n
        }

        // TODO: Is this check still needed?
        //if (IsUpgraded || !await DetermineFormatsAsync(cancellationToken).ConfigureAwait(false)) return IsUpgraded;

        using MemoryOwner<byte> receivedOwner = MemoryOwner<byte>.Allocate(1024);
        int received = await ReceiveAsync(receivedOwner.Memory, cancellationToken).ConfigureAwait(false);

        using MemoryOwner<byte> responseOwner = MemoryOwner<byte>.Allocate(256);
        FillWebResponse(receivedOwner.Span.Slice(0, received), responseOwner.Span, out int responseWritten);
        await SendAsync(responseOwner.Memory.Slice(0, responseWritten), cancellationToken).ConfigureAwait(false);

        // Begin receiving/sending data as WebSocket frames.
        IsUpgraded = true;
        _socketStream = _webSocketStream = new WebSocketStream(_socketStream);

        received = await ReceiveAsync(receivedOwner.Memory, cancellationToken).ConfigureAwait(false);
        if (IsTLSRequested(receivedOwner.Span.Slice(0, received)))
        {
            await SendAsync(_okBytes.ToArray(), cancellationToken).ConfigureAwait(false);

            var secureSocketStream = new SslStream(_socketStream, false, ValidateRemoteCertificate);
            _socketStream = secureSocketStream;

            await secureSocketStream.AuthenticateAsServerAsync(certificate).ConfigureAwait(false);
        }
        else throw new Exception("The client did not send 'StartTLS'.");
        return IsUpgraded;
    }

    private ValueTask<int> ReadFromStreamAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
    {
        Stream tunnel = _socketStream;
        if (BypassReceiveSecureTunnel > 0 && _webSocketStream != null)
        {
            tunnel = _webSocketStream;
            BypassReceiveSecureTunnel--;
        }
        return tunnel.ReadAsync(buffer, cancellationToken);
    }

    public void Dispose()
    {
        if (_disposed) return;
        try
        {
            _socketStream.Dispose();
            _webSocketStream?.Dispose();

            _sendSemaphore.Dispose();
            _receiveSemaphore.Dispose();
        }
        catch { /* The socket doesn't like being shutdown/closed and will throw a fit everytime. */ }
        finally { _disposed = true; }
    }

    private static void Encipher(IStreamCipher cipher, Memory<byte> buffer, bool isWebSocket)
    {
        Span<byte> bufferSpan = buffer.Span;
        Encipher(cipher, bufferSpan, bufferSpan, isWebSocket);
    }
    private static void Encipher(IStreamCipher cipher, ReadOnlySpan<byte> source, Span<byte> destination, bool isWebSocket)
    {
        if (isWebSocket)
        {
            // Reverse the packet id and encrypt/decrypt it.
            Span<byte> idBuffer = [source[5], source[4]];
            cipher.Process(idBuffer);

            // After encryption/decryption, then reverse it back and place it on the original buffer.
            destination[4] = idBuffer[1];
            destination[5] = idBuffer[0];
        }
        else cipher.Process(source, destination);
    }
    private static bool ValidateRemoteCertificate(object sender, X509Certificate? certificate, X509Chain? chain, SslPolicyErrors sslPolicyErrors) => true;
}