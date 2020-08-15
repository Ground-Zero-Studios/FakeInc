using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using GroundZero.Assets;
using System.IO;
using System.Security.Cryptography;

namespace GroundZero.Managers
{
    public class Game : MonoBehaviour
    {
        public static Game instance;

        // Game related stuff
        public enum Phase { Create, Select, Game, End }
        public Phase gamePhase = Phase.Create;

        public static Player player;

        public static List<Country> countries = new List<Country>();

        public static Country selectedCountry = null;

        [Range(0f, 10f)]
        public float gameSpeed = 1f;

        [Range(1, 31)]
        public int startingDay;
        [Range(1, 12)]
        public int startingMonth;
        [Range(1, 9999)]
        public int startingYear;

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
        public TMP_Dropdown         newsTypeDropdown;

        private GameObject          newsAttributesArea;
        private TextMeshProUGUI     newsAttributesText;
        private string              newsAttributesDefaultText;


        private float updateTimer = 0f;
        private int gameTick = 0;

        private bool handlingEvents = false;

        public struct FilePaths
        {
            public static string
                language = Application.streamingAssetsPath + "/Language/{0}.ini",
                settings = Application.streamingAssetsPath + "/settings.ini",
                mods = Application.streamingAssetsPath + "/Mods";
        }

        void Start()
        {
            instance = this;
            Settings.Load();
            Mods.Load(FilePaths.mods);


            List<string> localizationPaths = new List<string>();

            localizationPaths.Add(FilePaths.language);

            foreach(Mod mod in Mods.list)
            {
                localizationPaths.Add(FilePaths.mods + "/" + mod.Identifier + "/Language/{0}.ini");
                
            }

            foreach(string path in localizationPaths)
            {
                Localization.Load(string.Format(path, Settings.GetString("language")));
            }


            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

            confirmSelectionTextDisplay = confirmSelectionPanel.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            confirmSelectionDefaultText = Localization.Get("confirmSelectionText");
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
            creationPanelDefaultTitle = Localization.Get("creationPanelTitle");
            creationPanelText = creationPanel.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            creationPanelDefaultText = Localization.Get("creationPanelText");
            creationPanelStartButton = creationPanel.transform.Find("StartButton").GetComponent<Button>();
            
            // News Info Area
            newsInfoArea = creationPanel.transform.Find("NewsInfoArea").gameObject;
            newsInfoText = newsInfoArea.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            newsInfoDefaultText = Localization.Get("newsInfoDefaultText");

            newsNameInput = newsInfoArea.transform.Find("NewsNameInput").GetComponent<TMP_InputField>();
            newsNameInput.transform.Find("TextArea").Find("Placeholder").GetComponent<TextMeshProUGUI>().text = Localization.Get("newsFieldName");
            newsNameInputText = newsNameInput.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            newsNameInputDefaultText = Localization.Get("newsNameInputText");

            newsAdjectiveInput = newsInfoArea.transform.Find("NewsAdjectiveInput").GetComponent<TMP_InputField>();
            newsAdjectiveInput.transform.Find("TextArea").Find("Placeholder").GetComponent<TextMeshProUGUI>().text = Localization.Get("newsFieldAdjective");
            newsAdjectiveInputText = newsAdjectiveInput.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            newsAdjectiveInputDefaultText = Localization.Get("newsAdjectiveInputText");

            newsBeliverInput = newsInfoArea.transform.Find("NewsBeliverInput").GetComponent<TMP_InputField>();
            newsBeliverInput.transform.Find("TextArea").Find("Placeholder").GetComponent<TextMeshProUGUI>().text = Localization.Get("newsFieldBeliver");
            newsBeliverInputText = newsBeliverInput.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            newsBeliverInputDefaultText = Localization.Get("newsBeliverInputText");

            newsDisbeliverInput = newsInfoArea.transform.Find("NewsDisbeliverInput").GetComponent<TMP_InputField>();
            newsDisbeliverInput.transform.Find("TextArea").Find("Placeholder").GetComponent<TextMeshProUGUI>().text = Localization.Get("newsFieldDisbeliver");
            newsDisbeliverInputText = newsDisbeliverInput.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            newsDisbeliverInputDefaultText = Localization.Get("newsDisbeliverInputText");

            // News Type Area
            newsTypeArea = creationPanel.transform.Find("NewsTypeArea").gameObject;
            newsTypeText = newsTypeArea.transform.Find("NewsTypeText").GetComponent<TextMeshProUGUI>();
            newsTypeDropdown = newsTypeArea.transform.Find("NewsTypeDropdown").GetComponent<TMP_Dropdown>();
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
                }
                else
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

        public void OnCountryClick(Country country)
        {
            // If there's an open menu, we dont want to register a click.
            // We also only want to handle the selection phase here.
            if ((isUIOpen()) || (gamePhase != Phase.Select))
            {
                return;
            }

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
            
        }

        public void OnButtonClick(string button)
        {
            if (button.Equals("Play", System.StringComparison.InvariantCultureIgnoreCase))
            {
                if (creationPanel.activeSelf)
                {
                    if (canStartGame())
                    {
                        creationPanel.SetActive(false);
                        player.newsName = newsNameInput.text;
                        player.newsAdj = newsAdjectiveInput.text;
                        player.newsBAdj = newsBeliverInput.text;
                        player.newsDAdj = newsDisbeliverInput.text;
                        player.newsType = (Player.NewsType) newsTypeDropdown.value;
                        gamePhase = Phase.Select;
                    }
                }
            }
        }

        public bool canStartGame()
        {
            if (newsNameInput.text == "") { return false; }
            if (newsAdjectiveInput.text == "") { return false; }
            if (newsBeliverInput.text == "") { return false; }
            if (newsDisbeliverInput.text == "") { return false; }

            return true;
        }

        public bool isUIOpen()
        {
            if (confirmSelectionPanel.activeSelf) { return true; }
            if (eventPanel.activeSelf) { return true; }
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
            }
            else
            if (phase.Equals("Game", System.StringComparison.InvariantCultureIgnoreCase))
            {
                gamePhase = Phase.Game;
            }
            else
            if (phase.Equals("End", System.StringComparison.InvariantCultureIgnoreCase))
            {
                gamePhase = Phase.End;
            }
        }

        public static Country GetRandomCountry()
        {
            Country[] countryArray = countries.ToArray();
            return countryArray[Random.Range(0, countryArray.Length - 1)];
        }


        public void GameTick() // 1 day per second, 10 ticks per second = 10 ticks per day
        {

            if (gamePhase == Phase.Game)
            {
                if (!Mods.handlingEvents)
                {
                    Mods.HandleEvents();
                }
            }


            if (Game.instance.isUIOpen()) { return; } // Dont proceed if player is busy with UI;
            gameTick += 1;

            if (gameTick > 10) // Once every 10 ticks...
            {
                gameTick = 1;
                // Date forward. (+1 day)
                Date.Forward();
            }

            // Execute every tick...
            foreach (Country country in countries.ToArray())
            {
                // Country "think" void.
                country.Think();
            }

            dateDisplay.text = Date.ToString;
            monthDisplay.text = Date.MonthToString(Date.month);
            infoTextDisplay.text = string.Format(infoDefaultText, 0);
        }

        public void CallEventScreen(string name)
        {
            eventPanel.SetActive(true);
            eventTitleDisplay.text = Localization.Get(name + "Title");
            eventTextDisplay.text = Localization.Get(name + "Text");
            eventButtonText.text = Localization.Get(name + "Option");
        }
    }

    public static class Date
    {
        public static int startingDay = Mathf.Clamp(Game.instance.startingDay, 1, 31);
        public static int startingMonth = Mathf.Clamp(Game.instance.startingMonth, 1, 12);
        public static int startingYear = Mathf.Clamp(Game.instance.startingYear, 1, 9999);

        // Remember: 1 day per second at default speed.
        public static byte day = (byte)startingDay;
        public static byte month = (byte)startingMonth;
        public static short year = (short)startingYear;

        public static int elapsedDays, elapsedMonths, elapsedYears = 0;

        public static void Forward()
        {
            Forward(1);
        }
        public static void Forward(byte days)
        {
            day += days;
            elapsedDays++;
            Process();
        }

        public static void Process()
        {
            byte maxday = MaxDay(month);

            while (month > 12)
            {
                month -= 12;
                year += 1;
                elapsedYears++;
            }

            while (day > maxday)
            {
                day -= maxday;
                month += 1;
                elapsedMonths++;
                maxday = MaxDay(month);
            }
        }

        public static byte MaxDay(byte month)
        {
            byte maxday = 30;
            // Get the max day of the month

            switch (month)
            {
                case 1:
                case 3:
                case 7:
                case 8:
                case 10:
                case 12:
                    maxday = 31;
                    break;
                case 2:
                    maxday = 29;
                    break;
                case 4:
                case 5:
                case 6:
                case 9:
                case 11:
                    maxday = 30;
                    break;
            }

            return maxday;
        }

        public static string MonthToString(byte month)
        {
            switch (month)
            {
                case 1:
                    return Localization.Get("january");
                case 2:
                    return Localization.Get("february");
                case 3:
                    return Localization.Get("march");
                case 4:
                    return Localization.Get("april");
                case 5:
                    return Localization.Get("may");
                case 6:
                    return Localization.Get("june");
                case 7:
                    return Localization.Get("july");
                case 8:
                    return Localization.Get("august");
                case 9:
                    return Localization.Get("september");
                case 10:
                    return Localization.Get("october");
                case 11:
                    return Localization.Get("november");
                case 12:
                    return Localization.Get("december");
            }

            return "The X month";
        }
        public static new string ToString => $"{day}/{month}/{year}";
    }
}