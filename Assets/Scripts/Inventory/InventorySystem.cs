using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;	
 
public class InventorySystem : MonoBehaviour
{
 
   
   public GameObject ItemInfoUI;
   public static InventorySystem Instance { get; set; }
 
    public GameObject inventoryScreenUI;

    public List<GameObject> slotList = new List<GameObject>();

    public List<string> itemList = new List<string>();

    private GameObject itemToAdd;

    private GameObject whatSlotToEquip;

    public bool isOpen;

    public bool isFull;

 
 
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
 
 
    void Start()
    {
        isOpen = false;
        isFull = false;

        PopulateSlotList();

        Cursor.visible = false;
    }

    private void PopulateSlotList()
    {
        foreach (Transform child in inventoryScreenUI.transform)
        {
            if (child.CompareTag("Slot"))
            {
                slotList.Add(child.gameObject);
            }
        }
    }
 
 
    void Update()
    {
 
        if (Input.GetKeyDown(KeyCode.I) && !isOpen)
        {
 
		      	Debug.Log("i is pressed");
            inventoryScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            isOpen = true;
        }
        else if (Input.GetKeyDown(KeyCode.I) && isOpen)
        {
            inventoryScreenUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            isOpen = false;
        }
    }


    public void AddToInventory(string itemName)
    {
       
        whatSlotToEquip = FindNextEmptySlot();
        GameObject loadedItem = Resources.Load<GameObject>(itemName);
        if (loadedItem == null)
        {
            Debug.LogError("❌ Error: Could not find item prefab in Resources folder: " + itemName);
            return;
        }

        itemToAdd = Instantiate(loadedItem, whatSlotToEquip.transform);
        itemToAdd.name = itemName; // Set the name correctly
        itemToAdd.transform.SetParent(whatSlotToEquip.transform);
        itemList.Add(itemToAdd.name);

        Debug.Log(itemToAdd);
        
        
    }

    private GameObject FindNextEmptySlot()
    {
        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }

        }
        return null;
    }

    public bool CheckIfFull()
    {
        int counter = 0;

        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount > 0)
            {
                counter += 1;
            }
        }

        if (counter == slotList.Count)
        {
            return true;
            isFull = true;
        }
        else
        {
            return false;
            isFull = false;
        }
    }

    public void ReCalculateList()
    {
        itemList.Clear();
        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount > 0)
            {
                string name = slot.transform.GetChild(0).name;
                string str2 = "(Clone)";
                string result = name.Replace(str2, "");
                itemList.Add(result);
            }
        }
    }

    public void RemoveFromInventory(string itemName)
    {
        // Find the item slot containing the item to remove
        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount > 0)
            {
                GameObject itemInSlot = slot.transform.GetChild(0).gameObject;
                if (itemInSlot.name == itemName || itemInSlot.name == itemName + "(Clone)")
                {
                    Destroy(itemInSlot); // Remove item visually from the slot
                    itemList.Remove(itemName); // Remove from itemList
                    ReCalculateList(); // Ensure the list stays synced
                    Debug.Log(itemName + " removed from inventory.");
                    return;
                }
            }
        }
        Debug.LogWarning("Tried to remove an item that wasn't in the inventory: " + itemName);
    }
    
 }