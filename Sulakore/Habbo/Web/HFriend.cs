using System.Diagnostics;
using System.Runtime.Serialization;

namespace Sulakore.Habbo.Web
{
    [DataContract]
    [DebuggerDisplay("Name: {Name}")]
    public class HFriend
    {
        [DataMember(Name = "name", IsRequired = true, Order = 1)]
        public string Name { get; set; }

        [DataMember(Name = "motto", IsRequired = true, Order = 2)]
        public string Motto { get; set; }

        [DataMember(Name = "uniqueId", IsRequired = true, Order = 3)]
        public string UniqueId { get; set; }

        [DataMember(Name = "figureString", IsRequired = true, Order = 4)]
        public string FigureId { get; set; }
    }
}