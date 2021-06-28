using System.Threading.Tasks;

namespace Core.Domain.Repositories.Lastfm
{
    public interface ILastfmUsernameRepository
    {
        Task AddUserAsync(long telegramUserId, string lastfmUsername);

        Task UpdateUserAsync(long telegramUserId, string lastfmUsername);

        Task<string> TryGetUserAsync(long telegramUserId);

        Task RemoveUserAsync(long telegramUserId);
    }
}
