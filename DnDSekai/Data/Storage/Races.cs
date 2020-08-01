using System;
using System.Collections.Generic;
using System.Text;

using DnDSekai.Data.Types;
using Newtonsoft.Json;

namespace DnDSekai.Data.Storage
{
    public static class Races
    {
        private static Dictionary<string, Race> races;

        static Races()
        {
            races = new Dictionary<string, Race>();
        }

        public static void Save(string name)
        {
            DataStorage.SaveData(races[name].filePath, JsonConvert.SerializeObject(races[name], Formatting.Indented));
        }

        public static void Load(List<Tuple<string, string>> files)
        {
            foreach (Tuple<string, string> t in files)
            {
                string tempName = t.Item1[(t.Item1.LastIndexOf('/') + 1)..t.Item1.LastIndexOf('.')];
                races[tempName] = JsonConvert.DeserializeObject<Race>(t.Item2);
                races[tempName].filePath = t.Item1;
            }
        }

        public static void Create(string path)
        {
            races[path.Substring(path.LastIndexOf('/') + 1)] = new Race(path);
            Save(path.Substring(path.LastIndexOf('/') + 1));
        }

        public static void Delete(string name)
        {
            DataStorage.DeleteData(races[name].filePath);
            races.Remove(name);
        }

        public static Race Get(string name)
        {
            return races[name];
        }

        public static bool Exists(string name)
        {
            return races.ContainsKey(name);
        }
    }
}
