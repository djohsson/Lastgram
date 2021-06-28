using Core.Domain.Repositories.Lastfm;
using Core.Domain.Services.Lastfm;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace CoreTest.Domain.Services.Lastfm
{
    [TestFixture]
    public class LastfmUsernameServiceTest
    {
        private LastfmUsernameService lastfmUsernameService;

        private Mock<ILastfmUsernameRepository> lastfmUsernameRepositoryMock;

        [SetUp]
        public void Setup()
        {
            lastfmUsernameRepositoryMock = new();

            lastfmUsernameService = new(lastfmUsernameRepositoryMock.Object);
        }

        [TestCase("", false)]
        [TestCase(null, false)]
        [TestCase("Bob", true)]
        public async Task DoNotAddInvalidUsername(string username, bool isValid)
        {
            lastfmUsernameRepositoryMock
                .Setup(m => m.TryGetUserAsync(It.IsAny<long>()))
                .ReturnsAsync(value: null);

            await lastfmUsernameService.AddOrUpdateUsernameAsync(1, username);

            var timesCalled = isValid ? Times.Once() : Times.Never();

            lastfmUsernameRepositoryMock
                .Verify(m => m.AddUserAsync(It.IsAny<long>(), It.IsAny<string>()), timesCalled);
        }

        [Test]
        public async Task UpdateExistingUser()
        {
            const int id = 1;
            const string username = "Bob";

            lastfmUsernameRepositoryMock
                .Setup(m => m.TryGetUserAsync(It.Is<long>(i => i == id)))
                .ReturnsAsync(username);

            await lastfmUsernameService.AddOrUpdateUsernameAsync(id, username);

            lastfmUsernameRepositoryMock
                .Verify(m => m.AddUserAsync(It.IsAny<long>(), It.IsAny<string>()), Times.Never);

            lastfmUsernameRepositoryMock
                .Verify(m => m.UpdateUserAsync(It.Is<long>(i => i == id), It.Is<string>(u => u == username)), Times.Once);
        }
    }
}
