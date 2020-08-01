using System;
using System.Collections.Generic;
using System.Linq;

using DnDSekai.Data.Storage;

namespace DnDSekai.Data.Types
{
    public class Skill
    {
        public string filePath;

        public string name;
        public string description;

        // Skill level required for effect, 0 is scaling
        public Dictionary<int, Dictionary<string, Effect>> effects;

        public Skill(string filePath)
        {
            this.filePath = filePath;

            name = filePath.Substring(filePath.LastIndexOf('/') + 1);
            description = "";

            effects = new Dictionary<int, Dictionary<string, Effect>>();
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

        public void AddEffect(int level, string name, string type, int value)
        {
            if (!effects.ContainsKey(level)) effects[level] = new Dictionary<string, Effect>(); ;
            if (!effects[level].ContainsKey(name)) effects[level][name] = new Effect();
            effects[level][name].effects[type] = value;
            Save();
        }

        public void AddEffectSpecial(int level, string name, string special)
        {
            if (!effects.ContainsKey(level)) effects[level] = new Dictionary<string, Effect>(); ;
            if (!effects[level].ContainsKey(name)) effects[level][name] = new Effect();
            effects[level][name].special.Add(special);
            Save();
        }

        public void RemoveEffect(int level, string name)
        {
            if(effects.ContainsKey(level) && effects[level].ContainsKey(name))
            {
                effects[level].Remove(name);
                if (effects[level].Count <= 0) effects.Remove(level);
            }
            Save();
        }

        public Dictionary<string, Effect> GetEffects(int level)
        {
            Dictionary<string, Effect> temp = new Dictionary<string, Effect>();

            for (int i = 1; i <= level; i++)
            {
                foreach (KeyValuePair<string, Effect> k in effects[i] ?? Enumerable.Empty<KeyValuePair<string, Effect>>())
                {
                    if (!temp.ContainsKey(k.Key))
                        temp[k.Key] = k.Value;
                    else
                        temp[k.Key].Merge(k.Value);
                }
            }

            return temp;
        }

        public void Save()
        {
            Skills.Save(filePath[(filePath.LastIndexOf('/') + 1)..filePath.LastIndexOf('.')]);
        }
    }
}
