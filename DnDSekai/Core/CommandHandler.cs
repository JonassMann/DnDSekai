using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Collections.Generic;
using System.Linq;

namespace DnDSekai.Core
{
    public class CommandHandler
    {
        private static CommandService _commands;
        private readonly DiscordSocketClient _client;
        private readonly IServiceProvider _services;

        public static List<CommandInfo> GetCommandInfo()
        {
            List<CommandInfo> commands = _commands.Commands.ToList();
            return commands;
        }

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
            if (Users.users.ContainsKey(message.Author.Id))
            {
                if (Users.users[message.Author.Id].prefix && !message.HasStringPrefix(Config.bot.cmdPrefix, ref argPos))
                {
                    return;
                }
            }
            else if (!message.HasStringPrefix(Config.bot.cmdPrefix, ref argPos))
            {
                return;
            }

            var context = new SocketCommandContext(_client, message);

            string newContent = message.Content;
            if(newContent.StartsWith("!")) newContent = newContent.Substring(1);

            if (Users.users.ContainsKey(message.Author.Id) && newContent.Contains(Users.users[message.Author.Id].shortcutSymbol))
            {
                string[] words = newContent.Split(' ');

                newContent = "";

                foreach (string s in words)
                {
                    string code = s.Substring(s.IndexOf(Users.users[message.Author.Id].shortcutSymbol) + 1);

                    if (s.Contains(Users.users[message.Author.Id].shortcutSymbol) && Users.users[message.Author.Id].shortcuts.ContainsKey(code))
                        newContent += s.Replace(Users.users[message.Author.Id].shortcutSymbol + code, Users.users[message.Author.Id].shortcuts[code]) + " ";
                    else
                        newContent += s + " ";
                }
                
                newContent = newContent[0..^1];
            }
            
            await _commands.ExecuteAsync(context, newContent, _services);
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