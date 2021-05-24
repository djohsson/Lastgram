using Core.Domain.Models.Lastfm;
using Core.Domain.Services.Lastfm;
using Core.Domain.Services.Spotify;
using IF.Lastfm.Core.Objects;
using Lastgram.Commands;
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
                .Setup(m => m.GetLatestScrobbleAsync(It.IsAny<string>()))
                .ReturnsAsync(new LastfmScrobble
                {
                    LastfmTrack = new LastfmTrack
                    {
                        ArtistName = "artist",
                        Name = "name",
                        Url = new Uri("https://lastfm/track")
                    }
                });

            await nowPlayingCommand.ExecuteCommandAsync(
                MessageHelper.CreateCommandMessage(nowPlayingCommand),
                (chat, response) => Task.CompletedTask);

            lastfmServiceMock.Verify(m => m.GetLatestScrobbleAsync(It.Is<string>(u => u == usernameFromService)), Times.Once);
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
                .Setup(m => m.GetLatestScrobbleAsync(It.IsAny<string>()))
                .ReturnsAsync(value: null);

            Assert.ThrowsAsync<CommandException>(async ()
                => await nowPlayingCommand.ExecuteCommandAsync(
                    MessageHelper.CreateCommandMessage(nowPlayingCommand),
                    (chat, response) => Task.CompletedTask));

            lastfmServiceMock.Verify(m => m.GetLatestScrobbleAsync(It.Is<string>(u => u == usernameFromService)), Times.Once);
        }
    }
}
