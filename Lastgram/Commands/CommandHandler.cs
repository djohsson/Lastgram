using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Lastgram.Commands
{
    internal class CommandHandler : ICommandHandler
    {
        private readonly INowPlayingService nowPlayingService;
        private readonly IForgetMeService forgetMeService;

        public CommandHandler(INowPlayingService nowPlayingService,
            IForgetMeService forgetMeService)
        {
            this.nowPlayingService = nowPlayingService;
            this.forgetMeService = forgetMeService;
        }

        public async Task HandleCommandAsync(CommandType commandType, Message message, Func<Chat, string, Task> responseFunc)
        {
            switch (commandType)
            {
                case CommandType.NowPlaying:
                    await nowPlayingService.HandleCommandAsync(message, responseFunc);
                    break;
                case CommandType.ForgetMe:
                    await forgetMeService.HandleCommandAsync(message, responseFunc);
                    break;
                case CommandType.Unknown:
                    break;
                default:
                    throw new ArgumentException($"Unknown command type {commandType}");
            }
        }
    }
}
