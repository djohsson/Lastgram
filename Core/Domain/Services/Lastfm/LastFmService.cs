using Core.Domain.Models.Lastfm;
using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Objects;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Domain.Services.Lastfm
{
    public class LastfmService : ILastfmService
    {
        private const int TOP_TRACKS_COUNT = 5;

        private readonly IUserApi userApi;
        private readonly ITrackApi trackAPI;

        public LastfmService(IUserApi userApi, ITrackApi trackAPI)
        {
            this.userApi = userApi;
            this.trackAPI = trackAPI;
        }

        public async Task<LastfmScrobble> GetLatestScrobbleAsync(string username)
        {
            var response = await userApi.GetRecentScrobbles(username, count: 1, extendedResponse: true);

            var track = response.FirstOrDefault();

            if (track == null)
            {
                return null;
            }

            int? userPlayCount = await GetPlayedCountAsync(track.Name, track.ArtistName, username);

            return new LastfmScrobble()
            {
                LastfmTrack = new LastfmTrack
                {
                    Name = track.Name,
                    ArtistName = track.ArtistName,
                    UserPlayCount = userPlayCount,
                    Url = track.Url,
                },
                IsNowPlaying = track.IsNowPlaying.Value,
                TimePlayed = track.TimePlayed?.DateTime,
            };
        }

        public async Task<IReadOnlyList<LastfmTrack>> GetTopTracksAsync(string username)
        {
            var response = await userApi.GetWeeklyTrackChartAsync(username);

            var tracks = new List<LastfmTrack>();

            if (response.Success)
            {
                tracks.AddRange(response.Content.Take(TOP_TRACKS_COUNT).Select(ToLastfmTrack));
            }

            return tracks;
        }

        public async Task<int?> GetPlayedCountAsync(string trackName, string artistName, string username)
        {
            var response = await trackAPI.GetInfoAsync(trackName, artistName, username);

            return response.Success && response.Content.UserPlayCount.HasValue
                ? response.Content.UserPlayCount.Value
                : null;
        }

        private static LastfmTrack ToLastfmTrack(LastTrack track)
        {
            return new LastfmTrack
            {
                Name = track.Name,
                ArtistName = track.ArtistName
            };
        }
    }
}
