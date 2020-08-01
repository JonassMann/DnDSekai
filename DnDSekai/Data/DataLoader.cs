using System;
using System.Collections.Generic;

using System.IO;
using DnDSekai.Core;
using DnDSekai.Data.Storage;

namespace DnDSekai.Data
{
    public static class DataLoader
    {
        public static List<Tuple<string, string>> classes;
        public static List<Tuple<string, string>> races;
        public static List<Tuple<string, string>> skills;
        public static List<Tuple<string, string>> spells;

        public static void LoadAllData()
        {
            classes = new List<Tuple<string, string>>();
            races = new List<Tuple<string, string>>();
            skills = new List<Tuple<string, string>>();
            spells = new List<Tuple<string, string>>();

            if (!Directory.Exists("Resources")) Directory.CreateDirectory("Resources");
            if (!Directory.Exists($"Resources/{Config.bot.worldName}")) Directory.CreateDirectory($"Resources/{Config.bot.worldName}");

            foreach (string file in Directory.EnumerateFiles($"Resources/{Config.bot.worldName}", "*.dndsclass", SearchOption.AllDirectories))
                classes.Add(new Tuple<string, string>(file.Replace('\\', '/'), File.ReadAllText(file)));
            Classes.Load(classes);

            foreach (string file in Directory.EnumerateFiles($"Resources/{Config.bot.worldName}", "*.dndsrace", SearchOption.AllDirectories))
                races.Add(new Tuple<string, string>(file.Replace('\\', '/'), File.ReadAllText(file)));
            Races.Load(races);

            foreach (string file in Directory.EnumerateFiles($"Resources/{Config.bot.worldName}", "*.dndsskill", SearchOption.AllDirectories))
                skills.Add(new Tuple<string, string>(file.Replace('\\', '/'), File.ReadAllText(file)));
            Skills.Load(skills);

            foreach (string file in Directory.EnumerateFiles($"Resources/{Config.bot.worldName}", "*.dndsspell", SearchOption.AllDirectories))
                spells.Add(new Tuple<string, string>(file.Replace('\\', '/'), File.ReadAllText(file)));
            Spells.Load(spells);
        }
    }
}