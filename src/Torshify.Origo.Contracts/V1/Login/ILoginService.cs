using System.ServiceModel;

namespace Torshify.Origo.Contracts.V1.Login
{
    [ServiceContract(
        Name = "LoginService",
        Namespace = "http://schemas.torshify/v1")]
    public interface ILoginService
    {
        [OperationContract]
        bool IsLoggedIn();

        [OperationContract]
        string GetRememberedUser();

        [OperationContract]
        void Login(string userName, string password, bool remember);

        [OperationContract]
        void ForgetRememberedUser();

        [OperationContract]
        void Relogin();
    }
}