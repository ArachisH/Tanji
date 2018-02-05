using System;
using System.Threading.Tasks;

using Sulakore.Protocol;
using Sulakore.Communication;

namespace Tangine
{
    internal class ContractorProxy : IHConnection
    {
        public string Host { get; set; }
        public ushort Port { get; set; }
        public string Address { get; set; }

        public int TotalOutgoing { get; set; }
        public int TotalIncoming { get; set; }

        public HNode Local => throw new NotSupportedException();
        public HNode Remote { get; }

        public ContractorProxy(HNode remote)
        {
            Remote = remote;
        }

        public Task<int> SendToClientAsync(byte[] data)
        {
            return Remote.SendPacketAsync(4, data.Length, data);
        }
        public Task<int> SendToClientAsync(HMessage packet)
        {
            return SendToClientAsync(packet.ToBytes());
        }
        public Task<int> SendToClientAsync(ushort header, params object[] values)
        {
            return SendToClientAsync(HMessage.Construct(header, values));
        }

        public Task<int> SendToServerAsync(byte[] data)
        {
            return Remote.SendPacketAsync(5, data.Length, data);
        }
        public Task<int> SendToServerAsync(HMessage packet)
        {
            return SendToServerAsync(packet.ToBytes());
        }
        public Task<int> SendToServerAsync(ushort header, params object[] values)
        {
            return SendToServerAsync(HMessage.Construct(header, values));
        }

        public void Dispose()
        {
            Dispose(true);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Remote?.Dispose();
            }
        }
    }
}