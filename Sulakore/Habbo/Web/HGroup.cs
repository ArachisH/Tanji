using System.Diagnostics;
using System.Runtime.Serialization;

namespace Sulakore.Habbo.Web
{
    [DataContract]
    [DebuggerDisplay("Name: {Name}")]
    public class HGroup
    {
        [DataMember(Name = "id", IsRequired = true, Order = 1)]
        public string Id { get; set; }

        [DataMember(Name = "name", IsRequired = true, Order = 2)]
        public string Name { get; set; }

        [DataMember(Name = "description", IsRequired = true, Order = 3)]
        public string Description { get; set; }

        [DataMember(Name = "type", IsRequired = true, Order = 4)]
        public string Type { get; set; }

        [DataMember(Name = "roomId", IsRequired = true, Order = 5)]
        public string RoomId { get; set; }

        [DataMember(Name = "badgeCode", IsRequired = true, Order = 6)]
        public string BadgeCode { get; set; }

        [DataMember(Name = "primaryColour", IsRequired = true, Order = 7)]
        public string PrimaryColor { get; set; }

        [DataMember(Name = "secondaryColour", IsRequired = true, Order = 8)]
        public string SecondaryColor { get; set; }

        [DataMember(Name = "isAdmin", IsRequired = true, Order = 9)]
        public bool IsAdmin { get; set; }
    }
}