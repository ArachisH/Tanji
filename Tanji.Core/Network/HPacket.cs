using Tanji.Core.Habbo.Network.Buffers;

namespace Tanji.Core.Network;

public readonly record struct HPacket(HPacketWriter Writer, short Id, bool IsOutgoing, int Step);