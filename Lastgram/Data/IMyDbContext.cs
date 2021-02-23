using Lastgram.Models;
using Microsoft.EntityFrameworkCore;

namespace Lastgram.Data
{
    public interface IMyDbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<SpotifyTrack> SpotifyTracks { get; set; }
    }
}
