using System;
using System.ServiceModel;

using Torshify.Origo.Contracts.V1;
using Torshify.Origo.Contracts.V1.Player;
using Torshify.Origo.Extensions;
using Torshify.Origo.Interfaces;
using Torshify.Origo.Services.V1.Login;
using log4net;

namespace Torshify.Origo.Services.V1.Player
{
    [ServiceBehavior(
        UseSynchronizationContext = false, IncludeExceptionDetailInFaults = true)]
    [ServiceLocatorServiceBehavior]
    public class PlayerControlService : IPlayerControlService
    {
        #region Fields

        private readonly PlayerCallbackHandler _callbackHandler;
        private readonly IMusicPlayerController _musicPlayerController;
        private ILog _log = LogManager.GetLogger(typeof (PlayerControlService));

        #endregion Fields

        #region Constructors

        public PlayerControlService(IMusicPlayerController musicPlayerController, PlayerCallbackHandler callbackHandler)
        {
            _musicPlayerController = musicPlayerController;
            _callbackHandler = callbackHandler;
        }

        #endregion Constructors

        #region Methods

        public PlayerStatus GetStatus()
        {
            _log.Debug("GetStatus called");

            LoginService.EnsureUserIsLoggedIn();

            PlayerStatus status = new PlayerStatus();
            status.CurrentTrack = _musicPlayerController.CurrentTrack;
            status.IsPlaying = _musicPlayerController.IsPlaying;
            status.TotalTime = _musicPlayerController.CurrentTrack != null ? _musicPlayerController.CurrentTrack.Duration : 0;
            status.ElapsedTime = _musicPlayerController.Elapsed.TotalMilliseconds;
            status.Volume = _musicPlayerController.Volume;
            return status;
        }

        public void TogglePause()
        {
            _log.Debug("TogglePause called");

            LoginService.EnsureUserIsLoggedIn();
            _musicPlayerController.TogglePause();
        }

        public void SetVolume(float volume)
        {
            _log.Debug("SetVolume called");

            LoginService.EnsureUserIsLoggedIn();
            _musicPlayerController.Volume = volume;
        }

        public float GetVolume()
        {
            _log.Debug("GetVolume called");

            LoginService.EnsureUserIsLoggedIn();
            return _musicPlayerController.Volume;
        }

        public void Seek(double milliseconds)
        {
            _log.Debug("Seek called");

            LoginService.EnsureUserIsLoggedIn();
            _musicPlayerController.Seek(TimeSpan.FromMilliseconds(milliseconds));
        }

        public void Subscribe()
        {
            _log.Debug("Subscribe called");
            _callbackHandler.Register(OperationContext.Current.GetCallbackChannel<IPlayerCallback>());
        }

        public void Unsubscribe()
        {
            _log.Debug("Unsubscribe called");
            _callbackHandler.Unregister(OperationContext.Current.GetCallbackChannel<IPlayerCallback>());
        }

        #endregion Methods
    }
}