using Lastgram.Data;
using Lastgram.Data.Repositories;
using Lastgram.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LastgramTest.Data
{
    [TestFixture]
    public class SpotifyTrackRepositoryTest
    {
        private DbContextOptions<MyDbContext> options;

        private static readonly List<SpotifyTrack> Tracks = new List<SpotifyTrack>()
        {
            new SpotifyTrack() { Md5 = "C7662A41609BB5C4BF8F654FC4C866D9", CreatedAt = DateTime.UtcNow, Artist = new Artist() { Name = "Evert Taube" }, Track = "Kinesiska Muren", Url = "www.evert.taube.se" },
            new SpotifyTrack() { Md5 = "745944B338378F1DF655F02BDF3155E6", CreatedAt = DateTime.UtcNow, Artist = new Artist() { Name = "Smash Mouth" }, Track = "All Star", Url = "www.google.com" },
            new SpotifyTrack() { Md5 = "FDB4A9B6A9273BB70475E2AADD7544A0", CreatedAt = DateTime.UtcNow, Artist = new Artist() { Name = "Donkey Kong" }, Track = "DK Rap", Url = "www.donkeykong.dk" },
        };

        [SetUp]
        public void SetUp()
        {
            options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (var context = new MyDbContext(options))
            {
                context.SpotifyTracks.AddRange(Tracks);
                context.SaveChanges();
            }
        }

        [Test]
        public async Task ShouldAddTrackToDatabase()
        {
            using (var context = new MyDbContext(options))
            {
                var spotifyTrackRepository = new SpotifyTrackRepository(context);

                await spotifyTrackRepository.AddSpotifyTrackAsync(new Artist() { Name = "Noisestorm" }, "Crab Rave", "www.youtube.com");
            }

            using (var context = new MyDbContext(options))
            {
                var track = await context.SpotifyTracks.FirstOrDefaultAsync(t => t.Artist.Name.Equals("Noisestorm"));

                Assert.NotNull(track);
            }
        }

        [TestCase("", "", "")]
        [TestCase(null, null, null)]
        [TestCase("Kevin Bacon", "Mall Cop Theme", null)]
        [TestCase("Kevin Bacon", "", "www.kevin.bacon")]
        public async Task ShouldNotAddInvalidTracksToDatabase(string artist, string trackName, string url)
        {
            using (var context = new MyDbContext(options))
            {
                var spotifyTrackRepository = new SpotifyTrackRepository(context);

                await spotifyTrackRepository.AddSpotifyTrackAsync(new Artist() { Name = artist }, trackName, url);
            }

            using (var context = new MyDbContext(options))
            {
                var tracksFromDatabase = await context.SpotifyTracks.ToListAsync();

                Assert.AreEqual(Tracks.Count, tracksFromDatabase.Count);
            }
        }

        [Test]
        public async Task AddAlreadyExistingTrackShouldNotAddToDatabase()
        {
            using (var context = new MyDbContext(options))
            {
                var spotifyTrackRepository = new SpotifyTrackRepository(context);

                await spotifyTrackRepository.AddSpotifyTrackAsync(new Artist() { Name = Tracks[0].Artist.Name }, Tracks[0].Track, Tracks[0].Url);
            }

            using (var context = new MyDbContext(options))
            {
                var tracksFromDatabase = await context.SpotifyTracks.ToListAsync();

                Assert.AreEqual(Tracks.Count, tracksFromDatabase.Count);
            }
        }
    }
}
