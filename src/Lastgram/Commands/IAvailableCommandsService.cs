using System;
using System.Collections.Generic;

namespace Lastgram.Commands
{
    public interface IAvailableCommandsService
    {
        public IReadOnlyList<ICommand> GetBotCommands();

        public bool TryParseCommandType(string text, out Type type);
    }
}
