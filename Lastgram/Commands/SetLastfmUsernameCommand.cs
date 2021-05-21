using Lastgram.Data.Repositories;
using Lastgram.Utils;
using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Lastgram.Commands
{
    public class SetLastfmUsernameCommand : ICommand
    {
        private const int MAX_USERNAME_LENGTH = 255;

        private readonly IUserRepository userRepository;

        public SetLastfmUsernameCommand(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public string CommandName => "setusername";

        public string CommandDescription => "Set a Last.fm username to associate with your Telegram account";

        public async Task ExecuteCommandAsync(Message message, Func<Chat, string, Task> responseFunc)
        {
            string username = message.GetParameters().FirstOrDefault();

            if (string.IsNullOrEmpty(username))
            {
                await responseFunc(message.Chat, "Please specify a last.fm username 🕵️‍♀️");
                return;
            }

            if (username.Length > MAX_USERNAME_LENGTH)
            {
                await responseFunc(message.Chat, "Too long username 📏");
                return;
            }

            await userRepository.AddOrUpdateUserAsync(message.From.Id, username);

            await responseFunc(message.Chat, $"Registered <i>{username}</i> 🥳");
        }
    }
}
