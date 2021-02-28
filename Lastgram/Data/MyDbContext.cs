using Lastgram.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace Lastgram.Data
{
    public class MyDbContext : DbContext, IMyDbContext, IDisposable
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
#if DEBUG
            optionsBuilder.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddSimpleConsole()));
#endif
        }

        public DbSet<User> Users { get; set; }

        public DbSet<SpotifyTrack> SpotifyTracks { get; set; }
    }
}
