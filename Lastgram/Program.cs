using Autofac;
using Lastgram.Commands;
using Lastgram.Data;
using Lastgram.Lastfm;
using Lastgram.Spotify;
using Microsoft.EntityFrameworkCore;
using System;
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
            RegisterDbContext(builder);
            builder.RegisterType<Bot>().As<IBot>();
            builder.RegisterType<UserRepository>().As<IUserRepository>().SingleInstance();
            builder.RegisterType<CommandHandler>().As<ICommandHandler>();
            builder.RegisterType<ForgetMeCommand>().As<ICommand>().SingleInstance();
            builder.RegisterType<NowPlayingCommand>().As<ICommand>().SingleInstance();
            builder.RegisterType<LastfmService>().As<ILastfmService>().SingleInstance();
            builder.RegisterType<SpotifyService>().As<ISpotifyService>().SingleInstance();
            builder.RegisterType<SpotifyTrackRepository>().As<ISpotifyTrackRepository>().SingleInstance();
            Container = builder.Build();
        }

        private static void RegisterDbContext(ContainerBuilder builder)
        {
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseNpgsql(Environment.GetEnvironmentVariable("LASTGRAM_CONNECTIONSTRING"))
                .Options;

            var context = new MyDbContext(options);

            builder.RegisterInstance(context).As<IMyDbContext>().SingleInstance();
        }

        private static void ApplyMigrations(ILifetimeScope scope)
        {
            var dbContext = scope.Resolve<IMyDbContext>();
            dbContext.Database.Migrate();
        }
    }
}
