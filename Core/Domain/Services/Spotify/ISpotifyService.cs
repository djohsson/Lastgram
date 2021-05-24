using System.Threading.Tasks;

namespace Core.Domain.Services.Spotify
{
    public interface ISpotifyService
    {
        Task<string> TryGetLinkToTrackAsync(string artist, string track);
    }
}
