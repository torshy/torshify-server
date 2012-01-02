using System;
using System.ComponentModel;
using System.ServiceModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

using Torshify.Origo.Contracts.V1;
using Torshify.Origo.Embedded.LoginService;
using Torshify.Origo.Embedded.QueryService;
using Torshify.Origo.Embedded.TrackPlayerService;

namespace Torshify.Origo.Embedded
{
    public class MainWindowViewModel : INotifyPropertyChanged, LoginServiceCallback
    {
        #region Fields

        private string _loginStatus;
        private ICollectionView _searchResults;

        #endregion Fields

        #region Constructors

        public MainWindowViewModel()
        {
            Login();
            SearchCommand = new SimpleCommand<string>(ExecuteSearch);
            PlayCommand = new SimpleCommand<Track>(ExecutePlay);
        }

        #endregion Constructors

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

        #region Properties

        public string LoginStatus
        {
            get { return _loginStatus; }
            set
            {
                if (_loginStatus != value)
                {
                    _loginStatus = value;
                    OnPropertyChanged("LoginStatus");
                }
            }
        }

        public ICommand PlayCommand
        {
            get;
            private set;
        }

        public ICommand SearchCommand
        {
            get;
            private set;
        }

        public ICollectionView SearchResults
        {
            get { return _searchResults; }
            set
            {
                if (_searchResults != value)
                {
                    _searchResults = value;
                    OnPropertyChanged("SearchResults");
                }
            }
        }

        #endregion Properties

        #region Methods

        void LoginServiceCallback.OnLoggedIn()
        {
            LoginStatus = "Logged in";
        }

        void LoginServiceCallback.OnLoggedOut()
        {
            LoginStatus = "Logged out";
        }

        void LoginServiceCallback.OnLoginError(string message)
        {
            LoginStatus = message;
        }

        void LoginServiceCallback.OnPing()
        {
        }

        private void ExecutePlay(Track track)
        {
            try
            {
                TrackPlayerServiceClient trackPlayer = new TrackPlayerServiceClient();
                trackPlayer.Play(track.ID);
                trackPlayer.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void ExecuteSearch(string searchString)
        {
            QueryServiceClient query = null;

            try
            {
                query = new QueryServiceClient();
                var result = query.Query(searchString, 0, 10, 0, 10, 0, 10);
                query.Close();
                SearchResults = CollectionViewSource.GetDefaultView(result.Tracks);
            }
            catch (Exception e)
            {
                if (query != null)
                {
                    query.Abort();
                }

                MessageBox.Show(e.Message);
            }
        }

        private void Login()
        {
            try
            {
                var args = Environment.GetCommandLineArgs();
                var userName = args[1];
                var password = args[2];
                var login = new LoginServiceClient(new InstanceContext(this));

                login.Subscribe();
                login.Login(userName, password, false);
            }
            catch (Exception e)
            {
                LoginStatus = e.Message;
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion Methods

        #region Nested Types

        private class SimpleCommand<T> : ICommand
        {
            #region Fields

            private readonly Func<T, bool> _canExecute;
            private readonly Action<T> _execute;

            #endregion Fields

            #region Constructors

            public SimpleCommand(Action<T> execute, Func<T, bool> canExecute = null)
            {
                _execute = execute;
                _canExecute = canExecute;
            }

            #endregion Constructors

            #region Events

            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            #endregion Events

            #region Methods

            public bool CanExecute(object parameter)
            {
                if (_canExecute != null)
                {
                    return _canExecute((T) parameter);
                }

                return true;
            }

            public void Execute(object parameter)
            {
                _execute((T) parameter);
            }

            #endregion Methods
        }

        #endregion Nested Types
    }
}