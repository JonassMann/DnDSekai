using System;
using System.Collections.Generic;
using System.Text;

using DnDSekai.Data.Types;
using Newtonsoft.Json;

namespace DnDSekai.Data.Storage
{
    public static class Classes
    {
        private static Dictionary<string, Class> classes;

        static Classes()
        {
            classes = new Dictionary<string, Class>();
        }

        public static void Save(string name)
        {
            DataStorage.SaveData(classes[name].filePath, JsonConvert.SerializeObject(classes[name], Formatting.Indented));
        }

        public static void Load(List<Tuple<string, string>> files)
        {
            foreach (Tuple<string, string> t in files)
            {
                string tempName = t.Item1[(t.Item1.LastIndexOf('/') + 1)..t.Item1.LastIndexOf('.')];
                classes[tempName] = JsonConvert.DeserializeObject<Class>(t.Item2);
                classes[tempName].filePath = t.Item1;
            }
        }

        public static void Create(string path)
        {
            classes[path.Substring(path.LastIndexOf('/') + 1)] = new Class(path);
            Save(path.Substring(path.LastIndexOf('/') + 1));
        }

        public static void Delete(string name)
        {
            DataStorage.DeleteData(classes[name].filePath);
            classes.Remove(name);
        }

        public static Class Get(string name)
        {
            return classes[name];
        }

        public static bool Exists(string name)
        {
            return classes.ContainsKey(name);
        }
    }
}
