using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public GameObject towerBreakParticle;
    public bool mainTowerPlayer;
    public bool mainTowerEnemy;
    private HealthBar healthBar;

    private int mainTowerHealth = 3000;
    private int towerHealth = 1200;


    private float health;

    private void Start()
    {
        healthBar = GetComponentInChildren<HealthBar>();
        if (mainTowerPlayer || mainTowerEnemy)
        {
            health = mainTowerHealth;
        }
        else
        {
            health = towerHealth;
        }
        healthBar.setMaxHealth((int)health);
        healthBar.gameObject.SetActive(false);
    }
    
    public void subtractHealth(float damage)
    {
        healthBar.gameObject.SetActive(true);
        health -= damage;
        if (health <= 0)
        {
            destroyTower();
        }
        if (healthBar != null)
            healthBar.setHealth((int)health);
    }
    
    public void destroyTower() 
    {
        if (mainTowerPlayer)
        {
            GameManager.instance.gameOver();
            return;
        }
        else if (mainTowerEnemy)
        {
            GameManager.instance.win(true);
            return;
        }
        GameObject go = Instantiate(towerBreakParticle) as GameObject;
        go.transform.position = transform.position;
        Destroy(gameObject);
    }
}
