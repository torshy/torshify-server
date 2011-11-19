using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Torshify.Origo.Contracts.V1.Query
{
    [DataContract(
        Name = "QueryResult",
        Namespace = "http://schemas.torshify/v1")]
    public class QueryResult : IExtensibleDataObject
    {
        [DataMember(Name = "Albums", Order = 0)]
        public IEnumerable<Album> Albums { get; set; }
        [DataMember(Name = "Artists", Order = 1)]
        public IEnumerable<Artist> Artists { get; set; }
        [DataMember(Name = "Tracks", Order = 2)]
        public IEnumerable<Track> Tracks { get; set; }
        [DataMember(Name = "TotalAlbums", Order = 3)]
        public int TotalAlbums { get; set; }
        [DataMember(Name = "TotalArtists", Order = 4)]
        public int TotalArtists { get; set; }
        [DataMember(Name = "TotalTracks", Order = 5)]
        public int TotalTracks { get; set; }
        [DataMember(Name = "Query", Order = 6)]
        public string Query { get; set; }
        [DataMember(Name = "DidYouMean", Order = 7)]
        public string DidYouMean { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }
}