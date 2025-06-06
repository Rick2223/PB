using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class ButtonPuzzle3D : MonoBehaviour
{
    public Animator gateLeft;
    public Animator gateRight;
    public TMP_Text wrongorright;
    private int currentStep = 0;

    [SerializeField] private UnityEvent onAccessGranted;

    public UnityEvent OnAccessGranted => onAccessGranted;

    // Define the correct sequence (0 = Red, 1 = Blue, 2 = Green)
    private int[] correctSequence = { 1, 0, 2, 3, 5 };

    private void Start()
    {
        wrongorright.gameObject.SetActive(false);
    }
    
    public void OnButtonPress(int buttonID)
    {
        if (buttonID == correctSequence[currentStep])
        {
            currentStep++;
            if (currentStep >= correctSequence.Length)
            {
                OpenGate();
            }
        }
        else
        {
            Debug.Log("Wrong button! Resetting.");
            currentStep = 0;
            ShowFeedback("Incorrect sequence! Try again!", Color.red);
            
        }
    }

    private void OpenGate()
    {
        Debug.Log("Gate opened!");
        ShowFeedback("Correct sequence! Gate opened!", Color.green);
        onAccessGranted?.Invoke();
        
    }

    private void ShowFeedback(string message, Color color)
    {
        wrongorright.color = color;
        wrongorright.text = message;
        wrongorright.gameObject.SetActive(true);
        Invoke("HideFeedback", 2f);
    }

    private void HideFeedback()
    {
        wrongorright.gameObject.SetActive(false);
    }
}
