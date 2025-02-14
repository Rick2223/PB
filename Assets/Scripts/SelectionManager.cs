using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
 
public class SelectionManager : MonoBehaviour
{
 
    public GameObject interaction_Info_UI;
    TextMeshProUGUI interaction_text;
 
    private void Start()
    {
        interaction_text = interaction_Info_UI.GetComponent<TextMeshProUGUI>();
    }
 
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 3))
        {
            var selectionTransform = hit.transform;
 
            if (selectionTransform.GetComponent<InteractableObject>())
            {
                if (hit.transform.gameObject.tag == "canPickUp")
                {
                    interaction_text.text = "Press 'E' to pick up " + selectionTransform.GetComponent<InteractableObject>().GetItemName();
                    interaction_Info_UI.SetActive(true);
                }
                else
                {
                    interaction_text.text = selectionTransform.GetComponent<InteractableObject>().GetItemName();
                    interaction_Info_UI.SetActive(true);
                }
            }
            else 
            { 
                interaction_Info_UI.SetActive(false);
            }
 
        }
        else
        {
            interaction_Info_UI.SetActive(false);
        }
    }
}