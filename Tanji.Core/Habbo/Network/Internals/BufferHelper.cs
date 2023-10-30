using System.Buffers;
using System.Diagnostics;

namespace Tanji.Core.Habbo.Network;

internal static class BufferHelper
{
    [DebuggerStepThrough]
    public static IMemoryOwner<byte> Rent(int minBufferSize, out Memory<byte> trimmedRegion)
    {
        var trimmedOwner = MemoryPool<byte>.Shared.Rent(minBufferSize);
        trimmedRegion = trimmedOwner.Memory.Slice(0, minBufferSize);
        return trimmedOwner;
    }
}