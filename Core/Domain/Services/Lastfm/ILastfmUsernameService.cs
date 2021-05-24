using System.Threading.Tasks;

namespace Core.Domain.Services.Lastfm
{
    public interface ILastfmUsernameService
    {
        Task AddOrUpdateUsernameAsync(int telegramUserId, string username);

        Task<string> TryGetUsernameAsync(int telegramUserId);

        Task RemoveUsernameAsync(int telegramUserId);
    }
}
