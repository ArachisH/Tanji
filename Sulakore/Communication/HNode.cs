using System;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Collections.Generic;

using Sulakore.Crypto;
using Sulakore.Protocol;

namespace Sulakore.Communication
{
    public class HNode : IDisposable
    {
        private static readonly Dictionary<int, TcpListener> _listeners;

        public bool IsConnected => Client.Connected;

        public Socket Client { get; }
        public HotelEndPoint EndPoint { get; private set; }

        public string Username { get; set; }
        public string Password { get; set; }
        public IPEndPoint SOCKS5EndPoint { get; set; }

        public RC4 Encrypter { get; set; }
        public bool IsEncrypting { get; set; }

        public RC4 Decrypter { get; set; }
        public bool IsDecrypting { get; set; }

        static HNode()
        {
            _listeners = new Dictionary<int, TcpListener>();
        }
        public HNode()
            : this(new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
        { }
        public HNode(Socket client)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            if (client.RemoteEndPoint != null)
            {
                EndPoint = new HotelEndPoint((IPEndPoint)client.RemoteEndPoint);
            }

            Client = client;
            Client.NoDelay = true;
        }

        private async Task<bool> ConnectAsync()
        {
            bool connected = true;
            try
            {
                IPEndPoint endPoint = (SOCKS5EndPoint ?? EndPoint);
                IAsyncResult result = Client.BeginConnect(endPoint, null, null);
                await Task.Factory.FromAsync(result, Client.EndConnect).ConfigureAwait(false);

                if (!Client.Connected) return (connected = false);
                if (SOCKS5EndPoint != null)
                {
                    await SendAsync(new byte[]
                    {
                        0x05, // Version 5
                        0x02, // 2 Authentication Methods Present
                        0x00, // No Authentication
                        0x02  // Username + Password
                    }).ConfigureAwait(false);

                    byte[] response = await ReceiveAsync(2).ConfigureAwait(false);
                    if (response?.Length != 2 || response[1] == 0xFF) return (connected = false);

                    int index = 0;
                    byte[] payload = null;
                    if (response[1] == 0x02) // Username + Password Required
                    {
                        index = 0;
                        payload = new byte[byte.MaxValue];
                        payload[index++] = 0x01;

                        // Username
                        payload[index++] = (byte)Username.Length;
                        byte[] usernameData = Encoding.Default.GetBytes(Username);
                        Buffer.BlockCopy(usernameData, 0, payload, index, usernameData.Length);
                        index += usernameData.Length;

                        // Password
                        payload[index++] = (byte)Password.Length;
                        byte[] passwordData = Encoding.Default.GetBytes(Password);
                        Buffer.BlockCopy(passwordData, 0, payload, index, passwordData.Length);
                        index += passwordData.Length;

                        await SendAsync(payload, index).ConfigureAwait(false);
                        response = await ReceiveAsync(2).ConfigureAwait(false);
                        if (response?.Length != 2 || response[1] != 0x00) return (connected = false);
                    }

                    index = 0;
                    payload = new byte[255];
                    payload[index++] = 0x05;
                    payload[index++] = 0x01;
                    payload[index++] = 0x00;

                    payload[index++] = (byte)(EndPoint.AddressFamily ==
                        AddressFamily.InterNetwork ? 0x01 : 0x04);

                    // Destination Address
                    byte[] addressBytes = EndPoint.Address.GetAddressBytes();
                    Buffer.BlockCopy(addressBytes, 0, payload, index, addressBytes.Length);
                    index += (ushort)addressBytes.Length;

                    byte[] portData = BitConverter.GetBytes((ushort)EndPoint.Port);
                    if (BitConverter.IsLittleEndian)
                    {
                        // Big-Endian Byte Order
                        Array.Reverse(portData);
                    }
                    Buffer.BlockCopy(portData, 0, payload, index, portData.Length);
                    index += portData.Length;

                    await SendAsync(payload, index);
                    response = await ReceiveAsync(byte.MaxValue);
                    if (response?.Length < 2 || response[1] != 0x00) return (connected = false);
                }
            }
            catch { return (connected = false); }
            finally
            {
                if (!connected)
                {
                    Disconnect();
                }
            }
            return IsConnected;
        }
        public Task<bool> ConnectAsync(IPEndPoint endpoint)
        {
            EndPoint = (endpoint as HotelEndPoint);
            if (EndPoint == null)
            {
                EndPoint = new HotelEndPoint(endpoint);
            }
            return ConnectAsync();
        }
        public Task<bool> ConnectAsync(string host, int port)
        {
            return ConnectAsync(HotelEndPoint.Parse(host, port));
        }
        public Task<bool> ConnectAsync(IPAddress address, int port)
        {
            return ConnectAsync(new HotelEndPoint(address, port));
        }
        public Task<bool> ConnectAsync(IPAddress[] addresses, int port)
        {
            return ConnectAsync(new HotelEndPoint(addresses[0], port));
        }

        public async Task<HMessage> ReceivePacketAsync()
        {
            byte[] lengthBlock = await AttemptReceiveAsync(4, 3).ConfigureAwait(false);
            if (lengthBlock == null)
            {
                Disconnect();
                return null;
            }

            byte[] body = await AttemptReceiveAsync(BigEndian.ToInt32(lengthBlock, 0), 3).ConfigureAwait(false);
            if (body == null)
            {
                Disconnect();
                return null;
            }

            var data = new byte[4 + body.Length];
            Buffer.BlockCopy(lengthBlock, 0, data, 0, 4);
            Buffer.BlockCopy(body, 0, data, 4, body.Length);

            return new HMessage(data);
        }
        public Task<int> SendPacketAsync(HMessage packet)
        {
            return SendAsync(packet.ToBytes());
        }
        public Task<int> SendPacketAsync(string signature)
        {
            return SendAsync(HMessage.ToBytes(signature));
        }
        public Task<int> SendPacketAsync(ushort id, params object[] values)
        {
            return SendAsync(HMessage.Construct(id, values));
        }

        public Task<int> SendAsync(byte[] buffer)
        {
            return SendAsync(buffer, buffer.Length);
        }
        public Task<int> SendAsync(byte[] buffer, int size)
        {
            return SendAsync(buffer, 0, size);
        }
        public Task<int> SendAsync(byte[] buffer, int offset, int size)
        {
            return SendAsync(buffer, offset, size, SocketFlags.None);
        }

        public Task<byte[]> ReceiveAsync(int size)
        {
            return ReceiveBufferAsync(size, SocketFlags.None);
        }
        public Task<int> ReceiveAsync(byte[] buffer)
        {
            return ReceiveAsync(buffer, buffer.Length);
        }
        public Task<int> ReceiveAsync(byte[] buffer, int size)
        {
            return ReceiveAsync(buffer, 0, size);
        }
        public Task<int> ReceiveAsync(byte[] buffer, int offset, int size)
        {
            return ReceiveAsync(buffer, offset, size, SocketFlags.None);
        }
        public async Task<byte[]> AttemptReceiveAsync(int size, int attempts)
        {
            int totalBytesRead = 0;
            var data = new byte[size];
            int nullBytesReadCount = 0;
            do
            {
                int bytesLeft = (data.Length - totalBytesRead);
                int bytesRead = await ReceiveAsync(data, totalBytesRead, bytesLeft).ConfigureAwait(false);

                if (IsConnected && bytesRead > 0)
                {
                    nullBytesReadCount = 0;
                    totalBytesRead += bytesRead;
                }
                else if (!IsConnected || ++nullBytesReadCount >= attempts)
                {
                    return null;
                }
            }
            while (totalBytesRead != data.Length);
            return data;
        }

        public Task<byte[]> PeekAsync(int size)
        {
            return ReceiveBufferAsync(size, SocketFlags.Peek);
        }
        public Task<int> PeekAsync(byte[] buffer)
        {
            return PeekAsync(buffer, buffer.Length);
        }
        public Task<int> PeekAsync(byte[] buffer, int size)
        {
            return PeekAsync(buffer, 0, size);
        }
        public Task<int> PeekAsync(byte[] buffer, int offset, int size)
        {
            return ReceiveAsync(buffer, offset, size, SocketFlags.Peek);
        }

        protected async Task<byte[]> ReceiveBufferAsync(int size, SocketFlags socketFlags)
        {
            var buffer = new byte[size];
            int read = await ReceiveAsync(buffer, 0, size, socketFlags).ConfigureAwait(false);
            if (read == -1) return null;

            var trimmedBuffer = new byte[read];
            Buffer.BlockCopy(buffer, 0, trimmedBuffer, 0, read);

            return trimmedBuffer;
        }
        protected async Task<int> SendAsync(byte[] buffer, int offset, int size, SocketFlags socketFlags)
        {
            if (!IsConnected) return -1;
            if (IsEncrypting && Encrypter != null)
            {
                buffer = Encrypter.Parse(buffer);
            }

            int sent = -1;
            try
            {
                IAsyncResult result = Client.BeginSend(buffer, offset, size, socketFlags, null, null);
                sent = await Task.Factory.FromAsync(result, Client.EndSend).ConfigureAwait(false);
            }
            catch { sent = -1; }
            return sent;
        }
        protected async Task<int> ReceiveAsync(byte[] buffer, int offset, int size, SocketFlags socketFlags)
        {
            if (!IsConnected) return -1;
            if (buffer == null)
            {
                throw new NullReferenceException("Buffer cannot be null.");
            }
            else if (buffer.Length == 0 || size == 0) return 0;

            int read = -1;
            try
            {
                IAsyncResult result = Client.BeginReceive(buffer, offset, size, socketFlags, null, null);
                read = await Task.Factory.FromAsync(result, Client.EndReceive).ConfigureAwait(false);
            }
            catch { read = -1; }

            if (read > 0 && IsDecrypting && Decrypter != null)
            {
                Decrypter.RefParse(buffer, offset, read, socketFlags.HasFlag(SocketFlags.Peek));
            }
            return read;
        }

        public void Disconnect()
        {
            if (IsConnected)
            {
                try
                {
                    Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, false);
                    Client.Shutdown(SocketShutdown.Both);
                    Client.Disconnect(false);
                }
                catch { }
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (IsConnected)
                {
                    try
                    {
                        Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
                        Client.Shutdown(SocketShutdown.Both);
                    }
                    catch { }
                }
                Client.Close();
            }
        }

        public static void StopListeners(int? port = null)
        {
            foreach (TcpListener listener in _listeners.Values)
            {
                if (port != null)
                {
                    if (port != ((IPEndPoint)listener.LocalEndpoint).Port) continue;
                }
                listener.Stop();
            }
        }
        public static async Task<HNode> AcceptAsync(int port)
        {
            TcpListener listener = null;
            if (!_listeners.TryGetValue(port, out listener))
            {
                listener = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
                _listeners.Add(port, listener);
            }

            try
            {
                listener.Start();
                Socket client = await listener.AcceptSocketAsync().ConfigureAwait(false);
                return new HNode(client);
            }
            finally
            {
                listener.Stop();
                if (_listeners.ContainsKey(port))
                {
                    _listeners.Remove(port);
                }
            }
        }

        public static Task<HNode> ConnectNewAsync(string host, int port)
        {
            return ConnectNewAsync(HotelEndPoint.Parse(host, port));
        }
        public static async Task<HNode> ConnectNewAsync(IPEndPoint endpoint)
        {
            HNode remote = null;
            try
            {
                remote = new HNode();
                await remote.ConnectAsync(endpoint).ConfigureAwait(false);
            }
            catch { remote = null; }
            finally
            {
                if (!remote?.IsConnected ?? false)
                {
                    remote = null;
                }
            }
            return remote;
        }
        public static Task<HNode> ConnectNewAsync(IPAddress address, int port)
        {
            return ConnectNewAsync(new HotelEndPoint(address, port));
        }
        public static Task<HNode> ConnectNewAsync(IPAddress[] addresses, int port)
        {
            return ConnectNewAsync(new HotelEndPoint(addresses[0], port));
        }
    }
}