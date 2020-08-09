using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;

public class GameManager : MonoBehaviour
{
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

    public Transform mulliganButtons;

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

    public Transform EffectIconQueue;

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

    public TMP_Text effectText;

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
    private int removeCardAtIndex;
    private bool firstTimeStartUp;

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
    public bool IsActionPhase { get => isActionPhase; set => isActionPhase = value; }
    public int RemoveCardAtIndex { get => removeCardAtIndex; set => removeCardAtIndex = value; }

    //Multiplayer
    private const byte END_TURN = 12;
    private const byte DRAW_CARDS = 13;
    private const byte PAY_TO_DISCARD = 14;
    private const byte DISCARD_CARDS = 15;
    private const byte FIRST_TURN_SYNC_EVENT = 32;
    private const byte REBUILD_DECKS_SYNC_EVENT = 54;

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

        if (eventCode == END_TURN)
        {
            ActiveHero(true).ResetMana();

            if (topHero.DamageBonus > 0)
            {
                topHero.Damage -= topHero.DamageBonus;
                topHero.SetDamage();
                topHero.DamageBonus = 0;
            }

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
        else if (eventCode == DRAW_CARDS)
        {
            object[] data = (object[])photonEvent.CustomData;
            int drawNum = (int)data[0];

            for (int i = 0; i < drawNum; i++)
            {
                DrawCard(UIManager.Instance.GetActiveDeckList(true), GetActiveHand(true));
            }
        }
        else if (eventCode == PAY_TO_DISCARD)
        {
            DrawCard(UIManager.Instance.GetActiveDeckList(true), GetActiveHand(true));
            ActiveHero(true).AdjustGold(1, false);
        }
        else if (eventCode == DISCARD_CARDS)
        {
            object[] data = (object[])photonEvent.CustomData;
            int[] cards = (int[])data[0];

            foreach (int position in cards)
            {
                Transform t = GetActiveHand(true).GetChild(position);
                DiscardCard(t.gameObject);
            }
        }
        else if (eventCode == REBUILD_DECKS_SYNC_EVENT)
        {
            object[] data = (object[])photonEvent.CustomData;
            List<CardPhoton> newCards = (List<CardPhoton>)DataHandler.Instance.ByteArrayToObject((byte[])data[0]);

            List<Card> newDeck = new List<Card>();
            for (int i = 0; i < newCards.Count; i ++)
            {
                if (newCards[i] is StarterDataPhoton)
                {
                    StarterData sd = ScriptableObject.CreateInstance<StarterData>();
                    sd.Init((StarterDataPhoton)newCards[i]);
                    newDeck.Add(sd);
                }
                else if (newCards[i] is MinionDataPhoton)
                {
                    MinionData md = ScriptableObject.CreateInstance<MinionData>();
                    md.Init((MinionDataPhoton)newCards[i]);
                    newDeck.Add(md);
                }
                else if (newCards[i] is EssentialsDataPhoton)
                {
                    EssentialsData ed = ScriptableObject.CreateInstance<EssentialsData>();
                    ed.Init((EssentialsDataPhoton)newCards[i]);
                    newDeck.Add(ed);
                }
            }

            UIManager.Instance.enemyDeck = newDeck;

            int drawAmount = ActiveHero(true).HandSize - UIManager.Instance.GetActiveHandList(true).Count;
            for (int i = 0; i < drawAmount; i++)
            {
                DrawCard(UIManager.Instance.GetActiveDeckList(true), GetActiveHand(true));
            }

            UIManager.Instance.GetActiveDiscardList(true).Clear();

            foreach (Transform card in GetActiveDiscardPile(true))
            {
                Destroy(card.gameObject);
            }
        }
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
                tmp = SpawnCard(null, card.GetComponent<CardVisual>().Ed);
            }
            else if (card.GetComponent<CardVisual>().Sd != null)
            {
                tmp = SpawnCard(null, card.GetComponent<CardVisual>().Sd);
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
            if (tmpCv.Md != null || tmpCv.Sd != null)
            {
                tmpCv.CurrentDamage = cv.CurrentDamage;
                tmpCv.CurrentHealth = cv.CurrentHealth;
                tmpCv.health.text = tmpCv.CurrentHealth.ToString();
                tmpCv.damage.text = tmpCv.CurrentDamage.ToString();

                Destroy(card);
            }
            
            if (cv.Ed != null)
            {
                if (card.transform.parent != essentialsPile)
                {
                    Destroy(card);
                }
            }

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
        //if (enable)
        //    instructionsObj.GetComponent<TMP_Text>().text = "";

        if (!StartGameController.Instance.tutorial)
        {
            shopButton.interactable = enable;
            allyDiscardPileButton.interactable = enable;
            enemyDiscardPileButton.interactable = enable;
        }
        if (bottomHero.MyTurn)
        {
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

        SendEndTurn();

        if (StartGameController.Instance.tutorial && GetActiveHand(true) == enemyHand)
        {
            StartGameController.Instance.TutorialObject.GetComponent<TutorialTextController>().ShowUI();
        }
    }

    public void SendEndTurn()
    {
        ActiveHero(false).ResetMana();
        bottomHero.AttackButton.parent.gameObject.SetActive(false);
        bottomHero.AttackButton.gameObject.SetActive(false);
        UIManager.Instance.HighlightHeroPortraitAndName();
        UIManager.Instance.GlowCards();
        endButton.interactable = false;

        foreach (Transform t in GetActiveMinionZone(true))
        {
            CardVisual cv = t.GetComponent<CardVisual>();
            cv.IsTapped = false;
            cv.ChangeTappedAppearance();
            ResetDamage(t);
        }

        foreach (Transform t in GetActiveMinionZone(false))
        {
            CardVisual cv = t.GetComponent<CardVisual>();
            if (cv.Md && cv.IsSilenced)
                cv.ActivateSilence(false);
            if (cv.Md && cv.IsCombatEffectActivated)
                DisableCombatEffect(cv);
        }

        if (bottomHero.DamageBonus > 0)
        {
            bottomHero.Damage -= bottomHero.DamageBonus;
            bottomHero.SetDamage();
            bottomHero.DamageBonus = 0;
        }

        PhotonNetwork.RaiseEvent(END_TURN, null, new RaiseEventOptions { Receivers = ReceiverGroup.Others },
            SendOptions.SendReliable);
    }

    public void StartTurn()
    {
        StartGameController.Instance.StartingPowerSelected = true;
        isActionPhase = false;
        CardVisual cv;
        foreach (Transform t in GetActiveMinionZone(true))
        {
            cv = t.GetComponent<CardVisual>();
            DisableCombatEffect(cv);
            UnTapMinions(t);
            ResetDamage(t);
        }

        foreach (Transform t in GetActiveMinionZone(false))
        {
            cv = t.GetComponent<CardVisual>();
            if (cv.Md && cv.IsSilenced)
                cv.ActivateSilence(false);
            if (cv.Md && cv.IsCombatEffectActivated)
                DisableCombatEffect(cv);
        }

        UIManager.Instance.HighlightHeroPortraitAndName();
        UIManager.Instance.GlowCards();
        UIManager.Instance.AttachPlayCard();
        EnableOrDisablePlayerControl(true);
        if (!StartGameController.Instance.FirstTurn)
        {
            ActiveHero(true).AttackButton.parent.gameObject.SetActive(true);
            ActiveHero(true).AttackButton.gameObject.SetActive(true);
        }
        buyFirstCard = false;
        firstChangeShop = false;
        DisableExpressBuy();

        endButton.interactable = true;
        buyButton.interactable = true;
        changeButton.interactable = true;
        //enemyDiscardPileButton.interactable = false;

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

        //ActiveHero(true).AdjustDiscard(false);
        isForcedDiscard = false;

        //AdjustDeckHeight();
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

            //AdjustDeckHeight();
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

                foreach (Transform t in alliedDiscardPile)
                {
                    Destroy(t.gameObject);
                }

                ShuffleCurrentDeck(deck);
                allyDeckCounter.text = deck.Count.ToString();

                List<CardPhoton> deckToSend = new List<CardPhoton>();
                foreach (Card card in deck)
                {
                    if (card is MinionData)
                    {
                        deckToSend.Add(new MinionDataPhoton((MinionData)card));
                    }
                    else if (card is StarterData)
                    {
                        deckToSend.Add(new StarterDataPhoton((StarterData)card));
                    }
                    else if (card is EssentialsData)
                    {
                        deckToSend.Add(new EssentialsDataPhoton((EssentialsData)card));
                    }
                }

                byte[] cardByte = DataHandler.Instance.ObjectToByteArray(deckToSend);
                object[] data = new object[] { cardByte };

                PhotonNetwork.RaiseEvent(REBUILD_DECKS_SYNC_EVENT, data, new RaiseEventOptions { Receivers = ReceiverGroup.Others },
                    SendOptions.SendReliable);

                //function calls itself to continue the draw since deck is no longer empty
                DrawCard(deck, playerHand);
            }
            //else
            //{
            //    for (int i = 0; i < UIManager.Instance.enemyDiscards.Count; i++)
            //    {
            //        deck.Add(UIManager.Instance.enemyDiscards[i]);
            //    }

            //    UIManager.Instance.enemyDiscards.Clear();

            //    foreach (Transform t in enemyDiscardPile)
            //    {
            //        Destroy(t.gameObject);
            //    }

            //    ShuffleCurrentDeck(deck);
            //    enemyDeckCounter.text = deck.Count.ToString();
            //}

            //AdjustDeckHeight();
            ////function calls itself to continue the draw since deck is no longer empty
            //DrawCard(deck, playerHand);
        }

        if (isActionPhase)
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
        buyButton.interactable = false;
        changeButton.interactable = false;
        isActionPhase = false;
        Transform activeHand = GetActiveHand(true);
        int handSize = ActiveHero(true).HandSize;
        int drawNum;
        selectedDiscards = new List<GameObject>();
        hasSwitchedCard = false;

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
            return;
        }
        else if (UIManager.Instance.GetActiveHandList(true).Count < handSize)
        {
            drawNum = handSize - UIManager.Instance.GetActiveHandList(true).Count;
            object[] data = new object[] { drawNum };

            PhotonNetwork.RaiseEvent(DRAW_CARDS, data, new RaiseEventOptions { Receivers = ReceiverGroup.Others },
                SendOptions.SendReliable);

            for (int i = 0; i < drawNum; i++)
            {
                DrawCard(UIManager.Instance.GetActiveDeckList(true), activeHand);
            }
        }

        if (StartGameController.Instance.FirstTurn)
        {
            StartGameController.Instance.FirstTurn = false;
            PhotonNetwork.RaiseEvent(FIRST_TURN_SYNC_EVENT, null, new RaiseEventOptions { Receivers = ReceiverGroup.Others },
                SendOptions.SendReliable);
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
            instructionsObj.GetComponent<TMP_Text>().text = "Opponent's Turn...";
        }
    }

    // Allows player to pay gold to draw an additional card and then discard a card
    public void EndPhaseCardSwitch()
    {
        submitDiscardsButton.gameObject.SetActive(true);
        ActiveHero(true).AdjustGold(1, false);

        PhotonNetwork.RaiseEvent(PAY_TO_DISCARD, null, new RaiseEventOptions { Receivers = ReceiverGroup.Others },
            SendOptions.SendReliable);

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

        if (selectedDiscards.Count == discardNum || selectedDiscards.Count == ActiveHero(true).HasToDiscard)
        {
            int[] cards = new int[discardNum];

            int i = 0;
            foreach (GameObject t in selectedDiscards)
            {
                int position = t.transform.GetSiblingIndex();
                cards[i] = position;
            }

            object[] data = new object[] { cards };

            PhotonNetwork.RaiseEvent(DISCARD_CARDS, data, new RaiseEventOptions { Receivers = ReceiverGroup.Others },
                SendOptions.SendReliable);

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
            instructionsObj.GetComponent<TMP_Text>().text = "";
            if (!isActionPhase)
                isActionPhase = true;
            
            if (ActiveHero(true).HasToDiscard > 0)
            {
                ActiveHero(true).AdjustDiscard(false);
                foreach (Transform t in activeHand)
                {
                    t.gameObject.AddComponent<PlayCard>();
                }
                return;
            }

            if (!hasSwitchedCard && !isForcedDiscard)
            {
                if (ActiveHero(true).Gold > 0)
                {
                    instructionsObj.GetComponent<TMP_Text>().text = "Pay 1 gold to draw and discard a card?";
                    cardSwitchButtonYes.gameObject.SetActive(true);
                    cardSwitchButtonNo.gameObject.SetActive(true);
                    hasSwitchedCard = true;
                }
            }
            else
            {
                hasSwitchedCard = false; //resets if player switch card check
                instructionsObj.GetComponent<TMP_Text>().text = "Opponent's Turn...";
                SwitchTurn();
            }
        }
        else if (selectedDiscards.Count < discardNum)
        {
            instructionsObj.GetComponent<TMP_Text>().text = "You haven't selected enough, select more cards to discard";
        }
        else
        {
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
            else
            {
                tmp.GetComponent<CardVisual>().inShop = false;
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
                cv.TapEffect = false;
                cv.ChangeTappedAppearance();
            }
        }
    }

    private void ResetDamage(Transform t)
    {
        CardVisual cv = t.GetComponent<CardVisual>();
        cv.ResetDamage();
    }

    private void DisableCombatEffect(CardVisual cv)
    {
        if (cv.Md != null && cv.Md.ConditionID != 7)
        {
            cv.IsCombatEffectActivated = false;
            cv.CombatEffectActivated(false);
        }
    }

    public void DisableExpressBuy()
    {
        if (hasExpressBuy)
        {
            hasExpressBuy = false;
            expressBuyImage.gameObject.SetActive(false);
        }
    }

    //Used to change the deck's visual height
    public void AdjustDeckHeight()
    {
        Vector3 tmp = GetActiveDeck(true).transform.localPosition; //saves current decks x and y pos
        GetActiveDeck(true).transform.localPosition = new Vector3(tmp.x, tmp.y, 100f); //resets deck to default height

        float height = UIManager.Instance.GetActiveDeckList(true).Count * 2f; //total "thickness" of the cards

        GetActiveDeck(true).transform.localPosition = new Vector3(tmp.x, tmp.y, 100f - height); //setting new height of deck

        if (UIManager.Instance.GetActiveDeckList(true).Count == 0) //deck disappears if there are no cards
        {
            GetActiveDeck(true).GetChild(0).transform.gameObject.SetActive(false);
            GetActiveDeck(true).GetChild(1).transform.gameObject.SetActive(false);
        }
        else //if the deck regains cards make it reappear
        {
            GetActiveDeck(true).GetChild(0).transform.gameObject.SetActive(true);
            GetActiveDeck(true).GetChild(1).transform.gameObject.SetActive(true);
        }
    }

    //used for rotating card animation
    public IEnumerator FlipCard(GameObject card)
    {
        for (float i = 0f; i <= 360f; i += 10)
        {
            card.transform.rotation = Quaternion.Euler(0f, i, 0f);
            yield return new WaitForSeconds(0f);
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
        if (t.name.Equals("EnemyHand"))
            tmp.transform.Find("CardBack").gameObject.SetActive(true);
        tmp.SetActive(false);

        if (c is MinionData md)
            tmp.GetComponent<CardVisual>().Md = md;
        else if (c is StarterData sd)
            tmp.GetComponent<CardVisual>().Sd = sd;
        else if (c is EssentialsData ed)
            tmp.GetComponent<CardVisual>().Ed = ed;

        tmp.SetActive(true);

        StartCoroutine(FlipCard(tmp)); //calls card flip animation
    }
}
