using IF.Lastfm.Core.Objects;
using Lastgram.Spotify;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Lastgram.Response
{
    public class TrackResponseService : ITrackResponseService
    {
        private readonly ISpotifyService spotifyService;

        public TrackResponseService(ISpotifyService spotifyService)
        {
            this.spotifyService = spotifyService;
        }

        public string GetResponseForTrack(LastTrack topTrack, string url)
        {
            var artistAndTrack = HttpUtility.HtmlEncode($"{topTrack.ArtistName} - {topTrack.Name}");
            string encodedLastfmUrl = Regex.Replace(topTrack.Url.AbsoluteUri, "([\"])", @"\$1");

            string response = $"🎵 <b>{artistAndTrack}</b>\n";

            response += "🔗 ";

            if (!string.IsNullOrEmpty(url))
            {
                response += $"<a href =\"{url}\">Spotify</a> | ";
            }

            response += $"<a href =\"{encodedLastfmUrl}\">Lastfm</a>\n\n";
            return response;
        }
    }
}
