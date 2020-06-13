using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Translation : MonoBehaviour
{
    public static Dictionary<string, string> entries = new Dictionary<string, string>();
    static INIParser ini = new INIParser();


    public static void Load(string file)
    {
        if (!file.EndsWith(".ini"))
        {
            file = file + ".ini";
        }

        entries.Clear();
        string[] lines = File.ReadAllLines(file);
        ini.Open(file);
        foreach(string line in lines)
        {
            if (line.StartsWith("#")) { continue; }
            if (line.Trim() == "") { continue; }

            string[] words = line.Split('=');

            string word = words[0].Trim();

   
            if(!entries.ContainsKey(word))
            {
                entries.Add(word, ini.ReadValue("lang", word, "Invalid Entry"));
            }
            else
            {
                Debug.LogWarning($"ATTENTION! Tried to load a already existant translation key \"{word}\"");
            }
        }
        ini.Close();
    }

    public static string Get(string entryname)
    {
        foreach(KeyValuePair<string, string> entry in entries)
        {
            if (entry.Key.ToLower() == entryname.ToLower())
            {
                return entry.Value;
            }
        }
        return null;
    }

}