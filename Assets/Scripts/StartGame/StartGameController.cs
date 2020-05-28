using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StartGameController : MonoBehaviour
{
    public static StartGameController Instance { get; private set; } = null;
    public GameObject HeroUI { get => heroUI; set => heroUI = value; }

    //Components
    [SerializeField]
    private GameObject coin;
    [SerializeField]
    private GameObject coinToss;
    [SerializeField]
    private TextMeshProUGUI reactionText;
    private Animator animator;

    public GameObject overallUI;
    [SerializeField]
    private GameObject heroUI;
    [SerializeField]
    private GameObject warrior;
    [SerializeField]
    private GameObject rogue;
    [SerializeField]
    private GameObject mage;

    public Sprite warriorImage;
    public Sprite rogueImage;
    public Sprite mageImage;

    public GameObject bottomHero;
    public GameObject topHero;

    private int turn = 0;
    private int cnt = 0;
    private int mulliganCount = 0;

    [SerializeField]
    private Button tailsButton;
    [SerializeField]
    private Button headsButton;

    //Variables 
    private enum Coin { IDLE, TAILS, HEADS }
    private Coin coinValue;

    private void Awake()
    {
        if (Instance)
        {
            DestroyImmediate(gameObject);
            return;
        }

        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.longBoardSetup)
        {
            GameManager.Instance.EnableOrDisableChildren(GameManager.Instance.canvas.gameObject, false);
            GameManager.Instance.alliedDeck.parent.gameObject.SetActive(true);
            GameManager.Instance.enemyDeck.parent.gameObject.SetActive(true);
            GameManager.Instance.EnableOrDisableChildren(overallUI, true);
            heroUI.SetActive(false);
            SetupGameBoard();
        }
        else
        {
            StartCoroutine(ShortGameSetup());
        }
    }

    private IEnumerator ShortGameSetup()
    {
        yield return new WaitForSeconds(1f);
        GameManager.Instance.bottomHero.SetPlayerName("Player 1");
        GameManager.Instance.topHero.SetPlayerName("Player 2");
        GameManager.Instance.bottomHero.MyTurn = false;
        GameManager.Instance.topHero.MyTurn = true;
        GameManager.Instance.bottomHero.SetHero(4, 4, 1, 6, 5, 'W', warriorImage);
        GameManager.Instance.topHero.SetHero(4, 4, 1, 6, 6, 'M', mageImage);
        GameManager.Instance.bottomHero.AdjustGold(10, true);
        GameManager.Instance.SwitchTurn();
        UIManager.Instance.ShowHideAttackButton();
        InitialDraw();
        UIManager.Instance.AttachPlayCard();
    }

    public void SetupGameBoard()
    {
        //Get the animator component in Coin
        animator = coin.GetComponent<Animator>();

        //Generate a random number and see which Player goes first
        int value = Random.Range(0, 10000);

        if (value % 2 == 0)
        {
            reactionText.text = "Player 1 Chooses...";
            turn = 0;
        }
        else
        {
            reactionText.text = "Player 2 Chooses...";
            turn = 1;
        }

        coinValue = Coin.IDLE;

        //Setup each Player
        GameManager.Instance.bottomHero = bottomHero.GetComponent<Hero>();
        GameManager.Instance.topHero = topHero.GetComponent<Hero>();
        GameManager.Instance.bottomHero.SetPlayerName("Player 1");
        GameManager.Instance.topHero.SetPlayerName("Player 2");

        overallUI.gameObject.SetActive(true);
    }

    public void FlipCoin(int value)
    {
        tailsButton.interactable = false;
        headsButton.interactable = false;

        StartCoroutine(FlipCoinAnimation(value));
    }

    IEnumerator FlipCoinAnimation(int value)
    {
        animator.SetBool("flip", true);
        yield return new WaitForSeconds(2.5f);
        animator.SetBool("flip", false);

        if (turn == 0)
        {
            if ((int)coinValue == value)
            {
                reactionText.text = "You Win!";
                GameManager.Instance.bottomHero.MyTurn = true;
                GameManager.Instance.topHero.MyTurn = false;
            }
            else
            {
                reactionText.text = "You Lose!";
                GameManager.Instance.topHero.MyTurn = true;
                GameManager.Instance.bottomHero.MyTurn = false;
            }
        }
        else
        {
            if ((int)coinValue == value)
            {
                reactionText.text = "You Win!";
                GameManager.Instance.topHero.MyTurn = true;
                GameManager.Instance.bottomHero.MyTurn = false;
            }
            else
            {
                reactionText.text = "You Lose!";
                GameManager.Instance.bottomHero.MyTurn = true;
                GameManager.Instance.topHero.MyTurn = false;
            }
        }

        yield return new WaitForSeconds(2f);

        coinToss.SetActive(false);
        reactionText.text = "";
        ShowHeroUI();
    }

    public int GetCoinValue()
    {
        int value = Random.Range(0, 1000);
        int result;

        if (value % 2 == 0)
        {
            result = 0;
            coinValue = Coin.TAILS;
        }
        else
        {
            result = 1;
            coinValue = Coin.HEADS;
        }

        return result;
    }

    private void ShowHeroUI()
    {
        if (!HeroUI.activeSelf)
        {
            HeroUI.SetActive(true);
        }

        if (GameManager.Instance.bottomHero.MyTurn)
        {
            reactionText.text = "Player 1 Chooses....";
        }
        else
        {
            reactionText.text = "Player 2 Chooses....";
        }
    }

    public void SetHero(int hero)
    {
        GameManager.Instance.alliedHand.gameObject.SetActive(true);
        GameManager.Instance.alliedMinionZone.gameObject.SetActive(true);
        GameManager.Instance.enemyHand.gameObject.SetActive(true);
        GameManager.Instance.enemyMinionZone.gameObject.SetActive(true);

        //Warrior
        if (hero == 0)
        {
            if (GameManager.Instance.bottomHero.MyTurn)
            {

                GameManager.Instance.bottomHero.SetHero(4, 4, 1, 6, 5, 'W', warriorImage);
            }
            else
            {
                GameManager.Instance.topHero.SetHero(4, 4, 1, 6, 5, 'W', warriorImage);
            }

            warrior.SetActive(false);
        }
        //Rogue
        else if (hero == 1)
        {
            if (GameManager.Instance.bottomHero.MyTurn)
            {
                GameManager.Instance.bottomHero.SetHero(4, 4, 2, 6, 5, 'R', rogueImage);
            }
            else
            {
                GameManager.Instance.topHero.SetHero(4, 4, 2, 6, 5, 'R', rogueImage);
            }

            rogue.SetActive(false);
        }
        //Mage 
        else
        {
            if (GameManager.Instance.bottomHero.MyTurn)
            {
                GameManager.Instance.bottomHero.SetHero(4, 4, 1, 6, 6, 'M', mageImage);
            }
            else
            {
                GameManager.Instance.topHero.SetHero(4, 4, 1, 6, 6, 'M', mageImage);
            }

            mage.SetActive(false);
        }

        GameManager.Instance.SwitchTurn();
        cnt++;

        if (cnt == 2)
        {
            HeroUI.SetActive(false);
            reactionText.text = "";

            //Begin the initial draw
            InitialDraw();

            GameManager.Instance.EnableOrDisableChildren(bottomHero.gameObject, true, true);
            GameManager.Instance.EnableOrDisableChildren(topHero.gameObject, true, true);
            GameManager.Instance.EnableOrDisableChildren(GameManager.Instance.topHero.AttackButton.parent.gameObject, false);
            GameManager.Instance.EnableOrDisableChildren(GameManager.Instance.bottomHero.AttackButton.parent.gameObject, false);
            GameManager.Instance.EnableOrDisableChildren(GameManager.Instance.alliedDeck.gameObject, true, true);
            GameManager.Instance.EnableOrDisableChildren(GameManager.Instance.enemyDeck.gameObject, true, true);
            GameManager.Instance.EnableOrDisableChildren(GameManager.Instance.shopButton.gameObject, true, true);
            GameManager.Instance.EnableOrDisableChildren(GameManager.Instance.endButton.gameObject, true, true);

            //Determine which player has the mulligan button to show first
            SetActiveMulligan();
        }
        else
        {
            ShowHeroUI();
        }
    }

    public void InitialDraw()
    {
        //intial draw for bottom player
        for (int i = 0; i < 5; i++)
        {
            GameManager.Instance.DrawCard(UIManager.Instance.allyDeck, GameManager.Instance.alliedHand);
        }

        //intial draw for top player
        for (int i = 0; i < 5; i++)
        {
            GameManager.Instance.DrawCard(UIManager.Instance.enemyDeck, GameManager.Instance.enemyHand);
        }
    }

    public void Mulligan()
    { 
        if (GameManager.Instance.GetCurrentPlayer() == 0)
        {
            for (int i = 0; i < 5; i++)
            {
                UIManager.Instance.allyDeck.Add(UIManager.Instance.allyHand[i]);
                Destroy(GameManager.Instance.alliedHand.GetChild(i).gameObject);
            }

            UIManager.Instance.allyHand.Clear();
            GameManager.Instance.ShuffleCurrentDeck(UIManager.Instance.allyDeck);

            for (int i = 0; i < 5; i++)
            {
                GameManager.Instance.DrawCard(UIManager.Instance.allyDeck, GameManager.Instance.alliedHand);
            }
            GameManager.Instance.SwitchTurn();
            mulliganCount++;
            SetActiveMulligan();
        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                UIManager.Instance.enemyDeck.Add(UIManager.Instance.enemyHand[i]);
                Destroy(GameManager.Instance.enemyHand.GetChild(i).gameObject);
            }

            UIManager.Instance.enemyHand.Clear();
            GameManager.Instance.ShuffleCurrentDeck(UIManager.Instance.enemyDeck);

            for (int i = 0; i < 5; i++)
            {
                GameManager.Instance.DrawCard(UIManager.Instance.enemyDeck, GameManager.Instance.enemyHand);
            }
            GameManager.Instance.SwitchTurn();
            mulliganCount++;
            SetActiveMulligan();
        }
    }

    public void SetActiveMulligan()
    {
        if (mulliganCount < 2)
        {
            if (GameManager.Instance.GetCurrentPlayer() == 0)
            {
                GameManager.Instance.allyMulliganButton.gameObject.SetActive(true);
            }

            else
            {
                GameManager.Instance.enemyMulliganButton.gameObject.SetActive(true);
            }
        }

        else
        {
            GameManager.Instance.allyMulliganButton.gameObject.SetActive(false);
            GameManager.Instance.enemyMulliganButton.gameObject.SetActive(false);
            UIManager.Instance.AttachPlayCard();

        }
    }
}
