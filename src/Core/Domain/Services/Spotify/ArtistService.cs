using Core.Data.Models;
using Core.Domain.Repositories.Spotify;
using System.Threading.Tasks;

namespace Core.Domain.Services.Spotify
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
