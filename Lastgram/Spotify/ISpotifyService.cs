using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lastgram.Spotify
{
    public interface ISpotifyService
    {
        Task<SpotifySearchResponse> TryGetLinkToTrackAsync(string track);
    }
}
