using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using GroundZero.Managers;
using GroundZero.Assets;
using GroundZero;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]

public class Country : MonoBehaviour
{

    // Variables
    public string shortname;
    public string fullname;

    public int countrySize;

    public Dictionary<string, uint> resources = new Dictionary<string, uint>();

    // Other
#pragma warning disable CS0108 // O membro oculta o membro herdado; nova palavra-chave ausente
    SpriteRenderer renderer;
#pragma warning restore CS0108 // O membro oculta o membro herdado; nova palavra-chave ausente

    void Start()
    {
        Game.countries.Add(this);

        renderer = GetComponent<SpriteRenderer>();

        if ((shortname == null) || (shortname == ""))
        {
            shortname = Localization.Get(char.ToLower(name[0]) + name.Substring(1) + "Short");
            if ((shortname == null) || (shortname == ""))
            {
                shortname = name;
            }
        }

        if ((fullname == null) || (fullname == ""))
        {
            fullname = Localization.Get(char.ToLower(name[0]) + name.Substring(1) + "Full");
            if ((fullname == null) || (fullname == ""))
            {
                fullname = shortname;
            }
        }

        if (countrySize == 0)
        {
            countrySize = Random.Range(1,101);
        }

        Effect.scopes.Add(name);
        Condition.scopes.Add(name);

    }

    public void AddResource(string resource, uint amount)
    {
        if (resources.ContainsKey(resource))
        {
            resources[resource] += amount;
        }
        else
        {
            resources.Add(resource, amount);
        }
    }

    public void SubtractResource(string resource, uint amount)
    {
        if (resources.ContainsKey(resource))
        {
            resources[resource] -= amount;
        }
        else
        {
            resources.Add(resource, 0);
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            int k = 0;
            while (k < 500)
            {
                Think();
                k += 1;
            }
        }
        if (Game.instance.gamePhase == Game.Phase.Select)
        {
            if (Game.selectedCountry == this)
            {
                renderer.color = Color.red;
            }
            else
            {
                renderer.color = Color.white;
            }
        } else
        if (Game.instance.gamePhase == Game.Phase.Game)
        {
            renderer.color = new Color(0.9f, 0.9f, 0.9f);

            if (Game.selectedCountry == this)
            {
                renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b - 0.5f);
            }
        }
    }

    public static Country Find(string name)
    {
        foreach(Country country in Game.countries)
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
            Game.instance.OnCountryClick(this);
            Game.player.OnCountryClick(this);
        }
        
    }

    public void Think()
    {
        // TODO: Manage resources
    }

}

