using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Translation
{
    public static List<TranslationEntry> entryList = new List<TranslationEntry>();
    public static void Load(string language)
    {
        // Load the translations to the entries.
        INIParser ini = new INIParser();
        ini.Open(string.Format(FilePaths.language, language));
        string section = "lang";

        foreach(TranslationEntry entry in Translation.entryList.ToArray())
        {
            entry.content = ini.ReadValue(section, entry.index, "");
        }

        // Create entries if they dont already exist
        string[] lines = File.ReadAllLines(string.Format(FilePaths.language, language));

        foreach (string line in lines)
        {
            if (line.StartsWith("#")) { continue; }
            if (line.StartsWith("[")) { continue; }

            string _line = line.ToLower();
            string[] words = _line.Split(' ');

            if ((words[0] != null) || (words[0] == " ")) { continue; }
            if (!HasIndex(words[0]))
            {
                Debug.Log($"Added missing translation index\"{words[0]}\"");
                Translation.entryList.Add(new TranslationEntry(words[0]));
            }
        }

        Debug.Log(Translation.entryList.Count);
        ini.Close();
    }

    public static string GetIndex(string index)
    {
        foreach(TranslationEntry entry in Translation.entryList.ToArray())
        {
            if (entry.index.ToLower() == index.ToLower())
            {
                return entry.content;
            }
        }
        return "";
    }

    public static bool HasIndex(string index)
    {
        foreach(TranslationEntry entry in Translation.entryList.ToArray())
        {
            if (entry.index.ToLower() == index.ToLower())
            {
                return true;
            }
        }
        return false;
    }

    public static TranslationEntry
        languageName = new TranslationEntry("languageName"); // The misspellling is intentional.

    public struct Countries
    {
        public struct FullName
        {
            public static TranslationEntry
                alaska = new TranslationEntry("alaskaFull"),
                albania = new TranslationEntry("albaniaFull"),
                argentina = new TranslationEntry("argentinaFull"),
                australia = new TranslationEntry("australia"),
                bolivia = new TranslationEntry("boliviaFull"),
                bosnia = new TranslationEntry("bosniaFull"),
                brazil = new TranslationEntry("brazilFull"),
                canada = new TranslationEntry("canadaFull"),
                carribean = new TranslationEntry("carribeanFull"),
                centralAfrica = new TranslationEntry("centralAfricaFull"),
                centralAmerica = new TranslationEntry("centralAmericaFull"),
                centralAsia = new TranslationEntry("centralAsiaFull"),
                chile = new TranslationEntry("chileFull"),
                china = new TranslationEntry("chinaFull"),
                colombia = new TranslationEntry("colombiaFull"),
                czech = new TranslationEntry("czechFull"),
                eastAfrica = new TranslationEntry("eastAfricaFull"),
                ecuador = new TranslationEntry("ecuadorFull"),
                europe = new TranslationEntry("europeFull"),
                franceGuinea = new TranslationEntry("franceGuineaFull"),
                greenland = new TranslationEntry("greenlandFull"),
                indonesia = new TranslationEntry("indonesiaFull"),
                japan = new TranslationEntry("japanFull"),
                mexico = new TranslationEntry("mexicoFull"),
                moldova = new TranslationEntry("moldovaFull"),
                mongolia = new TranslationEntry("mongoliaFull"),
                montenegro = new TranslationEntry("montenegroFull"),
                newGuinea = new TranslationEntry("newGuineaFull"),
                northAfrica = new TranslationEntry("northAfricaFull"),
                northKorea = new TranslationEntry("northKoreaFull"),
                norway = new TranslationEntry("norwayFull"),
                paraguay = new TranslationEntry("paraguayFull"),
                peru = new TranslationEntry("peruFull"),
                russia = new TranslationEntry("russiaFull"),
                serbia = new TranslationEntry("serbiaFull"),
                southEastAsia = new TranslationEntry("southEastAsiaFull"),
                southernAfrica = new TranslationEntry("southernAfricaFull"),
                southernAsia = new TranslationEntry("southernAsia"),
                southKorea = new TranslationEntry("southKoreaFull"),
                southPole = new TranslationEntry("southPoleFull"),
                ukraine = new TranslationEntry("ukraineFull"),
                unitedStates = new TranslationEntry("unitedStatesFull"),
                uruguay = new TranslationEntry("uruguayFull"),
                venezuela = new TranslationEntry("venezuelaFull"),
                westAfrica = new TranslationEntry("westAfricaFull"),
                westernAsia = new TranslationEntry("westernAsiaFull");
        }
        public struct ShortName
        {
            public static TranslationEntry
                alaska = new TranslationEntry("alaskaShort"),
                albania = new TranslationEntry("albaniaShort"),
                argentina = new TranslationEntry("argentinaShort"),
                australia = new TranslationEntry("australia"),
                bolivia = new TranslationEntry("boliviaShort"),
                bosnia = new TranslationEntry("bosniaShort"),
                brazil = new TranslationEntry("brazilShort"),
                canada = new TranslationEntry("canadaShort"),
                carribean = new TranslationEntry("carribeanShort"),
                centralAfrica = new TranslationEntry("centralAfricaShort"),
                centralAmerica = new TranslationEntry("centralAmericaShort"),
                centralAsia = new TranslationEntry("centralAsiaShort"),
                chile = new TranslationEntry("chileShort"),
                china = new TranslationEntry("chinaShort"),
                colombia = new TranslationEntry("colombiaShort"),
                czech = new TranslationEntry("czechShort"),
                eastAfrica = new TranslationEntry("eastAfricaShort"),
                ecuador = new TranslationEntry("ecuadorShort"),
                europe = new TranslationEntry("europeShort"),
                franceGuinea = new TranslationEntry("franceGuineaShort"),
                greenland = new TranslationEntry("greenlandShort"),
                indonesia = new TranslationEntry("indonesiaShort"),
                japan = new TranslationEntry("japanShort"),
                mexico = new TranslationEntry("mexicoShort"),
                moldova = new TranslationEntry("moldovaShort"),
                mongolia = new TranslationEntry("mongoliaShort"),
                montenegro = new TranslationEntry("montenegroShort"),
                newGuinea = new TranslationEntry("newGuineaShort"),
                northAfrica = new TranslationEntry("northAfricaShort"),
                northKorea = new TranslationEntry("northKoreaShort"),
                norway = new TranslationEntry("norwayShort"),
                paraguay = new TranslationEntry("paraguayShort"),
                peru = new TranslationEntry("peruShort"),
                russia = new TranslationEntry("russiaShort"),
                serbia = new TranslationEntry("serbiaShort"),
                southEastAsia = new TranslationEntry("southEastAsiaShort"),
                southernAfrica = new TranslationEntry("southernAfricaShort"),
                southernAsia = new TranslationEntry("southernAsia"),
                southKorea = new TranslationEntry("southKoreaShort"),
                southPole = new TranslationEntry("southPoleShort"),
                ukraine = new TranslationEntry("ukraineShort"),
                unitedStates = new TranslationEntry("unitedStatesShort"),
                uruguay = new TranslationEntry("uruguayShort"),
                venezuela = new TranslationEntry("venezuelaShort"),
                westAfrica = new TranslationEntry("westAfricaShort"),
                westernAsia = new TranslationEntry("westernAsiaShort");
        }
    }
    public struct Panels
    {
        public static TranslationEntry
            infoHeader              = new TranslationEntry("infoHeader"),
            confirmSelectionText    = new TranslationEntry("confirmSelectionText"),
            creationPanelTitle      = new TranslationEntry("creationPanelTitle"),
            creationPanelText       = new TranslationEntry("creationPanelText"),
            newsInfoText            = new TranslationEntry("newsInfoText"),
            newsNameInputText       = new TranslationEntry("newsNameInputText"),
            newsBeliverInputText    = new TranslationEntry("newsBeliverInputText"),
            newsDisbeliverInputText = new TranslationEntry("newsDisbeliverInputText"),
            newsAdjectiveInputText  = new TranslationEntry("newsAdjectiveInputText"),
            newsTypetext            = new TranslationEntry("newsTypetext"),
            newsAttributeText       = new TranslationEntry("newsAttributeText");
    }
    public struct Date
    {
        public static TranslationEntry
            dateDisplay = new TranslationEntry("dateDisplay", "DD/MM/YYYY");
        public struct Months
        {
            public static TranslationEntry
                january = new TranslationEntry("january", "January"),
                february = new TranslationEntry("february", "February"),
                march = new TranslationEntry("march", "March"),
                april = new TranslationEntry("april", "April"),
                may = new TranslationEntry("may", "May"),
                june = new TranslationEntry("june", "June"),
                july = new TranslationEntry("july", "July"),
                august = new TranslationEntry("august", "August"),
                september = new TranslationEntry("september", "September"),
                october = new TranslationEntry("october", "October"),
                november = new TranslationEntry("november", "November"),
                december = new TranslationEntry("december", "December");
        }
    }
    public struct Events
    {
        public struct Generic
        {
            public static TranslationEntry
                firstPopTitle = new TranslationEntry("firstPopTitle"),
                firstPopText = new TranslationEntry("firstPopText");
        }
    }
}
public class TranslationEntry
{
    
    /// <summary>
    /// This translation index/name in the translation files.
    /// </summary>
    public string index;

    /// <summary>
    /// The string content of the translation
    /// </summary>
    public string content;

    public static implicit operator string(TranslationEntry etr) => etr.content;
    public static explicit operator TranslationEntry(string str) => new TranslationEntry(str);

    /// <summary>
    /// A translation entry.
    /// </summary>
    /// <param name="indexname">This translation index/name in the translation files.</param>
    public TranslationEntry(string indexname)
    {
        this.index = indexname;
        this.content = "";
        Translation.entryList.Add(this);
    }

    /// <summary>
    /// A translation entry.
    /// </summary>
    /// <param name="indexname">This translation index/name in the translation files.</param>
    /// <param name="defaultContent">The default content of the translation</param>
    public TranslationEntry(string indexname, string defaultContent)
    {
        this.index = indexname;
        this.content = defaultContent;
        Translation.entryList.Add(this);
    }
}