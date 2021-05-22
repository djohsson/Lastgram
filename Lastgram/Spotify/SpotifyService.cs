using Lastgram.Data.Models;
using Lastgram.Spotify.Repositories;
using SpotifyAPI.Web;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Lastgram.Spotify
{
    public class SpotifyService : ISpotifyService
    {
        private static readonly SpotifyClientConfig ClientConfig = SpotifyClientConfig.CreateDefault();

        private readonly ISpotifyTrackRepository spotifyTrackRepository;
        private readonly IOAuthClient oauthClient;
        private readonly IArtistService artistService;
        private readonly Func<SpotifyClientConfig, ISpotifyClient> spotifyClientProvider;
        private readonly ClientCredentialsRequest credentialsRequest;
        private readonly SemaphoreSlim semaphore;

        private ISpotifyClient spotifyClient;
        private ClientCredentialsTokenResponse tokenResponse = null;

        public SpotifyService(
            ISpotifyTrackRepository spotifyTrackRepository,
            IOAuthClient oauthClient,
            IArtistService artistService,
            Func<SpotifyClientConfig, ISpotifyClient> spotifyClientProvider,
            Func<ClientCredentialsRequest> clientCredentialsRequestProvider)
        {
            this.spotifyTrackRepository = spotifyTrackRepository;
            this.oauthClient = oauthClient;
            this.artistService = artistService;
            this.spotifyClientProvider = spotifyClientProvider;

            semaphore = new SemaphoreSlim(1, 1);
            credentialsRequest = clientCredentialsRequestProvider();
        }

        public async Task<string> TryGetLinkToTrackAsync(string artistName, string track)
        {
            var url = await spotifyTrackRepository.TryGetSpotifyTrackUrlAsync(artistName, track);

            if (url != null)
            {
                return url;
            }

            await RenewAccessTokenIfExpiredAsync();

            try
            {
                url = await SearchForTrackOnSpotifyAsync(artistName, track);

                await StoreTrackAsync(artistName, track, url);
            }
            catch (APIException e)
            {
                Console.WriteLine(e);
            }

            return url;
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

        private async Task StoreTrackAsync(string artistName, string track, string url)
        {
            Artist artist = await artistService.GetOrAddArtistAsync(artistName);

            await spotifyTrackRepository.AddSpotifyTrackAsync(artist, track, url);
        }

        private async Task RenewAccessTokenIfExpiredAsync()
        {
            // This does not fix all of the potential raceconditions in this file. But ehh, it will probably never happen :^)
            await semaphore.WaitAsync();

            try
            {
                if (!AccessTokenIsExpired())
                {
                    return;
                }

                await RenewAccessTokenAsync();
            }
            finally
            {
                semaphore.Release();
            }
        }

        private bool AccessTokenIsExpired()
        {
            if (tokenResponse == null)
            {
                return true;
            }

            var expiresAt = tokenResponse
                .CreatedAt
                .AddSeconds(tokenResponse.ExpiresIn)
                .Subtract(TimeSpan.FromMinutes(5));

            return expiresAt < DateTime.UtcNow;
        }

        private async Task RenewAccessTokenAsync()
        {
            tokenResponse = await oauthClient.RequestToken(credentialsRequest);

            spotifyClient = spotifyClientProvider(ClientConfig.WithToken(tokenResponse.AccessToken));
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
