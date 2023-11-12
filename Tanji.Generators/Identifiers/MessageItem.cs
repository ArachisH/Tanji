using System.Text.Json.Serialization;

namespace Tanji.Generators.Identifiers;

internal sealed record MessageItem
{
    public string Name { get; init; } = string.Empty;
    public short UnityId { get; init; }

    public string UnityStructure { get; init; } = string.Empty;
    public uint[] PostShuffleHashes { get; init; } = Array.Empty<uint>();

    [JsonIgnore]
    public string BackingFieldName { get; set; } = string.Empty;
}