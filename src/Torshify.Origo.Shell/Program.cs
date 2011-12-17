using System;
using System.ServiceModel;
using Torshify.Origo.Shell.LoginService;
using Torshify.Origo.Shell.PlayerControlService;
using Torshify.Origo.Shell.PlaylistPlayerService;
using Track = Torshify.Origo.Shell.PlayerControlService.Track;

namespace Torshify.Origo.Shell
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ReadLine();

            var login = new LoginServiceClient(new InstanceContext(new MyLoginCallbacks()));
            login.Login

            var control = new PlayerControlServiceClient(new InstanceContext(new MyPlayerControlCallbacks()));
            control.Subscribe();

            var player = new PlaylistPlayerServiceClient();
            player.Initialize(new[]
                                {
                                    "spotify:track:2lvILTIWBbzFeHF95zSWoF",
                                    "spotify:track:50JVjWk5JwoJsIQLcqHftd"
                                });

            control.SetVolume(0.4f);
            Console.ReadLine();
        }

        public class MyLoginCallbacks : LoginServiceCallback
        {
            public void OnLoggedIn()
            {
                Console.WriteLine("Logged in");
            }

            public void OnLoginError(string message)
            {
                Console.WriteLine("Loggin error : " + message);
            }

            public void OnLoggedOut()
            {
                Console.WriteLine("Logged out");
            }

            public void OnPing()
            {
            }
        }

        public class MyPlayerControlCallbacks : PlayerControlServiceCallback
        {
            public void OnTrackChanged(Track track)
            {
                Console.WriteLine("Track changed to " + track.Name);
            }

            public void OnTrackComplete(Track track)
            {
                Console.WriteLine("Track complete (" + track.Name + ")");
            }

            public void OnElapsed(double elapsedMs, double totalMs)
            {
                
            }

            public void OnPlayStateChanged(bool isPlaying)
            {
                
            }

            public void OnVolumeChanged(float volume)
            {
                Console.WriteLine("Volume changed: " + volume);
            }

            public void OnPing()
            {
                
            }
        }
    }
}
