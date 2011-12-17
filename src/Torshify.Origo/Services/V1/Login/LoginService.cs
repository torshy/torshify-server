using System.ServiceModel;
using Microsoft.Practices.ServiceLocation;
using Torshify.Origo.Contracts.V1;
using Torshify.Origo.Contracts.V1.Login;
using Torshify.Origo.Extensions;

namespace Torshify.Origo.Services.V1.Login
{
    [ServiceBehavior(UseSynchronizationContext = false, IncludeExceptionDetailInFaults = true)]
    public class LoginService : ILoginService
    {
        public void Subscribe()
        {
            var callbackHandler = ServiceLocator.Current.Resolve<LoginCallbackHandler>();
            callbackHandler.Register(OperationContext.Current.GetCallbackChannel<ILoginCallback>());
        }

        public void Unsubscribe()
        {
            var callbackHandler = ServiceLocator.Current.Resolve<LoginCallbackHandler>();
            callbackHandler.Unregister(OperationContext.Current.GetCallbackChannel<ILoginCallback>());
        }

        public bool IsLoggedIn()
        {
            var session = ServiceLocator.Current.Resolve<ISession>();
            return session.ConnectionState == ConnectionState.Offline ||
                   session.ConnectionState == ConnectionState.LoggedIn;
        }

        public string GetRememberedUser()
        {
            var session = ServiceLocator.Current.Resolve<ISession>();
            return session.GetRememberedUser();
        }

        public void Login(string userName, string password, bool remember)
        {
            var session = ServiceLocator.Current.Resolve<ISession>();
            session.Login(userName, password, remember);
        }

        public void ForgetRememberedUser()
        {
            var session = ServiceLocator.Current.Resolve<ISession>();
            session.ForgetStoredLogin();
        }

        public void Relogin()
        {
            var session = ServiceLocator.Current.Resolve<ISession>();
            session.Relogin();
        }

        public static void EnsureUserIsLoggedIn()
        {
            LoginService service = new LoginService();
            if (!service.IsLoggedIn())
            {
                throw new FaultException<NotLoggedInFault>(new NotLoggedInFault(), "Not logged in");
            }
        }
    }
}