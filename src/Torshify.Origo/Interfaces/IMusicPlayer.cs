namespace Torshify.Origo.Interfaces
{
    public interface IMusicPlayer
    {
        int EnqueueSamples(int channels, int rate, byte[] samples, int frames);
        void Pause();
        void Play();
        void ClearBuffers();
        float Volume { get; set; }
        int GetBufferLength();
    }
}