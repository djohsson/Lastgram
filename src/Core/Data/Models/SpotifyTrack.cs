using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Data.Models
{
    [Index(nameof(Track))]
    public class SpotifyTrack : BaseEntity
    {
        [MaxLength(255)]
        public string Track { get; set; }

        [Required]
        [MaxLength(128)]
        public string Url { get; set; }

        public Guid ArtistId { get; set; }
        public Artist Artist { get; set; }
    }
}
