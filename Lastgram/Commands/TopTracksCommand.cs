using Lastgram.Lastfm;
using Lastgram.Spotify;
using Lastgram.Utils;
using System;
using System.Threading.Tasks;
using System.Web;
using Telegram.Bot.Types;

namespace Lastgram.Commands
{
    public class TopTracksCommand : ICommand
    {
        private readonly ILastfmService lastfmService;
        private readonly ILastfmUsernameService lastfmUsernameService;
        private readonly ISpotifyService spotifyService;

        public TopTracksCommand(
            ILastfmService lastfmService,
            ILastfmUsernameService lastfmUsernameService,
            ISpotifyService spotifyService)
        {
            this.lastfmService = lastfmService;
            this.lastfmUsernameService = lastfmUsernameService;
            this.spotifyService = spotifyService;
        }

        public string CommandName => "toptracks";

        public string CommandDescription => "Retrieve top tracks for the week";

        public async Task ExecuteCommandAsync(Message message, Func<Chat, string, Task> responseFunc)
        {
            var lastfmUsername = await lastfmUsernameService.TryGetUsernameAsync(message.From.Id);
            lastfmUsername = HttpUtility.HtmlEncode(lastfmUsername);

            if (string.IsNullOrEmpty(lastfmUsername))
            {
                throw new CommandException("No username set 😢");
            }

            LastfmTopTracksResponse topTracksResponse = await lastfmService.GetTopTracksAsync(lastfmUsername);

            if (!topTracksResponse.IsSuccess)
            {
                throw new CommandException($"Could not retrieve top tracks for <i>{lastfmUsername}</i>");
            }

            string response = await GetResponseAsync(lastfmUsername, topTracksResponse);

            await responseFunc(message.Chat, response);
        }

        private async Task<string> GetResponseAsync(string lastfmUsername, LastfmTopTracksResponse topTracksResponse)
        {
            string response = $"<i>{lastfmUsername}'s</i> top tracks for the week:\n";

            foreach (var topTrack in topTracksResponse.TopTracks)
            {
                var url = await spotifyService.TryGetLinkToTrackAsync(topTrack.ArtistName, topTrack.Name);

                response += ResponseHelper.GetResponseForTrack(topTrack, url);
                response += "\n\n";
            }

            return response;
        }
    }
}
