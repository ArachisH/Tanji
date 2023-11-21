using System.Net;
using System.Text;
using System.Buffers;
using System.Net.Sockets;
using System.Buffers.Text;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

using Tanji.Core.Cryptography.Ciphers;
using Tanji.Core.Habbo.Network.Formats;

namespace Tanji.Core.Habbo.Network;

public sealed class HNode : IDisposable
{
    private static ReadOnlySpan<byte> _okBytes => "OK"u8;
    private static ReadOnlySpan<byte> _startTLSBytes => "StartTLS"u8;
    private static ReadOnlySpan<byte> _rfc6455GuidBytes => "258EAFA5-E914-47DA-95CA-C5AB0DC85B11"u8;
    private static ReadOnlySpan<byte> _secWebSocketKeyBytes => "Sec-WebSocket-Key: "u8;
    private static ReadOnlySpan<byte> _upgradeWebSocketResponseBytes => "HTTP/1.1 101 Switching Protocols\r\nConnection: Upgrade\r\nUpgrade: websocket\r\nSec-WebSocket-Accept: "u8;

    private readonly Socket _socket;
    private readonly SemaphoreSlim _sendSemaphore, _receiveSemaphore, _packetSendSemaphore, _packetReceiveSemaphore;

    private byte[] _mask;
    private bool _disposed;
    private Stream _socketStream;
    private Stream? _webSocketStream;

    public int PacketsReceived { get; set; }
    public IHFormat? ReceiveFormat { get; set; }

    public IStreamCipher? Encrypter { get; set; }
    public IStreamCipher? Decrypter { get; set; }

    public EndPoint? RemoteEndPoint { get; }
    public int BypassReceiveSecureTunnel { get; set; }

    public bool IsUpgraded { get; private set; }
    public bool IsWebSocket => _webSocketStream != null;
    public bool IsConnected => !_disposed && _socket.Connected;

    private HNode(Socket socket)
    {
        _socket = socket;
        _socketStream = new BufferedNetworkStream(socket);

        _sendSemaphore = new SemaphoreSlim(1, 1);
        _receiveSemaphore = new SemaphoreSlim(1, 1);
        _packetSendSemaphore = new SemaphoreSlim(1, 1);
        _packetReceiveSemaphore = new SemaphoreSlim(1, 1);

        _mask = new byte[4];

        socket.NoDelay = true;
        socket.LingerState = new LingerOption(false, 0);

        RemoteEndPoint = socket.RemoteEndPoint;
    }

    public async ValueTask<int> SendAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
    {
        if (!IsConnected) return -1;
        if (buffer.Length == 0) return 0;

        await _sendSemaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            await _socketStream.WriteAsync(buffer, cancellationToken).ConfigureAwait(false);
            await _socketStream.FlushAsync(cancellationToken).ConfigureAwait(false);
            return buffer.Length;
        }
        catch { return -1; }
        finally { _sendSemaphore.Release(); }
    }
    public async ValueTask SendPacketAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
    {
        await _packetSendSemaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            if (Encrypter != null)
            {
                using IMemoryOwner<byte> tempBufferOwner = BufferHelper.Rent(buffer.Length, out Memory<byte> tempBuffer);

                Encipher(Encrypter, buffer.Span, tempBuffer.Span, IsWebSocket);
                await SendAsync(tempBuffer, cancellationToken).ConfigureAwait(false);
            }
            else await SendAsync(buffer, cancellationToken).ConfigureAwait(false);
        }
        finally { _packetSendSemaphore.Release(); }
    }

    public async ValueTask<int> ReceiveAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
    {
        if (!IsConnected) return -1;
        if (buffer.Length == 0) return 0;

        await _receiveSemaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            Stream tunnel = _socketStream;
            if (BypassReceiveSecureTunnel > 0 && _webSocketStream != null)
            {
                tunnel = _webSocketStream;
                BypassReceiveSecureTunnel--;
            }
            return await tunnel.ReadAsync(buffer, cancellationToken).ConfigureAwait(false);
        }
        catch { return -1; }
        finally { _receiveSemaphore.Release(); }
    }
    public async ValueTask<short> ReceivePacketAsync(IBufferWriter<byte> writer, CancellationToken cancellationToken = default)
    {
        if (ReceiveFormat == null)
        {
            ThrowHelper.ThrowNullReferenceException(nameof(ReceiveFormat));
        }

        short packetId = 0;
        await _packetReceiveSemaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            Memory<byte> buffer = writer.GetMemory(ReceiveFormat.MinBufferSize);

            int received = 0;
            do received = await ReceiveAsync(buffer, cancellationToken).ConfigureAwait(false);
            while (received == 0);

            if (received != buffer.Length) return 0;
            if (!ReceiveFormat.TryReadHeader(buffer.Span, out int length, out packetId, out _)) return 0;

            writer.Advance(received);
            int packetBytesLeft = length - ReceiveFormat.MinPacketLength;

            // TODO: Populate the writer...
        }
        finally
        {
            PacketsReceived++;
            _packetReceiveSemaphore.Release();
        }

        return packetId;
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

        using IMemoryOwner<byte> receiveOwner = BufferHelper.Rent(256, out Memory<byte> receiveRegion);
        int received = await ReceiveAsync(receiveRegion, cancellationToken).ConfigureAwait(false);

        // Create the mask that will be used for the WebSocket payloads.
        //RandomNumberGenerator.Fill(_mask);

        IsUpgraded = true;
        _socketStream = _webSocketStream = new WebSocketStream(_socketStream, _mask, false); // Anything now being sent or received through the stream will be parsed using the WebSocket protocol.

        await SendAsync(_startTLSBytes.ToArray(), cancellationToken).ConfigureAwait(false);
        received = await ReceiveAsync(receiveRegion, cancellationToken).ConfigureAwait(false);
        if (!IsTLSAccepted(receiveRegion.Span.Slice(0, received))) return false;

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

        using IMemoryOwner<byte> receivedOwner = BufferHelper.Rent(1024, out Memory<byte> receivedRegion);
        int received = await ReceiveAsync(receivedRegion, cancellationToken).ConfigureAwait(false);

        using IMemoryOwner<byte> responseOwner = BufferHelper.Rent(256, out Memory<byte> responseRegion);
        FillWebResponse(receivedRegion.Span.Slice(0, received), responseRegion.Span, out int responseWritten);
        await SendAsync(responseRegion.Slice(0, responseWritten), cancellationToken).ConfigureAwait(false);

        // Begin receiving/sending data as WebSocket frames.
        IsUpgraded = true;
        _socketStream = _webSocketStream = new WebSocketStream(_socketStream);

        received = await ReceiveAsync(receivedRegion, cancellationToken).ConfigureAwait(false);
        if (IsTLSRequested(receivedRegion.Span.Slice(0, received)))
        {
            await SendAsync(_okBytes.ToArray(), cancellationToken).ConfigureAwait(false);

            var secureSocketStream = new SslStream(_socketStream, false, ValidateRemoteCertificate);
            _socketStream = secureSocketStream;

            await secureSocketStream.AuthenticateAsServerAsync(certificate).ConfigureAwait(false);
        }
        else throw new Exception("The client did not send 'StartTLS'.");
        return IsUpgraded;
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
            _packetSendSemaphore.Dispose();
            _packetReceiveSemaphore.Dispose();
        }
        catch { /* The socket doesn't like being shutdown/closed and will throw a fit everytime. */ }
        finally
        {
            _disposed = true;
        }
    }

    private static void Encipher(IStreamCipher cipher, ReadOnlySpan<byte> source, Span<byte> destination, bool isWebSocket)
    {
        if (isWebSocket)
        {
            // Reverse the packet id and encrypt/decrypt it.
            Span<byte> idBuffer = stackalloc byte[2] { source[5], source[4] };
            cipher.Process(idBuffer);

            // After encryption/decryption, then reverse it back and place it on the original buffer.
            destination[4] = idBuffer[1];
            destination[5] = idBuffer[0];
        }
        else cipher.Process(source, destination);
    }
    private static bool ValidateRemoteCertificate(object sender, X509Certificate? certificate, X509Chain? chain, SslPolicyErrors sslPolicyErrors) => true;

    public static async ValueTask<HNode> AcceptAsync(int port, CancellationToken cancellationToken = default)
    {
        using var listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        listenSocket.Bind(new IPEndPoint(IPAddress.Any, port));
        listenSocket.LingerState = new LingerOption(false, 0);
        listenSocket.Listen(1);

        Socket socket = await listenSocket.AcceptAsync(cancellationToken).ConfigureAwait(false);
        listenSocket.Close();

        return new HNode(socket);
    }
    public static async ValueTask<HNode> ConnectAsync(EndPoint remoteEP, CancellationToken cancellationToken = default)
    {
        var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            await socket.ConnectAsync(remoteEP, cancellationToken).ConfigureAwait(false);
        }
        catch { /* Ignore all exceptions. */ }
        if (!socket.Connected)
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
        return new HNode(socket);
    }
    public static async ValueTask<HNode> ConnectAsync(string host, int port, CancellationToken cancellationToken = default)
    {
        IPAddress[] addresses = await Dns.GetHostAddressesAsync(host, cancellationToken).ConfigureAwait(false);
        var remoteEP = new IPEndPoint(addresses[0], port);

        return await ConnectAsync(remoteEP, cancellationToken).ConfigureAwait(false);
    }
}