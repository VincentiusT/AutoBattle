using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gatotkaca : Player
{
   
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void attack(GameObject enemy, bool isTower)
    {
        if (enemy != null)
        {
            if (!isTower)
                enemy.GetComponent<Enemy>().subtractHealth(attackDamage);
            else
                enemy.GetComponent<Tower>().subtractHealth(attackDamage);
        }
        base.attack(enemy, isTower);
    }


    private void hitEnemy()
    {

    }

}
