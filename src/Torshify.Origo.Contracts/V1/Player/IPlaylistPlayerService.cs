using System.ServiceModel;

namespace Torshify.Origo.Contracts.V1.Player
{
    [ServiceContract(
        Name = "PlaylistPlayerService",
        Namespace = "http://schemas.torshify/v1")]
    public interface IPlaylistPlayerService
    {
        [OperationContract]
        [FaultContract(typeof(NotLoggedInFault))]
        void Initialize(string[] links);
        
        [OperationContract]
        [FaultContract(typeof(NotLoggedInFault))]
        void Enqueue(string[] links);
        
        [OperationContract]
        [FaultContract(typeof(NotLoggedInFault))]
        void Next();
        
        [OperationContract]
        [FaultContract(typeof(NotLoggedInFault))]
        void Previous();
        
        [OperationContract]
        [FaultContract(typeof(NotLoggedInFault))]
        PlaylistTrack GetCurrent();
        
        [OperationContract]
        [FaultContract(typeof(NotLoggedInFault))]
        PlaylistTrack[] GetPlaylist();
        
        [OperationContract]
        [FaultContract(typeof(NotLoggedInFault))]
        void SetShuffle(bool shuffle);
        
        [OperationContract]
        [FaultContract(typeof(NotLoggedInFault))]
        void SetRepeat(bool repeat);
        
        [OperationContract]
        [FaultContract(typeof(NotLoggedInFault))]
        bool GetRepeat();
        
        [OperationContract]
        [FaultContract(typeof(NotLoggedInFault))]
        bool GetShuffle();
    }
}