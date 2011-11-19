using System.ServiceModel;

namespace Torshify.Origo.Contracts.V1.Player
{
    [ServiceContract(
        Name = "PlaylistPlayerService",
        Namespace = "http://schemas.torshify/v1")]
    public interface IPlaylistPlayerService
    {
        [OperationContract]
        void Initialize(string link);
        [OperationContract]
        void Enqueue(string link);
        [OperationContract]
        void Next();
        [OperationContract]
        void Previous();
        [OperationContract]
        PlaylistTrack GetCurrent();
        [OperationContract]
        PlaylistTrack[] GetPlaylist();
    }
}