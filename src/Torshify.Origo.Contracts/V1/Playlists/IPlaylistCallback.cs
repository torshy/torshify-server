using System.ServiceModel;

namespace Torshify.Origo.Contracts.V1.Playlists
{
    public interface IPlaylistCallback
    {
        [OperationContract(IsOneWay = true)]
        void OnPlaylistAdded(Playlist playlist, int position);

        [OperationContract(IsOneWay = true)]
        void OnPlaylistRemoved(Playlist playlist, int position);

        [OperationContract(IsOneWay = true)]
        void OnPlaylistMoved(Playlist playlist, int oldIndex, int newIndex);

        [OperationContract(IsOneWay = true)]
        void OnPing();
    }
}