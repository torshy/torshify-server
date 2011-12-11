using System;
using System.ServiceModel;
using System.Threading;
using Microsoft.Practices.ServiceLocation;
using Torshify.Origo.Contracts.V1;
using Torshify.Origo.Contracts.V1.Login;
using Torshify.Origo.Extensions;

namespace Torshify.Origo.Services.V1.Login
{
    [ServiceBehavior(UseSynchronizationContext = false)]
    public class LoginService : ILoginService
    {
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
            ManualResetEventSlim wait = new ManualResetEventSlim();
            session.LoginComplete += (sender, args) => wait.Set();
            session.Login(userName, password, remember);
            wait.Wait(5000);
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