using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Deck : MonoBehaviour
{
    public GameObject hitAreaCircle;
    public LayerMask unwalkable;
    public GameObject[] cover;
    public Image[] heroesInDeckUI;
    public Image nextHeroUI;
    public TextMeshProUGUI[] heroesInDeckCost;
    public TextMeshProUGUI coinText;
    public GameObject limit;

    public GameObject[] heroes;
    private Queue<GameObject> heroesInQueue;
    private List<GameObject> heroesInDeck;
    private Queue<Identity> heroesInQueueIdentity;
    private List<Identity> heroesInDeckIdentity;

    private float coin = 100;
    private int originalCoin;
    private Vector3 worldPosition;
    private Vector3 putHeroCoord;
    private GameObject pickedHero;
    private float hitAreaSize;
    private bool isPicked=false;
    private bool cannotPickNow = false;
    private int index;
    private bool thereIsHero;
    private Color cannotPutHeroColor = new Color(1,0,0,0.2f);
    private Color normalPutHeroColor = new Color(1,1,1,0.2f);

    private void Start()
    {
        heroes = shuffle(heroes.Length,heroes);
        heroesInDeck = new List<GameObject>();
        heroesInQueue = new Queue<GameObject>();
        heroesInDeckIdentity = new List<Identity>();
        heroesInQueueIdentity = new Queue<Identity>();
        originalCoin = (int)coin;
        coin = 0;
        for (int i = 0; i < 4; i++)
        {
            heroesInDeck.Add(heroes[i]);
            heroesInDeckIdentity.Add(heroes[i].GetComponent<Player>().playerIdentity);
            Debug.Log(heroes[i].name);
        }
        for(int i = 4; i < heroes.Length; i++)
        {
            heroesInQueue.Enqueue(heroes[i]);
            heroesInQueueIdentity.Enqueue(heroes[i].GetComponent<Player>().playerIdentity);
            Debug.Log(heroes[i].name);
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
            if (Input.GetMouseButtonUp(0) && thereIsHero)
            {
                putHero();
            }
        }

        checkEnoughCoin();

        if(coin < originalCoin)
        {
            coin += Time.deltaTime * 5;
            coinText.text = coin.ToString("0");
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
        int cst = heroesInDeckIdentity[index].cost;
        if (cst > coin)
        {
            thereIsHero = false;
            return;
        }
        limit.SetActive(true);
        thereIsHero = true;
        if (!cannotPickNow)
        {
            GameObject go;
            if (heroesInDeck[index] != null)
            {
                go = Instantiate(heroesInDeck[index]) as GameObject;
                pickedHero = go;
                heroesInDeckUI[index].enabled = false;
                heroesInDeckCost[index].enabled = false;
                hitAreaSize = pickedHero.GetComponent<Player>().playerIdentity.attackRadius / 5f;
                hitAreaCircle.SetActive(true);
                hitAreaCircle.transform.localScale = new Vector3(hitAreaSize, hitAreaSize,hitAreaSize);
            }
        }
        worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        putHeroCoord = new Vector3(worldPosition.x,0.5f,worldPosition.z);

        if (hitAreaCircle.activeSelf)
        {
            hitAreaCircle.transform.position = putHeroCoord;
            if(Physics.OverlapSphere(pickedHero.transform.position, 1f, unwalkable).Length > 0)
                hitAreaCircle.GetComponent<SpriteRenderer>().color = cannotPutHeroColor;
            else
                hitAreaCircle.GetComponent<SpriteRenderer>().color = normalPutHeroColor;
        }
        if(pickedHero!=null) pickedHero.transform.position = putHeroCoord;
        cannotPickNow = true;
    }

    private void putHero()
    {
        if (pickedHero == null) { limit.SetActive(false) ; return; }
        if (Physics.OverlapSphere(pickedHero.transform.position, 0.5f, unwalkable).Length <= 0)
        {
            int cst = heroesInDeckIdentity[index].cost;
            coin -= cst;
            isPicked = false;
            cannotPickNow = false;
            GameObject temp=null;
            temp = heroesInDeck[index];
            StartCoroutine(pickedHero.GetComponent<Player>().spawnThisPlayer());
            heroesInDeck[index] = heroesInQueue.Dequeue();
            heroesInDeckIdentity[index] = heroesInQueueIdentity.Dequeue(); 
            heroesInQueue.Enqueue(temp);
            heroesInQueueIdentity.Enqueue(temp.GetComponent<Player>().playerIdentity);
            heroesInDeckUI[index].enabled = true;
            heroesInDeckCost[index].enabled = true;
            hitAreaCircle.SetActive(false);
            changeDeck();
            
        }
        else
        {
            hitAreaCircle.SetActive(false);
            Destroy(pickedHero);
            heroesInDeckUI[index].enabled =true;
            heroesInDeckCost[index].enabled = true;
        }
        limit.SetActive(false);
    }

    private void showDeck()
    {
        for (int i = 0; i < heroesInDeck.Count; i++)
        {
            heroesInDeckUI[i].sprite = heroesInDeckIdentity[i].deckArtwork;
            heroesInDeckCost[i].text = heroesInDeckIdentity[i].cost.ToString("0");
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

    private void checkEnoughCoin()
    {
        for(int i=0; i<heroesInDeckIdentity.Count ;i++)
        {
            if(heroesInDeckIdentity[i].cost > coin)
            {
                cover[i].SetActive(true);
            }
            else
            {
                cover[i].SetActive(false);
            }
        }
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
