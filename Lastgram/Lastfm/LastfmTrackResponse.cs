using IF.Lastfm.Core.Objects;

namespace Lastgram.Lastfm
{
    public class LastfmTrackResponse
    {
        public LastfmTrackResponse(LastTrack track, bool success)
        {
            Track = track;
            Success = success;
        }

        public LastTrack Track { get; }

        public bool Success { get; }
    }
}
