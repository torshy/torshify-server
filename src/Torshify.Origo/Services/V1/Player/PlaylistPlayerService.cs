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
    [ServiceBehavior(UseSynchronizationContext = false, IncludeExceptionDetailInFaults = true)]
    public class PlaylistPlayerService : IPlaylistPlayerService
    {
        public void Initialize(string[] linkId)
        {
            LoginService.EnsureUserIsLoggedIn();
            var playlistController = ServiceLocator.Current.Resolve<IPlaylistController>();
            playlistController.Initialize(linkId);
        }

        public void Enqueue(string[] linkId)
        {
            LoginService.EnsureUserIsLoggedIn();
            var playlistController = ServiceLocator.Current.Resolve<IPlaylistController>();
            playlistController.Enqueue(linkId);
        }

        public void Next()
        {
            LoginService.EnsureUserIsLoggedIn();
            var playlistController = ServiceLocator.Current.Resolve<IPlaylistController>();
            playlistController.Next();
        }

        public void Previous()
        {
            LoginService.EnsureUserIsLoggedIn();
            var playlistController = ServiceLocator.Current.Resolve<IPlaylistController>();
            playlistController.Previous();
        }

        public PlaylistTrack GetCurrent()
        {
            LoginService.EnsureUserIsLoggedIn();
            var playlistController = ServiceLocator.Current.Resolve<IPlaylistController>();
            return playlistController.Current;
        }

        public PlaylistTrack[] GetPlaylist()
        {
            LoginService.EnsureUserIsLoggedIn();
            var playlistController = ServiceLocator.Current.Resolve<IPlaylistController>();
            return playlistController.Sequence;
        }

        public void SetShuffle(bool shuffle)
        {
            LoginService.EnsureUserIsLoggedIn();
            var playlistController = ServiceLocator.Current.Resolve<IPlaylistController>();
            playlistController.Shuffle = shuffle;
        }

        public void SetRepeat(bool repeat)
        {
            LoginService.EnsureUserIsLoggedIn();
            var playlistController = ServiceLocator.Current.Resolve<IPlaylistController>();
            playlistController.Repeat = repeat;
        }

        public bool GetRepeat()
        {
            LoginService.EnsureUserIsLoggedIn();
            var playlistController = ServiceLocator.Current.Resolve<IPlaylistController>();
            return playlistController.Repeat;
        }

        public bool GetShuffle()
        {
            LoginService.EnsureUserIsLoggedIn();
            var playlistController = ServiceLocator.Current.Resolve<IPlaylistController>();
            return playlistController.Shuffle;
        }
    }
}