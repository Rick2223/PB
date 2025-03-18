using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class EnvironmentData
{
    public List<string> pickedupItems;
    public List<int> openedDoors;

    public EnvironmentData(List<string> _pickedupItems, List<int> _openedDoors)
    {
        pickedupItems = _pickedupItems;
        openedDoors = _openedDoors;
        
    }
    
}
