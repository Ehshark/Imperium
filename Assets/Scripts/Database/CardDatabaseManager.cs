﻿//This script uses magic numbers - the number of ScriptableObjects inside Assets/Resources/Minions/, etc.

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class CardDatabaseManager : MonoBehaviour
{
    public List<Sprite> allSprites;

    public GameObject minionPrefab;
    public GameObject starterPrefab;
    public GameObject itemPrefab;

    private List<MinionData> warriorMinions;
    private List<MinionData> rogueMinions;
    private List<MinionData> mageMinions;

    private MinionData currentMinion;
    private EssentialsData currentEssential;
    private StarterData currentStarter;

    private List<MinionData> dealtWarriorCards;
    private List<MinionData> dealtRogueCards;
    private List<MinionData> dealtMageCards;

    private List<StarterData> allyStarters;
    private List<StarterData> enemyStarters;

    public List<StarterData> starters;
    public List<EssentialsData> essentials;
    public List<MinionData> minions;

    public List<Card> allyDeck; //decklist to test adding starters
    public List<Card> enemyDeck;
    public List<Card> allyHand; //list of cards in hand
    public List<Card> enemyHand;
    public List<Card> allyDiscards;
    public List<Card> enemyDiscards;

    private MinionData tempMinion;
    private StarterData tempStarter;
    private EssentialsData tempEssential;
    public Card tempCard;

    private bool allyDiscardClosed = true;
    private bool enemyDiscardClosed = true;

    public Dictionary<int, string> minionConditions;
    public Dictionary<int, string> minionEffects;
    public Dictionary<int, string> minionClasses;

    public static CardDatabaseManager Instance { get; private set; } = null;

    private GameObject lastSelectedCard;
    public GameObject LastSelectedCard { get => lastSelectedCard; set => lastSelectedCard = value; }

    public Transform enlargedCard;

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

        LoadScriptableObjectsToLists();

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

        //Set the starter cards for both players
        SetStarterDeck();

        //Sets all 9 cards in the shop, 3 cards per pile
        SetWarriorMinion();
        SetRogueMinion();
        SetMageMinion();
        SetEssentials();
    }

    public void LoadScriptableObjectsToLists()
    {
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
        for (int i = 0; i < 4; i++)
        {
            tempEssential = Resources.Load("Essentials/" + (i + 1)) as EssentialsData;
            essentials.Add(tempEssential);
        }
    }

    public void GlowCards()
    {
        Hero activeHero = GameManager.Instance.ActiveHero(true);
        Transform activeHand = GameManager.Instance.GetActiveHand(true);
        int heroManaCost = activeHero.CurrentMana;
        int mvCost;
        CardVisual cv;
        Card data;

        foreach (Transform m in activeHand)
        {
            if (m != null)
            {
                cv = m.GetComponent<CardVisual>();
                data = cv.GetCardData();
                mvCost = data.GoldAndManaCost;

                //if the card is a health potion, check whether health is already full
                if (data.EffectId1 == 20)
                {
                    if (activeHero.CurrentHealth == activeHero.TotalHealth)
                        m.Find("GlowPanel").gameObject.SetActive(false);
                    else
                        m.Find("GlowPanel").gameObject.SetActive(true);
                }
                //if the card is a mana potion, check whether mana is already full
                else if (data.EffectId1 == 21)
                {
                    if (heroManaCost == activeHero.TotalMana)
                        m.Find("GlowPanel").gameObject.SetActive(false);
                    else
                        m.Find("GlowPanel").gameObject.SetActive(true);
                }
                //else the card is not a potion, so check if its cost is less than current mana
                else
                {
                    if (mvCost <= heroManaCost)
                    {
                        m.Find("GlowPanel").gameObject.SetActive(true);
                    }
                    else
                    {
                        m.Find("GlowPanel").gameObject.SetActive(false);
                    }
                }
            }
        }
        foreach (Transform m in GameManager.Instance.GetActiveHand(false))
        {
            if (m != null)
            {
                m.Find("GlowPanel").gameObject.SetActive(false);
            }
        }
    }

    public void AttachPlayCard()
    {
        foreach (Transform m in GameManager.Instance.GetActiveHand(true))
        {
            m.gameObject.AddComponent<PlayCard>();
        }
    }

    public void DisplayShop()
    {
        GameManager.Instance.shop.gameObject.SetActive(true);
    }


    public void DisplayOption()
    {
        Debug.Log(GameManager.Instance.menu.gameObject);
        if (GameManager.Instance.menu.gameObject.activeSelf)
        {
            GameManager.Instance.menu.gameObject.SetActive(false);
        }
        else
        {
            GameManager.Instance.menu.gameObject.SetActive(true);
        }

    }

    public void optionMenu()
    {
        GameManager.Instance.pMenu.gameObject.SetActive(false);
        GameManager.Instance.option.gameObject.SetActive(true);
    }

    public void optionMenuBack()
    {

        GameManager.Instance.pMenu.gameObject.SetActive(true);
        GameManager.Instance.option.gameObject.SetActive(false);
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
    }

    public void LoadSprites()
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

    public void SortPiles()
    {
        warriorMinions = new List<MinionData>();
        rogueMinions = new List<MinionData>();
        mageMinions = new List<MinionData>();

        for (int i = 0; i < minions.Count; i++)
        {
            if (minions[i].CardClass.Equals("Warrior"))
                warriorMinions.Add(minions[i]);

            if (minions[i].CardClass.Equals("Rogue"))
                rogueMinions.Add(minions[i]);

            if (minions[i].CardClass.Equals("Mage"))
                mageMinions.Add(minions[i]);
        }
    }

    public void SetWarriorMinion()
    {
        //TODO: FIND REAL NUM FOR RMW
        for (int i = 0; i < 25; i++)
        {
            currentMinion = warriorMinions[i];
            dealtWarriorCards.Add(currentMinion);
            CardVisual mv = GameManager.Instance.warriorShopPile.GetChild(i).GetComponent<CardVisual>();
            mv.Md = currentMinion;
            GameManager.Instance.warriorShopPile.GetChild(i).gameObject.SetActive(true);
            //Pop added
            warriorMinions.Remove(warriorMinions[i]);
        }
    }
    //TODO: FIND REAL NUM FOR RMW

    public void SetRogueMinion()
    {
        for (int i = 0; i < 25; i++)
        {
            currentMinion = rogueMinions[i];
            dealtRogueCards.Add(currentMinion);
            CardVisual mv = GameManager.Instance.rogueShopPile.GetChild(i).GetComponent<CardVisual>();
            mv.Md = currentMinion;
            GameManager.Instance.rogueShopPile.GetChild(i).gameObject.SetActive(true);
            //Pop added
            rogueMinions.Remove(rogueMinions[i]);
        }
    }
    //TODO: FIND REAL NUM FOR RMW

    public void SetMageMinion()
    {
        for (int i = 0; i < 25; i++)
        {
            currentMinion = mageMinions[i];
            dealtMageCards.Add(currentMinion);
            CardVisual mv = GameManager.Instance.mageShopPile.GetChild(i).GetComponent<CardVisual>();
            mv.Md = currentMinion;
            GameManager.Instance.mageShopPile.GetChild(i).gameObject.SetActive(true);
            //Pop added
            mageMinions.Remove(mageMinions[i]);
        }
    }

    public GameObject GetNextShopCard(string cardType, bool change, MinionData minion = null)
    {
        GameObject tmp = Instantiate(minionPrefab);
        tmp.SetActive(false);
        if (cardType.Equals("Warrior"))
        {
            if (warriorMinions.Count != 0)
            {
                tmp.GetComponent<CardVisual>().Md = warriorMinions[0];
                warriorMinions.Remove(warriorMinions[0]);
                if (change)
                    warriorMinions.Add(minion);
            }
        }
        else if (cardType.Equals("Rogue"))
        {
            if (rogueMinions.Count != 0)
            {
                tmp.GetComponent<CardVisual>().Md = rogueMinions[0];
                rogueMinions.Remove(rogueMinions[0]);
                if (change)
                    rogueMinions.Add(minion);
            }
        }
        else
        {
            if (mageMinions.Count != 0)
            {
                tmp.GetComponent<CardVisual>().Md = mageMinions[0];
                mageMinions.Remove(mageMinions[0]);
                if (change)
                    mageMinions.Add(minion);
            }
        }

        if (tmp.GetComponent<CardVisual>().Md != null)
        {
            tmp.GetComponent<CardVisual>().inShop = true;
            tmp.AddComponent<ShowShopCard>();
            tmp.SetActive(true);
        }

        return tmp;
    }

    public bool CanChangeShopCard(string cardType)
    {
        bool result = false;
        if (cardType.Equals("Warrior"))
        {
            if (warriorMinions.Count != 0)
                result = true;
        }
        else if (cardType.Equals("Rogue"))
        {
            if (rogueMinions.Count != 0)
                result = true;
        }
        else
        {
            if (mageMinions.Count != 0)
                result = true;
        }

        return result;
    }

    public void ShuffleStarterDeck()
    {
        for (int i = 0; i < starters.Count; i++)
        {
            int rnd = Random.Range(0, starters.Count);
            tempStarter = starters[rnd];
            starters[rnd] = starters[i];
            starters[i] = tempStarter;
        }
    }

    public void SetStarterDeck()
    {
        allyStarters = new List<StarterData>();
        enemyStarters = new List<StarterData>();
        allyDeck = new List<Card>();
        enemyDeck = new List<Card>();

        for (int i = 0; i < starters.Count; i++)
        {
            allyStarters.Add(starters[i]);
            allyDeck.Add(starters[i]); //add starter cards to decklist
        }

        ShuffleStarterDeck();

        for (int i = 0; i < starters.Count; i++)
        {
            enemyStarters.Add(starters[i]);
            enemyDeck.Add(starters[i]);
        }
    }

    public void SetEssentials()
    {
        //TODO: how many essentials?
        for (int i = 0; i < 4; i++)
        {
            currentEssential = essentials[i];
            CardVisual cv = GameManager.Instance.essentialsPile.GetChild(i).GetComponent<CardVisual>();
            cv.Ed = currentEssential;
            GameManager.Instance.essentialsPile.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void HighlightHeroPortraitAndName()
    {
        if (GameManager.Instance.GetCurrentPlayer() == 0)
        {
            GameManager.Instance.bottomHero.HeroImageBorder.color = Color.green;
            GameManager.Instance.bottomHero.PlayerNameBox.color = Color.green;
            GameManager.Instance.topHero.HeroImageBorder.color = Color.white;
            GameManager.Instance.topHero.PlayerNameBox.color = Color.white;
        }
        else
        {
            GameManager.Instance.bottomHero.HeroImageBorder.color = Color.white;
            GameManager.Instance.bottomHero.PlayerNameBox.color = Color.white;
            GameManager.Instance.topHero.HeroImageBorder.color = Color.green;
            GameManager.Instance.topHero.PlayerNameBox.color = Color.green;
        }
    }

    public void ShowHideAttackButton()
    {
        if (GameManager.Instance.GetCurrentPlayer() == 0)
        {
            GameManager.Instance.bottomHero.AttackButton.parent.gameObject.SetActive(true);
            GameManager.Instance.topHero.AttackButton.parent.gameObject.SetActive(false);
        }
        else
        {
            GameManager.Instance.topHero.AttackButton.parent.gameObject.SetActive(true);
            GameManager.Instance.bottomHero.AttackButton.parent.gameObject.SetActive(false);
        }
    }

    public List<MinionData> GetTopCardsFromDeck(string cardType)
    {
        List<MinionData> mList = new List<MinionData>();

        if (cardType.Equals("Warrior"))
        {
            if (warriorMinions.Count != 0)
            {
                mList.Add(warriorMinions[0]);
                mList.Add(warriorMinions[1]);
            }
        }
        else if (cardType.Equals("Rogue"))
        {
            if (rogueMinions.Count != 0)
            {
                mList.Add(rogueMinions[0]);
                mList.Add(rogueMinions[1]);
            }
        }
        else
        {
            if (mageMinions.Count != 0)
            {
                mList.Add(mageMinions[0]);
                mList.Add(mageMinions[1]);
            }
        }

        return mList;
    }

    public List<MinionData> MoveTopCardsToBottom(string cardType, List<MinionData> data)
    {
        List<MinionData> mList = new List<MinionData>();

        if (cardType.Equals("Warrior"))
        {
            if (warriorMinions.Count != 0)
            {
                foreach (MinionData md in data)
                {
                    warriorMinions.Remove(md);
                }

                foreach (MinionData md in data)
                {
                    warriorMinions.Add(md);
                }
            }
        }
        else if (cardType.Equals("Rogue"))
        {
            if (rogueMinions.Count != 0)
            {
                foreach (MinionData md in data)
                {
                    rogueMinions.Remove(md);
                }

                foreach (MinionData md in data)
                {
                    rogueMinions.Add(md);
                }
            }
        }
        else
        {
            if (mageMinions.Count != 0)
            {
                foreach (MinionData md in data)
                {
                    mageMinions.Remove(md);
                }

                foreach (MinionData md in data)
                {
                    mageMinions.Add(md);
                }
            }
        }

        return mList;
    }

    //displays discard pile for bottom player
    public void DisplayAllyDiscards()
    {
        if (allyDiscardClosed) //checks to see if discard UI is open or closed
        {
            GameManager.Instance.alliedDiscardUI.gameObject.SetActive(true);
            for (int i = 0; i < allyDiscards.Count; i++)
            {
                //display cards in the UI equal to the amount of cards in the discard pile
                GameManager.Instance.SpawnCard(GameManager.Instance.alliedDiscardUI.transform.Find("CardPile/Cards"), allyDiscards[i]);
            }

            allyDiscardClosed = false; //sets shop to "open" state
        }
        else
        {
            for (int i = 0; i < GameManager.Instance.alliedDiscardUI.transform.Find("CardPile/Cards").childCount; i++)
            {
                Destroy(GameManager.Instance.alliedDiscardUI.transform.Find("CardPile/Cards").GetChild(i).gameObject); //destroys all cards in the UI
            }

            allyDiscardClosed = true; //sets shop to "closed" state
            GameManager.Instance.alliedDiscardUI.gameObject.SetActive(false);
        }
    }

    //displays discard pile for top player
    public void DisplayEnemyDiscards()
    {
        if (enemyDiscardClosed) //checks to see if discard UI is open or closed
        {
            GameManager.Instance.enemyDiscardUI.gameObject.SetActive(true);

            foreach (Card c in enemyDiscards)
            {
                //display cards in the UI equal to the amount of cards in the discard pile
                GameManager.Instance.SpawnCard(GameManager.Instance.enemyDiscardUI.transform.Find("CardPile/Cards"), c);
            }

            enemyDiscardClosed = false; //sets shop to "open" state
        }
        else
        {
            for (int i = 0; i < GameManager.Instance.enemyDiscardUI.transform.Find("CardPile/Cards").childCount; i++)
            {
                Destroy(GameManager.Instance.enemyDiscardUI.transform.Find("CardPile/Cards").GetChild(i).gameObject); //destroys all cards in the UI
            }

            enemyDiscardClosed = true; //sets shop to "closed" state
        }
    }

    public List<Card> GetActiveDeckList(bool activeWanted)
    {
        if (activeWanted)
        {
            if (GameManager.Instance.GetCurrentPlayer() == 0)
            {
                return allyDeck;
            }
            return enemyDeck;
        }
        else
        {
            if (GameManager.Instance.GetCurrentPlayer() == 0)
            {
                return enemyDeck;
            }
            return allyDeck;
        }
    }

    public List<Card> GetActiveHandList(bool activeWanted)
    {
        if (activeWanted)
        {
            if (GameManager.Instance.GetCurrentPlayer() == 0)
            {
                return allyHand;
            }
            return enemyHand;
        }
        else
        {
            if (GameManager.Instance.GetCurrentPlayer() == 0)
            {
                return enemyHand;
            }
            return allyHand;
        }
    }

    public List<Card> GetActiveDiscardList(bool activeWanted)
    {
        if (activeWanted)
        {
            if (GameManager.Instance.GetCurrentPlayer() == 0)
            {
                return allyDiscards;
            }
            return enemyDiscards;
        }
        else
        {
            if (GameManager.Instance.GetCurrentPlayer() == 0)
            {
                return enemyDiscards;
            }
            return allyDiscards;
        }
    }
}