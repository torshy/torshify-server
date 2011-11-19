using System.Diagnostics;
using System.Runtime.Serialization;

namespace Torshify.Origo.Contracts.V1
{
    [DebuggerDisplay("{Name}")]
    [DataContract(
        Name = "Album",
        Namespace = "http://schemas.torshify/v1")]
    public class Album : IExtensibleDataObject 
    {
        [DataMember(Name = "ID", Order = 0)]
        public string ID { get; set; }
        [DataMember(Name = "CoverID", Order = 1)]
        public string CoverID { get; set; }
        [DataMember(Name = "IsAvailable", Order = 2)]
        public bool IsAvailable { get; set; }
        [DataMember(Name = "Name", Order = 3)]
        public string Name { get; set; }
        [DataMember(Name = "Type", Order = 4)]
        public string Type { get; set; }
        [DataMember(Name = "Year", Order = 5)]
        public int Year { get; set; }
        [DataMember(Name = "Artist", Order = 6)]
        public Artist Artist { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }
}