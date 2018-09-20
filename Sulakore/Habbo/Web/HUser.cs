using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Sulakore.Habbo.Web
{
    [DataContract]
    [DebuggerDisplay("Name: {Name}")]
    public class HUser
    {
        private static readonly DataContractJsonSerializer _serializer;

        [DataMember(Name = "uniqueId", EmitDefaultValue = false, Order = 1)]
        public string UniqueId { get; set; }

        [DataMember(Name = "name", EmitDefaultValue = false, Order = 2)]
        public string Name { get; set; }

        [DataMember(Name = "figureString", EmitDefaultValue = false, Order = 2)]
        public string FigureId { get; set; }

        [DataMember(Name = "motto", EmitDefaultValue = false, Order = 3)]
        public string Motto { get; set; }

        [DataMember(Name = "buildersClubMember", EmitDefaultValue = false, Order = 4)]
        public bool? IsBuildersClubMember { get; set; }

        [DataMember(Name = "habboClubMember", EmitDefaultValue = false, Order = 5)]
        public bool? IsHabboClubMember { get; set; }

        [DataMember(Name = "lastWebAccess", EmitDefaultValue = false, Order = 6)]
        public DateTime? LastWebAccess { get; set; }

        [DataMember(Name = "creationTime", EmitDefaultValue = false, Order = 7)]
        public DateTime? CreationTime { get; set; }

        [DataMember(Name = "memberSince", EmitDefaultValue = false, Order = 8)]
        public DateTime? MemberSince { get; set; }

        [DataMember(Name = "sessionLogId", EmitDefaultValue = false, Order = 9)]
        public long SessionLogId { get; set; }

        [DataMember(Name = "loginLogId", EmitDefaultValue = false, Order = 10)]
        public long LoginLogId { get; set; }

        [DataMember(Name = "email", EmitDefaultValue = false, Order = 11)]
        public string Email { get; set; }

        [DataMember(Name = "identityId", EmitDefaultValue = false, Order = 12)]
        public long IdentityId { get; set; }

        [DataMember(Name = "emailVerified", EmitDefaultValue = false, Order = 13)]
        public bool? IsEmailVerified { get; set; }

        [DataMember(Name = "identityVerified", EmitDefaultValue = false, Order = 14)]
        public bool? IsIdentityVerified { get; set; }

        [DataMember(Name = "identityType", EmitDefaultValue = false, Order = 15)]
        public string IdentityType { get; set; }

        [DataMember(Name = "trusted", EmitDefaultValue = false, Order = 16)]
        public bool? IsTrusted { get; set; }

        [DataMember(Name = "force", EmitDefaultValue = false, Order = 17)]
        public string[] Force { get; set; }

        [DataMember(Name = "accountId", EmitDefaultValue = false, Order = 18)]
        public int AccountId { get; set; }

        [DataMember(Name = "country", EmitDefaultValue = false, Order = 19)]
        public string Country { get; set; }

        [DataMember(Name = "traits", EmitDefaultValue = false, Order = 20)]
        public string[] Traits { get; set; }

        [DataMember(Name = "partner", EmitDefaultValue = false, Order = 21)]
        public string Partner { get; set; }

        [DataMember(Name = "profileVisible", EmitDefaultValue = false, Order = 22)]
        public bool? IsProfileVisible { get; set; }

        [DataMember(Name = "selectedBadges", EmitDefaultValue = false, Order = 23)]
        public HBadge[] SelectedBadges { get; set; }

        [DataMember(Name = "banned", EmitDefaultValue = false, Order = 24)]
        public bool? IsBanned { get; set; }

        static HUser()
        {
            _serializer = new DataContractJsonSerializer(typeof(HUser), new DataContractJsonSerializerSettings
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

        public static HUser Create(string userJson)
        {
            using (var jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(userJson)))
            {
                return (HUser)_serializer.ReadObject(jsonStream);
            }
        }
    }
}