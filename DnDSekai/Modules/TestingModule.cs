using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;

using Discord.Commands;
using DnDSekai.Core;
using DnDSekai.Data.Storage;
using Discord;

using DnDSekai.Data;

namespace DnDSekai.Modules
{
    public class TestingModule : ModuleBase<SocketCommandContext>
    {
        [Command("Save", RunMode = RunMode.Async)]
        [Summary("Tests Save function for individual items")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task Save(string path, [Remainder]string text)
        {
            DataStorage.SaveData(path, text);

            await Context.Channel.SendMessageAsync($"Saved");
        }

        [Command("Delete", RunMode = RunMode.Async)]
        [Summary("Tests Delete function")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task Delete(string path)
        {
            DataStorage.DeleteData(path);
            await Context.Channel.SendMessageAsync($"Deleted {path}");
        }

        [Command("ClearDeleted", RunMode = RunMode.Async)]
        [Summary("Tetsts ClearDeleted function")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task ClearDeleted()
        {
            DataStorage.ClearDeleted();
            await Context.Channel.SendMessageAsync("Cleared deleted files");
        }

        [Command("Exists", RunMode = RunMode.Async)]
        [Summary("Tests DataExists function")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task FileExists(string path)
        {
            await Context.Channel.SendMessageAsync(DataStorage.DataExists(path).ToString());
        }
    }
}
