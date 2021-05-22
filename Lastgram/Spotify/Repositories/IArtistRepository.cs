using Lastgram.Data.Models;
using System.Threading.Tasks;

namespace Lastgram.Spotify.Repositories
{
    public interface IArtistRepository
    {
        Task<Artist> TryGetArtistAsync(string artistName);

        Task<Artist> AddArtistAsync(string artistName);
    }
}
