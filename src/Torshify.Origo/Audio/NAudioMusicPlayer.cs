using System;

using NAudio.Wave;

using Torshify.Origo.Interfaces;

namespace Torshify.Origo.Audio
{
    public class NAudioMusicPlayer : IMusicPlayer
    {
        #region Fields

        private float _volume = 0.2f;
        private WaveOut _waveOut;
        private BufferedWaveProvider _waveProvider;

        #endregion Fields

        #region Properties

        public float Volume
        {
            get { return _volume; }
            set { _volume = value; }
        }

        #endregion Properties

        #region Methods

        public void ClearBuffers()
        {
            if (_waveOut != null)
            {
                try
                {
                    _waveOut.Dispose();
                }
                catch
                {
                }

                _waveOut = null;
            }

            _waveProvider = null;
        }

        public int EnqueueSamples(int channels, int rate, byte[] samples, int frames)
        {
            int consumed = 0;

            if (_waveProvider == null || frames == 0)
            {
                _waveOut = new WaveOut();
                _waveProvider = new BufferedWaveProvider(new WaveFormat(rate, channels));
                _waveProvider.BufferDuration = TimeSpan.FromSeconds(1);
                _waveOut.Init(_waveProvider);
                _waveOut.Play();
            }

            if ((_waveProvider.BufferLength - _waveProvider.BufferedBytes) > samples.Length)
            {
                _waveOut.Volume = _volume;
                _waveProvider.AddSamples(samples, 0, samples.Length);
                consumed = frames;
            }

            return consumed;
        }

        public void Pause()
        {
            if (_waveOut != null)
            {
                _waveOut.Pause();
            }
        }

        public void Play()
        {
            if (_waveOut != null)
            {
                _waveOut.Play();
            }
        }

        public int GetBufferLength()
        {
            if (_waveProvider != null)
            {
                return _waveProvider.BufferedBytes;
            }

            return 0;
        }

        #endregion Methods
    }
}