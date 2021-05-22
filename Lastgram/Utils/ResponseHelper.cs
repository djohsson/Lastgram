using IF.Lastfm.Core.Objects;
using System.Text.RegularExpressions;
using System.Web;

namespace Lastgram.Utils
{
    public static class ResponseHelper
    {
        public static string GetResponseForTrack(LastTrack track, string url)
        {
            var artistAndTrack = HttpUtility.HtmlEncode($"{track.ArtistName} - {track.Name}");
            string encodedLastfmUrl = Regex.Replace(track.Url.AbsoluteUri, "([\"])", @"\$1");

            string response = $"🎵 <b>{artistAndTrack}</b>\n";

            response += "🔗 ";

            if (!string.IsNullOrEmpty(url))
            {
                response += $"<a href =\"{url}\">Spotify</a> | ";
            }

            response += $"<a href =\"{encodedLastfmUrl}\">Lastfm</a>";
            return response;
        }
    }
}
