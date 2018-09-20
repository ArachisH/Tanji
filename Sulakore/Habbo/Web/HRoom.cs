using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Sulakore.Habbo.Web
{
    [DataContract]
    [DebuggerDisplay("Name: {Name}")]
    public class HRoom
    {
        [DataMember(Name = "id", IsRequired = true, Order = 1)]
        public int Id { get; set; }

        [DataMember(Name = "name", IsRequired = true, Order = 2)]
        public string Name { get; set; }

        [DataMember(Name = "description", IsRequired = true, Order = 3)]
        public string Description { get; set; }

        [DataMember(Name = "creationTime", IsRequired = true, Order = 4)]
        public DateTime CreationTime { get; set; }

        [DataMember(Name = "habboGroupId", IsRequired = true, Order = 5)]
        public string GroupId { get; set; }

        [DataMember(Name = "tags", IsRequired = true, Order = 6)]
        public string[] Tags { get; set; }

        [DataMember(Name = "maximumVisitors", IsRequired = true, Order = 7)]
        public int MaximumVisitors { get; set; }

        [DataMember(Name = "showOwnerName", IsRequired = true, Order = 8)]
        public bool IsShowingOwnerName { get; set; }

        [DataMember(Name = "ownerName", IsRequired = true, Order = 9)]
        public string OwnerName { get; set; }

        [DataMember(Name = "ownerUniqueId", IsRequired = true, Order = 10)]
        public string OwnerUniqueId { get; set; }

        [DataMember(Name = "categories", IsRequired = true, Order = 11)]
        public string[] Categories { get; set; }

        [DataMember(Name = "rating", IsRequired = true, Order = 12)]
        public int Rating { get; set; }

        [DataMember(Name = "thumbnailUrl", IsRequired = true, Order = 13)]
        public string ThumbnailUrl { get; set; }

        [DataMember(Name = "imageUrl", IsRequired = true, Order = 14)]
        public string RoomPhotoUrl { get; set; }

        [DataMember(Name = "uniqueId", IsRequired = true, Order = 15)]
        public string UniqueId { get; set; }
    }
}