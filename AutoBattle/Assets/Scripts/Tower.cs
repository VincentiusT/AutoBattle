using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public bool mainTower;
    private HealthBar healthBar;
    private int mainTowerHealth = 3000;
    private int towerHealth = 1200;

    private float health;

    private void Start()
    {
        healthBar = GetComponentInChildren<HealthBar>();
        if (mainTower)
        {
            health = mainTowerHealth;
        }
        else
        {
            health = towerHealth;
        }
        healthBar.setMaxHealth((int)health);
    }
    
    public void subtractHealth(float damage)
    {
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
        Destroy(gameObject);
    }
}
