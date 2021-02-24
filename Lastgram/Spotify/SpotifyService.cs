using Lastgram.Data;
using SpotifyAPI.Web;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Lastgram.Spotify
{
    class SpotifyService : ISpotifyService
    {
        private static readonly ClientCredentialsRequest CredentialsRequest = new ClientCredentialsRequest(
            Environment.GetEnvironmentVariable("LASTGRAM_SPOTIFY_CLIENTID"),
            Environment.GetEnvironmentVariable("LASTGRAM_SPOTIFY_CLIENTSECRET"));
        private static readonly SpotifyClientConfig ClientConfig = SpotifyClientConfig.CreateDefault();
        private static readonly OAuthClient OAuthClient = new OAuthClient(ClientConfig);

        private readonly ISpotifyTrackRepository spotifyTrackRepository;
        private readonly SemaphoreSlim semaphore;

        private SpotifyClient spotifyClient;
        private ClientCredentialsTokenResponse tokenResponse;

        public SpotifyService(ISpotifyTrackRepository spotifyTrackRepository)
        {
            semaphore = new SemaphoreSlim(1, 1);

            // Not very testable...
            RenewAccessTokenAsync().Wait();

            this.spotifyTrackRepository = spotifyTrackRepository;
        }

        public async Task<string> TryGetLinkToTrackAsync(string artist, string track)
        {
            var url = await spotifyTrackRepository.TryGetSpotifyTrackUrlAsync(artist, track);

            if (url != null)
            {
                return url;
            }

            await semaphore.WaitAsync();

            await RenewAccessTokenIfExpiredAsync();

            semaphore.Release();

            try
            {
                url = await SearchForTrackOnSpotifyAsync(artist, track);

                await spotifyTrackRepository.AddSpotifyTrackAsync(artist, track, url);
            }
            catch(APIUnauthorizedException e)
            {
                Console.WriteLine(e);
            }
            catch (APIException e)
            {
                Console.WriteLine(e);
            }

            return url;
        }

        private async Task RenewAccessTokenIfExpiredAsync()
        {
            if (AccessTokenIsExpired())
            {
                return;
            }

            await RenewAccessTokenAsync();
        }

        private bool AccessTokenIsExpired()
            => tokenResponse.CreatedAt.AddSeconds(tokenResponse.ExpiresIn).AddMinutes(5) > DateTime.UtcNow;

        private async Task RenewAccessTokenAsync()
        {
            tokenResponse = await OAuthClient.RequestToken(CredentialsRequest);

            spotifyClient = new SpotifyClient(ClientConfig.WithToken(tokenResponse.AccessToken));
        }

        private async Task<string> SearchForTrackOnSpotifyAsync(string artist, string track)
        {
            SearchRequest searchRequest = CreateSearchRequest(artist, track);

            var trackResponse = await spotifyClient.Search.Item(searchRequest);

            if (!trackResponse.Tracks.Items.Any())
            {
                return null;
            }

            return trackResponse.Tracks.Items.First().ExternalUrls["spotify"];
        }

        private static SearchRequest CreateSearchRequest(string artist, string track)
        {
            return new SearchRequest(SearchRequest.Types.Track, $"{artist} {track}")
            {
                Limit = 1,
                Market = "SE"
            };
        }
    }
}
