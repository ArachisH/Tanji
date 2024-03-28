using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Tanji.Core.Net.Messages;

[DebuggerDisplay("{Id,nq}")]
public readonly record struct HMessage
{
    public required short Id { get; init; }
    public required bool IsOutgoing { get; init; }

    public uint Hash { get; init; }

    [SetsRequiredMembers]
    public HMessage(short id, bool isOutgoing)
    {
        Id = id;
        IsOutgoing = isOutgoing;
    }
}