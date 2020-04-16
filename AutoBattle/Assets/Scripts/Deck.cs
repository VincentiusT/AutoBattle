using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Deck : MonoBehaviour
{
    public LayerMask unwalkable;
    public GameObject[] heroes;
    public Image[] heroesInDeckUI;
    public Image nextHeroUI;
    public TextMeshProUGUI[] heroesInDeckCost;

    private Queue<GameObject> heroesInQueue;
    private List<GameObject> heroesInDeck;
    private Queue<Identity> heroesInQueueIdentity;
    private List<Identity> heroesInDeckIdentity;

    private Vector3 worldPosition;
    private Vector3 putHeroCoord;
    private GameObject pickedHero;
    private bool isPicked=false;
    private bool cannotPickNow = false;
    private int index;

    private void Start()
    {
        heroes = shuffle(heroes.Length,heroes);
        heroesInDeck = new List<GameObject>();
        heroesInQueue = new Queue<GameObject>();
        heroesInDeckIdentity = new List<Identity>();
        heroesInQueueIdentity = new Queue<Identity>();
        for (int i = 0; i < 4; i++)
        {
            heroesInDeck.Add(heroes[i]);
            heroesInDeckIdentity.Add(heroes[i].GetComponent<Player>().playerIdentity);
        }
        for(int i = 4; i < 8; i++)
        {
            heroesInQueue.Enqueue(heroes[i]);
            heroesInQueueIdentity.Enqueue(heroes[i].GetComponent<Player>().playerIdentity);
        }

        showDeck();
    }

    private void Update()
    {

        if (isPicked)
        {
            if (Input.GetMouseButton(0))
            {
                dragAndDropHero();
            }
            if (Input.GetMouseButtonUp(0))
            {
                putHero();
            }
        }
    }

    public void pickHeroes(int index)
    {
        isPicked = true;
        cannotPickNow = false;
        this.index = index;
    }

    private void dragAndDropHero()
    {
        if (!cannotPickNow)
        {
            GameObject go;
            if (heroesInDeck[index] != null)
            {
                go = Instantiate(heroesInDeck[index]) as GameObject;
                pickedHero = go;
                heroesInDeckUI[index].enabled = false;
                heroesInDeckCost[index].enabled = false;
            }
        }

        worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        putHeroCoord = new Vector3(worldPosition.x,0.5f,worldPosition.z);
        if(pickedHero!=null) pickedHero.transform.position = putHeroCoord;
        cannotPickNow = true;
    }

    private void putHero()
    {
        isPicked = false;
        cannotPickNow = false;
        if (pickedHero == null) return;

        if (Physics.OverlapSphere(pickedHero.transform.position, 1f, unwalkable).Length <= 0)
        {
            GameObject temp=null;
            temp = heroesInDeck[index];
            pickedHero.GetComponent<Player>().spawnThisPlayer();
            heroesInDeck[index] = heroesInQueue.Dequeue();
            heroesInDeckIdentity[index] = heroesInQueueIdentity.Dequeue(); 
            heroesInQueue.Enqueue(temp);
            heroesInQueueIdentity.Enqueue(temp.GetComponent<Player>().playerIdentity);
            heroesInDeckUI[index].enabled = true;
            heroesInDeckCost[index].enabled = true;
            changeDeck();
            
        }
        else
        {
            Destroy(pickedHero);
            heroesInDeckUI[index].enabled =true;
            heroesInDeckCost[index].enabled = true;
        }
    }

    private void showDeck()
    {
        for (int i = 0; i < heroesInDeck.Count; i++)
        {
            heroesInDeckUI[i].sprite = heroesInDeckIdentity[i].deckArtwork;
            heroesInDeckCost[index].text = heroesInDeckIdentity[i].cost.ToString("0");
            heroesInDeckUI[i].preserveAspect = true;
        }
        nextHeroUI.sprite = heroesInQueueIdentity.Peek().deckArtwork;
        nextHeroUI.preserveAspect = true;
    }

    private void changeDeck()
    {
        heroesInDeckUI[index].sprite = heroesInDeckIdentity[index].deckArtwork;
        heroesInDeckCost[index].text = heroesInDeckIdentity[index].cost.ToString("0");
        heroesInDeckUI[index].preserveAspect = true;
        nextHeroUI.sprite = heroesInQueueIdentity.Peek().deckArtwork;
        nextHeroUI.preserveAspect = true;
    }

    private GameObject[] shuffle(int x, GameObject[] arr)
    {
        GameObject tempGO;
        for (int i = 0; i < x; i++)
        {
            int rnd = Random.Range(0, x);
            tempGO = arr[i];
            arr[i] = arr[rnd];
            arr[rnd] = tempGO;
        }

        return arr;
    }

}
