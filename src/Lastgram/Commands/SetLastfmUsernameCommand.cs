using Core.Domain.Services.Lastfm;
using Lastgram.Utils;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Telegram.Bot.Types;

namespace Lastgram.Commands
{
    public class SetLastfmUsernameCommand : ICommand
    {
        private const int MAX_USERNAME_LENGTH = 255;

        private readonly ILastfmUsernameService lastfmUsernameService;

        public SetLastfmUsernameCommand(ILastfmUsernameService lastfmUsenamerService)
        {
            this.lastfmUsernameService = lastfmUsenamerService;
        }

        public string CommandName => "setusername";

        public string CommandDescription => "Set a Last.fm username to associate with your Telegram account";

        public async Task ExecuteCommandAsync(Message message, Func<Chat, string, Task> responseFunc)
        {
            string username = message.GetParameters().FirstOrDefault();

            if (string.IsNullOrEmpty(username))
            {
                throw new CommandException("Please specify a last.fm username 🕵️‍♀️");
            }

            if (username.Length > MAX_USERNAME_LENGTH)
            {
                throw new CommandException("Too long username 📏");
            }

            await lastfmUsernameService.AddOrUpdateUsernameAsync(message.From.Id, username);

            string escapedUsername = HttpUtility.HtmlEncode(username);

            await responseFunc(message.Chat, $"Registered <i>{escapedUsername}</i> 🥳");
        }
    }
}
