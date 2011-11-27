using System;
using System.Collections.Generic;
using System.Timers;

namespace Torshify.Origo.Services
{
    public class KeepAliveManager
    {
        #region Fields

        private static readonly KeepAliveManager _instance = new KeepAliveManager();

        private List<Action> _callbacks;
        private Timer _keepAliveTimer;
        private object _lock = new object();

        #endregion Fields

        #region Constructors

        private KeepAliveManager()
        {
            _callbacks = new List<Action>();
            _keepAliveTimer = new Timer(10000);
            _keepAliveTimer.Elapsed += OnKeepAliveTimerElapsed;
            _keepAliveTimer.Start();
        }

        #endregion Constructors

        #region Properties

        public static KeepAliveManager Instance
        {
            get
            {
                return _instance;
            }
        }

        #endregion Properties

        #region Methods

        public void Register(Action action)
        {
            lock (_lock)
            {
                _callbacks.Add(action);
            }
        }

        public void Unregister(Action action)
        {
            lock (_lock)
            {
                _callbacks.Remove(action);
            }
        }

        private void OnKeepAliveTimerElapsed(object sender, ElapsedEventArgs e)
        {
            lock (_lock)
            {
                for (int i = 0; i < _callbacks.Count; i++)
                {
                    var callback = _callbacks[i];

                    try
                    {
                        callback();
                    }
                    catch
                    {
                        _callbacks.Remove(callback);
                        i--;
                    }
                }
            }
        }

        #endregion Methods
    }
}