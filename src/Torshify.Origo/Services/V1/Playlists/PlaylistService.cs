using System.ServiceModel;
using Microsoft.Practices.ServiceLocation;
using Torshify.Origo.Contracts.V1;
using Torshify.Origo.Contracts.V1.Playlists;
using Torshify.Origo.Extensions;
using System.Linq;
using Torshify.Origo.Services.V1.Login;

namespace Torshify.Origo.Services.V1.Playlists
{
    [ServiceBehavior(UseSynchronizationContext = false)]
    public class PlaylistService : IPlaylistService
    {
        public void Subscribe()
        {
            LoginService.EnsureUserIsLoggedIn();
        }

        public void Unsubscribe()
        {
            LoginService.EnsureUserIsLoggedIn();
        }

        public Playlist[] GetPlaylists()
        {
            LoginService.EnsureUserIsLoggedIn();
            var session = ServiceLocator.Current.Resolve<ISession>();
            return session.PlaylistContainer.Playlists.Select(Convertion.ConvertToDto).ToArray();
        }
    }
}