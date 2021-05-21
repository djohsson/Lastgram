using IF.Lastfm.Core.Objects;
using Lastgram.Data.Repositories;
using Lastgram.Lastfm;
using Lastgram.Spotify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Telegram.Bot.Types;

namespace Lastgram.Commands
{
    public class TopTracksCommand : ICommand
    {
        private readonly ILastfmService lastfmService;
        private readonly IUserRepository userRepository;
        private readonly ISpotifyService spotifyService;

        public TopTracksCommand(ILastfmService lastfmService, IUserRepository userRepository, ISpotifyService spotifyService)
        {
            this.lastfmService = lastfmService;
            this.userRepository = userRepository;
            this.spotifyService = spotifyService;
        }

        public string CommandName => "toptracks";

        public string CommandDescription => "Retrieve top tracks for the week";

        public async Task ExecuteCommandAsync(Message message, Func<Chat, string, Task> responseFunc)
        {
            var lastfmUsername = await userRepository.TryGetUserAsync(message.From.Id);

            if (string.IsNullOrEmpty(lastfmUsername))
            {
                await responseFunc(message.Chat, "No username set");
                return;
            }

            LastfmTopTracksResponse topTracksResponse = await lastfmService.GetTopTracksAsync(lastfmUsername);

            if (!topTracksResponse.IsSuccess)
            {
                await responseFunc(message.Chat, $"Could not retrieve top tracks for <i>{lastfmUsername}</i>");
                return;
            }

            string response = await GetResponse(lastfmUsername, topTracksResponse);

            await responseFunc(message.Chat, response);
        }

        private async Task<string> GetResponse(string lastfmUsername, LastfmTopTracksResponse response)
        {
            string output = $"<i>{lastfmUsername}'s</i> top tracks for the week:\n";

            foreach (var topTrack in response.TopTracks)
            {
                var url = await spotifyService.TryGetLinkToTrackAsync(topTrack.ArtistName, topTrack.Name);
                var artistAndTrack = HttpUtility.HtmlEncode($"{topTrack.ArtistName} - {topTrack.Name}");
                string encodedLastfmUrl = Regex.Replace(topTrack.Url.AbsoluteUri, "([\"])", @"\$1");

                output += $"<b>{artistAndTrack}</b>\n";

                if (!string.IsNullOrEmpty(url))
                {
                    output += $"<a href =\"{url}\">Spotify</a> | ";
                }

                output += $"<a href =\"{encodedLastfmUrl}\">Lastfm</a>\n\n";
            }

            return output;
        }
    }
}
