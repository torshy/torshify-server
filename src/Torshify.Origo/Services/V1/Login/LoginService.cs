using System;
using System.ServiceModel;

using log4net;

using Microsoft.Practices.ServiceLocation;

using Torshify.Origo.Contracts.V1;
using Torshify.Origo.Contracts.V1.Login;
using Torshify.Origo.Extensions;

namespace Torshify.Origo.Services.V1.Login
{
    [ServiceBehavior(UseSynchronizationContext = false, IncludeExceptionDetailInFaults = true)]
    [ServiceLocatorServiceBehavior]
    public class LoginService : ILoginService
    {
        #region Fields

        private readonly ISession _session;
        private readonly LoginCallbackHandler _callbackHandler;

        private ILog _log = LogManager.GetLogger(typeof (LoginService));

        #endregion Fields

        #region Constructors

        public LoginService(ISession session, LoginCallbackHandler callbackHandler)
        {
            _session = session;
            _callbackHandler = callbackHandler;
        }

        #endregion Constructors

        #region Methods

        public static void EnsureUserIsLoggedIn()
        {
            LoginService service = ServiceLocator.Current.Resolve<LoginService>();
            if (!service.IsLoggedIn())
            {
                throw new FaultException<NotLoggedInFault>(new NotLoggedInFault(), "Not logged in");
            }
        }

        public void Subscribe()
        {
            _log.Info("Subscribe called");

            _callbackHandler.Register(OperationContext.Current.GetCallbackChannel<ILoginCallback>());
        }

        public void Unsubscribe()
        {
            _log.Info("Unsubscribe called");

            _callbackHandler.Unregister(OperationContext.Current.GetCallbackChannel<ILoginCallback>());
        }

        public bool IsLoggedIn()
        {
            _log.Debug("IsLoggedIn called");

            try
            {
                return _session.ConnectionState == ConnectionState.Offline ||
                       _session.ConnectionState == ConnectionState.LoggedIn;
            }
            catch (Exception e)
            {
                _log.Error(e);
                return false;
            }
        }

        public string GetRememberedUser()
        {
            _log.Debug("GetRememberedUser called");

            try
            {
                return _session.GetRememberedUser();
            }
            catch (Exception e)
            {
                _log.Error(e);
            }

            return null;
        }

        public void Login(string userName, string password, bool remember)
        {
            _log.Debug("Login called");

            try
            {
                _session.Login(userName, password, remember);
            }
            catch (Exception e)
            {
                _log.Error(e);
                throw new Exception("Unable to login");
            }
        }

        public void ForgetRememberedUser()
        {
            _log.Debug("ForgetRememberedUser called");

            try
            {
                _session.ForgetStoredLogin();
            }
            catch (Exception e)
            {
                _log.Error(e);
            }
        }

        public void Relogin()
        {
            _log.Debug("Relogin called");

            try
            {
                _session.Relogin();
            }
            catch (Exception e)
            {
                _log.Error(e);
            }
        }

        #endregion Methods
    }
}