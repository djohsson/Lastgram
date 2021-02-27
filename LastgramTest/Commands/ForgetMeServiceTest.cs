using Lastgram.Commands;
using Lastgram.Data;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace LastgramTest.Commands
{
    [TestFixture]
    class ForgetMeServiceTest
    {
        private IForgetMeCommand forgetMeService;
        private Mock<IUserRepository> userRepositoryMock;

        [SetUp]
        public void Setup()
        {
            userRepositoryMock = new Mock<IUserRepository>();
            forgetMeService = new ForgetMeCommand(userRepositoryMock.Object);
        }

        [Test]
        public void ShouldRemoveFromRepository()
        {
            forgetMeService.ExecuteCommandAsync(
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
