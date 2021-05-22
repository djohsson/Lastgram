using Lastgram.Commands;
using Lastgram.Lastfm;
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
        private Mock<ILastfmUsernameService> lastfmUserServiceMock;
        private Mock<ISpotifyService> spotifyServiceMock;

        [SetUp]
        public void Setup()
        {
            lastfmServiceMock = new();
            lastfmUserServiceMock = new();
            spotifyServiceMock = new();

            topTracksCommand = new(
                lastfmServiceMock.Object,
                lastfmUserServiceMock.Object,
                spotifyServiceMock.Object);
        }

        [Test]
        public void DoNotGetTracksIfNoUsernameIsSet()
        {
            lastfmUserServiceMock
                .Setup(m => m.TryGetUsernameAsync(It.IsAny<int>()))
                .ReturnsAsync(string.Empty);

            Assert.ThrowsAsync<CommandException>(
                async () => await topTracksCommand.ExecuteCommandAsync(
                    MessageHelper.CreateCommandMessage(topTracksCommand),
                    (chat, response) => Task.CompletedTask));

            lastfmServiceMock.Verify(m => m.GetTopTracksAsync(It.IsAny<string>()), Times.Never);
        }
    }
}
