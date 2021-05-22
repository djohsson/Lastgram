using Lastgram.Data.Models;
using System.Threading.Tasks;

namespace Lastgram.Spotify.Repositories
{
    public interface ISpotifyTrackRepository
    {
        Task AddSpotifyTrackAsync(Artist artist, string track, string url);

        Task<string> TryGetSpotifyTrackUrlAsync(string artist, string track);
    }
}
