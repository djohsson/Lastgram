using Lastgram.Commands;
using Lastgram.Data.Repositories;
using LastgramTest.Helpers;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

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
                MessageHelper.CreateMessage(id: 1, text: "/forgetme"),
                (chat, message) => Task.CompletedTask
            );

            userRepositoryMock.Verify(m => m.RemoveUserAsync(It.Is<int>(id => id == 1)), Times.Once);
        }
    }
}
