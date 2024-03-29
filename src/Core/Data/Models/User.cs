﻿using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Data.Models
{
    [Index(nameof(TelegramUserId))]
    public class User : BaseEntity
    {
        [Required]
        public long TelegramUserId { get; set; }

        [Required]
        public string LastfmUsername { get; set; }

        public override bool Equals(object obj)
        {
            return obj is User user &&
                   TelegramUserId == user.TelegramUserId &&
                   LastfmUsername == user.LastfmUsername;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(TelegramUserId, LastfmUsername);
        }
    }
}
