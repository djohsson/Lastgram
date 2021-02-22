using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Types;

namespace Lastgram
{
    internal static class MessageHelper
    {
        public static bool TryParseCommandType(Message message, out CommandType commandType)
        {
            commandType = CommandType.Unknown;

            if (string.IsNullOrEmpty(message.Text))
            {
                return false;
            }

            var command = message.Text.Split(" ").FirstOrDefault();

            if (string.IsNullOrEmpty(command))
            {
                return false;
            }

            command = RemoveBotNameFromCommand(command);

            switch (command.ToLowerInvariant())
            {
                case "np":
                    commandType = CommandType.NowPlaying;
                    break;

                case "forgetme":
                    commandType = CommandType.ForgetMe;
                    break;

                default:
                    commandType = CommandType.Unknown;
                    return false;
            }

            return true;
        }

        public static List<string> GetParameters(this Message message)
        {
            if (string.IsNullOrEmpty(message.Text))
            {
                return new List<string>();
            }

            return message.Text.Split(" ").Skip(1).ToList();
        }

        private static string RemoveBotNameFromCommand(string command)
        {
            int indexOfAtSign = command.IndexOf("@");

            return indexOfAtSign > 0
                ? command.Substring(1, indexOfAtSign - 1)
                : command.Substring(1);
        }
    }
}
