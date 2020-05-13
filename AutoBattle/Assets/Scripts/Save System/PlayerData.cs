using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class PlayerData
{
    public int highscore;

    public PlayerData(Inventory inventory)
    {
    
        highscore = Inventory.highscore;
    }
}
