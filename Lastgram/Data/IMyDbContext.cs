using Lastgram.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace Lastgram.Data
{
    public interface IMyDbContext
    {
        DatabaseFacade Database { get; }

        DbSet<User> Users { get; set; }

        DbSet<SpotifyTrack> SpotifyTracks { get; set; }

        DbSet<Artist> Artists { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
