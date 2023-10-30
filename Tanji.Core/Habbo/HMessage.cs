using System.Diagnostics;

namespace Tanji.Core.Habbo;

[DebuggerDisplay("{Id,nq}")]
public record HMessage(string? Name, short Id, uint Hash, string? Structure, bool IsOutgoing, string? TypeName, string? ParserTypeName, int References)
{
    public static implicit operator short(HMessage message) => message.Id;
    public static implicit operator string?(HMessage message) => message.Name;
    public static implicit operator bool(HMessage message) => message.IsOutgoing;

    public HMessage(short id, bool isOutgoing)
        : this(null, id, 0, null, isOutgoing, null, null, 0)
    { }
}