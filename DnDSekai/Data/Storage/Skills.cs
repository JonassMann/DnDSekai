using System;
using System.Collections.Generic;
using System.Text;

using DnDSekai.Data.Types;
using Newtonsoft.Json;

namespace DnDSekai.Data.Storage
{
    public static class Skills
    {
        private static Dictionary<string, Skill> skills;

        static Skills()
        {
            skills = new Dictionary<string, Skill>();
        }

        public static void Save(string name)
        {
            DataStorage.SaveData(skills[name].filePath, JsonConvert.SerializeObject(skills[name], Formatting.Indented));
        }

        public static void Load(List<Tuple<string, string>> files)
        {
            foreach (Tuple<string, string> t in files)
            {
                string tempName = t.Item1[(t.Item1.LastIndexOf('/') + 1)..t.Item1.LastIndexOf('.')];
                skills[tempName] = JsonConvert.DeserializeObject<Skill>(t.Item2);
                skills[tempName].filePath = t.Item1;
            }
        }

        public static void Create(string path)
        {
            skills[path.Substring(path.LastIndexOf('/') + 1)] = new Skill(path);
            Save(path.Substring(path.LastIndexOf('/') + 1));
        }

        public static void Delete(string name)
        {
            DataStorage.DeleteData(skills[name].filePath);
            skills.Remove(name);
        }

        public static Skill Get(string name)
        {
            return skills[name];
        }

        public static bool Exists(string name)
        {
            return skills.ContainsKey(name);
        }
    }
}
