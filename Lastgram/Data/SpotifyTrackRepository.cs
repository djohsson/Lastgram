using Lastgram.Models;
using System;
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

        public async Task AddSpotifyTrackAsync(string artistAndName, string url)
        {
            var track = new SpotifyTrack
            {
                Md5 = Hasher.CreateMD5(artistAndName),
                Url = url,
                ValidUntil = DateTime.UtcNow.AddDays(30)
            };

            await context.SpotifyTracks.AddAsync(track);

            await context.SaveChangesAsync();
        }

        public async Task<string> TryGetSpotifyTrackUrlAsync(string artistAndName)
        {
            var md5 = Hasher.CreateMD5(artistAndName);

            var track = await context.SpotifyTracks.FindAsync(md5);

            if (track == null)
            {
                return null;
            }

            if (track.ValidUntil < DateTime.UtcNow)
            {
                context.SpotifyTracks.Remove(track);

                await context.SaveChangesAsync();

                return null;
            }

            return track.Url;
        }
    }
}
