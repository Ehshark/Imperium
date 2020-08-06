using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;
using System.Linq;

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
    public bool StartingPowerSelected { get => startingPowerSelected; set => startingPowerSelected = value; }
    public bool FirstTurn { get => firstTurn; set => firstTurn = value; }

    //Components
    [SerializeField]
    private GameObject coin;
    [SerializeField]
    private GameObject coinToss;
    [SerializeField]
    private TextMeshProUGUI reactionText;

    public GameObject overallUI;
    [SerializeField]
    private GameObject heroUI;
    [SerializeField]
    private GameObject warrior;
    [SerializeField]
    private GameObject rogue;
    [SerializeField]
    private GameObject mage;

    private bool firstTurn = true;

    public Sprite warriorImage;
    public Sprite rogueImage;
    public Sprite mageImage;

    public GameObject bottomHero;
    public GameObject topHero;

    [SerializeField]
    private Button tailsButton;
    [SerializeField]
    private Button headsButton;

    //Tutorial
    public bool tutorial;
    [SerializeField]
    private GameObject tutorialUI;
    [SerializeField]
    private GameObject tutorialObject;

    //Multiplayer
    private bool startingPowerSelected = false;
    const byte MULLIGAN_EVENT = 6;
    const byte MULLIGAN_REFUSED_EVENT = 7;
    private bool bottomMullReady = false;
    private bool topMullReady = false;

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
        if (eventCode == 5) //start multiplayer match
        {
            StartCoroutine(MultiplayerSetup());
        }
        else if (eventCode == MULLIGAN_EVENT)
        {
            foreach (Transform child in GameManager.Instance.enemyHand)
                Destroy(child.gameObject);
            UIManager.Instance.enemyDeck = new List<Card>();
            UIManager.Instance.enemyHand.Clear();

            object[] data = (object[])photonEvent.CustomData;
            List<StarterDataPhoton> enemySdp = (List<StarterDataPhoton>)DataHandler.Instance.ByteArrayToObject((byte[])data[0]);
            for (int i = 0; i < enemySdp.Count; i++)
            {
                UIManager.Instance.enemyDeck.Add(ScriptableObject.CreateInstance<StarterData>());
                ((StarterData)UIManager.Instance.enemyDeck[i]).Init(enemySdp[i]);
            }

            for (int i = 0; i < GameManager.Instance.topHero.HandSize; i++)
                GameManager.Instance.DrawCard(UIManager.Instance.enemyDeck, GameManager.Instance.enemyHand);
            topMullReady = true;
        }
        else if (eventCode == MULLIGAN_REFUSED_EVENT)
        {
            topMullReady = true;
        }
        else if (eventCode == 20) //sync starting power selection
        {
            object[] data = (object[])photonEvent.CustomData;
            int powerCount = (int)data[0];
            int selection = (int)data[1];
            Hero hero = GameManager.Instance.topHero;
            foreach (KeyValuePair<int, string> entry in UIManager.Instance.minionEffects)
            {
                if (selection == entry.Key)
                {
                    if (powerCount == 1)
                    {
                        hero.GetComponent<Hero>().Ability1.sprite = UIManager.Instance.allSprites.Where(x => x.name == entry.Value).SingleOrDefault();
                        hero.GetComponent<Hero>().Ability1.gameObject.SetActive(true);
                    }
                    else if (powerCount == 2)
                    {
                        hero.GetComponent<Hero>().Ability2.sprite = UIManager.Instance.allSprites.Where(x => x.name == entry.Value).SingleOrDefault();
                        hero.GetComponent<Hero>().Ability2.gameObject.SetActive(true);
                    }
                    else if (powerCount == 3)
                    {
                        hero.GetComponent<Hero>().Ability3.sprite = UIManager.Instance.allSprites.Where(x => x.name == entry.Value).SingleOrDefault();
                        hero.GetComponent<Hero>().Ability3.gameObject.SetActive(true);
                    }
                }
            }

            startingPowerSelected = true;
            GameManager.Instance.instructionsObj.GetComponent<TMP_Text>().text = "";
        }
        else if (eventCode == 16)
        {
            GameManager.Instance.instructionsObj.GetComponent<TMP_Text>().text = "Opponent Leveled Up! Power Selection is Progress...";

            Hero hero = GameManager.Instance.ActiveHero(true);
            hero.IncreaseLevel(1);
            hero.IncreaseExp(6);
        }
        else if (eventCode == 32)
            firstTurn = false;
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
                GameManager.Instance.buyButton.interactable = false;
                GameManager.Instance.changeButton.interactable = false;
                GameManager.Instance.endButton.interactable = false;
            }

            if (EventManager.Instance.HostChoice == 1)
            {
                GameManager.Instance.bottomHero.SetHero(4, 4, 1, 6, 5, 'W', warriorImage);
                GameManager.Instance.bottomHero.Abilities.GetChild(2).gameObject.SetActive(true);
            }
            else if (EventManager.Instance.HostChoice == 2)
                GameManager.Instance.bottomHero.SetHero(4, 4, 2, 6, 5, 'R', rogueImage);
            else if (EventManager.Instance.HostChoice == 3)
                GameManager.Instance.bottomHero.SetHero(4, 4, 1, 6, 6, 'M', mageImage);

            if (EventManager.Instance.ClientChoice == 1)
            {
                GameManager.Instance.topHero.SetHero(4, 4, 1, 6, 5, 'W', warriorImage);
                GameManager.Instance.topHero.Abilities.GetChild(2).gameObject.SetActive(true);
            }
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
                GameManager.Instance.buyButton.interactable = false;
                GameManager.Instance.changeButton.interactable = false;
                GameManager.Instance.endButton.interactable = false;
            }
            else
            {
                GameManager.Instance.bottomHero.MyTurn = true;
                GameManager.Instance.topHero.MyTurn = false;
            }

            if (EventManager.Instance.HostChoice == 1)
            {
                GameManager.Instance.topHero.SetHero(4, 4, 1, 6, 5, 'W', warriorImage);
                GameManager.Instance.topHero.Abilities.GetChild(2).gameObject.SetActive(true);
            }
            else if (EventManager.Instance.HostChoice == 2)
                GameManager.Instance.topHero.SetHero(4, 4, 2, 6, 5, 'R', rogueImage);
            else if (EventManager.Instance.HostChoice == 3)
                GameManager.Instance.topHero.SetHero(4, 4, 1, 6, 6, 'M', mageImage);

            if (EventManager.Instance.ClientChoice == 1)
            {
                GameManager.Instance.bottomHero.SetHero(4, 4, 1, 6, 5, 'W', warriorImage);
                GameManager.Instance.bottomHero.Abilities.GetChild(2).gameObject.SetActive(true);
            }
            else if (EventManager.Instance.ClientChoice == 2)
                GameManager.Instance.bottomHero.SetHero(4, 4, 2, 6, 5, 'R', rogueImage);
            else if (EventManager.Instance.ClientChoice == 3)
                GameManager.Instance.bottomHero.SetHero(4, 4, 1, 6, 6, 'M', mageImage);
        }

        InitialDraw();
        yield return new WaitForSeconds(2.5f);
        GameManager.Instance.mulliganButtons.gameObject.SetActive(true);
        yield return new WaitUntil(() => (bottomMullReady == true && topMullReady == true));
        //GameManager.Instance.instructionsObj.GetComponent<TMP_Text>().text = "Both Mulligans Finished";

        if (GameManager.Instance.bottomHero.Clan == 'W' || GameManager.Instance.topHero.Clan == 'W')
        {
            if (GameManager.Instance.bottomHero.Clan == 'W')
            {
                InstantiateSkillTree();
            }
            else if (GameManager.Instance.topHero.Clan == 'W')
            {
                GameManager.Instance.instructionsObj.GetComponent<TMP_Text>().text = "Opponent is choosing a power...";
            }
            yield return new WaitUntil(() => startingPowerSelected == true);
        }

        GameManager.Instance.instructionsObj.GetComponent<TMP_Text>().text = "";

        if (PhotonNetwork.IsMasterClient && EventManager.Instance.HostFirst)
            GameManager.Instance.StartTurn();
        else if (!PhotonNetwork.IsMasterClient && !EventManager.Instance.HostFirst)
            GameManager.Instance.StartTurn();
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
        UIManager.Instance.ShuffleStarterDeck();
        UIManager.Instance.allyDeck = new List<Card>();

        foreach (StarterData s in UIManager.Instance.Starters)
            UIManager.Instance.allyDeck.Add(s);

        List<StarterDataPhoton> allySdp = new List<StarterDataPhoton>();
        foreach (StarterData s in UIManager.Instance.allyDeck)
            allySdp.Add(new StarterDataPhoton(s));

        foreach (Transform child in GameManager.Instance.alliedHand)
            Destroy(child.gameObject);

        UIManager.Instance.allyHand.Clear();

        for (int i = 0; i < GameManager.Instance.bottomHero.HandSize; i++)
            GameManager.Instance.DrawCard(UIManager.Instance.allyDeck, GameManager.Instance.alliedHand);

        bottomMullReady = true;

        byte[] allySdpByte = DataHandler.Instance.ObjectToByteArray(allySdp);
        object[] data = new object[] { allySdpByte };
        PhotonNetwork.RaiseEvent(MULLIGAN_EVENT, data, new RaiseEventOptions { Receivers = ReceiverGroup.Others },
            SendOptions.SendReliable);
        GameManager.Instance.instructionsObj.GetComponent<TMP_Text>().text = "Waiting for opponent...";
        GameManager.Instance.mulliganButtons.gameObject.SetActive(false);
    }

    public void MulliganRefused()
    {
        bottomMullReady = true;
        PhotonNetwork.RaiseEvent(MULLIGAN_REFUSED_EVENT, null, new RaiseEventOptions { Receivers = ReceiverGroup.Others },
            SendOptions.SendReliable);
        GameManager.Instance.mulliganButtons.gameObject.SetActive(false);
        PhotonNetwork.RaiseEvent(MULLIGAN_REFUSED_EVENT, null, new RaiseEventOptions { Receivers = ReceiverGroup.Others },
            SendOptions.SendReliable);
        GameManager.Instance.instructionsObj.GetComponent<TMP_Text>().text = "Waiting for opponent...";
    }

    public void InstantiateSkillTree()
    {
        GameObject tree = Instantiate(GameManager.Instance.skillTreePrefab);
        tree.transform.SetParent(GameManager.Instance.canvas, false);
    }

    private void DetermineGameBackground() {
        UIManager.Instance.ChosenBack = Random.Range(0, 9);

    }
}
