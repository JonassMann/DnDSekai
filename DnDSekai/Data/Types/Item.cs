using System;
using System.Collections.Generic;
using System.Linq;

using DnDSekai.Data.Storage;

namespace DnDSekai.Data.Types
{
    public class Item
    {
        public string filePath;

        public string name;
        public string description;

        public Dictionary<string, int> stats;
        public Dictionary<string, int> skills;

        public Dictionary<string, Effect> effects;
        public Dictionary<string, Attack> attacks;

        public Item(string filePath)
        {
            this.filePath = filePath + ".dndsitem";

            name = filePath.Substring(filePath.LastIndexOf('/') + 1);
            description = "";

            stats = new Dictionary<string, int>();
            skills = new Dictionary<string, int>();

            stats["hp"] = 0;
            stats["mp"] = 0;
            stats["strength"] = 0;
            stats["agility"] = 0;
            stats["magic"] = 0;
            stats["intelligence"] = 0;
            stats["charisma"] = 0;
            stats["luck"] = 0;
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

        public void AddSkill(string name, int level)
        {
            skills[name] = level;
            Save();
        }

        public void RemoveSkill(string name)
        {
            skills.Remove(name);
            Save();
        }

        public void AddEffect(string name, string effect, int num)
        {
            if (!effects.ContainsKey(name)) effects[name] = new Effect();
            effects[name].effects[effect] = num;
            Save();
        }

        public void AddSpecial(string name, string special)
        {
            if (!effects.ContainsKey(name)) effects[name] = new Effect();
            if (!effects[name].special.Contains(special)) effects[name].special.Add(special);
            Save();
        }

        public void RemoveEffect(string name)
        {
            effects.Remove(name);
            Save();
        }

        public void AddAttack(string name, int size, int count, int mod)
        {
            Attack temp = new Attack();
            temp.diceSize = size;
            temp.diceCount = count;
            temp.modifier = mod;
            attacks[name] = temp;
            Save();
        }

        public void RemoveAttack(string name)
        {
            attacks.Remove(name);
            Save();
        }

        public void Save()
        {
            Classes.Save(filePath[(filePath.LastIndexOf('/') + 1)..filePath.LastIndexOf('.')]);
        }
    }

    public struct Attack
    {
        public int diceSize;
        public int diceCount;
        public int modifier;
    }
}
