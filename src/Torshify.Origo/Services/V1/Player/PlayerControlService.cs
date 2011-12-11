using System;
using System.ServiceModel;

using Microsoft.Practices.ServiceLocation;
using Torshify.Origo.Contracts.V1;
using Torshify.Origo.Contracts.V1.Player;
using Torshify.Origo.Extensions;
using Torshify.Origo.Interfaces;
using Torshify.Origo.Services.V1.Login;

namespace Torshify.Origo.Services.V1.Player
{
    [ServiceBehavior(
        UseSynchronizationContext = false)]
    public class PlayerControlService : IPlayerControlService
    {
        #region Methods

        public PlayerStatus GetStatus()
        {
            LoginService.EnsureUserIsLoggedIn();
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
            LoginService.EnsureUserIsLoggedIn();
            var musicPlayerController = ServiceLocator.Current.Resolve<IMusicPlayerController>();
            musicPlayerController.TogglePause();
        }

        public void SetVolume(float volume)
        {
            LoginService.EnsureUserIsLoggedIn();
            var musicPlayerController = ServiceLocator.Current.Resolve<IMusicPlayerController>();
            musicPlayerController.Volume = volume;
        }

        public float GetVolume()
        {
            LoginService.EnsureUserIsLoggedIn();
            var musicPlayerController = ServiceLocator.Current.Resolve<IMusicPlayerController>();
            return musicPlayerController.Volume;
        }

        public void Seek(double milliseconds)
        {
            LoginService.EnsureUserIsLoggedIn();
            var musicPlayerController = ServiceLocator.Current.Resolve<IMusicPlayerController>();
            musicPlayerController.Seek(TimeSpan.FromMilliseconds(milliseconds));
        }

        public void Subscribe()
        {
            LoginService.EnsureUserIsLoggedIn();
            var callbackHandler = ServiceLocator.Current.Resolve<PlayerCallbackHandler>();
            callbackHandler.Register(OperationContext.Current.GetCallbackChannel<IPlayerCallback>());
        }

        public void Unsubscribe()
        {
            LoginService.EnsureUserIsLoggedIn();
            var callbackHandler = ServiceLocator.Current.Resolve<PlayerCallbackHandler>();
            callbackHandler.Unregister(OperationContext.Current.GetCallbackChannel<IPlayerCallback>());
        }

        #endregion Methods
    }
}