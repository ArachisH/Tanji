using Sulakore.Protocol;

namespace Sulakore.Habbo
{
    public class HItem : HData
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public int RoomId { get; set; }
        public int Category { get; set; }
        public string SlotId { get; set; }
        public object[] Stuff { get; set; }
        public bool IsGroupable { get; set; }
        public int SecondsToExpiration { get; set; }
        public bool HasRentPeriodStarted { get; set; }

        public HItem(HMessage packet)
        {
            packet.ReadInteger();

            string unknown1 = packet.ReadString();

            Id = packet.ReadInteger();
            TypeId = packet.ReadInteger();
            packet.ReadInteger();

            Category = packet.ReadInteger();
            Stuff = ReadData(packet, Category);

            IsGroupable = packet.ReadBoolean();
            packet.ReadBoolean();
            packet.ReadBoolean();
            packet.ReadBoolean();
            SecondsToExpiration = packet.ReadInteger();

            HasRentPeriodStarted = packet.ReadBoolean();
            RoomId = packet.ReadInteger();

            if (unknown1 == "S")
            {
                SlotId = packet.ReadString();
                packet.ReadInteger();
            }
        }
        public static HItem[] Parse(HMessage packet)
        {
            int loc1 = packet.ReadInteger();
            int loc2 = packet.ReadInteger();

            var items = new HItem[packet.ReadInteger()];
            for (int i = 0; i < items.Length; i++)
            {
                items[i] = new HItem(packet);
            }
            return items;
        }
    }
}