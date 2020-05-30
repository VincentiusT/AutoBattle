using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelController : MonoBehaviour
{
    public void toPlay(int idx)
    {
        if (idx == 1) SceneManager.LoadScene("play");
        else if (idx == 2)SceneManager.LoadScene("play2");
        else if (idx == 3)SceneManager.LoadScene("play3");
        else if (idx == 4) SceneManager.LoadScene("play4");
        else if (idx == 5) SceneManager.LoadScene("play5");
        else if (idx == 6) SceneManager.LoadScene("play6");
        else if (idx == 7) SceneManager.LoadScene("play7");
    }

    public void backToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
