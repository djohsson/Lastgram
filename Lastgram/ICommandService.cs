using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Lastgram
{
    public interface ICommandService
    {
        Task HandleCommandAsync(Message message, Func<Chat, string, Task> responseFunc);
    }
}
