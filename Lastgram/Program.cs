using Autofac;
using IF.Lastfm.Core.Api;
using Lastgram.Commands;
using Lastgram.Data;
using Lastgram.Data.Repositories;
using Lastgram.Lastfm;
using Lastgram.Spotify;
using Microsoft.EntityFrameworkCore;
using SpotifyAPI.Web;
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
            RegisterLastAuth(builder);
            RegisterCommands(builder);
            RegisterRepositories(builder);
            RegisterServices(builder);

            builder.RegisterType<Bot>().As<IBot>();
            builder.RegisterType<UserApi>().As<IUserApi>().SingleInstance();

            Container = builder.Build();
        }

        private static void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<LastfmService>().As<ILastfmService>().SingleInstance();
            builder.RegisterType<ArtistService>().As<IArtistService>().SingleInstance();

            RegisterSpotifyService(builder);
        }

        private static void RegisterSpotifyService(ContainerBuilder builder)
        {
            builder.Register<Func<SpotifyClientConfig, ISpotifyClient>>(c =>
            {
                return config => new SpotifyClient(config);
            });

            builder.Register<Func<ClientCredentialsRequest>>(c =>
            {
                return () => new ClientCredentialsRequest(
                    Environment.GetEnvironmentVariable("LASTGRAM_SPOTIFY_CLIENTID"),
                    Environment.GetEnvironmentVariable("LASTGRAM_SPOTIFY_CLIENTSECRET"));
            });

            builder.RegisterType<OAuthClient>().As<IOAuthClient>();

            builder.RegisterType<SpotifyService>().As<ISpotifyService>().SingleInstance();
        }

        private static void RegisterRepositories(ContainerBuilder builder)
        {
            builder.RegisterType<UserRepository>().As<IUserRepository>().SingleInstance();
            builder.RegisterType<SpotifyTrackRepository>().As<ISpotifyTrackRepository>().SingleInstance();
            builder.RegisterType<ArtistRepository>().As<IArtistRepository>().SingleInstance();
        }

        private static void RegisterCommands(ContainerBuilder builder)
        {
            builder.RegisterType<CommandHandler>().As<ICommandHandler>();
            builder.RegisterType<ForgetMeCommand>().As<ICommand>().SingleInstance();
            builder.RegisterType<NowPlayingCommand>().As<ICommand>().SingleInstance();
        }

        private static void RegisterLastAuth(ContainerBuilder builder)
        {
            var auth = new LastAuth(
                Environment.GetEnvironmentVariable("LASTGRAM_LASTFM_APIKEY"),
                Environment.GetEnvironmentVariable("LASTGRAM_LASTFM_APISECRET"));

            builder.RegisterInstance(auth).As<ILastAuth>();
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
