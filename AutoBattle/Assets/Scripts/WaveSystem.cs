using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveSystem : MonoBehaviour
{
    public enum SpawnState { SPAWNING, WAITING, COUNTING};

    [System.Serializable]
    public class Wave
    {
        public string name;
        public MiniWave[] miniWave;
        public float rate;
    }
    [System.Serializable]
    public class MiniWave
    {
        public GameObject enemy;
        public int count;
    }

    public List<Transform> spawnPoint;
    public Wave[] waves;
    private int nextWave = 0;
    private int nextMiniWave = 0;

    public float restTime = 10f;
    public float waveCountDown;

    public TextMeshProUGUI timerText;
    
    float totalTime = 70;
    bool isSuddenDeath=false;
    float searchCountdown = 1f;
    int tempIdx = 0;
    bool stopTimer = false;

    private SpawnState state = SpawnState.COUNTING;

    private void Start()
    {
        waveCountDown = restTime;
    }

    private void Update()
    {
        if (!stopTimer) timer();
        if (state == SpawnState.WAITING)
        {
            if (!thereIsEnemy())
            {
                waveComplete();
                return;
            }
            else
            {
                return;
            }
        }

        if(waveCountDown <= 0)
        {
            if(state != SpawnState.SPAWNING)
            {
                StartCoroutine(spawnWave(waves[nextWave]));
            }
        }
        else
        {
            waveCountDown -= Time.deltaTime;
        }
    }

    private bool thereIsEnemy()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("enemy") == null)
            {
                return false;
            }
        }
        return true;
    }

    private void waveComplete()
    {
        Debug.Log("wave complete");

        state = SpawnState.COUNTING;
        waveCountDown -= Time.deltaTime;

        if(nextWave+1 > waves.Length - 1)
        {
            nextWave = 0;
            Debug.Log("all waves complete ");
        }
        else
        {
            nextWave++;
            nextMiniWave = 0;
        }
        
    }

    private IEnumerator spawnWave(Wave wave)
    {
        state = SpawnState.SPAWNING;

        while (nextMiniWave < wave.miniWave.Length)
        {
            for (int i = 0; i < wave.miniWave[nextMiniWave].count; i++)
            {
                spawnEnemy(wave.miniWave[nextMiniWave].enemy);
                yield return new WaitForSeconds(1f/wave.rate);
            }
            
            nextMiniWave++;
            yield return new WaitForSeconds(5f);
        }

        waveCountDown = restTime;
        state = SpawnState.WAITING;
        yield break;
    }

    private void spawnEnemy(GameObject enemy)
    {
        GameObject go;
        go = Instantiate(enemy, spawnPoint[getRandomIndex()].position + new Vector3(0, 0, -2),Quaternion.identity) as GameObject;
        go.transform.SetParent(transform);
    }

    private int getRandomIndex()
    {
        if (spawnPoint.Count <= 0) return 0;
        for(int i = 0; i < spawnPoint.Count; i++)
        {
            if (spawnPoint[i] == null)
            {
                spawnPoint.RemoveAt(i);
            }
        }
        int idx = 0;
        while (idx == tempIdx)
        {
            idx = Random.Range(0, spawnPoint.Count);
        }
        tempIdx = idx;
        return idx;
    }

    void timer()
    {
        if (!isSuddenDeath)
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
                isSuddenDeath = GameManager.instance.checkSuddendeath();
                if (isSuddenDeath)
                {
                    timerText.text = "SUDDEN DEATH!";
                }
                else stopTimer = true;
            }
        }
        else
        {
            isSuddenDeath = GameManager.instance.checkSuddendeath();
            if (!isSuddenDeath) stopTimer=true;
        }
    }
}
