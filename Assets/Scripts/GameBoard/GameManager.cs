using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //Player
    public static string UserName { get; set; }
    public static Player player { get; set; } = null;

    public Transform instructionsObj;

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
    private float turnTimer;

    public bool IsPromoting { get => isPromoting; set => isPromoting = value; }
    public GameObject MinionToPromote { get => minionToPromote; set => minionToPromote = value; }
    public float TurnTimer { get => turnTimer; set => turnTimer = value; }
    public GameObject MinionToSacrifice { get => minionToSacrifice; set => minionToSacrifice = value; }

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

    //TODO: Function to disable play card contol 
}
