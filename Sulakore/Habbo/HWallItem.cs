using System.Collections.Generic;

using Sulakore.Protocol;

namespace Sulakore.Habbo
{
    public class HWallItem : HData
    {
        public int Id { get; set; }
        public int TypeId { get; set; }

        public string Location { get; set; }
        public string State { get; set; }
        public int SecondsToExpiration { get; set; }
        public int UsagePolicy { get; set; }

        public int OwnerId { get; set; }
        public string OwnerName { get; set; }

        public HWallItem(HMessage packet)
        {
            Id = int.Parse(packet.ReadString());
            TypeId = packet.ReadInteger();

            Location = packet.ReadString();
            State = packet.ReadString();
            SecondsToExpiration = packet.ReadInteger();
            UsagePolicy = packet.ReadInteger();

            OwnerId = packet.ReadInteger();
        }

        public void Update(HWallItem furni)
        {
            Location = furni.Location;
            State = furni.State;
        }

        public static HWallItem[] Parse(HMessage packet)
        {
            int ownersCount = packet.ReadInteger();
            var owners = new Dictionary<int, string>(ownersCount);
            for (int i = 0; i < ownersCount; i++)
            {
                owners.Add(packet.ReadInteger(), packet.ReadString());
            }

            var furniture = new HWallItem[packet.ReadInteger()];
            for (int i = 0; i < furniture.Length; i++)
            {
                var furni = new HWallItem(packet);
                furni.OwnerName = owners[furni.OwnerId];

                furniture[i] = furni;
            }
            return furniture;
        }
    }
}
