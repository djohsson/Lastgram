using Lastgram.Data;
using Lastgram.Data.Models;
using Lastgram.Utils;
using System;
using System.Threading.Tasks;

namespace Lastgram.Spotify.Repositories
{
    public class SpotifyTrackRepository : ISpotifyTrackRepository
    {
        private readonly IMyDbContext context;

        public SpotifyTrackRepository(IMyDbContext context)
        {
            this.context = context;
        }

        public async Task AddSpotifyTrackAsync(Artist artist, string track, string url)
        {
            if (artist == null || string.IsNullOrEmpty(url) || string.IsNullOrEmpty(track))
            {
                return;
            }

            string artistAndName = FormatArtistAndTrack(artist.Name, track);
            string formattedTrack = track.Length > 255 ? track.Substring(0, 255) : track;

            var spotifyTrack = new SpotifyTrack
            {
                Md5 = Hasher.CreateMD5(artistAndName),
                Url = url,
                Artist = artist,
                Track = formattedTrack
            };

            await context.SpotifyTracks.AddAsync(spotifyTrack);

            try
            {
                await context.SaveChangesAsync();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e);
            }
        }

        public async Task<string> TryGetSpotifyTrackUrlAsync(string artist, string track)
        {
            string artistAndName = FormatArtistAndTrack(artist, track);

            var md5 = Hasher.CreateMD5(artistAndName);

            var spotifyTrack = await context.SpotifyTracks.FindAsync(md5);

            if (spotifyTrack == null)
            {
                return null;
            }

            return spotifyTrack.Url;
        }

        private static string FormatArtistAndTrack(string artist, string track)
        {
            return $"{artist} - {track}";
        }
    }
}
