using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DnDSekai.Core.Config;

namespace DnDSekai.Discord
{
    public class CommandHandler
    {
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _client;
        private readonly IServiceProvider _services;

        public CommandHandler(IServiceProvider services)
        {
            _commands = services.GetRequiredService<CommandService>();
            _client = services.GetRequiredService<DiscordSocketClient>();
            _services = services;

            _commands.CommandExecuted += CommandExecutedAsync;

            _client.MessageReceived += MessageReceivedAsync;
        }

        public async Task InitializeAsync()
        {
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        public async Task MessageReceivedAsync(SocketMessage rawMessage)
        {
            // Ensures we don't process system/other bot messages
            if (!(rawMessage is SocketUserMessage message))
            {
                return;
            }

            if (message.Source != MessageSource.User)
            {
                return;
            }

            // Sets the argument position away from the prefix we set
            var argPos = 0;


            // Determine if the message has a valid prefix, and adjust argPos based on prefix
            if (!message.HasStringPrefix(Config.bot.cmdPrefix, ref argPos))
            {
                return;
            }

            var context = new SocketCommandContext(_client, message);

            // Execute command if one is found that matches
            await _commands.ExecuteAsync(context, argPos, _services);
        }

        public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            // If a command isn't found, log that info to console and exit this method
            if (!command.IsSpecified)
            {
                System.Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")} Commands    Command failed to execute for {context.User.Username} | {result.ErrorReason}");
                return;
            }

            // Log success to the console and exit this method
            if (result.IsSuccess)
            {
                System.Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")} Commands    Command [{command.Value.Name}] executed for {context.User.Username}");
                return;
            }

            // Failure scenario, let's let the user know
            await context.Channel.SendMessageAsync($"Error -> [{result}]");
        }
    }
}