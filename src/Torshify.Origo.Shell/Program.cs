using System;
using Torshify.Origo.Shell.PlaylistPlayerService;
using Torshify.Origo.Shell.QueryService;

namespace Torshify.Origo.Shell
{
    class Program
    {
        static void Main(string[] args)
        {
            //PrintArtistAlbums("spotify:artist:2CIMQHirSU0MQqyYHq0eOx");
            PlaylistPlayerServiceClient player = new PlaylistPlayerServiceClient();
            player.Initialize("spotify:track:2lvILTIWBbzFeHF95zSWoF");
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
    }
}
