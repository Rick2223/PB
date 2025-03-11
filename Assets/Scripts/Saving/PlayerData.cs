using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class PlayerData
{
    public float[] playerPositionAndRotation;
    public string[] inventoryContent;
    public string[] quickSlotsContent;

    public PlayerData(float[] _playerPosAndRot, string[] _inventoryContent, string[] _quickSlotsContent)
    {
        playerPositionAndRotation = _playerPosAndRot;
        inventoryContent = _inventoryContent;
        quickSlotsContent = _quickSlotsContent;
    }
}
