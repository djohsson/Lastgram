using Lastgram.Data.Repositories;
using Lastgram.Models;
using System.Threading.Tasks;

namespace Lastgram.Data
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
