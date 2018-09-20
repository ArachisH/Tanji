using System.Diagnostics;
using System.Runtime.Serialization;

namespace Sulakore.Habbo.Web
{
    [DataContract]
    [DebuggerDisplay("Name: {Name}")]
    public class HBadge
    {
        [DataMember(Name = "badgeIndex", EmitDefaultValue = false, Order = 1)]
        public int Index { get; set; }

        [DataMember(Name = "code", IsRequired = true, Order = 2)]
        public string Code { get; set; }

        [DataMember(Name = "name", IsRequired = true, Order = 3)]
        public string Name { get; set; }

        [DataMember(Name = "description", IsRequired = true, Order = 4)]
        public string Description { get; set; }
    }
}