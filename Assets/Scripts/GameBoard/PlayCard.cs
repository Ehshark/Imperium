using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;

public class PlayCard : MonoBehaviourPunCallbacks
{
    private GameObject card;
    private GameObject summonPanel;
    private CardVisual cv;
    private GameObject glowPanel;
    private TMP_Text promoteButtonText;
    private TMP_Text instructionsText;
    private Button promoteButton;
    private Card thisCard;

    private const byte PLAY_MINION = 8;
    private const byte PLAY_RESOURCE = 9;
    private const byte PROMOTE_MINION = 10;
    private const byte REMOVE_MINION = 11;

    private void Start()
    {
        instructionsText = GameManager.Instance.instructionsObj.GetComponent<TMP_Text>();
        cv = gameObject.GetComponent<CardVisual>();

        if (cv.Md != null)
        {
            thisCard = cv.Md;
            foreach (Transform t in transform)
                if (t.name.Equals("SummonPanel"))
                    summonPanel = t.gameObject;

            foreach (Transform t in summonPanel.transform)
            {
                if (t.name.Equals("PlayMinionButton"))
                    t.GetComponent<Button>().onClick.AddListener(PlayMinion);

                else if (t.name.Equals("PromoteMinionButton"))
                {
                    promoteButton = t.GetComponent<Button>();
                    promoteButton.onClick.AddListener(StartPromoteButtonFunction);
                    promoteButtonText = t.GetComponentInChildren<TMP_Text>();
                }

            }
        }

        else if (cv.Ed != null)
        {
            thisCard = cv.Ed;
            foreach (Transform t in transform)
                if (t.name.Equals("UsePanel"))
                    summonPanel = t.gameObject;

            foreach (Transform t in summonPanel.transform)
                if (t.name.Equals("UseButton"))
                    t.GetComponent<Button>().onClick.AddListener(PlayItem);
        }

        else if (cv.Sd != null)
        {
            thisCard = cv.Sd;
            if (cv.Sd.AttackDamage == 0)
            {
                foreach (Transform t in transform)
                    if (t.name.Equals("UsePanel"))
                        summonPanel = t.gameObject;

                foreach (Transform t in summonPanel.transform)
                    if (t.name.Equals("UseButton"))
                        t.GetComponent<Button>().onClick.AddListener(PlayItem);
            }

            else
            {
                foreach (Transform t in transform)
                    if (t.name.Equals("SummonPanel"))
                        summonPanel = t.gameObject;

                foreach (Transform t in summonPanel.transform)
                {
                    if (t.name.Equals("PlayMinionButton"))
                        t.GetComponent<Button>().onClick.AddListener(PlayMinion);

                    else if (t.name.Equals("PromoteMinionButton"))
                    {
                        promoteButton = t.GetComponent<Button>();
                        promoteButton.onClick.AddListener(StartPromoteButtonFunction);
                        promoteButtonText = t.GetComponentInChildren<TMP_Text>();
                    }
                }
            }
        }
    }

    //Connected to the play button in summon panel
    public void PlayMinion()
    {
        if (!CanPlayCard())
            return;
        summonPanel.SetActive(false);
        MoveCardFromHand(true);
    }

    //Connected to the promote button in summon panel
    public void StartPromoteButtonFunction()
    {
        if (!CanPlayCard())
            return;
        if (!GameManager.Instance.IsPromoting)
        {
            StartOrCancelPromotionEvent(true);
            GameManager.Instance.MinionToPromote = gameObject;
        }

        else
        {
            StartOrCancelPromotionEvent(false);
            GameManager.Instance.MinionToPromote = null;
        }
    }

    private void StartOrCancelPromotionEvent(bool promoting)
    {
        //We want to start the promoting process
        if (promoting)
        {
            if (GameManager.Instance.GetActiveMinionZone(true).childCount == 0)
            {
                string cantPromote = "No minions to sacrifice!";
                StartCoroutine(GameManager.Instance.SetInstructionsText(cantPromote));
                return;
            }

            //Change the color of the button to red
            ColorBlock cb = promoteButton.colors;
            cb.normalColor = Color.red;
            promoteButton.colors = cb;

            promoteButtonText.text = "Cancel";
            instructionsText.text = "Please select an enemy minion to sacrifice";
            GameManager.Instance.EnableOrDisablePlayerControl(!promoting);
            GameManager.Instance.IsPromoting = promoting;
            EventManager.Instance.PostNotification(EVENT_TYPE.SACRIFICE_MINION);
        }

        //We want to cancel the promoting process
        else
        {
            //Change the color of the button to white
            ColorBlock cb = promoteButton.colors;
            cb.normalColor = Color.white;
            promoteButton.colors = cb;

            promoteButtonText.text = "Promote";
            instructionsText.text = "";
            GameManager.Instance.EnableOrDisablePlayerControl(!promoting);
            GameManager.Instance.IsPromoting = promoting;
            EventManager.Instance.PostNotification(EVENT_TYPE.SACRIFICE_MINION);
        }
    }

    public void ShowSummonPanel()
    {
        //if this card was not clicked last and is not the very first clicked card this game
        if (gameObject != UIManager.Instance.LastSelectedCard && UIManager.Instance.LastSelectedCard != null)
        {
            foreach (Transform t in UIManager.Instance.LastSelectedCard.transform)
            {
                if ((t.name.Equals("SummonPanel") || t.name.Equals("UsePanel")) && t.gameObject.activeSelf)
                {
                    t.gameObject.SetActive(false);
                }
            }
                   
        }

        if (summonPanel.activeSelf)
            summonPanel.SetActive(false);

        else
            summonPanel.SetActive(true);
    }

    private void MoveCardFromHand(bool isMinion)
    {
        bool promote = false;
        DelayCommand dc = new DelayCommand(GameManager.Instance.GetActiveHand(true), 1f);
        dc.AddToQueue();
        card = gameObject;
        CardVisual cv = card.GetComponent<CardVisual>();

        byte[] cardByte;
        MinionDataPhoton mdp;
        StarterDataPhoton sdp;
        EssentialsDataPhoton edp;
        string type;

        if (isMinion)
        {
            Card cardData = card.GetComponent<CardVisual>().CardData;
            if (UIManager.Instance.GetActiveHandList(true).Contains(cardData))
            {
                UIManager.Instance.GetActiveHandList(true).Remove(cardData);
            }

            if (GameManager.Instance.IsPromoting)
            {
                card = GameManager.Instance.MinionToPromote;
                StartOrCancelPromotionEvent(false);
                summonPanel.SetActive(false);
                cv.AdjustHealth(2, true);
                cv.IsPromoted = true;
                cv.PromotedHealth = cv.CurrentHealth;
                promote = true;
            }

            MoveCardCommand mc = new MoveCardCommand(card, GameManager.Instance.GetActiveMinionZone(true));
            mc.AddToQueue();

            //Add Condition Scripts 
            if (thisCard is MinionData)
            {
                GameManager.Instance.GetComponent<ConditionAndEffectAssigner>().Md = thisCard as MinionData;
                GameManager.Instance.GetComponent<ConditionAndEffectAssigner>().Card = card;
                EventManager.Instance.PostNotification(EVENT_TYPE.ASSIGN_CONDITIONS);
            }

            if (thisCard is MinionData)
            {
                mdp = new MinionDataPhoton(cv.Md);
                cardByte = DataHandler.Instance.ObjectToByteArray(mdp);
                type = "Minion";
            }
            else
            {
                sdp = new StarterDataPhoton(cv.Sd);
                cardByte = DataHandler.Instance.ObjectToByteArray(sdp);
                type = "Starter";
            }

            object[] data = new object[] { cardByte, type };
            if (!promote)
            {
                PlayCardPun.Instance.SendData(PLAY_MINION, data);
            }
            else
            {
                PlayCardPun.Instance.SendData(PROMOTE_MINION, data);
            }
        }
        else
        {
            Card cardData = card.GetComponent<CardVisual>().CardData;
            if (UIManager.Instance.GetActiveHandList(true).Contains(cardData))
            {
                UIManager.Instance.GetActiveHandList(true).Remove(cardData);
            }

            MoveCardCommand mc = new MoveCardCommand(card, GameManager.Instance.GetActiveDiscardPile(true), UIManager.Instance.GetActiveDiscardList(true));
            mc.AddToQueue();
            //GameManager.Instance.MoveCard(card, GameManager.Instance.GetActiveDiscardPile(true), GameManager.Instance.GetActiveDiscardPileList(true), true);

            if (thisCard is StarterData)
            {
                sdp = new StarterDataPhoton(cv.Sd);
                cardByte = DataHandler.Instance.ObjectToByteArray(sdp);
                type = "Starter";
            }
            else
            {
                edp = new EssentialsDataPhoton(cv.Ed);
                cardByte = DataHandler.Instance.ObjectToByteArray(edp);
                type = "Essential";
            }

            object[] data = new object[] { cardByte, type };
            PlayCardPun.Instance.SendData(PLAY_RESOURCE, data);
        }

        AdjustHeroResources();
    }

    //This function is called when PostNotification is called on the SACRIFICE_SELECTED event and isPromoting is true
    public void PromoteMinionWithPlayback()
    {
        DelayCommand dc = new DelayCommand(GameManager.Instance.GetActiveHand(true), 1f);
        dc.AddToQueue();

        MoveCardCommand mc = new MoveCardCommand(GameManager.Instance.MinionToSacrifice,
            GameManager.Instance.GetActiveDiscardPile(true), UIManager.Instance.GetActiveDiscardList(true));
        mc.AddToQueue();
        //GameManager.Instance.MoveCard(GameManager.Instance.MinionToSacrifice, GameManager.Instance.GetActiveDiscardPile(true), GameManager.Instance.GetActiveDiscardPileList(true), true);

        CardVisual cv = GameManager.Instance.MinionToSacrifice.GetComponent<CardVisual>();
        MinionDataPhoton mdp;
        StarterDataPhoton sdp;
        byte[] cardByte;
        string type;

        if (cv.Md != null)
        {
            mdp = new MinionDataPhoton(cv.Md);
            cardByte = DataHandler.Instance.ObjectToByteArray(mdp);
            type = "Minion";
        }
        else
        {
            sdp = new StarterDataPhoton(cv.Sd);
            cardByte = DataHandler.Instance.ObjectToByteArray(sdp);
            type = "Starter";
        }

        object[] data = new object[] { cardByte, type };
        PlayCardPun.Instance.SendData(REMOVE_MINION, data);

        MoveCardFromHand(true);
    }

    public void PlayItem()
    {
        if (!CanPlayCard())
            return;
        summonPanel.SetActive(false);
        MoveCardFromHand(false);
    }

    private void AdjustHeroResources()
    {
        if (thisCard.GoldAndManaCost == 0)
        {
            if (thisCard is StarterData)
            {
                if (thisCard.EffectId1 == 20)
                    GameManager.Instance.ActiveHero(true).AdjustHealth(1, true);
                else if (thisCard.EffectId1 == 21)
                    GameManager.Instance.ActiveHero(true).AdjustMana(1, true);
            }
            else if (thisCard is EssentialsData)
            {
                if (thisCard.EffectId1 == 20)
                    GameManager.Instance.ActiveHero(true).AdjustHealth(2, true);
                else if (thisCard.EffectId1 == 21)
                    GameManager.Instance.ActiveHero(true).AdjustMana(2, true);
            }
        }
        else if (thisCard.GoldAndManaCost > 0)
        {
            GameManager.Instance.ActiveHero(true).AdjustMana(thisCard.GoldAndManaCost, false);
            if (thisCard.EffectId1 == 18 && thisCard is StarterData)
            {
                GameManager.Instance.ActiveHero(true).AdjustGold(2, true);
                GameManager.Instance.ActiveHero(true).GainExp(1);
            }
            else if (thisCard.EffectId1 == 18 && thisCard is EssentialsData)
            {
                GameManager.Instance.ActiveHero(true).AdjustGold(4, true);
                GameManager.Instance.ActiveHero(true).GainExp(2);
            }
            else if (thisCard.EffectId1 == 14 && thisCard is EssentialsData)
            {
                GameManager.Instance.ActiveHero(true).AdjustDamage(1, true);
            }
        }
    }

    private bool CanPlayCard()
    {
        string message;
        bool canPlay = true;
        if (GameManager.Instance.ActiveHero(true).CurrentHealth == GameManager.Instance.ActiveHero(true).TotalHealth && thisCard.EffectId1 == 20)
        {
            message = "Health is already full!";
            StartCoroutine(GameManager.Instance.SetInstructionsText(message));
            canPlay = false;
        }

        else if (GameManager.Instance.ActiveHero(true).CurrentMana == GameManager.Instance.ActiveHero(true).TotalMana && thisCard.EffectId1 == 21)
        {
            message = "Mana is already full!";
            StartCoroutine(GameManager.Instance.SetInstructionsText(message));
            canPlay = false;
        }

        else if (GameManager.Instance.ActiveHero(true).CurrentMana < thisCard.GoldAndManaCost)
        {
            message = "Not enough Mana!";
            StartCoroutine(GameManager.Instance.SetInstructionsText(message));
            canPlay = false;
        }

        else
            canPlay = true;

        return canPlay;
    }
}
