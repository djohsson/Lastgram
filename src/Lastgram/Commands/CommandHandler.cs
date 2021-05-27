using Microsoft.Extensions.DependencyInjection;
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
        private readonly IServiceProvider serviceProvider;

        public CommandHandler(IServiceProvider serviceProvider, IEnumerable<ICommand> commands)
        {
            this.commands = new List<ICommand>(commands);
            this.serviceProvider = serviceProvider;
        }

        public async Task ExecuteCommandAsync(Message message, Func<Chat, string, Task> responseFunc)
        {
            if (!TryParseCommandType(message, out Type type))
            {
                return;
            }

            try
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var command = (ICommand)scope.ServiceProvider.GetRequiredService(type);

                    await command.ExecuteCommandAsync(message, responseFunc);
                }
            }
            catch (CommandException e)
            {
                await responseFunc(message.Chat, e.Message);
            }
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

        private bool TryParseCommandType(Message message, out Type type)
        {
            type = null;

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

            var command = commands.FirstOrDefault(c => c.CommandName.Equals(commandText));
            type = command.GetType();

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
