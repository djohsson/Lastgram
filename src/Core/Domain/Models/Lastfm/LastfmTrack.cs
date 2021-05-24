using System;

namespace Core.Domain.Models.Lastfm
{
    public record LastfmTrack(string Name, string ArtistName, Uri Url, int? UserPlayCount);
}
