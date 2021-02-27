using Lastgram.Models;
using Lastgram.Utils;
using System.Threading.Tasks;

namespace Lastgram.Data
{
    public class SpotifyTrackRepository : ISpotifyTrackRepository
    {
        private readonly IMyDbContext context;

        public SpotifyTrackRepository(IMyDbContext context)
        {
            this.context = context;
        }

        public async Task AddSpotifyTrackAsync(string artist, string track, string url)
        {
            if (string.IsNullOrEmpty(url) || (string.IsNullOrEmpty(artist) && string.IsNullOrEmpty(track)))
            {
                return;
            }

            string artistAndName = FormatArtistAndTrack(artist, track);
            string formattedArtist = artist.Length > 255 ? artist.Substring(0, 255) : artist;
            string formattedTrack = track.Length > 255 ? track.Substring(0, 255) : track;

            var spotifyTrack = new SpotifyTrack
            {
                Md5 = Hasher.CreateMD5(artistAndName),
                Url = url,
                Artist = formattedArtist,
                Track = formattedTrack
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

            return spotifyTrack.Url;
        }

        private static string FormatArtistAndTrack(string artist, string track)
        {
            return $"{artist} - {track}";
        }
    }
}
