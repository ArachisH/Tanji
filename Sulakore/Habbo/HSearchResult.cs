using Sulakore.Protocol;

namespace Sulakore.Habbo
{
    public class HSearchResult : HData
    {
        public string SearchCode { get; set; }
        public string Text { get; set; }

        public int ActionAllowed { get; set; }
        public bool ForceClosed { get; set; }
        public int ViewMode { get; set; }

        public HRoomEntry[] Rooms { get; set; }

        public HSearchResult(HMessage packet)
        {
            SearchCode = packet.ReadString();
            Text = packet.ReadString();

            ActionAllowed = packet.ReadInteger();
            ForceClosed = packet.ReadBoolean();
            ViewMode = packet.ReadInteger();

            Rooms = new HRoomEntry[packet.ReadInteger()];
            for (int i = 0; i < Rooms.Length; i++)
            {
                Rooms[i] = new HRoomEntry(packet);
            }
        }

        public static HSearchResult[] Parse(HMessage packet)
        {
            string searchCode = packet.ReadString();
            string filter = packet.ReadString();

            var results = new HSearchResult[packet.ReadInteger()];
            for (int i = 0; i < results.Length; i++)
            {
                results[i] = new HSearchResult(packet);
            }
            return results;
        }
    }
}
