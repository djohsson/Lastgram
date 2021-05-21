using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Lastgram.Commands
{
    public interface ICommand
    {
        string CommandName { get; }

        string CommandDescription { get; }

        Task ExecuteCommandAsync(Message message, Func<Chat, string, Task> responseFuncAsync);
    }
}
