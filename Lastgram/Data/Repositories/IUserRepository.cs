using System.Threading.Tasks;

namespace Lastgram.Data.Repositories
{
    public interface IUserRepository
    {
        Task AddOrUpdateUserAsync(int telegramUserId, string lastFmUsername);

        Task<string> TryGetUserAsync(int telegramUserId);

        Task RemoveUserAsync(int telegramUserId);
    }
}
