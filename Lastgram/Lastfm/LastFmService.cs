using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Objects;
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

            LastTrack track = null;

            if (response.Any())
            {
                track = response.Content.FirstOrDefault(t => t.TimePlayed != null)
                    ?? response.Content.FirstOrDefault();
            }

            return new LastfmTrackResponse(track, response.Success && response.Content.Any());
        }
    }
}
