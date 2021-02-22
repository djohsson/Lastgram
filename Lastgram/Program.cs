using Autofac;
using System;

namespace Lastgram
{
    class Program
    {
        private static IContainer Container { get; set; }

        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<UserRepository>().As<IUserRepository>().SingleInstance();
            builder.RegisterType<Bot>().As<IBot>();
            builder.RegisterType<NowPlayingService>().As<INowPlayingService>().SingleInstance();
            builder.RegisterType<ForgetMeService>().As<IForgetMeService>().SingleInstance();
            builder.RegisterType<LastFmService>().As<ILastFmService>().SingleInstance();
            builder.RegisterType<SpotifyService>().As<ISpotifyService>().SingleInstance();
            builder.RegisterType<CommandHandler>().As<ICommandHandler>();
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
