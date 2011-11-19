using System.Runtime.Serialization;

namespace Torshify.Origo.Contracts.V1.Query
{
    [DataContract(
        Name = "ArtistBrowseResult",
        Namespace = "http://schemas.torshify/v1")]
    public class ArtistBrowseResult : IExtensibleDataObject
    {
        [DataMember(Name = "Artist", Order = 0)]
        public Artist Artist { get; set; }
        [DataMember(Name = "Albums", Order = 1)]
        public Album[] Albums { get; set; }
        [DataMember(Name = "BackendRequestDuration", Order = 2)]
        public double BackendRequestDuration { get; set; }
        [DataMember(Name = "Biography", Order = 3)]
        public string Biography { get; set; }
        [DataMember(Name = "Tracks", Order = 4)]
        public Track[] Tracks { get; set; }
        [DataMember(Name = "Portraits", Order = 5)]
        public string[] Portraits { get; set; }
        [DataMember(Name = "SimilarArtists", Order = 6)]
        public Artist[] SimilarArtists { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }
}