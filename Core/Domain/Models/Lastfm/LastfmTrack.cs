using System;

namespace Core.Domain.Models.Lastfm
{
    public record LastfmTrack
    {
        public string Name { get; init; }

        public string ArtistName { get; init; }

        public Uri Url { get; init; }

        public int? UserPlayCount { get; init; }

    }
}
