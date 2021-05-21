using System;

namespace Lastgram.Commands
{
    [Serializable]
    public class CommandException : Exception
    {
        public CommandException() : base() { }

        public CommandException(string message) : base(message) { }

        public CommandException(string message, Exception inner) : base(message, inner) { }

        protected CommandException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
