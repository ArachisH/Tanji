using Sulakore.Habbo;

namespace Tanji.Habbo
{
    public abstract class TGame : IGame
    {
        public Incoming In { get; protected set; }
        public Outgoing Out { get; protected set; }

        public string Path { get; init; }
        public bool IsUnity { get; init; }

        public string Revision { get; protected set; }
        public bool IsPostShuffle { get; protected set; }

        public abstract short Resolve(string name, bool isOutgoing);
        public abstract MessageInfo GetInformation(HMessage message);
    }
}