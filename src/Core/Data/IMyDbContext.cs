using Core.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Data
{
    public interface IMyDbContext : IDisposable
    {
        DatabaseFacade Database { get; }

        DbSet<User> Users { get; set; }

        DbSet<SpotifyTrack> SpotifyTracks { get; set; }

        DbSet<Artist> Artists { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
