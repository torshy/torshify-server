using System.ServiceModel;

namespace Torshify.Origo.Contracts.V1.Player
{
    public interface IPlayerCallback
    {
        [OperationContract(IsOneWay = true)]
        void OnTrackChanged(Track track);

        [OperationContract(IsOneWay = true)]
        void OnTrackComplete(Track track);

        [OperationContract(IsOneWay = true)]
        void OnElapsed(double elapsedMs, double totalMs);

        [OperationContract(IsOneWay = true)]
        void OnPlayStateChanged(bool isPlaying);

        [OperationContract(IsOneWay = true)]
        void OnVolumeChanged(float volume);
    }
}