using System;
using System.Threading;
using System.Timers;

using Microsoft.Practices.ServiceLocation;
using Torshify.Origo.Contracts.V1;
using Torshify.Origo.Extensions;
using Torshify.Origo.Interfaces;

using Timer = System.Timers.Timer;

namespace Torshify.Origo.Audio
{
    public class MusicPlayerController : IMusicPlayerController, IStartable
    {
        #region Fields

        private readonly IMusicPlayer _musicPlayer;
        private readonly ISession _session;

        private Track _currentTrack;
        private TimeSpan _elapsed;
        private bool _isPlaying;
        private Timer _timer;

        #endregion Fields

        #region Constructors

        public MusicPlayerController(ISession session, IMusicPlayer musicPlayer)
        {
            _musicPlayer = musicPlayer;
            _session = session;
            _timer = new Timer();
            _timer.Elapsed += OnTimerElapsed;
            _timer.Interval = 100;
        }

        #endregion Constructors

        #region Events

        public event EventHandler CurrentTrackChanged;

        public event EventHandler ElapsedChanged;

        public event EventHandler IsPlayingChanged;

        #endregion Events

        #region Properties

        public Track CurrentTrack
        {
            get
            {
                return _currentTrack;
            }
            private set
            {
                if (_currentTrack != value)
                {
                    _currentTrack = value;
                    OnCurrentTrackChanged();
                }
            }
        }

        public TimeSpan Elapsed
        {
            get
            {
                return _elapsed;
            }
            private set
            {
                if (_elapsed != value)
                {
                    _elapsed = value;
                    OnElapsedChanged();
                }
            }
        }

        public bool IsPlaying
        {
            get
            {
                return _isPlaying;
            }
            private set
            {
                if (_isPlaying != value)
                {
                    _isPlaying = value;
                    _timer.Enabled = value;
                    OnIsPlayingChanged();
                }
            }
        }

        public float Volume
        {
            get
            {
                return _musicPlayer.Volume;
            }
            set
            {
                _musicPlayer.Volume = value;
            }
        }

        #endregion Properties

        #region Methods

        public void Play(string trackId)
        {
            IsPlaying = false;

            var session = ServiceLocator.Current.Resolve<ISession>();
            using (var link = session.FromLink<ITrackAndOffset>(trackId))
            {
                using (var track = link.Object.Track)
                {
                    while (!track.IsLoaded)
                    {
                        Thread.Yield();
                    }

                    CurrentTrack = AutoMapper.Mapper.Map<ITrack, Track>(track);
                    Elapsed = TimeSpan.Zero;

                    var status = session.PlayerLoad(track);
                    if (status == Error.OK)
                    {
                        session.PlayerPlay();
                    }
                    else
                    {
                        throw new Exception(status.GetMessage());
                    }
                }
            }
        }

        public void TogglePause()
        {
            if (CurrentTrack == null)
            {
                return;
            }

            if (IsPlaying)
            {
                _musicPlayer.Play();
            }
            else
            {
                _musicPlayer.Play();
            }
        }

        public void Start()
        {
            _session.MusicDeliver += OnMusicDeliver;
        }

        private void OnCurrentTrackChanged()
        {
            var handler = CurrentTrackChanged;

            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void OnElapsedChanged()
        {
            var handler = ElapsedChanged;

            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void OnIsPlayingChanged()
        {
            var handler = IsPlayingChanged;

            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void OnMusicDeliver(object sender, MusicDeliveryEventArgs e)
        {
            e.ConsumedFrames = _musicPlayer.EnqueueSamples(e.Channels, e.Rate, e.Samples, e.Frames);
            IsPlaying = true;
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            Elapsed = Elapsed.Add(TimeSpan.FromMilliseconds(_timer.Interval));

            if (_musicPlayer.GetBufferLength() == 0)
            {
                IsPlaying = false;
            }
        }

        #endregion Methods
    }
}