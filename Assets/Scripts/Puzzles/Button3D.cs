using UnityEngine;

public class Button3D : MonoBehaviour
{
    public ButtonPuzzle3D puzzleManager; // Assign the PuzzleManager
    public int buttonID; 
    private Vector3 originalPosition;
    public float pressDepth = 0.2f;
    public float resetSpeed = 5f;
    
    private void Start()
    {
        originalPosition = transform.position;
    }
    
    private void OnMouseDown()
    {
        puzzleManager.OnButtonPress(buttonID);
        PressButton();
    }

    private void PressButton()
    {
        transform.position = originalPosition - new Vector3(-pressDepth, 0, 0);
        Invoke("ResetButton", 0.2f);
    }

    private void ResetButton()
    {
        transform.position = Vector3.Lerp(transform.position, originalPosition, Time.deltaTime * resetSpeed);
    }
}
