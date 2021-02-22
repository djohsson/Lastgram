﻿using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Lastgram
{
    public class ForgetMeService : IForgetMeService
    {
        private readonly IUserRepository userRepository;

        public ForgetMeService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public Task HandleCommandAsync(Message message, Func<Chat, string, Task> responseFunc)
        {
            userRepository.RemoveUser(message.From.Id);

            return Task.CompletedTask;
        }
    }
}
