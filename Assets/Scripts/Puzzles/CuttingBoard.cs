using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class CuttingBoard : MonoBehaviour
{
    [Header("Ingredients")]
    public List<string> requiredIngredients; // Add "Bread", "Apple", "Cheese"
    private List<string> placedIngredients = new List<string>();

    [Header("Ingredient Models")]
    public List<GameObject> ingredientModels; // Drag pre-placed models here (disable them in the editor)

    [Header("Door")]
    public GameObject door; // The door to unlock

    [Header("UI Feedback")]
    public TMP_Text feedbackText; // Assign your TMP text element here

    [SerializeField] private UnityEvent onAccessGranted;

    public UnityEvent OnAccessGranted => onAccessGranted;

    [SerializeField] private AudioClip addsound; //sound played when picking up the object

    private AudioSource audioSource; //audio source to play the sound
    

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnMouseDown()
    {
        GameObject equippedItem = EquipSystem.Instance.selectedItem;

        if (equippedItem == null)
        {
            ShowFeedback("No ingredient equipped!");
            return;
        }

        string itemName = equippedItem.name.Replace("(Clone)", "").Trim();

        if (requiredIngredients.Contains(itemName) && !placedIngredients.Contains(itemName))
        {
            placedIngredients.Add(itemName);
            ShowFeedback(itemName + " placed on the cutting board!");

            // Activate the correct model
            ActivateIngredientModel(itemName);

            // Remove from quickslot and clear the equipped item
            RemoveFromQuickSlot(equippedItem);

            // Check if all ingredients are placed
            CheckCompletion();

            audioSource.clip = addsound; //sets the audio clip to the one we set in the inspector
            audioSource.Play(); //plays the sound
        }
        else
        {
            ShowFeedback(itemName + " is not needed or already placed.");
        }
    }

    private void CheckCompletion()
    {
        if (placedIngredients.Count == requiredIngredients.Count)
        {
            UnlockDoor();
        }
    }

    private void UnlockDoor()
    {
        if (door != null)
        {
            ShowFeedback("All ingredients placed! Door unlocked.");
            //door.SetActive(false); // Adjust this to trigger an animation instead if needed
            onAccessGranted?.Invoke();
        }
    }

    private void RemoveFromQuickSlot(GameObject item)
    {
        item.transform.SetParent(null);

        if (EquipSystem.Instance.selectedItemModel != null)
        {
            Destroy(EquipSystem.Instance.selectedItemModel.gameObject); // Remove hand model
            EquipSystem.Instance.selectedItemModel = null;
        }

        EquipSystem.Instance.selectedItem = null;
        EquipSystem.Instance.selectedNumber = -1;
    }

    private void ActivateIngredientModel(string itemName)
    {
        foreach (GameObject model in ingredientModels)
        {
            if (model.name.Equals(itemName, System.StringComparison.OrdinalIgnoreCase))
            {
                model.SetActive(true); // Show the pre-placed ingredient model
                break;
            }
        }
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
