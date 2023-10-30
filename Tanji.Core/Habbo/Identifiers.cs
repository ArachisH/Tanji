using System.Collections;

using Tanji.Core.Habbo.Canvas;

namespace Tanji.Core.Habbo;

public abstract class Identifiers : IReadOnlyList<HMessage>
{
    private readonly List<HMessage> _messages;
    private readonly Dictionary<short, HMessage> _byId;
    private readonly Dictionary<uint, HMessage> _byHash;
    private readonly Dictionary<string, HMessage> _byName;

    public bool IsOutgoing { get; }

    public HMessage this[short id] => _byId[id];
    public HMessage this[uint hash] => _byHash[hash];
    public HMessage this[string name] => _byName[name];

    protected Identifiers(bool isOutgoing)
        : this(768, isOutgoing)
    { }
    protected Identifiers(int count, bool isOutgoing)
    {
        IsOutgoing = isOutgoing;

        _messages = new List<HMessage>(count);
        _byId = new Dictionary<short, HMessage>(count);
        _byHash = new Dictionary<uint, HMessage>(count);
        _byName = new Dictionary<string, HMessage>(count);
    }

    public bool TryGetMessage(short id, out HMessage? message) => _byId.TryGetValue(id, out message);
    public bool TryGetMessage(uint hash, out HMessage? message) => _byHash.TryGetValue(hash, out message);
    public bool TryGetMessage(string name, out HMessage? message) => _byName.TryGetValue(name, out message);

    public void TrimExcess()
    {
        _byId.TrimExcess();
        _byHash.TrimExcess();
        _byName.TrimExcess();
        _messages.TrimExcess();
    }

    protected virtual void Register(HMessage message, string propertyName, ref HMessage backingField)
    {
        backingField = new HMessage(propertyName, message.Id, message.Hash, message.Structure, message.IsOutgoing, message.TypeName, message.ParserTypeName, message.References);

        _byId.Add(backingField.Id, backingField);
        _byName.Add(propertyName, backingField);
        if (!string.IsNullOrWhiteSpace(message.Name))
        {
            _byName.Add(message.Name, backingField);
        }
        if (message.Hash != 0)
        {
            _byHash.Add(message.Hash, backingField);
        }
        _messages.Add(backingField);
    }
    protected virtual HMessage ResolveMessage(IGame game, string name, short unityId, string? unityStructure, params uint[] postShuffleHashes)
    {
        HMessage? message = default;
        if (game.Platform != HPlatform.Unity)
        {
            for (int i = 0; i < postShuffleHashes.Length; i++)
            {
                if (game.TryResolveMessage(name, postShuffleHashes[i], IsOutgoing, out message)) break;
            }
        }
        else if (unityId > 0) message = new HMessage(name, unityId, 0, unityStructure, IsOutgoing, null, null, 0);

        if (message is not null)
        {
            _byId.Add(message.Id, message);
            if (game.Platform != HPlatform.Unity)
            {
                _byHash.Add(message.Hash, message);
            }
        }
        else throw new MessageResolvingException(name, game.Revision ?? "<none>");

        _byName.Add(name, message);
        _messages.Add(message);
        return message;
    }

    #region IReadOnlyList<HMessage> Implementation
    public int Count => _messages.Count;
    HMessage IReadOnlyList<HMessage>.this[int index] => _messages[index];

    IEnumerator IEnumerable.GetEnumerator() => _messages.GetEnumerator();
    public IEnumerator<HMessage> GetEnumerator() => ((IEnumerable<HMessage>)_messages).GetEnumerator();
    #endregion
}