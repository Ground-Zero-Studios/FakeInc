using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace GroundZero
{

    public class Localization : MonoBehaviour
    {
        public static Dictionary<string, string> entries = new Dictionary<string, string>();
        static INIParser ini = new INIParser();
        public static bool isLoaded = false;

        public static void Load(string file)
        {
            if (!file.EndsWith(".ini"))
            {
                file = file + ".ini";
            }

            if (!File.Exists(file))
            {
                Debug.LogWarning("Skipping localization file \"" + file + "\": File not found");
                return;
            }
            
            string[] lines = File.ReadAllLines(file);
            ini.Open(file);
            foreach (string line in lines)
            {
                if (line.StartsWith("#")) { continue; }
                if (line.StartsWith("[")) { continue; }
                if (line.StartsWith("]")) { continue; }
                if (line.StartsWith(";")) { continue; }
                if (line.Trim() == "") { continue; }

                string[] words = line.Split('=');

                string word = words[0].Trim();


                if (!entries.ContainsKey(word))
                {
                    entries.Add(word, ini.ReadValue("lang", word, "Invalid Entry"));
                }
                else
                {
                    Debug.LogWarning($"ATTENTION! Tried to load a already existant translation key \"{word}\"");
                }
            }
            ini.Close();

            string dump = "";

            foreach (string entry in entries.Values)
            {
                dump += entry + "\n";
            }

            Debug.Log($"Localization for {file} loaded.\nEntries: {entries.Count}\nENTRY DUMP: {dump}");
            isLoaded = true;
        }

        public static void Clear()
        {
            entries.Clear();
        }

        public static IEnumerator LoadAsync(string file)
        {
            isLoaded = false;
            if (!file.EndsWith(".ini"))
            {
                file = file + ".ini";
            }

            string[] lines = File.ReadAllLines(file);
            ini.Open(file);

            int i = 0;
            while (i <= lines.Length - 1)
            {
                string line = lines[i];

                if (line.StartsWith("#")) { continue; }
                if (line.Trim() == "") { continue; }

                string[] words = line.Split('=');

                string word = words[0].Trim().ToLower();


                if (!entries.ContainsKey(word))
                {
                    entries.Add(word, ini.ReadValue("lang", word, "Invalid Entry"));
                }
                else
                {
                    Debug.LogWarning($"ATTENTION! Tried to load a already existent translation key \"{word}\"");
                }
                i++;
                yield return null;
            }
            ini.Close();
            Debug.Log($"Localization for {file} loaded.\nEntries: {entries.Count}");
            isLoaded = true;
        }

        public static string Get(string entryname)
        {
            if (entries.ContainsKey(entryname))
            {
                string result = entries[entryname];

                result = Regex.Unescape(result);
                
                return result;
            }

            return entryname;
        }

        public static bool Exists(string entryname)
        {
            if (entries.ContainsKey(entryname))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}