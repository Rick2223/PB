using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class DoorController : MonoBehaviour
{
    private Animator animator;
    public bool isOpen = false;
    public int doorID = -1;

    [Header("Door Settings")]
    [SerializeField] private bool requiresKey = false; // Does this door need a key?
    [SerializeField] public bool requiresCode = false; 
    [SerializeField] private string requiredKeyName = "CellKey"; // Key name

    [Header("Animation Settings")]
    [SerializeField] private string openAnimationName = "OpenDoor"; // Animation for opening
    [SerializeField] private string closeAnimationName = "CloseDoor"; // Animation for closing


    public TMP_Text feedbackText; // Assign your TMP text element here
    public static DoorController Instance { get; set; }

    
    
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
            ShowFeedback("This is locked. You need a " + requiredKeyName + "to open");
            return;
        }
        else if (requiresCode)
        {
            Debug.Log(gameObject.name + " is locked. You need a code.");
            ShowFeedback("This door is locked. You need a code.");
            return;
        }

        if (isOpen)
        {
            animator.Play(closeAnimationName);
            AudioManager.Instance.PlayEffect("GateOpen");
        }
        else
        {
            animator.Play(openAnimationName);
            AudioManager.Instance.PlayEffect("GateOpen");
        }
        isOpen = !isOpen;
    }

    public void ForceOpen()
    {
        animator.Play(openAnimationName);
        isOpen = true;
    }

    private void ShowFeedback(string message)
    {
        if (feedbackText != null)
        {
            feedbackText.text = message;
            feedbackText.ForceMeshUpdate(); // Ensure TMP updates immediately

            CancelInvoke(nameof(ClearFeedback));
            Invoke(nameof(ClearFeedback), 2f); // Clear after 2 seconds
        }
    }

    private void ClearFeedback()
    {
        if (feedbackText != null)
        {
            feedbackText.text = "";
        }
    }
}
