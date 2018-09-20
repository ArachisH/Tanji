using System;

namespace Sulakore.Habbo
{
    [Flags]
    public enum HRoomFlags
    {
        None = 0,
        HasCustomThumbnail = 1,
        HasGroup = 2,
        HasAdvertisement = 4,
        ShowOwner = 8,
        AllowPets = 16,
        ShowRoomAd = 32
    }
}
