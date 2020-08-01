using System;
using System.Collections.Generic;
using System.Linq;

using DnDSekai.Data.Storage;

namespace DnDSekai.Data.Types
{
    public class Spell
    {
        public string filePath;

        public string name;
        public string description;

        public int baseLevel;
        public int level;

        public Dictionary<string, Effect> baseEffects;
        public Dictionary<string, Effect> effects;

        public List<string> components;

        public Spell(string filePath)
        {
            this.filePath = filePath;

            name = filePath.Substring(filePath.LastIndexOf('/') + 1);
            description = "";

            baseLevel = 0;
            level = 0;

            baseEffects = new Dictionary<string, Effect>();
            effects = new Dictionary<string, Effect>();
            components = new List<string>();
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

        public void SetLevel(int level)
        {
            this.level = level;
            Save();
        }

        public void AddEffect(string name, string type, int value)
        {
            if (!baseEffects.ContainsKey(name)) baseEffects[name] = new Effect();
            baseEffects[name].effects[type] = value;
            Save();
        }

        public void AddEffectSpecial(string name, string special)
        {
            if (!baseEffects.ContainsKey(name)) baseEffects[name] = new Effect();
            baseEffects[name].special.Add(special);
            Save();
        }

        public void RemoveEffect(string name)
        {
            if (baseEffects.ContainsKey(name)) baseEffects.Remove(name);
            Save();
        }

        public void UpdateEffects()
        {
            foreach (string s in components ?? Enumerable.Empty<string>())
            {
                Spell temp = Spells.Get(s);
                temp.UpdateEffects();
                foreach (KeyValuePair<string, Effect> k in temp.effects)
                {
                    if (!baseEffects.ContainsKey(k.Key))
                        effects[k.Key] = k.Value;
                    else
                        effects[k.Key].Merge(k.Value);
                }
            }
            Save();
        }

        public Dictionary<string, Effect> GetSpell()
        {
            return effects;
        }

        public void Save()
        {
            Spells.Save(filePath[(filePath.LastIndexOf('/') + 1)..filePath.LastIndexOf('.')]);
        }
    }
}
