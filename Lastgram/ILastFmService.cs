using IF.Lastfm.Core.Objects;
using System.Threading.Tasks;

namespace Lastgram
{
    public interface ILastFmService
    {
        Task<LastfmTrackResponse> GetNowPlayingAsync(string username);
    }
}
