using Lastgram.Data;
using Lastgram.Data.Repositories;
using Lastgram.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LastgramTest.Data
{
    [TestFixture]
    public class UserRepositoryTest
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
 
        [TestCase("", false)]
        [TestCase(null, false)]
        [TestCase("Bob", true)]
        public async Task AddAsyncRequiresUsername(string username, bool valid)
        {
            using (var context = new MyDbContext(options))
            {
                var userRepository = new UserRepository(context);

                await userRepository.AddOrUpdateUserAsync(4, username);
            }
            
            using (var context = new MyDbContext(options))
            {
                var user = await context.Users.FindAsync(4);

                if (valid)
                {
                    Assert.NotNull(user);
                }
                else
                {
                    Assert.Null(user);
                }
            }
        }

        [Test]
        public async Task AddAsyncExistingUserShouldBeUpdated()
        {
            using (var context = new MyDbContext(options))
            {
                var userRepository = new UserRepository(context);

                await userRepository.AddOrUpdateUserAsync(1, "Bob"); 
            }

            using (var context = new MyDbContext(options))
            {
                var user = await context.Users.FindAsync(1);

                Assert.AreEqual("Bob", user.LastfmUsername);
            }
        }

        [Test]
        public async Task RemoveNonExistingUser()
        {
            using (var context = new MyDbContext(options))
            {
                var userRepository = new UserRepository(context);

                await userRepository.RemoveUserAsync(4);
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
                var userRepository = new UserRepository(context);

                await userRepository.RemoveUserAsync(3);
            }

            using (var context = new MyDbContext(options))
            {
                var usersFromDatabase = await context.Users.ToListAsync();
                var user = await context.Users.FindAsync(3);

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
                var userRepository = new UserRepository(context);

                var user = await userRepository.TryGetUserAsync(telegramUserId);

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
