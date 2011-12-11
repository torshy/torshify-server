using System.ServiceModel;

namespace Torshify.Origo.Contracts.V1.Player
{
    [ServiceContract(
        Name = "TrackPlayerService",
        Namespace = "http://schemas.torshify/v1")]
    public interface ITrackPlayerService
    {
        [OperationContract]
        [FaultContract(typeof(NotLoggedInFault))]
        void Play(string trackId);
    }
}