using System.ServiceModel;

using Microsoft.Practices.ServiceLocation;
using Torshify.Origo.Contracts.V1;
using Torshify.Origo.Contracts.V1.Player;
using Torshify.Origo.Extensions;
using Torshify.Origo.Interfaces;

namespace Torshify.Origo.Services.V1.Player
{
    [ServiceBehavior(UseSynchronizationContext = false)]
    public class PlaylistPlayerService : IPlaylistPlayerService
    {
        public void Initialize(string linkId)
        {
            var playlistController = ServiceLocator.Current.Resolve<IPlaylistController>();
            playlistController.Initialize(linkId);
        }

        public void Enqueue(string linkId)
        {
            var playlistController = ServiceLocator.Current.Resolve<IPlaylistController>();
            playlistController.Enqueue(linkId);
        }

        public void Next()
        {
            var playlistController = ServiceLocator.Current.Resolve<IPlaylistController>();
            playlistController.Next();
        }

        public void Previous()
        {
            var playlistController = ServiceLocator.Current.Resolve<IPlaylistController>();
            playlistController.Previous();
        }

        public PlaylistTrack GetCurrent()
        {
            var playlistController = ServiceLocator.Current.Resolve<IPlaylistController>();
            return playlistController.Current;
        }

        public PlaylistTrack[] GetPlaylist()
        {
            var playlistController = ServiceLocator.Current.Resolve<IPlaylistController>();
            return playlistController.Sequence;
        }
    }
}