using System.Threading.Tasks;

namespace Lastgram.Data
{
    public interface ISpotifyTrackRepository
    {
        Task AddSpotifyTrackAsync(string artistAndName, string url);

        Task<string> TryGetSpotifyTrackUrlAsync(string artistAndName);
    }
}
