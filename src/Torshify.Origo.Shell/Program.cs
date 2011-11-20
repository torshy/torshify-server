using System;
using System.ServiceModel;
using Torshify.Origo.Shell.PlayerControlService;
using Torshify.Origo.Shell.PlaylistPlayerService;
using Torshify.Origo.Shell.QueryService;
using Track = Torshify.Origo.Shell.PlayerControlService.Track;

namespace Torshify.Origo.Shell
{
    class Program
    {
        static void Main(string[] args)
        {
            //PrintArtistAlbums("spotify:artist:2CIMQHirSU0MQqyYHq0eOx");

            PlayerControlServiceClient control = new PlayerControlServiceClient(new InstanceContext(new MyPlayerControlCallbacks()));
            control.Subscribe();

            PlaylistPlayerServiceClient player = new PlaylistPlayerServiceClient();
            player.Initialize(new[]
                                  {
                                      "spotify:track:2lvILTIWBbzFeHF95zSWoF",
                                      "spotify:track:50JVjWk5JwoJsIQLcqHftd"
                                  });

            control.SetVolume(0.01f);
            Console.ReadLine();
        }

        private static void PrintArtistAlbums(string artistLink)
        {
            QueryServiceClient query = new QueryServiceClient();
            var result = query.ArtistBrowse(artistLink, ArtistBrowsingType.NoTracks);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(result.Artist.Name);
            Console.ForegroundColor = ConsoleColor.Gray;

            foreach (var album in result.Albums)
            {
                Console.WriteLine(album.Name);
            }

            query.Close();
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
        }
    }
}
