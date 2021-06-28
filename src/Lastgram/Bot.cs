using Lastgram.Commands;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
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
        private CancellationTokenSource cts;

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
            cts = new();
            CancellationToken ct = cts.Token;

            var commands = availableCommandsService.GetBotCommands();

            var botCommands = commands.Select(c => new BotCommand
            {
                Command = c.CommandName,
                Description = c.CommandDescription,
            });

            await telegramBotClient.SetMyCommandsAsync(botCommands, ct);

            telegramBotClient.StartReceiving(new DefaultUpdateHandler(HandleUpdateAsync, HandleErrorAsync), ct);
        }

        public void Stop()
        {
            cts.Cancel();
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (update.Message is not Message message)
            {
                return;
            }

            if (message.Date.ToUniversalTime() < started)
            {
                return;
            }

            await ExecuteCommandAsync(message);
        }

        private static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private async Task ExecuteCommandAsync(Message message)
        {
            try
            {
                await commandHandler.ExecuteCommandAsync(message, SendMessageAsync);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                await SendMessageAsync(message.Chat, "Oops, something went wrong 😢");
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
