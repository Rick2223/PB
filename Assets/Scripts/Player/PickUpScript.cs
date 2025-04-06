using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PickUpScript : MonoBehaviour
{
    
    //if you copy from below this point, you are legally required to like the video
    public float pickUpRange = 3f; //how far the player can pickup the object from
    private int LayerNumber; //layer index
    public string ItemName;

    

    public string GetItemName()
    {
        return ItemName;
    }

    //Reference to script which includes mouse movement of player (looking around)
    //we want to disable the player looking around when rotating the object
    //example below 
    //MouseLookScript mouseLookScript;
    void Start()
    {
        LayerNumber = LayerMask.NameToLayer("holdLayer"); //if your holdLayer is named differently make sure to change this ""
        if (string.IsNullOrEmpty(ItemName))
        {
            ItemName = gameObject.name;
            Debug.Log("ItemName was empty, setting it to " + ItemName);

        }

        //mouseLookScript = player.GetComponent<MouseLookScript>();
    }
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetKeyDown(KeyCode.E) && Physics.Raycast(ray, out hit, pickUpRange))
        {
            PickUpScript pickUpScript = hit.collider.GetComponent<PickUpScript>();

            if (pickUpScript != null && hit.collider.CompareTag("canPickUp"))
            {
                if (InventorySystem.Instance.isFull)
                {
                    Debug.Log("Inventory is full");
                }
                else
                {
                    InventorySystem.Instance.AddToInventory(pickUpScript.GetItemName());
                    Debug.Log("Adding item to inventory: " + pickUpScript.GetItemName());
                    hit.collider.gameObject.SetActive(false);
                    InventorySystem.Instance.itemsPickedup.Add(gameObject.name);
                    Destroy(hit.collider.gameObject); // Destroys the *specific* object hit by the ray
                    
                }
            }
        }
    }

    
}
