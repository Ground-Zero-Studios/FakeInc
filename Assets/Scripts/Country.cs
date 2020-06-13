using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using System.CodeDom;
using System.Globalization;
using System.Runtime.InteropServices.ComTypes;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]

public class Country : MonoBehaviour
{
    // Variables
    public string shortname;
    public string fullname;
    public uint population;
    public uint belivers;
    public uint disbelivers;
    public float wealth;

    public int countrySize;

    public float startingBirthRate;
    public float startingDeathRate;
    public float startingStability;
    [Range(0,1)]
    public float startingAttractivity;

    public ushort startingAirports;
    public ushort startingSeaports;

    public ushort airports; // max 65.535
    public ushort seaports;


    // Factors
    [HideInInspector]
    public float birthrate = 0f;
    [HideInInspector]
    public float deathrate = 0f;
    [HideInInspector]
    public float stability = 0.5f;
    [HideInInspector]
    public float attractivity = 0f;

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

        airports = startingAirports;
        seaports = startingSeaports;
        stability = startingStability;
    }

    uint GetTotalPop()
    {
        return population + belivers + disbelivers;
    }

    public string PoliticalState()
    {
        if (stability >= 0.8)
        {
            return "Organized";
        } else
        if (stability >= 0.5)
        {
            return "Disorder";
        } else
        if (stability >= 0.2)
        {
            return "Chaos";
        } else
        if (stability >= 0)
        {
            return "Anarchy";
        } else
        {
            return "Destroyed";
        }
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

    public void Think()
    {
        Collider2D[] collidersInRange = Physics2D.OverlapCircleAll(transform.position,6f);

        foreach(Collider2D collider in collidersInRange)
        {
            Country country = collider.transform.GetComponent<Country>();
            if (country == null) { continue; }

            
        }
    }

    void TransferPop(Country to, uint amount)
    {
        population -= amount;
        to.population += amount;
        //TODO: Chance of transferring belivers and disbelivers.
    }

}