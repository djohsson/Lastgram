using Lastgram.Commands;
using Lastgram.Data;
using Lastgram.Lastfm;
using Lastgram.Spotify;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace LastgramTest
{
    [TestFixture]
    class NowPlayingServiceTest
    {
        private INowPlayingService nowPlayingService;
        private Mock<IUserRepository> userRepositoryMock;
        private Mock<ILastFmService> lastfmServiceMock;
        private Mock<ISpotifyService> spotifyServiceMock;

        [SetUp]
        public void Setup()
        {
            userRepositoryMock = new Mock<IUserRepository>();
            lastfmServiceMock = new Mock<ILastFmService>();
            spotifyServiceMock = new Mock<ISpotifyService>();
            nowPlayingService = new NowPlayingService(userRepositoryMock.Object, lastfmServiceMock.Object, spotifyServiceMock.Object);
        }

        [Test]
        public void AddUserToRepositoryIfProvidingUsername()
        {
            string lastFmUsername = "John";

            nowPlayingService.HandleCommandAsync(
                new Message() {
                    From = new User()
                    {
                        Id = 1
                    },
                    Text = $"/np {lastFmUsername}"
                },
                (chat, message) => Task.CompletedTask
            );

            userRepositoryMock.Verify(mocks => mocks.AddUserAsync(It.Is<int>(id => id == 1), It.Is<string>(username => username == lastFmUsername)), Times.Once);
        }

        [Test]
        public void GetUserFromRepositoryIfNoneIsProvided()
        {
            string lastFmUsername = "John";
            string lastFmUsernameFromRepo = string.Empty;

            userRepositoryMock.Setup(m => m.TryGetUserAsync(It.IsAny<int>())).ReturnsAsync(lastFmUsername);
            lastfmServiceMock.Setup(m => m.GetNowPlayingAsync(It.IsAny<string>())).ReturnsAsync(new LastfmTrackResponse(null, false));

            nowPlayingService.HandleCommandAsync(
                new Message()
                {
                    From = new User()
                    {
                        Id = 1
                    },
                    Text = "/np"
                },
                (chat, message) =>
                {
                    lastFmUsernameFromRepo = message;
                    return Task.CompletedTask;
                }
            );

            Assert.AreEqual("Could not find <i>John</i> on last.fm", lastFmUsernameFromRepo);
            userRepositoryMock.Verify(m => m.TryGetUserAsync(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void DoNotAddUserToRepositoryIfTemporary()
        {
            nowPlayingService.HandleCommandAsync(
                new Message()
                {
                    From = new User()
                    {
                        Id = 1
                    },
                    Text = $"/np John temp"
                },
                (chat, message) => Task.CompletedTask
            );

            userRepositoryMock.Verify(mocks => mocks.AddUserAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void AddTelegramUsernameIfNothingInRepository()
        {
            string lastFmUsername = "John";
            string lastFmUsernameFromRepo = string.Empty;
            string telegramUsername = string.Empty;

            userRepositoryMock.Setup(m => m.TryGetUserAsync(It.IsAny<int>())).ReturnsAsync(string.Empty);
            lastfmServiceMock.Setup(m => m.GetNowPlayingAsync(It.IsAny<string>())).ReturnsAsync(new LastfmTrackResponse(null, false));

            nowPlayingService.HandleCommandAsync(
                new Message()
                {
                    From = new User()
                    {
                        Id = 1,
                        Username = lastFmUsername
                    },
                    Text = "/np"
                },
                (chat, message) =>
                {
                    telegramUsername = message;
                    return Task.CompletedTask;
                }
            );

            Assert.AreEqual("Could not find <i>John</i> on last.fm", telegramUsername);
            userRepositoryMock.Verify(m => m.AddUserAsync(It.IsAny<int>(), lastFmUsername), Times.Once);
        }
    }
}
