using System;
using System.IO;
using System.ServiceModel;
using System.Threading.Tasks;

using Microsoft.Practices.ServiceLocation;

using Torshify.Origo.Contracts.V1.Image;
using Torshify.Origo.Extensions;

namespace Torshify.Origo.Services.V1.Image
{
    [ServiceBehavior(UseSynchronizationContext = false)]
    public class ImageService : IImageService
    {
        #region Methods

        public IAsyncResult BeginGetAlbumImage(string link, AsyncCallback callback, object state)
        {
            if (string.IsNullOrEmpty(link))
            {
                return null;
            }

            var session = ServiceLocator.Current.Resolve<ISession>();
            var trackLink = session.FromLink<ITrackAndOffset>(link);
            if (trackLink != null)
            {
                using (trackLink)
                {
                    using (trackLink.Object.Track)
                    {
                        using (trackLink.Object.Track.Album)
                        {
                            return BeginGetImage(trackLink.Object.Track.Album.CoverId, callback, state);
                        }
                    }
                }
            }

            return null;
        }

        public IAsyncResult BeginGetArtistImage(string link, AsyncCallback callback, object state)
        {
            if (string.IsNullOrEmpty(link))
            {
                return null;
            }

            var session = ServiceLocator.Current.Resolve<ISession>();
            var artistLink = session.FromLink<IArtist>(link);
            if (artistLink != null)
            {
                using (artistLink)
                {
                    return GetArtistImageTask(session, artistLink.Object).ContinueWith(t => callback(t));
                }
            }

            return null;
        }

        public IAsyncResult BeginGetImage(string link, AsyncCallback callback, object state)
        {
            if (string.IsNullOrEmpty(link))
            {
                return null;
            }

            return GetImageTask(link).ContinueWith(t => callback(t));
        }

        public IAsyncResult BeginGetTrackImage(string link, AsyncCallback callback, object state)
        {
            if (string.IsNullOrEmpty(link))
            {
                return null;
            }

            var session = ServiceLocator.Current.Resolve<ISession>();
            var trackLink = session.FromLink<ITrackAndOffset>(link);
            if (trackLink != null)
            {
                using (trackLink)
                {
                    using (trackLink.Object.Track)
                    {
                        using (trackLink.Object.Track.Album)
                        {
                            return BeginGetAlbumImage(trackLink.Object.Track.Album.CoverId, callback, state);
                        }
                    }

                }
            }

            return null;
        }

        public Stream EndGetAlbumImage(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public Stream EndGetArtistImage(IAsyncResult result)
        {
            return ((Task<Stream>)result).Result;
        }

        public Stream EndGetImage(IAsyncResult result)
        {
            return ((Task<Stream>)result).Result;
        }

        public Stream EndGetTrackImage(IAsyncResult result)
        {
            return ((Task<Stream>)result).Result;
        }

        public Stream GetAlbumImage(string link)
        {
            throw new System.NotImplementedException();
        }

        public Stream GetArtistImage(string link)
        {
            if (string.IsNullOrEmpty(link))
            {
                return null;
            }

            var session = ServiceLocator.Current.Resolve<ISession>();
            var artistLink = session.FromLink<IArtist>(link);
            if (artistLink != null)
            {
                using (artistLink)
                {
                    return GetArtistImageTask(session, artistLink.Object).Result;
                }
            }

            return new MemoryStream();
        }

        public Stream GetImage(string link)
        {
            if (string.IsNullOrEmpty(link))
            {
                return new MemoryStream(); ;
            }

            return GetImageTask(link).Result;
        }

        public Stream GetTrackImage(string link)
        {
            if (string.IsNullOrEmpty(link))
            {
                return null;
            }

            var session = ServiceLocator.Current.Resolve<ISession>();
            var trackLink = session.FromLink<ITrackAndOffset>(link);
            if (trackLink != null)
            {
                using (trackLink)
                {
                    using (trackLink.Object.Track)
                    {
                        using (trackLink.Object.Track.Album)
                        {
                            return GetImage(trackLink.Object.Track.Album.CoverId);
                        }
                    }

                }
            }

            return null;
        }

        private Task<MemoryStream> GetImageTask(string link)
        {
            var session = ServiceLocator.Current.Resolve<ISession>();
            return session
                .GetImageAsync(link)
                .ContinueWith(t =>
                {
                    using (t.Result)
                    {
                        return new MemoryStream(t.Result.Data);
                    }
                });
        }

        private Task<Stream> GetArtistImageTask(ISession session, IArtist artist)
        {
            return session
                .BrowseAsync(artist, ArtistBrowseType.NoTracks | ArtistBrowseType.NoAlbums)
                .ContinueWith(t =>
                {
                    using (t.Result)
                    {
                        if (t.Result.Portraits.Count > 0)
                        {
                            using (var image = t.Result.Portraits[0])
                            {
                                return
                                    new MemoryStream(
                                        image.WaitForCompletion().Data);
                            }
                        }

                        if (t.Result.Albums.Count > 0)
                        {
                            foreach (var album in t.Result.Albums)
                            {
                                using (album)
                                {
                                    if (!string.IsNullOrEmpty(album.CoverId))
                                    {
                                        return GetImage(album.CoverId);
                                    }
                                }
                            }
                        }
                    }

                    return new MemoryStream();
                });
        }

        #endregion Methods
    }
}