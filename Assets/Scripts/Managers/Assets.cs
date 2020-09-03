using GroundZero.Managers;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/* ----------------------------------------------------------------------------------------
 *                                        ATTENTION                                        
 * This code is being stored for reference only. It is deprecated and no longer supported.
 * This used to be the "modding" support for the game, which has been scrapped on 28/08/2020
 * 
 * 
 * 
 * ----------------------------------------------------------------------------------------
 */
namespace GroundZero.Assets
{
    public static class Mods
    {
        /*
        public static bool hasLoadedBaseMod = false;

        public static bool handlingEvents = false;
        public static List<Mod> list = new List<Mod>();
        public static void Load(string modspath)
        {
            Condition.LoadBuiltIns();
            Effect.LoadBuiltIns();
            string[] mods = Directory.GetFiles(modspath, "*.ini");

            foreach(string mod in mods)
            {
                mod.Replace('\\', '/');
                Mod m = new Mod();
                if (mod.EndsWith("BaseGame.ini")) { hasLoadedBaseMod = true; }
                m.LoadFromFile(mod);
                list.Add(m);
            }

            if (!hasLoadedBaseMod)
            {
                throw new System.Exception("Failed to load base mod file.");
            }
        }
        */
    }


    public class Mod
    {
        /*
        public string Name, Identifier, Desc, Author, Icon;

        public Dictionary<int, string> sounds = new Dictionary<int, string>();
        public Dictionary<int, string> resources = new Dictionary<int, string>();
        public Dictionary<int, string> events = new Dictionary<int, string>();
        public Dictionary<int, string> conditions = new Dictionary<int, string>();
        public Dictionary<int, string> effects = new Dictionary<int, string>();

        public List<Event> eventList = new List<Event>();
        public Dictionary<int, Condition> conditionList = new Dictionary<int, Condition>();
        public Dictionary<int, Effect> effectList = new Dictionary<int, Effect>();

        static INIParser ini = new INIParser();


        public void LoadFromFile(string file)
        {
            if (!File.Exists(file))
            {
                Debug.LogWarning("Attempted to load missing mod file: " + file);
                return;
            }

            ini.Open(file);
            Name = ini.ReadValue("Mod", "Name", "Unknown Mod");
            Identifier = ini.ReadValue("Mod", "Identifier", "UnknownMod");
            Desc = ini.ReadValue("Mod", "Desc", "No Description");
            Author = ini.ReadValue("Mod", "Author", "Unknown Author");
            Icon = ini.ReadValue("Mod", "Icon", "ERR");

            int i = 0;
            while (ini.IsKeyExists("Sounds", i.ToString()) && i < 201)
            {
                string val = ini.ReadValue("Sounds", i.ToString(), "ERR");
                if (val == "0") { continue; }
                if (sounds.ContainsKey(i)) { continue; }
                sounds.Add(i, val);
                i++;
            }


            i = 0;
            while (ini.IsKeyExists("Resources", i.ToString()) && i < 51)
            {
                string val = ini.ReadValue("Resources", i.ToString(), "ERR");
                if (val == "0") { continue; }
                if (resources.ContainsKey(i)) { continue; }
                resources.Add(i, val);
                Condition.tests.Add(val);
                i++;
            }

            i = 0;
            while (ini.IsKeyExists("Events", i.ToString()) && i < 1001)
            {
                string val = ini.ReadValue("Events", i.ToString(), "ERR");
                if (val == "0") { continue; }
                if (events.ContainsKey(i)) { continue; }
                events.Add(i, val);
                i++;
            }

            i = 0;
            while (ini.IsKeyExists("Conditions", i.ToString()) && i < 6001)
            {
                string val = ini.ReadValue("Conditions", i.ToString(), "ERR");
                if (val == "0") { continue; }
                if (conditions.ContainsKey(i)) { continue; }
                conditions.Add(i, val);
                i++;
            }

            i = 0;
            while (ini.IsKeyExists("Effects", i.ToString()) && i < 6001)
            {
                string val = ini.ReadValue("Effects", i.ToString(), "ERR");
                if (val == "0") { continue; }
                if (effects.ContainsKey(i)) { continue; }
                effects.Add(i, val);
                i++;
            }


            // Create Conditions

            for (int k = 0; k < conditions.Count; k++)
            {
                if (ini.IsSectionExists(conditions[k]))
                {
                    Condition cond = new Condition();
                    cond.Test = ini.ReadValue(conditions[k], "Test", "Nothing");
                    cond.Operator = ini.ReadValue(conditions[k], "Operator", "Nothing");
                    cond.Scope = ini.ReadValue(conditions[k], "Scope", "Nothing");
                    cond.Value = ini.ReadValue(conditions[k], "Value", "Nothing");

                    cond.Register();

                    conditionList.Add(k, cond);
                }
            }



            // Create Effects

            for (int k = 0; k < effects.Count; k++)
            {
                if (ini.IsSectionExists(effects[k]))
                {
                    Effect eff = new Effect();
                    eff.Variable = ini.ReadValue(effects[k], "Variable", "Nothing");
                    eff.Scope = ini.ReadValue(effects[k], "Scope", "Nothing");
                    eff.Operator = ini.ReadValue(effects[k], "Operator", "Nothing");
                    eff.Value = ini.ReadValue(effects[k], "Value", "Nothing");

                    effectList.Add(k, eff);
                }
            }


            // Create Events
            for (int k = 0; k < events.Count; k++)
            {
                if (ini.IsSectionExists(events[k]))
                {
                    if (!ini.IsKeyExists(events[k], "Identifier")
                        || !ini.IsKeyExists(events[k], "Conditions")
                        || !ini.IsKeyExists(events[k], "Effects"))
                    {
                        continue;
                    }
                }

                Event ev = new Event(this);
                ev.Identifier = ini.ReadValue(events[k], "Identifier", "NULL");

                string condString = ini.ReadValue(events[k], "Conditions", "0");
                string effString = ini.ReadValue(events[k], "Effects", "0");

                string[] conds = condString.Split(',');
                string[] effs = effString.Split(',');

                foreach(string cond in conds) // cond will be "1", "2" etc.
                {
                    int index = int.Parse(cond);
                    ev.conditions.Add(conditionList[index]);
                }

                foreach(string eff in effs)
                {
                    int index = int.Parse(eff);
                    ev.effects.Add(effectList[index]);
                }

                eventList.Add(ev);

            }

            ini.Close();

        }
        */
    }

    public class Event
    {/*
        static string[] socialMedias =
        {
        "Facebook", "Twitch", "Instagram", "Discord", "MySpace", "Fandom", "Wiki",
        "WhatsApp", "YouTube", "VidLii", "Snapchat", "Skype", "Spotify", "Xbox", "Playstation", "Wii U Online"
        };

        static string[] hackerGroups =
        {
        "Anonymous", "HackerZ", "IBF", "Legion of Doom", "Chaos Computer Club",
        "Homebrew Computer Club", "Masters of Deception", "Lizard Squad", "Goat of Numbers",
        "Nine-Tailed-Fox", "Hammer Down", "LeetHaxxorz", "TheUnHacked", "TheTruth", "Truth Revealers",
        "Justice Warriors", "Bright Side", "Dark Side", "Gray Side", "Joestar's Technique", "Leo's Truth",
        "Trumped", "Democracy", "Democrats For Justice", "Democrats for Truth", "Democrats for Union",
        "Republicans For Justice", "Republicans For Truth", "Gray Tide", "L33T HAXXRS", "RealCheaters",
        "Green Alert", "Green Plague", "Sample Text", "The Voice of Reason", "TH3 V0RTEX", "Due to Rag",
        "The Bearscapers", "Greentext", "Frasing Lees", "RKALX", "Striving for Justice", "Freedom Fighters",
        "Automata", "Pandemic Legion", "ClusterfuckData", "Weebos", "Wehraboos", "YiffCode", "GreenPaw",
        "Watch_Doggos", "Pandemic", "<CODE>", "Index", "KeyBoard", ".net!", ".package", "SharpEdge", "(C)Able"
        };

        static string[] firstNames =
        {
        "Austin", "Arnold", "Albert", "Alberto", "Arnaldo", "Amy", "Amy", "Anna", "Ana", "Alexander", "Alex", "Alexa", "Adolf",
        "Bernardo", "Breno", "Bon", "Bennie", "Beatriz", "Beatrice", "Bea",
        "Charlie", "Charles", "Chief", "Carl", "Cuca", "Caio", "Cindy", "Charlotte",
        "Daniel", "Deyde", "Donald", "Dick", "Dinah", "Don", "Damares", "Damaris", "Dâmaris",
        "Erwin", "Edward", "Edwin", "Earl", "Eva", "Emillia", "Emily", "Emilia", "Everton",
        "Ferdinando", "Fernando", "Fernanda", "Faustin",
        "Guile", "Gulliver", "Gustavo", "Gustav", "Gorbachev", "Guill", "Guile", "Guilherme",
        "Homer", "Haru", "Hita", "Hillary", "Hugh",
        "Iminy", "Ivy",
        "Juliet", "James", "John", "Jack", "Joe", "Jean", "João", "Juan", "Joan", "Jay", "Jaymes", "Joana",
        "Karl", "Kayne", "Kane", "Karlson", "Kira", "Katlin",
        "Luna", "Luan", "Laurence", "Leto", "Lito",
        "Maria", "Manuel", "Max", "Maxwell", "Michael", "Michel", "Miguel",
        "Natan", "Nathan", "Nicolas", "Nicholas", "Nikolas", "Neo",
        "Oscar", "Odwin",
        "Paul", "Paulo", "Paru", "Pedro", "Peter", "Pauline", "Patricia", "Patrick",
        "Q.", "Quebec",
        "Romeo", "Raul",
        "Stuart", "Steve", "Shirley", "Stella", "Shira", "Sazuki", "Saitama",
        "Thomas", "Thomás", "Truman", "Trump", "Terry", "Tom", "Terrence",
        "Umberto",
        "Vermillion",
        "Winderson", "Windy", "Waffles", "Wheatley",
        "Xaun",
        "Yderson", "Yumi", "Yoko",
        "Zyan"
        };

        static string[] secondNames =
        {
        "Aizawa","America","Armstrong","Austin","Azevedo",
        "Clinton","Crews","Crews","chan",
        "Davis","Deford","des","dos Santos",
        "Fernandez",
        "Garcia","Gater","González","Gorbachev",
        "Igrejas",
        "Jameson","Jamesson","Joestar","Johanesson","Johnson","Jones",
        "Karlson","Korea","kun",
        "Lockheart",
        "Machado","Martín","Martíns","Miller","Montgommery",
        "Newman",
        "Pessoa","Powers",
        "Rockford","Rodriguez",
        "Sam","Sanchez","Santana","Santos","Smith","Sun","Sundowner","sama",
        "Uzumaki",
        "VI Britannia", "von Helm",
        "Watson","White","Williams","Wilson"
        };

        static string[] specialNames =
        {
        "Terry Crews", "X AE B11", "John Cena", "Karl Marx", "Joseph Stalin", "Donald Trump", "Hillary Clinton",
        "Barack Obama",  "Cindy Montgommery", "Terrence Deford", "James Rockford", "Al. E Gater", "Ben Dover",
        "Hugh Raye", "Dick Tate", "Kay Oss", "Jim Nassium", "Earl E. Bird", "Felix Cited", "Lori Driver",
        "Sally Forth", "Yul B. Allwright", "Max E. Mumm", "Heywood U Cuddleme", "Bea O' Problem",
        "Ovuvuevuevue Enyetuenwuevue Ugbemugbem Osas", "Raul Menendez", "\"Section\"", "Major Leo",
        "xxxxxxxx_you're_about_to_get_fucked_up_by_scp_173_xxxxxxxx", "Master Chief", "Carl Johnson",
        "\"Mystic Magnestism\"", "\"The Life\"", "\"Fire Fox\"", "\"GLaDOS\"", "Big Smoke", "Ligma Bowls",
        "Jens", "\"Kaisar\"", "\"Pretentious Panda\"", "\"Hoovy\"", "\"incurser\"", "\"Cab\"", "\"Uri\"",
        "\"Lumoize\"", "\"Koala\"", "\"Oberynne\"", "\"Purdig\"", "Bill Nye", "Hdolf Atler", "Fegelein",
        "\"public class Name\"", "\"World Prop\"", "Jacques", "Marcelo", "Cindy", "\"OWO\"", "\"qwp\"",
        "Lelouch VI Britannia", "Light Yagami", "Dr. Octogonapus", "\"The Chosen One\"", "Beautrice",
        "Mello", "John Wick", "\"Albion Online\"", "\"EVE Online\"", "X", "Mr. X", "Faustão"
        };


        public static string GetRandomSocialMedia()
        {
            return socialMedias[UnityEngine.Random.Range(0, socialMedias.Length)];
        }

        public static string GetRandomHackerGroup()
        {
            return hackerGroups[UnityEngine.Random.Range(0, hackerGroups.Length)];
        }

        public static string GetRandomName()
        {
            int num = UnityEngine.Random.Range(0, 51);

            if (num == 50)
            {
                string name = specialNames[UnityEngine.Random.Range(0, specialNames.Length)];

                return name;
            }
            else
            {
                string firstname = firstNames[UnityEngine.Random.Range(0, firstNames.Length)];
                string secondname = firstNames[UnityEngine.Random.Range(0, firstNames.Length)];

                return firstname + " " + secondname;
            }
        }

        public static string GetSpecialName()
        {
            string name = specialNames[UnityEngine.Random.Range(0, specialNames.Length)];
            return name;
        }

        public bool
            useDefaultLocalization = false,
            displayScreen = false,
            repeating = false,
            delayed = false;

        private bool triggered = false;

        public string Identifier;

        public List<Condition> conditions = new List<Condition>();
        public List<Effect> effects = new List<Effect>();

        public Mod master;

        public Event(Mod master)
        {
            this.master = master;

        }

        public void TryTrigger()
        {
            foreach(Condition cond in conditions)
            { 
                if (!cond.isFullfilled())
                {
                    return;
                }
            }

            Trigger();
        }

        public void Trigger()
        {
            foreach(Effect eff in effects)
            {
                eff.Apply();
            }

            if (!repeating)
            {
                triggered = true;
            }
        }
        */
    }

    public class Condition
    {
        /*
        public static List<string> tests = new List<string>();
        public static List<string> operators = new List<string>();
        public static List<string> scopes = new List<string>();        

        public string Test, Operator, Scope, Value;

        public bool hasBeenMet = false;
        public enum CheckType
        {
            DayPass,MonthPass,YearPass,CountryThink,CountryClick,
        }

        public void Evaluate()
        {
            if (isFullfilled())
            {
                hasBeenMet = true;
            }
        }

        public static void LoadBuiltIns()
        {
            tests.Add("Day");
            tests.Add("Month");
            tests.Add("Year");

            operators.Add("Elapsed");
            operators.Add("Equals");
            operators.Add("Greater");
            operators.Add("Lesser");
        }
        public bool isFullfilled()
        {
            if ((!TestIsValid()) || (!OperatorIsValid()))
            {
                return false;
            }

            if (Test == "Day")
            {
                switch (Operator)
                {
                    case "Elapsed":
                        if (Date.elapsedDays >= int.Parse(Value))
                        {
                            return true;
                        }
                        break;
                    case "Equals":
                        if (Date.day == int.Parse(Value))
                        {
                            return true;
                        }
                        break;
                }
            } else
            if (Test == "Month")
            {

            } else
            if (Test == "Year")
            {

            }


            return false;
        }

        public void Register()
        {
            if ((!TestIsValid()) || (!OperatorIsValid()))
            {
                return;
            }


            switch (Operator)
            {

            }

        }



        bool TestIsValid()
        {
            if (tests.Contains(Test)) { return true; }
            return false;
        }

        bool OperatorIsValid()
        {
            if (operators.Contains(Operator)) { return true; }
            return false;
        }
        */
    }

    public class Effect
    {
        //public string Variable, Scope, Operator, Value;

        //public static void LoadBuiltIns()
        //{
        //    variables.Add("Nothing");
        //    variables.Add("PlaySound");

        //    scopes.Add("Nothing");
        //    scopes.Add("World");
        //    scopes.Add("Random");
        //    scopes.Add("Everyone");

        //    operators.Add("Add");
        //    operators.Add("Subtract");
        //    operators.Add("Divide");
        //    operators.Add("Multiply");
        //}

        //public void Apply()
        //{
        //    // TODO IMPLEMENT
        //}

        //public static List<string> variables = new List<string>();
        //public static List<string> scopes = new List<string>();
        //public static List<string> operators = new List<string>();



    }



}
