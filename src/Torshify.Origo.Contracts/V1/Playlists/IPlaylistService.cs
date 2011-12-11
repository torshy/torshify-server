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
        [FaultContract(typeof(NotLoggedInFault))]
        void Subscribe();

        [OperationContract]
        [FaultContract(typeof(NotLoggedInFault))]
        void Unsubscribe();

        [OperationContract]
        [FaultContract(typeof(NotLoggedInFault))]
        Playlist[] GetPlaylists();
    }
}