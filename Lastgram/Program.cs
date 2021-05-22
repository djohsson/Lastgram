using Autofac;
using IF.Lastfm.Core.Api;
using Lastgram.Commands;
using Lastgram.Data;
using Lastgram.Lastfm;
using Lastgram.Lastfm.Repositories;
using Lastgram.Spotify;
using Lastgram.Spotify.Repositories;
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
                await ApplyMigrationsAsync(scope);

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
            RegisterServices(builder);

            builder.RegisterType<Bot>().As<IBot>();
            builder.RegisterType<UserApi>().As<IUserApi>().SingleInstance();

            Container = builder.Build();
        }

        private static void RegisterServices(ContainerBuilder builder)
        {
            RegisterLastfmServices(builder);
            RegisterSpotifyServices(builder);
        }

        private static void RegisterLastfmServices(ContainerBuilder builder)
        {
            builder.RegisterType<LastfmUsernameRepository>().As<ILastfmUsernameRepository>().SingleInstance();
            builder.RegisterType<LastfmService>().As<ILastfmService>().SingleInstance();
            builder.RegisterType<LastfmUsernameService>().As<ILastfmUsernameService>().SingleInstance();
        }

        private static void RegisterSpotifyServices(ContainerBuilder builder)
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
            builder.RegisterType<ArtistService>().As<IArtistService>().SingleInstance();

            builder.RegisterType<SpotifyTrackRepository>().As<ISpotifyTrackRepository>().SingleInstance();
            builder.RegisterType<ArtistRepository>().As<IArtistRepository>().SingleInstance();
        }

        private static void RegisterCommands(ContainerBuilder builder)
        {
            builder.RegisterType<CommandHandler>().As<ICommandHandler>();
            builder.RegisterType<ForgetMeCommand>().As<ICommand>().SingleInstance();
            builder.RegisterType<NowPlayingCommand>().As<ICommand>().SingleInstance();
            builder.RegisterType<TopTracksCommand>().As<ICommand>().SingleInstance();
            builder.RegisterType<SetLastfmUsernameCommand>().As<ICommand>().SingleInstance();
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

        private static async Task ApplyMigrationsAsync(ILifetimeScope scope)
        {
            var dbContext = scope.Resolve<IMyDbContext>();
            await dbContext.Database.MigrateAsync();
        }
    }
}
