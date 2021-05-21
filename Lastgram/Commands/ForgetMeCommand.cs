using Lastgram.Data.Repositories;
using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Lastgram.Commands
{
    public class ForgetMeCommand : IForgetMeCommand
    {
        private readonly IUserRepository userRepository;

        public ForgetMeCommand(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public string CommandName => "forgetme";

        public string CommandDescription => "Remove entries related to you from the database";

        public async Task ExecuteCommandAsync(Message message, Func<Chat, string, Task> responseFunc)
        {
            await userRepository.RemoveUserAsync(message.From.Id);

            await responseFunc(message.Chat, "You have been removed! 👊😎");
        }
    }
}
