using System;
using System.Threading.Tasks;

using Sulakore.Protocol;

namespace Sulakore.Communication
{
    public interface IHConnection : IDisposable
    {
        ushort Port { get; }
        string Host { get; }
        string Address { get; }

        int TotalOutgoing { get; }
        int TotalIncoming { get; }

        HNode Local { get; }
        HNode Remote { get; }

        Task<int> SendToServerAsync(byte[] data);
        Task<int> SendToServerAsync(HMessage packet);
        Task<int> SendToServerAsync(ushort id, params object[] values);

        Task<int> SendToClientAsync(byte[] data);
        Task<int> SendToClientAsync(HMessage packet);
        Task<int> SendToClientAsync(ushort id, params object[] values);
    }
}