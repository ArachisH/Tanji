using Sulakore.Protocol;

namespace Sulakore.Habbo
{
    public class HGroupEntry
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BadgeCode { get; set; }
        public string PrimaryColor { get; set; }
        public string SecondaryColor { get; set; }

        public bool Favorite { get; set; }
        public int OwnerId { get; set; }
        public bool HasForum { get; set; }

        public HGroupEntry(HMessage packet)
        {
            Id = packet.ReadInteger();
            Name = packet.ReadString();
            BadgeCode = packet.ReadString();
            PrimaryColor = packet.ReadString();
            SecondaryColor = packet.ReadString();

            Favorite = packet.ReadBoolean();
            OwnerId = packet.ReadInteger();
            HasForum = packet.ReadBoolean();
        }
    }
}
