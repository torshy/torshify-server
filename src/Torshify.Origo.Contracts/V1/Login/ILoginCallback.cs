using System.ServiceModel;

namespace Torshify.Origo.Contracts.V1.Login
{
    public interface ILoginCallback
    {
        [OperationContract(IsOneWay = true)]
        void OnLoggedIn();

        [OperationContract(IsOneWay = true)]
        void OnLoginError(string message);

        [OperationContract(IsOneWay = true)]
        void OnLoggedOut();

        [OperationContract(IsOneWay = true)]
        void OnPing();
    }
}