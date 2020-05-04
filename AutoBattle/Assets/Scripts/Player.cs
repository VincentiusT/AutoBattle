using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    public Identity playerIdentity;
    public List<GameObject> towers;
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
    private int type;
    public Sprite deckArtwork; 

    private float radius = 6f;
    private float attackDamage;
    private float nextAttackTime;
    private bool isLocked;
    private bool canAttack;
    private GameObject tempGO;

    private HealthBar healthBar;
    private Transform cam;

    private void Start()    
    {
        cam = Camera.main.transform;
        child = transform.GetChild(0).gameObject;
        towers = new List<GameObject>();
        towers.AddRange(GameObject.FindGameObjectsWithTag("EnemyTower"));
        healthBar = GetComponentInChildren<HealthBar>();
        child.GetComponent<SpriteRenderer>().sprite = playerIdentity.artwork;
        attackDamage = playerIdentity.attack;
        attackSpeed = playerIdentity.attackSpeed;
        attackRadius = playerIdentity.attackRadius;
        health = playerIdentity.health;
        spawnTime = playerIdentity.spawnTime;
        speed = playerIdentity.speed;
        deckArtwork = playerIdentity.deckArtwork;
        type = playerIdentity.type;
        target = getClosestGameObject(towers);

        healthBar.setMaxHealth((int)health);

        healthBar.gameObject.SetActive(false);

        GameManager.instance.totalEnemyTower = towers.Count;
        if (radius < attackRadius) radius = attackRadius;
        
    }

    private void Update()
    {
        if (!isPlaced || !canAttack) return;
        if(type==1) //attack normal
        {
            moveNormal();
        }
        else if(type ==2) //attack tower
        {
            moveTower();
        }
        
        if(health <= 0)
        {
            dead();
        }
    }

    private void moveNormal()
    {
        if (!isLocked) enemies = checkEnemyInRadius(radius);

        if (enemies.Length > 0 && !isLocked)
        {
            currentEnemy = getClosestGameObject(enemies);
            if (Vector3.Distance(transform.position, currentEnemy.position) <= attackRadius)
            {
                speed = 0;
                if (Time.time >= nextAttackTime)
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
            if (isLocked && target == null)
            {
                isLocked = false;
                towers.Remove(tempGO);
                GameManager.instance.totalEnemyTower = towers.Count;
            }
            if (towers.Count <= 0) return;
            if (!isLocked) target = getClosestGameObject(towers);
            if (Vector3.Distance(transform.position, target.position) <= attackRadius)
            {
                speed = 0;
                isLocked = true;
                tempGO = target.gameObject;
                if (Time.time >= nextAttackTime)
                {
                    target.GetComponent<Tower>().subtractHealth(attackDamage);
                    nextAttackTime = Time.time + 1f * attackSpeed;
                }
            }
            else
            {
                speed = playerIdentity.speed;
            }
        }
    }

    private void moveTower()
    {
        if (isLocked && target == null)
        {
            isLocked = false;
            towers.Remove(tempGO);
            GameManager.instance.totalEnemyTower = towers.Count;
        }
        if (towers.Count <= 0) return;
        if (!isLocked) target = getClosestGameObject(towers);
        if (Vector3.Distance(transform.position, target.position) <= attackRadius)
        {
            speed = 0;
            isLocked = true;
            tempGO = target.gameObject;
            if (Time.time >= nextAttackTime)
            {
                target.GetComponent<Tower>().subtractHealth(attackDamage);
                nextAttackTime = Time.time + 1f * attackSpeed;
            }
        }
        else
        {
            speed = playerIdentity.speed;
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
            enemy.GetComponent<Enemy>().subtractHealth(attackDamage);
        }
    }

    public void subtractHealth(float damage)
    {
        healthBar.gameObject.SetActive(true);
        health -= damage;
        healthBar.setHealth((int)health);
    }

    void dead()
    {
        isKeepUpdatetingPath = false;
        Destroy(gameObject);
    }

    public IEnumerator spawnThisPlayer()
    {
        isPlaced = true;
        yield return new WaitForSeconds(playerIdentity.spawnTime);
        canAttack = true;
        StartCoroutine(updatePath());
    }

    
}
