using System;
using System.Collections.Generic;
using System.Text;

using DnDSekai.Data.Types;
using Newtonsoft.Json;

namespace DnDSekai.Data.Storage
{
    public static class Items
    {
        private static Dictionary<string, Item> items;

        static Items()
        {
            items = new Dictionary<string, Item>();
        }

        public static void Save(string name)
        {
            DataStorage.SaveData(items[name].filePath, JsonConvert.SerializeObject(items[name], Formatting.Indented));
        }

        public static void Load(List<Tuple<string, string>> files)
        {
            foreach (Tuple<string, string> t in files)
            {
                string tempName = t.Item1[(t.Item1.LastIndexOf('/') + 1)..t.Item1.LastIndexOf('.')];
                items[tempName] = JsonConvert.DeserializeObject<Item>(t.Item2);
                items[tempName].filePath = t.Item1;
            }
        }

        public static void Create(string path)
        {
            items[path.Substring(path.LastIndexOf('/') + 1)] = new Item(path);
            Save(path.Substring(path.LastIndexOf('/') + 1));
        }

        public static void Delete(string name)
        {
            DataStorage.DeleteData(items[name].filePath);
            items.Remove(name);
        }

        public static Item Get(string name)
        {
            return items[name];
        }

        public static bool Exists(string name)
        {
            return items.ContainsKey(name);
        }
    }
}
