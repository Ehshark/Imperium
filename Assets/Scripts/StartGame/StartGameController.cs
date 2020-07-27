using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using ExitGames.Client.Photon;

public class StartGameController : MonoBehaviour
{
    public static StartGameController Instance { get; private set; } = null;
    public bool isTesting = false;
    public GameObject HeroUI { get => heroUI; set => heroUI = value; }
    public int HostChoice { get => EventManager.Instance.HostChoice; set => EventManager.Instance.HostChoice = value; }
    public int ClientChoice { get => EventManager.Instance.ClientChoice; set => EventManager.Instance.ClientChoice = value; }
    public GameObject TutorialUI { get => tutorialUI; set => tutorialUI = value; }
    public GameObject TutorialObject { get => tutorialObject; set => tutorialObject = value; }
    public bool HostFirst { get => EventManager.Instance.HostFirst; set => EventManager.Instance.HostFirst = value; }

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

    //Tutorial
    public bool tutorial;
    [SerializeField]
    private GameObject tutorialUI;
    [SerializeField]
    private GameObject tutorialObject;

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
        if (tutorial)
        {
            StartCoroutine(TutorialSetup());
        }
        else if (isTesting)
        {
            StartCoroutine(ShortGameSetup());
        }
    }

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    private void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        if (eventCode == 5)
        {
            StartCoroutine(MultiplayerSetup());
        }
    }

    private IEnumerator MultiplayerSetup()
    {
        yield return new WaitForSeconds(1f);
        if (PhotonNetwork.IsMasterClient)
        {
            GameManager.Instance.bottomHero.SetPlayerName(PhotonNetwork.NickName + " - Host");
            GameManager.Instance.topHero.SetPlayerName(PhotonNetwork.PlayerListOthers[0].NickName);
            if (EventManager.Instance.HostFirst)
            {
                GameManager.Instance.bottomHero.MyTurn = true;
                GameManager.Instance.topHero.MyTurn = false;
            }
            else
            {
                GameManager.Instance.bottomHero.MyTurn = false;
                GameManager.Instance.topHero.MyTurn = true;
            }

            if (EventManager.Instance.HostChoice == 1)
                GameManager.Instance.bottomHero.SetHero(4, 4, 1, 6, 5, 'W', warriorImage);
            else if (EventManager.Instance.HostChoice == 2)
                GameManager.Instance.bottomHero.SetHero(4, 4, 2, 6, 5, 'R', rogueImage);
            else if (EventManager.Instance.HostChoice == 3)
                GameManager.Instance.bottomHero.SetHero(4, 4, 1, 6, 6, 'M', mageImage);

            if (EventManager.Instance.ClientChoice == 1)
                GameManager.Instance.topHero.SetHero(4, 4, 1, 6, 5, 'W', warriorImage);
            else if (EventManager.Instance.ClientChoice == 2)
                GameManager.Instance.topHero.SetHero(4, 4, 2, 6, 5, 'R', rogueImage);
            else if (EventManager.Instance.ClientChoice == 3)
                GameManager.Instance.topHero.SetHero(4, 4, 1, 6, 6, 'M', mageImage);
        }

        else
        {
            GameManager.Instance.bottomHero.SetPlayerName(PhotonNetwork.NickName);
            GameManager.Instance.topHero.SetPlayerName(PhotonNetwork.PlayerListOthers[0].NickName + " - Host");
            if (EventManager.Instance.HostFirst)
            {
                GameManager.Instance.bottomHero.MyTurn = false;
                GameManager.Instance.topHero.MyTurn = true;
            }
            else
            {
                GameManager.Instance.bottomHero.MyTurn = true;
                GameManager.Instance.topHero.MyTurn = false;
            }

            if (EventManager.Instance.HostChoice == 1)
                GameManager.Instance.topHero.SetHero(4, 4, 1, 6, 5, 'W', warriorImage);
            else if (EventManager.Instance.HostChoice == 2)
                GameManager.Instance.topHero.SetHero(4, 4, 2, 6, 5, 'R', rogueImage);
            else if (EventManager.Instance.HostChoice == 3)
                GameManager.Instance.topHero.SetHero(4, 4, 1, 6, 6, 'M', mageImage);

            if (EventManager.Instance.ClientChoice == 1)
                GameManager.Instance.bottomHero.SetHero(4, 4, 1, 6, 5, 'W', warriorImage);
            else if (EventManager.Instance.ClientChoice == 2)
                GameManager.Instance.bottomHero.SetHero(4, 4, 2, 6, 5, 'R', rogueImage);
            else if (EventManager.Instance.ClientChoice == 3)
                GameManager.Instance.bottomHero.SetHero(4, 4, 1, 6, 6, 'M', mageImage);
        }

        InitialDraw();
        //GameManager.Instance.StartTurn();

        //if (GameManager.Instance.ActiveHero(true).Clan == 'W')
        //{
        //    InstantiateSkillTree();
        //}
    }

    private IEnumerator ShortGameSetup()
    {
        yield return new WaitForSeconds(1f);
        GameManager.Instance.bottomHero.SetPlayerName("Player 1");
        GameManager.Instance.topHero.SetPlayerName("Player 2");
        GameManager.Instance.bottomHero.MyTurn = false;
        GameManager.Instance.topHero.MyTurn = true;
        GameManager.Instance.bottomHero.SetHero(4, 4, 1, 6, 5, 'W', warriorImage);
        //GameManager.Instance.bottomHero.SetHero(4, 4, 2, 6, 5, 'R', rogueImage);
        //GameManager.Instance.bottomHero.SetHero(4, 4, 1, 6, 6, 'M', mageImage);
        GameManager.Instance.topHero.SetHero(4, 4, 1, 6, 6, 'M', mageImage);
        GameManager.Instance.bottomHero.AdjustGold(10, true);
        InitialDraw();
        yield return new WaitForSeconds(3f);
        GameManager.Instance.SwitchTurn();
        UIManager.Instance.ShowHideAttackButton();

        if (GameManager.Instance.ActiveHero(true).Clan == 'W')
        {
            InstantiateSkillTree();
        }
    }

    private IEnumerator TutorialSetup()
    {
        yield return new WaitForSeconds(1f);
        GameManager.Instance.bottomHero.SetPlayerName("Player 1");
        GameManager.Instance.topHero.SetPlayerName("Player 2");
        GameManager.Instance.bottomHero.MyTurn = false;
        GameManager.Instance.topHero.MyTurn = true;
        GameManager.Instance.bottomHero.SetHero(4, 4, 2, 6, 5, 'R', rogueImage);
        GameManager.Instance.topHero.SetHero(4, 4, 1, 6, 6, 'M', mageImage);
        InitialDraw();
        yield return new WaitForSeconds(3f);
        GameManager.Instance.SwitchTurn();
        UIManager.Instance.ShowHideAttackButton();

        //Set Tutorial Stuff
        tutorialUI.SetActive(true);
        GameManager.Instance.ActiveHero(true).AttackButton.Find("AttackIcon").GetComponent<Button>().interactable = false;
        GameManager.Instance.shopButton.interactable = false;
        GameManager.Instance.endButton.interactable = false;
        GameManager.Instance.allyDiscardPileButton.interactable = false;
        GameManager.Instance.enemyDiscardPileButton.interactable = false;
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

        cnt++;

        if (GameManager.Instance.bottomHero.MyTurn)
        {
            if (GameManager.Instance.bottomHero.Clan == 'W')
            {
                GameManager.Instance.WarriorSetup = true;
                InstantiateSkillTree();
            }
            else
            {
                SwitchHeroChoosing();
            }
        }
        else if (GameManager.Instance.topHero.MyTurn)
        {
            if (GameManager.Instance.topHero.Clan == 'W')
            {
                GameManager.Instance.WarriorSetup = true;
                InstantiateSkillTree();
            }
            else
            {
                SwitchHeroChoosing();
            }
        }

        if (cnt == 2)
        {
            HeroUI.SetActive(false);
            reactionText.text = "";

            //Begin the initial draw
            InitialDraw();

            //Determine which player has the mulligan button to show first
            SetActiveMulligan();
        }
    }

    public void SwitchHeroChoosing()
    {
        GameManager.Instance.SwitchTurn();

        if (GameManager.Instance.WarriorSetup)
        {
            GameManager.Instance.WarriorSetup = false;
        }

        if (cnt != 2)
        {
            ShowHeroUI();
        }
    }

    public void InitialDraw()
    {
        Hero activeHero = GameManager.Instance.ActiveHero(true);
        Hero inactiveHero = GameManager.Instance.ActiveHero(false);
        List<Card> activeList = UIManager.Instance.GetActiveDeckList(true);
        List<Card> inactiveList = UIManager.Instance.GetActiveDeckList(false);
        Transform activeHand = GameManager.Instance.GetActiveHand(true);
        Transform inactiveHand = GameManager.Instance.GetActiveHand(false);
        //intial draw for active player
        for (int i = 0; i < activeHero.HandSize; i++)
        {
            GameManager.Instance.DrawCard(activeList, activeHand);
        }

        //intial draw for other player
        for (int i = 0; i < inactiveHero.HandSize; i++)
        {
            GameManager.Instance.DrawCard(inactiveList, inactiveHand);
        }
    }

    public void Mulligan()
    {
        for (int i = 0; i < GameManager.Instance.ActiveHero(true).HandSize; i++)
        {
            UIManager.Instance.GetActiveDeckList(true).Add(UIManager.Instance.GetActiveHandList(true)[i]);
            Destroy(GameManager.Instance.GetActiveHand(true).GetChild(i).gameObject);
        }

        UIManager.Instance.GetActiveHandList(true).Clear();
        GameManager.Instance.ShuffleCurrentDeck(UIManager.Instance.GetActiveDeckList(true));

        for (int i = 0; i < GameManager.Instance.ActiveHero(true).HandSize; i++)
        {
            GameManager.Instance.DrawCard(UIManager.Instance.GetActiveDeckList(true), GameManager.Instance.GetActiveHand(true));
        }

        GameManager.Instance.SwitchTurn();
        mulliganCount++;
        SetActiveMulligan();
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

    public void InstantiateSkillTree()
    {
        GameObject tree = Instantiate(GameManager.Instance.skillTreePrefab);
        tree.transform.SetParent(GameManager.Instance.canvas, false);
    }
}
