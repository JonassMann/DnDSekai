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
    public class RaceModule : ModuleBase<SocketCommandContext>
    {
        [Command("RaceCreate", RunMode = RunMode.Async)]
        [Summary("Creates race")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task RaceCreate(string filePath, string fileName)
        {
            Config.SetWorkPath(filePath);
            Config.SetWorkName(fileName);

            Races.Create($"{Config.workPath}/{Config.workName}");
            await Context.Channel.SendMessageAsync($"Created race {fileName} in {filePath}");
        }

        [Command("RaceDelete", RunMode = RunMode.Async)]
        [Summary("Deletes race")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task RaceDelete(string fileName)
        {
            Races.Delete(fileName);
            await Context.Channel.SendMessageAsync($"Deleted race {fileName}");
        }

        [Command("RaceName", RunMode = RunMode.Async)]
        [Summary("Sets race name")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetRaceName([Remainder]string name)
        {
            Races.Get(Config.workName).SetName(name);
            await Context.Channel.SendMessageAsync($"Set race {Config.workName} name to {name}");
        }

        [Command("RaceDesc", RunMode = RunMode.Async)]
        [Summary("Sets race description")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetRaceDescription([Remainder]string description)
        {
            Races.Get(Config.workName).SetDescription(description);
            await Context.Channel.SendMessageAsync($"Set race {Config.workName} description to {description}");
        }

        [Command("RaceStats", RunMode = RunMode.Async)]
        [Summary("Sets race stats")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetRaceStats(int hp, int mp, int strength, int agility, int magic, int intelligence, int charisma, int luck)
        {
            Races.Get(Config.workName).SetStats(hp, mp, strength, agility, magic, intelligence, charisma, luck);
            await Context.Channel.SendMessageAsync($"Set race {Config.workName} stats");
        }

        [Command("RaceStat", RunMode = RunMode.Async)]
        [Summary("Sets race stat")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetRaceStat(string stat, int value)
        {
            Races.Get(Config.workName).SetStat(stat, value);
            await Context.Channel.SendMessageAsync($"Set race {Config.workName} {stat} stat to {value}");
        }

        [Command("RaceGrowths", RunMode = RunMode.Async)]
        [Summary("Sets race growths")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetRaceGrowths(int hp, int mp, int strength, int agility, int magic, int intelligence, int charisma, int luck)
        {
            Races.Get(Config.workName).SetGrowths(hp, mp, strength, agility, magic, intelligence, charisma, luck);
            await Context.Channel.SendMessageAsync($"Set race {Config.workName} growths");
        }

        [Command("RaceGrowth", RunMode = RunMode.Async)]
        [Summary("Sets race stat")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetRaceGrowth(string stat, int value)
        {
            Races.Get(Config.workName).SetGrowth(stat, value);
            await Context.Channel.SendMessageAsync($"Set race {Config.workName} {stat} growth to {value}");
        }

        [Command("RaceAddSkill", RunMode = RunMode.Async)]
        [Summary("Adds race skill")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task AddRaceSkill(string skill, int level, int levelReq = 1)
        {
            Races.Get(Config.workName).AddSkill(levelReq, skill, level);
            await Context.Channel.SendMessageAsync($"Added skill {skill} to race {Config.workName} at level {level}");
        }

        [Command("RaceRemoveSkill", RunMode = RunMode.Async)]
        [Summary("Removes race skill")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task RemoveRaceSkill(string skill)
        {
            Races.Get(Config.workName).RemoveSkill(skill);
            await Context.Channel.SendMessageAsync($"Removed skill {skill} from race {Config.workName}");
        }

        [Command("RaceGetSkill", RunMode = RunMode.Async)]
        [Summary("Gets race skill")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task GetRaceSkill(int level)
        {
            Dictionary<string, int> skills = Races.Get(Config.workName).GetSkills(level);
            string skillText = "Skills:\n";
            foreach (KeyValuePair<string, int> k in skills ?? Enumerable.Empty<KeyValuePair<string, int>>())
            {
                skillText += $"{k.Key} - Level {k.Value}\n";
            }
            await Context.Channel.SendMessageAsync(skillText);
        }

        [Command("RaceGet", RunMode = RunMode.Async)]
        [Summary("Gets race info")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task GetRace(string name = "")
        {
            if (name == "") name = Config.workName;
            Race info = Races.Get(name);

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