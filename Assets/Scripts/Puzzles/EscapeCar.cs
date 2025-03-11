using UnityEngine.SceneManagement;
using UnityEngine;

public class EscapeCar : MonoBehaviour
{
    public string requiredItemName = "Carkey";
    public string endSceneName = "EndScene";
    public float interactionDistance = 10f;

    public Transform player;


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
