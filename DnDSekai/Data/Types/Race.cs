using System;
using System.Collections.Generic;
using System.Linq;

using DnDSekai.Data.Storage;

namespace DnDSekai.Data.Types
{
    public class Race
    {
        public string filePath;

        public string name;
        public string description;

        public Dictionary<string, int> stats;
        public Dictionary<string, int> growths;
        public Dictionary<int, List<Tuple<string, int>>> skills;

        public Class(string filePath)
        {
            this.filePath = filePath + ".dndsrace";

            name = filePath.Substring(filePath.LastIndexOf('/') + 1);
            description = "";

            stats = new Dictionary<string, int>();
            growths = new Dictionary<string, int>();
            skills = new Dictionary<int, List<Tuple<string, int>>>();

            stats["hp"] = 0;
            stats["mp"] = 0;
            stats["strength"] = 0;
            stats["agility"] = 0;
            stats["magic"] = 0;
            stats["intelligence"] = 0;
            stats["charisma"] = 0;
            stats["luck"] = 0;

            growths["hp"] = 0;
            growths["mp"] = 0;
            growths["strength"] = 0;
            growths["agility"] = 0;
            growths["magic"] = 0;
            growths["intelligence"] = 0;
            growths["charisma"] = 0;
            growths["luck"] = 0;
        }

        public void SetName(string name)
        {
            this.name = name;
            Save();
        }

        public void SetDescription(string description)
        {
            this.description = description;
            Save();
        }

        public void SetStats(int hp, int mp, int strength, int agility, int magic, int intelligence, int charisma, int luck)
        {
            stats["hp"] = hp;
            stats["mp"] = mp;
            stats["strength"] = strength;
            stats["agility"] = agility;
            stats["magic"] = magic;
            stats["intelligence"] = intelligence;
            stats["charisma"] = charisma;
            stats["luck"] = luck;
            Save();
        }

        public bool SetStat(string stat, int value)
        {
            if (!stats.ContainsKey(stat))
                return false;
            stats[stat] = value;
            Save();
            return true;
        }

        public void SetGrowths(int hp, int mp, int strength, int agility, int magic, int intelligence, int charisma, int luck)
        {
            growths["hp"] = hp;
            growths["mp"] = mp;
            growths["strength"] = strength;
            growths["agility"] = agility;
            growths["magic"] = magic;
            growths["intelligence"] = intelligence;
            growths["charisma"] = charisma;
            growths["luck"] = luck;
            Save();
        }

        public bool SetGrowth(string stat, int value)
        {
            if (!growths.ContainsKey(stat))
                return false;
            growths[stat] = value;
            Save();
            return true;
        }

        public void AddSkill(int levelReq, string name, int level)
        {
            if (!skills.ContainsKey(levelReq)) skills[levelReq] = new List<Tuple<string, int>>();
            skills[levelReq].Add(new Tuple<string, int>(name, level));
            skills[levelReq] = skills[levelReq].Distinct().ToList();
            Save();
        }

        public void RemoveSkill(string skill)
        {
            foreach (KeyValuePair<int, List<Tuple<string, int>>> k in skills)
            {
                k.Value.RemoveAll(t => t.Item1 == skill);
                if (!k.Value.Any()) skills.Remove(k.Key);
            }
            Save();
        }

        public Dictionary<string, int> GetSkills(int lvl)
        {
            Dictionary<string, int> temp = new Dictionary<string, int>();

            for (int i = 1; i <= lvl; i++)
            {
                foreach (Tuple<string, int> t in skills[i])
                {
                    temp[t.Item1] = t.Item2;
                }
            }

            return temp;
        }

        public void Save()
        {
            Races.Save(filePath[(filePath.LastIndexOf('/') + 1)..filePath.LastIndexOf('.')]);
        }
    }
}
