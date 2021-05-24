using Core.Data.Models;
using System.Threading.Tasks;

namespace Core.Domain.Services.Spotify
{
    public interface IArtistService
    {
        Task<Artist> GetOrAddArtistAsync(string artist);
    }
}
