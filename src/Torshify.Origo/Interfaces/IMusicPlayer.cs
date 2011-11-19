namespace Torshify.Origo.Interfaces
{
    public interface IMusicPlayer
    {
        #region Properties

        float Volume
        {
            get; set;
        }

        #endregion Properties

        #region Methods

        void ClearBuffers();

        int EnqueueSamples(int channels, int rate, byte[] samples, int frames);

        int GetBufferLength();

        void Pause();

        void Play();

        #endregion Methods
    }
}