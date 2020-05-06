using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    public GameObject deadParticle;
    public Identity enemyIdentity;
    public List<GameObject> towers;
    public LayerMask playerMask;

    private Collider[] players;
    private GameObject child;
    private Transform currentPlayer;
    private float attackRadius;
    private float attackSpeed;
    private float health;
    private float spawnTime;
    private int type;

    private float radius = 6f;
    private float attackDamage;
    private float nextAttackTime;
    private bool isLocked;
    private GameObject tempGO;

    private HealthBar healthBar;
    private Transform cam;

    void Start()
    {
        cam = Camera.main.transform;
        child = transform.GetChild(0).gameObject;
        towers = new List<GameObject>();
        towers.AddRange(GameObject.FindGameObjectsWithTag("PlayerTower"));
        
        healthBar = GetComponentInChildren<HealthBar>();

        child.GetComponent<SpriteRenderer>().sprite = enemyIdentity.artwork;
        attackDamage = enemyIdentity.attack;
        attackSpeed = enemyIdentity.attackSpeed;
        attackRadius = enemyIdentity.attackRadius;
        health = enemyIdentity.health;
        spawnTime = enemyIdentity.spawnTime;
        speed = enemyIdentity.speed;
        target = getClosestGameObject(towers);
        type = enemyIdentity.type;

        healthBar.setMaxHealth((int)health);
        healthBar.gameObject.SetActive(false);

        GameManager.instance.totalPlayerTower = towers.Count;
        StartCoroutine(updatePath());
    }
    
    void Update()
    {
        if (type == 1)
        {
            moveNormal();
        }
        else if (type == 2)
        {
            moveTower();
        }

        if (health <= 0)
        {
            dead();
        }
    }

    private void moveNormal()
    {
        if (!isLocked) players = checkPlayerInRadius(radius);

        if (players.Length > 0 && !isLocked)
        {
            currentPlayer = getClosestGameObject(players);
            if (currentPlayer.GetComponent<Player>().isPlaced)
            {
                if (Vector3.Distance(transform.position, currentPlayer.position) < attackRadius)
                {
                    if (Time.time >= nextAttackTime)
                    {
                        attack(currentPlayer.gameObject);
                        nextAttackTime = Time.time + 1f / attackSpeed;
                    }
                }
                else
                {
                    speed = enemyIdentity.speed;
                }
                target = currentPlayer;
            }
        }
        else
        {
            if (isLocked && target == null)
            {
                
                isLocked = false;
                towers.Remove(tempGO);
                GameManager.instance.totalPlayerTower = towers.Count;
            }
            if (towers.Count <= 0) return; 
            if (!isLocked)
            {
                target = getClosestGameObject(towers);
            }
            if (Vector3.Distance(transform.position, target.position) <= attackRadius)
            {
                speed = 0;
                isLocked = true;
                tempGO = target.gameObject;
                if (Time.time >= nextAttackTime)
                {
                    target.GetComponent<Tower>().subtractHealth(attackDamage);
                    nextAttackTime = Time.time + 1f / attackSpeed;
                }
            }
            else
            {
                speed = enemyIdentity.speed;
            }
        }

    }

    private void moveTower()
    {
        if (isLocked && target == null)
        {
            isLocked = false;
            towers.Remove(tempGO);
            GameManager.instance.totalPlayerTower = towers.Count;
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
                nextAttackTime = Time.time + 1f / attackSpeed;
            }
        }
        else
        {
            speed = enemyIdentity.speed;
        }
    }

    private void attack(GameObject player)
    {
        speed = 0;
        if (player != null)
        {
            player.GetComponent<Player>().subtractHealth(attackDamage);
        }
    }

    public void subtractHealth(float damage)
    {
        health -= damage;
        if (healthBar != null)
        {
            healthBar.gameObject.SetActive(true);
            healthBar.setHealth((int)health);
        }
    }

    private Collider[] checkPlayerInRadius(float _radius)
    {
        return Physics.OverlapSphere(transform.position, _radius, playerMask);
    }

    void dead()
    {
        GameObject go = Instantiate(deadParticle) as GameObject;
        go.transform.position = transform.position;
        Destroy(go, 1f);
        isKeepUpdatetingPath = false;
        Destroy(gameObject);
    }
}
