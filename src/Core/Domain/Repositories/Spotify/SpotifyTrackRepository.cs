using Core.Data;
using Core.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Core.Domain.Repositories.Spotify
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

            string formattedTrack = track.Length > 255
                ? track.Substring(0, 255)
                : track;

            var existingTrack = await context.SpotifyTracks.FirstOrDefaultAsync(t =>
                t.Track == formattedTrack &&
                t.Artist.Name == artist.Name);

            if (existingTrack != null)
            {
                return;
            }

            var spotifyTrack = new SpotifyTrack
            {
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
            string formattedTrack = track.Length > 255
                ? track.Substring(0, 255)
                : track;

            var spotifyTrack = await context.SpotifyTracks
                .FirstOrDefaultAsync(t =>
                    t.Track == formattedTrack
                    && t.Artist.Name == artist);

            if (spotifyTrack == null)
            {
                return null;
            }

            return spotifyTrack.Url;
        }
    }
}
