using Core.Data.Models;
using System.Threading.Tasks;

namespace Core.Domain.Repositories.Spotify
{
    public interface ISpotifyTrackRepository
    {
        Task AddSpotifyTrackAsync(Artist artist, string track, string url);

        Task<string> TryGetSpotifyTrackUrlAsync(string artist, string track);
    }
}
