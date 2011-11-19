﻿using System;
using System.Collections.Generic;
using System.Linq;

using Torshify.Origo.Contracts.V1;
using Torshify.Origo.Interfaces;
using Torshify.Origo.Services.V1;

namespace Torshify.Origo.Audio
{
    public class PlaylistController : IPlaylistController
    {
        #region Fields

        private readonly IMusicPlayerController _musicPlayerController;
        private readonly ISession _session;

        private Playlist<PlaylistTrack> _playlist;

        #endregion Fields

        #region Constructors

        public PlaylistController(ISession session, IMusicPlayerController musicPlayerController)
        {
            _session = session;
            _musicPlayerController = musicPlayerController;
            _playlist = new Playlist<PlaylistTrack>();
            _playlist.CurrentChanged += OnPlaylistCurrentChanged;
        }

        #endregion Constructors

        #region Methods

        public bool Repeat
        {
            get { return _playlist.Repeat; }
            set { _playlist.Repeat = value; }
        }

        public void Enqueue(string linkId)
        {
            var tracks = GetTracks(linkId);
            var tracksToEnqueue = tracks.Select(t => new PlaylistTrack {Track = t, IsQueued = true});

            _playlist.Enqueue(tracksToEnqueue.ToArray());
        }

        public void Initialize(string linkId)
        {
            var tracks = GetTracks(linkId);
            var trackToInitialize = tracks.Select(t => new PlaylistTrack { Track = t });

            _playlist.Initialize(trackToInitialize.ToArray());
        }

        public void Next()
        {
            _playlist.Next();
        }

        public void Previous()
        {
            _playlist.Previous();
        }

        public PlaylistTrack Current
        {
            get { return _playlist.Current; }
        }

        public PlaylistTrack[] Sequence
        {
            get { return _playlist.Sequence.ToArray(); }
        }

        public bool Shuffle
        {
            get
            {
                return _playlist.Shuffle;
            }
            set
            {
                _playlist.Shuffle = value;
            }
        }

        private IEnumerable<Track> GetTracks(string linkId)
        {
            IEnumerable<Track> tracksToAdd = new List<Track>();

            using (var link = _session.FromLink(linkId))
            {
                switch (link.Type)
                {
                    case LinkType.Invalid:
                        break;
                    case LinkType.Track:
                        tracksToAdd = GetTracks((ILink<ITrackAndOffset>)link);
                        break;
                    case LinkType.Album:
                        tracksToAdd = GetTracks((ILink<IAlbum>)link);
                        break;
                    case LinkType.Artist:
                        tracksToAdd = GetTracks((ILink<IArtist>)link);
                        break;
                    case LinkType.Search:
                        break;
                    case LinkType.Playlist:
                        break;
                    case LinkType.Profile:
                        break;
                    case LinkType.Starred:
                        break;
                    case LinkType.LocalTrack:
                        break;
                    case LinkType.Image:
                        break;
                }
            }

            return tracksToAdd;
        }

        private IEnumerable<Track> GetTracks(ILink<IArtist> link)
        {
            using (link.Object)
            {
                IArtistBrowse browse = _session.BrowseAsync(link.Object, ArtistBrowseType.NoAlbums).Result;
                return browse.Tracks.Select(Convertion.ConvertToDto).ToArray();
            }
        }

        private IEnumerable<Track> GetTracks(ILink<IAlbum> link)
        {
            using (link.Object)
            {
                IAlbumBrowse browse = _session.BrowseAsync(link.Object).Result;
                return browse.Tracks.Select(Convertion.ConvertToDto).ToArray();
            }
        }

        private IEnumerable<Track> GetTracks(ILink<ITrackAndOffset> link)
        {
            yield return Convertion.ConvertToDto(link.Object.Track);
        }

        private void OnPlaylistCurrentChanged(object sender, EventArgs e)
        {
            var playlistTrack = _playlist.Current;
            if (playlistTrack != null)
            {
                _musicPlayerController.Play(playlistTrack.Track.ID);
            }
        }

        #endregion Methods
    }

}