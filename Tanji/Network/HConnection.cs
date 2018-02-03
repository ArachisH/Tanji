using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using Sulakore.Protocol;
using Sulakore.Communication;

namespace Tanji.Network
{
    public class HConnection : IHConnection, IDisposable
    {
        private bool _isIntercepting;
        private readonly object _disconnectLock;

        public event EventHandler<EventArgs> Connected;
        protected virtual void OnConnected(EventArgs e)
        {
            Connected?.Invoke(this, e);
        }

        public event EventHandler<EventArgs> Disconnected;
        protected virtual void OnDisconnected(EventArgs e)
        {
            Disconnected?.Invoke(this, e);
        }

        public event EventHandler<DataInterceptedEventArgs> DataOutgoing;
        protected virtual void OnDataOutgoing(DataInterceptedEventArgs e)
        {
            DataOutgoing?.Invoke(this, e);
        }

        public event EventHandler<DataInterceptedEventArgs> DataIncoming;
        protected virtual void OnDataIncoming(DataInterceptedEventArgs e)
        {
            DataIncoming?.Invoke(this, e);
        }

        public ushort Port { get; private set; }
        public string Host { get; private set; }
        public string Address { get; private set; }

        public int TotalOutgoing { get; private set; }
        public int TotalIncoming { get; private set; }

        public HNode Local { get; private set; }
        public HNode Remote { get; private set; }

        public int SocketSkip { get; set; } = 2;
        public bool IsConnected { get; private set; }

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

        public Task<int> SendToServerAsync(byte[] data)
        {
            return Remote.SendAsync(data);
        }
        public Task<int> SendToServerAsync(HMessage packet)
        {
            return SendToServerAsync(packet.ToBytes());
        }
        public Task<int> SendToServerAsync(ushort header, params object[] chunks)
        {
            return Remote.SendAsync(HMessage.Construct(header, chunks));
        }

        public Task<int> SendToClientAsync(byte[] data)
        {
            return Local.SendAsync(data);
        }
        public Task<int> SendToClientAsync(HMessage packet)
        {
            return SendToClientAsync(packet.ToBytes());
        }
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