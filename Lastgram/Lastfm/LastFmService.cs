using IF.Lastfm.Core.Api;
using System.Linq;
using System.Threading.Tasks;

namespace Lastgram.Lastfm
{
    public class LastfmService : ILastfmService
    {
        private readonly IUserApi userApi;

        public LastfmService(IUserApi userApi)
        {
            this.userApi = userApi;
        }

        public async Task<LastfmTrackResponse> GetNowPlayingAsync(string username)
        {
            var response = await userApi.GetRecentScrobbles(username, count: 1);

            return new LastfmTrackResponse(response.FirstOrDefault(), response.Success && response.Any());
        }
    }
}
