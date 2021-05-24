using Core.Domain.Services.Lastfm;
using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Lastgram.Commands
{
    public class ForgetMeCommand : ICommand
    {
        private readonly ILastfmUsernameService lastfmUsernameService;

        public ForgetMeCommand(ILastfmUsernameService lastfmUsernameService)
        {
            this.lastfmUsernameService = lastfmUsernameService;
        }

        public string CommandName => "forgetme";

        public string CommandDescription => "Remove entries related to you from the database";

        public async Task ExecuteCommandAsync(Message message, Func<Chat, string, Task> responseFunc)
        {
            await lastfmUsernameService.RemoveUsernameAsync(message.From.Id);

            await responseFunc(message.Chat, "You have been removed! 👊😎");
        }
    }
}
