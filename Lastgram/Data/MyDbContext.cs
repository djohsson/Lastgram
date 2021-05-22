using Lastgram.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace Lastgram.Data
{
    public class MyDbContext : DbContext, IMyDbContext, IDisposable
    {
        /// <summary>
        /// Only used for creating migrations
        /// </summary>
        public MyDbContext()
            : base(new DbContextOptionsBuilder<MyDbContext>()
                .UseNpgsql(Environment.GetEnvironmentVariable("LASTGRAM_CONNECTIONSTRING"))
                .Options)
        {
        }

        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
#if DEBUG
            optionsBuilder
                .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddSimpleConsole()));
#endif
        }

        public DbSet<User> Users { get; set; }

        public DbSet<SpotifyTrack> SpotifyTracks { get; set; }

        public DbSet<Artist> Artists { get; set; }
    }
}
