using System.Threading.Tasks;
using System.Collections.Generic;

using DnDSekai.Core;
using DnDSekai.Data;
using Discord.Commands;
using Discord;

namespace DnDSekai.Modules
{
    public class CoreModule : ModuleBase<SocketCommandContext>
    {
        [Command("AddUser", RunMode = RunMode.Async)]
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

        [Command("RemoveUser", RunMode = RunMode.Async)]
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

        [Command("World", RunMode = RunMode.Async)]
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

        [Command("Prefix", RunMode = RunMode.Async)]
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

        [Command("AddShortcut", RunMode = RunMode.Async)]
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

        [Command("RemoveShortcut", RunMode = RunMode.Async)]
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

        [Command("ChangeShortcut", RunMode = RunMode.Async)]
        [Summary("Changes shortcut symbol for user")]
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

        [Command("SetWork", RunMode = RunMode.Async)]
        [Summary("Changes work variables")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetWorkVariables(string path, string name)
        {
            Config.SetWorkPath(path);
            Config.SetWorkName(name);
            await Context.Channel.SendMessageAsync($"Work path: {Config.workPath}\nWork name: {Config.workName}");
        }

        [Command("SetWorkPath", RunMode = RunMode.Async)]
        [Summary("Changes work path")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetWorkPath(string path)
        {
            Config.SetWorkPath(path);
            await Context.Channel.SendMessageAsync($"Work path: {Config.workPath}\nWork name: {Config.workName}");
        }

        [Command("SetWorkName", RunMode = RunMode.Async)]
        [Summary("Changes work path")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetWorkName(string name)
        {
            Config.SetWorkName(name);
            await Context.Channel.SendMessageAsync($"Work path: {Config.workPath}\nWork name: {Config.workName}");
        }

        [Command("ResetWork", RunMode = RunMode.Async)]
        [Summary("Changes work type")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task ResetWork()
        {
            Config.ResetWork();
            await Context.Channel.SendMessageAsync($"Work path: {Config.workPath}\nWork name: {Config.workName}");
        }

        [Command("LoadAll", RunMode = RunMode.Async)]
        [Summary("Loads all files")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task LoadAll()
        {
            DataLoader.LoadAllData();
            await Context.Channel.SendMessageAsync($"All data loaded");
        }

        [Command("Help")]
        [Summary("Lists commands and summaries")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task Help(string start = "")
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
                if (commands[i].Name.ToLower().StartsWith(start.ToLower()))
                {
                    string embedFieldText = commands[i].Summary ?? "No description available\n";
                    string name = commands[i].Name;

                    for (int j = 1; j < commands[i].Aliases.Count; j++)
                        name += $" ({commands[i].Aliases[j]})";

                    builder.AddField(name, embedFieldText);
                    counter++;
                }

                if (counter >= 20 || i == commands.Count - 1)
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
