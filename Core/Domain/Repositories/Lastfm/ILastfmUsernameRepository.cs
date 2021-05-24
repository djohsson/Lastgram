using System.Threading.Tasks;

namespace Core.Domain.Repositories.Lastfm
{
    public interface ILastfmUsernameRepository
    {
        Task AddUserAsync(int telegramUserId, string lastfmUsername);

        Task UpdateUserAsync(int telegramUserId, string lastfmUsername);

        Task<string> TryGetUserAsync(int telegramUserId);

        Task RemoveUserAsync(int telegramUserId);
    }
}
