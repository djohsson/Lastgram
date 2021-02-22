namespace Lastgram.Spotify
{
    public class SpotifySearchResponse
    {
        public SpotifySearchResponse(bool success, string url = default)
        {
            Success = success;
            Url = url;
        }

        public bool Success { get; }

        public string Url { get; }
    }
}
