using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Player Identity", menuName ="PlayerIdentity")]
public class Identity : ScriptableObject
{
    public new string name;

    public Sprite deckArtwork;

    public int cost;
    public float health;
    public float speed;
    public float attack;
    public float attackSpeed;
    public float attackRadius;
    public float spawnTime;
    public int type;
}
