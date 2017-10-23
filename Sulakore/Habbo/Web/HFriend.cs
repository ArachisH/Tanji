using System.Diagnostics;
using System.Runtime.Serialization;

namespace Sulakore.Habbo.Web
{
    [DataContract]
    [DebuggerDisplay("Name: {Name}")]
    public class HFriend
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "motto")]
        public string Motto { get; set; }

        [DataMember(Name = "uniqueId")]
        public string UniqueId { get; set; }

        [DataMember(Name = "figureString")]
        public string FigureId { get; set; }
    }
}