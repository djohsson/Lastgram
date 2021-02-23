using System;
using System.ComponentModel.DataAnnotations;

namespace Lastgram.Models
{
    public class SpotifyTrack
    {
        [Key]
        public string Md5 { get; set; }

        public string Url { get; set; }

        public DateTime ValidUntil { get; set; }
    }
}
