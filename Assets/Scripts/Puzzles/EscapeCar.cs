using UnityEngine.SceneManagement;
using UnityEngine;

public class EscapeCar : MonoBehaviour
{
    public string requiredItemName = "Carkey";
    public string endSceneName = "EndScene";
    public float interactionDistance = 10f;

    public Transform player;

    [SerializeField] private AudioClip carstartsound; //sound played when picking up the object
    private AudioSource audioSource; //audio source to play the sound


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    private void OnMouseDown()
    {
        if (IsPlayerCloseEnough())
        {
            if (EquipSystem.Instance.selectedItem.name != requiredItemName)
            {
                Debug.Log("You need a car key to escape!");
            }
            else
            {
                audioSource.clip = carstartsound; //sets the audio clip to the one we set in the inspector
                audioSource.Play(); //plays the sound
                LoadScene();
            }
        }
        else
        {
            Debug.Log("You are too far away from the car!");
        }
        
    }

    private bool IsPlayerCloseEnough()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        return distance <= interactionDistance;
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(endSceneName);
        Debug.Log("Loading end scene...");
    }
}
