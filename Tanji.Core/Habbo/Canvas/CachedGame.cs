using System.Text.Json;
using System.Text.Json.Nodes;

using Tanji.Core.Habbo.Network.Formats;

namespace Tanji.Core.Habbo.Canvas;

public sealed class CachedGame : HGame
{
    private const string EVAWIRE_NAME = "EvaWire";
    private const string WEDGIE_IN_NAME = "Wedgie-In";
    private const string WEDGIE_OUT_NAME = "Wedgie-Out";
    private const string EVAWIRE_UNITY_NAME = "EvaWire-Unity";

    private readonly Dictionary<uint, HMessage> _messagesByHash;
    private readonly Dictionary<string, HMessage> _inMessagesByName;
    private readonly Dictionary<string, HMessage> _outMessagesByName;

    public override HPlatform Platform { get; }
    public override int MinimumConnectionAttempts { get; }

    public override IHFormat SendPacketFormat { get; }
    public override IHFormat ReceivePacketFormat { get; }

    public CachedGame(string cachedGameJsonPath)
        : base(HPatches.None)
    {
        if (!File.Exists(cachedGameJsonPath))
        {
            ThrowHelper.ThrowFileNotFoundException("Failed to locate the specified json file.", cachedGameJsonPath);
        }

        var cachedGameNode = JsonNode.Parse(File.ReadAllBytes(cachedGameJsonPath));
        if (cachedGameNode == null)
        {
            ThrowHelper.ThrowArgumentException($"Failed to parse the json file located at: {cachedGameJsonPath}", nameof(cachedGameJsonPath));
        }

        Path = GetNonNullableValue<string>(cachedGameNode, Camelize(nameof(Path)));
        Platform = Enum.Parse<HPlatform>(GetNonNullableValue<string>(cachedGameNode, Camelize(nameof(Platform))));
        Revision = GetNonNullableValue<string>(cachedGameNode, Camelize(nameof(Revision)));

        IsPostShuffle = GetNonNullableValue<bool>(cachedGameNode, Camelize(nameof(IsPostShuffle)));
        MinimumConnectionAttempts = GetNonNullableValue<int>(cachedGameNode, Camelize(nameof(MinimumConnectionAttempts)));

        SendPacketFormat = GetFormat(GetNonNullableValue<string>(cachedGameNode, Camelize(nameof(SendPacketFormat))));
        ReceivePacketFormat = GetFormat(GetNonNullableValue<string>(cachedGameNode, Camelize(nameof(ReceivePacketFormat))));

        var incomingNode = GetNonNullableValue<JsonArray>(cachedGameNode, Camelize(nameof(Incoming)));
        _inMessagesByName = new Dictionary<string, HMessage>(incomingNode.Count);

        var outgoingNode = GetNonNullableValue<JsonArray>(cachedGameNode, Camelize(nameof(Outgoing)));
        _outMessagesByName = new Dictionary<string, HMessage>(outgoingNode.Count);

        _messagesByHash = new Dictionary<uint, HMessage>(outgoingNode.Count + incomingNode.Count);
        CacheMessages(incomingNode, _inMessagesByName, _messagesByHash, false);
        CacheMessages(outgoingNode, _outMessagesByName, _messagesByHash, true);
    }

    public override bool TryResolveMessage(string name, uint hash, bool isOutgoing, out HMessage message)
    {
        if (!_messagesByHash.TryGetValue(hash, out message))
        {
            message = (isOutgoing ? _outMessagesByName : _inMessagesByName).GetValueOrDefault(name);
        }
        return message != default;
    }

    protected override void Dispose(bool disposing)
    {
        if (!disposing) return;

        _messagesByHash.Clear();
        _messagesByHash.EnsureCapacity(0);

        _inMessagesByName.Clear();
        _inMessagesByName.EnsureCapacity(0);

        _outMessagesByName.Clear();
        _outMessagesByName.EnsureCapacity(0);

    }

    public static void SaveAs(string cachedGameJsonPath, HGame game, Outgoing outgoing, Incoming incoming)
    {
        using FileStream messagesJsonStream = File.Open(cachedGameJsonPath, FileMode.Create);
        using var cachedGameJson = new Utf8JsonWriter(messagesJsonStream, new JsonWriterOptions { Indented = true });

        cachedGameJson.WriteStartObject();
        cachedGameJson.WriteString(Camelize(nameof(game.Path)), game.Path);
        cachedGameJson.WriteString(Camelize(nameof(game.Platform)), game.Platform.ToString());
        cachedGameJson.WriteString(Camelize(nameof(game.Revision)), game.Revision);
        cachedGameJson.WriteBoolean(Camelize(nameof(game.IsPostShuffle)), game.IsPostShuffle);
        cachedGameJson.WriteNumber(Camelize(nameof(game.MinimumConnectionAttempts)), game.MinimumConnectionAttempts);

        cachedGameJson.WriteString(Camelize(nameof(game.SendPacketFormat)), GetFormatName(game.SendPacketFormat));
        cachedGameJson.WriteString(Camelize(nameof(game.ReceivePacketFormat)), GetFormatName(game.ReceivePacketFormat));

        WriteIdentifiers(cachedGameJson, Camelize(nameof(Outgoing)), outgoing);
        WriteIdentifiers(cachedGameJson, Camelize(nameof(Incoming)), incoming);

        cachedGameJson.WriteEndObject();
        cachedGameJson.Flush();
    }

    private static IHFormat GetFormat(string name) => name switch
    {
        EVAWIRE_NAME => IHFormat.EvaWire,
        EVAWIRE_UNITY_NAME => IHFormat.EvaWireUnity,

        WEDGIE_IN_NAME => IHFormat.WedgieIn,
        WEDGIE_OUT_NAME => IHFormat.WedgieOut,

        _ => throw new NotSupportedException("Format is not recognized internally.")
    };
    private static string GetFormatName(IHFormat format) => format switch
    {
        EvaWireFormat evaWireFormat when evaWireFormat.IsUnity => EVAWIRE_UNITY_NAME,
        EvaWireFormat => EVAWIRE_NAME,

        WedgieFormat wedgieFormat when wedgieFormat.IsOutgoing => WEDGIE_OUT_NAME,
        WedgieFormat => WEDGIE_IN_NAME,

        _ => throw new NotSupportedException("Format is not recognized internally.")
    };

    public static string Camelize(ReadOnlySpan<char> value)
    {
        Span<char> camelCased = stackalloc char[value.Length];
        camelCased[0] = char.ToLower(value[0]);
        for (int i = 1; i < value.Length; i++)
        {
            camelCased[i] = value[i];
        }
        return new string(camelCased);
    }
    private static T GetNonNullableValue<T>(JsonNode? parentNode, string propertyName)
    {
        if (parentNode == null)
        {
            ThrowHelper.ThrowNullReferenceException("The parent node can not be null.");
        }

        JsonNode? childNode = parentNode[propertyName];
        if (childNode == null)
        {
            ThrowHelper.ThrowNullReferenceException("The child node was not found in the parent node.");
        }

        return childNode.GetValue<T>();
    }
    private static void WriteIdentifiers(Utf8JsonWriter cachedGameJson, string propertyName, Identifiers identifiers)
    {
        cachedGameJson.WriteStartArray(propertyName);
        foreach (HMessage message in identifiers)
        {
            cachedGameJson.WriteStartObject();
            cachedGameJson.WriteString(Camelize(nameof(HMessage.Name)), message.Name);
            cachedGameJson.WriteNumber(Camelize(nameof(HMessage.Id)), message.Id);
            cachedGameJson.WriteNumber(Camelize(nameof(HMessage.Hash)), message.Hash);
            cachedGameJson.WriteString(Camelize(nameof(HMessage.Structure)), message.Structure);
            cachedGameJson.WriteNumber(Camelize(nameof(HMessage.References)), message.References);
            cachedGameJson.WriteString(Camelize(nameof(HMessage.TypeName)), message.TypeName);
            cachedGameJson.WriteString(Camelize(nameof(HMessage.ParserTypeName)), message.ParserTypeName);
            cachedGameJson.WriteEndObject();
        }
        cachedGameJson.WriteEndArray();
    }
    private static void CacheMessages(JsonArray identifiers, Dictionary<string, HMessage> messagesByName, Dictionary<uint, HMessage> _messagesByHash, bool isOutgoing)
    {
        for (int i = 0; i < identifiers.Count; i++)
        {
            var message = new HMessage(GetNonNullableValue<short>(identifiers[i], Camelize(nameof(HMessage.Id))), isOutgoing)
            {
                Name = GetNonNullableValue<string>(identifiers[i], Camelize(nameof(HMessage.Name))),
                Hash = GetNonNullableValue<uint>(identifiers[i], Camelize(nameof(HMessage.Hash))),
                Structure = GetNonNullableValue<string>(identifiers[i], Camelize(nameof(HMessage.Structure))),
                TypeName = GetNonNullableValue<string>(identifiers[i], Camelize(nameof(HMessage.TypeName))),
                ParserTypeName = GetNonNullableValue<string>(identifiers[i], Camelize(nameof(HMessage.ParserTypeName))),
                References = GetNonNullableValue<int>(identifiers[i], Camelize(nameof(HMessage.References)))
            };
            messagesByName.Add(message.Name, message);
            _messagesByHash.Add(message.Hash, message);
        }
    }
}