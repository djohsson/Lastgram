using System;
using System.ComponentModel.DataAnnotations;

namespace Lastgram.Models
{
    public class SpotifyTrack
    {
        [Key]
        public string Md5 { get; set; }

        [MaxLength(255)]
        public string Artist { get; set; }

        [MaxLength(255)]
        public string Track { get; set; }

        [Required]
        public string Url { get; set; }

        [Required]
        public DateTime ValidUntil { get; set; }
    }
}
