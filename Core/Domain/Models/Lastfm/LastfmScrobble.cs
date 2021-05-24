using System;

namespace Core.Domain.Models.Lastfm
{
    public record LastfmScrobble
    {
        public LastfmTrack LastfmTrack { get; init; }

        public bool IsNowPlaying { get; init; }

        public DateTime? TimePlayed { get; init; }
    }
}
