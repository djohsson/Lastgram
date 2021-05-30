using Lastgram.Commands;
using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace LastgramTest.Commands.Mocks
{
    internal class CommandMock : ICommand
    {
        private readonly Chat chat;
        private readonly string response;

        public CommandMock()
        {
        }

        public CommandMock(Chat chat, string response)
        {
            this.chat = chat;
            this.response = response;
        }

        public async Task ExecuteCommandAsync(Message message, Func<Chat, string, Task> responseFuncAsync)
        {
            await responseFuncAsync(chat, response);
        }
    }
}
