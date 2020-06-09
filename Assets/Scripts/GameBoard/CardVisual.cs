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
    public int CurrentHealth { get => currentHealth; set => currentHealth = value; }
    private int totalHealth;
    private int currentDamage;
    private int totalDamage;
    private bool isTapped;
    public bool IsTapped { get => isTapped; set => isTapped = value; }

    private Damage dmgAbsorbed;
    public Damage DmgAbsorbed { get => dmgAbsorbed; set => dmgAbsorbed = value; }
    public int CurrentDamage { get => currentDamage; set => currentDamage = value; }
    public int TotalDamage { get => totalDamage; set => totalDamage = value; }

    public TMP_Text cost;
    public TMP_Text health;
    public TMP_Text damage;
    public TMP_Text regDmgAbs;
    public TMP_Text poisonDmgAbs;
    public TMP_Text stealthDmgAbs;
    public TMP_Text lifestealDmgAbs;

    public Image costImage;
    public Image cardBackground;
    public Image condition;
    public Image allyClass;
    public Image silenceIcon;
    public Image effect1;
    public Image effect2;

    public Button increaseDmgAbsorbed;
    public Button decreaseDmgAbsorbed;

    public bool inShop;

    private PlayCard pc;

    public GameObject damageObjects;
    void OnEnable()
    {
        dmgAbsorbed = new Damage();
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
        increaseDmgAbsorbed.onClick.AddListener(delegate { AssignDamageAbsorbed(true); });
        decreaseDmgAbsorbed.onClick.AddListener(delegate { AssignDamageAbsorbed(false); });
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
            {
                damage.text = CardData.AttackDamage.ToString();
                totalDamage = CardData.AttackDamage;
                currentDamage = CardData.AttackDamage;
            }
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
            GameManager.Instance.ActiveHero(true).CanPlayCards &&
            (transform.parent.name.Equals("Hand") || transform.parent.name.Equals("EnemyHand")))
        {
            pc = gameObject.GetComponent<PlayCard>();
            if (pc)
                pc.ShowSummonPanel();

            UIManager.Instance.LastSelectedCard = gameObject;
        }
    }

    public void AdjustHealth(int amount, bool add)
    {
        if (cardData is MinionData || cardData is StarterData)
        {
            if (add)
            {
                currentHealth = currentHealth + amount;
            }
            else
            {
                currentHealth = currentHealth - amount;

                if (currentHealth <= 0)
                {
                    DestroyMinion();
                }
            }

            health.text = currentHealth.ToString();
        }
    }

    public void DestroyMinion()
    {
        int currentPlayer = GameManager.Instance.GetCurrentPlayer();
        currentHealth = totalHealth;

        if (currentPlayer == 0)
        {
            GameManager.Instance.MoveCard(gameObject, GameManager.Instance.alliedDiscardPile, GameManager.Instance.alliedDiscardPileList, true);
        }
        else
        {
            GameManager.Instance.MoveCard(gameObject, GameManager.Instance.enemyDiscardPile, GameManager.Instance.enemyDiscardPileList, true);
        }
    }

    public void AssignDamageAbsorbed(bool isIncrease)
    {
        StartCombat sc = GameManager.Instance.ActiveHero(true).AttackButton.parent.GetComponent<StartCombat>();
        TMP_Text dmgText;
        DefendListener dl = GameManager.Instance.gameObject.GetComponent<DefendListener>();
        if (!dl.DamageSelected.Equals(""))
        {
            if (dl.DamageSelected.Equals("damage"))
            {
                dmgText = regDmgAbs;
            }
            else if (dl.DamageSelected.Equals("poisonTouch"))
            {
                dmgText = poisonDmgAbs;
            }
            else if (dl.DamageSelected.Equals("stealth"))
            {
                dmgText = stealthDmgAbs;
            }
            else
            {
                dmgText = lifestealDmgAbs;
            }

            if (isIncrease)
            {
                if (dmgAbsorbed.TotalDamageAbsorbed() < currentHealth && dmgAbsorbed.DamageAbsorbed[dl.DamageSelected] < sc.totalDamage[dl.DamageSelected] &&
                    CheckAlliesAssignment(dl.DamageSelected) < sc.totalDamage[dl.DamageSelected])
                {
                    if (dmgAbsorbed.DamageAbsorbed[dl.DamageSelected] == 0)
                    {
                        dmgText.transform.parent.gameObject.SetActive(true);
                    }
                    dmgAbsorbed.DamageAbsorbed[dl.DamageSelected]++;
                }
                dmgText.text = dmgAbsorbed.DamageAbsorbed[dl.DamageSelected].ToString();
            }
            else
            {
                if (dmgAbsorbed.TotalDamageAbsorbed() > 0 && dmgAbsorbed.DamageAbsorbed[dl.DamageSelected] > 0)
                {
                    dmgAbsorbed.DamageAbsorbed[dl.DamageSelected]--;
                }
                dmgText.text = dmgAbsorbed.DamageAbsorbed[dl.DamageSelected].ToString();
                if (dmgAbsorbed.DamageAbsorbed[dl.DamageSelected] == 0)
                {
                    dmgText.transform.parent.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            StartCoroutine(GameManager.Instance.SetInstructionsText("Select a damage type first"));
        }
    }

    private int CheckAlliesAssignment(string damageType)
    {
        int damageForType = 0;
        foreach (Transform t in GameManager.Instance.GetActiveMinionZone(false))
        {
            damageForType += t.GetComponent<CardVisual>().DmgAbsorbed.DamageAbsorbed[damageType];
        }
        damageForType += GameManager.Instance.ActiveHero(false).DmgAbsorbed.DamageAbsorbed[damageType];
        return damageForType;
    }
    public void AdjustDamage(int amount, bool add)
    {
        if (add)
        {
            currentDamage = currentDamage + amount;
        }
        else
        {
            currentDamage = currentDamage - amount;

            if (currentDamage < 0)
            {
                currentDamage = 0;
            }
        }

        damage.text = currentDamage.ToString();
    }

    public void ResetDamage()
    {
        currentDamage = totalDamage;
        damage.text = currentDamage.ToString();
    }

    //TODO: OnHover Function to highlight the card
}
