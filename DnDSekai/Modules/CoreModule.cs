using System.Threading.Tasks;
using System.Collections.Generic;

using DnDSekai.Core;
using Discord.Commands;
using Discord;

namespace DnDSekai.Modules
{
    public class CoreModule : ModuleBase<SocketCommandContext>
    {
        [Command("adduser", RunMode = RunMode.Async)]
        [Summary("Adds user to user list")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task AddUser(ulong id)
        {
            Users.AddUser(id);

            var m = await Context.Channel.SendMessageAsync($"Added user {Context.Guild.GetUser(id)}");
            await Task.Delay(Config.bot.delay);
            await m.DeleteAsync();
            await Context.Message.DeleteAsync();
        }

        [Command("removeuser", RunMode = RunMode.Async)]
        [Summary("Removes user from user list")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task RemoveUser(ulong id)
        {
            Users.RemoveUser(id);

            var m = await Context.Channel.SendMessageAsync($"Removed user {Context.Guild.GetUser(id)}");
            await Task.Delay(Config.bot.delay);
            await m.DeleteAsync();
            await Context.Message.DeleteAsync();
        }

        [Command("world", RunMode = RunMode.Async)]
        [Summary("Changes current world")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task World([Remainder]string world)
        {
            Config.ChangeWorld(world);

            var m = await Context.Channel.SendMessageAsync($"Current world changed to {world}");
            await Task.Delay(Config.bot.delay);
            await m.DeleteAsync();
            await Context.Message.DeleteAsync();
        }

        [Command("prefix", RunMode = RunMode.Async)]
        [Summary("Toggles usage of prefix")]
        public async Task TogglePrefix()
        {
            ulong id = Context.Message.Author.Id;

            if (!Users.TogglePrefix(id))
            {
                var ms = await Context.Channel.SendMessageAsync($"No user with id {id}");
                await Task.Delay(Config.bot.delay);
                await ms.DeleteAsync();
                await Context.Message.DeleteAsync();
            }
            else
            {
                var m = await Context.Channel.SendMessageAsync($"Toggled prefix {(Users.users[id].prefix ? "on" : "off")} for {Context.Guild.GetUser(id)}");
                await Task.Delay(Config.bot.delay);
                await m.DeleteAsync();
                await Context.Message.DeleteAsync();
            }
        }

        [Command("addshortcut", RunMode = RunMode.Async)]
        [Alias("asc")]
        [Summary("Adds a shortcut for user")]
        public async Task AddShortCut(string sc, string lc)
        {
            ulong id = Context.Message.Author.Id;

            if (!Users.AddShortcut(id, sc, lc))
            {
                var ms = await Context.Channel.SendMessageAsync($"No user with id {id}");
                await Task.Delay(Config.bot.delay);
                await ms.DeleteAsync();
                await Context.Message.DeleteAsync();
            }
            else
            {
                var m = await Context.Channel.SendMessageAsync($"Added shortcut {lc} => {sc} for {Context.Guild.GetUser(id)}");
                await Task.Delay(Config.bot.delay);
                await m.DeleteAsync();
                await Context.Message.DeleteAsync();
            }
        }

        [Command("removeshortcut", RunMode = RunMode.Async)]
        [Alias("rsc")]
        [Summary("Removes shortcut from user")]
        public async Task RemoveShortcut(string sc)
        {
            ulong id = Context.Message.Author.Id;

            if (!Users.RemoveShortcut(id, sc))
            {
                var ms = await Context.Channel.SendMessageAsync($"No user with id {id}");
                await Task.Delay(Config.bot.delay);
                await ms.DeleteAsync();
                await Context.Message.DeleteAsync();
            }
            else
            {
                var m = await Context.Channel.SendMessageAsync($"Removed shortcut {sc} from {Context.Guild.GetUser(id)}");
                await Task.Delay(Config.bot.delay);
                await m.DeleteAsync();
                await Context.Message.DeleteAsync();
            }
        }

        [Command("changeshortcut", RunMode = RunMode.Async)]
        [Alias("csc")]
        [Summary("Changes shortcut marker for user")]
        public async Task ChangeShortcut(string symbol)
        {
            ulong id = Context.Message.Author.Id;

            if (!Users.ChangeShortcutSymbol(id, symbol))
            {
                var ms = await Context.Channel.SendMessageAsync($"No user with id {id}");
                await Task.Delay(Config.bot.delay);
                await ms.DeleteAsync();
                await Context.Message.DeleteAsync();
            }
            else
            {
                var m = await Context.Channel.SendMessageAsync($"Changed shortcut marker to {symbol} for {Context.Guild.GetUser(id)}");
                await Task.Delay(Config.bot.delay);
                await m.DeleteAsync();
                await Context.Message.DeleteAsync();
            }
        }

        [Command("help")]
        [Summary("Lists commands and summaries")]
        public async Task Help()
        {
            List<CommandInfo> commands = CommandHandler.GetCommandInfo();

            var builder = new EmbedBuilder();
            builder.WithTitle("Commands");

            var embed = builder.Build();
            await Context.Channel.SendMessageAsync(null, false, embed);

            int counter = 0;
            builder = new EmbedBuilder();

            for (int i = 0; i < commands.Count; i++)
            {
                string embedFieldText = commands[i].Summary ?? "No description available\n";
                string name = commands[i].Name;

                for (int j = 1; j < commands[i].Aliases.Count; j++)
                    name += $" ({commands[i].Aliases[j]})";

                builder.AddField(name, embedFieldText);
                counter++;

                if (counter >= 20 || i == commands.Count-1)
                {
                    counter = 0;
                    embed = builder.Build();
                    await Context.Channel.SendMessageAsync(null, false, embed);
                    builder = new EmbedBuilder();
                }
            }
        }
    }
}
