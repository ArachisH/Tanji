using Sulakore.Protocol;

namespace Sulakore.Habbo
{
    public class HFriendRequests
    {
        public int Unk { get; set; }
        public int Requests { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Motto { get; set; }

        public HFriendRequests(HMessage packet)
        {
            Unk = packet.ReadInteger();

            Requests = packet.ReadInteger();
            for (int i = 0; i < Requests; i++)
            {
                Id = packet.ReadInteger();
                Name = packet.ReadString();
                Motto = packet.ReadString();
            }
        }
    }
}
