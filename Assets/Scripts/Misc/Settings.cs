using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GroundZero.Managers;
using System.IO;
using UnityEditor.UIElements;

namespace GroundZero
{
    public class Settings
    {
        public static List<Setting> list = new List<Setting>();
        public static void Load()
        {
            // Create a new INI Parser
            INIParser ini = new INIParser();

            // Open the game's setting file for reading.
            string iniFile = Game.FilePaths.settings;

            // Open the file with INI Parser
            ini.Open(iniFile);

            Debug.Log("Opening File " + iniFile);

            // Store the file contents to a string array
            string[] lines = File.ReadAllLines(iniFile);
            
            // Read each contents from the string array
            foreach(string line in lines)
            {
                // Ignore comment and section lines
                if (line.StartsWith("[")) { continue; }
                if (line.StartsWith("]")) { continue; }
                if (line.StartsWith("/")) { continue; }
                if (line.StartsWith("#")) { continue; }
                if (line.StartsWith("!")) { continue; }

                // Split string into words
                string[] words = line.Split(' ');

                // Create a new setting with the first word (the INI key for it)
                list.Add(new Setting(words[0]));
                Debug.Log($"Added {words[0]} to settings list.");
            }


            // Loop through the setting keys and assign their value.
            foreach (Setting setting in Settings.list)
            {
                setting.value = ini.ReadValue("settings", setting.key, "");
            }

            ini.Close();
        }

        public static Setting Get(string key, bool ignoreCase)
        {
            if (ignoreCase)
            {
                return list.Find(item => item.key.ToLower() == key.ToLower());
            }
            else
            {
                return list.Find(item => item.key == key);
            }
        }
        
        public static Setting Get(string key) => Get(key, true);
        
        public static KeyCode GetKey(string key)
        {
            return list.Find(item => item.key == key).toKeyCode();
        }

        public static string GetString(string key)
        {
            return list.Find(item => item.key == key);
        }


    }
    public class Setting
    {
        public string key, value;
        public Setting(string key)
        {
            this.key = key;
            this.value = "";
            Settings.list.Add(this);
        }
        public Setting(string key, string value)
        {
            this.key = key;
            this.value = value;
            Settings.list.Add(this);
        }

        public KeyCode toKeyCode()
        {
            return (KeyCode)System.Enum.Parse(typeof(KeyCode), value);
        }

        public static implicit operator int(Setting s) => int.Parse(s.value);

        public static implicit operator bool(Setting s) => bool.Parse(s.value);

        public static implicit operator float(Setting s) => float.Parse(s.value);

        public static implicit operator string(Setting s) => s.value;
    }
}
