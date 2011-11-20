using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;
using Torshify.Origo.Contracts.V1.Player;
using Torshify.Origo.Interfaces;
using log4net;

namespace Torshify.Origo.Services.V1.Player
{
    public class PlayerCallbackHandler : IStartable
    {
        #region Fields

        private readonly IMusicPlayerController _playerController;
        private readonly List<IPlayerCallback> _callbacks;
        private readonly object _callbacksLock = new object();
        private readonly ILog _log = LogManager.GetLogger(typeof (PlayerCallbackHandler));

        #endregion Fields

        #region Constructors

        public PlayerCallbackHandler(IMusicPlayerController playerController)
        {
            _playerController = playerController;
            _callbacks = new List<IPlayerCallback>();
        }

        #endregion Constructors

        #region Methods

        void IStartable.Start()
        {
            _playerController.CurrentTrackChanged += OnCurrentTrackChanged;
            _playerController.ElapsedChanged += OnElapsedChanged;
            _playerController.IsPlayingChanged += OnIsPlayingChanged;
            _playerController.TrackComplete += OnTrackComplete;
            _playerController.VolumeChanged += OnVolumeChanged;
        }

        public bool Register(IPlayerCallback callback)
        {
            lock (_callbacksLock)
            {
                if (!_callbacks.Contains(callback))
                {
                    _callbacks.Add(callback);
                    return true;
                }
            }

            return false;
        }

        public bool Unregister(IPlayerCallback callback)
        {
            lock (_callbacksLock)
            {
                return _callbacks.Remove(callback);
            }
        }

        private void Execute(Action<IPlayerCallback> action)
        {
            var faultedClients = new List<IPlayerCallback>();

            lock (_callbacksLock)
            {
                foreach (var client in _callbacks)
                {
                    try
                    {
                        action(client);
                    }
                    catch (CommunicationException)
                    {
                        ((ICommunicationObject)client).Abort();
                        faultedClients.Add(client);
                    }
                    catch (Exception)
                    {
                        ((ICommunicationObject)client).Abort();
                        faultedClients.Add(client);
                    }
                }
            }

            foreach (var faultedClient in faultedClients)
            {
                Unregister(faultedClient);
            }
        }

        private void OnTrackComplete(object sender, EventArgs e)
        {
            try
            {
                var currentTrack = _playerController.CurrentTrack;

                if (currentTrack != null)
                {
                    ThreadPool.QueueUserWorkItem(
                        state => Execute(
                            callback =>
                            callback.OnTrackComplete(currentTrack)));
                }
            }
            catch (Exception exception)
            {
                _log.Error(exception.Message, exception);
            }
        }

        private void OnCurrentTrackChanged(object sender, EventArgs e)
        {
            try
            {
                if (_playerController.CurrentTrack != null)
                {
                    var track = _playerController.CurrentTrack;

                    ThreadPool.QueueUserWorkItem(
                        state => Execute(callback => callback.OnTrackChanged(track)));
                }
            }
            catch (Exception exception)
            {
                _log.Error(exception.Message, exception);
            }
        }

        private void OnElapsedChanged(object sender, EventArgs e)
        {
            try
            {
                if (_playerController.CurrentTrack != null)
                {
                    var elapsedMs = _playerController.Elapsed.TotalMilliseconds;
                    var totalMs = _playerController.CurrentTrack.Duration;

                    ThreadPool.QueueUserWorkItem(
                        state => Execute(
                            callback =>
                            callback.OnElapsed(elapsedMs, totalMs)));
                }
            }
            catch (Exception exception)
            {
                _log.Error(exception.Message, exception);
            }
        }

        private void OnIsPlayingChanged(object sender, EventArgs e)
        {
            try
            {
                var isPlaying = _playerController.IsPlaying;

                ThreadPool.QueueUserWorkItem(
                    state => Execute(
                        callback =>
                        callback.OnPlayStateChanged(isPlaying)));
            }
            catch (Exception exception)
            {
                _log.Error(exception.Message, exception);
            }
        }

        private void OnVolumeChanged(object sender, EventArgs e)
        {
            try
            {
                float volume = _playerController.Volume;

                ThreadPool.QueueUserWorkItem(
                    state => Execute(
                        callback =>
                        callback.OnVolumeChanged(volume)));
            }
            catch (Exception exception)
            {
                _log.Error(exception.Message, exception);
            }
        }
        #endregion Methods
    }
}