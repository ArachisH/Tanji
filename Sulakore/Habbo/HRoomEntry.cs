using Sulakore.Protocol;

namespace Sulakore.Habbo
{
    public class HRoomEntry
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int OwnerId { get; set; }
        public string OwnerName { get; set; }

        public int DoorMode { get; set; }
        public int UserCount { get; set; }
        public int MaxUserCount { get; set; }

        public string Description { get; set; }
        public int TradeMode { get; set; }
        public int Ranking { get; set; }
        public int Category { get; set; }
        public int Stars { get; set; }

        public string[] Tags { get; set; }
        
        public string ThumbnailUrl { get; set; }
        
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string GroupBadgeCode { get; set; }
        
        public string AdName { get; set; }
        public string AdDescription { get; set; }
        public int AdExpiresInMinutes { get; set; }

        public bool ShowOwner { get; set; }
        public bool AllowPets { get; set; }
        public bool ShowEntryAd { get; set; }

        public HRoomEntry(HMessage packet)
        {
            Id = packet.ReadInteger();
            Name = packet.ReadString();

            OwnerId = packet.ReadInteger();
            OwnerName = packet.ReadString();

            DoorMode = packet.ReadInteger();
            UserCount = packet.ReadInteger();
            MaxUserCount = packet.ReadInteger();

            Description = packet.ReadString();
            TradeMode = packet.ReadInteger();
            Ranking = packet.ReadInteger();
            Category = packet.ReadInteger();
            Stars = packet.ReadInteger();

            Tags = new string[packet.ReadInteger()];
            for (int i = 0; i < Tags.Length; i++)
            {
                Tags[i] = packet.ReadString();
            }

            var roomEntryBitmask = (HRoomFlags)packet.ReadInteger();

            if (roomEntryBitmask.HasFlag(HRoomFlags.HasCustomThumbnail))
            {
                ThumbnailUrl = packet.ReadString();
            }
            if (roomEntryBitmask.HasFlag(HRoomFlags.HasGroup))
            {
                GroupId = packet.ReadInteger();
                GroupName = packet.ReadString();
                GroupBadgeCode = packet.ReadString();
            }
            if (roomEntryBitmask.HasFlag(HRoomFlags.HasAdvertisement))
            {
                AdName = packet.ReadString();
                AdDescription = packet.ReadString();
                AdExpiresInMinutes = packet.ReadInteger();
            }

            ShowOwner = roomEntryBitmask.HasFlag(HRoomFlags.ShowOwner);
            AllowPets = roomEntryBitmask.HasFlag(HRoomFlags.AllowPets);
            ShowEntryAd = roomEntryBitmask.HasFlag(HRoomFlags.ShowRoomAd);
        }
    }
}
