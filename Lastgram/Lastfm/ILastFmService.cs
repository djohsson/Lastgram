using System.Threading.Tasks;

namespace Lastgram.Lastfm
{
    public interface ILastFmService
    {
        Task<LastfmTrackResponse> GetNowPlayingAsync(string username);
    }
}
