using System.Runtime.Serialization;

namespace Torshify.Origo.Contracts.V1.Query
{
    [DataContract(
        Name = "AlbumBrowseResult",
        Namespace = "http://schemas.torshify/v1")]
    public class AlbumBrowseResult : IExtensibleDataObject
    {
        [DataMember(Name = "Album", Order = 0)]
        public Album Album { get; set; }
        [DataMember(Name = "Artist", Order = 1)]
        public Artist Artist { get; set; }
        [DataMember(Name = "Copyrights", Order = 2)]
        public string[] Copyrights { get; set; }
        [DataMember(Name = "Review", Order = 3)]
        public string Review { get; set; }
        [DataMember(Name = "Tracks", Order = 4)]
        public Track[] Tracks { get; set; }
        [DataMember(Name = "BackendRequestDuration", Order = 5)]
        public double BackendRequestDuration { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }
}