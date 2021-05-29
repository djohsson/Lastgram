using Core.Data;
using Core.Data.Models;
using Core.Domain.Repositories.Lastfm;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreTest.Domain.Repositories.Lastfm
{
    [TestFixture]
    public class LastfmUsernameRepositoryTest
    {
        private DbContextOptions<MyDbContext> options;

        private static readonly List<User> Users = new List<User>()
        {
            new User() { TelegramUserId = 1, LastfmUsername = "John" },
            new User() { TelegramUserId = 2, LastfmUsername = "Jane" },
            new User() { TelegramUserId = 3, LastfmUsername = "Smith" },
        };

        [SetUp]
        public void SetUp()
        {
            options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (var context = new MyDbContext(options))
            {
                context.Users.AddRange(Users);
                context.SaveChanges();
            }
        }

        [Test]
        public async Task CanAddUsernameToDb()
        {
            using (var context = new MyDbContext(options))
            {
                var lastfmUsernameRepository = new LastfmUsernameRepository(context);

                await lastfmUsernameRepository.AddUserAsync(4, "Bob");
            }

            using (var context = new MyDbContext(options))
            {
                var user = await context.Users.FirstOrDefaultAsync(u => u.TelegramUserId == 4);

                Assert.AreEqual("Bob", user.LastfmUsername);
            }
        }

        [Test]
        public async Task RemoveNonExistingUser()
        {
            using (var context = new MyDbContext(options))
            {
                var lastfmUsernameRepository = new LastfmUsernameRepository(context);

                await lastfmUsernameRepository.RemoveUserAsync(4);
            }

            using (var context = new MyDbContext(options))
            {
                var usersFromDatabase = await context.Users.ToListAsync();

                Assert.AreEqual(Users.Count, usersFromDatabase.Count);
                Assert.IsTrue(Users.All(usersFromDatabase.Contains));
            }
        }

        [Test]
        public async Task RemoveExistingUserShouldRemoveFromDatabase()
        {
            using (var context = new MyDbContext(options))
            {
                var lastfmUsernameRepository = new LastfmUsernameRepository(context);

                await lastfmUsernameRepository.RemoveUserAsync(3);
            }

            using (var context = new MyDbContext(options))
            {
                var usersFromDatabase = await context.Users.ToListAsync();
                var user = await context.Users.FirstOrDefaultAsync(u => u.TelegramUserId == 3);

                Assert.AreEqual(Users.Count - 1, usersFromDatabase.Count);
                Assert.Null(user);
            }
        }

        [TestCase(1, true)]
        [TestCase(4, false)]
        public async Task TryGetUserAsync(int telegramUserId, bool exists)
        {
            using (var context = new MyDbContext(options))
            {
                var lastfmUsernameRepository = new LastfmUsernameRepository(context);

                var user = await lastfmUsernameRepository.TryGetUserAsync(telegramUserId);

                if (exists)
                {
                    Assert.IsNotNull(user);
                }
                else
                {
                    Assert.Null(user);
                }
            }
        }
    }
}
