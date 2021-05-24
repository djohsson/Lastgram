using Core.Domain.Services.Lastfm;
using Lastgram.Commands;
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
        private Mock<ILastfmUsernameService> lastfmUsernameServiceMock;

        [SetUp]
        public void Setup()
        {
            lastfmUsernameServiceMock = new();
            forgetMeCommand = new(lastfmUsernameServiceMock.Object);
        }

        [Test]
        public async Task ShouldRemoveFromRepository()
        {
            await forgetMeCommand.ExecuteCommandAsync(
                MessageHelper.CreateMessage(id: 1, text: "/forgetme"),
                (chat, message) => Task.CompletedTask
            );

            lastfmUsernameServiceMock.Verify(m => m.RemoveUsernameAsync(It.Is<int>(id => id == 1)), Times.Once);
        }
    }
}
