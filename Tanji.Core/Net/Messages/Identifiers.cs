using System.Diagnostics;

using Tanji.Core.Canvas;

namespace Tanji.Core.Net.Messages;

[DebuggerDisplay("Resolved = {Resolved,nq}")]
public abstract class Identifiers
{
    public bool IsOutgoing { get; init; }
    public int Resolved { get; private set; }

    public Identifiers(bool isOutgoing)
    {
        IsOutgoing = isOutgoing;
    }

    protected void Register(HMessage value, ref HMessage backingField)
    {
        backingField = value;
        Resolved += value == default ? -1 : 1;
    }
    protected HMessage ResolveMessage(IGame game, string name, short unityId, ReadOnlySpan<uint> postShuffleHashes)
    {
        HMessage message = default;
        if (game.Platform != HPlatform.Unity)
        {
            // Resolve by Hash
            for (int i = 0; i < postShuffleHashes.Length; i++)
            {
                if (game.TryResolveMessage(postShuffleHashes[i], out message)) return message;
            }

            // Resolve by Name
            if (game.TryResolveMessage(name, out message)) return message;
        }
        else if (unityId > 0) return new HMessage(unityId, IsOutgoing);

        return message;
    }
}