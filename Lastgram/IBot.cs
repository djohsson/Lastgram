using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lastgram
{
    public interface IBot
    {
        Task StartAsync();

        void Stop();
    }
}
