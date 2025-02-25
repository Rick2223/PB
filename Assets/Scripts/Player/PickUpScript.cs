using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PickUpScript : MonoBehaviour
{
    public GameObject player;
    public Transform holdPos;
    //if you copy from below this point, you are legally required to like the video
    public float throwForce = 500f; //force at which the object is thrown at
    public float pickUpRange = 3f; //how far the player can pickup the object from
    public GameObject heldObj; //object which we pick up
    public Rigidbody heldObjRb; //rigidbody of object we pick up
    private bool canDrop = true; //this is needed so we don't throw/drop object when rotating the object
    private int LayerNumber; //layer index
    public bool holding;
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
        if (Input.GetKeyDown(KeyCode.E)) //change E to whichever key you want to press to pick up
        {
            if (heldObj == null) //if currently not holding anything
            {
                holding = true;
                //perform raycast to check if player is looking at object within pickuprange
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickUpRange))
                {
                    //make sure pickup tag is attached
                    if (hit.transform.gameObject.tag == "canPickUp")
                    {
                        //pass in object hit into the PickUpObject function
                        PickUpObject(hit.transform.gameObject);
                    }
                }
            }
            else
            {
                holding = false;
                if(canDrop == true)
                {
                    StopClipping(); //prevents object from clipping through walls
                    DropObject();
                }
            }
        }
        
        if (Input.GetKeyDown(KeyCode.N) && heldObj != null) // Check if player is holding the item
        {
            // Get the PickUpScript directly from the held object
            PickUpScript itemScript = heldObj.GetComponent<PickUpScript>();

            if (itemScript != null) // Ensure the held item has a PickUpScript
            {
                if (InventorySystem.Instance.isFull)
                {
                    Debug.Log("Inventory is full");
                }
                else
                {
                    string pickedItemName = itemScript.ItemName; // Get the item name
                    Debug.Log("Adding item to inventory: " + pickedItemName);

                    InventorySystem.Instance.AddToInventory(pickedItemName);
                    Destroy(heldObj); // Remove the item from the world
                    heldObj = null; // Clear the held object since it's picked up
                }
            }
            else
            {
                Debug.LogError("Held object has no PickUpScript attached! Object name: " + heldObj.name);
            }
        }

        if (heldObj != null) //if player is holding object
        {
           
            MoveObject(); //keep object position at holdPos
            if (Input.GetKeyDown(KeyCode.Mouse0) && canDrop == true) //Mous0 (leftclick) is used to throw, change this if you want another button to be used)
            {
                StopClipping();
                ThrowObject();
            }
  

        }
    }
    void PickUpObject(GameObject pickUpObj)
    {
        if (pickUpObj.GetComponent<Rigidbody>()) //make sure the object has a RigidBody
        {
            heldObj = pickUpObj; //assign heldObj to the object that was hit by the raycast (no longer == null)
            heldObjRb = pickUpObj.GetComponent<Rigidbody>(); //assign Rigidbody
            heldObjRb.isKinematic = true;
            heldObjRb.transform.parent = holdPos.transform; //parent object to holdposition
            heldObj.layer = LayerNumber; //change the object layer to the holdLayer
            //make sure object doesnt collide with player, it can cause weird bugs
            Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), true);
        }
    }
    void DropObject()
    {
        if (heldObj == null)
        {
            Debug.LogError("❌ DropObject called but heldObj is null.");
            return;
        }

        // Ensure the item has the required components
        InteractableObject interactable = heldObj.GetComponent<InteractableObject>();
        if (interactable == null)
        {
            Debug.LogError("❌ Dropped item is missing InteractableObject component.");
            return;
        }

        // Get item name to remove from inventory
        string itemName = interactable.GetItemName();
        if (string.IsNullOrEmpty(itemName))
        {
            Debug.LogError("❌ Item name is null or empty.");
            return;
        }

        // Enable physics and unparent the object
        Rigidbody rb = heldObj.GetComponent<Rigidbody>();
        Collider col = heldObj.GetComponent<Collider>();

        if (rb != null) rb.isKinematic = false;
        if (col != null) col.enabled = true;

        heldObj.transform.SetParent(null);
        heldObj.transform.position = transform.position + transform.forward;

        // Remove from Inventory
        InventorySystem.Instance.RemoveFromInventory(itemName);
        EquipSystem.Instance.RemoveFromQuickSlots(itemName); // Remove from quick slots if equipped
        InventorySystem.Instance.ReCalculateList(); // Ensure the list stays synced

        // Ensure it's pickable again
        interactable.ItemName = itemName;
        heldObj.tag = "canPickUp";

        // Clear the held object reference
        heldObj = null;

        Debug.Log("✅ Dropped and removed item from inventory: " + itemName);
    }


    void MoveObject()
    {
        //keep object position the same as the holdPosition position
        heldObj.transform.position = holdPos.transform.position;
    }
    
    void ThrowObject()
    {
        //same as drop function, but add force to object before undefining it
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        heldObj.layer = 0;
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null;
        heldObjRb.AddForce(transform.forward * throwForce);
        heldObj = null;
        holding = false;
    }
    void StopClipping() //function only called when dropping/throwing
    {
        var clipRange = Vector3.Distance(heldObj.transform.position, transform.position); //distance from holdPos to the camera
        //have to use RaycastAll as object blocks raycast in center screen
        //RaycastAll returns array of all colliders hit within the cliprange
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), clipRange);
        //if the array length is greater than 1, meaning it has hit more than just the object we are carrying
        if (hits.Length > 1)
        {
            //change object position to camera position 
            heldObj.transform.position = transform.position + new Vector3(0f, -0.5f, 0f); //offset slightly downward to stop object dropping above player 
            //if your player is small, change the -0.5f to a smaller number (in magnitude) ie: -0.1f
        }
    }
}
