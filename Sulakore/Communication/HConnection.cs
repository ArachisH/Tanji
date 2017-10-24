using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using Sulakore.Protocol;

namespace Sulakore.Communication
{
    /// <summary>
    /// Represents a connection handler to intercept incoming/outgoing data from a post-shuffle hotel.
    /// </summary>
    public class HConnection : IHConnection, IDisposable
    {
        private bool _isIntercepting;
        private readonly object _disconnectLock;

        /// <summary>
        /// Occurs when the intercepted local <see cref="HNode"/> initiates the handshake with the server.
        /// </summary>
        public event EventHandler<EventArgs> Connected;
        /// <summary>
        /// Raises the <see cref="Connected"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnConnected(EventArgs e)
        {
            Connected?.Invoke(this, e);
        }

        /// <summary>
        /// Occurs when either client/server have been disconnected, or when <see cref="Disconnect"/> has been called if <see cref="IsConnected"/> is true.
        /// </summary>
        public event EventHandler<EventArgs> Disconnected;
        /// <summary>
        /// Raises the <see cref="Disconnected"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnDisconnected(EventArgs e)
        {
            Disconnected?.Invoke(this, e);
        }

        /// <summary>
        /// Occurs when outgoing data from the local <see cref="HNode"/> has been intercepted.
        /// </summary>
        public event EventHandler<DataInterceptedEventArgs> DataOutgoing;
        /// <summary>
        /// Raises the <see cref="DataOutgoing"/> event.
        /// </summary>
        /// <param name="e">An <see cref="DataInterceptedEventArgs"/> that contains the event data.</param>
        /// <returns></returns>
        protected virtual void OnDataOutgoing(DataInterceptedEventArgs e)
        {
            DataOutgoing?.Invoke(this, e);
        }

        /// <summary>
        /// Occurs when incoming data from the remote <see cref="HNode"/> has been intercepted.
        /// </summary>
        public event EventHandler<DataInterceptedEventArgs> DataIncoming;
        /// <summary>
        /// Raises the <see cref="DataIncoming"/> event.
        /// </summary>
        /// <param name="e">An <see cref="DataInterceptedEventArgs"/> that contains the event data.</param>
        /// <returns></returns>
        protected virtual void OnDataIncoming(DataInterceptedEventArgs e)
        {
            DataIncoming?.Invoke(this, e);
        }

        /// <summary>
        /// Gets the port of the remote endpoint.
        /// </summary>
        public ushort Port { get; private set; }
        /// <summary>
        /// Gets the host name of the remote endpoint.
        /// </summary>
        public string Host { get; private set; }
        /// <summary>
        /// Gets the IP address of the remote endpoint.
        /// </summary>
        public string Address { get; private set; }

        /// <summary>
        /// Gets the total amount of packets the remote <see cref="HNode"/> has been sent from local.
        /// </summary>
        public int TotalOutgoing { get; private set; }
        /// <summary>
        /// Gets the total amount of packets the local <see cref="HNode"/> received from remote.
        /// </summary>
        public int TotalIncoming { get; private set; }

        /// <summary>
        /// Gets the <see cref="HNode"/> representing the local connection.
        /// </summary>
        public HNode Local { get; private set; }
        /// <summary>
        /// Gets the <see cref="HNode"/> representing the remote connection.
        /// </summary>
        public HNode Remote { get; private set; }

        public int SocketSkip { get; set; } = 2;
        public bool IsConnected { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HConnection"/> class.
        /// </summary>
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
            Host = endpoint.Host;
            Port = (ushort)endpoint.Port;

            _isIntercepting = true;
            int interceptCount = 0;
            while (!IsConnected && _isIntercepting)
            {
                try
                {
                    Local = await HNode.AcceptAsync(endpoint.Port).ConfigureAwait(false);
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

                    Remote = await HNode.ConnectNewAsync(endpoint).ConfigureAwait(false);
                    if (!_isIntercepting) break;

                    if (BigEndian.ToUInt16(buffer, 4) != 4000)
                    {
                        buffer = await Local.ReceiveAsync(512).ConfigureAwait(false);
                        await Remote.SendAsync(buffer).ConfigureAwait(false);

                        buffer = await Remote.ReceiveAsync(1024).ConfigureAwait(false);
                        await Local.SendAsync(buffer).ConfigureAwait(false);
                        continue;
                    }
                    if (!_isIntercepting) break;

                    IsConnected = true;
                    OnConnected(EventArgs.Empty);

                    TotalIncoming = 0;
                    TotalOutgoing = 0;
                    Task readOutgoingTask = ReadOutgoingAsync();
                    Task readIncomingTask = ReadIncomingAsync();
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
            HNode.StopListeners(endpoint.Port);
            _isIntercepting = false;
        }

        /// <summary>
        /// Sends data to the remote <see cref="HNode"/> in an asynchronous operation.
        /// </summary>
        /// <param name="data">The data to send to the node.</param>
        /// <returns></returns>
        public Task<int> SendToServerAsync(byte[] data)
        {
            return Remote.SendAsync(data);
        }
        /// <summary>
        /// Sends data to the remote <see cref="HNode"/> using the specified header, and chunks in an asynchronous operation.
        /// </summary>
        /// <param name="header">The header to be used for the construction of the packet.</param>
        /// <param name="chunks">The chunks/values that the packet will carry.</param>
        /// <returns></returns>
        public Task<int> SendToServerAsync(ushort header, params object[] chunks)
        {
            return Remote.SendAsync(HMessage.Construct(header, chunks));
        }

        /// <summary>
        /// Sends data to the local <see cref="HNode"/> in an asynchronous operation.
        /// </summary>
        /// <param name="data">The data to send to the node.</param>
        /// <returns></returns>
        public Task<int> SendToClientAsync(byte[] data)
        {
            return Local.SendAsync(data);
        }
        /// <summary>
        /// Sends data to the local <see cref="HNode"/> using the specified header, and chunks in an asynchronous operation.
        /// </summary>
        /// <param name="header">The header to be used for the construction of the packet.</param>
        /// <param name="chunks">The chunks/values that the packet will carry.</param>
        /// <returns></returns>
        public Task<int> SendToClientAsync(ushort header, params object[] chunks)
        {
            return Local.SendAsync(HMessage.Construct(header, chunks));
        }

        private async Task ReadOutgoingAsync()
        {
            HMessage packet = await Local.ReceivePacketAsync().ConfigureAwait(false);
            if (IsConnected && packet != null && !packet.IsCorrupted)
            {
                packet.Destination = HDestination.Server;
                try { HandleOutgoing(packet, ++TotalOutgoing); }
                catch { Disconnect(); }
            }
            else Disconnect();
        }
        private void HandleOutgoing(HMessage packet, int count)
        {
            HandleMessage(packet, count, ReadOutgoingAsync, Remote, OnDataOutgoing);
        }

        private async Task ReadIncomingAsync()
        {
            HMessage packet = await Remote.ReceivePacketAsync().ConfigureAwait(false);
            if (IsConnected && packet != null && !packet.IsCorrupted)
            {
                packet.Destination = HDestination.Client;
                try { HandleIncoming(packet, ++TotalIncoming); }
                catch { Disconnect(); }
            }
            else Disconnect();
        }
        private void HandleIncoming(HMessage packet, int count)
        {
            HandleMessage(packet, count, ReadIncomingAsync, Local, OnDataIncoming);
        }

        private void HandleExecutions(IList<HMessage> executions)
        {
            var executeTaskList = new List<Task<int>>(executions.Count);
            for (int i = 0; i < executions.Count; i++)
            {
                byte[] executionData =
                    executions[i].ToBytes();

                HNode node = (executions[i].Destination ==
                    HDestination.Server ? Remote : Local);

                executeTaskList.Add(
                    node.SendAsync(executionData));
            }
            Task.WhenAll(executeTaskList).Wait();
        }
        private void HandleMessage(HMessage packet, int count, Func<Task> continuation, HNode node, Action<DataInterceptedEventArgs> eventRaiser)
        {
            var args = new DataInterceptedEventArgs(packet, count, continuation);
            eventRaiser(args);

            if (!args.IsBlocked)
            {
                node.SendAsync(args.Packet.ToBytes()).Wait();
                HandleExecutions(args.Executions);
            }

            if (!args.HasContinued)
            {
                args.Continue();
            }
        }

        /// <summary>
        /// Disconnects from the remote endpoint.
        /// </summary>
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

                    TotalOutgoing = TotalIncoming = 0;
                    if (IsConnected)
                    {
                        IsConnected = false;
                        OnDisconnected(EventArgs.Empty);
                    }
                }
                finally { Monitor.Exit(_disconnectLock); }
            }
        }

        /// <summary>
        /// Releases all resources used by the <see cref="HConnection"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
        /// <summary>
        /// Releases all resources used by the <see cref="HConnection"/>.
        /// </summary>
        /// <param name="disposing">The value that determines whether managed resources should be disposed.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Disconnect();
            }
        }
    }
}