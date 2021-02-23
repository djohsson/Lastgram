using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lastgram.Models
{
    public class User
    {
        [Key]
        public int TelegramUserId { get; set; }

        public string LastfmUsername { get; set; }
    }
}
