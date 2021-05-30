using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Lastgram.Commands
{
    public interface ICommandHandler
    {
        Task ExecuteCommandAsync(Message message, Func<Chat, string, Task> responseFunc);
    }
}
