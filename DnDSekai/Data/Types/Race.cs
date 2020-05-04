using System;
using System.Collections.Generic;

namespace DnDSekai.Data.Types
{
    class Race
    {
        public string filePath;
        public string uniqueName;

        public string name;
        public string description;

        public Dictionary<string, int> statGrowth;
        public Dictionary<string, int> skills;

        public Race(string filePath, string uniqueName)
        {
            this.filePath = filePath;
            this.uniqueName = uniqueName;

            name = "Name";
            description = "";

            statGrowth = new Dictionary<string, int>();
            skills = new Dictionary<string, int>();

            statGrowth["maxhp"] = 0;
            statGrowth["maxmp"] = 0;
            statGrowth["strength"] = 0;
            statGrowth["agility"] = 0;
            statGrowth["magic"] = 0;
            statGrowth["intelligence"] = 0;
            statGrowth["charisma"] = 0;
            statGrowth["luck"] = 0;
        }

        public void SetName(string name)
        {
            this.name = name;
        }

        public void SetDescription(string description)
        {
            this.description = description;
        }

        public void SetStats(int hp, int mp, int strength, int agility, int magic, int intelligence, int charisma, int luck)
        {
            statGrowth["maxhp"] = hp;
            statGrowth["maxmp"] = mp;
            statGrowth["strength"] = strength;
            statGrowth["agility"] = agility;
            statGrowth["magic"] = magic;
            statGrowth["intelligence"] = intelligence;
            statGrowth["charisma"] = charisma;
            statGrowth["luck"] = luck;
        }

        public bool SetStat(string stat, int value)
        {
            if (!statGrowth.ContainsKey(stat))
                return false;
            statGrowth[stat] = value;
            return true;
        }

        public void AddSkill(string name, int level)
        {
            skills[name] = level;
        }

        public bool RemoveSkill(string skill)
        {
            if (skills.ContainsKey(skill))
            {
                skills.Remove(skill);
                return true;
            }
            return false;
        }
    }
}
