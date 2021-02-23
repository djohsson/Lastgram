using System.Threading.Tasks;

namespace Lastgram.Data
{
    public interface ISpotifyTrackRepository
    {
        Task AddSpotifyTrackAsync(string artist, string track, string url);

        Task<string> TryGetSpotifyTrackUrlAsync(string artist, string track);
    }
}
