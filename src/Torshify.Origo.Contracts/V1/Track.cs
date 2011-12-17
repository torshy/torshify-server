using System.Diagnostics;
using System.Runtime.Serialization;

namespace Torshify.Origo.Contracts.V1
{
    [DebuggerDisplay("{Name}")]
    [DataContract(
        Name = "Track",
        Namespace = "http://schemas.torshify/v1")]
    public class Track : IExtensibleDataObject
    {
        [DataMember(Name = "Name", Order = 0)]
        public string Name { get; set; }
        [DataMember(Name = "Album", Order = 1)]
        public Album Album { get; set; }
        [DataMember(Name = "Duration", Order = 2)]
        public double Duration { get; set; }
        [DataMember(Name = "Index", Order = 3)]
        public int Index { get; set; }
        [DataMember(Name = "Disc", Order = 4)]
        public int Disc { get; set; }
        [DataMember(Name = "Popularity", Order = 5)]
        public int Popularity { get; set; }
        [DataMember(Name = "IsStarred", Order = 6)]
        public bool IsStarred { get; set; }
        [DataMember(Name = "OfflineStatus", Order = 7)]
        public string OfflineStatus { get; set; }
        [DataMember(Name = "IsLocal", Order = 8)]
        public bool IsLocal { get; set; }
        [DataMember(Name = "Artists", Order = 9)]
        public Artist[] Artists { get; set; }
        [DataMember(Name = "ID", Order = 10)]
        public string ID { get; set; }
        [DataMember(Name = "IsAvailable", Order = 11)]
        public bool IsAvailable { get; set; }
        public ExtensionDataObject ExtensionData { get; set; }
    }
}