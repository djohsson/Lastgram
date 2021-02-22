using SpotifyAPI.Web;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Lastgram
{
    class SpotifyService : ISpotifyService
    {
        private readonly SpotifyClient spotifyClient;

        public SpotifyService()
        {
            var config = SpotifyClientConfig.CreateDefault();

            var test2 = Environment.GetEnvironmentVariable("LASTGRAM_TELEGRAM_KEY");
            var test = Environment.GetEnvironmentVariable("LASTGRAM_SPOTIFY_CLIENTID");

            var request = new ClientCredentialsRequest(
                Environment.GetEnvironmentVariable("LASTGRAM_SPOTIFY_CLIENTID"),
                Environment.GetEnvironmentVariable("LASTGRAM_SPOTIFY_CLIENTSECRET"));

            var response = new OAuthClient(config).RequestToken(request).Result;

            spotifyClient = new SpotifyClient(config.WithToken(response.AccessToken));
        }

        public async Task<SpotifySearchResponse> TryGetLinkToTrackAsync(string trackName)
        {
            var searchRequest = new SearchRequest(SearchRequest.Types.Track, trackName)
            {
                Limit = 1,
                Market = "SE"
            };

            try
            {
                var trackResponse = await spotifyClient.Search.Item(searchRequest);

                if (trackResponse.Tracks.Total > 0)
                {
                    return new SpotifySearchResponse(true, trackResponse.Tracks.Items.First().ExternalUrls["spotify"]);
                }
            }
            catch (APIException e)
            {
            }

            return new SpotifySearchResponse(false);
        }
    }
}
