using Core.Data;
using Core.Domain.Repositories.Lastfm;
using Core.Domain.Repositories.Spotify;
using Core.Domain.Services.Lastfm;
using Core.Domain.Services.Spotify;
using IF.Lastfm.Core.Api;
using Lastgram.Commands;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SpotifyAPI.Web;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lastgram
{
    public class Program
    {
        public static async Task Main()
        {
            ServiceCollection services = new();
            ConfigureServices(services);

            var provider = services.BuildServiceProvider();

            await ApplyMigrationsAsync(provider);

            await RunBotAsync(provider);
        }

        private static async Task RunBotAsync(ServiceProvider provider)
        {
            var bot = provider.GetRequiredService<IBot>();

            await bot.StartAsync();

            await Task.Run(() => Thread.Sleep(Timeout.Infinite));
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            services
                .AddDbContext<IMyDbContext, MyDbContext>(
                    options => options.UseNpgsql(Environment.GetEnvironmentVariable("LASTGRAM_CONNECTIONSTRING")))
                .AddTransient<ILastfmUsernameRepository, LastfmUsernameRepository>()
                .AddTransient<IArtistRepository, ArtistRepository>()
                .AddTransient<ISpotifyTrackRepository, SpotifyTrackRepository>()
                .AddTransient<ILastfmService, LastfmService>()
                .AddTransient<ILastfmUsernameService, LastfmUsernameService>()
                .AddTransient<IArtistService, ArtistService>()
                .AddSingleton<ISpotifyService, SpotifyService>()
                .AddTransient<IUserApi, UserApi>()
                .AddTransient<ITrackApi, TrackApi>()
                .AddTransient<IOAuthClient, OAuthClient>()
                .AddTransient<ICommandHandler, CommandHandler>()
                .AddTransient<ICommand, ForgetMeCommand>()
                .AddTransient<ForgetMeCommand>()
                .AddTransient<ICommand, NowPlayingCommand>()
                .AddTransient<NowPlayingCommand>()
                .AddTransient<ICommand, TopTracksCommand>()
                .AddTransient<TopTracksCommand>()
                .AddTransient<ICommand, SetLastfmUsernameCommand>()
                .AddTransient<SetLastfmUsernameCommand>()
                .AddTransient<IBot, Bot>()
                .AddTransient<Func<SpotifyClientConfig, ISpotifyClient>>(c =>
                {
                    return config => new SpotifyClient(config);
                })
                .AddTransient<Func<ClientCredentialsRequest>>(c =>
                {
                    return () => new ClientCredentialsRequest(
                        Environment.GetEnvironmentVariable("LASTGRAM_SPOTIFY_CLIENTID"),
                        Environment.GetEnvironmentVariable("LASTGRAM_SPOTIFY_CLIENTSECRET"));
                })
                .AddSingleton<ILastAuth>(new LastAuth(
                    Environment.GetEnvironmentVariable("LASTGRAM_LASTFM_APIKEY"),
                    Environment.GetEnvironmentVariable("LASTGRAM_LASTFM_APISECRET")));
        }

        private static async Task ApplyMigrationsAsync(ServiceProvider provider)
        {
            var dbContext = provider.GetRequiredService<IMyDbContext>();
            await dbContext.Database.MigrateAsync();
        }
    }
}
