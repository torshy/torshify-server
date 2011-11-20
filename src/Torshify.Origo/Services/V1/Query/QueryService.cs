using System;
using System.ServiceModel;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using Torshify.Origo.Contracts.V1;
using Torshify.Origo.Contracts.V1.Query;
using Torshify.Origo.Extensions;

namespace Torshify.Origo.Services.V1.Query
{
    [ServiceBehavior(UseSynchronizationContext = false)]
    public class QueryService : IQueryService
    {
        #region Methods

        public QueryResult Query(
            string query,
            int trackOffset,
            int trackCount,
            int albumOffset,
            int albumCount,
            int artistOffset,
            int artistCount)
        {
            var session = ServiceLocator.Current.Resolve<ISession>();

            return session
                .SearchAsync(query, trackOffset, trackCount, albumOffset, albumCount, artistOffset, artistCount)
                .ContinueWith(t => Convertion.ConvertToDto(t.Result)).Result;
        }

        public IAsyncResult BeginQuery(
            string query,
            int trackOffset,
            int trackCount,
            int albumOffset,
            int albumCount,
            int artistOffset,
            int artistCount,
            AsyncCallback callback,
            object state)
        {
            var session = ServiceLocator.Current.Resolve<ISession>();

            return session
                .SearchAsync(query, trackOffset, trackCount, albumOffset, albumCount, artistOffset, artistCount)
                .ContinueWith(t => Convertion.ConvertToDto(t.Result))
                .ContinueWith(t => callback(t));
        }

        public QueryResult EndQuery(IAsyncResult result)
        {
            return ((Task<QueryResult>)result).Result;
        }

        public AlbumBrowseResult AlbumBrowse(string albumId)
        {
            var session = ServiceLocator.Current.Resolve<ISession>();

            using (var link = session.FromLink<IAlbum>(albumId))
            {
                return session
                    .BrowseAsync(link.Object)
                    .ContinueWith(t => Convertion.ConvertToDto(t.Result)).Result;
            }
        }

        public IAsyncResult BeginAlbumBrowse(string albumId, AsyncCallback callback, object state)
        {
            var session = ServiceLocator.Current.Resolve<ISession>();
            var link = session.FromLink<IAlbum>(albumId);

            return session
                .BrowseAsync(link.Object)
                .ContinueWith(t =>
                            {
                                using (link)
                                {
                                    using (link.Object)
                                    {
                                        return Convertion.ConvertToDto(t.Result);
                                    }
                                }
                            })
                .ContinueWith(t => callback(t));
        }

        public AlbumBrowseResult EndAlbumBrowse(IAsyncResult result)
        {
            return ((Task<AlbumBrowseResult>)result).Result;
        }

        public ArtistBrowseResult ArtistBrowse(string artistId, ArtistBrowsingType type)
        {
            var session = ServiceLocator.Current.Resolve<ISession>();
            
            using (var link = session.FromLink<IArtist>(artistId))
            {
                return session
                    .BrowseAsync(link.Object, Convertion.ConvertBack(type))
                    .ContinueWith(t => Convertion.ConvertToDto(t.Result)).Result;
            }
        }

        public IAsyncResult BeginArtistBrowse(string artistId, ArtistBrowsingType type, AsyncCallback callback, object state)
        {
            var session = ServiceLocator.Current.Resolve<ISession>();
            var link = session.FromLink<IArtist>(artistId);

            return session
                .BrowseAsync(link.Object, Convertion.ConvertBack(type))
                .ContinueWith(t =>
                {
                    using (link)
                    {
                        using (link.Object)
                        {
                            return Convertion.ConvertToDto(t.Result);
                        }
                    }
                })
                .ContinueWith(t => callback(t));
        }

        public ArtistBrowseResult EndArtistBrowse(IAsyncResult result)
        {
            return ((Task<ArtistBrowseResult>)result).Result;
        }

        public Playlist GetPlaylist(string link)
        {
            var session = ServiceLocator.Current.Resolve<ISession>();
            var linkObject = session.FromLink<IPlaylist>(link);

            if (linkObject != null)
            {
                using(linkObject)
                {
                    return Convertion.ConvertToDto(linkObject.Object);
                }
            }

            return null;
        }

        #endregion Methods
    }
}