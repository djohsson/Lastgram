using IF.Lastfm.Core.Objects;
using Lastgram.Commands;
using Lastgram.Lastfm;
using Lastgram.Spotify;
using LastgramTest.Helpers;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace LastgramTest.Commands
{
    [TestFixture]
    public class NowPlayingCommandTest
    {
        private NowPlayingCommand nowPlayingCommand;

        private Mock<ILastfmService> lastfmServiceMock;
        private Mock<ILastfmUsernameService> lastfmUsernameServiceMock;
        private Mock<ISpotifyService> spotifyServiceMock;

        [SetUp]
        public void Setup()
        {
            lastfmServiceMock = new();
            lastfmUsernameServiceMock = new();
            spotifyServiceMock = new();

            nowPlayingCommand = new(
                lastfmServiceMock.Object, spotifyServiceMock.Object, lastfmUsernameServiceMock.Object);
        }

        [Test]
        public async Task UseUsernameFromService()
        {
            string usernameFromService = "username";
            lastfmUsernameServiceMock
                .Setup(m => m.TryGetUsernameAsync(It.IsAny<int>()))
                .ReturnsAsync(usernameFromService);

            lastfmServiceMock
                .Setup(m => m.GetNowPlayingAsync(It.IsAny<string>()))
                .ReturnsAsync(new LastfmTrackResponse
                {
                    IsSuccess = true,
                    Track = new LastTrack
                    {
                        ArtistName = "artist",
                        Name = "name",
                        Url = new Uri("https://lastfm/track")
                    }
                });

            await nowPlayingCommand.ExecuteCommandAsync(
                MessageHelper.CreateCommandMessage(nowPlayingCommand),
                (chat, response) => Task.CompletedTask);

            lastfmServiceMock.Verify(m => m.GetNowPlayingAsync(It.Is<string>(u => u == usernameFromService)), Times.Once);
        }

        [Test]
        public void ThrowIfNotRegistered()
        {
            lastfmUsernameServiceMock
                .Setup(m => m.TryGetUsernameAsync(It.IsAny<int>()))
                .ReturnsAsync(string.Empty);

            Assert.ThrowsAsync<CommandException>(async ()
                => await nowPlayingCommand.ExecuteCommandAsync(
                    MessageHelper.CreateCommandMessage(nowPlayingCommand),
                    (chat, response) => Task.CompletedTask));
        }

        [Test]
        public void ThrowIfNoTrackIsFound()
        {
            string usernameFromService = "username";
            lastfmUsernameServiceMock
                .Setup(m => m.TryGetUsernameAsync(It.IsAny<int>()))
                .ReturnsAsync(usernameFromService);

            lastfmServiceMock
                .Setup(m => m.GetNowPlayingAsync(It.IsAny<string>()))
                .ReturnsAsync(new LastfmTrackResponse
                {
                    IsSuccess = false,
                    Track = null,
                });

            Assert.ThrowsAsync<CommandException>(async ()
                => await nowPlayingCommand.ExecuteCommandAsync(
                    MessageHelper.CreateCommandMessage(nowPlayingCommand),
                    (chat, response) => Task.CompletedTask));

            lastfmServiceMock.Verify(m => m.GetNowPlayingAsync(It.Is<string>(u => u == usernameFromService)), Times.Once);
        }
    }
}
