namespace Lastgram
{
    public interface IUserRepository
    {
        void AddUser(int telegramUserId, string lastFmUsername);

        bool TryGetUser(int telegramUserId, out string lastFmUsername);

        void RemoveUser(int telegramUserId);
    }
}
