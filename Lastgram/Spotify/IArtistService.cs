using Lastgram.Data.Models;
using System.Threading.Tasks;

namespace Lastgram.Spotify
{
    public interface IArtistService
    {
        Task<Artist> GetOrAddArtistAsync(string artist);
    }
}
