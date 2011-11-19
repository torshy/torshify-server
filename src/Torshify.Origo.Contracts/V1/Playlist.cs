using System.Diagnostics;
using System.Runtime.Serialization;

namespace Torshify.Origo.Contracts.V1
{
    [DebuggerDisplay("{Name}")]
    [DataContract(
        Name = "Playlist",
        Namespace = "http://schemas.torshify/v1")]
    public class Playlist : IExtensibleDataObject
    {
        [DataMember(Name = "ID", Order = 0)]
        public string ID { get; set; }
        [DataMember(Name = "Name", Order = 1)]
        public string Name { get; set; }
        [DataMember(Name = "IsCollaborative", Order = 2)]
        public bool IsCollaborative { get; set; }
        [DataMember(Name = "OfflineStatus", Order = 3)]
        public string OfflineStatus { get; set; }
        [DataMember(Name = "PendingChanges", Order = 4)]
        public bool PendingChanges { get; set; }
        [DataMember(Name = "ImageID", Order = 5)]
        public string ImageID { get; set; }
        [DataMember(Name = "Subscribers", Order = 6)]
        public string[] Subscribers { get; set; }
        [DataMember(Name = "Tracks", Order = 7)]
        public Track[] Tracks { get; set; }
        [DataMember(Name = "Description", Order = 8)]
        public string Description { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }
}