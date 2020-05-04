using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

namespace DnDSekai.Core
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
                string json = JsonConvert.SerializeObject(bot, Formatting.Indented);
                File.WriteAllText($"{configFolder}/{configFile}", json);
            }
            else
            {
                string json = File.ReadAllText($"{configFolder}/{configFile}");
                bot = JsonConvert.DeserializeObject<BotConfig>(json);
            }
        }

        public static void ChangeWorld(string name)
        {
            bot.worldName = name;
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
        public int delay;
    }
}
