using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public float interactionDistance = 3f;
    private Camera playerCamera;
    private InventorySystem inventory; // Make sure this is set

    void Start()
    {
        playerCamera = Camera.main;
        inventory = InventorySystem.Instance; // Automatically assign the inventory system
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance))
            {
                DoorController door = hit.collider.GetComponent<DoorController>();
                if (door != null)
                {
                    string equippedItem = inventory.GetEquippedItemName(); // Get the equipped item name
                    door.ToggleDoor(equippedItem);
                }
            }
        }
    }
}
