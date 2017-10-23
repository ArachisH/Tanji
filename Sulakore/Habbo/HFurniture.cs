using System.Globalization;
using System.Collections.Generic;

using Sulakore.Protocol;

namespace Sulakore.Habbo
{
    public class HFurniture
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public int OwnerId { get; set; }
        public string OwnerName { get; set; }

        public HPoint Tile { get; set; }
        public HDirection Direction { get; set; }

        public HFurniture(int id, int typeId, int ownerId,
            string ownerName, HPoint tile, HDirection direction)
        {
            Id = id;
            TypeId = typeId;
            OwnerId = ownerId;
            OwnerName = ownerName;
            Tile = tile;
            Direction = direction;
        }

        public static IReadOnlyList<HFurniture> Parse(HMessage packet)
        {
            int ownersCount = packet.ReadInteger();
            var owners = new Dictionary<int, string>(ownersCount);

            for (int i = 0; i < ownersCount; i++)
            {
                int ownerId = packet.ReadInteger();
                string ownerName = packet.ReadString();

                owners.Add(ownerId, ownerName);
            }

            int furnitureCount = packet.ReadInteger();
            var furnitureList = new List<HFurniture>(furnitureCount);

            for (int i = 0; i < furnitureList.Capacity; i++)
            {
                int id = packet.ReadInteger();
                int typeId = packet.ReadInteger();

                int x = packet.ReadInteger();
                int y = packet.ReadInteger();
                var direction = (HDirection)packet.ReadInteger();
                var z = double.Parse(packet.ReadString(), CultureInfo.InvariantCulture);

                packet.ReadString();
                packet.ReadInteger();

                int category = packet.ReadInteger();
                HStuffData.ReadStuffData(category, packet);

                packet.ReadInteger();
                packet.ReadInteger();

                int ownerId = packet.ReadInteger();
                if (typeId < 0) packet.ReadString();

                var furniture = new HFurniture(id, typeId, ownerId,
                    owners[ownerId], new HPoint(x, y, z), direction);

                furnitureList.Add(furniture);
            }
            return furnitureList;
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(TypeId)}: {TypeId}, " +
                $"{nameof(OwnerId)}: {OwnerId}, {nameof(OwnerName)}: {OwnerName}";
        }
    }
}