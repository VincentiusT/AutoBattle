using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HeroesSelectionManager : MonoBehaviour
{
    public GameObject descFrame;
    public GameObject statFrame;
    public TextMeshProUGUI nameTag;
    public TextMeshProUGUI statText;
    public TextMeshProUGUI heroDescText;
    public Identity[] playerIdentity;

    int index;

    private void Start()
    {
        statFrame.SetActive(false);
    }

    public void backToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void showStat(int idx)
    {
        statFrame.SetActive(true);
        index = idx;
        if (descFrame.activeSelf) descFrame.SetActive(false);
        nameTag.text = playerIdentity[idx].name;
        string type="";
        if (playerIdentity[idx].type == 1)
        {
            type = "attack enemy";
        }
        else
        {
            type = "attack tower";
        }
        statText.text = ": "+playerIdentity[idx].health + "\n: "+
        playerIdentity[idx].attack + "\n: "+
        playerIdentity[idx].attackSpeed + "\n: "+
        playerIdentity[idx].attackRadius + "\n: "+
        playerIdentity[idx].speed + "\n: "+
        playerIdentity[idx].spawnTime + "\n: "+
        type + "\n: "+
        playerIdentity[idx].cost + "\n";
    }

    public void closeStat()
    {
        statFrame.SetActive(false);
        descFrame.SetActive(false);
    }

    public void heroDesc()
    {
        descFrame.SetActive(true);
        heroDescText.text = playerIdentity[index].desc;
    }
}
