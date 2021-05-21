using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Lastgram.Commands
{
    public interface ICommand
    {
        string CommandName { get => "/command"; }

        string CommandDescription { get => "description"; }

        Task ExecuteCommandAsync(Message message, Func<Chat, string, Task> responseFuncAsync);
    }
}
