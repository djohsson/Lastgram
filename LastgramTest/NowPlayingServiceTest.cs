using Lastgram;
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

        [SetUp]
        public void Setup()
        {
            userRepositoryMock = new Mock<IUserRepository>();
            lastfmServiceMock = new Mock<ILastFmService>();
            nowPlayingService = new NowPlayingService(userRepositoryMock.Object, lastfmServiceMock.Object);
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

            userRepositoryMock.Verify(mocks => mocks.AddUser(It.Is<int>(id => id == 1), It.Is<string>(username => username == lastFmUsername)), Times.Once);
        }

        [Test]
        public void GetUserFromRepositoryIfNoneIsProvided()
        {
            string lastFmUsername = "John";
            string lastFmUsernameFromRepo = string.Empty;

            userRepositoryMock.Setup(m => m.TryGetUser(It.IsAny<int>(), out lastFmUsername)).Returns(true);

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

            Assert.AreEqual(lastFmUsername, lastFmUsernameFromRepo);
            userRepositoryMock.Verify(m => m.TryGetUser(It.IsAny<int>(), out lastFmUsernameFromRepo), Times.Once);
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

            userRepositoryMock.Verify(mocks => mocks.AddUser(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void AddTelegramUsernameIfNothingInRepository()
        {
            string lastFmUsername = "John";
            string lastFmUsernameFromRepo = string.Empty;
            string telegramUsername = string.Empty;

            userRepositoryMock.Setup(m => m.TryGetUser(It.IsAny<int>(), out lastFmUsernameFromRepo)).Returns(false);

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

            Assert.AreEqual(lastFmUsername, telegramUsername);
            userRepositoryMock.Verify(m => m.AddUser(It.IsAny<int>(), lastFmUsername), Times.Once);
        }
    }
}
