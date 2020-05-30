using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NyiBlorong: Player
{
    public GameObject snakeObject;
    private Queue<Transform> snake;
    private Queue<Vector3> currEnemies;
    private float arrowSpeed = 20f;
    
    private GameObject currEnemy;
    private bool tower;

    protected override void Start()
    {
        snake = new Queue<Transform>();
        currEnemies = new Queue<Vector3>();
        base.Start();
    }

    protected override void Update()
    {
        if (snake.Count > 0 && currEnemies.Count > 0)
        {
            arrowMovement();
            if (Vector3.Distance(snake.Peek().position, currEnemies.Peek()) < 0.01f)
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
        go = Instantiate(snakeObject) as GameObject;
        go.transform.position = transform.position;
        Vector3 lookPos = currEnemy.transform.position - go.transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        rotation *= Quaternion.Euler(90, 0, 0);
        go.transform.rotation = rotation;
        snake.Enqueue(go.transform);
    }

    private void arrowMovement()
    {
        snake.Peek().position = Vector3.MoveTowards(snake.Peek().position, currEnemies.Peek(), arrowSpeed * Time.deltaTime);
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
        Destroy(snake.Dequeue().gameObject);
    }

    protected override void dead()
    {
        if (snake.Count > 0)
            Destroy(snake.Dequeue().gameObject);
        base.dead();
    }
}
