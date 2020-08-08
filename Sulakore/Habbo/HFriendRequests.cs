using Sulakore.Protocol;

namespace Sulakore.Habbo
{
    public class HFriendRequests
    {
        public int Count01 { get; set; }
        public int Count02 { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Motto { get; set; }

        public HFriendRequests(HMessage packet)
        {
            Count01 = packet.ReadInteger();
            Count02 = packet.ReadInteger();
            Id = packet.ReadInteger();
            Name = packet.ReadString();
            Motto = packet.ReadString();
        }
    }
}
