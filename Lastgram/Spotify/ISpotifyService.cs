using System.Threading.Tasks;

namespace Lastgram.Spotify
{
    public interface ISpotifyService
    {
        Task<string> TryGetLinkToTrackAsync(string artist, string track);
    }
}
