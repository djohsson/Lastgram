using Lastgram.Models;
using System.Threading.Tasks;

namespace Lastgram.Data
{
    public interface IArtistService
    {
        Task<Artist> GetOrAddArtistAsync(string artist);
    }
}
