using Lastgram.Models;
using System.Threading.Tasks;

namespace Lastgram.Data
{
    internal class UserRepository : IUserRepository
    {
        private readonly MyDbContext context;

        public UserRepository(MyDbContext context)
        {
            this.context = context;
        }

        public async Task AddUserAsync(int telegramUserId, string lastFmUsername)
        {
            var user = await context.Users.FindAsync(telegramUserId);

            if (user == null)
            {
                await context.Users.AddAsync(new User
                {
                    TelegramUserId = telegramUserId,
                    LastfmUsername = lastFmUsername,
                });
            } else
            {
                user.LastfmUsername = lastFmUsername;
                context.Users.Update(user);
            }

            await context.SaveChangesAsync();
        }

        public async Task RemoveUserAsync(int telegramUserId)
        {
            var user = await context.Users.FindAsync(telegramUserId);

            if (user == null)
            {
                return;
            }

            context.Remove(user);

            await context.SaveChangesAsync();
        }

        public async Task<string> TryGetUserAsync(int telegramUserId)
        {
            var user = await context.Users.FindAsync(telegramUserId);

            return user?.LastfmUsername;
        }
    }
}
