using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Inventory inven;
    public bool isGameOver;
    public GameObject howToPlayPanel;
    public GameObject pausePanel;
    public GameObject GameOverPanel;
    public GameObject winPanel;
    public GameObject[] stars;
    [HideInInspector]
    public int totalPlayerTower=3, totalEnemyTower=3;

    private void Awake()
    {
        instance = this;
        Time.timeScale = 1f;
    }

    public bool checkSuddendeath()
    {
        bool suddenDeath = false;
        totalEnemyTower = GameObject.FindGameObjectsWithTag("EnemyTower").Length;
        totalPlayerTower = GameObject.FindGameObjectsWithTag("PlayerTower").Length;
        if (totalPlayerTower < totalEnemyTower)
        {
            gameOver();
        }
        else if(totalPlayerTower == totalEnemyTower)
        {
            //sudden death
            suddenDeath = true;
        }
        else
        {
            win(false);
        }
        return suddenDeath;
    }

    public void gameOver()
    {
        isGameOver = true;
        GameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void win(bool allTowerDestroyed)
    {
        winPanel.SetActive(true);
        if (allTowerDestroyed)
        {
            for(int i = 0; i < 3; i++)
            {
                stars[i].SetActive(true);
            }
            Inventory.star += 3;
        }
        else
        {
            int t = GameObject.FindGameObjectsWithTag("EnemyTower").Length;
            int str = 3 - t;
            for (int i=0;i< str; i++)
            {
                stars[i].SetActive(true);
            }
            Inventory.star += str;
        }
        inven.Save();
        Time.timeScale = 0f;
    }

    public void restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void pause(bool pauseNow)
    {
        if (pauseNow)
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void nextStage()
    {
        if(SceneManager.GetActiveScene().buildIndex + 1 == SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene("Level");
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void home()
    {
        SceneManager.LoadScene("Menu");
    }

    public void openHowToPlay()
    {
        howToPlayPanel.SetActive(true);
    }

    public void closeHowToPlay()
    {
        howToPlayPanel.SetActive(false);
    }
}
