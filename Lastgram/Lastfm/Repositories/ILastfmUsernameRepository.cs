using System.Threading.Tasks;

namespace Lastgram.Lastfm.Repositories
{
    public interface ILastfmUsernameRepository
    {
        Task AddUserAsync(int telegramUserId, string lastFmUsername);

        Task UpdateUserAsync(int telegramUserId, string lastFmUsername);

        Task<string> TryGetUserAsync(int telegramUserId);

        Task RemoveUserAsync(int telegramUserId);
    }
}
