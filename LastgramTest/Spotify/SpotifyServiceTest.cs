using Lastgram.Spotify;
using Lastgram.Spotify.Repositories;
using Moq;
using NUnit.Framework;
using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LastgramTest.Spotify
{
    [TestFixture]
    public class SpotifyServiceTest
    {
        private ISpotifyService spotifyService;

        private Mock<ISpotifyTrackRepository> spotifyTrackRepositoryMock;
        private Mock<ISpotifyClient> spotifyClientMock;
        private Mock<IOAuthClient> oauthClientMock;
        private Mock<IArtistService> artistServiceMock;
        private Mock<ISearchClient> searchClientMock;

        [SetUp]
        public void SetUp()
        {
            spotifyTrackRepositoryMock = new Mock<ISpotifyTrackRepository>();
            searchClientMock = new Mock<ISearchClient>();
            spotifyClientMock = new Mock<ISpotifyClient>();
            oauthClientMock = new Mock<IOAuthClient>();
            artistServiceMock = new Mock<IArtistService>();

            spotifyClientMock.SetupGet(m => m.Search).Returns(searchClientMock.Object);

            spotifyService = new SpotifyService(
                spotifyTrackRepositoryMock.Object,
                oauthClientMock.Object,
                artistServiceMock.Object,
                c => spotifyClientMock.Object,
                () => new ClientCredentialsRequest("id", "secret"));
        }

        [Test]
        public async Task ReturnUrlFromRepository()
        {
            string urlInRepository = "www.evert.taube.se";

            spotifyTrackRepositoryMock.Setup(m =>
                m.TryGetSpotifyTrackUrlAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>()))
               .ReturnsAsync(urlInRepository);

            var url = await spotifyService.TryGetLinkToTrackAsync("Evert Taube", "Kinesiska Muren");

            Assert.AreEqual(urlInRepository, url);
        }

        [Test]
        public async Task ReturnUrlFromSpotifyIfNoneInRepository()
        {
            oauthClientMock.Setup(m =>
                m.RequestToken(It.IsAny<ClientCredentialsRequest>()))
                .ReturnsAsync(new ClientCredentialsTokenResponse()
                {
                    CreatedAt = DateTime.UtcNow,
                    ExpiresIn = 60,
                    AccessToken = "Token"
                });

            var url = "https://www.spotify/tracks/1";
            MockSearch(url);

            var resultUrl = await spotifyService.TryGetLinkToTrackAsync("Evert Taube", "Kinesiska Muren");

            oauthClientMock.Verify(m => m.RequestToken(It.IsAny<ClientCredentialsRequest>()), Times.Once);
            spotifyClientMock.Verify(m => m.Search, Times.Once);
            Assert.AreEqual(url, resultUrl);
        }

        private void MockSearch(string url)
        {
            var urlDictionary = new Dictionary<string, string>();
            urlDictionary.Add("spotify", url);

            searchClientMock.Setup(m => m.Item(It.IsAny<SearchRequest>()))
                .ReturnsAsync(new SearchResponse()
                {
                    Tracks = new Paging<FullTrack, SearchResponse>()
                    {
                        Items = new List<FullTrack>()
                        {
                            new FullTrack()
                            {
                                ExternalUrls = urlDictionary,
                            }
                        }
                    }
                });
        }
    }
}
