using Lastgram.Commands;
using Lastgram.Data.Repositories;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace LastgramTest.Commands
{
    [TestFixture]
    class ForgetMeCommandTest
    {
        private ForgetMeCommand forgetMeCommand;
        private Mock<IUserRepository> userRepositoryMock;

        [SetUp]
        public void Setup()
        {
            userRepositoryMock = new Mock<IUserRepository>();
            forgetMeCommand = new ForgetMeCommand(userRepositoryMock.Object);
        }

        [Test]
        public async Task ShouldRemoveFromRepository()
        {
            await forgetMeCommand.ExecuteCommandAsync(
                new Message()
                {
                    From = new User()
                    {
                        Id = 1,
                    }
                },
                (chat, message) => Task.CompletedTask
            );

            userRepositoryMock.Verify(m => m.RemoveUserAsync(It.Is<int>(id => id == 1)), Times.Once);
        }
    }
}
