using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.EventSystems;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;
using UnityEditor;

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
    private int currentDamage;
    private int totalDamage;
    private int promotedHealth;
    private bool isPromoted;
    private bool isTapped;
    private bool healEffect;
    private bool tapEffect;

    private bool isSilenced = false;
    public bool IsSilenced { get => isSilenced; set => isSilenced = value; }

    public bool IsTapped { get => isTapped; set => isTapped = value; }

    private bool isCombatEffectActivated = false;
    public bool IsCombatEffectActivated { get => isCombatEffectActivated; set => isCombatEffectActivated = value; }

    private Damage dmgAbsorbed;
    public Damage DmgAbsorbed { get => dmgAbsorbed; set => dmgAbsorbed = value; }
    public int CurrentDamage { get => currentDamage; set => currentDamage = value; }
    public int TotalDamage { get => totalDamage; set => totalDamage = value; }
    public bool HealEffect { get => healEffect; set => healEffect = value; }
    public int CurrentHealth { get => currentHealth; set => currentHealth = value; }
    public int TotalHealth { get => totalHealth; set => totalHealth = value; }
    public bool IsPromoted { get => isPromoted; set => isPromoted = value; }
    public int PromotedHealth { get => promotedHealth; set => promotedHealth = value; }
    public bool TapEffect { get => tapEffect; set => tapEffect = value; }

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
    public Image silenceImage;


    public ParticleSystem particleGlow;

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

        if (md != null || (sd != null && currentDamage != 0))
        {
            increaseDmgAbsorbed.onClick.AddListener(delegate { AssignDamageAbsorbed(true); });
            decreaseDmgAbsorbed.onClick.AddListener(delegate { AssignDamageAbsorbed(false); });
        }

        if (md != null && md.ConditionID == 7)
        {
            IsCombatEffectActivated = true;
        }
    }

    public void UpdateCardDescriptions()
    {
        if (md != null)
        {
            TMP_Text text = descriptions[1].GetComponent<TMP_Text>();
            text.text = "This minion's gold and mana cost";
            text = descriptions[3].GetComponent<TMP_Text>();
            text.text = "This minion's current health";
            text = descriptions[5].GetComponent<TMP_Text>();
            text.text = "This minion's damage in combat";
            text = descriptions[7].GetComponent<TMP_Text>();
            text.text = "When this minion attacks, if you control a " + cardData.AllyClass + " minion, its damage increases by 1";
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
            text.text = "This card's mana cost";
            text = descriptions[3].GetComponent<TMP_Text>();
            text.text = "This minion's health";
            text = descriptions[5].GetComponent<TMP_Text>();
            text.text = "This minion's damage in combat";
            text = descriptions[7].GetComponent<TMP_Text>();
            text.text = cardData.EffectText1;
            text = descriptions[9].GetComponent<TMP_Text>();
            text.text = cardData.EffectText2;
        }

        else if (ed != null)
        {
            TMP_Text text = descriptions[1].GetComponent<TMP_Text>();
            text.text = "This card's mana cost";
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

        //set the card's card art
        if (md != null)
        {
            if (md.CardClass.Equals("Warrior"))
                cardBackground.sprite = Resources.Load<Sprite>("VisualAssets/CardArt/warrior");
            else if (md.CardClass.Equals("Rogue"))
                cardBackground.sprite = Resources.Load<Sprite>("VisualAssets/CardArt/rogue");
            else
                cardBackground.sprite = Resources.Load<Sprite>("VisualAssets/CardArt/mage");
        }
        else if (sd != null)
        {
            if (currentDamage != 0)
                cardBackground.sprite = Resources.Load<Sprite>("VisualAssets/CardArt/fantasyWolf");
            else if (sd.EffectId1 == 20)
                cardBackground.sprite = Resources.Load<Sprite>("VisualAssets/CardArt/healthVial");
            else if (sd.EffectId1 == 21)
                cardBackground.sprite = Resources.Load<Sprite>("VisualAssets/CardArt/manaVial");
            else
                cardBackground.sprite = Resources.Load<Sprite>("VisualAssets/CardArt/pouchWithGold");
        }
        else
        {
            if (ed.EffectId1 == 18)
                cardBackground.sprite = Resources.Load<Sprite>("VisualAssets/CardArt/chest");
            else if (ed.EffectId1 == 20)
                cardBackground.sprite = Resources.Load<Sprite>("VisualAssets/CardArt/healthPotion");
            else if (ed.EffectId1 == 21)
                cardBackground.sprite = Resources.Load<Sprite>("VisualAssets/CardArt/manaPotion");
            else
                cardBackground.sprite = Resources.Load<Sprite>("VisualAssets/CardArt/ragePotion");
        }


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
        if (UIManager.Instance.enlargedCard.childCount > 0)
        {
            foreach (Transform t in UIManager.Instance.enlargedCard)
            {
                Destroy(t.gameObject);
            }
        }

        UIManager.Instance.LastEnlargedCard = null;
    }

    void EnlargeMinionCard()
    {
        if (gameObject.transform.parent.name.Equals("EnemyHand"))
        {
            return;
        }
        if (UIManager.Instance.LastEnlargedCard)
            CollapseMinionCard();
        GameObject go = Instantiate(gameObject);
        CardVisual cv = go.GetComponent<CardVisual>();
        cv.currentDamage = currentDamage;
        cv.currentHealth = currentHealth;
        go.transform.SetParent(UIManager.Instance.enlargedCard, false);
        go.transform.position = UIManager.Instance.enlargedCard.position;
        go.GetComponent<RectTransform>().pivot = new Vector3(UIManager.Instance.enlargedCard.GetComponent<RectTransform>().
            pivot.x, UIManager.Instance.enlargedCard.GetComponent<RectTransform>().pivot.y);
        go.transform.localScale = new Vector3(3, 3, 3);

        if (go.GetComponent<PlayCard>() != null)
        {
            Destroy(go.GetComponent<PlayCard>());
        }

        go.AddComponent<EnlargedCardBehaviour>();
        foreach (Transform t in go.transform)
        {
            if (t.name.Equals("CardBack") || t.name.Equals("SummonPanel") || t.name.Equals("DamageObjects") ||
                t.name.Equals("GlowPanel") || t.name.Equals("UsePanel") || t.name.Equals("ParticleGlow"))
            {
                t.gameObject.SetActive(false);
            }
        }
        if (UIManager.Instance.LastEnlargedCard)
            UIManager.Instance.LastEnlargedCard.GetComponent<CardVisual>().isEnlarged = false;
        isEnlarged = true;
        UIManager.Instance.LastEnlargedCard = gameObject;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && !inShop)
        {
            if (!transform.parent.name.Equals("EnlargedCard"))
            {
                if (UIManager.Instance.LastEnlargedCard == null || UIManager.Instance.LastEnlargedCard != gameObject)
                    EnlargeMinionCard();
                else
                    CollapseMinionCard();
            }
        }

        else if (eventData.button == PointerEventData.InputButton.Left &&
            GameManager.Instance.ActiveHero(true).CanPlayCards && (gameObject.GetComponent<PlayCard>() != null))
        {
            pc = gameObject.GetComponent<PlayCard>();
            if (pc)
                pc.ShowSummonPanel();

            UIManager.Instance.LastSelectedCard = gameObject;
        }

        else if (eventData.button == PointerEventData.InputButton.Left &&
            GameManager.Instance.ActiveHero(true).CanPlayCards && (gameObject.GetComponent<TutorialPlayCard>() != null))
        {
            TutorialPlayCard tpc = gameObject.GetComponent<TutorialPlayCard>();
            if (tpc)
                tpc.ShowSummonPanel();

            UIManager.Instance.LastSelectedCard = gameObject;
        }

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            CollapseMinionCard();
        }
    }

    public void AdjustHealth(int amount, bool add)
    {
        if (cardData is MinionData || cardData is StarterData)
        {
            if (add)
            {
                currentHealth += amount;
            }
            else
            {
                currentHealth -= amount;

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
        currentHealth = totalHealth;
        DelayCommand dc = new DelayCommand(transform, 1f);
        dc.AddToQueue();
        MoveCardCommand mc;

        //Get the Card's Data
        Card card = gameObject.GetComponent<CardVisual>().CardData;

        //Check for shock
        ShockListener sl = gameObject.GetComponent<ShockListener>();
        if (sl)
        {
            mc = new MoveCardCommand(gameObject, GameManager.Instance.GetActiveDiscardPile(false), UIManager.Instance.GetActiveDiscardList(false));
        }
        else
        {
            if (GameManager.Instance.IsDefending)
            {
                mc = new MoveCardCommand(gameObject, GameManager.Instance.GetActiveDiscardPile(false), UIManager.Instance.GetActiveDiscardList(false));
            }
            else
            {
                mc = new MoveCardCommand(gameObject, GameManager.Instance.GetActiveDiscardPile(true), UIManager.Instance.GetActiveDiscardList(true));
            }
        }

        mc.AddToQueue();
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
                    CheckAlliesAssignment(dl.DamageSelected) < sc.totalDamage[dl.DamageSelected] && CheckCombatEffects(dl.DamageSelected))
                {
                    if (dmgAbsorbed.DamageAbsorbed[dl.DamageSelected] == 0)
                    {
                        dmgText.transform.parent.gameObject.SetActive(true);
                    }
                    dmgAbsorbed.DamageAbsorbed[dl.DamageSelected]++;
                }
                else if (dmgAbsorbed.TotalDamageAbsorbed() == currentHealth)
                {
                    StartCoroutine(GameManager.Instance.SetInstructionsText("That minion cannot absorb more damage."));
                }
                else if (dmgAbsorbed.DamageAbsorbed[dl.DamageSelected] == sc.totalDamage[dl.DamageSelected] ||
                    CheckAlliesAssignment(dl.DamageSelected) == sc.totalDamage[dl.DamageSelected])
                {
                    StartCoroutine(GameManager.Instance.SetInstructionsText("You don't need to assign more damage of that type."));
                }
                else if (!CheckCombatEffects(dl.DamageSelected))
                {
                    StartCoroutine(GameManager.Instance.SetInstructionsText("To absorb Stealth damage, minion needs vigilance."));
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
            if (t.gameObject.activeSelf)
            {
                damageForType += t.GetComponent<CardVisual>().DmgAbsorbed.DamageAbsorbed[damageType];
            }
        }
        damageForType += GameManager.Instance.ActiveHero(false).DmgAbsorbed.DamageAbsorbed[damageType];
        return damageForType;
    }

    private bool CheckCombatEffects(string damageType)
    {
        bool canAssignDamage = false;
        if (damageType.Equals("stealth"))
        {
            if (md && md.EffectId1 == 9)
            {
                canAssignDamage = true;
            }
        }
        else
        {
            canAssignDamage = true;
        }
        return canAssignDamage;
    }

    public void AdjustDamage(int amount, bool add)
    {
        if (add)
        {
            currentDamage += amount;
            damage.color = Color.green;
        }
        else
        {
            currentDamage -= amount;
            damage.color = Color.white;

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
        damage.color = Color.white;
    }

    public void ResetHealth()
    {
        currentHealth = totalHealth;
        health.text = currentHealth.ToString();
    }

    public Card GetCardData()
    {
        if (md != null)
            return md;
        else if (sd != null)
            return sd;
        else
            return ed;
    }

    public void ChangeTappedAppearance()
    {
        Color originalColor = cardBackground.color;
        if (isTapped)
            originalColor.a = 0.5f;
        else
            originalColor.a = 1f;

        cardBackground.color = originalColor;
    }

    public void CombatEffectActivated(bool activate)
    {
        Transform glowObj = transform.Find("GlowPanel");
        Image glow = glowObj.GetComponent<Image>();
        if (glow.color != new Color(1f, 0f, 0f, 1f))
            glow.color = new Color(1f, 0f, 0f, 1f);

        if (activate)
            glowObj.gameObject.SetActive(true);
        else
            glowObj.gameObject.SetActive(false);
    }

    //TODO: OnHover Function to highlight the card

    public void ActivateSilence(bool activate)
    {
        if (activate)
        {
            isSilenced = true;
            silenceImage.enabled = true;
        }
        else
        {
            isSilenced = false;
            silenceImage.enabled = false;
        }
    }

    public void ResetDamageObjectsUI()
    {
        Transform amounts = damageObjects.transform.Find("DamageAmounts");
        foreach (Transform t in amounts)
        {
            t.gameObject.SetActive(false);
            foreach (Transform t2 in t)
            {
                if (t2.name.Contains("Text"))
                {
                    t2.GetComponent<TMP_Text>().text = "0";
                }
            }
        }
    }
}
