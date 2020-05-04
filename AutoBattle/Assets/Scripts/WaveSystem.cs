using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveSystem : MonoBehaviour
{
    private float waveBar;
    public TextMeshProUGUI timerText;
    public EnemySpawner spawner;

    private float maxValue = 30;

    private float[] wave;
    private bool[] waveActive;

    float totalTime = 70;
    int totalWave=3;
    int x=1;
    bool restWave = false;
    float restTime = 5f;
    float originalRestTime;

    private void Start()
    {
        wave = new float[totalWave+1];
        waveActive = new bool[totalWave+1];
        wave[0] = 0;
       // wave[1] = 10; wave[2] = 20; wave[3] = 30;
        for (int i = 1; i <= totalWave; i++)
        {
            wave[i] = wave[i - 1] + 10;
        }
        waveActive[1] = true;
        waveBar = 0;
        originalRestTime = restTime;
    }

    private void Update()
    {
        if (restWave)
        {
            restTime -= Time.deltaTime;
            if (restTime <= 0)
            {
                restTime = originalRestTime;
                restWave = false;
            }
        }
        else
            checkWave();

        timer();
    }

    private void checkWave()
    {
        if (x > totalWave)
        {
            //all wave complete
            return;
        }
        if (waveActive[x])
        {
            spawner.spawnByWave(x);

            waveBar += 0.8f * Time.deltaTime;
            if(waveBar >= wave[x])
            {
                x++;
                restWave=true;
                if(x<=totalWave) waveActive[x] = true;
                return;
            }
            
        }
    }

    void timer()
    {
        string plusZero1 = "", plusZero2="";
        float x = totalTime % 60;
        float y = totalTime / 60;
        if(x < 10)
        {
            plusZero1 = "0";
        }
        if(y < 10)
        {
            plusZero2 = "0";
        }
        timerText.text = plusZero2+((int)y).ToString("0") + ":" + plusZero1+((int)x).ToString("0");
        totalTime -= Time.deltaTime;
        if (totalTime <= 0)
        {
            GameManager.instance.checkWin();
        }
    }
}
