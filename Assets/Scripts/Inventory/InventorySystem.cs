using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class InventorySystem : MonoBehaviour
{
 
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

        PopulateSlotLost();
    }

    private void PopulateSlotLost()
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
            isOpen = true;
 
        }
        else if (Input.GetKeyDown(KeyCode.I) && isOpen)
        {
            inventoryScreenUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
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

        Debug.Log(itemToAdd);
        if (itemToAdd != null)
        {
            foreach (GameObject slot in slotList)
            {
                if (slot.transform.childCount == 0)
                {
                    GameObject item = Instantiate(itemToAdd, slot.transform);
                    item.name = itemToAdd.name;
                    itemList.Add(itemToAdd.name);
                    break;
                }
            }
        }
        
        
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
    
 
}