using System.Text;
using System.Diagnostics;

using Flazzy.ABC;

namespace Tanji.Core.Habbo.Canvas.Flash;

[DebuggerDisplay("{ToString()}")]
public readonly record struct FlashMessage(short Id, string? Structure, bool IsOutgoing, ASClass MessageClass, ASClass? ParserClass, List<FlashMessageReference> References)
{
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