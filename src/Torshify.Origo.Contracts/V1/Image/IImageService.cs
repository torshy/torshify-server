using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Torshify.Origo.Contracts.V1.Image
{
    [ServiceContract(
        Name = "ImageService",
        Namespace = "http://schemas.torshify/v1")]
    public interface IImageService
    {
        [OperationContract]
        [FaultContract(typeof(NotLoggedInFault))]
        [WebGet(UriTemplate = "album/{link}")]
        Stream GetAlbumImage(string link);

        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginGetAlbumImage(string link, AsyncCallback callback, object state);
        Stream EndGetAlbumImage(IAsyncResult result);

        [OperationContract]
        [FaultContract(typeof(NotLoggedInFault))]
        [WebGet(UriTemplate = "track/{link}")]
        Stream GetTrackImage(string link);

        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginGetTrackImage(string link, AsyncCallback callback, object state);
        Stream EndGetTrackImage(IAsyncResult result);

        [OperationContract]
        [FaultContract(typeof(NotLoggedInFault))]
        [WebGet(UriTemplate = "artist/{link}")]
        Stream GetArtistImage(string link);

        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginGetArtistImage(string link, AsyncCallback callback, object state);
        Stream EndGetArtistImage(IAsyncResult result);

        [OperationContract]
        [FaultContract(typeof(NotLoggedInFault))]
        [WebGet(UriTemplate = "id/{link}")]
        Stream GetImage(string link);

        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginGetImage(string link, AsyncCallback callback, object state);
        Stream EndGetImage(IAsyncResult result);
    }
}