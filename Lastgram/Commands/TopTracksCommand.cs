using Lastgram.Data.Repositories;
using Lastgram.Lastfm;
using Lastgram.Response;
using Lastgram.Spotify;
using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Lastgram.Commands
{
    public class TopTracksCommand : ICommand
    {
        private readonly ILastfmService lastfmService;
        private readonly IUserRepository userRepository;
        private readonly ISpotifyService spotifyService;
        private readonly ITrackResponseService trackResponseService;

        public TopTracksCommand(
            ILastfmService lastfmService,
            IUserRepository userRepository,
            ISpotifyService spotifyService,
            ITrackResponseService trackResponseService)
        {
            this.lastfmService = lastfmService;
            this.userRepository = userRepository;
            this.spotifyService = spotifyService;
            this.trackResponseService = trackResponseService;
        }

        public string CommandName => "toptracks";

        public string CommandDescription => "Retrieve top tracks for the week";

        public async Task ExecuteCommandAsync(Message message, Func<Chat, string, Task> responseFunc)
        {
            var lastfmUsername = await userRepository.TryGetUserAsync(message.From.Id);

            if (string.IsNullOrEmpty(lastfmUsername))
            {
                await responseFunc(message.Chat, "No username set 😢");
                return;
            }

            LastfmTopTracksResponse topTracksResponse = await lastfmService.GetTopTracksAsync(lastfmUsername);

            if (!topTracksResponse.IsSuccess)
            {
                await responseFunc(message.Chat, $"Could not retrieve top tracks for <i>{lastfmUsername}</i>");
                return;
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

                response += trackResponseService.GetResponseForTrack(topTrack, url);
                response += "\n\n";
            }

            return response;
        }
    }
}
