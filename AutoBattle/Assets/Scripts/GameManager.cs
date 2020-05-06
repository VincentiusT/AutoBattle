using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool isGameOver;
    public GameObject pausePanel;
    public GameObject GameOverPanel;
    public GameObject winPanel;
    public int totalPlayerTower, totalEnemyTower;

    private void Awake()
    {
        instance = this;
        Time.timeScale = 1f;
    }

    public bool checkWin()
    {
        bool suddenDeath = false;
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
            win();
        }
        return suddenDeath;
    }

    public void gameOver()
    {
        isGameOver = true;
        GameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void win()
    {
        winPanel.SetActive(true);
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

    public void home()
    {
        SceneManager.LoadScene("Menu");
    }
}
