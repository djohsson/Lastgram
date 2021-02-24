using Lastgram.Data;
using Lastgram.Lastfm;
using Lastgram.Spotify;
using Lastgram.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Telegram.Bot.Types;

namespace Lastgram.Commands
{
    public class NowPlayingCommand : INowPlayingCommand
    {
        private readonly IUserRepository userRepository;
        private readonly ILastFmService lastFmService;
        private readonly ISpotifyService spotifyService;

        public NowPlayingCommand(IUserRepository userRepository, ILastFmService lastFmService, ISpotifyService spotifyService)
        {
            this.userRepository = userRepository;
            this.lastFmService = lastFmService;
            this.spotifyService = spotifyService;
        }

        public string CommandName => "np";

        public string CommandDescription => "/np [username [temp]]";

        public async Task ExecuteCommandAsync(Message message, Func<Chat, string, Task> responseFunc)
        {
            string lastfmUsername;
            List<string> parameters = message.GetParameters();

            if (!parameters.Any())
            {
                // User did not provide Last.fm username. Try fetching one from the repository
                lastfmUsername = await userRepository.TryGetUserAsync(message.From.Id);

                if (string.IsNullOrEmpty(lastfmUsername))
                {
                    lastfmUsername = string.IsNullOrEmpty(message.From.Username)
                        ? message.From.FirstName
                        : message.From.Username;

                    await userRepository.AddUserAsync(message.From.Id, lastfmUsername);
                }
            }
            else if (parameters.Count == 1)
            {
                // User has provided a Last.fm username
                lastfmUsername = parameters.First();

                await userRepository.AddUserAsync(message.From.Id, lastfmUsername);
            }
            else if (parameters.Count == 2 && parameters.Last().ToLowerInvariant().Equals("temp"))
            {
                // User has provided a temporary Last.fm username
                lastfmUsername = parameters.First();
            }
            else
            {
                // Invalid input
                return;
            }

            var track = await lastFmService.GetNowPlayingAsync(lastfmUsername);
            string response;

            if (track.Success)
            {
                var url = await spotifyService.TryGetLinkToTrackAsync(track.Track.ArtistName, track.Track.Name);

                response = GetResponseMessage(lastfmUsername, track, url);
            }
            else
            {
                lastfmUsername = HttpUtility.HtmlEncode(lastfmUsername);

                response = $"Could not find <i>{lastfmUsername}</i> on last.fm";
            }

            await responseFunc(message.Chat, response);
        }

        private static string GetResponseMessage(string lastfmUsername, LastfmTrackResponse track, string url)
        {
            string response;
            string artistAndName = HttpUtility.HtmlEncode($"{track.Track.ArtistName} - {track.Track.Name}");
            string username = HttpUtility.HtmlEncode(lastfmUsername);

            if (!string.IsNullOrEmpty(url) && (track.Track.IsNowPlaying ?? false))
            {
                // Now playing, and found a Spotify URL
                response = $"<i>{username} is currently playing</i>\n<a href=\"{url}\"><b>{artistAndName}</b></a>";
            }
            else if (!string.IsNullOrEmpty(url))
            {
                // Not currently playing, but found a Spotify URL
                response = $"<i>{username} played</i>\n<a href=\"{url}\"><b>{artistAndName}</b></a>\n<i>on {GetTimePlayed(track)}</i>";
            }
            else
            {
                // Not currently playing, and did not find a Spotify URL
                response = $"<i>{username} played</i>\n<b>{artistAndName}</b>\n<i>on {GetTimePlayed(track)}</i>";
            }

            return response;
        }

        private static string GetTimePlayed(LastfmTrackResponse track)
            => track.Track.TimePlayed?.DateTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");
    }
}
