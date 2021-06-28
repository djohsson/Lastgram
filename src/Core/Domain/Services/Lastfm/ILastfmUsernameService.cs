using System.Threading.Tasks;

namespace Core.Domain.Services.Lastfm
{
    public interface ILastfmUsernameService
    {
        Task AddOrUpdateUsernameAsync(long telegramUserId, string username);

        Task<string> TryGetUsernameAsync(long telegramUserId);

        Task RemoveUsernameAsync(long telegramUserId);
    }
}
