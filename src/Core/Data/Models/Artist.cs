using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Data.Models
{
    [Index(nameof(Name))]
    public class Artist : BaseEntity
    {
        [MaxLength(255)]
        [Required]
        public string Name { get; set; }

        public List<SpotifyTrack> Tracks { get; set; }
    }
}
