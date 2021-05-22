using Lastgram.Data.Models;
using Lastgram.Spotify.Repositories;
using System.Threading.Tasks;

namespace Lastgram.Spotify
{
    public class ArtistService : IArtistService
    {
        private readonly IArtistRepository artistRepository;

        public ArtistService(IArtistRepository artistRepository)
        {
            this.artistRepository = artistRepository;
        }

        public async Task<Artist> GetOrAddArtistAsync(string artistName)
        {
            Artist artist = await artistRepository.TryGetArtistAsync(artistName);

            return artist ?? await artistRepository.AddArtistAsync(artistName);
        }
    }
}
