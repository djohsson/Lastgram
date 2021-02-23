using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lastgram.Models
{
    public class SpotifyTrack
    {
        [Key]
        public string Md5 { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(255)]
        public string Artist { get; set; }

        [MaxLength(255)]
        public string Track { get; set; }

        [Required]
        public string Url { get; set; }
    }
}
