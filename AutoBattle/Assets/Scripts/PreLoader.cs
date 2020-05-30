using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreLoader : MonoBehaviour
{
   public void preLoaderComplete()
    {
        SceneManager.LoadScene("Menu");
    }
}
