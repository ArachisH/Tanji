using Sulakore.Protocol;

namespace Sulakore.Habbo
{
    public class HFriendsInterface
    {
        public int Unk01 { get; set; }
        public int Unk02 { get; set; }
        public int Limit { get; set; }
        public int Unk04 { get; set; }
        public int Unk05 { get; set; }
        public int Unk06 { get; set; }

        public HFriendsInterface(HMessage packet)
        {
            Unk01 = packet.ReadInteger();
            Unk02 = packet.ReadInteger();
            Limit = packet.ReadInteger();
            Unk04 = packet.ReadInteger();
            Unk05 = packet.ReadInteger();
            Unk06 = packet.ReadInteger();
        }
    }
}
