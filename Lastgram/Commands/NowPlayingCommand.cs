using Lastgram.Lastfm;
using Lastgram.Spotify;
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

            var track = await lastfmService.GetNowPlayingAsync(lastfmUsername);

            if (!track.IsSuccess)
            {
                string encodedUsername = HttpUtility.HtmlEncode(lastfmUsername);

                throw new CommandException($"Could not find <i>{encodedUsername}</i> on last.fm 😢");
            }

            var url = await spotifyService.TryGetLinkToTrackAsync(track.Track.ArtistName, track.Track.Name);

            string response = GetResponseMessage(lastfmUsername, track, url);

            await responseFunc(message.Chat, response);
        }

        private static string GetResponseMessage(string lastfmUsername, LastfmTrackResponse track, string url)
        {
            string response;
            string encodedUsername = HttpUtility.HtmlEncode(lastfmUsername);

            if (track.Track.IsNowPlaying ?? false)
            {
                response = $"<i>{encodedUsername} is currently playing</i>\n";
            }
            else
            {
                response = $"<i>{encodedUsername} played</i>\n";
            }

            response += ResponseHelper.GetResponseForTrack(track.Track, url);
            response += "\n";

            if (!track.Track.IsNowPlaying ?? true)
            {
                response += $"<i>on {GetTimePlayed(track)}</i>";
            }

            return response;
        }

        private static string GetTimePlayed(LastfmTrackResponse track)
            => track.Track.TimePlayed?.DateTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");
    }
}
