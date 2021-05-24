using Core.Domain.Models.Lastfm;
using Core.Domain.Services.Lastfm;
using Core.Domain.Services.Spotify;
using Lastgram.Utils;
using System;
using System.Threading.Tasks;
using System.Web;
using Telegram.Bot.Types;

namespace Lastgram.Commands
{
    public class NowPlayingCommand : ICommand
    {
        private readonly ILastfmService lastfmService;
        private readonly ILastfmUsernameService lastfmUsernameService;
        private readonly ISpotifyService spotifyService;

        public NowPlayingCommand(
            ILastfmService lastfmService,
            ISpotifyService spotifyService,
            ILastfmUsernameService lastfmUsernameService)
        {
            this.lastfmService = lastfmService;
            this.spotifyService = spotifyService;
            this.lastfmUsernameService = lastfmUsernameService;
        }

        public string CommandName => "np";

        public string CommandDescription => "Get the currently playing song for set last.fm user";

        public async Task ExecuteCommandAsync(Message message, Func<Chat, string, Task> responseFunc)
        {
            string lastfmUsername = await lastfmUsernameService.TryGetUsernameAsync(message.From.Id);

            if (string.IsNullOrEmpty(lastfmUsername))
            {
                throw new CommandException("Seems like you haven't registered a username 😢");
            }

            var latestScrobble = await lastfmService.GetLatestScrobbleAsync(lastfmUsername);

            if (latestScrobble == null)
            {
                string encodedUsername = HttpUtility.HtmlEncode(lastfmUsername);

                throw new CommandException($"Could not find <i>{encodedUsername}</i> on last.fm 😢");
            }

            var url = await spotifyService.TryGetLinkToTrackAsync(
                latestScrobble.LastfmTrack.ArtistName,
                latestScrobble.LastfmTrack.Name);

            string response = GetResponseMessage(lastfmUsername, latestScrobble, url);

            await responseFunc(message.Chat, response);
        }

        private static string GetResponseMessage(string lastfmUsername, LastfmScrobble lastfmScrobble, string spotifyUrl)
        {
            string response;
            string encodedUsername = HttpUtility.HtmlEncode(lastfmUsername);

            if (lastfmScrobble.IsNowPlaying)
            {
                response = $"<i>{encodedUsername} is currently playing</i>\n";
            }
            else
            {
                response = $"<i>{encodedUsername} played</i>\n";
            }

            response += ResponseHelper.GetResponseForTrack(lastfmScrobble.LastfmTrack, spotifyUrl);
            response += "\n";

            int? userPlayCount = lastfmScrobble.LastfmTrack.UserPlayCount;

            if (userPlayCount.HasValue)
            {
                response += $"🎧 {userPlayCount.Value}\n";
            }

            if (!lastfmScrobble.IsNowPlaying)
            {
                response += $"<i>on {GetTimePlayed(lastfmScrobble)}</i>";
            }

            return response;
        }

        private static string GetTimePlayed(LastfmScrobble track)
            => track.TimePlayed?.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");
    }
}
