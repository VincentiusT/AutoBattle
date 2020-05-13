using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
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
        }
        else if (mainTowerEnemy)
        {
            GameManager.instance.win(true);
        }
        Destroy(gameObject);
        
    }
}
