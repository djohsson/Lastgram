using System.Threading.Tasks;

namespace Lastgram.Lastfm
{
    public interface ILastfmUsernameService
    {
        Task AddOrUpdateUsernameAsync(int telegramUserId, string username);

        Task<string> TryGetUsernameAsync(int telegramUserId);

        Task RemoveUsernameAsync(int telegramUserId);
    }
}
