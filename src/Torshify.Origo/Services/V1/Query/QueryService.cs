using System;
using System.ServiceModel;
using System.Threading.Tasks;

using log4net;

using Torshify.Origo.Contracts.V1;
using Torshify.Origo.Contracts.V1.Query;
using Torshify.Origo.Extensions;
using Torshify.Origo.Services.V1.Login;

namespace Torshify.Origo.Services.V1.Query
{
    [ServiceBehavior(UseSynchronizationContext = false, IncludeExceptionDetailInFaults = true)]
    [ServiceLocatorServiceBehavior]
    public class QueryService : IQueryService
    {
        #region Fields

        private readonly ISession _session;

        private ILog _log = LogManager.GetLogger(typeof(QueryService));

        #endregion Fields

        #region Constructors

        public QueryService(ISession session)
        {
            _session = session;
        }

        #endregion Constructors

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
            LoginService.EnsureUserIsLoggedIn();

            return _session
                .SearchAsync(query, trackOffset, trackCount, albumOffset, albumCount, artistOffset, artistCount, 0, 0, SearchType.Standard)
                .ContinueWith(t => Convertion.ConvertToDto(t.Result))
                .ContinueWith(t =>
                {
                    if (t.Exception != null)
                    {
                        _log.Error(t.Exception);
                        return new QueryResult();
                    }

                    return t.Result;
                }).Result;
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
            LoginService.EnsureUserIsLoggedIn();

            return _session
                .SearchAsync(query, trackOffset, trackCount, albumOffset, albumCount, artistOffset, artistCount, 0, 0, SearchType.Standard)
                .ContinueWith(t => Convertion.ConvertToDto(t.Result))
                .ContinueWith(t => callback(t));
        }

        public QueryResult EndQuery(IAsyncResult result)
        {
            return ((Task<QueryResult>)result).Result;
        }

        public AlbumBrowseResult AlbumBrowse(string albumId)
        {
            LoginService.EnsureUserIsLoggedIn();

            using (var link = _session.FromLink<IAlbum>(albumId))
            {
                using (link.Object)
                {
                    return _session
                        .BrowseAsync(link.Object)
                        .ContinueWith(t => Convertion.ConvertToDto(t.Result))
                        .ContinueWith(t =>
                        {
                            if (t.Exception != null)
                            {
                                _log.Error(t.Exception);
                                return new AlbumBrowseResult();
                            }

                            return t.Result;
                        }).Result;
                }
            }
        }

        public IAsyncResult BeginAlbumBrowse(string albumId, AsyncCallback callback, object state)
        {
            LoginService.EnsureUserIsLoggedIn();
            var link = _session.FromLink<IAlbum>(albumId);

            return _session
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
            LoginService.EnsureUserIsLoggedIn();

            using (var link = _session.FromLink<IArtist>(artistId))
            {
                using (link.Object)
                {
                    return _session
                        .BrowseAsync(link.Object, Convertion.ConvertBack(type))
                        .ContinueWith(t => Convertion.ConvertToDto(t.Result))
                        .ContinueWith(t =>
                        {
                            if (t.Exception != null)
                            {
                                _log.Error(t.Exception);
                                return new ArtistBrowseResult();
                            }

                            return t.Result;
                        }).Result;
                }
            }
        }

        public IAsyncResult BeginArtistBrowse(string artistId, ArtistBrowsingType type, AsyncCallback callback, object state)
        {
            LoginService.EnsureUserIsLoggedIn();
            var link = _session.FromLink<IArtist>(artistId);

            return _session
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
            LoginService.EnsureUserIsLoggedIn();

            var linkObject = _session.FromLink<IPlaylist>(link);

            if (linkObject != null)
            {
                using (linkObject)
                {
                    return Convertion.ConvertToDto(linkObject.Object);
                }
            }

            return null;
        }

        #endregion Methods
    }
}