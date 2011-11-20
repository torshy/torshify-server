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
        PlayerStatus GetStatus();
        [OperationContract]
        void TogglePause();
        [OperationContract]
        void SetVolume(float volume);
        [OperationContract]
        float GetVolume();
        [OperationContract]
        void Seek(double milliseconds);
        [OperationContract]
        void Subscribe();
        [OperationContract]
        void Unsubscribe();
    }
}