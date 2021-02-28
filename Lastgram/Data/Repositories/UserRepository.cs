using Lastgram.Models;
using System.Threading.Tasks;

namespace Lastgram.Data.Repositories
{
    public  class UserRepository : IUserRepository
    {
        private readonly IMyDbContext context;

        public UserRepository(IMyDbContext context)
        {
            this.context = context;
        }

        public async Task AddOrUpdateUserAsync(int telegramUserId, string lastFmUsername)
        {
            if (string.IsNullOrEmpty(lastFmUsername))
            {
                return;
            }

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

            context.Users.Remove(user);

            await context.SaveChangesAsync();
        }

        public async Task<string> TryGetUserAsync(int telegramUserId)
        {
            var user = await context.Users.FindAsync(telegramUserId);

            return user?.LastfmUsername;
        }
    }
}
