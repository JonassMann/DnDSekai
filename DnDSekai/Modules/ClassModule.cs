using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;

using DnDSekai.Core;
using DnDSekai.Data.Storage;
using DnDSekai.Data.Types;
using Discord.Commands;
using Discord;

namespace DnDSekai.Modules
{
    public class ClassModule : ModuleBase<SocketCommandContext>
    {
        [Command("ClassCreate", RunMode = RunMode.Async)]
        [Summary("Creates class")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task ClassCreate(string filePath, string fileName)
        {
            Config.SetWorkPath(filePath);
            Config.SetWorkName(fileName);

            Classes.Create($"{Config.workPath}/{Config.workName}");
            await Context.Channel.SendMessageAsync($"Created class {fileName} in {filePath}");
        }

        [Command("ClassDelete", RunMode = RunMode.Async)]
        [Summary("Deletes class")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task ClassDelete(string fileName)
        {
            Classes.Delete(fileName);
            await Context.Channel.SendMessageAsync($"Deleted class {fileName}");
        }

        [Command("ClassName", RunMode = RunMode.Async)]
        [Summary("Sets class name")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetClassName([Remainder]string name)
        {
            Classes.Get(Config.workName).SetName(name);
            await Context.Channel.SendMessageAsync($"Set class {Config.workName} name to {name}");
        }

        [Command("ClassDesc", RunMode = RunMode.Async)]
        [Summary("Sets class description")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetClassDescription([Remainder]string description)
        {
            Classes.Get(Config.workName).SetDescription(description);
            await Context.Channel.SendMessageAsync($"Set class {Config.workName} description to {description}");
        }

        [Command("ClassStats", RunMode = RunMode.Async)]
        [Summary("Sets class stats")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetClassStats(int hp, int mp, int strength, int agility, int magic, int intelligence, int charisma, int luck)
        {
            Classes.Get(Config.workName).SetStats(hp, mp, strength, agility, magic, intelligence, charisma, luck);
            await Context.Channel.SendMessageAsync($"Set class {Config.workName} stats");
        }

        [Command("ClassStat", RunMode = RunMode.Async)]
        [Summary("Sets class stat")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetClassStat(string stat, int value)
        {
            Classes.Get(Config.workName).SetStat(stat, value);
            await Context.Channel.SendMessageAsync($"Set class {Config.workName} {stat} stat to {value}");
        }

        [Command("ClassGrowths", RunMode = RunMode.Async)]
        [Summary("Sets class growths")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetClassGrowths(int hp, int mp, int strength, int agility, int magic, int intelligence, int charisma, int luck)
        {
            Classes.Get(Config.workName).SetGrowths(hp, mp, strength, agility, magic, intelligence, charisma, luck);
            await Context.Channel.SendMessageAsync($"Set class {Config.workName} growths");
        }

        [Command("ClassGrowth", RunMode = RunMode.Async)]
        [Summary("Sets class stat")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetClassGrowth(string stat, int value)
        {
            Classes.Get(Config.workName).SetGrowth(stat, value);
            await Context.Channel.SendMessageAsync($"Set class {Config.workName} {stat} growth to {value}");
        }

        [Command("ClassAddSkill", RunMode = RunMode.Async)]
        [Summary("Adds class skill")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task AddClassSkill(string skill, int level, int levelReq = 1)
        {
            Classes.Get(Config.workName).AddSkill(levelReq, skill, level);
            await Context.Channel.SendMessageAsync($"Added skill {skill} to class {Config.workName} at level {level}");
        }

        [Command("ClassRemoveSkill", RunMode = RunMode.Async)]
        [Summary("Removes class skill")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task RemoveClassSkill(string skill)
        {
            Classes.Get(Config.workName).RemoveSkill(skill);
            await Context.Channel.SendMessageAsync($"Removed skill {skill} from class {Config.workName}");
        }

        [Command("ClassGetSkill", RunMode = RunMode.Async)]
        [Summary("Gets class skill")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task GetClassSkill(int level)
        {
            Dictionary<string, int> skills = Classes.Get(Config.workName).GetSkills(level);
            string skillText = "Skills:\n";
            foreach (KeyValuePair<string, int> k in skills ?? Enumerable.Empty<KeyValuePair<string, int>>())
            {
                skillText += $"{k.Key} - Level {k.Value}\n";
            }
            await Context.Channel.SendMessageAsync(skillText);
        }

        [Command("ClassGet", RunMode = RunMode.Async)]
        [Summary("Gets class info")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task GetClass(string name = "")
        {
            if (name == "") name = Config.workName;
            Class info = Classes.Get(name);

            var builder = new EmbedBuilder();
            builder.WithTitle(info.name);
            builder.AddField(info.name, info.description);
            builder.AddField("Stats", $"HP: {info.stats["hp"]} | MP: {info.stats["mp"]}\n" +
                                      $"Strength: {info.stats["strength"]} | Agility: {info.stats["agility"]}\n" +
                                      $"Magic: {info.stats["magic"]} | Intelligence: {info.stats["intelligence"]}\n" +
                                      $"Charisma: {info.stats["charisma"]} | Luck: {info.stats["luck"]}");

            builder.AddField("Growths", $"HP: {info.growths["hp"]} | MP: {info.growths["mp"]}\n" +
                                      $"Strength: {info.growths["strength"]} | Agility: {info.growths["agility"]}\n" +
                                      $"Magic: {info.growths["magic"]} | Intelligence: {info.growths["intelligence"]}\n" +
                                      $"Charisma: {info.growths["charisma"]} | Luck: {info.growths["luck"]}");

            builder.WithFooter(info.filePath);

            var embed = builder.Build();
            await Context.Channel.SendMessageAsync(null, false, embed);

            foreach (KeyValuePair<int, List<Tuple<string, int>>> k in info.skills)
            {
                builder = new EmbedBuilder();
                builder.WithTitle($"Skills {k.Key}");

                foreach (Tuple<string, int> t in k.Value)
                {
                    builder.AddField($"{t.Item1} {t.Item2}", $"Description", true);
                }

                builder.WithFooter(info.filePath);

                embed = builder.Build();
                await Context.Channel.SendMessageAsync(null, false, embed);
            }
        }
    }
}