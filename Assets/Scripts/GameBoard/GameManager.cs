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
    public Transform skillTree;
    public GameObject skillTreePrefab;
    public Transform StartGameManager;

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
    public Transform alliedDiscardUI;

    public Transform enemyMinionZone;
    public Transform enemyHand;
    public Transform enemyDeck;
    public Transform enemyDiscardPile;
    public Transform enemyDiscardUI;

    public List<GameObject> selectedDiscards;
    public Transform submitDiscardsButton;
    public Transform exitDiscardsButton;
    public Transform recycleButton;
    public Transform cardSwitchButtonYes;
    public Transform cardSwitchButtonNo;

    public Transform shop;
    public Transform warriorShopPile;
    public Transform rogueShopPile;
    public Transform mageShopPile;
    public Transform essentialsPile;
    public Transform option;
    public Transform menu;
    public Transform pMenu;
    public Transform trashUI;

    public Hero bottomHero;
    public Hero topHero;

    public Button buyButton;
    public Button changeButton;
    public Button endButton;
    public Button shopButton;
    public Button exitShopButton;
    public Button moveButton;
    public Button allyDiscardPileButton;
    public Button enemyDiscardPileButton;
    public Image expressBuyImage;

    private bool isPromoting = false;
    private bool isDefending = false;
    private bool hasExpressBuy = false;
    private GameObject minionToPromote;
    private GameObject minionToSacrifice;
    private List<GameObject> minionsAttacking = new List<GameObject>();
    private bool buyFirstCard;
    private bool firstChangeShop;
    private bool isEffect;
    private float turnTimer;
    private bool hasSwitchedCard = false;
    private bool isForcedDiscard = false;
    private bool isActionPhase = false;
    private bool warriorSetup;
    private bool inHeroPower;
    private Color lastSelectedColor;
    private int removeCardAtIndex;

    public static GameManager Instance { get; private set; } = null;
    public bool IsPromoting { get => isPromoting; set => isPromoting = value; }
    public GameObject MinionToPromote { get => minionToPromote; set => minionToPromote = value; }
    public float TurnTimer { get => turnTimer; set => turnTimer = value; }
    public GameObject MinionToSacrifice { get => minionToSacrifice; set => minionToSacrifice = value; }
    public List<GameObject> MinionsAttacking { get => minionsAttacking; set => minionsAttacking = value; }
    public bool BuyFirstCard { get => buyFirstCard; set => buyFirstCard = value; }
    public bool FirstChangeShop { get => firstChangeShop; set => firstChangeShop = value; }
    public bool IsEffect { get => isEffect; set => isEffect = value; }
    public bool IsDefending { get => isDefending; set => isDefending = value; }
    public bool HasExpressBuy { get => hasExpressBuy; set => hasExpressBuy = value; }
    public bool WarriorSetup { get => warriorSetup; set => warriorSetup = value; }
    public bool InHeroPower { get => inHeroPower; set => inHeroPower = value; }
    public Color LastSelectedColor { get => lastSelectedColor; set => lastSelectedColor = value; }
    public bool IsActionPhase { get => isActionPhase; set => isActionPhase = value; }
    public int RemoveCardAtIndex { get => removeCardAtIndex; set => removeCardAtIndex = value; }

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

    public GameObject MoveCard(GameObject card, Transform to, List<Card> list = null, bool returnCard = false, bool simple = false)
    {
        CardVisual cv = card.GetComponent<CardVisual>();
        if (!simple)
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
                tmp = SpawnCard(null, card.GetComponent<CardVisual>().Sd, false);
            }
            else
            {
                tmp = null;
            }

            tmp.transform.SetParent(to, false);
            tmp.transform.localScale = new Vector3(1, 1, 1);

            if (list != null)
            {
                list.Add(card.GetComponent<CardVisual>().GetCardData());
            }

            CardVisual tmpCv = tmp.GetComponent<CardVisual>();
            tmpCv.CurrentDamage = cv.CurrentDamage;
            tmpCv.CurrentHealth = cv.CurrentHealth;
            tmpCv.health.text = tmpCv.CurrentHealth.ToString();
            tmpCv.damage.text = tmpCv.CurrentDamage.ToString();

            Destroy(card);

            if (returnCard)
            {
                return tmp;
            }
            else
            {
                return null;
            }
        }
        else
        {
            card.transform.SetParent(to.transform, false);
            card.transform.position = to.transform.position;
            if (to.name.Equals("MinionArea") || to.name.Equals("EnemyMinionArea"))
            {
                if (card.GetComponent<PlayCard>())
                {
                    Destroy(card.GetComponent<PlayCard>());
                }
            }
            return null;
        }
    }

    public Hero ActiveHero(bool activeWanted)
    {
        if (activeWanted)
        {
            if (bottomHero.MyTurn)
                return bottomHero;
            else
                return topHero;
        }
        else
        {
            if (bottomHero.MyTurn)
                return topHero;
            else
                return bottomHero;
        }
    }

    public void EnableOrDisablePlayerControl(bool enable)
    {
        allyDiscardPileButton.interactable = enable;
        enemyDiscardPileButton.interactable = enable;
        if (enable)
            instructionsObj.GetComponent<TMP_Text>().text = "";
        shopButton.interactable = enable;

        ActiveHero(true).CanPlayCards = enable;
        if (enable)
        {
            UIManager.Instance.GlowCards();
        }
        else
        {
            foreach (Transform m in GetActiveHand(true))
            {
                if (m != null)
                {
                    m.Find("GlowPanel").gameObject.SetActive(false);
                }
            }
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

        StartTurn();
    }

    void StartTurn()
    {
        isActionPhase = false;
        CardVisual cv;
        foreach (Transform t in GetActiveMinionZone(true))
        {
            cv = t.GetComponent<CardVisual>();
            cv.IsCombatEffectActivated = false;
            cv.CombatEffectActivated(false);
            UnTapMinions(t);
            ResetDamage(t);
            cv.ActivateSilence(false);
        }

        UIManager.Instance.HighlightHeroPortraitAndName();
        UIManager.Instance.ShowHideAttackButton();
        UIManager.Instance.GlowCards();
        UIManager.Instance.AttachPlayCard();
        EnableOrDisablePlayerControl(true);
        buyFirstCard = false;
        firstChangeShop = false;
        DisableExpressBuy();

        GameManager.Instance.ActiveHero(true).AttackButton.gameObject.SetActive(true);

        //TODO: Handle opponent discard logic here
        if (ActiveHero(true).HasToDiscard > 0)
        {
            isForcedDiscard = true;
            instructionsObj.GetComponent<TMP_Text>().text = "Please Select A Card To Discard";

            EventManager.Instance.PostNotification(EVENT_TYPE.DISCARD_CARD);
            submitDiscardsButton.gameObject.SetActive(true);
        }
        else
            isActionPhase = true;

        ActiveHero(true).AdjustDiscard(false);
        isForcedDiscard = false;
    }

    public int GetCurrentPlayer()
    {
        int player;

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
    public void DrawCard(List<Card> deck, Transform playerHand)
    {
        if (deck.Count > 0)
        {
            DelayCommand dc = new DelayCommand(GetActiveHand(true), 0.25f);
            dc.AddToQueue();
            InstantiateCommand ic = new InstantiateCommand(deck[0], playerHand);
            ic.AddToQueue();

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
            if (playerHand == alliedHand)
            {
                for (int i = 0; i < UIManager.Instance.allyDiscards.Count; i++)
                {
                    deck.Add(UIManager.Instance.allyDiscards[i]);
                }

                UIManager.Instance.allyDiscards.Clear();

                foreach (Transform t in GameManager.Instance.alliedDiscardPile)
                {
                    Destroy(t.gameObject);
                }

                ShuffleCurrentDeck(deck);
                allyDeckCounter.text = deck.Count.ToString();
            }
            else
            {
                for (int i = 0; i < UIManager.Instance.enemyDiscards.Count; i++)
                {
                    deck.Add(UIManager.Instance.enemyDiscards[i]);
                }

                UIManager.Instance.enemyDiscards.Clear();

                foreach (Transform t in GameManager.Instance.enemyDiscardPile)
                {
                    Destroy(t.gameObject);
                }

                ShuffleCurrentDeck(deck);
                enemyDeckCounter.text = deck.Count.ToString();
            }

            //function calls itself to continue the draw since deck is no longer empty
            DrawCard(deck, playerHand);
        }

        if(isActionPhase)
            EventManager.Instance.PostNotification(EVENT_TYPE.ACTION_DRAW);

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

    //End phase, player draws/selects cards to discard until hand size is 5, then prompt player to spend 1 gold to draw 1 card and discard 1 card 
    public void EndTurn()
    {
        isActionPhase = false;
        Transform activeHand = GetActiveHand(true);
        int handSize = ActiveHero(true).HandSize;
        int drawNum;
        selectedDiscards = new List<GameObject>();
        hasSwitchedCard = false;

        GameManager.Instance.ActiveHero(true).ResetMana();
        EnableOrDisablePlayerControl(false);

        foreach (Transform t in activeHand)
        {
            Destroy(t.gameObject.GetComponent<PlayCard>());
        }

        if (UIManager.Instance.GetActiveHandList(true).Count > handSize)
        {
            instructionsObj.GetComponent<TMP_Text>().text = "Please Select A Card To Discard";
            EventManager.Instance.PostNotification(EVENT_TYPE.DISCARD_CARD);
            submitDiscardsButton.gameObject.SetActive(true);
        }
        else if (UIManager.Instance.GetActiveHandList(true).Count < handSize)
        {
            drawNum = handSize - UIManager.Instance.GetActiveHandList(true).Count;

            for (int i = 0; i < drawNum; i++)
            {
                DrawCard(UIManager.Instance.GetActiveDeckList(true), activeHand);
            }
        }
        if (ActiveHero(true).Gold > 0)
        {
            instructionsObj.GetComponent<TMP_Text>().text = "Do you want to trade 1 gold to switch an additional card?";
            cardSwitchButtonYes.gameObject.SetActive(true);
            cardSwitchButtonNo.gameObject.SetActive(true);
            hasSwitchedCard = true;
        }
        else
        {
            SwitchTurn();
            instructionsObj.GetComponent<TMP_Text>().text = "No cards needed to discard and no gold. Passing Turn";
        }
    }

    // Allows player to pay gold to draw an additional card and then discard a card
    public void EndPhaseCardSwitch()
    {
        submitDiscardsButton.gameObject.SetActive(true);
        ActiveHero(true).AdjustGold(1, false);
        DrawCard(UIManager.Instance.GetActiveDeckList(true), GetActiveHand(true));
        EventManager.Instance.PostNotification(EVENT_TYPE.DISCARD_CARD);
        instructionsObj.GetComponent<TMP_Text>().text = "Please Select A Card To Discard";
        cardSwitchButtonYes.gameObject.SetActive(false);
        cardSwitchButtonNo.gameObject.SetActive(false);
    }

    public void SubmitDiscard()
    {
        Transform activeHand = GetActiveHand(true);
        int discardNum = UIManager.Instance.GetActiveHandList(true).Count - ActiveHero(true).HandSize;

        if (selectedDiscards.Count == discardNum)
        {
            foreach (GameObject t in selectedDiscards)
            {
                DiscardCard(t);
            }

            foreach (Transform t in activeHand)
            {
                Destroy(t.gameObject.GetComponent<DiscardCardListener>());
            }

            selectedDiscards.Clear();

            submitDiscardsButton.gameObject.SetActive(false);

            if (!isActionPhase)
                isActionPhase = true;

            if (!hasSwitchedCard && !isForcedDiscard)
            {
                instructionsObj.GetComponent<TMP_Text>().text = "Pay 1 gold to draw and discard a card?";
                cardSwitchButtonYes.gameObject.SetActive(true);
                cardSwitchButtonNo.gameObject.SetActive(true);
                hasSwitchedCard = true;
            }
            else
            {
                hasSwitchedCard = false; //resets if player switch card check
                SwitchTurn();
            }
        }
        else if (selectedDiscards.Count < discardNum)
        {
            instructionsObj.GetComponent<TMP_Text>().text = "You haven't selected enough, select more cards to discard";
        }
        else {
            instructionsObj.GetComponent<TMP_Text>().text = "You selected too many, select less cards to discard";
        }
    }

    public GameObject SpawnCard(Transform to, Card card, bool inShop = false)
    {
        GameObject tmp;

        if (card is MinionData md)
        {
            tmp = Instantiate(UIManager.Instance.minionPrefab);
            tmp.SetActive(false);
            tmp.GetComponent<CardVisual>().Md = md;

            if (inShop)
            {
                tmp.GetComponent<CardVisual>().inShop = true;
            }

            tmp.SetActive(true);
            tmp.transform.SetParent(to, false);
            return tmp;
        }
        else if (card is EssentialsData ed)
        {
            tmp = Instantiate(UIManager.Instance.itemPrefab);
            tmp.SetActive(false);
            tmp.GetComponent<CardVisual>().Ed = ed;

            if (inShop)
            {
                tmp.GetComponent<CardVisual>().inShop = true;
            }

            tmp.SetActive(true);
            tmp.transform.SetParent(to, false);
            return tmp;
        }
        else if (card is StarterData sd)
        {
            tmp = Instantiate(UIManager.Instance.starterPrefab);
            tmp.SetActive(false);
            tmp.GetComponent<CardVisual>().Sd = sd;
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

        yield return new WaitForSeconds(3f);

        text.text = "";
    }

    public void ChangeCardColour(GameObject card, Color color)
    {
        CardVisual cv = card.GetComponent<CardVisual>();
        if (cv.IsTapped)
            color.a = 0.5f;
        if (cv)
            cv.cardBackground.color = color;
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

    private void UnTapMinions(Transform t)
    {
        CardVisual cv = t.GetComponent<CardVisual>();
        if (cv != null)
        {
            if ((cv.Md != null || cv.Sd != null) && cv.IsTapped)
            {
                cv.IsTapped = false;
                cv.ChangeTappedAppearance();
            }
        }
    }

    private void ResetDamage(Transform t)
    {
        CardVisual cv = t.GetComponent<CardVisual>();
        cv.ResetDamage();
    }

    public void DisableExpressBuy()
    {
        if (hasExpressBuy)
        {
            hasExpressBuy = false;
            expressBuyImage.gameObject.SetActive(false);
        }
    }

    public Transform GetActiveHand(bool activeWanted)
    {
        if (activeWanted)
        {
            if (GetCurrentPlayer() == 0)
            {
                return alliedHand;
            }
            return enemyHand;
        }
        else
        {
            if (GetCurrentPlayer() == 0)
            {
                return enemyHand;
            }
            return alliedHand;
        }
    }

    public Transform GetActiveDiscardPile(bool activeWanted)
    {
        if (activeWanted)
        {
            if (GetCurrentPlayer() == 0)
            {
                return alliedDiscardPile;
            }
            return enemyDiscardPile;
        }
        else
        {
            if (GetCurrentPlayer() == 0)
            {
                return enemyDiscardPile;
            }
            return alliedDiscardPile;
        }
    }

    public Transform GetActiveDiscardUI(bool activeWanted)
    {
        if (activeWanted)
        {
            if (GetCurrentPlayer() == 0)
            {
                return alliedDiscardUI;
            }

            return enemyDiscardUI;
        }
        else
        {
            if (GetCurrentPlayer() == 0)
            {
                return enemyDiscardUI;
            }
            return alliedDiscardUI;
        }
    }
    
    public Transform GetActiveMinionZone(bool activeWanted)
    {
        if (activeWanted)
        {
            if (GetCurrentPlayer() == 0)
            {
                return alliedMinionZone;
            }
            return enemyMinionZone;
        }
        else
        {
            if (GetCurrentPlayer() == 0)
            {
                return enemyMinionZone;
            }
            return alliedMinionZone;
        }
    }

    public void DiscardCard(GameObject card)
    {
        Card cardData = card.GetComponent<CardVisual>().CardData;

        if (UIManager.Instance.GetActiveHandList(true).Contains(cardData))
        {
            UIManager.Instance.GetActiveHandList(true).Remove(cardData);
        }

        MoveCard(card, Instance.GetActiveDiscardPile(true), UIManager.Instance.GetActiveDiscardList(true), true);
    }

    //TODO: Function to disable play card contol 
    public Transform GetActiveDeck(bool activeWanted)
    {
        if (activeWanted)
        {
            if (GetCurrentPlayer() == 0)
            {
                return alliedDeck;
            }
            return enemyDeck;
        }
        else
        {
            if (GetCurrentPlayer() == 0)
            {
                return enemyDeck;
            }
            return alliedDeck;
        }
    }

    public TMP_Text GetActiveDeckCounter(bool activeWanted)
    {
        if (activeWanted)
        {
            if (GetCurrentPlayer() == 0)
            {
                return allyDeckCounter;
            }
            return enemyDeckCounter;
        }
        else
        {
            if (GetCurrentPlayer() == 0)
            {
                return enemyDeckCounter;
            }
            return allyDeckCounter;
        }
    }

    public void InstantiateCardToHand(Card c, Transform t)
    {
        GameObject cardType;

        if (c is MinionData)
            cardType = UIManager.Instance.minionPrefab;
        else if (c is StarterData)
            cardType = UIManager.Instance.starterPrefab;
        else
            cardType = UIManager.Instance.itemPrefab;

        GameObject tmp = Instantiate(cardType, t);
        tmp.SetActive(false);

        if (c is MinionData md)
            tmp.GetComponent<CardVisual>().Md = md;
        else if (c is StarterData sd)
            tmp.GetComponent<CardVisual>().Sd = sd;
        else if (c is EssentialsData ed)
            tmp.GetComponent<CardVisual>().Ed = ed;

        tmp.SetActive(true);
    }
}
