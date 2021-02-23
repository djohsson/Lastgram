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
            // Not very testable...
            var config = SpotifyClientConfig.CreateDefault();

            var request = new ClientCredentialsRequest(
                Environment.GetEnvironmentVariable("LASTGRAM_SPOTIFY_CLIENTID"),
                Environment.GetEnvironmentVariable("LASTGRAM_SPOTIFY_CLIENTSECRET"));

            var response = new OAuthClient(config).RequestToken(request).Result;

            spotifyClient = new SpotifyClient(config.WithToken(response.AccessToken));

            this.spotifyTrackRepository = spotifyTrackRepository;
        }

        public async Task<string> TryGetLinkToTrackAsync(string artist, string track)
        {
            var url = await spotifyTrackRepository.TryGetSpotifyTrackUrlAsync(artist, track);

            if (url != null)
            {
                return url;
            }

            SearchRequest searchRequest = CreateSearchRequest(artist, track);

            try
            {
                var trackResponse = await spotifyClient.Search.Item(searchRequest);

                if (trackResponse.Tracks.Total > 0)
                {
                    url = trackResponse.Tracks.Items.First().ExternalUrls["spotify"];

                    await spotifyTrackRepository.AddSpotifyTrackAsync(artist, track, url);

                    return url;
                }
            }
            catch (APIException e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        private static SearchRequest CreateSearchRequest(string artist, string track)
        {
            return new SearchRequest(SearchRequest.Types.Track, $"{artist} - {track}")
            {
                Limit = 1,
                Market = "SE"
            };
        }
    }
}
