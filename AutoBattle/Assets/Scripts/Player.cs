using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    public Identity playerIdentity;
    public Transform mainTarget;
    public LayerMask enemyMask;
    public LayerMask unwalkableMask;
    public bool isPlaced=false;

    private Collider[] enemies;
    private GameObject child;
    private Transform currentEnemy;
    private float attackRadius;
    private float attackSpeed;
    private float health;
    private float spawnTime;
    public Sprite deckArtwork; 

    private float radius = 6f;
    private float attackDamage;
    private float nextAttackTime;
    private bool done;

    private HealthBar healthBar;

    private void Start()    
    {
        child = transform.GetChild(0).gameObject;
        mainTarget = GameObject.FindGameObjectWithTag("EnemyTower").transform;
        healthBar = GetComponentInChildren<HealthBar>();

        child.GetComponent<SpriteRenderer>().sprite = playerIdentity.artwork;
        attackDamage = playerIdentity.attack;
        attackSpeed = playerIdentity.attackSpeed;
        attackRadius = playerIdentity.attackRadius;
        health = playerIdentity.health;
        spawnTime = playerIdentity.spawnTime;
        speed = playerIdentity.speed;
        deckArtwork = playerIdentity.deckArtwork;
        target = mainTarget;

        healthBar.setMaxHealth((int)health);
    
    }

    private void Update()
    {
        if (!isPlaced) return;

        enemies = checkEnemyInRadius(radius);

        if (enemies.Length > 0)
        {
            currentEnemy = getClosestGameObject(enemies);
            if(Vector3.Distance(transform.position,currentEnemy.position) < attackRadius)
            {
                if(Time.time >= nextAttackTime)
                {
                    attack(currentEnemy.gameObject);
                    nextAttackTime = Time.time + 1f / attackSpeed;
                }
            }
            else
            {
                speed = playerIdentity.speed;
            }
            target = currentEnemy;
        }
        else
        {
            target = mainTarget;
            speed = playerIdentity.speed;
        }

        if(health <= 0)
        {
            dead();
        }
    }

    private Collider[] checkEnemyInRadius(float _radius)
    {
        return Physics.OverlapSphere(transform.position, _radius, enemyMask);
    }

    private void attack(GameObject enemy)
    {
        if(enemy != null)
        {
            speed = 0;
            enemy.GetComponent<Enemy>().subtractHealth(attackDamage);
        }
    }

    public void subtractHealth(float damage)
    {
        health -= damage;
        healthBar.setHealth((int)health);
    }

    void dead()
    {
        isKeepUpdatetingPath = false;
        Destroy(gameObject);
    }

    public void spawnThisPlayer()
    {
        isPlaced = true;
        StartCoroutine(updatePath());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
