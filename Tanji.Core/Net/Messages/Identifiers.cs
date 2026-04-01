using System.Diagnostics;
using System.Runtime.InteropServices;

using Tanji.Core.Canvas;

namespace Tanji.Core.Net.Messages;

[DebuggerDisplay("Resolved = {Resolved,nq}")]
public abstract class Identifiers
{
    private readonly Dictionary<short, HMessage> _messagesById;
    private readonly Dictionary<uint, HMessage> _messagesByHash;
    private readonly Dictionary<string, HMessage> _messagesByName;

    public bool IsOutgoing { get; init; }
    public int Resolved { get; private set; }

    public ref readonly HMessage this[short id] => ref CollectionsMarshal.GetValueRefOrNullRef(_messagesById, id);
    //public ref readonly HMessage this[uint hash] => ref CollectionsMarshal.GetValueRefOrNullRef(_messagesByHash, hash);
    public ref readonly HMessage this[string name] => ref CollectionsMarshal.GetValueRefOrNullRef(_messagesByName, name);

    public Identifiers(bool isOutgoing)
    {
        _messagesById = [];
        _messagesByHash = [];
        _messagesByName = [];

        IsOutgoing = isOutgoing;
    }

    protected void Register(ref HMessage backingField, string name, HMessage value)
    {
        backingField = value;
        if (value != default)
        {
            _messagesByName.Add(name, value);
            _messagesById.Add(value.Id, value);
            _messagesByHash.Add(value.Hash, value);
            Resolved++;
        }
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