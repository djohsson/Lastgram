using Lastgram.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Lastgram.Data
{
    public class SpotifyTrackRepository : ISpotifyTrackRepository
    {
        private readonly MyDbContext context;

        public SpotifyTrackRepository(MyDbContext context)
        {
            this.context = context;
        }

        public async Task AddSpotifyTrackAsync(string artist, string track, string url)
        {
            string artistAndName = FormatArtistAndTrack(artist, track);
            string formattedArtist = artist.Length > 255 ? artist.Substring(0, 255) : artist;
            string formattedTrack = track.Length > 255 ? track.Substring(0, 255) : track;

            var spotifyTrack = new SpotifyTrack
            {
                Md5 = Hasher.CreateMD5(artistAndName),
                Url = url,
                Artist = formattedArtist,
                Track = formattedTrack,
                ValidUntil = DateTime.UtcNow.AddDays(30)
            };

            await context.SpotifyTracks.AddAsync(spotifyTrack);

            await context.SaveChangesAsync();
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

            if (spotifyTrack.ValidUntil < DateTime.UtcNow)
            {
                context.SpotifyTracks.Remove(spotifyTrack);

                await context.SaveChangesAsync();

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
