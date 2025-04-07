using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField] private AudioClip doorsound; //sound played when picking up the object

    private AudioSource audioSource; //audio source to play the sound
    public static DoorController Instance { get; set; }

    
    
    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("No Animator component found on " + gameObject.name);
        }
        audioSource = GetComponent<AudioSource>();
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
            audioSource.clip = doorsound; //sets the audio clip to the one we set in the inspector
            audioSource.Play(); //plays the sound
        }
        else
        {
            animator.Play(openAnimationName);
            audioSource.clip = doorsound; //sets the audio clip to the one we set in the inspector
            audioSource.Play(); //plays the sound
        }
        isOpen = !isOpen;
    }

    public void ForceOpen()
    {
        animator.Play(openAnimationName);
        isOpen = true;
    }
}
