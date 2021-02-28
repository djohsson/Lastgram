using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Lastgram.Commands
{
    internal class CommandHandler : ICommandHandler
    {
        private readonly IEnumerable<ICommand> commands;

        public CommandHandler(IEnumerable<ICommand> commands)
        {
            this.commands = new List<ICommand>(commands);
        }

        public async Task ExecuteCommandAsync(Message message, Func<Chat, string, Task> responseFunc)
        {
            if (!TryParseCommand(message, out ICommand command))
            {
                return;
            }

            await command.ExecuteCommandAsync(message, responseFunc);
        }

        public IReadOnlyList<BotCommand> GetBotCommands()
        {
            return commands.Select(c =>
                new BotCommand
                {
                    Command = c.CommandName,
                    Description = c.CommandDescription
                }).ToList();
        }

        private bool TryParseCommand(Message message, out ICommand command)
        {
            command = null;

            if (string.IsNullOrEmpty(message.Text))
            {
                return false;
            }

            var commandText = message.Text.Split(" ").FirstOrDefault();

            if (string.IsNullOrEmpty(commandText))
            {
                return false;
            }

            commandText = RemoveBotNameFromCommand(commandText);
            commandText = commandText.ToLower();

            command = commands.FirstOrDefault(c => c.CommandName.Equals(commandText));

            return command != null;
        }

        private static string RemoveBotNameFromCommand(string command)
        {
            int indexOfAtSign = command.IndexOf("@");

            return indexOfAtSign > 0
                ? command.Substring(1, indexOfAtSign - 1)
                : command.Substring(1);
        }
    }
}
