using System.Text;
using System.Drawing;
using System.Diagnostics.CodeAnalysis;

using Tanji.Core.Net.Messages;
using Tanji.Core.Infrastructure.Configuration;

namespace Tanji.Core.Infrastructure.Models;

/*
 * TODO: Check if it makes sense to instead use this as a 'StringBuilder',
 * and have it return 'chunks' that only consist of Text, and Highlight regions.
 */
public sealed class PacketLog
{
    public const string DEFAULT_SEPARATOR = "\n---------------\n";

    private static readonly char[] DIRECTION_ARROWS = ['⇽', '⇾'];
    private static readonly string[] DIRECTION_LABELS = ["Incoming", "Outgoing"];

    private readonly StringBuilder _logText;
    private readonly Queue<(int, Color)> _highlights;

    private static ReadOnlySpan<char> Superscripts =>
        ['\u2070', '\u00B9', '\u00B2', '\u00B3', '\u2074',
         '\u2075', '\u2076', '\u2077', '\u2078', '\u2079'];

    public int Length => _logText.Length;
    public int Repetitions { get; set; } = 1;

    public Color DefaultHighlight { get; init; }

    [SetsRequiredMembers]
    public PacketLog(Color defaultHighlight)
    {
        _logText = new StringBuilder();
        _highlights = new Queue<(int, Color)>();

        DefaultHighlight = defaultHighlight;
    }

    public PacketLog AppendLine() => Append('\n');
    public PacketLog AppendSpace() => Append(' ');

    public PacketLog Open() => Append('[');
    public PacketLog Close() => Append(']');

    public PacketLog Enclose(in ReadOnlySpan<char> value) => Open().Append(value, DefaultHighlight).Close();
    public PacketLog Enclose(in ReadOnlySpan<char> value, Color highlight) => Open().Append(value, highlight).Close();

    public PacketLog Append(char character) => Append(character, DefaultHighlight);
    public PacketLog Append(in ReadOnlySpan<char> value) => Append(value, DefaultHighlight);

    // TODO: Use indexed based highlights for reducing amount of tuples being queued?
    public PacketLog Append(char character, Color highlight)
    {
        _logText.Append(character);
        _highlights.Enqueue((1, highlight));

        return this;
    }
    public PacketLog Append(in ReadOnlySpan<char> value, Color highlight)
    {
        _logText.Append(value);
        _highlights.Enqueue((value.Length, highlight));

        return this;
    }

    public override string ToString() => _logText.ToString();
    public IEnumerable<(int length, Color highlight)> GetHighlights() => _highlights;

    public static PacketLog Create(ReadOnlySpan<byte> packetBufferSpan, in HMessage message, PacketLoggingOptions options)
    {
        var pLog = new PacketLog(options.DefaultHighlight);

        if (IsLoggable(message.Structure, options.IsLoggingStructure))
        {
            // [%Structure%]\n
            pLog.Enclose(message.Structure, options.StructureHighlight).AppendLine();
        }

        bool hasMessageHash = false, hasMessageName = false;
        if (IsLoggable(message.Name, options.IsLoggingMessageName))
        {
            // [%Name%]_
            hasMessageName = true;
            pLog.Enclose(message.Name, options.DetailHighlight).AppendSpace();
        }

        if (hasMessageHash && options.IsLoggingMessageHash)
        {
            // [mHash: %Hash%]_
            hasMessageHash = true;

            pLog.Open()
                .Append("mHash: ")
                .Append(message.Hash.ToString(), options.DetailHighlight)
                .Close().AppendLine();
        }

        if ((!hasMessageName && options.IsLoggingMessageName) ||
            (!hasMessageHash && options.IsLoggingMessageHash))
        {
            pLog.Enclose("Unknown Message", Color.HotPink).AppendLine();
        }

        int directionIndex = message.IsOutgoing ? 1 : 0;
        Color directionHighlight = message.IsOutgoing ? options.OutgoingHighlight : options.IncomingHighlight;

        pLog.Append(DIRECTION_LABELS[directionIndex], directionHighlight).AppendSpace()
            .Enclose(message.Id.ToString(), directionHighlight).AppendSpace()
            .Append(DIRECTION_ARROWS[directionIndex]).AppendSpace()
            .Append(ToString(packetBufferSpan), directionHighlight)
            .Append(DEFAULT_SEPARATOR);

        return pLog;
    }

    private static string ToString(ReadOnlySpan<byte> bufferSpan)
    {
        int nullByteCount = 0;
        var builder = new StringBuilder(bufferSpan.Length * 2);
        for (int i = 0; i < bufferSpan.Length; i++)
        {
            byte bufferSpanByte = bufferSpan[i];
            if (bufferSpanByte == 0)
            {
                nullByteCount++;
                continue; // Do not append, continue until end of null bytes for compacting.
            }

            if (nullByteCount > 0)
            {
                builder.Append('⦏').Append('\u2400');
                if (nullByteCount > 1)
                {
                    AppendSuperscript(builder, nullByteCount);
                }
                builder.Append('⦐');
                nullByteCount = 0;
            }

            if (bufferSpanByte is <= 31 or (>= 127 and <= 159))
            {
                builder.Append('⦏').Append(bufferSpanByte.ToString("X2")).Append('⦐');
            }
            else builder.Append((char)bufferSpanByte);
        }
        return builder.ToString();
    }
    private static bool IsLoggable(in ReadOnlySpan<char> value, bool isLogging)
    {
        return isLogging && !value.IsWhiteSpace();
    }
    private static StringBuilder AppendSuperscript(StringBuilder builder, int number)
    {
        if (number >= 10)
        {
            int charsUsed = 0;
            Span<char> joined = stackalloc char[10];
            for (int i = joined.Length - 1; i >= 0 && number > 0; i--, charsUsed++)
            {
                joined[i] = Superscripts[number % 10];
                number /= 10;
            }
            builder.Append(joined[^charsUsed..]);
        }
        else if (number >= 0)
        {
            builder.Append(Superscripts[number]);
        }
        return builder;
    }
}