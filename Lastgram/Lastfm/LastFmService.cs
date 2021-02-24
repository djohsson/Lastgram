using IF.Lastfm.Core.Api;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Lastgram.Lastfm
{
    public class LastfmService : ILastfmService
    {
        private readonly LastfmClient lastfmClient;

        public LastfmService()
        {
            lastfmClient = new LastfmClient(
                Environment.GetEnvironmentVariable("LASTGRAM_LASTFM_APIKEY"),
                Environment.GetEnvironmentVariable("LASTGRAM_LASTFM_APISECRET"));
        }

        public async Task<LastfmTrackResponse> GetNowPlayingAsync(string username)
        {
            var response = await lastfmClient.User.GetRecentScrobbles(username, count: 1);

            return new LastfmTrackResponse(response.Content.FirstOrDefault(), response.Success && response.Content.Any());
        }
    }
}
