using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lastgram.Data.Models
{
    public class SpotifyTrack
    {
        [Key]
        [MaxLength(32)]
        public string Md5 { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(255)]
        public string Track { get; set; }

        [Required]
        [MaxLength(128)]
        public string Url { get; set; }

        public int ArtistId { get; set; }
        public Artist Artist { get; set; }
    }
}
