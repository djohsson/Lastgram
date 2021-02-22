using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lastgram
{
    public interface ISpotifyService
    {
        Task<SpotifySearchResponse> TryGetLinkToTrackAsync(string track);
    }
}
