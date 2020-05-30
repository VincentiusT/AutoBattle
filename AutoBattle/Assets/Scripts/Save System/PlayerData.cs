using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class PlayerData
{
    public int star;
    public bool doneTutorial;

    public PlayerData(Inventory inventory)
    {
    
        star = Inventory.star;
        doneTutorial = Inventory.doneTutorial;
    }
}
