using Lastgram;
using Lastgram.Commands;
using Lastgram.Data;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace LastgramTest
{
    [TestFixture]
    class ForgetMeServiceTest
    {
        private IForgetMeService forgetMeService;
        private Mock<IUserRepository> userRepositoryMock;

        [SetUp]
        public void Setup()
        {
            userRepositoryMock = new Mock<IUserRepository>();
            forgetMeService = new ForgetMeService(userRepositoryMock.Object);
        }

        [Test]
        public void ShouldRemoveFromRepository()
        {
            forgetMeService.HandleCommandAsync(
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
