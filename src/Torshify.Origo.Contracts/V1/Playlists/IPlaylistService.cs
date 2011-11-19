using System.ServiceModel;

namespace Torshify.Origo.Contracts.V1.Playlists
{
    [ServiceContract(
        Name = "PlaylistService",
        Namespace = "http://schemas.torshify/v1",
        CallbackContract = typeof(IPlaylistCallback))]
    public interface IPlaylistService
    {
        [OperationContract]
        void Subscribe();
        [OperationContract]
        void Unsubscribe();
        [OperationContract]
        Playlist[] GetPlaylists();
    }
}