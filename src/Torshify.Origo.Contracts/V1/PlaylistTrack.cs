using System.Runtime.Serialization;

namespace Torshify.Origo.Contracts.V1
{
    [DataContract(
        Name = "PlaylistTrack",
        Namespace = "http://schemas.torshify/v1")]
    public class PlaylistTrack : IExtensibleDataObject
    {
        [DataMember(Name = "IsQueued", Order = 0)]
        public bool IsQueued { get; set; }
        [DataMember(Name = "Track", Order = 1)]
        public Track Track { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }
}