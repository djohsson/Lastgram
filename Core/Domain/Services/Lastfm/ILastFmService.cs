using Core.Domain.Models.Lastfm;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Domain.Services.Lastfm
{
    public interface ILastfmService
    {
        /// <summary>
        /// Get latest scrobble from the specified username @ lastfm
        /// </summary>
        /// <returns>null if fail</returns>
        Task<LastfmScrobble> GetLatestScrobbleAsync(string username);

        /// <summary>
        /// Get weekly top tracks from the specified username @ lastfm
        /// </summary>
        /// <returns>Empty list if fail</returns>
        Task<IReadOnlyList<LastfmTrack>> GetTopTracksAsync(string username);

        /// <summary>
        /// Get the amount of time the specified user has scrobbled a song
        /// </summary>
        /// <returns>null if fail</returns>
        Task<int?> GetPlayedCountAsync(string trackName, string artistName, string username);
    }
}
