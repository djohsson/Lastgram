using System;

namespace Core.Domain.Models.Lastfm
{
    public record LastfmScrobble(LastfmTrack LastfmTrack, bool IsNowPlaying, DateTime? TimePlayed);
}
