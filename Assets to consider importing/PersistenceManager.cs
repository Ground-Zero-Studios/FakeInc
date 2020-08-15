using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Graypoint.OdinSerializer;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PersistenceManager : MonoBehaviour
{
    [HideInInspector]
    public PersistentData gameState = new PersistentData();
    private GameManager m_manager;

    public void SaveState(string filePath)
    {
        gameState.position = m_manager.controller.transform.position;
        gameState.favors = m_manager.offerPoints;
        gameState.patronGod = m_manager.currentGod;
        gameState.health = m_manager.playerStats.health;
        gameState.currentScene = m_manager.currentSceneName;
        if(m_manager.Amanager.specialAbilityObject != null)
            gameState.specialAbility = m_manager.Amanager.specialAbilityObject.AbilityName;
        SaveInventory();
        
        byte[] bytes = SerializationUtility.SerializeValue(gameState, DataFormat.Binary);
        File.WriteAllBytes(filePath, bytes);
        Debug.Log(filePath);
    }

    public void LoadState(string filePath) // this'll be called from per scene
    {
        if (!File.Exists(filePath)) return; // No state to load
        
        byte[] bytes = File.ReadAllBytes(filePath);
        gameState = SerializationUtility.DeserializeValue<PersistentData>(bytes, DataFormat.Binary);
        if(m_manager == null)
        {
            m_manager = GameManager.Instance;
            Debug.Log("manager is null");
        }
        if(gameState == null)
            Debug.Log("state is null");

        m_manager.offerPoints = gameState.favors;
        m_manager.currentGod = gameState.patronGod;
        m_manager.playerStats.health = gameState.health;
        m_manager.Amanager.specialAbilityObject = m_manager.abilityDatabase.getAbilityObject(gameState.specialAbility);
        //Debug.Log(m_manager.Amanager.specialAbilityObject.AbilityName);
        //m_manager.Amanager.AddAbility();
        //m_manager.Amanager.UpdateSpecialAbility();
        m_manager.UpdateGods();
        LoadInventory();
    }

    private void SaveInventory()
    {
        for(int i = 0; i < m_manager.InventorySystem.items.Length; i++)
        {
            SerializableSlot slot = new SerializableSlot();
            if(m_manager.InventorySystem.items[i]._Item != null)
            {
                slot.itemID = m_manager.InventorySystem.items[i]._Item.ItemID;
                slot.amount = m_manager.InventorySystem.items[i].amount;
                gameState.inventory[i] = slot;
            }
            else
            {
                break;
            }
        }
        Debug.Log(Application.persistentDataPath);
            
        for(int y = 0; y < m_manager.InventorySystem.equipment.Length; y++)
        {
            SerializableSlot slot = new SerializableSlot();
            if(m_manager.InventorySystem.equipment[y]._Item != null)
            {
                slot.itemID = m_manager.InventorySystem.equipment[y]._Item.ItemID;
                slot.amount = m_manager.InventorySystem.equipment[y].amount;
                //Debug.Log(y);
                gameState.armor[y] = slot;
            }
        }
    }
    private void LoadInventory()
    {
        for(int i = 0; i < gameState.inventory.Length; i++)
        {
            if(gameState.inventory[i] == null)
                break;
            m_manager.InventorySystem.items[i]._Item = m_manager.itemDatabase.getItem(gameState.inventory[i].itemID);
            m_manager.InventorySystem.items[i].amount = gameState.inventory[i].amount;
            m_manager.InventorySystem.items[i].image.SetActive(true);
            m_manager.InventorySystem.items[i].image.GetComponent<Image>().sprite = m_manager.itemDatabase.getItem(gameState.inventory[i].itemID).ItemImage;
        }
        for(int y = 0; y < gameState.armor.Length; y++)
        {
            if(gameState.armor[y] != null || (gameState.armor[y] != null && gameState.armor[y].itemID != -1))
            {
                m_manager.InventorySystem.equipment[y]._Item = m_manager.itemDatabase.getItem(gameState.armor[y].itemID);
                m_manager.InventorySystem.equipment[y].amount = gameState.armor[y].amount;
                m_manager.InventorySystem.equipment[y].currentItem = m_manager.InventorySystem.equipment[y]._Item as Equipment;
                m_manager.Emanager.UpdateStats();
               // m_manager.UImanager.UpdateWeapon();
                m_manager.InventorySystem.equipment[y].UpdateImage();
            }
        }
    }

    public void LoadSaveState(string path) // this'll be called from the main menu
    {
        LoadState(path);
        m_manager.LoadSceneAdditive(gameState.currentScene);
       // m_manager.controller.transform.position = gameState.position;
    }

    private void Awake() {
        m_manager = GameManager.Instance;
    }
}

public class PersistentData
{
    public int favors;
    public GameManager.god patronGod; // reference to the enumerator integer
    public float health;
    public string currentScene; // not supported yet
    public Vector3 position;
    public string specialAbility;
    public SerializableSlot[] inventory = new SerializableSlot[8];
    public SerializableSlot[] armor = new SerializableSlot[4];
}

public class SerializableSlot
{  
    public int itemID = -1;
    public int amount = -1;
}
