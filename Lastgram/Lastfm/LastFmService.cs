using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Objects;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lastgram.Lastfm
{
    public class LastfmService : ILastfmService
    {
        private const int TOP_TRACKS_COUNT = 5;

        private readonly IUserApi userApi;

        public LastfmService(IUserApi userApi)
        {
            this.userApi = userApi;
        }

        public async Task<LastfmTrackResponse> GetNowPlayingAsync(string username)
        {
            var response = await userApi.GetRecentScrobbles(username, count: 1);

            return new LastfmTrackResponse
            {
                Track = response.FirstOrDefault(),
                IsSuccess = response.Success && response.Any()
            };
        }

        public async Task<LastfmTopTracksResponse> GetTopTracksAsync(string username)
        {
            var response = await userApi.GetWeeklyTrackChartAsync(username);

            if (response.Success)
            {
                return new LastfmTopTracksResponse
                {
                    TopTracks = response.Content.Take(TOP_TRACKS_COUNT).ToList(),
                    IsSuccess = true,
                };
            }

            return new LastfmTopTracksResponse
            {
                TopTracks = new List<LastTrack>(),
                IsSuccess = false,
            };
        }
    }
}
