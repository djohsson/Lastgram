using Lastgram.Data;
using Lastgram.Lastfm;
using Lastgram.Spotify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Telegram.Bot.Types;

namespace Lastgram.Commands
{
    public class NowPlayingService : INowPlayingService
    {
        private readonly IUserRepository userRepository;
        private readonly ILastFmService lastFmService;
        private readonly ISpotifyService spotifyService;

        public NowPlayingService(IUserRepository userRepository, ILastFmService lastFmService, ISpotifyService spotifyService)
        {
            this.userRepository = userRepository;
            this.lastFmService = lastFmService;
            this.spotifyService = spotifyService;
        }

        public async Task HandleCommandAsync(Message message, Func<Chat, string, Task> responseFunc)
        {
            string lastFmUsername;
            List<string> parameters = message.GetParameters();

            if (!parameters.Any())
            {
                // User did not provide Last.fm username. Try fetching one from the repository
                if (!userRepository.TryGetUser(message.From.Id, out lastFmUsername))
                {
                    lastFmUsername = message.From.Username;

                    userRepository.AddUser(message.From.Id, lastFmUsername);
                }
            }
            else if (parameters.Count == 1)
            {
                // User has provided a Last.fm username
                lastFmUsername = parameters.First();

                userRepository.AddUser(message.From.Id, lastFmUsername);
            }
            else if (parameters.Count == 2 && parameters.Last().ToLowerInvariant().Equals("temp"))
            {
                // User has provided a temporary Last.fm username
                lastFmUsername = parameters.First();
            }
            else
            {
                // Invalid input
                return;
            }

            var track = await lastFmService.GetNowPlayingAsync(lastFmUsername);
            string response;

            if (!track.Success)
            {
                lastFmUsername = HttpUtility.HtmlEncode(lastFmUsername);

                response = $"Could not find <i>{lastFmUsername}</i> on last.fm";
            }
            else
            {
                var spotifySearchResponse = await spotifyService.TryGetLinkToTrackAsync($"{track.Track.ArtistName} - {track.Track.Name}");

                response = GetResponseMessage(message, track, spotifySearchResponse);
            }

            await responseFunc(message.Chat, response);
        }

        private static string GetResponseMessage(Message message, LastfmTrackResponse track, SpotifySearchResponse spotifySearchResponse)
        {
            string response;
            string artistAndName = HttpUtility.HtmlEncode($"{track.Track.ArtistName} - {track.Track.Name}");
            string username = HttpUtility.HtmlEncode(message.From.Username);

            if (spotifySearchResponse.Success && (track.Track.IsNowPlaying ?? false))
            {
                response = $"{username} is currently playing\n<a href=\"{spotifySearchResponse.Url}\"><b>{artistAndName}</b></a>";
            }
            else if (spotifySearchResponse.Success)
            {
                response = $"{username} played\n<a href=\"{spotifySearchResponse.Url}\"><b>{artistAndName}</b></a>\non {track.Track.TimePlayed?.DateTime}";
            }
            else
            {
                response = $"{username} played\n<b>{artistAndName}</b>\non {track.Track.TimePlayed?.DateTime}";
            }

            return response;
        }
    }
}
