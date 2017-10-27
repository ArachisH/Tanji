using Sulakore.Communication;

namespace Tanji.Manipulators
{
    public interface IReceiver
    {
        bool IsReceiving { get; }

        void HandleOutgoing(DataInterceptedEventArgs e);
        void HandleIncoming(DataInterceptedEventArgs e);
    }
}