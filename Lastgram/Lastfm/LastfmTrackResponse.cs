using IF.Lastfm.Core.Objects;

namespace Lastgram.Lastfm
{
    public class LastfmTrackResponse
    {
        public LastTrack Track { get; init; }

        public bool IsSuccess { get; init; }
    }
}
