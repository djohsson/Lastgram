using Core.Data;
using Core.Data.Models;
using System.Threading.Tasks;

namespace Core.Domain.Repositories.Lastfm
{
    public class LastfmUsernameRepository : ILastfmUsernameRepository
    {
        private readonly IMyDbContext context;

        public LastfmUsernameRepository(IMyDbContext context)
        {
            this.context = context;
        }

        public async Task AddOrUpdateUserAsync(int telegramUserId, string lastFmUsername)
        {
            var user = await context.Users.FindAsync(telegramUserId);

            if (user == null)
            {
                await context.Users.AddAsync(new User
                {
                    TelegramUserId = telegramUserId,
                    LastfmUsername = lastFmUsername,
                });
            }
            else
            {
                user.LastfmUsername = lastFmUsername;
                context.Users.Update(user);
            }

            await context.SaveChangesAsync();
        }

        public async Task AddUserAsync(int telegramUserId, string lastFmUsername)
        {
            await context.Users.AddAsync(new User
            {
                TelegramUserId = telegramUserId,
                LastfmUsername = lastFmUsername,
            });

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

        public async Task UpdateUserAsync(int telegramUserId, string lastFmUsername)
        {
            var user = await context.Users.FindAsync(telegramUserId);

            if (user == null)
            {
                return;
            }

            user.LastfmUsername = lastFmUsername;
            context.Users.Update(user);

            await context.SaveChangesAsync();
        }
    }
}
