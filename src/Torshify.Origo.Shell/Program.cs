using System;
using System.ServiceModel;
using System.Threading;

using Torshify.Origo.Shell.LoginService;
using Torshify.Origo.Shell.PlayerControlService;
using Torshify.Origo.Shell.QueryService;

using Track = Torshify.Origo.Shell.PlayerControlService.Track;

namespace Torshify.Origo.Shell
{
    class Program : LoginServiceCallback
    {
        #region Fields

        private ManualResetEventSlim _loginEvent;

        #endregion Fields

        #region Methods

        void LoginServiceCallback.OnLoggedIn()
        {
            Console.WriteLine("Logged in");
            _loginEvent.Set();
        }

        void LoginServiceCallback.OnLoggedOut()
        {
            Console.WriteLine("Logged out");
        }

        void LoginServiceCallback.OnLoginError(string message)
        {
            Console.WriteLine("Loggin error : " + message);
        }

        void LoginServiceCallback.OnPing()
        {
        }

        public void Run(string userName, string password)
        {
            Thread.Sleep(4000);

            _loginEvent = new ManualResetEventSlim();

            var login = new LoginServiceClient(new InstanceContext(this));
            login.Subscribe();
            
            login.Login(userName, password, false);
            Console.WriteLine("Logging in...");
            if (!_loginEvent.Wait(10000))
            {
                Console.WriteLine("Unable to login");
                return;
            }

            var query = new QueryServiceClient();
            Console.WriteLine("Searching");

            var result = query.Query("NOFX", 0, 10, 0, 10, 0, 10);

            foreach (var track in result.Tracks)
            {
                Console.WriteLine(track.Name);
            }

            foreach (var album in result.Albums)
            {
                Console.WriteLine(album.Name);
            }

            //var control = new PlayerControlServiceClient(new InstanceContext(new MyPlayerControlCallbacks()));
            //control.Subscribe();

            //var player = new PlaylistPlayerServiceClient();
            //player.Initialize(new[]
            //                    {
            //                        "spotify:track:2lvILTIWBbzFeHF95zSWoF",
            //                        "spotify:track:50JVjWk5JwoJsIQLcqHftd"
            //                    });

            //control.SetVolume(0.4f);
            Console.ReadLine();
        }

        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                return;
            }

            new Program().Run(args[0], args[1]);
        }

        #endregion Methods

        #region Nested Types

        public class MyPlayerControlCallbacks : PlayerControlServiceCallback
        {
            #region Methods

            public void OnElapsed(double elapsedMs, double totalMs)
            {
            }

            public void OnPing()
            {
            }

            public void OnPlayStateChanged(bool isPlaying)
            {
            }

            public void OnTrackChanged(Track track)
            {
                Console.WriteLine("Track changed to " + track.Name);
            }

            public void OnTrackComplete(Track track)
            {
                Console.WriteLine("Track complete (" + track.Name + ")");
            }

            public void OnVolumeChanged(float volume)
            {
                Console.WriteLine("Volume changed: " + volume);
            }

            #endregion Methods
        }

        #endregion Nested Types
    }
}