using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

using Discord.WebSocket;
using Discord.Commands;
using Discord;

using DnDSekai.Core;

namespace DnDSekai
{
    class Program
    {
        private DiscordSocketClient _client;

        static void Main(string[] args)
        {
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        public async Task MainAsync()
        {
            var services = ConfigureServices();
            var client = services.GetRequiredService<DiscordSocketClient>();
            _client = client;

            client.Log += LogAsync;
            client.Ready += ReadyAsync;
            services.GetRequiredService<CommandService>().Log += LogAsync;

            await client.LoginAsync(TokenType.Bot, Config.bot.token);
            await client.StartAsync();

            await services.GetRequiredService<CommandHandler>().InitializeAsync();

            await Task.Delay(-1);
        }

        private Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }

        private Task ReadyAsync()
        {
            Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")} Program     Connected as {_client.CurrentUser}");
            return Task.CompletedTask;
        }

        public IServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandler>()
                .AddSingleton(new AudioService())
                .BuildServiceProvider();
        }
    }
}