using System.Collections.Generic;
using UnityEngine;

public class Events : MonoBehaviour
{
    public static List<Event> eventList = new List<Event>();

    public static Event firstPop = new Event
    (
        "firstPop",
        new Condition[]
        {
            new Condition("Beliver", "World", "GreaterThan", "2")
        },
        Event.Type.TriggerOnce,
        new Effect[]
        {
            new Effect("Pop", "Brazil", "Add", "25")
        }
    );

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
    };
    static string[] firstNames =
    {
        "Austin", "Arnold", "Albert", "Alberto", "Arnaldo", "Amy", "Amy", "Anna", "Ana", "Alexander", "Alex", "Alexa", "Adolf",
        "Bernardo", "Breno", "Bon", "Bennie", "Beatriz", "Beatrice", "Bea",
        "Charlie", "Charles", "Chief", "Carl", "Cuca", "Caio", "Cindy",
        "Daniel", "Deyde", "Donald", "Dick", "Dinah", "Don", "Damares", "Damaris", "Dâmaris",
        "Erwin", "Edward",
        "Ferdinando", "Fernando", "Fernanda", "Faustão",
        "Guile", "Gulliver", "Gustavo", "Gustav",
        "Homer", "Haru", "Hita", "Hillary", "Hugh",
        "Iminy",
        "Juliet", "James", "John", "Jack", "Joe", "Jean", "João", "Juan", "Joan", "Jay", "Jaymes", "Joana",
        "Karl", "Kayne", "Kane",
        "Luna", "Luan", "Laurence", "Leto", "Lito",
        "Maria", "Manuel", "Max", "Maxwell",
        "Natan", "Nathan", "Nicolas", "Nicholas", "Nikolas", "Neo",
        "Oscar", "Odwin",
        "Paul", "Paulo", "Paru", "Pedro", "Peter", "Pauline", "Patricia", "Patrick",
        "Q.",
        "Romeo", "Raul",
        "Stuart", "Steve", "Shirley", "Stella",
        "Thomas", "Thomás", "Truman", "Trump", "Terry", "Tom", "Terrence",
        "Umberto",
        "V.",
        "Winderson", "Windy", "Waffles",
        "Xaun",
        "Yderson", "Yumi", "Yoko",
        "Z."
    };
    static string[] secondNames =
    {
        "Lockheart", "Montgommery", "Deford", "Rockford", "Gater", "Clinton", "Powers", "Jameson", "Crews",
        "Miller", "Smith", "Jones", "Williams", "Davis", "Wilson", "Rodriguez", "Sanchez", "Fernandez", "Garcia",
        "Martín", "Martíns", "Pessoa", "dos Santos", "Santos", "Igrejas", "González", "Machado", "Santana",
        "Jamesson", "Johanesson", "White"
    };

    static string[] specialNames =
    {
        "Terry Crews", "X AE A12", "John Cena", "Karl Marx", "Joseph Stalin", "Donald Trump", "Hillary Clinton",
        "Barack Obama",  "Cindy Montgommery", "Terrence Deford", "James Rockford", "Al. E Gater", "Ben Dover",
        "Hugh Raye", "Dick Tate", "Kay Oss", "Jim Nassium", "Earl E. Bird", "Felix Cited", "Lori Driver",
        "Sally Forth", "Yul B. Allwright", "Max E. Mumm", "Heywood U Cuddleme", "Bea O' Problem",
        "Ovuvuevuevue Enyetuenwuevue Ugbemugbem Osas", "Raul Menendez", "\"Section\""
    };

    public static string GetRandomSocialMedia()
    {
        return socialMedias[Random.Range(0, socialMedias.Length-1)];
    }

    public static string GetRandomHackerGroup()
    {
        return hackerGroups[Random.Range(0, hackerGroups.Length-1)];
    }

    public static string GetRandomName()
    {
        int num = Random.Range(0, 10);

        if (num == 10)
        {
            string firstname = firstNames[Random.Range(0, firstNames.Length - 1)];
            string secondname = firstNames[Random.Range(0, firstNames.Length - 1)];

            return firstname + " " + secondname;
        } else
        {
            string name = specialNames[Random.Range(0, specialNames.Length - 1)];

            return name;
        }
    }

    public static string GetSpecialName()
    {
        string name = specialNames[Random.Range(0, specialNames.Length - 1)];
        return name;
    }

    public static void Call(string name)
    {
        foreach(Event ev in eventList)
        {
            if (ev.name == name)
            {
                Call(ev);
            }
        }
    }

    public static void Call(Event ev)
    {
        if (ev.eventType == Event.Type.TriggerOnce)
        {
            if (ev.hasTriggered) { return; }
        }

        if (ev.AllConditionsFullfilled())
        {
            ev.ApplyEffects();
            GameManager.instance.CallEventScreen(ev.name);
            ev.hasTriggered = true;
        }
    }

    public static void Refresh()
    {
        // Cancel if player is already busy with another UI.
        if (GameManager.instance.isUIOpen()) {return; }


        foreach(Event ev in eventList)
        {
            Call(ev);
        }
    }
}

public class Event
{
    public string name;
    public Condition[] conditions;
    public Effect[] effects;
    public enum Type { TriggerOnce, Timed }
    public Type eventType = Type.TriggerOnce;
    public bool hasTriggered = false;
    public Event(string name, Condition[] conditions, Type eventType, Effect[] effects)
    {
        this.name = name;
        this.conditions = conditions;
        this.eventType = eventType;
        this.effects = effects;
        Events.eventList.Add(this);
    }

    public Event(string name, Condition[] conditions, Type eventType, Effect[] effects, int eventTimer)
    {
        this.name = name;
        this.conditions = conditions;
        this.eventType = eventType;
        this.effects = effects;
        Events.eventList.Add(this);
    }

    public bool AllConditionsFullfilled()
    {
        foreach(Condition condition in conditions)
        {
            if (!condition.isFullfilled())
            {
                return false;
            }
        }
        return true;
    }

    public void ApplyEffects()
    {
        foreach(Effect effect in effects)
        {
            effect.Apply();
        }
    }
}

public class Condition
{

    public string trigger;
    public string target;
    public string operation;
    public string value;
    public Condition(string trigger, string target, string operation, string value)
    {
        this.trigger = trigger;
        this.target = target;
        this.operation = operation;
        this.value = value;
    }

    public Condition(string trigger, string operation, string value)
    {
        this.trigger = trigger.ToLower();
        this.operation = operation.ToLower();
        this.value = value.ToLower();
        this.target = "world";
    }

    public bool CheckOperation(string operation, uint firstvalue, uint secondvalue)
    {
        if (operation == "equals")
        {
            if (firstvalue == secondvalue) { return true; }
        } else
        if (operation == "lessthan")
        {
            if (firstvalue < secondvalue) { return true; }
        } else
        if (operation == "greaterthan")
        {
            if (firstvalue > secondvalue) { return true;} 
        }
        return false;
    }

        public bool CheckOperation(string operation, float firstvalue, float secondvalue)
    {
        if (operation == "equals")
        {
            if (firstvalue == secondvalue) { return true; }
        } else
        if (operation == "lessthan")
        {
            if (firstvalue < secondvalue) { return true; }
        } else
        if (operation == "greaterthan")
        {
            if (firstvalue > secondvalue) { return true;} 
        }
        return false;
    }
    public bool isFullfilled()
    {
        Country country;
        if (target == "random")
        {
            country = GameManager.GetRandomCountry();
        } else
        if (target == "player")
        {
            country = Player.playerCountry;
        } else
        {
            country = GameManager.GetCountry(target);
        }
        
        if (country == null)
        {
            return false;
        }

        if (trigger == "pop")
        {
            if (target == "world")
            {
                return CheckOperation(operation, GameManager.GetWorldPop(), uint.Parse(value));
            }
            return CheckOperation(operation, country.population, uint.Parse(value));
        } else
        if ((trigger == "beliver") || (trigger == "belivers"))
        {
            if (target == "world")
            {
                return CheckOperation(operation, GameManager.GetWorldPop(), uint.Parse(value));
            }
            return CheckOperation(operation, country.belivers, uint.Parse(value));
        } else
        if ((trigger == "disbeliver") || (trigger == "disbelivers"))
        {
            if (target == "world")
            {
                return CheckOperation(operation, GameManager.GetWorldPop(), uint.Parse(value));
            }
            return CheckOperation(operation, country.disbelivers, uint.Parse(value));
        } else
        if (trigger == "stability")
        {
            if (target == "world")
            {
                return CheckOperation(operation, GameManager.GetWorldPop(), uint.Parse(value));
            }
            return CheckOperation(operation, country.stability, float.Parse(value));
        } else
        if (trigger == "boolean")
        {
            if (operation == "equals")
            {
                if (value == "true")
                {
                    return true;
                } else
                {
                    return false;
                }
            }
        }
        return false;
    }
}

public class Effect
{
    public string type;
    public string target;
    public string operation;
    public string value;

    public Effect(string type, string target, string operation, string value)
    {
        this.type = type.ToLower();
        this.target = target.ToLower();
        this.operation = operation.ToLower();
        this.value = value.ToLower();
    }

    public void DoOperation(uint firstvalue, string operation, uint secondvalue)
    {
        if (operation == "add")
        {
            firstvalue += secondvalue;
        } else
        if (operation == "subtract")
        {
            firstvalue -= secondvalue;
        } else
        if (operation == "multiply")
        {
            firstvalue *= secondvalue;
        } else
        if (operation == "divide")
        {
            firstvalue /= secondvalue;
        } else
        if (operation == "set")
        {
            firstvalue = secondvalue;
        }
    }

        public void DoOperation(float firstvalue, string operation, float secondvalue)
    {
        if (operation == "add")
        {
            firstvalue += secondvalue;
        } else
        if (operation == "subtract")
        {
            firstvalue -= secondvalue;
        } else
        if (operation == "multiply")
        {
            firstvalue *= secondvalue;
        } else
        if (operation == "divide")
        {
            firstvalue /= secondvalue;
        } else
        if (operation == "set")
        {
            firstvalue = secondvalue;
        }
    }
    public void Apply()
    {
        Country country;
        if (target == "world")
        {
            country = GameManager.GetRandomCountry();
        } else
        if (target == "player")
        {
            country = Player.playerCountry;
        } else
        {
            country = GameManager.GetCountry(target);
        }

        if (country == null)
        {
            return;
        }

        if (type == "pop")
        {
            DoOperation(country.population, operation, uint.Parse(value));
        } else
        if (type == "beliver")
        {
            DoOperation(country.belivers, operation, uint.Parse(value));
        } else
        if (type == "disbeliver")
        {
            DoOperation(country.disbelivers, operation, uint.Parse(value));
        } else
        if (type == "stability")
        {
            DoOperation(country.stability, operation, float.Parse(value));
        }
        if ((type == "wealth") || (type == "money"))
        {
            DoOperation(country.wealth, operation, float.Parse(value));
        }
    }
}