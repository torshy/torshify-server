using System.ServiceModel;

namespace Torshify.Origo.Contracts.V1.Player
{
    [ServiceContract(
        Name = "PlaylistPlayerService",
        Namespace = "http://schemas.torshify/v1")]
    public interface IPlaylistPlayerService
    {
        [OperationContract]
        void Initialize(string[] links);
        [OperationContract]
        void Enqueue(string[] links);
        [OperationContract]
        void Next();
        [OperationContract]
        void Previous();
        [OperationContract]
        PlaylistTrack GetCurrent();
        [OperationContract]
        PlaylistTrack[] GetPlaylist();
        [OperationContract]
        void SetShuffle(bool shuffle);
        [OperationContract]
        void SetRepeat(bool repeat);
        [OperationContract]
        bool GetRepeat();
        [OperationContract]
        bool GetShuffle();
    }
}