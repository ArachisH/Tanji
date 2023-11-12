using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Tanji.Core.Habbo;

[DebuggerDisplay("{Id,nq}")]
public readonly record struct HMessage
{
    public readonly required short Id { get; init; }
    public readonly required bool IsOutgoing { get; init; }

    public readonly uint Hash { get; init; }
    public readonly int References { get; init; }

    public readonly string? Name { get; init; }
    public readonly string? Structure { get; init; }

    public readonly string? TypeName { get; init; }
    public readonly string? ParserTypeName { get; init; }

    [SetsRequiredMembers]
    public HMessage(short id, bool isOutgoing)
    {
        Id = id;
        IsOutgoing = isOutgoing;
    }
}