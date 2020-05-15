using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.EventSystems;

public class CardVisual : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private MinionData md;
    public MinionData Md { get => md; set => md = value; }

    private StarterData sd;
    public StarterData Sd { get => sd; set => sd = value; }

    private EssentialsData ed;
    public EssentialsData Ed { get => ed; set => ed = value; }

    private Card cardData;
    public Card CardData { get => cardData; set => cardData = value; }

    bool isEnlarged = false;
    public List<Transform> descriptions;

    private int currentHealth;
    private int totalHealth;

    public int CurrentHealth { get => currentHealth; set => currentHealth = value; }
    public int TotalHealth { get => totalHealth; set => totalHealth = value; }

    public TMP_Text cost;
    public TMP_Text health;
    public TMP_Text damage;

    public Image costImage;
    public Image cardBackground;
    public Image condition;
    public Image allyClass;
    public Image silenceIcon;
    public Image effect1;
    public Image effect2;    public bool inShop;    private PlayCard pc;
    void OnEnable()
    {
        if (md != null)
            cardData = md;
        else if (ed != null)
            cardData = ed;
        else if (sd != null)
            cardData = sd;

        if (md != null || sd != null || ed != null)
        {
            PopulateCard();
            UpdateCardDescriptions();
        }
    }

    public void UpdateCardDescriptions()
    {
        if (md != null)
        {
            TMP_Text text = descriptions[1].GetComponent<TMP_Text>();
            text.text = "This minion's gold and mana cost is " + cardData.GoldAndManaCost;
            text = descriptions[3].GetComponent<TMP_Text>();
            text.text = "This minion's health is " + cardData.Health;
            text = descriptions[5].GetComponent<TMP_Text>();
            text.text = "This minion's damage is " + cardData.AttackDamage;
            text = descriptions[7].GetComponent<TMP_Text>();
            text.text = "When this minion attacks, if you control a " + cardData.AllyClass + " minion, this minion's damage increases by 1";
            text = descriptions[9].GetComponent<TMP_Text>();
            text.text = cardData.ConditionText;
            text = descriptions[11].GetComponent<TMP_Text>();
            text.text = cardData.EffectText1;
            text = descriptions[13].GetComponent<TMP_Text>();
            text.text = cardData.EffectText2;
        }

        else if (sd != null)
        {
            TMP_Text text = descriptions[1].GetComponent<TMP_Text>();
            text.text = "This card's mana cost is " + cardData.GoldAndManaCost;
            text = descriptions[3].GetComponent<TMP_Text>();
            text.text = "This minion's health is " + cardData.Health;
            text = descriptions[5].GetComponent<TMP_Text>();
            text.text = "This minion's damage is " + cardData.AttackDamage;
            text = descriptions[7].GetComponent<TMP_Text>();
            text.text = cardData.EffectText1;
            text = descriptions[9].GetComponent<TMP_Text>();
            text.text = cardData.EffectText2;
        }

        else if (ed != null)
        {
            TMP_Text text = descriptions[1].GetComponent<TMP_Text>();
            text.text = "This card's mana cost is " + cardData.GoldAndManaCost;
            text = descriptions[3].GetComponent<TMP_Text>();
            text.text = cardData.EffectText1;
            text = descriptions[5].GetComponent<TMP_Text>();
            text.text = cardData.EffectText2;
        }
    }

    void PopulateCard()
    {
        if (!inShop)
        {
            //Display mana image if the card is present in the shop
            costImage.sprite = UIManager.Instance.allSprites.Where(x => x.name == "mana").SingleOrDefault();
        }

        //set the cost
        cost.text = CardData.GoldAndManaCost.ToString();
        //set the health
        if (health != null)
        {
            if (CardData.Health != 0)
            {
                health.text = CardData.Health.ToString();
                totalHealth = CardData.Health;
                currentHealth = CardData.Health;
            }
            else
                health.transform.parent.gameObject.SetActive(false);
        }

        //set the damage
        if (damage != null)
        {
            if (CardData.AttackDamage != 0)
                damage.text = CardData.AttackDamage.ToString();
            else
                damage.transform.parent.gameObject.SetActive(false);
        }

        //set the card's color
        cardBackground.color = CardData.Color;
        //set the condition icon
        if (condition != null)
            foreach (KeyValuePair<int, string> entry in UIManager.Instance.minionConditions)
                if (CardData.ConditionID == entry.Key)
                    condition.sprite = UIManager.Instance.allSprites.Where(x => x.name == entry.Value).SingleOrDefault();
        //set the effect1 icons
        if (CardData.EffectId1 != 0 && CardData.EffectId1 != 999)
        {
            foreach (KeyValuePair<int, string> entry in UIManager.Instance.minionEffects)
                if (CardData.EffectId1 == entry.Key)
                    effect1.sprite = UIManager.Instance.allSprites.Where(x => x.name == entry.Value).SingleOrDefault();
        }
        else
            effect1.transform.parent.gameObject.SetActive(false);

        //set the effect2 icons
        if (CardData.EffectId2 != 0 && CardData.EffectId2 != 999)
        {
            foreach (KeyValuePair<int, string> entry in UIManager.Instance.minionEffects)
                if (CardData.EffectId2 == entry.Key)
                    effect2.sprite = UIManager.Instance.allSprites.Where(x => x.name == entry.Value).SingleOrDefault();
        }
        else
            effect2.transform.parent.gameObject.SetActive(false);


        //set the allied class icon
        if (CardData.AllyClassID != 0)
            foreach (KeyValuePair<int, string> entry in UIManager.Instance.minionClasses)
                if (CardData.AllyClassID == entry.Key)
                    allyClass.sprite = UIManager.Instance.allSprites.Where(x => x.name == entry.Value).SingleOrDefault();
    }

    void CollapseMinionCard()
    {
        transform.localScale = new Vector3(1, 1, 1);
        isEnlarged = false;
        foreach (Transform t in descriptions)
            t.gameObject.SetActive(false);
    }

    void EnlargeMinionCard()
    {
        transform.localScale = new Vector3(4, 4, 4);
        isEnlarged = true;
        TMP_Text text = descriptions[13].GetComponent<TMP_Text>();
        if (!text.text.Equals(""))
            foreach (Transform t in descriptions)
                t.gameObject.SetActive(true);
        else
            for (int i = 0; i < descriptions.Count - 2; i++)
                descriptions[i].gameObject.SetActive(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (!isEnlarged)
                EnlargeMinionCard();
            else
                CollapseMinionCard();
        }

        else if (eventData.button == PointerEventData.InputButton.Left &&
            GameManager.Instance.ActiveHero().CanPlayCards &&
            (transform.parent.name.Equals("Hand") || transform.parent.name.Equals("EnemyHand")))
        {
            pc = gameObject.GetComponent<PlayCard>();
            if (pc)
                pc.ShowSummonPanel();

            UIManager.Instance.LastSelectedCard = gameObject;
        }
    }

    //TODO: OnHover Function to highlight the card
}
