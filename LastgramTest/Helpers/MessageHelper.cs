using Lastgram.Commands;
using Telegram.Bot.Types;

namespace LastgramTest.Helpers
{
    public static class MessageHelper
    {
        public static Message CreateCommandMessage(
            ICommand command,
            int id = 0,
            string parameters = "")
        {
            return CreateMessage(
                id,
                $"/{command.CommandName} {parameters}");
        }

        public static Message CreateMessage(
            int id = 0,
            string text = "")
        {
            return new Message()
            {
                From = new User()
                {
                    Id = id
                },
                Text = text,
            };
        }
    }
}
