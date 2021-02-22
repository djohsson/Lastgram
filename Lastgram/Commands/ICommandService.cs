using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Lastgram.Commands
{
    public interface ICommandService
    {
        Task HandleCommandAsync(Message message, Func<Chat, string, Task> responseFunc);
    }
}
