using Lastgram.Commands;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Lastgram
{
    public class Bot : IBot
    {
        private readonly ITelegramBotClient telegramBotClient;
        private readonly HttpClient httpClient;
        private readonly ICommandHandler commandHandler;
        private readonly IAvailableCommandsService availableCommandsService;

        private DateTime started;

        public Bot(
            ICommandHandler commandHandler,
            IAvailableCommandsService availableCommandsService)
        {
            this.commandHandler = commandHandler;

            httpClient = new HttpClient();

            string apiKey = GetApiKey();

            telegramBotClient = new TelegramBotClient(apiKey, httpClient);
            this.availableCommandsService = availableCommandsService;
        }

        public async Task StartAsync()
        {
            started = DateTime.UtcNow;

            var commands = availableCommandsService.GetBotCommands();

            var botCommands = commands.Select(c => new BotCommand
            {
                Command = c.CommandName,
                Description = c.CommandDescription,
            });

            await telegramBotClient.SetMyCommandsAsync(botCommands);

            telegramBotClient.OnMessage += OnMessage;
            telegramBotClient.StartReceiving();
        }

        public void Stop()
        {
            telegramBotClient.StopReceiving();
            telegramBotClient.OnMessage -= OnMessage;
        }

        private async void OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Date.ToUniversalTime() < started)
            {
                return;
            }

            await ExecuteCommandAsync(e);
        }

        private async Task ExecuteCommandAsync(MessageEventArgs eventArgs)
        {
            try
            {
                await commandHandler.ExecuteCommandAsync(eventArgs.Message, SendMessageAsync);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                await SendMessageAsync(eventArgs.Message.Chat, "Oops, something went wrong 😢");
            }
        }

        private async Task SendMessageAsync(Chat chat, string text)
        {
            await telegramBotClient.SendTextMessageAsync(
                chatId: chat,
                text: text,
                parseMode: ParseMode.Html,
                disableWebPagePreview: true,
                disableNotification: true
            );
        }

        private static string GetApiKey()
        {
            string apiKey;
#if DEBUG
            apiKey = Environment.GetEnvironmentVariable("LASTGRAM_TELEGRAM_DEBUG_KEY");
#else
            apiKey = Environment.GetEnvironmentVariable("LASTGRAM_TELEGRAM_KEY");
#endif
            return apiKey;
        }
    }
}
