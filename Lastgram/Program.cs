using Autofac;
using Lastgram.Commands;
using Lastgram.Data;
using Lastgram.Lastfm;
using Lastgram.Spotify;
using System;

namespace Lastgram
{
    class Program
    {
        private static IContainer Container { get; set; }

        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Bot>().As<IBot>();
            builder.RegisterType<UserRepository>().As<IUserRepository>().SingleInstance();
            builder.RegisterType<CommandHandler>().As<ICommandHandler>();
            builder.RegisterType<NowPlayingService>().As<INowPlayingService>().SingleInstance();
            builder.RegisterType<ForgetMeService>().As<IForgetMeService>().SingleInstance();
            builder.RegisterType<LastFmService>().As<ILastFmService>().SingleInstance();
            builder.RegisterType<SpotifyService>().As<ISpotifyService>().SingleInstance();
            Container = builder.Build();

            using (var scope = Container.BeginLifetimeScope())
            {
                var bot = scope.Resolve<IBot>();

                bot.StartAsync().Wait();

                Console.ReadKey();
            } 
        }
    }
}
