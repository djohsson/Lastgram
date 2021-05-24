using Core.Data.Models;
using System.Threading.Tasks;

namespace Core.Domain.Repositories.Spotify
{
    public interface IArtistRepository
    {
        Task<Artist> TryGetArtistAsync(string artistName);

        Task<Artist> AddArtistAsync(string artistName);
    }
}
