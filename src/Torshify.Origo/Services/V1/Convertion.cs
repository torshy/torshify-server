using AutoMapper;
using Torshify.Origo.Contracts.V1;
using Torshify.Origo.Contracts.V1.Query;

namespace Torshify.Origo.Services.V1
{
    public static class Convertion
    {
        #region Methods

        public static QueryResult ConvertToDto(ISearch search)
        {
            using (search)
            {
                return Mapper.Map<ISearch, QueryResult>(search);
            }
        }

        public static ArtistBrowseResult ConvertToDto(IArtistBrowse browse)
        {
            using(browse)
            {
                return Mapper.Map<IArtistBrowse, ArtistBrowseResult>(browse);
            }
        }

        public static AlbumBrowseResult ConvertToDto(IAlbumBrowse browse)
        {
            using(browse)
            {
                return Mapper.Map<IAlbumBrowse, AlbumBrowseResult>(browse);
            }   
        }

        public static Playlist ConvertToDto(IPlaylist playlist)
        {
            using(playlist)
            {
                return Mapper.Map<IPlaylist, Playlist>(playlist);
            }
        }

        public static Album ConvertToDto(IAlbum album)
        {
            using (album)
            {
                return Mapper.Map<IAlbum, Album>(album);
            }
        }

        public static Artist ConvertToDto(IArtist artist)
        {
            using (artist)
            {
                return Mapper.Map<IArtist, Artist>(artist);
            }
        }

        public static Track ConvertToDto(ITrack track)
        {
            using (track)
            {
                return Mapper.Map<ITrack, Track>(track);
            }
        }

        public static ArtistBrowseType ConvertBack(ArtistBrowsingType type)
        {
            ArtistBrowseType artistBrowseType;
            switch (type)
            {
                case ArtistBrowsingType.Full:
                    artistBrowseType = ArtistBrowseType.Full;
                    break;
                case ArtistBrowsingType.NoAlbums:
                    artistBrowseType = ArtistBrowseType.NoAlbums;
                    break;
                case ArtistBrowsingType.NoTracks:
                    artistBrowseType = ArtistBrowseType.NoTracks;
                    break;
                default:
                    artistBrowseType = ArtistBrowseType.Full;
                    break;
            }
            return artistBrowseType;
        }

        #endregion Methods
    }
}