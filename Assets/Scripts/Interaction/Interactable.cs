using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    
    public string message;

    public UnityEvent onInteraction;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Interact()
    {
        onInteraction.Invoke();
    }
    
    

}
