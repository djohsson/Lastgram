using System.Threading.Tasks;

namespace Lastgram.Data
{
    public interface IUserRepository
    {
        Task AddUserAsync(int telegramUserId, string lastFmUsername);

        Task<string> TryGetUserAsync(int telegramUserId);

        Task RemoveUserAsync(int telegramUserId);
    }
}
