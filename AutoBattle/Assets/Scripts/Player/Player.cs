using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    public GameObject deadParticle;
    public Identity playerIdentity;
    [HideInInspector]
    public List<GameObject> towers;
    public LayerMask enemyMask;
    public LayerMask unwalkableMask;
    [HideInInspector]
    public bool isPlaced=false;

    private Collider[] enemies;
    private GameObject child;
    private Transform currentEnemy;
    protected float attackRadius;
    protected float attackSpeed;
    protected float health;
    protected float spawnTime;
    protected int type;
    protected Sprite deckArtwork;
    protected Animator playerAnim;

    private float radius = 6f;
    protected float attackDamage;
    private float nextAttackTime;
    private bool isLocked;
    private bool canAttack;
    private GameObject tempGO;

    private HealthBar healthBar;
    private Transform cam;

    protected virtual void Start()    
    {
        cam = Camera.main.transform;
        child = transform.GetChild(0).gameObject;
        towers = new List<GameObject>();
        towers.AddRange(GameObject.FindGameObjectsWithTag("EnemyTower"));
        healthBar = GetComponentInChildren<HealthBar>();
        attackDamage = playerIdentity.attack;
        attackSpeed = playerIdentity.attackSpeed;
        attackRadius = playerIdentity.attackRadius;
        health = playerIdentity.health;
        spawnTime = playerIdentity.spawnTime;
        speed = playerIdentity.speed;
        deckArtwork = playerIdentity.deckArtwork;
        type = playerIdentity.type;
        target = getClosestGameObject(towers);
        playerAnim = GetComponentInChildren<Animator>();

        healthBar.setMaxHealth((int)health);

        healthBar.gameObject.SetActive(false);

        GameManager.instance.totalEnemyTower = towers.Count;
        if (radius < attackRadius) radius = attackRadius;
        
    }

    protected virtual void Update()
    {
        if (!isPlaced || !canAttack) return;
        if (type==1) //attack normal
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

    protected virtual void moveNormal()
    {
        if (!isLocked) enemies = checkEnemyInRadius(radius);

        if (enemies.Length > 0 && !isLocked)
        {
            currentEnemy = getClosestGameObject(enemies);
            if ((transform.position - currentEnemy.position).sqrMagnitude <= attackRadius * attackRadius)
            {
                speed = 0;
                if (Time.time >= nextAttackTime)
                {
                    attack(currentEnemy.gameObject,false);
                    nextAttackTime = Time.time + 1f / attackSpeed;
                }
            }
            else
            {
                isAttacking = false;
                speed = playerIdentity.speed;
                if (playerAnim != null) playerAnim.SetBool("move", true);
            }
            target = currentEnemy;
        }
        else
        {
            if (isLocked && target == null)
            {
                isLocked = false;
                towers.Remove(tempGO);
            }
            if (towers.Count <= 0) return;
            if (!isLocked) target = getClosestGameObject(towers);
            if (target == null)return;
            if (Vector3.Distance(transform.position, target.position) <= attackRadius)
            {
                speed = 0;
                isLocked = true;
                tempGO = target.gameObject;
                if (Time.time >= nextAttackTime)
                {
                    attack(target.gameObject,true);
                    //target.GetComponent<Tower>().subtractHealth(attackDamage);
                    nextAttackTime = Time.time + 1f / attackSpeed;
                }
            }
            else
            {
                speed = playerIdentity.speed;
                isAttacking = false;
                if (playerAnim != null) playerAnim.SetBool("move", true);
            }
        }
    }

    protected virtual void moveTower()
    {
        if (isLocked && target == null)
        {
            isLocked = false;
            towers.Remove(tempGO);
        }
        if (towers.Count <= 0) return;
        if (!isLocked) target = getClosestGameObject(towers);
        if (target == null) return;
        if ((transform.position - target.position).sqrMagnitude <= attackRadius * attackRadius)
        {
            speed = 0;
            isLocked = true;
            tempGO = target.gameObject;
            if (Time.time >= nextAttackTime)
            {
                attack(target.gameObject, true);
                nextAttackTime = Time.time + 1f / attackSpeed;
            }
        }
        else
        {
            speed = playerIdentity.speed;
            isAttacking = false;
            if (playerAnim != null) playerAnim.SetBool("move", true);
        }
    }

    private Collider[] checkEnemyInRadius(float _radius)
    {
        return Physics.OverlapSphere(transform.position, _radius, enemyMask);
    }

    protected virtual void attack(GameObject enemy, bool isTower)
    {
        if (playerAnim != null)
        {
            isAttacking = true;
            playerAnim.SetBool("move", false);
            playerAnim.SetTrigger("attack");
        }
        if (enemy != null)
        {
            Vector3 lookPos = enemy.transform.position - transform.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = rotation;
            //transform.LookAt(enemy.transform);
        }
    }

    public void subtractHealth(float damage)
    {
        healthBar.gameObject.SetActive(true);
        health -= damage;
        healthBar.setHealth((int)health);
    }

    protected virtual void dead()
    {
        GameObject go = Instantiate(deadParticle) as GameObject;
        go.transform.position = transform.position;
        Destroy(go, 1f);
        isKeepUpdatetingPath = false;
        Destroy(gameObject);
    }

    public IEnumerator spawnThisPlayer()
    {
        isPlaced = true;
        yield return new WaitForSeconds(playerIdentity.spawnTime);
        canAttack = true;
        if (playerAnim != null)
        {
            playerAnim.SetBool("move", true);
        }
        StartCoroutine(updatePath());
    }
    
}
