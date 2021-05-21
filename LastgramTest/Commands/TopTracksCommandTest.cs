using Lastgram.Commands;
using Lastgram.Data.Repositories;
using Lastgram.Lastfm;
using Lastgram.Response;
using Lastgram.Spotify;
using LastgramTest.Helpers;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace LastgramTest.Commands
{
    [TestFixture]
    public class TopTracksCommandTest
    {
        private TopTracksCommand topTracksCommand;

        private Mock<ILastfmService> lastfmServiceMock;
        private Mock<IUserRepository> userRepositoryMock;
        private Mock<ISpotifyService> spotifyServiceMock;
        private Mock<ITrackResponseService> trackResponseServiceMock;

        [SetUp]
        public void Setup()
        {
            lastfmServiceMock = new();
            userRepositoryMock = new();
            spotifyServiceMock = new();
            trackResponseServiceMock = new();

            topTracksCommand = new(
                lastfmServiceMock.Object,
                userRepositoryMock.Object,
                spotifyServiceMock.Object,
                trackResponseServiceMock.Object);
        }

        [Test]
        public void DoNotGetTracksIfNoUsernameIsSet()
        {
            userRepositoryMock
                .Setup(m => m.TryGetUserAsync(It.IsAny<int>()))
                .ReturnsAsync(string.Empty);

            Assert.ThrowsAsync<CommandException>(
                async () => await topTracksCommand.ExecuteCommandAsync(
                    MessageHelper.CreateCommandMessage(topTracksCommand),
                    (chat, response) => Task.CompletedTask));

            lastfmServiceMock.Verify(m => m.GetTopTracksAsync(It.IsAny<string>()), Times.Never);
        }
    }
}
