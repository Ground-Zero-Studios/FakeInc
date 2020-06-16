using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]

public class Country : MonoBehaviour
{

    // Variables
    public string shortname;
    public string fullname;

    public int countrySize;

    public uint startingPopulation;
    public uint startingWealth;
    public uint startingWealthGrowth;

    public float startingBirthRate;
    public float startingDeathRate;
    public float startingStability;
    public float startingCorruption;
    [Range(0,1)]
    public float startingTourism;
    public float startingMining;

    public ushort startingAirports;
    public ushort startingSeaports;

    [HideInInspector]
    public ushort airports; // max 65.535
    [HideInInspector]
    public ushort seaports;


    // Factors
    [HideInInspector]
    public float birthrate = 0f;
    [HideInInspector]
    public float deathrate = 0f;
    [HideInInspector]
    public float wealthGrowth = 0.005f;
    [HideInInspector]
    public float mining = 0.2f;
    [HideInInspector]
    public float tourism = 0.2f;
    [HideInInspector]
    public uint wealth = 0;
    [HideInInspector]
    public uint population = 0;
    [HideInInspector]
    public uint belivers = 0;
    [HideInInspector]
    public uint disbelivers = 0;


    // Other
#pragma warning disable CS0108 // O membro oculta o membro herdado; nova palavra-chave ausente
    SpriteRenderer renderer;
#pragma warning restore CS0108 // O membro oculta o membro herdado; nova palavra-chave ausente

    void Start()
    {
        GameManager.countries.Add(this);

        renderer = GetComponent<SpriteRenderer>();
        
        foreach(KeyValuePair<string, string> entry in Translation.entries)
        {
            if (entry.Key.EndsWith("Short", System.StringComparison.InvariantCultureIgnoreCase))
            {
                string keyvalue = entry.Key.ToLower().Replace("short", "");

                if (keyvalue == name.ToLower())
                {
                    // Assign shortname
                    shortname = entry.Value;

                    // Find the fullname

                    foreach(KeyValuePair<string, string> secondentry in Translation.entries)
                    {
                        if (secondentry.Key.EndsWith("Full", System.StringComparison.InvariantCultureIgnoreCase))
                        {
                            string secondvalue = secondentry.Key.ToLower().Replace("full", "");
                            if (secondvalue == keyvalue)
                            {
                                fullname = secondentry.Value;
                            }
                        }
                    }
                }
            }
        }


        if ((shortname == null) || (shortname == ""))
        {
            shortname = name;
        }

        if ((fullname == null) || (fullname == ""))
        {
            fullname = shortname;
        }

        if (wealth <= 0)
        {
            wealth = GameManager.GetRandomMoney(countrySize);
        }

        if (countrySize == 0)
        {
            countrySize = Random.Range(1,101);
        }

        if (population <= 0)
        {
            population = GameManager.GetRandomPop(countrySize);
        }

        if (birthrate == 0)
        {
            uint rand = (uint)Random.Range(0, population/1000000) ;
            birthrate = rand * (100 / (population / 1000000));
        }

        if (deathrate == 0)
        {
            deathrate = Random.Range(0f, 1f);
        }


        airports = startingAirports;
        seaports = startingSeaports;
    }

    uint GetTotalPop()
    {
        return population + belivers + disbelivers;
    }

    void Update()
    {
        if (GameManager.instance.gamePhase == GameManager.Phase.Select)
        {
            if (GameManager.selectedCountry == this)
            {
                renderer.color = Color.red;
            }
            else
            {
                renderer.color = Color.white;
            }
        } else
        if (GameManager.instance.gamePhase == GameManager.Phase.Game)
        {
            renderer.color = new Color(0.9f, 0.9f, 0.9f);

            if (GameManager.selectedCountry == this)
            {
                renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b - 0.5f);
            }
        }
    }

    public static Country Find(string name)
    {
        foreach(Country country in GameManager.countries)
        {
            if (country.shortname == name) { return country; }
            if (country.fullname == name) { return country; }
            if (country.name == name) { return country; }
        }
        return null;
    }

    void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            GameManager.instance.OnCountryClick(this);
        }
        
    }

    public float stability()
    {
        return ((birthrate - deathrate) + wealthGrowth + tourism) / 3;
    }

    public void SendPlane(Country country)
    {
        int flightType = Random.Range(0, 2);
        if (flightType == 1)
        {
            // Commercial / Tourism Flight
            uint value = (uint) (200 * ((country.tourism + country.stability() + (wealth / startingWealth)) / 3));
            country.wealth += value;
            Debug.Log($"Sending a economic flight to {country.shortname}. They just got {value} in wealth");
        }
        else
        {
            // Immigration Flight
            uint value = (uint) 200 * 1; // Temporary
            population -= value;
            country.population += value;
            Debug.Log($"Sending a immigration flight to {country.shortname}.\n{value} population transfered from {shortname} to {country.shortname}");
        }
    }

    public void Think()
    {
        // Variables
        int num = 0;


        // Internal Stuff
        num = Random.Range(1, 10);
        if (num == 1)
        {
            population = population + (uint) (population * (birthrate/10) * (Random.Range(0f,1f)));
        }
        num = Random.Range(1, 10);
        if (num == 1)
        {
            population = population - (uint)(population * (deathrate / 10) * (Random.Range(0f + stability(), 2f - stability())));
        }

        // International Stuff
        Collider2D[] collidersInRange = Physics2D.OverlapCircleAll(transform.position,6f);
        foreach(Collider2D collider in collidersInRange)
        {
            Country country = collider.transform.GetComponent<Country>();
            if (country == null) { continue; }

            num = Random.Range(0, 100);
            if (num == 1)
            {
                // If the country's economy is stable
                if (country.wealth > country.startingWealth/10)
                {
                    #region Flights
                    // Only think about flights if you actually have an airport, duh.
                    if ((airports > 0) && (country.airports > 0))
                    {
                        // If is not having a hard time with airports
                        if (airports > startingAirports / 10)
                        {
                            // 100% chance for a flight to take off
                            SendPlane(country);
                        }
                        else
                        {
                            // Lower chances the lower the airport amounts.
                            float random = Random.value;
                            if (random < (startingAirports / airports))
                            {
                                SendPlane(country);
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    
                }
            }

            
        }
    }

}

