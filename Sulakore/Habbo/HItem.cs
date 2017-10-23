using Sulakore.Protocol;

using System.Collections.Generic;

namespace Sulakore.Habbo
{
    public class HItem
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public int RoomId { get; set; }
        public int Category { get; set; }
        public string SlotId { get; set; }
        public int SecondsToExpiration { get; set; }
        public bool HasRentPeriodStarted { get; set; }

        public HItem(int id, int typeId, int category,
            int secondsToExpiration, bool hasRentPeriodStarted, int roomId)
        {
            Id = id;
            TypeId = typeId;
            Category = category;
            SecondsToExpiration = secondsToExpiration;
            HasRentPeriodStarted = hasRentPeriodStarted;
            RoomId = roomId;
        }

        public static IReadOnlyList<HItem> Parse(HMessage packet)
        {
            packet.ReadInteger();
            packet.ReadInteger();

            int itemCount = packet.ReadInteger();
            var itemList = new List<HItem>(itemCount);

            for (int i = 0; i < itemList.Capacity; i++)
            {
                packet.ReadInteger();
                string s1 = packet.ReadString();

                int id = packet.ReadInteger();
                int typeId = packet.ReadInteger();
                packet.ReadInteger();

                int category = packet.ReadInteger();
                HStuffData.ReadStuffData(category, packet);

                packet.ReadBoolean();
                packet.ReadBoolean();
                packet.ReadBoolean();
                packet.ReadBoolean();
                int secondsToExpiration = packet.ReadInteger();

                bool hasRentPeriodStarted = packet.ReadBoolean();
                int roomId = packet.ReadInteger();

                var item = new HItem(id, typeId, category,
                    secondsToExpiration, hasRentPeriodStarted, roomId);

                if (s1 == "S")
                {
                    item.SlotId = packet.ReadString();
                    packet.ReadInteger();
                }
                itemList.Add(item);
            }
            return itemList;
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(TypeId)}: {TypeId}, " +
                $"{nameof(RoomId)}: {RoomId}, {nameof(Category)}: {Category}, {nameof(SlotId)}: {SlotId}, " +
                $"{nameof(SecondsToExpiration)}: {SecondsToExpiration}, {nameof(HasRentPeriodStarted)}: {HasRentPeriodStarted}";
        }
    }
}