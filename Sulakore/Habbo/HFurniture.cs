using System.Globalization;
using System.Collections.Generic;

using Sulakore.Protocol;

namespace Sulakore.Habbo
{
    public class HFurniture : HData
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public HPoint Tile { get; set; }
        public HDirection Facing { get; set; }

        public int Category { get; set; }
        public object[] Stuff { get; set; }

        public int OwnerId { get; set; }
        public string OwnerName { get; set; }

        public HFurniture(HMessage packet)
        {
            Id = packet.ReadInteger();
            TypeId = packet.ReadInteger();

            Tile = new HPoint(packet.ReadInteger(), packet.ReadInteger());
            Facing = (HDirection)packet.ReadInteger();
            Tile.Z = double.Parse(packet.ReadString(), CultureInfo.InvariantCulture);

            var loc1 = packet.ReadString();
            var loc3 = packet.ReadInteger();

            Category = packet.ReadInteger();
            Stuff = ReadData(packet, Category);

            var loc4 = packet.ReadInteger();
            var loc5 = packet.ReadInteger();

            OwnerId = packet.ReadInteger();
            if (TypeId < 0)
            {
                var loc6 = packet.ReadString();
            }
        }

        public void Update(HFurniture furni)
        {
            Tile = furni.Tile;
            Stuff = furni.Stuff;
            Facing = furni.Facing;
        }

        public static HFurniture[] Parse(HMessage packet)
        {
            int ownersCount = packet.ReadInteger();
            var owners = new Dictionary<int, string>(ownersCount);
            for (int i = 0; i < ownersCount; i++)
            {
                owners.Add(packet.ReadInteger(), packet.ReadString());
            }

            var furniture = new HFurniture[packet.ReadInteger()];
            for (int i = 0; i < furniture.Length; i++)
            {
                var furni = new HFurniture(packet);
                furni.OwnerName = owners[furni.OwnerId];

                furniture[i] = furni;
            }
            return furniture;
        }
    }
}