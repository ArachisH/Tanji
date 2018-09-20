using Sulakore.Protocol;

namespace Sulakore.Habbo
{
    public class HFriendData
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public HGender Gender { get; set; }
        public bool IsOnline { get; set; }
        public bool CanFollow { get; set; }
        public string Figure { get; set; }

        public int CategoryId { get; set; }

        public string Motto { get; set; }
        public string RealName { get; set; }

        public bool IsPersisted { get; set; }
        public bool IsPocketHabboUser { get; set; }
        public HRelationship RelationshipStatus { get; set; }

        public HFriendData(HMessage packet)
        {
            Id = packet.ReadInteger();
            Username = packet.ReadString();
            Gender = (packet.ReadInteger() == 1 ? HGender.Male : HGender.Female);

            IsOnline = packet.ReadBoolean();
            CanFollow = packet.ReadBoolean();
            Figure = packet.ReadString();
            CategoryId = packet.ReadInteger();
            Motto = packet.ReadString();
            RealName = packet.ReadString();
            packet.ReadString();

            IsPersisted = packet.ReadBoolean();
            packet.ReadBoolean();
            IsPocketHabboUser = packet.ReadBoolean();
            RelationshipStatus = (HRelationship)packet.ReadShort();
        }

        public void Update(HFriendData friend)
        {
            IsOnline = friend.IsOnline;
            Figure = friend.Figure;
            Motto = friend.Motto;
            RelationshipStatus = friend.RelationshipStatus;
        }

        public static HFriendData[] Parse(HMessage packet)
        {
            int removedFriends = packet.ReadInteger();
            int addedFriends = packet.ReadInteger();

            var friends = new HFriendData[packet.ReadInteger()];
            for (int i = 0; i < friends.Length; i++)
            {
                friends[i] = new HFriendData(packet);
            }
            return friends;
        }
    }
}
