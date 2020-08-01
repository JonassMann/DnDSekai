using System;
using System.Collections.Generic;
using System.Text;

using DnDSekai.Data.Types;
using Newtonsoft.Json;

namespace DnDSekai.Data.Storage
{
    public static class Spells
    {
        private static Dictionary<string, Spell> spells;

        static Spells()
        {
            spells = new Dictionary<string, Spell>();
        }

        public static void Save(string name)
        {
            DataStorage.SaveData(spells[name].filePath, JsonConvert.SerializeObject(spells[name], Formatting.Indented));
        }

        public static void Load(List<Tuple<string, string>> files)
        {
            foreach (Tuple<string, string> t in files)
            {
                string tempName = t.Item1[(t.Item1.LastIndexOf('/') + 1)..t.Item1.LastIndexOf('.')];
                spells[tempName] = JsonConvert.DeserializeObject<Spell>(t.Item2);
                spells[tempName].filePath = t.Item1;
            }
        }

        public static void Create(string path)
        {
            spells[path.Substring(path.LastIndexOf('/') + 1)] = new Spell(path);
            Save(path.Substring(path.LastIndexOf('/') + 1));
        }

        public static void Delete(string name)
        {
            DataStorage.DeleteData(spells[name].filePath);
            spells.Remove(name);
        }

        public static Spell Get(string name)
        {
            return spells[name];
        }

        public static bool Exists(string name)
        {
            return spells.ContainsKey(name);
        }
    }
}
