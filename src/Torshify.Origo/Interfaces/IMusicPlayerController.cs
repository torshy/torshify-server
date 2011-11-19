using System;

using Torshify.Origo.Contracts.V1;

namespace Torshify.Origo.Interfaces
{
    public interface IMusicPlayerController
    {
        #region Events

        event EventHandler CurrentTrackChanged;

        event EventHandler ElapsedChanged;

        event EventHandler IsPlayingChanged;

        #endregion Events

        #region Properties

        Track CurrentTrack
        {
            get;
        }

        TimeSpan Elapsed
        {
            get;
        }

        bool IsPlaying
        {
            get;
        }

        float Volume
        {
            get; 
            set;
        }

        #endregion Properties

        #region Methods

        void Play(string trackId);

        void TogglePause();

        #endregion Methods
    }

    public interface IPlaylistController
    {
        void Enqueue(string linkId);
        void Initialize(string linkId);
        void Next();
        void Previous();
        PlaylistTrack Current { get; }
        PlaylistTrack[] Sequence { get; }
    }
}