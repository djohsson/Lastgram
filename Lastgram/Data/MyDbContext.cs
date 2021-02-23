using Lastgram.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Lastgram.Data
{
    public class MyDbContext : DbContext, IMyDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(Environment.GetEnvironmentVariable("LASTGRAM_CONNECTIONSTRING"));
        }

        public DbSet<User> Users { get; set; }

        public DbSet<SpotifyTrack> SpotifyTracks { get; set; }
    }
}
