using System.Collections.Generic;

namespace Lastgram.Data
{
    internal class UserRepository : IUserRepository
    {
        private readonly IDictionary<int, string> lastFmUsers;

        public UserRepository()
        {
            lastFmUsers = new Dictionary<int, string>();
        }

        public void AddUser(int telegramUserId, string lastFmUsername)
        {
            if (lastFmUsers.ContainsKey(telegramUserId))
            {
                lastFmUsers[telegramUserId] = lastFmUsername;

                return;
            }

            lastFmUsers.Add(telegramUserId, lastFmUsername);
        }

        public void RemoveUser(int telegramUserId)
        {
            lastFmUsers.Remove(telegramUserId);
        }

        public bool TryGetUser(int telegramUserId, out string lastFmUsername)
        {
            return lastFmUsers.TryGetValue(telegramUserId, out lastFmUsername);
        }
    }
}
