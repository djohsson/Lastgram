using Lastgram.Commands;
using Lastgram.Data.Repositories;
using LastgramTest.Helpers;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

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
        public async Task AddUserToRepositoryIfValid(string lastfmUsername, bool shouldGetAdded)
        {
            try
            {
                await setLastfmUsernameCommand.ExecuteCommandAsync(
                MessageHelper.CreateMessage(id: 1, text: $"/setusername {lastfmUsername}"),
                (chat, message) => Task.CompletedTask);
            }
            catch (CommandException)
            {
                if (shouldGetAdded)
                {
                    Assert.Fail("Should not throw");
                }
            }

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

        [TestCase("John", true)]
        [TestCase("", false)]
        [TestCase(null, false)]
        public void ThrowIfInvalidUsername(string lastfmUsername, bool shouldGetAdded)
        {
            if (!shouldGetAdded)
            {
                Assert.ThrowsAsync<CommandException>(
                    async () => await setLastfmUsernameCommand.ExecuteCommandAsync(
                            MessageHelper.CreateMessage(text: $"/setusername {lastfmUsername}"),
                            (chat, message) => Task.CompletedTask));
            }
            else
            {
                Assert.DoesNotThrowAsync(
                    async () => await setLastfmUsernameCommand.ExecuteCommandAsync(
                            MessageHelper.CreateMessage(text: $"/setusername {lastfmUsername}"),
                            (chat, message) => Task.CompletedTask));
            }
        }

        [Test]
        public async Task DoNotAddTooLongUsername()
        {
            string lastfmUsername =
                "Loremipsumdolorsitamet,consecteturadipiscingelit.Morbicursusurnaacrcivulputate,sitametcongueenimsuscipit." +
                "Donecnecsemperjusto.Sedefficiturmattismollis.Proinrutrumpurusaduiiaculisposuere.Nullamnisitellus,tempus" +
                "tristiquescelerisquelobortis,consequatnecorci.Maecenasegetsemperdolor,euultricieselit.Fuscequisdiamtellus.";

            try
            {
                await setLastfmUsernameCommand.ExecuteCommandAsync(
                    MessageHelper.CreateMessage(text: $"/setusername {lastfmUsername}"),
                    (chat, message) => Task.CompletedTask);
            }
            catch (CommandException)
            {
                userRepositoryMock.Verify(
                   mocks => mocks.AddOrUpdateUserAsync(It.IsAny<int>(),
                   It.IsAny<string>()),
                   Times.Never);

                return;
            }

            Assert.Fail("Should throw");
        }
    }
}
