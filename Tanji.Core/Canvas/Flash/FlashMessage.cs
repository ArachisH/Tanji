using System.Text;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using Flazzy.ABC;

namespace Tanji.Core.Canvas.Flash;

[DebuggerDisplay("{ToString()}")]
public sealed record FlashMessage
{
    public required short Id { get; init; }
    public required bool IsOutgoing { get; init; }
    public required ASClass MessageClass { get; init; }

    public uint Hash { get; set; }
    public string? Structure { get; set; }
    public ASClass? ParserClass { get; set; }

    public List<FlashMessageReference> References { get; }

    public FlashMessage()
    {
        References = new List<FlashMessageReference>(10);
    }

    [SetsRequiredMembers]
    public FlashMessage(short id, bool isOutgoing, ASClass messageClass)
        : this()
    {
        ArgumentNullException.ThrowIfNull(messageClass, nameof(messageClass));

        Id = id;
        IsOutgoing = isOutgoing;
        MessageClass = messageClass;
    }

    public override string ToString()
    {
        var builder = new StringBuilder();
        builder.Append($"({Id}, {MessageClass.QName.Name}");
        if (ParserClass != null)
        {
            builder.Append($", {ParserClass.QName.Name}");
        }
        if (!string.IsNullOrWhiteSpace(Structure))
        {
            builder.Append($", {Structure}");
        }
        return builder.Append(')').ToString();
    }
}