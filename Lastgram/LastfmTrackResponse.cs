using IF.Lastfm.Core.Objects;

namespace Lastgram
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
