using System.IO;
using System.Text;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Sulakore.Habbo.Web
{
    [DataContract]
    [DebuggerDisplay("Name: {User?.Name}")]
    public class HProfile
    {
        private static readonly DataContractJsonSerializer _serializer;

        [DataMember(Name = "user", IsRequired = true, Order = 1)]
        public HUser User { get; set; }

        [DataMember(Name = "groups", IsRequired = true, Order = 2)]
        public HGroup[] Groups { get; set; }

        [DataMember(Name = "badges", IsRequired = true, Order = 3)]
        public HBadge[] Badges { get; set; }

        [DataMember(Name = "friends", IsRequired = true, Order = 4)]
        public HFriend[] Friends { get; set; }

        [DataMember(Name = "rooms", IsRequired = true, Order = 5)]
        public HRoom[] Rooms { get; set; }

        static HProfile()
        {
            _serializer = new DataContractJsonSerializer(typeof(HProfile), new DataContractJsonSerializerSettings
            {
                DateTimeFormat = new DateTimeFormat("yyyy-MM-dd'T'HH:mm:ss.fff+0000")
            });
        }

        public override string ToString()
        {
            using (var jsonStream = new MemoryStream())
            {
                _serializer.WriteObject(jsonStream, this);
                return Encoding.UTF8.GetString(jsonStream.ToArray());
            }
        }

        public static HProfile Create(string profileJson)
        {
            using (var jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(profileJson)))
            {
                return (HProfile)_serializer.ReadObject(jsonStream);
            }
        }
    }
}