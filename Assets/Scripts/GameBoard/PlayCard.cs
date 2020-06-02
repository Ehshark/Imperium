using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayCard : MonoBehaviour
{
    private GameObject card;
    private GameObject summonPanel;
    private CardVisual cv;
    private GameObject glowPanel;
    private TMP_Text promoteButtonText;
    private TMP_Text instructionsText;
    private Button promoteButton;
    private Card thisCard;

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
        if (!CanPlayItem())
            return;
        summonPanel.SetActive(false);
        StartCoroutine(MoveCardFromHand(true));
        //turns off glowpanel when moving minion
        if (glowPanel != null)
        {
            glowPanel.SetActive(false);
        }
        StartCoroutine(MoveCardFromHand(true));
    }

    //Connected to the promote button in summon panel
    public void StartPromoteButtonFunction()
    {
        if (!CanPlayItem())
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
        if (GameManager.Instance.alliedMinionZone.childCount == 0)
        {
            string cantPromote = "No minions to sacrifice!";
            StartCoroutine(GameManager.Instance.SetInstructionsText(cantPromote));
            return;
        }

        //We want to start the promoting process
        if (promoting)
        {
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
                    t.gameObject.SetActive(false);
            }
        }

        if (summonPanel.activeSelf)
            summonPanel.SetActive(false);

        else
            summonPanel.SetActive(true);
    }

    //This function is called when PostNotification is called on the SACRIFICE_SELECTED event and isPromoting is true
    public void StartPromotionCoroutine()
    {
        StartCoroutine(PromoteMinionWithPlayback());
    }

    IEnumerator MoveCardFromHand(bool isMinion)
    {
        Transform minionZone = GameManager.Instance.alliedHand;
        Image image = minionZone.GetComponent<Image>();
        Color color = image.color;
        while (image.color.a < 1) //use "< 1" when fading in
        {
            color.a += Time.deltaTime / 1; //fades out over 1 second. change to += to fade in
            image.color = color;
            yield return null;
        }
        color.a = .4f;
        image.color = color;

        card = gameObject;

        if (isMinion)
        {
            if (GameManager.Instance.IsPromoting)
            {
                card = GameManager.Instance.MinionToPromote;
                StartOrCancelPromotionEvent(false);
                summonPanel.SetActive(false);
            }

            MoveCardCommand mc = new MoveCardCommand(card, GameManager.Instance.alliedHand, GameManager.Instance.alliedMinionZone);
            mc.AddToQueue();

            //Add Condition Scripts 
            if (thisCard is MinionData)
            {
                GameManager.Instance.GetComponent<ConditionAndEffectAssigner>().Md = thisCard as MinionData;
                GameManager.Instance.GetComponent<ConditionAndEffectAssigner>().Card = card;
                EventManager.Instance.PostNotification(EVENT_TYPE.ASSIGN_CONDITIONS);
            }
        }

        else
        {
            MoveCardCommand mc = new MoveCardCommand(card, GameManager.Instance.alliedHand, GameManager.Instance.alliedDiscardPile);
            mc.AddToQueue();
        }

        AdjustHeroResources();
    }

    IEnumerator PromoteMinionWithPlayback()
    {
        Transform minionZone = GameManager.Instance.alliedMinionZone;
        Image image = minionZone.GetComponent<Image>();
        Color color = image.color;
        while (image.color.a < 1) //use "< 1" when fading in
        {
            color.a += Time.deltaTime / 1; //fades out over 1 second. change to += to fade in
            image.color = color;
            yield return null;
        }
        color.a = .4f;
        image.color = color;

        MoveCardCommand mc = new MoveCardCommand(GameManager.Instance.MinionToSacrifice,
            GameManager.Instance.alliedMinionZone, GameManager.Instance.alliedDiscardPile);
        mc.AddToQueue();
        StartCoroutine(MoveCardFromHand(true));
    }

    public void PlayItem()
    {
        if (!CanPlayItem())
            return;
        summonPanel.SetActive(false);
        StartCoroutine(MoveCardFromHand(false));
    }

    private void AdjustHeroResources()
    {
        if (thisCard.GoldAndManaCost == 0)
        {
            if (thisCard is StarterData)
            {
                if (thisCard.EffectId1 == 20)
                    GameManager.Instance.ActiveHero().AdjustHealth(1, true);
                else if (thisCard.EffectId1 == 21)
                    GameManager.Instance.ActiveHero().AdjustMana(1, true);
            }
            else if (thisCard is EssentialsData)
            {
                if (thisCard.EffectId1 == 20)
                    GameManager.Instance.ActiveHero().AdjustHealth(2, true);
                else if (thisCard.EffectId1 == 21)
                    GameManager.Instance.ActiveHero().AdjustMana(2, true);
            }
        }
        else if (thisCard.GoldAndManaCost > 0)
        {
            GameManager.Instance.ActiveHero().AdjustMana(thisCard.GoldAndManaCost, false);
            if (thisCard.EffectId1 == 18 && thisCard is StarterData)
            {
                GameManager.Instance.ActiveHero().AdjustGold(2, true);
                GameManager.Instance.ActiveHero().GainExp(1);
            }
            else if (thisCard.EffectId1 == 18 && thisCard is EssentialsData)
            {
                GameManager.Instance.ActiveHero().AdjustGold(4, true);
                GameManager.Instance.ActiveHero().GainExp(2);
            }
            else if (thisCard.EffectId1 == 14 && thisCard is EssentialsData)
            {
                GameManager.Instance.ActiveHero().AdjustDamage(1);
                //TODO: Take away 1 damage at the end of the turn.
            }
        }
    }

    private bool CanPlayItem()
    {
        string message;
        bool canPlay = true;
        if (GameManager.Instance.ActiveHero().CurrentHealth == GameManager.Instance.ActiveHero().TotalHealth && thisCard.EffectId1 == 20)
        {
            message = "Health is already full!";
            StartCoroutine(GameManager.Instance.SetInstructionsText(message));
            canPlay = false;
        }

        else if (GameManager.Instance.ActiveHero().CurrentMana == GameManager.Instance.ActiveHero().TotalMana && thisCard.EffectId1 == 21)
        {
            message = "Mana is already full!";
            StartCoroutine(GameManager.Instance.SetInstructionsText(message));
            canPlay = false;
        }

        else if (GameManager.Instance.ActiveHero().CurrentMana < thisCard.GoldAndManaCost)
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
