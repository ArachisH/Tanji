using System.Text;
using System.Drawing;
using System.Diagnostics.CodeAnalysis;

using Tanji.Core.Net.Messages;

namespace Tanji.Core.Infrastructure.Models;

public sealed class PacketLog
{
    private static ReadOnlySpan<char> Superscripts =>
        ['\u2070', '\u00B9', '\u00B2', '\u00B3', '\u2074',
         '\u2075', '\u2076', '\u2077', '\u2078', '\u2079'];

    public Queue<(int, Color)> Chunks { get; }
    public string? FullText { get; private set; }

    public required HMessage Message { get; init; }
    public required ulong Fingerprint { get; init; }

    public required int PacketLength { get; init; }
    public required string PacketText { get; init; }

    [SetsRequiredMembers]
    public PacketLog(ulong fingerprint, ref HMessage message, ref ReadOnlySpan<byte> packetBufferSpan)
    {
        Message = message;
        Fingerprint = fingerprint;
        Chunks = new Queue<(int, Color)>();
        PacketLength = packetBufferSpan.Length;
        PacketText = ToString(packetBufferSpan);
    }

    public void Generate(IPacketHighlightProvider highlights)
    {
        var allChunkText = new StringBuilder();
        if (!string.IsNullOrWhiteSpace(Message.Structure))
        {
            WriteChunk(allChunkText, '[', highlights.DefaultHighlight);
            WriteChunk(allChunkText, Message.Structure, highlights.StructureHighlight);
            WriteChunk(allChunkText, "]\n", highlights.DefaultHighlight);
        }

        bool hasName = !string.IsNullOrWhiteSpace(Message.Name);
        if (hasName)
        {
            WriteChunk(allChunkText, '[', highlights.DefaultHighlight);
            WriteChunk(allChunkText, Message.Name, highlights.DetailHighlight);
        }

        bool hasHash = Message.Hash > 0;
        if (hasHash)
        {
            if (hasName)
            {
                WriteChunk(allChunkText, ", mHash: ", highlights.DefaultHighlight);
            }
            WriteChunk(allChunkText, Message.Hash.ToString(), highlights.DetailHighlight);
        }

        if (!hasName && !hasHash)
        {
            WriteChunk(allChunkText, "< ! Unmapped Message ! >\n", Color.HotPink);
        }
        else
        {
            WriteChunk(allChunkText, "]\n", highlights.DefaultHighlight);
        }

        char arrow = Message.IsOutgoing ? '⇾' : '⇽';
        string direction = Message.IsOutgoing ? "Outgoing" : "Incoming";
        Color directionHighlight = Message.IsOutgoing ? highlights.OutgoingHighlight : highlights.IncomingHighlight;

        WriteChunk(allChunkText, direction, directionHighlight);
        WriteChunk(allChunkText, " [", highlights.DefaultHighlight);
        WriteChunk(allChunkText, Message.Id.ToString(), directionHighlight);
        WriteChunk(allChunkText, "] ", highlights.DefaultHighlight);
        WriteChunk(allChunkText, arrow, highlights.DefaultHighlight);
        WriteChunk(allChunkText, ' ', highlights.DefaultHighlight);
        WriteChunk(allChunkText, PacketText, directionHighlight);
        WriteChunk(allChunkText, "\n---------------\n", highlights.DefaultHighlight);

        FullText = allChunkText.ToString();
    }

    private void WriteChunkPosition(int length, Color highlight)
    {
        Chunks.Enqueue((length, highlight));
    }
    private void WriteChunk(StringBuilder builder, char character, Color highlight)
    {
        builder.Append(character);
        WriteChunkPosition(1, highlight);
    }
    private void WriteChunk(StringBuilder builder, ReadOnlySpan<char> text, Color highlight)
    {
        builder.Append(text);
        WriteChunkPosition(text.Length, highlight);
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