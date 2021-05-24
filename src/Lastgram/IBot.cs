using System.Threading.Tasks;

namespace Lastgram
{
    public interface IBot
    {
        Task StartAsync();

        void Stop();
    }
}
