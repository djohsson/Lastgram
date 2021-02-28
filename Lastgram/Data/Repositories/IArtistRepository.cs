using Lastgram.Models;
using System.Threading.Tasks;

namespace Lastgram.Data.Repositories
{
    public interface IArtistRepository
    {
        Task<Artist> TryGetArtistAsync(string artistName);

        Task<Artist> AddArtistAsync(string artistName);
    }
}
