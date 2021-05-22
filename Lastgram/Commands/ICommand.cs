using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Lastgram.Commands
{
    public interface ICommand
    {
        string CommandName { get => "command"; }

        string CommandDescription { get => "description"; }

        /// <summary>
        /// Execute command
        /// </summary>
        /// <exception cref="CommandException">Something whent wrong when executing command</exception>
        Task ExecuteCommandAsync(Message message, Func<Chat, string, Task> responseFuncAsync);
    }
}
