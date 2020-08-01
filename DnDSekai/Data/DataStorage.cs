using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using DnDSekai.Core;

namespace DnDSekai.Data
{
    public static class DataStorage
    {
        public static void SaveData(string path, string json)
        {
            if (path.Contains('/'))
            {
                string[] folders = path.Substring(0, path.LastIndexOf('.')).Split('/');
                string folderPath = "";
                for (int i = 0; i < folders.Length-1; i++)
                {
                    if (folderPath != "") folderPath += "/";
                    folderPath += folders[i];
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);
                }
            }

            File.WriteAllText(path, json);
        }

        public static void DeleteData(string path)
        {
            if (!Directory.Exists("Resources")) Directory.CreateDirectory("Resources");
            if (!Directory.Exists("Resources/Deleted")) Directory.CreateDirectory("Resources/Deleted");

            if (File.Exists(path))
                File.Move(path, $"Resources/Deleted/{path.Substring(path.LastIndexOf('/'))}");
        }

        public static void ClearDeleted()
        {
            Directory.Delete($"Resources/Deleted", true);
        }

        public static bool DataExists(string path)
        {
            return File.Exists(path);
        }
    }
}

/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using DnDSekai.Core;

namespace DnDSekai.Data
{
    public static class DataStorage
    {
        //List<Tuple<string path, string json>>
        public static void SaveData(List<Tuple<string, string>> data)
        {
            CheckPath();
            List<string> allPaths = new List<string>();

            foreach (Tuple<string, string> t in data)
                allPaths.Add(t.Item1.Substring(0, t.Item1.Contains('/') ? t.Item1.LastIndexOf('/') : t.Item1.Length));

            IEnumerable<string> paths = allPaths.Distinct();

            foreach (string s in paths)
            {
                if (s.Contains('/'))
                {
                    string[] folders = s.Split('/');
                    string folderPath = $"Resources/{Config.bot.worldName}";
                    for (int i = 0; i < folders.Length - 1; i++)
                    {
                        folderPath += "/";
                        folderPath += folders[i];
                        if (!Directory.Exists(folderPath))
                            Directory.CreateDirectory(folderPath);
                    }
                }
            }

            foreach (Tuple<string, string> d in data ?? Enumerable.Empty<Tuple<string, string>>())
                File.WriteAllText($"Resources/{Config.bot.worldName}/{d.Item1}", d.Item2);
        }

        public static void SaveData(string path, string json)
        {
            CheckPath();
            if (path.Contains('/'))
            {
                string[] folders = path.Split('/');
                string folderPath = $"Resources/{Config.bot.worldName}";
                for (int i = 0; i < folders.Length - 1; i++)
                {
                    folderPath += "/";
                    folderPath += folders[i];
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);
                }
            }

            File.WriteAllText($"Resources/{Config.bot.worldName}/{path}", json);
        }

        //Loads all data from folders in "paths"
        public static List<Tuple<string, string>> LoadData(List<string> paths)
        {
            CheckPath();
            List<Tuple<string, string>> files = new List<Tuple<string, string>>();

            foreach (string s in paths ?? Enumerable.Empty<string>())
            {
                string[] folders = s.Split('/');
                string folderPath = $"Resources/{Config.bot.worldName}";
                for (int i = 0; i < folders.Length - (folders.Last().EndsWith(".json") ? 1 : 0); i++)
                {
                    folderPath += "/";
                    folderPath += folders[i];
                    if (!Directory.Exists(folderPath)) return null;
                }

                if (folders.Last().EndsWith(".json"))
                {
                    if (!File.Exists($"Resources/{Config.bot.worldName}/{s}")) return null;
                    files.Add(new Tuple<string, string>(s, File.ReadAllText($"Resources/{Config.bot.worldName}/{s}")));
                }
                else
                {
                    foreach (string file in Directory.EnumerateFiles($"Resources/{Config.bot.worldName}/{s}", "*.json", SearchOption.TopDirectoryOnly))
                        files.Add(new Tuple<string, string>(file.Substring(file.LastIndexOf('\\') + 1), File.ReadAllText(file)));
                }
            }
            return files;
        }

        public static List<Tuple<string, string>> LoadData(string path)
        {
            CheckPath();
            List<Tuple<string, string>> files = new List<Tuple<string, string>>();

            string[] folders = path.Split('/');
            string folderPath = $"Resources/{Config.bot.worldName}";
            for (int i = 0; i < folders.Length - (folders.Last().EndsWith(".json") ? 1 : 0); i++)
            {
                folderPath += "/";
                folderPath += folders[i];
                if (!Directory.Exists(folderPath)) return null;
            }

            if (folders.Last().EndsWith(".json"))
            {
                if (!File.Exists($"Resources/{Config.bot.worldName}/{path}")) return null;
                files.Add(new Tuple<string, string>(path, File.ReadAllText($"Resources/{Config.bot.worldName}/{path}")));
            }
            else
            {
                foreach (string file in Directory.EnumerateFiles($"Resources/{Config.bot.worldName}/{path}", "*.json", SearchOption.TopDirectoryOnly))
                    files.Add(new Tuple<string, string>(file.Substring(file.LastIndexOf('\\') + 1, file.Length - 5), File.ReadAllText(file)));
            }
            return files;
        }

        public static void DeleteData(string path)
        {
            CheckPath();
            if (!Directory.Exists($"Resources/{Config.bot.worldName}/Deleted")) Directory.CreateDirectory($"Resources/{Config.bot.worldName}/Deleted");

            if (path.EndsWith(".json") && File.Exists($"Resources/{Config.bot.worldName}/{path}"))
                File.Move($"Resources/{Config.bot.worldName}/{path}", $"Resources/{Config.bot.worldName}/Deleted/{path}");
            else if (Directory.Exists($"Resources/{Config.bot.worldName}/{path}"))
                Directory.Move($"Resources/{Config.bot.worldName}/{path}", $"Resources/{Config.bot.worldName}/Deleted/{path}");
        }

        public static void ClearDeleted()
        {
            Directory.Delete($"Resources/{Config.bot.worldName}/Deleted", true);
        }

        public static bool DataExists(string path)
        {
            if (path.EndsWith(".json"))
                return File.Exists($"Resources/{Config.bot.worldName}/{path}");
            else
                return Directory.Exists($"Resources/{Config.bot.worldName}/{path}");
        }

        public static void CheckPath()
        {
            if (!Directory.Exists("Resources")) Directory.CreateDirectory("Resources");
            if (!Directory.Exists($"Resources/{Config.bot.worldName}")) Directory.CreateDirectory($"Resources/{Config.bot.worldName}");
        }
    }
}
*/
