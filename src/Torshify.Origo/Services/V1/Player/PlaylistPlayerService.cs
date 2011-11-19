using System.ServiceModel;

using Microsoft.Practices.ServiceLocation;
using Torshify.Origo.Contracts.V1;
using Torshify.Origo.Contracts.V1.Player;
using Torshify.Origo.Extensions;
using Torshify.Origo.Interfaces;

namespace Torshify.Origo.Services.V1.Player
{
    [ServiceBehavior(UseSynchronizationContext = false, IncludeExceptionDetailInFaults = true)]
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

        public void SetShuffle(bool shuffle)
        {
            var playlistController = ServiceLocator.Current.Resolve<IPlaylistController>();
            playlistController.Shuffle = shuffle;
        }

        public void SetRepeat(bool repeat)
        {
            var playlistController = ServiceLocator.Current.Resolve<IPlaylistController>();
            playlistController.Repeat = repeat;
        }

        public bool GetRepeat()
        {
            var playlistController = ServiceLocator.Current.Resolve<IPlaylistController>();
            return playlistController.Repeat;
        }

        public bool GetShuffle()
        {
            var playlistController = ServiceLocator.Current.Resolve<IPlaylistController>();
            return playlistController.Shuffle;
        }
    }
}