using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;

using Discord.Commands;
using DnDSekai.Core;
using Discord;

using DnDSekai.Data;

namespace DnDSekai.Modules
{
    public class TestingModule : ModuleBase<SocketCommandContext>
    {
        [Command("Save", RunMode = RunMode.Async)]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task Save(string path, string text)
        {
            List<Tuple<string, string>> files = new List<Tuple<string, string>>();
            files.Add(new Tuple<string, string>(path, text));
            DataStorage.SaveData(files);

            await Context.Channel.SendMessageAsync($"Saved");
        }

        [Command("Load", RunMode = RunMode.Async)]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task Save(string path)
        {
            List<string> paths = new List<string>();
            paths.Add(path);

            List<Tuple<string, string>> files = DataStorage.LoadData(paths);
            string text = "Files:\n";
            foreach (Tuple<string, string> f in files ?? Enumerable.Empty<Tuple<string, string>>())
            {
                text += $"Path: {f.Item1} - Text: {f.Item2}\n";
            }

            await Context.Channel.SendMessageAsync(text);
        }

        [Command("Delete", RunMode = RunMode.Async)]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task Delete(string path)
        {
            List<string> paths = new List<string>();
            paths.Add(path);

            DataStorage.DeleteData(paths);
        }

        [Command("Exists", RunMode = RunMode.Async)]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task FileExists(string path)
        {
            await Context.Channel.SendMessageAsync(DataStorage.DataExists(path).ToString());
        }
    }
}
