using System.ServiceModel;

namespace Torshify.Origo.Contracts.V1.Player
{
    [ServiceContract(
        Name = "PlayerControlService",
        Namespace = "http://schemas.torshify/v1",
        CallbackContract = typeof(IPlayerCallback))]
    public interface IPlayerControlService
    {
        [OperationContract]
        [FaultContract(typeof(NotLoggedInFault))]
        PlayerStatus GetStatus();

        [OperationContract]
        [FaultContract(typeof(NotLoggedInFault))]
        void TogglePause();

        [OperationContract]
        [FaultContract(typeof(NotLoggedInFault))]
        void SetVolume(float volume);

        [OperationContract]
        [FaultContract(typeof(NotLoggedInFault))]
        float GetVolume();

        [OperationContract]
        [FaultContract(typeof(NotLoggedInFault))]
        void Seek(double milliseconds);

        [OperationContract]
        [FaultContract(typeof(NotLoggedInFault))]
        void Subscribe();

        [OperationContract]
        [FaultContract(typeof(NotLoggedInFault))]
        void Unsubscribe();
    }
}