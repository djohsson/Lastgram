using System;
using System.ComponentModel.DataAnnotations;

namespace Lastgram.Data.Models
{
    public class User
    {
        [Key]
        public int TelegramUserId { get; set; }

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
