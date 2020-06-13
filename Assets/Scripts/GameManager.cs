using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // Game related stuff
    public enum Phase { Create, Select, Game, End }
    public Phase gamePhase =    Phase.Create;

    public static Player        player;

    public static List<Country> countries = new List<Country>();

    public static Country       selectedCountry = null;

    [Range(0f, 10f)]
    public float                gameSpeed = 1f;

    [Range(1, 31)]
    public int                  startingDay;
    [Range(1,12)]
    public int                  startingMonth;
    [Range(1,9999)]
    public int                  startingYear;

    public TextMeshProUGUI      dateDisplay;
    public TextMeshProUGUI      monthDisplay;
    public GameObject           confirmSelectionPanel;
    private TextMeshProUGUI     confirmSelectionTextDisplay;
    private string              confirmSelectionDefaultText;

    public GameObject           infoPanel;
    private TextMeshProUGUI     infoTextDisplay;
    private string              infoDefaultText;

    public GameObject           eventPanel;
    private TextMeshProUGUI     eventTextDisplay;
    private string              eventDefaultText;
    private TextMeshProUGUI     eventTitleDisplay;
    private string              eventDefaultTitle;
    private TextMeshProUGUI     eventButtonText;
    private string              eventButtonDefaultText;

    public GameObject           creationPanel;
    private TextMeshProUGUI     creationPanelTitle;
    private string              creationPanelDefaultTitle;
    private TextMeshProUGUI     creationPanelText;
    private string              creationPanelDefaultText;
    private Button              creationPanelStartButton;

    private GameObject          newsInfoArea;
    private TextMeshProUGUI     newsInfoText;
    private string              newsInfoDefaultText;
    private TMP_InputField      newsNameInput;
    private TextMeshProUGUI     newsNameInputText;
    private string              newsNameInputDefaultText;
    private TMP_InputField      newsAdjectiveInput;
    private TextMeshProUGUI     newsAdjectiveInputText;
    private string              newsAdjectiveInputDefaultText;
    private TMP_InputField      newsBeliverInput;
    private TextMeshProUGUI     newsBeliverInputText;
    private string              newsBeliverInputDefaultText;
    private TMP_InputField      newsDisbeliverInput;
    private TextMeshProUGUI     newsDisbeliverInputText;
    private string              newsDisbeliverInputDefaultText;
    private TMP_InputField[]    newsInfoInputs;

    private GameObject          newsTypeArea;
    private TextMeshProUGUI     newsTypeText;
    private string              newsTypeDefaultText;
    private TMP_Dropdown        newsTypeDropdown;

    private GameObject          newsAttributesArea;
    private TextMeshProUGUI     newsAttributesText;
    private string              newsAttributesDefaultText;


    private float updateTimer = 0f;
    private int gameTick = 0;

    void Start()
    {
        instance = this;
        Settings.Load();
        Translation.Load(Settings.language.value);

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        confirmSelectionTextDisplay = confirmSelectionPanel.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        confirmSelectionDefaultText = Translation.Panels.confirmSelectionText;
        confirmSelectionPanel.SetActive(false);

        infoTextDisplay = infoPanel.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        infoDefaultText = infoTextDisplay.text;
        infoPanel.SetActive(true);

        eventTitleDisplay = eventPanel.transform.Find("Title").GetComponent<TextMeshProUGUI>();
        eventDefaultTitle = eventTitleDisplay.text;
        eventTextDisplay = eventPanel.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        eventDefaultText = eventTextDisplay.text;
        eventButtonText = eventPanel.transform.Find("Button").transform.Find("Text").GetComponent<TextMeshProUGUI>();
        eventButtonDefaultText = eventButtonText.text;

        creationPanelTitle = creationPanel.transform.Find("Title").GetComponent<TextMeshProUGUI>();
        creationPanelDefaultTitle = Translation.Panels.creationPanelTitle;
        creationPanelText = creationPanel.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        creationPanelDefaultText = Translation.Panels.creationPanelText;
        creationPanelStartButton = creationPanel.transform.Find("StartButton").GetComponent<Button>();
        newsInfoArea = creationPanel.transform.Find("NewsInfoArea").gameObject;
        newsInfoText = newsInfoArea.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        newsInfoDefaultText = Translation.Panels.newsInfoText;

        newsNameInput = newsInfoArea.transform.Find("NewsNameInput").GetComponent<TMP_InputField>();
        newsNameInputText = newsNameInput.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        newsNameInputDefaultText = Translation.Panels.newsNameInputText;

        newsAdjectiveInput = newsInfoArea.transform.Find("NewsAdjectiveInput").GetComponent<TMP_InputField>();
        newsAdjectiveInputText = newsAdjectiveInput.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        newsAdjectiveInputDefaultText = Translation.Panels.newsAdjectiveInputText;

        newsBeliverInput = newsInfoArea.transform.Find("NewsBeliverInput").GetComponent<TMP_InputField>();
        newsBeliverInputText = newsBeliverInput.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        newsBeliverInputDefaultText = Translation.Panels.newsBeliverInputText;

        newsDisbeliverInput = newsInfoArea.transform.Find("NewsDisbeliverInput").GetComponent<TMP_InputField>();
        newsDisbeliverInputText = newsDisbeliverInput.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        newsDisbeliverInputDefaultText = Translation.Panels.newsDisbeliverInputText;
    }



    void Update()
    {
        if (gamePhase == Phase.Create)
        {
            if (!creationPanel.activeSelf)
            {
                creationPanel.SetActive(true);
            }

            if (canStartGame())
            {
                creationPanelStartButton.interactable = true;
            } else
            {
                creationPanelStartButton.interactable = false;
            }
        }

        // Game only ticks wen game is not in select mode, or in end screen.
        if (gamePhase == Phase.Game)
        {
            updateTimer -= Time.deltaTime;
        }

        // If it's time to update...
        if (updateTimer <= 0)
        {
            updateTimer = 1 / gameSpeed;
            // Call the game tick event.
            GameTick();
        }
    }


    public static void SetPlayerCountry(Country country)
    {
        Player.playerCountry = country;
    }


    public static Country GetCountry(string name)
    {
        foreach(Country country in countries)
        {
            if ((country.shortname == name) || (country.fullname == name))
            {
                return country;
            }
        }
        return null;
    }
    public void OnCountryClick(Country country)
    {
        // Select Phase
        if (isUIOpen())
        {
            return;
        }
        if (gamePhase == Phase.Select)
        {
            // If the country that we clicked isn't selected already
            if (selectedCountry != country)
            {
                // Select
                selectedCountry = country;
            }
            else
            {
                //If it's been already selected and we're clicking one more time...
                //Show the message to confirm the selection.

                confirmSelectionPanel.SetActive(true);
                
                confirmSelectionTextDisplay.text = string.Format(confirmSelectionDefaultText, country.shortname);
            }
        } else
        if (gamePhase == Phase.Game)
        {
            selectedCountry = country;
        }
    }

    public void OnButtonClick(string button)
    {
        if (button.Equals("Play",System.StringComparison.InvariantCultureIgnoreCase))
        {
            if (creationPanel.activeSelf)
            {
                if (canStartGame())
                {
                    creationPanel.SetActive(false);
                    gamePhase = Phase.Select;
                }
            }
        }
    }

    public bool canStartGame()
    {
        if (newsNameInput.text == "")       { return false; }
        if (newsAdjectiveInput.text == "")  { return false; }
        if (newsBeliverInput.text == "")    { return false; }
        if (newsDisbeliverInput.text == "") { return false; }

        return true;
    }

    public bool isUIOpen()
    {
        if (confirmSelectionPanel.activeSelf) { return true;  }
        if (eventPanel.activeSelf) { return true;  }
        if (creationPanel.activeSelf) { return true; }

        return false;
    }

    public static string Abbreviate(long num)
    {
        if (num > 999999999999999 || num < -999999999999999)
        {
            return num.ToString("0,,,,,,.#####Q", CultureInfo.InvariantCulture);
        }

        if (num > 999999999999 || num < -999999999999)
        {
            return num.ToString("0,,,,.####T", CultureInfo.InvariantCulture);
        }
        if (num > 999999999 || num < -999999999)
        {
            return num.ToString("0,,,.###B", CultureInfo.InvariantCulture);
        }
        else
        if (num > 999999 || num < -999999)
        {
            return num.ToString("0,,.##M", CultureInfo.InvariantCulture);
        }
        else
        if (num > 999 || num < -999)
        {
            return num.ToString("0,.#K", CultureInfo.InvariantCulture);
        }
        else
        {
            return num.ToString(CultureInfo.InvariantCulture);
        }
    }
    public void SetGamePhase(string phase)
    {
        if (phase.Equals("Create", System.StringComparison.InvariantCultureIgnoreCase))
        {
            gamePhase = Phase.Create;
        }
        if (phase.Equals("Select", System.StringComparison.InvariantCultureIgnoreCase))
        {
            gamePhase = Phase.Select;
        } else
        if (phase.Equals("Game", System.StringComparison.InvariantCultureIgnoreCase))
        {
            gamePhase = Phase.Game;
        } else
        if (phase.Equals("End", System.StringComparison.InvariantCultureIgnoreCase))
        {
            gamePhase = Phase.End;
        }
    }

    public static float GetWorldGrowthRate()
    {
        float result = 0;
        Country[] countryArray = countries.ToArray();
        foreach(Country country in countryArray)
        {
            result += (country.birthrate - country.deathrate);
        }

        result = result / countryArray.Length-1;
        return result;
    }

    public static Country GetRandomCountry()
    {
        Country[] countryArray = countries.ToArray();
        return countryArray[Random.Range(0,countryArray.Length-1)];
    }

    public static ulong GetWorldPop()
    {
        ulong result = 0;

        foreach(Country country in countries)
        {
            result += (ulong) country.population;
        }

        return result;
    }

    public static uint GetRandomPop(int countrySize)
    {
        int maxValue = (999999999 * countrySize) / 100;
        return (uint) Random.Range(1000, maxValue);
    }

    public static float GetRandomMoney(int countrySize)
    {
        return Random.Range(0, 101)/100;
    }

    public void GameTick() // 1 day per second, 10 ticks per second = 10 ticks per day
    {

        if (GameManager.instance.isUIOpen()) { return; } // Dont count time if player is busy with UI;
        gameTick += 1;

        if (gameTick > 10) // Once every 10 ticks...
        {
            gameTick = 1;
            // Date forward. (+1 day)
            Date.Forward();
            Events.Refresh();
        }

        // Execute every tick...
        foreach(Country country in countries.ToArray())
        {
            // Country "think" void.
            country.Think();
        }

        dateDisplay.text = Date.ToString;
        monthDisplay.text = Date.MonthToString(Date.month);
        infoTextDisplay.text = string.Format(infoDefaultText,Abbreviate((long) GetWorldPop()));
    }

    public void CallEventScreen(string name)
    {
        eventPanel.SetActive(true);
        eventTitleDisplay.text = Translation.GetIndex(name + "Title");
        eventTextDisplay.text = Translation.GetIndex(name + "Text");
        eventButtonText.text = Translation.GetIndex(name + "Option");
    }
}

public class FilePaths
{
    public static string 
        language = Application.streamingAssetsPath + "/Language/{0}.ini",
        settings = Application.streamingAssetsPath + "/settings.ini";
}

public class Date
{
    public static int startingDay = Mathf.Clamp(GameManager.instance.startingDay,1,31);
    public static int startingMonth = Mathf.Clamp(GameManager.instance.startingMonth,1,12);
    public static int startingYear = Mathf.Clamp(GameManager.instance.startingYear,1,9999);

    // Remember: 1 day per second at default speed.
    public static byte day = (byte) startingDay;
    public static byte month = (byte) startingMonth;
    public static short year = (short) startingYear;

    public static void Forward()
    {
        Forward(1);
    }
    public static void Forward(byte days)
    {
        day += days;
        Process();
    }

    public static void Process()
    {
        byte maxday = MaxDay(month);
        
        while(month > 12)
        {
            month -= 12;
            year += 1;
        }

        while(day > maxday)
        {
            day -= maxday;
            month += 1;
            maxday = MaxDay(month);
        }
    }

    public static byte MaxDay(byte month)
    {
        byte maxday = 30;
        // Get the max day of the month
        if (month == 1)
        {
            maxday = 31;
        }
        else
        if (month == 2)
        {
            maxday = 29;
        }
        else
        if (month == 3)
        {
            maxday = 31;
        }
        else
        if ((month >= 4) || (month <= 6))
        {
            maxday = 30;
        } else
        if ((month == 7) || (month == 8))
        {
            maxday = 31;
        } else
        if (month == 9)
        {
            maxday = 30;
        } else
        if (month == 10)
        {
            maxday = 31;
        } else
        if (month == 11)
        {
            maxday = 30;
        } else
        if (month == 12)
        {
            maxday = 31;
        }

        return maxday;
    }

    public static string MonthToString(byte month)
    {
        switch (month)
        {
            case 1:
                return Translation.Date.Months.january;
            case 2:
                return Translation.Date.Months.february;
            case 3:
                return Translation.Date.Months.march;
            case 4:
                return Translation.Date.Months.april;
            case 5:
                return Translation.Date.Months.may;
            case 6:
                return Translation.Date.Months.june;
            case 7:
                return Translation.Date.Months.july;
            case 8:
                return Translation.Date.Months.august;
            case 9:
                return Translation.Date.Months.september;
            case 10:
                return Translation.Date.Months.october;
            case 11:
                return Translation.Date.Months.november;
            case 12:
                return Translation.Date.Months.december;
        }

        return "The X month";
    }
    public static new string ToString => $"{day}/{month}/{year}";
}

public class Settings
{
    public static Setting 
        language = new Setting("language", "ENG");

    public static void Load()
    {
        INIParser ini = new INIParser();
        ini.Open(FilePaths.settings);
        language.value = ini.ReadValue("settings", "language", "ENG");
        Debug.Log($"Loaded {language.value} as language");
        ini.Close();
    }
}
public class Setting
{
    public List<Setting> settingList = new List<Setting>();
    public string key, value;
    public Setting(string key)
    {
        this.key = key;
        this.value = "";
    }
    public Setting(string key, string value)
    {
        this.key = key;
        this.value = value;
    }
}