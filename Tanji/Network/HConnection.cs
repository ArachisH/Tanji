using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Sulakore.Network;
using Sulakore.Network.Protocol;

namespace Tanji.Network
{
    public class HConnection : IHConnection
    {
        private bool _isIntercepting;
        private int _inSteps, _outSteps;

        private readonly object _disconnectLock;

        private const string CROSS_DOMAIN_POLICY = "<cross-domain-policy><allow-access-from domain=\"*\" to-ports=\"*\"/></cross-domain-policy>";

        /// <summary>
        /// Occurs when the connection between the client, and server have been intercepted.
        /// </summary>
        public event EventHandler<ConnectedEventArgs> Connected;
        protected virtual void OnConnected(ConnectedEventArgs e)
        {
            Connected?.Invoke(this, e);
        }

        /// <summary>
        /// Occurs when either the game client, or server have disconnected.
        /// </summary>
        public event EventHandler Disconnected;
        protected virtual void OnDisconnected(EventArgs e)
        {
            Disconnected?.Invoke(this, e);
        }

        /// <summary>
        /// Occurs when the client's outgoing data has been intercepted.
        /// </summary>
        public event EventHandler<DataInterceptedEventArgs> DataOutgoing;
        protected virtual void OnDataOutgoing(DataInterceptedEventArgs e)
        {
            DataOutgoing?.Invoke(this, e);
        }

        /// <summary>
        /// Occrus when the server's incoming data has been intercepted.
        /// </summary>
        public event EventHandler<DataInterceptedEventArgs> DataIncoming;
        protected virtual void OnDataIncoming(DataInterceptedEventArgs e)
        {
            DataIncoming?.Invoke(this, e);
        }

        public int SocketSkip { get; set; } = 2;
        public int ListenPort { get; set; } = 9567;
        public bool IsConnected { get; private set; }

        public HNode Local { get; private set; }
        public HNode Remote { get; private set; }

        public HConnection()
        {
            _disconnectLock = new object();
        }

        public Task InterceptAsync(IPEndPoint endpoint)
        {
            return InterceptAsync(new HotelEndPoint(endpoint));
        }
        public Task InterceptAsync(string host, int port)
        {
            return InterceptAsync(HotelEndPoint.Parse(host, port));
        }
        public async Task InterceptAsync(HotelEndPoint endpoint)
        {
            _isIntercepting = true;
            int interceptCount = 0;
            while (!IsConnected && _isIntercepting)
            {
                try
                {
                    Local = await HNode.AcceptAsync(ListenPort).ConfigureAwait(false);
                    if (!_isIntercepting) break;

                    if (++interceptCount == SocketSkip)
                    {
                        interceptCount = 0;
                        continue;
                    }

                    byte[] buffer = await Local.PeekAsync(6).ConfigureAwait(false);
                    if (!_isIntercepting) break;

                    if (buffer.Length == 0)
                    {
                        interceptCount--;
                        continue;
                    }

                    Remote = new HNode();
                    if (HFormat.WedgieOut.GetId(buffer) == 206)
                    {
                        Local.InFormat = HFormat.WedgieOut;
                        Local.OutFormat = HFormat.WedgieIn;

                        Remote.InFormat = HFormat.WedgieIn;
                        Remote.OutFormat = HFormat.WedgieOut;
                    }
                    else if (HFormat.EvaWire.GetId(buffer) == 4000)
                    {
                        Local.InFormat = HFormat.EvaWire;
                        Local.OutFormat = HFormat.EvaWire;

                        Remote.InFormat = HFormat.EvaWire;
                        Remote.OutFormat = HFormat.EvaWire;
                    }
                    else
                    {
                        buffer = await Local.ReceiveAsync(512).ConfigureAwait(false);
                        if (!_isIntercepting) break;

                        if (Encoding.UTF8.GetString(buffer).ToLower().Contains("<policy-file-request/>"))
                        {
                            await Local.SendAsync(Encoding.UTF8.GetBytes(CROSS_DOMAIN_POLICY)).ConfigureAwait(false);
                        }
                        else throw new Exception("Expected cross-domain policy request");
                        continue;
                    }

                    var args = new ConnectedEventArgs(endpoint);
                    OnConnected(args);
                    endpoint = args.HotelServer;

                    if (!await Remote.ConnectAsync(endpoint).ConfigureAwait(false)) break;
                    IsConnected = true;

                    _inSteps = 0;
                    _outSteps = 0;
                    Task interceptOutgoingTask = InterceptOutgoingAsync();
                    Task interceptIncomingTask = InterceptIncomingAsync();
                }
                finally
                {
                    if (!IsConnected)
                    {
                        Local?.Dispose();
                        Remote?.Dispose();
                    }
                }
            }
            HNode.StopListeners(ListenPort);
            _isIntercepting = false;
        }

        public Task<int> SendToServerAsync(byte[] data)
        {
            return Remote.SendAsync(data);
        }
        public Task<int> SendToServerAsync(HPacket packet)
        {
            return Remote.SendPacketAsync(packet);
        }
        public Task<int> SendToServerAsync(string signature)
        {
            return Remote.SendPacketAsync(signature);
        }
        public Task<int> SendToServerAsync(ushort id, params object[] values)
        {
            return Remote.SendPacketAsync(id, values);
        }

        public Task<int> SendToClientAsync(byte[] data)
        {
            return Local.SendAsync(data);
        }
        public Task<int> SendToClientAsync(HPacket packet)
        {
            return Local.SendPacketAsync(packet);
        }
        public Task<int> SendToClientAsync(string signature)
        {
            return Local.SendPacketAsync(signature);
        }
        public Task<int> SendToClientAsync(ushort id, params object[] values)
        {
            return Local.SendPacketAsync(id, values);
        }

        private Task<int> ClientRelayer(DataInterceptedEventArgs relayedFrom)
        {
            return SendToClientAsync(relayedFrom.Packet);
        }
        private Task<int> ServerRelayer(DataInterceptedEventArgs relayedFrom)
        {
            return SendToServerAsync(relayedFrom.Packet);
        }
        private async Task InterceptOutgoingAsync(DataInterceptedEventArgs continuedFrom = null)
        {
            HPacket packet = await Local.ReceivePacketAsync().ConfigureAwait(false);
            if (packet != null)
            {
                var args = new DataInterceptedEventArgs(packet, ++_outSteps, true,
                    InterceptOutgoingAsync, ServerRelayer);

                try { OnDataOutgoing(args); }
                catch { args.Restore(); }

                if (!args.IsBlocked && !args.WasRelayed)
                {
                    await SendToServerAsync(args.Packet).ConfigureAwait(false);
                }
                if (!args.HasContinued)
                {
                    args.Continue();
                }
            }
            else Disconnect();
        }
        private async Task InterceptIncomingAsync(DataInterceptedEventArgs continuedFrom = null)
        {
            HPacket packet = await Remote.ReceivePacketAsync().ConfigureAwait(false);
            if (packet != null)
            {
                var args = new DataInterceptedEventArgs(packet, ++_inSteps, false,
                    InterceptIncomingAsync, ClientRelayer);

                try { OnDataIncoming(args); }
                catch { args.Restore(); }

                if (!args.IsBlocked && !args.WasRelayed)
                {
                    await SendToClientAsync(args.Packet).ConfigureAwait(false);
                }
                if (!args.HasContinued)
                {
                    args.Continue();
                }
            }
            else Disconnect();
        }

        public void Disconnect()
        {
            if (Monitor.TryEnter(_disconnectLock))
            {
                try
                {
                    _isIntercepting = false;
                    if (Local != null)
                    {
                        Local.Dispose();
                        Local = null;
                    }
                    if (Remote != null)
                    {
                        Remote.Dispose();
                        Remote = null;
                    }
                    if (IsConnected)
                    {
                        IsConnected = false;
                        OnDisconnected(EventArgs.Empty);
                    }
                }
                finally { Monitor.Exit(_disconnectLock); }
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
                Disconnect();
            }
        }
    }
}