using System.Diagnostics;
using System.Runtime.Serialization;

namespace Torshify.Origo.Contracts.V1
{
    [DebuggerDisplay("{Name}")]
    [DataContract(
        Name = "Artist",
        Namespace = "http://schemas.torshify/v1")]
    public class Artist : IExtensibleDataObject
    {
        [DataMember(Name = "Name", Order = 0)]
        public string Name { get; set; }
        [DataMember(Name = "PortraitID", Order = 1)]
        public string PortraitID { get; set; }
        [DataMember(Name = "ID", Order = 2)]
        public string ID { get; set; }
        public ExtensionDataObject ExtensionData { get; set; }
    }
}