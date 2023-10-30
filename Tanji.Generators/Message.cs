using System.Text.Json.Serialization;

namespace Tanji.Generators;

public sealed record Message
{
    public string Name { get; set; }
    public short UnityId { get; set; }
    public string UnityStructure { get; set; }
    public uint[] PostShuffleHashes { get; set; }

    [JsonIgnore]
    public string BackingFieldName { get; set; }
}