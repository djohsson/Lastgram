using Core.Domain.Models.Lastfm;
using Core.Domain.Services.Lastfm;
using Core.Domain.Services.Spotify;
using Lastgram.Utils;
using System;
using System.Collections.Generic;
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

            IReadOnlyList<LastfmTrack> topTracks = await lastfmService.GetTopTracksAsync(lastfmUsername);

            if (topTracks.Count == 0)
            {
                throw new CommandException($"Could not retrieve top tracks for <i>{lastfmUsername}</i>");
            }

            string response = await GetResponseAsync(lastfmUsername, topTracks);

            await responseFunc(message.Chat, response);
        }

        private async Task<string> GetResponseAsync(string lastfmUsername, IReadOnlyList<LastfmTrack> topTracks)
        {
            string response = $"<i>{lastfmUsername}'s</i> top tracks for the week:\n";

            foreach (var topTrack in topTracks)
            {
                var url = await spotifyService.TryGetLinkToTrackAsync(topTrack.ArtistName, topTrack.Name);

                response += ResponseHelper.GetResponseForTrack(topTrack, url);
                response += "\n\n";
            }

            return response;
        }
    }
}
