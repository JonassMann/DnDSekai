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
            List<string> allPaths = new List<string>();

            foreach (Tuple<string, string> t in data)
                allPaths.Add(t.Item1.Substring(0, t.Item1.LastIndexOf('/')));

            IEnumerable<string> paths = allPaths.Distinct();

            foreach (string s in paths)
            {
                string[] folders = s.Split('/');
                string folderPath = "";
                for (int i = 0; i < folders.Length; i++)
                {
                    if (folderPath != "") folderPath += "/";
                    folderPath += folders[i];
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);
                }
            }

            foreach (Tuple<string, string> d in data ?? Enumerable.Empty<Tuple<string, string>>())
                File.WriteAllText(d.Item1, d.Item2);
        }

        public static void SaveData(string path, string json)
        {
            string[] folders = path.Substring(0, path.LastIndexOf('/')).Split('/');
            string folderPath = "";
            for (int i = 0; i < folders.Length; i++)
            {
                if (folderPath != "") folderPath += "/";
                folderPath += folders[i];
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);
            }

            File.WriteAllText(path, json);
        }

        //Loads all data from folders in "paths"
        public static List<Tuple<string, string>> LoadData(List<string> paths)
        {
            List<Tuple<string, string>> files = new List<Tuple<string, string>>();

            foreach (string s in paths ?? Enumerable.Empty<string>())
            {
                string[] folders = s.Split('/');
                string folderPath = "";
                for (int i = 0; i < folders.Length - (folders.Last().EndsWith(".json") ? 1 : 0); i++)
                {
                    if (folderPath != "") folderPath += "/";
                    folderPath += folders[i];
                    if (!Directory.Exists(folderPath)) return null;
                }

                if (folders.Last().EndsWith(".json"))
                {
                    if (!File.Exists(s)) return null;
                    files.Add(new Tuple<string, string>(s, File.ReadAllText(s)));
                }
                else
                {
                    foreach (string file in Directory.EnumerateFiles(s, "*.json", SearchOption.AllDirectories))
                        files.Add(new Tuple<string, string>(file.Replace('\\','/'), File.ReadAllText(file)));
                }
            }
            return files;
        }

        public static List<Tuple<string, string>> LoadData(string path)
        {
            List<Tuple<string, string>> files = new List<Tuple<string, string>>();

            if (!Directory.Exists(path)) return files;

            if (path.EndsWith(".json"))
            {
                if (!File.Exists(path)) return null;
                files.Add(new Tuple<string, string>(path, File.ReadAllText(path)));
            }
            else
            {
                foreach (string file in Directory.EnumerateFiles(path, "*.json", SearchOption.AllDirectories))
                    files.Add(new Tuple<string, string>(file.Replace('\\', '/'), File.ReadAllText(file)));
            }
            
            return files;
        }

        public static void DeleteData(List<string> paths)
        {
            foreach (string s in paths ?? Enumerable.Empty<string>())
            {
                if (s.EndsWith(".json"))
                    File.Delete(s);
                else
                    Directory.Delete(s, true);
            }
        }

        public static bool DataExists(string path)
        {
            if (path.EndsWith(".json"))
                return File.Exists(path);
            else
                return Directory.Exists(path);
        }
    }
}
