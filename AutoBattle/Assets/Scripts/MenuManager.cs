using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public RectTransform title;
    public TextMeshProUGUI starText;

    private void Start()
    {
        starText.text = Inventory.star.ToString("0");
    }

    private void Update()
    {
        titleIdle();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void titleIdle()
    {
        title.anchoredPosition =new Vector2(title.anchoredPosition.x, title.anchoredPosition.y + Mathf.Sin(Time.time*2f)/5f );
    }

    public void play()
    {
        SceneManager.LoadScene("play");
    }

    public void toHeroes()
    {
        SceneManager.LoadScene("HeroSelect");
    }
}
