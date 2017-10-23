using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

using Sulakore.Protocol;
using Sulakore.Protocol.Encryption;

namespace Sulakore.Communication
{
    public class HNode : IDisposable
    {
        private readonly object _receiveLock;

        public Socket Client { get; }
        public bool IsConnected => Client.Connected;
        public EndPoint RemoteEndPoint => Client.RemoteEndPoint;

        public Rc4 Encrypter { get; set; }
        public Rc4 Decrypter { get; set; }
        public HKeyExchange Exchange { get; set; }

        public bool IsDisposed { get; private set; }
        public int PacketsReceived { get; private set; }

        public HNode(Socket client)
        {
            Client = client;

            _receiveLock = new object();
        }

        public Task<HMessage> PeekAsync()
        {
            return ReceiveAsync(SocketFlags.Peek);
        }
        public Task<byte[]> PeekAsync(int length)
        {
            return ReceiveAsync(SocketFlags.Peek, length, false);
        }

        public Task<HMessage> ReceiveAsync()
        {
            return ReceiveAsync(SocketFlags.None);
        }
        public Task<byte[]> ReceiveAsync(int length)
        {
            return ReceiveAsync(SocketFlags.None, length, false);
        }

        public Task<int> SendAsync(HMessage packet)
        {
            return SendAsync(packet.ToBytes());
        }
        public async Task<int> SendAsync(byte[] data)
        {
            int length = 0;
            try
            {
                data = Encrypter?.SafeParse(data) ?? data;

                IAsyncResult result = Client.BeginSend(data, 0,
                    data.Length, SocketFlags.None, null, null);

                length = await Task.Factory.FromAsync(
                    result, Client.EndSend).ConfigureAwait(false);
            }
            catch { length = 0; }
            finally
            {
                if (length < 1)
                    Disconnect();
            }
            return length;
        }
        public Task<int> SendAsync(ushort header, params object[] values)
        {
            return SendAsync(HMessage.Construct(header, values));
        }

        public async Task<HMessage> ReceiveAsync(SocketFlags flags)
        {
            byte[] lengthData = await ReceiveAsync(
                flags, 4, true).ConfigureAwait(false);

            if (lengthData == null) return null;
            PacketsReceived++;

            int length = BigEndian.ToInt32(lengthData, 0);

            byte[] data = await ReceiveAsync(
                flags, length, true).ConfigureAwait(false);

            if (data == null) return null;
            var messageData = new byte[4 + length];

            Buffer.BlockCopy(lengthData, 0, messageData, 0, lengthData.Length);
            Buffer.BlockCopy(data, 0, messageData, lengthData.Length, data.Length);

            return new HMessage(messageData);
        }
        public async Task<byte[]> ReceiveAsync(SocketFlags flags, int length, bool isStrict)
        {
            var buffer = new byte[length];
            try
            {
                int readLength = await ReceiveAsync(
                    buffer, 0, length, flags).ConfigureAwait(false);

                if (!isStrict)
                {
                    var readData = new byte[readLength];
                    Buffer.BlockCopy(buffer, 0, readData, 0, readLength);
                    buffer = readData;
                }
                while (isStrict && (readLength < length))
                {
                    int bytesLeft = (length - readLength);

                    readLength += await ReceiveAsync(buffer,
                        readLength, bytesLeft, flags).ConfigureAwait(false);

                    if (readLength == 0 && bytesLeft > 0)
                    {
                        buffer = null;
                        break;
                    }
                }

                if (buffer != null)
                    Decrypter?.Parse(buffer);
            }
            catch { buffer = null; }
            finally
            {
                if (buffer == null)
                    Disconnect();
            }
            return buffer;
        }

        private Task<int> ReceiveAsync(byte[] buffer, int offset, int size, SocketFlags flags)
        {
            IAsyncResult result = Client.BeginReceive(buffer,
                offset, size, flags, null, null);

            return Task.Factory.FromAsync(
                result, Client.EndReceive);
        }

        public void Disconnect()
        {
            if (Monitor.TryEnter(Client))
            {
                try
                {
                    Exchange?.Dispose();
                    if (Client.Connected)
                    {
                        Client.Shutdown(SocketShutdown.Both);
                        Client.Close();
                    }
                    Encrypter = Decrypter = null;
                }
                finally { Monitor.Exit(Client); }
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed) return;
            if (disposing)
            {
                Disconnect();
            }
            IsDisposed = true;
        }

        public static async Task<HNode> ListenAsync(int port)
        {
            var listener = new TcpListener(IPAddress.Any, port);
            try
            {
                listener.Start();
                Socket client = await listener.AcceptSocketAsync()
                    .ConfigureAwait(false);

                return new HNode(client);
            }
            finally { listener.Stop(); }
        }
        public static async Task<HNode> ConnectAsync(string host, int port)
        {
            var socket = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            IAsyncResult result = socket.BeginConnect(
                host, port, null, null);

            await Task.Factory.FromAsync(result,
                socket.EndConnect).ConfigureAwait(false);

            return new HNode(socket);
        }
    }
}