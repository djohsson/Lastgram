using Lastgram.Commands;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Types;

namespace Lastgram
{
    internal static class MessageHelper
    {
        public static List<string> GetParameters(this Message message)
        {
            if (string.IsNullOrEmpty(message.Text))
            {
                return new List<string>();
            }

            return message.Text.Split(" ").Skip(1).ToList();
        }
    }
}
