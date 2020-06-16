using System.Collections.Generic;
using UnityEngine;

public class Events : MonoBehaviour
{
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

}

public class Event
{
    public string name;
    public enum Type { TriggerOnce, Timed }
    public Type eventType = Type.TriggerOnce;
    public bool hasTriggered = false;
}