using Lastgram.Commands;
using System;
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

        private DateTime started;

        public Bot(ICommandHandler commandHandler)
        {
            this.commandHandler = commandHandler;

            httpClient = new HttpClient();
            string apiKey = Environment.GetEnvironmentVariable("LASTGRAM_TELEGRAM_KEY");
            telegramBotClient = new TelegramBotClient(apiKey, httpClient);
        }

        public async Task StartAsync()
        {
            started = DateTime.UtcNow;

            var commands = commandHandler.GetBotCommands();
            await telegramBotClient.SetMyCommandsAsync(commands);

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

                await SendMessageAsync(eventArgs.Message.Chat, "Oops, something went wrong!");
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
    }
}
