using IF.Lastfm.Core.Objects;
using System.Threading.Tasks;

namespace Lastgram.Response
{
    public interface ITrackResponseService
    {
        string GetResponseForTrack(LastTrack topTrack, string url);
    }
}
