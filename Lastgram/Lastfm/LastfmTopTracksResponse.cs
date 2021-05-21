using IF.Lastfm.Core.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lastgram.Lastfm
{
    public class LastfmTopTracksResponse
    {
        public IReadOnlyList<LastTrack> TopTracks { get; init; }

        public bool IsSuccess { get; init; }
    }
}
