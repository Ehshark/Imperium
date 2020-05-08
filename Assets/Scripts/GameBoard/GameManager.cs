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

    public Hero bottomHero { get; set; }
    public Hero topHero { get; set; }

    private float turnTimer;
    public float TurnTimer { get => turnTimer; set => turnTimer = value; }

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

    public void CleanUpListeners()
    {
        Destroy(enemyMinionZone.gameObject.GetComponent<DestroyMinionListener>());
    }
}
