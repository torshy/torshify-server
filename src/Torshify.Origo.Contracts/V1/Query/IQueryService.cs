using System;
using System.ServiceModel;

namespace Torshify.Origo.Contracts.V1.Query
{
    [ServiceContract(
        Name = "QueryService",
        Namespace = "http://schemas.torshify/v1")]
    public interface IQueryService
    {
        [OperationContract]
        [FaultContract(typeof(NotLoggedInFault))]
        QueryResult Query(
            string query, 
            int trackOffset,
            int trackCount,
            int albumOffset,
            int albumCount,
            int artistOffset,
            int artistCount);

        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginQuery(
            string query, 
            int trackOffset,
            int trackCount,
            int albumOffset,
            int albumCount,
            int artistOffset,
            int artistCount, 
            AsyncCallback callback, 
            object state);
        QueryResult EndQuery(IAsyncResult result);

        [OperationContract]
        [FaultContract(typeof(NotLoggedInFault))]
        AlbumBrowseResult AlbumBrowse(string albumId);

        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginAlbumBrowse(string albumId, AsyncCallback callback, object state);
        AlbumBrowseResult EndAlbumBrowse(IAsyncResult result);

        [OperationContract]
        [FaultContract(typeof(NotLoggedInFault))]
        ArtistBrowseResult ArtistBrowse(string artistId, ArtistBrowsingType type);

        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginArtistBrowse(string artistId, ArtistBrowsingType type, AsyncCallback callback, object state);
        ArtistBrowseResult EndArtistBrowse(IAsyncResult result);

        [OperationContract]
        [FaultContract(typeof(NotLoggedInFault))]
        Playlist GetPlaylist(string link);
    }
}