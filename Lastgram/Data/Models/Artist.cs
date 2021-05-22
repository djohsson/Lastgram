using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lastgram.Data.Models
{
    public class Artist
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(255)]
        public string Name { get; set; }

        public List<SpotifyTrack> Tracks { get; set; }
    }
}
