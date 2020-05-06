﻿//This script uses magic numbers - the number of ScriptableObjects inside Assets/Resources/Minions/, etc.

using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;
//using Photon.Pun;

public class UIManager : MonoBehaviour
{
    public List<Sprite> allSprites;

    public GameObject minionPrefab;
    public GameObject starterPrefab;
    public GameObject itemPrefab;

    private List<MinionData> minions;
    private List<MinionData> warriorMinions;
    private List<MinionData> rogueMinions;
    private List<MinionData> mageMinions;

    public MinionData currentMinion;
    //public GameObject tempPrefab;

    //public List<MinionData> currentShopCards;
    public List<MinionData> dealtWarriorCards;
    public List<MinionData> dealtRogueCards;
    public List<MinionData> dealtMageCards;

    //public bool shopFull = false;

    private List<StarterData> starters;
    public StarterData currentStarter;
    private List<EssentialsData> essentials;
    public EssentialsData currentEssential;

    private MinionData tempMinion;
    private StarterData tempStarter;
    private EssentialsData tempEssential;

    public Dictionary<int, string> minionConditions;
    public Dictionary<int, string> minionEffects;
    public Dictionary<int, string> minionClasses;

    public static UIManager Instance { get; private set; } = null;

    //public Text yourName;
    //public Text opponentName;

    private void Awake()
    {
        if (Instance)
        {
            DestroyImmediate(gameObject);
            return;
        }

        Instance = this;
        //DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        minionConditions = new Dictionary<int, string>
        {
            {1,"bleed"},
            {2,"buy-first-card"},
            {3,"minion-defeated"},
            {4,"tap"},
            {5,"change-shop"},
            {6,"action-draw"},
            {7,"passive"}
        };

        minionEffects = new Dictionary<int, string>
        {
            {1,"draw-card"},
            {2,"peek-shop"},
            {3,"change-shop"},
            {4,"express-buy"},
            {5,"recycle"},
            {6,"heal-allied-minion"},
            {7,"poison-touch"},
            {8,"stealth"},
            {9,"vigilance"},
            {10,"lifesteal"},
            {11,"untap"},
            {12,"silence"},
            {13,"shock"},
            {14,"buff-allied-minion"},
            {15,"card-discard"},
            {16,"loot"},
            {17,"trash"},
            {18,"coins"},
            {19,"experience"},
            {20,"health"},
            {21,"mana"}
        };

        minionClasses = new Dictionary<int, string>
        {
            {1,"rogue"},
            {2,"warrior"},
            {3,"mage"}
        };

        //Load all the icon sprites to display on the cards
        LoadSprites();

        //Load all the minion scriptable objects from the resources folder and add them to the minions list
        minions = new List<MinionData>();
        for (int i = 0; i < 104; i++)
        {
            tempMinion = Resources.Load("Minions/" + (i + 1)) as MinionData;
            minions.Add(tempMinion);
        }

        //Do the same for the starter cards
        starters = new List<StarterData>();
        for (int i = 0; i < 10; i++)
        {
            tempStarter = Resources.Load("Starters/" + (i + 1)) as StarterData;
            starters.Add(tempStarter);
        }

        //Do the same for the essentials cards
        essentials = new List<EssentialsData>();
        for (int i = 0; i < 10; i++)
        {
            tempEssential = Resources.Load("Essentials/" + (i + 1)) as EssentialsData;
            essentials.Add(tempEssential);
        }

        //Shuffle each list holding the scriptable objects
        Shuffle();
        currentEssential = essentials[0];
        currentMinion = minions[0];
        currentStarter = starters[0];

        //Sort all the minion cards into 3 piles corresponding with their classes: warrior, rogue, mage
        SortPiles();

        dealtWarriorCards = new List<MinionData>();
        dealtRogueCards = new List<MinionData>();
        dealtMageCards = new List<MinionData>();

        //if (PhotonNetwork.IsMasterClient) {
        //    yourName.text = "Host: " + GameManager.UserName;
        //    opponentName.text = "Client: " + PhotonNetwork.PlayerListOthers[0].NickName;
        //}

        //else {
        //    yourName.text = "Client: " + GameManager.UserName;
        //    opponentName.text = "Host: " + PhotonNetwork.PlayerListOthers[0].NickName;
        //}
    }

    public void DisplayShop()
    {
        GameManager.Instance.shop.gameObject.SetActive(true);

        //Sets all 9 cards in the shop, 3 cards per pile
        SetWarriorMinion();
        SetRogueMinion();
        SetMageMinion();
    }

    private void Shuffle()
    {
        for (int i = 0; i < minions.Count; i++)
        {
            int rnd = Random.Range(0, minions.Count);
            tempMinion = minions[rnd];
            minions[rnd] = minions[i];
            minions[i] = tempMinion;
        }

        for (int i = 0; i < starters.Count; i++)
        {
            int rnd = Random.Range(0, starters.Count);
            tempStarter = starters[rnd];
            starters[rnd] = starters[i];
            starters[i] = tempStarter;
        }

        for (int i = 0; i < essentials.Count; i++)
        {
            int rnd = Random.Range(0, essentials.Count);
            tempEssential = essentials[rnd];
            essentials[rnd] = essentials[i];
            essentials[i] = tempEssential;
        }
    }

    void LoadSprites()
    {
        object[] loadedIcons = Resources.LoadAll("VisualAssets/game-icons-net/CardAndHeroAttributes", typeof(Sprite));
        allSprites = new List<Sprite>();
        for (int i = 0; i < loadedIcons.Length; i++)
            allSprites.Add((Sprite)loadedIcons[i]);

        loadedIcons = Resources.LoadAll("VisualAssets/game-icons-net/HeroAbilityConditions", typeof(Sprite));
        for (int i = 0; i < loadedIcons.Length; i++)
            allSprites.Add((Sprite)loadedIcons[i]);

        loadedIcons = Resources.LoadAll("VisualAssets/game-icons-net/MinionConditions", typeof(Sprite));
        for (int i = 0; i < loadedIcons.Length; i++)
            allSprites.Add((Sprite)loadedIcons[i]);

        loadedIcons = Resources.LoadAll("VisualAssets/game-icons-net/Game-UI", typeof(Sprite));
        for (int i = 0; i < loadedIcons.Length; i++)
            allSprites.Add((Sprite)loadedIcons[i]);
    }

    private void SortPiles()
    {
        warriorMinions = new List<MinionData>();
        rogueMinions = new List<MinionData>();
        mageMinions = new List<MinionData>();

        for (int i = 0; i < minions.Count; i++)
        {
            if (minions[i].CardClass.Equals("Warrior"))
            {
                warriorMinions.Add(minions[i]);
            }

            if (minions[i].CardClass.Equals("Rogue"))
            {
                rogueMinions.Add(minions[i]);
            }

            if (minions[i].CardClass.Equals("Mage"))
            {
                mageMinions.Add(minions[i]);
            }
        }
    }

    public void SetWarriorMinion()
    {
        for (int i = 0; i < 3; i++)
        {
            currentMinion = warriorMinions[i];
            dealtWarriorCards.Add(currentMinion);
            MinionVisual mv = GameManager.Instance.warriorShopPile.GetChild(i).GetComponent<MinionVisual>();
            mv.Md = currentMinion;
            GameManager.Instance.warriorShopPile.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void SetRogueMinion()
    {
        for (int i = 0; i < 3; i++)
        {
            currentMinion = rogueMinions[i];
            dealtRogueCards.Add(currentMinion);
            MinionVisual mv = GameManager.Instance.rogueShopPile.GetChild(i).GetComponent<MinionVisual>();
            mv.Md = currentMinion;
            GameManager.Instance.rogueShopPile.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void SetMageMinion()
    {
        for (int i = 0; i < 3; i++)
        {
            currentMinion = mageMinions[i];
            dealtMageCards.Add(currentMinion);
            MinionVisual mv = GameManager.Instance.mageShopPile.GetChild(i).GetComponent<MinionVisual>();
            mv.Md = currentMinion;
            GameManager.Instance.mageShopPile.GetChild(i).gameObject.SetActive(true);
        }
    }
}
