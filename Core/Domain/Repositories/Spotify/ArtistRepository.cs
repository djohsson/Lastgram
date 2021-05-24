using Core.Data;
using Core.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Core.Domain.Repositories.Spotify
{
    public class ArtistRepository : IArtistRepository
    {
        private readonly IMyDbContext context;

        public ArtistRepository(IMyDbContext context)
        {
            this.context = context;
        }

        public async Task<Artist> AddArtistAsync(string artistName)
        {
            var artist = await context.Artists.FirstOrDefaultAsync(a => a.Name.Equals(artistName));

            if (artist != null)
            {
                return artist;
            }

            artist = new Artist() { Name = artistName };

            await context.Artists.AddAsync(artist);
            await context.SaveChangesAsync();

            return artist;
        }

        public async Task<Artist> TryGetArtistAsync(string artistName)
        {
            return await context.Artists.FirstOrDefaultAsync(a => a.Name.Equals(artistName));
        }
    }
}
