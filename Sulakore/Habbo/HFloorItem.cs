using System.Globalization;
using System.Collections.Generic;

using Sulakore.Protocol;

namespace Sulakore.Habbo
{
    public class HFloorItem : HData
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public HPoint Tile { get; set; }
        public HDirection Facing { get; set; }

        public int Category { get; set; }
        public object[] Stuff { get; set; }

        public int SecondsToExpiration { get; set; }
        public int UsagePolicy { get; set; }

        public int OwnerId { get; set; }
        public string OwnerName { get; set; }

        public HFloorItem(HMessage packet)
        {
            Id = packet.ReadInteger();
            TypeId = packet.ReadInteger();

            var tile = new HPoint(packet.ReadInteger(), packet.ReadInteger());
            Facing = (HDirection)packet.ReadInteger();

            tile.Z = double.Parse(packet.ReadString(), CultureInfo.InvariantCulture);
            Tile = tile;

            packet.ReadString();
            packet.ReadInteger();

            Category = packet.ReadInteger();
            Stuff = ReadData(packet, Category);

            SecondsToExpiration = packet.ReadInteger();
            UsagePolicy = packet.ReadInteger();

            OwnerId = packet.ReadInteger();
            if (TypeId < 0)
            {
                packet.ReadString();
            }
        }

        public void Update(HFloorItem furni)
        {
            Tile = furni.Tile;
            Stuff = furni.Stuff;
            Facing = furni.Facing;
        }

        public static HFloorItem[] Parse(HMessage packet)
        {
            int ownersCount = packet.ReadInteger();
            var owners = new Dictionary<int, string>(ownersCount);
            for (int i = 0; i < ownersCount; i++)
            {
                owners.Add(packet.ReadInteger(), packet.ReadString());
            }

            var furniture = new HFloorItem[packet.ReadInteger()];
            for (int i = 0; i < furniture.Length; i++)
            {
                var furni = new HFloorItem(packet);
                furni.OwnerName = owners[furni.OwnerId];

                furniture[i] = furni;
            }
            return furniture;
        }
    }
}