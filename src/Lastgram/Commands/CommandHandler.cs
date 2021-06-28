using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;

namespace Lastgram.Commands
{
    public class CommandHandler : ICommandHandler
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IAvailableCommandsService availableCommandsService;

        public CommandHandler(
            IServiceProvider serviceProvider,
            IAvailableCommandsService availableCommandsService)
        {
            this.serviceProvider = serviceProvider;
            this.availableCommandsService = availableCommandsService;
        }

        public async Task ExecuteCommandAsync(Message message, Func<Chat, string, Task> responseFunc)
        {
            if (!availableCommandsService.TryParseCommandType(message.Text, out Type type))
            {
                return;
            }

            try
            {
                using var scope = serviceProvider.CreateScope();

                var command = (ICommand)scope.ServiceProvider.GetRequiredService(type);

                await command.ExecuteCommandAsync(message, responseFunc);
            }
            catch (CommandException e)
            {
                await responseFunc(message.Chat, e.Message);
            }
        }
    }
}
