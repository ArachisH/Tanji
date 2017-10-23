using System.Diagnostics;
using System.Runtime.Serialization;

namespace Sulakore.Habbo.Web
{
    [DataContract]
    [DebuggerDisplay("Name: {Name}")]
    public class HGroup
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "roomId")]
        public string RoomId { get; set; }

        [DataMember(Name = "badgeCode")]
        public string BadgeCode { get; set; }

        [DataMember(Name = "primaryColour")]
        public string PrimaryColor { get; set; }

        [DataMember(Name = "secondaryColour")]
        public string SecondaryColor { get; set; }

        [DataMember(Name = "isAdmin")]
        public bool IsAdmin { get; set; }
    }
}