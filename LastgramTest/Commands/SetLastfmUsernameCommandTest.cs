using Lastgram.Commands;
using Lastgram.Data.Repositories;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace LastgramTest.Commands
{
    [TestFixture]
    public class SetLastfmUsernameCommandTest
    {
        private SetLastfmUsernameCommand setLastfmUsernameCommand;

        private Mock<IUserRepository> userRepositoryMock;

        [SetUp]
        public void Setup()
        {
            userRepositoryMock = new();

            setLastfmUsernameCommand = new SetLastfmUsernameCommand(userRepositoryMock.Object);
        }

        [TestCase("John", true)]
        [TestCase("", false)]
        [TestCase(null, false)]
        public async Task AddUserToRepositoryIfCorrect(string lastfmUsername, bool shouldGetAdded)
        {
            await setLastfmUsernameCommand.ExecuteCommandAsync(
                new Message()
                {
                    From = new User()
                    {
                        Id = 1
                    },
                    Text = $"/setLastfmUsername {lastfmUsername}"
                },
                (chat, message) => Task.CompletedTask
            );

            if (shouldGetAdded)
            {
                userRepositoryMock.Verify(
                mocks => mocks.AddOrUpdateUserAsync(It.Is<int>(id => id == 1),
                It.Is<string>(username => username == lastfmUsername)),
                Times.Once);
            }
            else
            {
                userRepositoryMock.Verify(
                   mocks => mocks.AddOrUpdateUserAsync(It.IsAny<int>(),
                   It.IsAny<string>()),
                   Times.Never);
            }
        }

        [Test]
        public async Task DoNotAddTooLongUsername()
        {
            string lastfmUsername =
                "Loremipsumdolorsitamet,consecteturadipiscingelit.Morbicursusurnaacrcivulputate,sitametcongueenimsuscipit." +
                "Donecnecsemperjusto.Sedefficiturmattismollis.Proinrutrumpurusaduiiaculisposuere.Nullamnisitellus,tempus" +
                "tristiquescelerisquelobortis,consequatnecorci.Maecenasegetsemperdolor,euultricieselit.Fuscequisdiamtellus.";

            await setLastfmUsernameCommand.ExecuteCommandAsync(
                new Message()
                {
                    From = new User()
                    {
                        Id = 1
                    },
                    Text = $"/setLastfmUsername {lastfmUsername}"
                },
                (chat, message) => Task.CompletedTask
            );

            userRepositoryMock.Verify(
                   mocks => mocks.AddOrUpdateUserAsync(It.IsAny<int>(),
                   It.IsAny<string>()),
                   Times.Never);
        }
    }
}
