using Lastgram.Models;
using System.Threading.Tasks;

namespace Lastgram.Data.Repositories
{
    public interface ISpotifyTrackRepository
    {
        Task AddSpotifyTrackAsync(Artist artist, string track, string url);

        Task<string> TryGetSpotifyTrackUrlAsync(string artist, string track);
    }
}
