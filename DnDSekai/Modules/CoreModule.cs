using System.Threading.Tasks;

using Discord.Commands;
using DnDSekai.Core.Config;
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
            Config.AddUser(id);

            var m = await Context.Channel.SendMessageAsync($"Added user {Context.Guild.GetUser(id)}");
            await Task.Delay(5000);
            await m.DeleteAsync();
        }

        [Command("removeuser", RunMode = RunMode.Async)]
        [Alias("ruser")]
        [Summary("Adds user to user list")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task RemoveUser(ulong id)
        {
            Config.bot.users.Remove(id);
            Config.SaveConfig();

            var m = await Context.Channel.SendMessageAsync($"Removed user {Context.Guild.GetUser(id)}");
            await Task.Delay(5000);
            await m.DeleteAsync();
            await Context.Message.DeleteAsync();
        }
    }
}
