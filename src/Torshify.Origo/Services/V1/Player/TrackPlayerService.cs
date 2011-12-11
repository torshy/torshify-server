using System.ServiceModel;
using Microsoft.Practices.ServiceLocation;
using Torshify.Origo.Contracts.V1.Player;
using Torshify.Origo.Extensions;
using Torshify.Origo.Interfaces;
using Torshify.Origo.Services.V1.Login;

namespace Torshify.Origo.Services.V1.Player
{
    [ServiceBehavior(UseSynchronizationContext = false)]
    public class TrackPlayerService : ITrackPlayerService
    {
        public void Play(string trackId)
        {
            LoginService.EnsureUserIsLoggedIn();
            var musicPlayerController = ServiceLocator.Current.Resolve<IMusicPlayerController>();
            musicPlayerController.Play(trackId);
        }
    }
}