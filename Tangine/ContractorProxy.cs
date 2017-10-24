using System.Threading.Tasks;

using Sulakore.Protocol;
using Sulakore.Communication;

namespace Tangine
{
    internal class ContractorProxy : IHConnection
    {
        private readonly HNode _remoteContractor;

        public string Host { get; set; }
        public ushort Port { get; set; }
        public string Address { get; set; }

        public int TotalOutgoing { get; set; }
        public int TotalIncoming { get; set; }

        public ContractorProxy(HNode remoteContractor)
        {
            _remoteContractor = remoteContractor;
        }

        public Task<int> SendToClientAsync(byte[] data)
        {
            return _remoteContractor.SendPacketAsync(4, data.Length, data);
        }
        public Task<int> SendToClientAsync(ushort header, params object[] values)
        {
            return SendToClientAsync(HMessage.Construct(header, values));
        }

        public Task<int> SendToServerAsync(byte[] data)
        {
            return _remoteContractor.SendPacketAsync(5, data.Length, data);
        }
        public Task<int> SendToServerAsync(ushort header, params object[] values)
        {
            return SendToServerAsync(HMessage.Construct(header, values));
        }
    }
}