using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;
using Torshify.Origo.Contracts.V1.Login;
using Torshify.Origo.Interfaces;
using log4net;

namespace Torshify.Origo.Services.V1.Login
{
    public class LoginCallbackHandler : IStartable
    {
        #region Fields

        private readonly ISession _session;
        private readonly List<ILoginCallback> _callbacks;
        private readonly object _callbacksLock = new object();
        private readonly ILog _log = LogManager.GetLogger(typeof(LoginCallbackHandler));

        #endregion Fields

        #region Constructors

        public LoginCallbackHandler(ISession session)
        {
            _session = session;
            _callbacks = new List<ILoginCallback>();
        }

        #endregion Constructors

        #region Methods

        public void Start()
        {
            _session.LoginComplete += OnLoginComplete;
            _session.LogoutComplete += OnLogoutComplete;
        }

        public bool Register(ILoginCallback callback)
        {
            lock (_callbacksLock)
            {
                if (!_callbacks.Contains(callback))
                {
                    _callbacks.Add(callback);
                    KeepAliveManager.Instance.Register(callback.OnPing);
                    return true;
                }
            }

            return false;
        }

        public bool Unregister(ILoginCallback callback)
        {
            lock (_callbacksLock)
            {
                KeepAliveManager.Instance.Register(callback.OnPing);
                return _callbacks.Remove(callback);
            }
        }

        private void OnLoginComplete(object sender, SessionEventArgs e)
        {
            try
            {
                if (e.Status == Error.OK)
                {
                    ThreadPool.QueueUserWorkItem(
                        state => Execute(
                            callback =>
                            callback.OnLoggedIn()));
                }
                else
                {
                    ThreadPool.QueueUserWorkItem(
                        state => Execute(
                            callback =>
                            callback.OnLoginError(e.Status.GetMessage())));
                }
            }
            catch (Exception exception)
            {
                _log.Error(exception.Message, exception);
            }
        }

        private void OnLogoutComplete(object sender, SessionEventArgs e)
        {
            try
            {
                ThreadPool.QueueUserWorkItem(
                    state => Execute(
                        callback =>
                        callback.OnLoggedOut()));
            }
            catch (Exception exception)
            {
                _log.Error(exception.Message, exception);
            }
        }

        private void Execute(Action<ILoginCallback> action)
        {
            var faultedClients = new List<ILoginCallback>();

            lock (_callbacksLock)
            {
                foreach (var client in _callbacks)
                {
                    try
                    {
                        action(client);
                    }
                    catch (CommunicationException)
                    {
                        ((ICommunicationObject)client).Abort();
                        faultedClients.Add(client);
                    }
                    catch (Exception)
                    {
                        ((ICommunicationObject)client).Abort();
                        faultedClients.Add(client);
                    }
                }
            }

            foreach (var faultedClient in faultedClients)
            {
                Unregister(faultedClient);
            }
        }
        #endregion Methods
    }
}