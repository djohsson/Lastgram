using Core.Domain.Repositories.Lastfm;
using System.Threading.Tasks;

namespace Core.Domain.Services.Lastfm
{
    public class LastfmUsernameService : ILastfmUsernameService
    {
        private readonly ILastfmUsernameRepository userRepository;

        public LastfmUsernameService(ILastfmUsernameRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task AddOrUpdateUsernameAsync(long telegramUserId, string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return;
            }

            var user = await userRepository.TryGetUserAsync(telegramUserId);

            if (user == null)
            {
                await userRepository.AddUserAsync(telegramUserId, username);

                return;
            }

            await userRepository.UpdateUserAsync(telegramUserId, username);
        }

        public async Task RemoveUsernameAsync(long telegramUserId)
        {
            await userRepository.RemoveUserAsync(telegramUserId);
        }

        public async Task<string> TryGetUsernameAsync(long telegramUserId)
        {
            return await userRepository.TryGetUserAsync(telegramUserId);
        }
    }
}
