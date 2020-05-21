using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public bool longBoardSetup = true;

    //Player
    public static string UserName { get; set; }
    public static Player player { get; set; } = null;

    public Transform instructionsObj;
    public Transform canvas;

    public Transform StartCombatDamageUI;
    public TMP_Text alliedDamageCounter;
    public TMP_Text alliedStealthDamageCounter;
    public TMP_Text alliedLifestealDamageCounter;
    public TMP_Text alliedPoisonTouchDamageCounter;

    public TMP_Text allyDeckCounter;
    public TMP_Text enemyDeckCounter;

    public Transform allyMulliganButton;
    public Transform enemyMulliganButton;

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
    public Button endButton;
    public Button shopButton;

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
        //buyButton.interactable = enable;
        //changeButton.interactable = enable;
        ActiveHero().CanPlayCards = enable;
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

        UIManager.Instance.HighlightHeroPortraitAndName();
        UIManager.Instance.ShowHideAttackButton();
        UIManager.Instance.GlowCards();
        UIManager.Instance.AttachPlayCard();
        EnableOrDisablePlayerControl(true);
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
    //draws a single card, takes in a list of Card parameter and determines what kind of card it is and instantiate + populates it
    //TODO: change the instatiate prefab for cards to the enemy's hand as well
    //TODO: add deck counter and decrement
    public void DrawCard(List<Card> deck, Transform playerHand)
    {
        GameObject tmp;
        MinionData minion;
        StarterData starter;
        EssentialsData essentials;

        if (deck.Count > 0) //checks if deck is not empty
        {
            if (deck[0] is MinionData) //Card is a minion
            {
                minion = (MinionData)deck[0];
                tmp = Instantiate(UIManager.Instance.minionPrefab, playerHand) as GameObject;
                tmp.SetActive(false);
                tmp.GetComponent<CardVisual>().Md = minion;
                tmp.SetActive(true);
            }

            else if (deck[0] is StarterData) //Card is a starter
            {
                starter = (StarterData)deck[0];
                tmp = Instantiate(UIManager.Instance.starterPrefab, playerHand) as GameObject;
                tmp.SetActive(false);
                tmp.GetComponent<CardVisual>().Sd = starter;
                tmp.SetActive(true);
            }

            else if (deck[0] is EssentialsData) //Card is a essential
            {
                essentials = (EssentialsData)deck[0];
                tmp = Instantiate(UIManager.Instance.itemPrefab, playerHand) as GameObject;
                tmp.SetActive(false);
                tmp.GetComponent<CardVisual>().Ed = essentials;
                tmp.SetActive(true);
            }

            if (playerHand == alliedHand)
            {
                UIManager.Instance.allyHand.Add(deck[0]); //adds cards to the hand, used to remember the cards drawn for the mulligan in order to add them back into the deck
                deck.Remove(deck[0]);
                allyDeckCounter.text = deck.Count.ToString();
            }
            else
            {
                UIManager.Instance.enemyHand.Add(deck[0]);
                deck.Remove(deck[0]);
                enemyDeckCounter.text = deck.Count.ToString();
            }
        }

        else //no cards left in the deck, add the discard pile, reshuffle and continue the draw
        {
            Debug.Log("no cards in deck, please shuffle in discard pile and continue draw");
            //TODO: add discard pile to deck, shuffle the deck, continue the draw
        }

    }

    //Shuffle deck
    public void ShuffleCurrentDeck(List<Card> deck)
    {
        for (int i = 0; i < deck.Count; i++)
        {
            int rnd = Random.Range(0, deck.Count);
            UIManager.Instance.tempCard = deck[rnd];
            deck[rnd] = deck[i];
            deck[i] = UIManager.Instance.tempCard;
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

    public IEnumerator SetInstructionsText(string message)
    {
        TMP_Text text = instructionsObj.GetComponent<TMP_Text>();
        text.text = message;

        yield return new WaitForSeconds(2);

        text.text = "";
    }

    public void ChangeCardColour(GameObject card, Color color)
    {
        CardVisual cv = card.GetComponent<CardVisual>();

        if (cv.Md)
        {
            cv.cardBackground.color = color;
        }
        else if (cv.Sd)
        {
            cv.cardBackground.color = color;
        }
    }

    public int EnableOrDisableChildren(GameObject obj, bool enable, bool enableParent = false)
    {
        if (enableParent)
        {
            obj.SetActive(enable);
        }
        int numChildren = 0;
        GameObject parentObj = obj;
        if (parentObj != null)
        {
            numChildren = parentObj.transform.childCount;
            foreach (Transform t in parentObj.transform)
            {
                t.gameObject.SetActive(enable);
                if (EnableOrDisableChildren(t.gameObject, enable) == 0)
                {
                    continue;
                }
            }
        }
        return numChildren;
    }

    //TODO: Function to disable play card contol 
}
