using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

namespace DnDSekai.Core
{
    class Users
    {
        public const string userFolder = "Resources";
        private const string userFile = "users.json";

        public static Dictionary<ulong, User> users;

        static Users() //Loads config info on startup
        {
            if (!Directory.Exists(userFolder))
                Directory.CreateDirectory(userFolder);

            if (!File.Exists($"{userFolder}/{userFile}"))
            {
                users = new Dictionary<ulong, User>();
                string json = JsonConvert.SerializeObject(users, Formatting.Indented);
                File.WriteAllText($"{userFolder}/{userFile}", json);
            }
            else
            {
                string json = File.ReadAllText($"{userFolder}/{userFile}");
                users = JsonConvert.DeserializeObject<Dictionary<ulong, User>>(json);
            }
        }

        public static void AddUser(ulong id)
        {
            User tempUser = new User();
            tempUser.prefix = true;
            tempUser.shortcutSymbol = "%";
            tempUser.shortcuts = new Dictionary<string, string>();
            users.Add(id, tempUser);
            SaveUsers();
        }

        public static void RemoveUser(ulong id)
        {
            users.Remove(id);
            SaveUsers();
        }

        public static bool AddShortcut(ulong id, string sc, string lc)
        {
            if (!users.ContainsKey(id)) return false;
            users[id].shortcuts[sc] = lc;
            SaveUsers();
            return true;
        }

        public static bool RemoveShortcut(ulong id, string sc)
        {
            if (!users.ContainsKey(id)) return false;
            users[id].shortcuts.Remove(sc);
            SaveUsers();
            return true;
        }

        public static bool ChangeShortcutSymbol(ulong id, string symbol)
        {
            if (!users.ContainsKey(id)) return false;

            User temp = users[id];
            temp.shortcutSymbol = symbol;
            users[id] = temp;
            SaveUsers();
            return true;
        }

        public static bool TogglePrefix(ulong id)
        {
            if (!users.ContainsKey(id)) return false;

            User temp = users[id];
            temp.prefix = !temp.prefix;
            users[id] = temp;
            SaveUsers();
            return true;
        }

        public static void SaveUsers()
        {
            File.WriteAllText($"{userFolder}/{userFile}", JsonConvert.SerializeObject(users, Formatting.Indented));
        }
    }

    public struct User
    {
        public bool prefix;
        public string shortcutSymbol;
        public Dictionary<string, string> shortcuts;
    }
}
