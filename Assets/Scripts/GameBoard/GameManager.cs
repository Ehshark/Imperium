using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    //Player
    public static string UserName { get; set; }
    public static Player player { get; set; } = null;

    public Transform instructionsObj;

    public Transform StartCombatDamageUI;
    public TMP_Text alliedDamageCounter;
    public TMP_Text alliedStealthDamageCounter;
    public TMP_Text alliedLifestealDamageCounter;
    public TMP_Text alliedPoisonTouchDamageCounter;

    public Transform alliedMinionZone;
    public Transform alliedHand;
    public Transform alliedDeck;
    public Transform alliedDiscardPile;
    public List<GameObject> alliedDiscardPileList;

    public Transform enemyMinionZone;
    public Transform enemyHand;
    public Transform enemyDeck;
    public Transform enemyDiscardPile;
    public List<GameObject> enemyDiscardPileList;

    public Transform shop;
    public Transform warriorShopPile;
    public Transform rogueShopPile;
    public Transform mageShopPile;
    public Transform essentialsPile;

    public Hero bottomHero;
    public Hero topHero;

    public Button buyButton;
    public Button changeButton;

    private bool isPromoting = false;
    private GameObject minionToPromote;
    private GameObject minionToSacrifice;
    private List<GameObject> minionsAttacking = new List<GameObject>();
    private float turnTimer;

    public bool IsPromoting { get => isPromoting; set => isPromoting = value; }
    public GameObject MinionToPromote { get => minionToPromote; set => minionToPromote = value; }
    public float TurnTimer { get => turnTimer; set => turnTimer = value; }
    public GameObject MinionToSacrifice { get => minionToSacrifice; set => minionToSacrifice = value; }
    public List<GameObject> MinionsAttacking { get => minionsAttacking; set => minionsAttacking = value; }

    public static GameManager Instance { get; private set; } = null;
    

    private void Awake()
    {
        if (Instance)
        {
            DestroyImmediate(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        bottomHero.CanPlayCards = true;
        bottomHero.MyTurn = true;
        topHero.CanPlayCards = false;
        topHero.MyTurn = false;
    }

    //TODO:
    //public void CleanUpListeners()
    //{
    //    Destroy(enemyMinionZone.gameObject.GetComponent<SacrificeMinionListener>());
    //}

    public void MoveCard(GameObject go, Transform from, Transform to)
    {
        go.transform.SetParent(to, false);
        go.transform.position = to.position;
        if (from.name.Equals("Hand") || from.name.Equals("EnemyHand"))
            Destroy(go.GetComponent<PlayCard>());
    }

    public void MoveCard(GameObject card, Transform to, List<GameObject> list = null, bool destroy = false)
    {
        GameObject tmp;

        if (card.GetComponent<CardVisual>().Md != null)
        {
            tmp = SpawnCard(null, card.GetComponent<CardVisual>().Md);
        }
        else if (card.GetComponent<CardVisual>().Ed != null)
        {
            tmp = SpawnCard(null, null, card.GetComponent<CardVisual>().Ed);
        }
        else if (card.GetComponent<CardVisual>().Sd != null)
        {
            tmp = SpawnCard(null, null, null, card.GetComponent<CardVisual>().Sd, false);
        }
        else
        {
            tmp = null;
        }
        
        tmp.transform.SetParent(to, false);
        tmp.transform.localScale = new Vector3(1, 1, 1);

        if (list != null)
        {
            list.Add(tmp);
        }

        if (destroy)
        {
            Destroy(card);
        }
    }

    public Hero ActiveHero()
    {
        if (bottomHero.MyTurn)
            return bottomHero;
        else
            return topHero;
    }

    public void EnableOrDisablePlayerControl(bool enable)
    {
        if (enable)
        {
            buyButton.interactable = true;
            changeButton.interactable = true;
            ActiveHero().CanPlayCards = true;
        }
        else
        {
            buyButton.interactable = false;
            changeButton.interactable = false;
            ActiveHero().CanPlayCards = false;
        }
    }

    public void SwitchTurn()
    {
        if (bottomHero.MyTurn)
        {
            bottomHero.MyTurn = false;
            topHero.MyTurn = true;
        }
        else
        {
            topHero.MyTurn = false;
            bottomHero.MyTurn = true;
        }
    }

    public int GetCurrentPlayer()
    {
        int player = 0;

        if (bottomHero.MyTurn)
        {
            player = 0; //bottom player
        }
        else
        {
            player = 1; //top player
        }

        return player;
    }
    //draws a single card, takes in a Card parameter and determines what kind of card it is and instantiate + populates it
    //TODO: change the instatiate prefab for cards to the enemy's hand as well
    //TODO: remove/pop cards out of the deck when drawn
    public void DrawCard(Card topCard)
    {
        GameObject tmp;
        MinionData minion;
        StarterData starter;
        EssentialsData essentials;

        if (UIManager.Instance.allyDeck.Count > 0) //checks if deck is not empty
        {
            if (topCard is MinionData) //Card is a minion
            {
                minion = (MinionData)topCard;
                tmp = Instantiate(UIManager.Instance.minionPrefab, alliedHand) as GameObject;
                tmp.SetActive(false);
                tmp.GetComponent<CardVisual>().Md = minion;
                tmp.SetActive(true);
            }
            else if (topCard is StarterData) //Card is a starter
            {
                starter = (StarterData)topCard;
                tmp = Instantiate(UIManager.Instance.starterPrefab, alliedHand) as GameObject;
                tmp.SetActive(false);
                tmp.GetComponent<CardVisual>().Sd = starter;
                tmp.SetActive(true);
            }
            else if (topCard is EssentialsData) //Card is a essential
            {
                essentials = (EssentialsData)topCard;
                tmp = Instantiate(UIManager.Instance.itemPrefab, alliedHand) as GameObject;
                tmp.SetActive(false);
                tmp.GetComponent<CardVisual>().Ed = essentials;
                tmp.SetActive(true);
            }
        }
        else //no cards left in the deck, add the discard pile, reshuffle and continue the draw
        {
            Debug.Log("no cards in deck, please shuffle in discard pile and continue draw");
            //TODO: add discard pile to deck, shuffle the deck, continue the draw
        }
    }

    public GameObject SpawnCard(Transform to, MinionData minion = null, EssentialsData essential = null, StarterData starter = null, bool inShop = false)
    {
        GameObject tmp;

        if (minion != null)
        {
            tmp = Instantiate(UIManager.Instance.minionPrefab) as GameObject;
            tmp.SetActive(false);
            tmp.GetComponent<CardVisual>().Md = minion;

            if (inShop)
            {
                tmp.GetComponent<CardVisual>().inShop = true;
            }

            tmp.SetActive(true);
            tmp.transform.SetParent(to, false);
            return tmp;
        }
        else if (essential != null)
        {
            tmp = Instantiate(UIManager.Instance.itemPrefab) as GameObject;
            tmp.SetActive(false);
            tmp.GetComponent<CardVisual>().Ed = essential;

            if (inShop)
            {
                tmp.GetComponent<CardVisual>().inShop = true;
            }

            tmp.SetActive(true);
            tmp.transform.SetParent(to, false);
            return tmp;
        }
        else if (starter != null)
        {
            tmp = Instantiate(UIManager.Instance.starterPrefab) as GameObject;
            tmp.SetActive(false);
            tmp.GetComponent<CardVisual>().Sd = starter;
            tmp.SetActive(true);
            tmp.transform.SetParent(to, false);
            return tmp;
        }

        return null;
    }

    //TODO: Function to disable play card contol 
}
