using System;
using System.ServiceModel;

using log4net;

using Torshify.Origo.Contracts.V1.Player;
using Torshify.Origo.Extensions;
using Torshify.Origo.Interfaces;
using Torshify.Origo.Services.V1.Login;

namespace Torshify.Origo.Services.V1.Player
{
    [ServiceBehavior(UseSynchronizationContext = false, IncludeExceptionDetailInFaults = true)]
    [ServiceLocatorServiceBehavior]
    public class TrackPlayerService : ITrackPlayerService
    {
        #region Fields

        private readonly IMusicPlayerController _musicPlayerController;

        private ILog _log = LogManager.GetLogger(typeof (TrackPlayerService));

        #endregion Fields

        #region Constructors

        public TrackPlayerService(IMusicPlayerController musicPlayerController)
        {
            _musicPlayerController = musicPlayerController;
        }

        #endregion Constructors

        #region Methods

        public void Play(string trackId)
        {
            try
            {
                LoginService.EnsureUserIsLoggedIn();
                _musicPlayerController.Play(trackId);
            }
            catch(Exception e)
            {
                _log.Error(e);
            }
        }

        #endregion Methods
    }
}