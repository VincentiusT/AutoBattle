using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemies;
    public float spawnTime=3f;

    private float currTime;
    private int tempIndex;

    private void Update()
    {
        currTime += Time.deltaTime;
        if (currTime >= spawnTime)
        {
            spawn();
            currTime = 0;
        }
    }

    private void spawn()
    {
        GameObject go;
        go = Instantiate(enemies[getRandomIndex(enemies)]) as GameObject;
        go.transform.SetParent(transform);
        go.transform.position = transform.position + new Vector3(0,0,-2);
    }

    private int getRandomIndex(GameObject[] g)
    {
        if (g.Length <= 0) return 0;
        int idx=0;
        while (idx != tempIndex)
        {
            idx = Random.Range(0, g.Length);
        }
        tempIndex = idx;
        return idx;
    }
}
