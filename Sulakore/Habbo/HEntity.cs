using System;
using System.Globalization;

using Sulakore.Protocol;

namespace Sulakore.Habbo
{
    public class HEntity : HData
    {
        public int Id { get; set; }
        public int Index { get; set; }
        public HPoint Tile { get; set; }
        public string Name { get; set; }
        public string Motto { get; set; }
        public HGender Gender { get; set; }
        public int EntityType { get; set; }
        public string FigureId { get; set; }
        public string FavoriteGroup { get; set; }
        public HEntityUpdate LastUpdate { get; private set; }

        public HEntity(HMessage packet)
        {
            Id = packet.ReadInteger();
            Name = packet.ReadString();
            Motto = packet.ReadString();
            FigureId = packet.ReadString();
            Index = packet.ReadInteger();

            Tile = new HPoint(packet.ReadInteger(), packet.ReadInteger(),
                double.Parse(packet.ReadString(), CultureInfo.InvariantCulture));

            packet.ReadInteger();
            EntityType = packet.ReadInteger();

            switch (EntityType)
            {
                case 1:
                {
                    Gender = (HGender)packet.ReadString().ToLower()[0];
                    packet.ReadInteger();
                    packet.ReadInteger();
                    FavoriteGroup = packet.ReadString();
                    packet.ReadString();
                    packet.ReadInteger();
                    packet.ReadBoolean();
                    break;
                }
                case 2:
                {
                    packet.ReadInteger();
                    packet.ReadInteger();
                    packet.ReadString();
                    packet.ReadInteger();
                    packet.ReadBoolean();
                    packet.ReadBoolean();
                    packet.ReadBoolean();
                    packet.ReadBoolean();
                    packet.ReadBoolean();
                    packet.ReadBoolean();
                    packet.ReadInteger();
                    packet.ReadString();
                    break;
                }
                case 4:
                {
                    packet.ReadString();
                    packet.ReadInteger();
                    packet.ReadString();
                    for (int j = packet.ReadInteger(); j > 0; j--)
                    {
                        packet.ReadShort();
                    }
                    break;
                }
            }
        }

        public void Update(HEntityUpdate update)
        {
            if (!TryUpdate(update))
            {
                throw new ArgumentException("Entity index does not match.", nameof(update));
            }
        }
        public bool TryUpdate(HEntityUpdate update)
        {
            if (Index != update.Index) return false;

            Tile = update.Tile;
            LastUpdate = update;
            return true;
        }
        
        public static HEntity[] Parse(HMessage packet)
        {
            var entities = new HEntity[packet.ReadInteger()];
            for (int i = 0; i < entities.Length; i++)
            {
                entities[i] = new HEntity(packet);
            }
            return entities;
        }
    }
}