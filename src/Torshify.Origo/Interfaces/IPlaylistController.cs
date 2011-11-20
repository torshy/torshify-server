using Torshify.Origo.Contracts.V1;

namespace Torshify.Origo.Interfaces
{
    public interface IPlaylistController
    {
        #region Properties

        PlaylistTrack Current
        {
            get;
        }

        PlaylistTrack[] Sequence
        {
            get;
        }

        bool Shuffle
        {
            get; 
            set;
        }

        bool Repeat
        {
            get; 
            set;
        }

        #endregion Properties

        #region Methods

        void Enqueue(string[] linkId);

        void Initialize(string[] linkId);

        void Next();

        void Previous();

        #endregion Methods
    }
}