using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
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

    private float radius = 6f;
    private float attackDamage;
    private float nextAttackTime;

    private HealthBar healthBar;

    void Start()
    {
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

        healthBar.setMaxHealth((int)health);

        StartCoroutine(updatePath());
    }
    
    void Update()
    {

        players = checkPlayerInRadius(radius);

        if(players.Length > 0)
        {
            currentPlayer = getClosestGameObject(players);
            if (currentPlayer.GetComponent<Player>().isPlaced)
            {
                if (Vector3.Distance(transform.position, currentPlayer.position) < attackRadius )
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
            target = getClosestGameObject(towers);
            speed = enemyIdentity.speed;
        }

        if (health <= 0)
        {
            dead();
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
        if(healthBar!=null)
            healthBar.setHealth((int)health);
    }

    private Collider[] checkPlayerInRadius(float _radius)
    {
        return Physics.OverlapSphere(transform.position, _radius, playerMask);
    }

    void dead()
    {
        isKeepUpdatetingPath = false;
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
