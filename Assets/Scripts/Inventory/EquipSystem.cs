using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipSystem : MonoBehaviour
{
    public static EquipSystem Instance { get; set; }

    // -- UI -- //
    public GameObject quickSlotsPanel;

    public List<GameObject> quickSlotsList = new List<GameObject>();
 	

    public GameObject numbersHolder;

    public int selectedNumber = -1;
    public GameObject selectedItem;
    public GameObject holdPosition;
    public GameObject selecteditemModel;

   
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


    private void Start()
    {
        PopulateSlotList();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectQuickSlot(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectQuickSlot(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectQuickSlot(3);
        }
    }

    void SelectQuickSlot(int number)
    {
        if (checkIfSlotIsFull(number) == true)
        {
            
            if (selectedNumber != number)
            {
                selectedNumber = number;
                //Unselecting previousy selected item
                if (selectedItem != null)
                {
                    selectedItem.gameObject.GetComponent<InventoryItem>().isSelected = false;
                }
                
                selectedItem = GetSelectedItem(number);
                selectedItem.GetComponent<InventoryItem>().isSelected = true;

                SetEquippedModel(selectedItem);
                
                //Changing color of slotnumbers
                foreach (Transform child in numbersHolder.transform)
                {
                    child.transform.Find("Text").GetComponent<Text>().color = Color.gray;
                }
                Text toBeChanged = numbersHolder.transform.Find("number" + number).transform.Find("Text").GetComponent<Text>();
                toBeChanged.color = Color.white;
            }
            else //Trying to select the selected slot
            {
                selectedNumber = -1;
                //Unselecting previousy selected item
                if (selectedItem != null)
                {
                    selectedItem.gameObject.GetComponent<InventoryItem>().isSelected = false;
                    selectedItem = null;
                }

                if(selecteditemModel != null)
                {
                    DestroyImmediate(selecteditemModel.gameObject);
                    selecteditemModel = null;
                }

                //Changing color of slotnumbers
                foreach (Transform child in numbersHolder.transform)
                {
                    child.transform.Find("Text").GetComponent<Text>().color = Color.gray;
                }
            }
        }
    }

    

    private void SetEquippedModel(GameObject selectedItem)
    {
        if (selecteditemModel != null)
        {
            DestroyImmediate(selecteditemModel.gameObject);
            selecteditemModel = null;
        }
        
        string selectedItemName = selectedItem.name.Replace("(Clone)", "");
        selecteditemModel = Instantiate(Resources.Load<GameObject>(selectedItemName + "_Model"), new Vector3(1f, 0f, 0), Quaternion.Euler(0, -12.5f, -20f));
        selecteditemModel.transform.SetParent(holdPosition.transform, false);

        // Ensure the item has the correct tag for re-picking after being dropped
        selecteditemModel.tag = "canPickUp";

        // Copy PickUpScript and its data
        if (!selecteditemModel.GetComponent<PickUpScript>())
        {
            PickUpScript originalPickUpScript = selectedItem.GetComponent<PickUpScript>();
            PickUpScript newPickUpScript = selecteditemModel.AddComponent<PickUpScript>();

            // Copy relevant properties
            newPickUpScript.ItemName = originalPickUpScript?.ItemName ?? selectedItemName;
            newPickUpScript.player = originalPickUpScript?.player;
            newPickUpScript.holdPos = originalPickUpScript?.holdPos;
            newPickUpScript.throwForce = originalPickUpScript?.throwForce ?? 500f;
            newPickUpScript.pickUpRange = originalPickUpScript?.pickUpRange ?? 3f;
        }

        // Ensure Rigidbody is present
        if (!selecteditemModel.GetComponent<Rigidbody>())
        {
            Rigidbody rb = selecteditemModel.AddComponent<Rigidbody>();
            rb.isKinematic = true; // so it doesn't fall through the world when equipped
        }

        // Ensure Collider is present
        if (!selecteditemModel.GetComponent<Collider>())
        {
            Collider originalCollider = selectedItem.GetComponent<Collider>();
            if (originalCollider != null)
            {
                Collider newCollider = selecteditemModel.AddComponent(originalCollider.GetType()) as Collider;
                newCollider.isTrigger = originalCollider.isTrigger;
            }
        }

        // Ensure InteractableObject script is copied over
        if (selectedItem.GetComponent<InteractableObject>() && !selecteditemModel.GetComponent<InteractableObject>())
        {
            InteractableObject originalInteractableObject = selectedItem.GetComponent<InteractableObject>();
            InteractableObject newInteractableObject = selecteditemModel.AddComponent<InteractableObject>();

            // Copy any necessary properties from InteractableObject
            // Adjust this to fit your InteractableObject fields!
        }
    }

    
    
    GameObject GetSelectedItem(int slotNumber)
    {
        return quickSlotsList[slotNumber - 1].transform.GetChild(0).gameObject;
    }

    bool checkIfSlotIsFull(int slotNumber)
    {
        if (quickSlotsList[slotNumber - 1].transform.childCount > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    private void PopulateSlotList()
    {
        foreach (Transform child in quickSlotsPanel.transform)
        {
            if (child.CompareTag("QuickSlot"))
            {
                quickSlotsList.Add(child.gameObject);
            }
        }
    }

    public void AddToQuickSlots(GameObject itemToEquip)
    {
        // Find next free slot
        GameObject availableSlot = FindNextEmptySlot();
        // Set transform of our object
        itemToEquip.transform.SetParent(availableSlot.transform, false);

        InventorySystem.Instance.ReCalculateList();

    }


    private GameObject FindNextEmptySlot()
    {
        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return new GameObject();
    }

    public bool CheckIfFull()
    {

        int counter = 0;

        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount > 0)
            {
                counter += 1;
            }
        }

        if (counter == 3)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}