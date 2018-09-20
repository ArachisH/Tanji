using Sulakore.Protocol;

namespace Sulakore.Habbo
{
    public class HUserProfile
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Motto { get; set; }
        public string Figure { get; set; }
        public string CreationDate { get; set; }
        public int AchievementScore { get; set; }
        public int FriendCount { get; set; }

        public bool IsFriend { get; set; }
        public bool IsFriendRequestSent { get; set; }
        public bool IsOnline { get; set; }

        public HGroupEntry[] Groups { get; set; }

        public int LastAccessSinceInSeconds { get; set; }
        public bool OpenProfileView { get; set; }

        public HUserProfile(HMessage packet)
        {
            Id = packet.ReadInteger();
            Username = packet.ReadString();
            Motto = packet.ReadString();
            Figure = packet.ReadString();
            CreationDate = packet.ReadString();
            AchievementScore = packet.ReadInteger();
            FriendCount = packet.ReadInteger();

            IsFriend = packet.ReadBoolean();
            IsFriendRequestSent = packet.ReadBoolean();
            IsOnline = packet.ReadBoolean();

            Groups = new HGroupEntry[packet.ReadInteger()];
            for (int i = 0; i < Groups.Length; i++)
            {
                Groups[i] = new HGroupEntry(packet);
            }
            LastAccessSinceInSeconds = packet.ReadInteger();
            OpenProfileView = packet.ReadBoolean();
        }
    }
}
