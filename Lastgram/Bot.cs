using Lastgram.Commands;
using System;
using System.Collections.Generic;
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

        private IReadOnlyList<BotCommand> commands;
        private DateTime started;

        public Bot(ICommandHandler commandHandler)
        {
            this.commandHandler = commandHandler;

            httpClient = new HttpClient();
            string apiKey = Environment.GetEnvironmentVariable("LASTGRAM_TELEGRAM_KEY");
            telegramBotClient = new TelegramBotClient(apiKey, httpClient);

            CreateCommands();
        }

        public async Task StartAsync()
        {
            started = DateTime.UtcNow;

            await telegramBotClient.SetMyCommandsAsync(commands);
            telegramBotClient.OnMessage += OnMessage;
            telegramBotClient.StartReceiving();
        }

        public void Stop()
        {
            telegramBotClient.StopReceiving();
            telegramBotClient.OnMessage -= OnMessage;
        }

        private void CreateCommands()
        {
            commands = new List<BotCommand>
            {
                new BotCommand
                {
                    Command = "np",
                    Description = "Get a Spotify link to the currently playing song"
                },
                new BotCommand
                {
                    Command = "forgetme",
                    Description = "Remove entries related to you from the database"
                },
            };
        }

        private async void OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Date.ToUniversalTime() < started)
            {
                return;
            }

            if (!MessageHelper.TryParseCommandType(e.Message, out var command))
            {
                return;
            }

            await ExecuteCommand(e, command);
        }

        private async Task ExecuteCommand(MessageEventArgs eventArgs, CommandType commandType)
        {
            try
            {
                await commandHandler.HandleCommandAsync(commandType, eventArgs.Message, SendMessageAsync);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
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
