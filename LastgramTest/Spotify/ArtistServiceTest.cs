using Lastgram.Data.Models;
using Lastgram.Spotify;
using Lastgram.Spotify.Repositories;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace LastgramTest.Spotify
{
    [TestFixture]
    public class ArtistServiceTest
    {
        private IArtistService artistService;

        private Mock<IArtistRepository> artistRepositoryMock;

        [SetUp]
        public void SetUp()
        {
            artistRepositoryMock = new Mock<IArtistRepository>();

            artistService = new ArtistService(artistRepositoryMock.Object);
        }

        [TestCase(true)]
        [TestCase(false)]
        public async Task CreateArtistIfNoneExist(bool exists)
        {
            const string name = "Evert Taube";

            Artist artistInRepository = exists
                ? new Artist() { Name = name }
                : null;

            artistRepositoryMock.Setup(m => m.TryGetArtistAsync(It.IsAny<string>())).ReturnsAsync(value: artistInRepository);
            artistRepositoryMock.Setup(m => m.AddArtistAsync(It.IsAny<string>())).Returns<string>(
                (name) =>
                {
                    return Task.FromResult(new Artist() { Name = name });
                });

            var artist = await artistService.GetOrAddArtistAsync("Evert Taube");

            artistRepositoryMock.Verify(m => m.TryGetArtistAsync(It.IsAny<string>()), Times.Once);
            artistRepositoryMock.Verify(m => m.AddArtistAsync(It.IsAny<string>()), exists ? Times.Never : Times.Once);

            Assert.AreEqual(name, artist.Name);
        }
    }
}
