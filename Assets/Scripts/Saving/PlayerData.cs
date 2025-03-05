using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class PlayerData
{
    public float[] PlayerPositionAndRotation;
    //public string[] inventoryContent;

    public PlayerData(float[] _playerPosAndRot)
    {
        PlayerPositionAndRotation = _playerPosAndRot;
    }
}
