using System.Threading.Tasks;

namespace Sulakore.Communication
{
    public interface IHConnection
    {
        ushort Port { get; }
        string Host { get; }
        string Address { get; }

        int TotalOutgoing { get; }
        int TotalIncoming { get; }

        Task<int> SendToServerAsync(byte[] data);
        Task<int> SendToServerAsync(ushort header, params object[] values);

        Task<int> SendToClientAsync(byte[] data);
        Task<int> SendToClientAsync(ushort header, params object[] values);
    }
}