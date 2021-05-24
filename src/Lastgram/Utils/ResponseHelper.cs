using Core.Domain.Models.Lastfm;
using System.Text.RegularExpressions;
using System.Web;

namespace Lastgram.Utils
{
    internal static class ResponseHelper
    {
        public static string GetResponseForTrack(LastfmTrack track, string spotifyUrl)
        {
            var artistAndTrack = HttpUtility.HtmlEncode($"{track.ArtistName} - {track.Name}");
            string encodedLastfmUrl = Regex.Replace(track.Url.AbsoluteUri, "([\"])", @"\$1");

            string response = $"🎵 <b>{artistAndTrack}</b>\n";

            response += "🔗 ";

            if (!string.IsNullOrEmpty(spotifyUrl))
            {
                response += $"<a href =\"{spotifyUrl}\">Spotify</a> | ";
            }

            response += $"<a href =\"{encodedLastfmUrl}\">Lastfm</a>";
            return response;
        }
    }
}
