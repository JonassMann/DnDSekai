using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

namespace DnDSekai.Core.Config
{
    class Config
    {
        //Handles core config
        private const string configFolder = "Resources";
        private const string configFile = "config.json";

        public static BotConfig bot;

        static Config() //Loads config info on startup
        {
            if (!Directory.Exists(configFolder))
                Directory.CreateDirectory(configFolder);

            if (!File.Exists($"{configFolder}/{configFile}"))
            {
                bot = new BotConfig();
                bot.users = new Dictionary<ulong, bool>();
                string json = JsonConvert.SerializeObject(bot, Formatting.Indented);
                File.WriteAllText($"{configFolder}/{configFile}", json);
            }
            else
            {
                string json = File.ReadAllText($"{configFolder}/{configFile}");
                bot = JsonConvert.DeserializeObject<BotConfig>(json);
            }
        }

        public static void AddUser(ulong id)
        {
            bot.users.Add(id, true);
            SaveConfig();
        }

        public static void RemoveUser(ulong id)
        {
            bot.users.Remove(id);
            SaveConfig();
        }

        public static void SaveConfig()
        {
            File.WriteAllText($"{configFolder}/{configFile}", JsonConvert.SerializeObject(bot, Formatting.Indented));
        }
    }

    public struct BotConfig
    {
        public string token;
        public string cmdPrefix;
        public string worldName;

        public Dictionary<ulong, bool> users;
    }
}
