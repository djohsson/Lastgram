using System.ComponentModel.DataAnnotations;

namespace Lastgram.Models
{
    public class User
    {
        [Key]
        public int TelegramUserId { get; set; }

        [Required]
        public string LastfmUsername { get; set; }
    }
}
