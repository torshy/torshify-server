using System.ServiceModel;

using Microsoft.Practices.ServiceLocation;
using Torshify.Origo.Contracts.V1;
using Torshify.Origo.Contracts.V1.Player;
using Torshify.Origo.Extensions;
using Torshify.Origo.Interfaces;

namespace Torshify.Origo.Services.V1.Player
{
    [ServiceBehavior(
        UseSynchronizationContext = false)]
    public class PlayerControlService : IPlayerControlService
    {
        #region Methods

        public PlayerStatus GetStatus()
        {
            var musicPlayerController = ServiceLocator.Current.Resolve<IMusicPlayerController>();

            PlayerStatus status = new PlayerStatus();
            status.CurrentTrack = musicPlayerController.CurrentTrack;
            status.IsPlaying = musicPlayerController.IsPlaying;
            status.TotalTime = musicPlayerController.CurrentTrack != null ? musicPlayerController.CurrentTrack.Duration : 0;
            status.ElapsedTime = musicPlayerController.Elapsed.TotalMilliseconds;
            status.Volume = musicPlayerController.Volume;
            return status;
        }

        public void TogglePause()
        {
            var musicPlayerController = ServiceLocator.Current.Resolve<IMusicPlayerController>();
            musicPlayerController.TogglePause();
        }

        public void Subscribe()
        {
            var callbackHandler = ServiceLocator.Current.Resolve<PlayerCallbackHandler>();
            callbackHandler.Register(OperationContext.Current.GetCallbackChannel<IPlayerCallback>());
        }

        public void Unsubscribe()
        {
            var callbackHandler = ServiceLocator.Current.Resolve<PlayerCallbackHandler>();
            callbackHandler.Unregister(OperationContext.Current.GetCallbackChannel<IPlayerCallback>());
        }

        #endregion Methods
    }
}