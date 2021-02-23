using Lastgram.Models;
using Microsoft.EntityFrameworkCore;

namespace Lastgram.Data
{
    public class MyDbContext : DbContext, IMyDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(@"Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=mysecretpassword");
        }

        public DbSet<User> Users { get; set; }

        public DbSet<SpotifyTrack> SpotifyTracks { get; set; }
    }
}
