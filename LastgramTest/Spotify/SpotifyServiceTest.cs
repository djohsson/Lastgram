using Lastgram.Data;
using Lastgram.Spotify;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace LastgramTest.Spotify
{
    [TestFixture]
    public class SpotifyServiceTest
    {
        private ISpotifyService spotifyService;

        private Mock<ISpotifyTrackRepository> spotifyTrackRepositoryMock;

        [SetUp]
        public void SetUp()
        {
            spotifyTrackRepositoryMock = new Mock<ISpotifyTrackRepository>();

            spotifyService = new SpotifyService(spotifyTrackRepositoryMock.Object);
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
    }
}
