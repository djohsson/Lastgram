using Lastgram.Data;
using SpotifyAPI.Web;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Lastgram.Spotify
{
    class SpotifyService : ISpotifyService
    {
        private readonly SpotifyClient spotifyClient;
        private readonly ISpotifyTrackRepository spotifyTrackRepository;

        public SpotifyService(ISpotifyTrackRepository spotifyTrackRepository)
        {
            var config = SpotifyClientConfig.CreateDefault();

            var test2 = Environment.GetEnvironmentVariable("LASTGRAM_TELEGRAM_KEY");
            var test = Environment.GetEnvironmentVariable("LASTGRAM_SPOTIFY_CLIENTID");

            var request = new ClientCredentialsRequest(
                Environment.GetEnvironmentVariable("LASTGRAM_SPOTIFY_CLIENTID"),
                Environment.GetEnvironmentVariable("LASTGRAM_SPOTIFY_CLIENTSECRET"));

            var response = new OAuthClient(config).RequestToken(request).Result;

            spotifyClient = new SpotifyClient(config.WithToken(response.AccessToken));

            this.spotifyTrackRepository = spotifyTrackRepository;
        }

        public async Task<SpotifySearchResponse> TryGetLinkToTrackAsync(string trackName)
        {
            var url = await spotifyTrackRepository.TryGetSpotifyTrackUrlAsync(trackName);

            if (url != null)
            {
                return new SpotifySearchResponse(true, url);
            }

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
                    url = trackResponse.Tracks.Items.First().ExternalUrls["spotify"];

                    await spotifyTrackRepository.AddSpotifyTrackAsync(trackName, url);

                    return new SpotifySearchResponse(true, url);
                }
            }
            catch (APIException e)
            {
                Console.WriteLine(e);
            }

            return new SpotifySearchResponse(false);
        }
    }
}
