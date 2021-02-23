using Autofac;
using Lastgram.Commands;
using Lastgram.Data;
using Lastgram.Lastfm;
using Lastgram.Spotify;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Lastgram
{
    public class Program
    {
        private static IContainer Container { get; set; }

        public static async Task Main()
        {
            RegisterTypes();

            using (var scope = Container.BeginLifetimeScope())
            {
                ApplyMigrations(scope);

                var bot = scope.Resolve<IBot>();

                await bot.StartAsync();

                await Task.Run(() => Thread.Sleep(Timeout.Infinite));
            }
        }

        private static void RegisterTypes()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Bot>().As<IBot>();
            builder.RegisterType<MyDbContext>().SingleInstance();
            builder.RegisterType<UserRepository>().As<IUserRepository>().SingleInstance();
            builder.RegisterType<CommandHandler>().As<ICommandHandler>();
            builder.RegisterType<NowPlayingService>().As<INowPlayingService>().SingleInstance();
            builder.RegisterType<ForgetMeService>().As<IForgetMeService>().SingleInstance();
            builder.RegisterType<LastFmService>().As<ILastFmService>().SingleInstance();
            builder.RegisterType<SpotifyService>().As<ISpotifyService>().SingleInstance();
            builder.RegisterType<SpotifyTrackRepository>().As<ISpotifyTrackRepository>().SingleInstance();
            Container = builder.Build();
        }

        private static void ApplyMigrations(ILifetimeScope scope)
        {
            var dbContext = scope.Resolve<MyDbContext>();
            dbContext.Database.Migrate();
        }
    }
}
