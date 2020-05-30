using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arjuna : Player
{
    public GameObject arrow;
    private Queue<Transform> arr;
    private Queue<Vector3> currEnemies;
    private float arrowSpeed = 25f;
    
    private GameObject currEnemy;
    private bool tower;

    protected override void Start()
    {
        arr = new Queue<Transform>();
        currEnemies = new Queue<Vector3>();
        base.Start();
    }

    protected override void Update()
    {
        if (arr.Count > 0 && currEnemies.Count>0) 
        {
            arrowMovement();
            if (Vector3.Distance(arr.Peek().position, currEnemies.Peek()) < 0.01f)
            {
                hitEnemy();
            }
        }
        base.Update();
    }

    protected override void attack(GameObject enemy, bool isTower)
    {
        currEnemies.Enqueue(enemy.transform.position);
        tower = isTower;
        currEnemy = enemy;
        shootArrow();
        base.attack(enemy, isTower);
    }

    private void shootArrow()
    {
        GameObject go;
        go = Instantiate(arrow) as GameObject;
        go.transform.position = transform.position ;
        Vector3 lookPos = currEnemy.transform.position - go.transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        rotation *= Quaternion.Euler(90, 0, 0);
        go.transform.rotation = rotation;
        arr.Enqueue(go.transform);
    }

    private void arrowMovement()
    {
        arr.Peek().position =  Vector3.MoveTowards(arr.Peek().position, currEnemies.Peek(), arrowSpeed * Time.deltaTime);
    }

    private void hitEnemy()
    {
        if (currEnemy != null)
        {
            if (!tower)
                currEnemy.GetComponent<Enemy>().subtractHealth(attackDamage);
            else
                currEnemy.GetComponent<Tower>().subtractHealth(attackDamage);
        }

        currEnemies.Dequeue();
        Destroy(arr.Dequeue().gameObject);   
    }

    protected override void dead()
    {
        if(arr.Count>0)
            Destroy(arr.Dequeue().gameObject);
        base.dead();
    }
}
