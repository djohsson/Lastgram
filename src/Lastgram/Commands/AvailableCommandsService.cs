using System;
using System.Collections.Generic;
using System.Linq;

namespace Lastgram.Commands
{
    public class AvailableCommandsService : IAvailableCommandsService
    {
        private readonly IEnumerable<ICommand> commands;

        public AvailableCommandsService(IEnumerable<ICommand> commands)
        {
            this.commands = commands;
        }

        public IReadOnlyList<ICommand> GetBotCommands()
        {
            return commands.ToList();
        }

        public bool TryParseCommandType(string text, out Type type)
        {
            type = null;

            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            var commandText = text.Split(" ").FirstOrDefault();

            if (string.IsNullOrEmpty(commandText))
            {
                return false;
            }

            commandText = RemoveBotNameFromCommand(commandText);
            commandText = commandText.ToLower();

            var command = commands.FirstOrDefault(c => c.CommandName.Equals(commandText));

            type = command?.GetType();

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
