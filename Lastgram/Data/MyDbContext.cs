using Lastgram.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Lastgram.Data
{
    public class MyDbContext : DbContext, IMyDbContext, IDisposable
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<SpotifyTrack> SpotifyTracks { get; set; }
    }
}
