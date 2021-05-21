using System.Threading.Tasks;

namespace Lastgram.Lastfm
{
    public interface ILastfmService
    {
        Task<LastfmTrackResponse> GetNowPlayingAsync(string username);

        Task<LastfmTopTracksResponse> GetTopTracksAsync(string username);
    }
}
