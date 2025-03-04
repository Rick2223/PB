using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Animator animator;
    private bool isOpen = false;

    [Header("Door Settings")]
    [SerializeField] private bool requiresKey = false; // Does this door need a key?
    [SerializeField] private bool requiresCode = false; 
    [SerializeField] private string requiredKeyName = "CellKey"; // Key name

    [Header("Animation Settings")]
    [SerializeField] private string openAnimationName = "OpenDoor"; // Animation for opening
    [SerializeField] private string closeAnimationName = "CloseDoor"; // Animation for closing

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("No Animator component found on " + gameObject.name);
        }
    }

    public void SetCodeFalse()
    {
        requiresCode = false;
    }
    
    public void ToggleDoor(string equippedItem)
    {
        if (requiresKey && EquipSystem.Instance.selectedItem.name != requiredKeyName)
        {
            Debug.Log(gameObject.name + " is locked. You need the " + requiredKeyName + ".");
            return;
        }
        else if (requiresCode)
        {
            Debug.Log(gameObject.name + " is locked. You need a code.");
            return;
        }

        if (isOpen)
        {
            animator.Play(closeAnimationName);
        }
        else
        {
            animator.Play(openAnimationName);
        }
        isOpen = !isOpen;
    }
}
