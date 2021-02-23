using Lastgram.Data;
using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Lastgram.Commands
{
    public class ForgetMeService : IForgetMeService
    {
        private readonly IUserRepository userRepository;

        public ForgetMeService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task HandleCommandAsync(Message message, Func<Chat, string, Task> responseFunc)
        {
            await userRepository.RemoveUserAsync(message.From.Id);
        }
    }
}
