using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventory : MonoBehaviour
{
    public static int star;
    public static bool doneTutorial;
    public static bool load = true;

    private void Awake()
    {
        if (load)
        {
            Load();
            load = false;
        }
    }
    public void Save()
    {
        SaveSystem.SavePlayer(this);
    }
    public void Load()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        if (data == null)
        {
            return;
        }
        star = data.star;
        doneTutorial = data.doneTutorial;
        
    }
}
