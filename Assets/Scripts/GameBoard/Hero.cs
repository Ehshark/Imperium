using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Hero : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private int currentHealth;
    private int totalHealth;
    private int currentMana;
    private int totalMana;
    private int damage;
    private int damageBonus;
    private int experience;
    private int requredExp;
    private int level;
    private int gold;
    private int handSize;
    private int powerCount;
    private List<int> powers;

    private char clan;
    //private List<Ability> abilities;
    private bool hasExpressBuy;
    private int hasToDiscard;
    private bool myTurn;
    private bool canPlayCards;
    private bool isAttacking;
    private bool startedCombat;

    private Transform cardComponent;

    //Components
    [SerializeField]
    private Transform attackButton;
    [SerializeField]
    private Transform cancelButton;
    [SerializeField]
    private Transform submitButton;
    [SerializeField]
    private Transform defendButton;
    [SerializeField]
    private Transform damageObjects;
    [SerializeField]
    private Transform abilities;
    [SerializeField]
    private Transform textBack;

    [SerializeField]
    private TMP_Text healthText;
    [SerializeField]
    private Slider healthBar;
    [SerializeField]
    private TMP_Text expText;
    [SerializeField]
    private Slider expBar;
    [SerializeField]
    private TMP_Text manaText;
    [SerializeField]
    private Slider manaBar;
    [SerializeField]
    private TMP_Text levelText;
    [SerializeField]
    private TMP_Text goldText;
    [SerializeField]
    private TMP_Text playerName;
    [SerializeField]
    private Image heroImage;
    [SerializeField]
    private Image heroClan;

    [SerializeField]
    private TMP_Text damageText;
    [SerializeField]
    private TMP_Text discardText;
    [SerializeField]
    private TMP_Text regDmgAbs;
    [SerializeField]
    private TMP_Text poisonDmgAbs;
    [SerializeField]
    private TMP_Text stealthDmgAbs;
    [SerializeField]
    private TMP_Text lifestealDmgAbs;
    [SerializeField]
    private TMP_Text desc;

    [SerializeField]
    private Button increaseDmgAbsorbed;
    [SerializeField]
    private Button decreaseDmgAbsorbed;

    [SerializeField]
    private Image ability1;
    [SerializeField]
    private Image ability2;
    [SerializeField]
    private Image ability3;

    [SerializeField]
    private Image heroImageBorder;

    public ParticleSystem canAttackParticle;
    public Image HeroImageBorder { get => heroImageBorder; set => heroImageBorder = value; }

    [SerializeField]
    private Image playerNameBox;
    public Image PlayerNameBox { get => playerNameBox; set => playerNameBox = value; }

    private Damage dmgAbsorbed;
    public Damage DmgAbsorbed { get => dmgAbsorbed; set => dmgAbsorbed = value; }

    public int CurrentHealth { get => currentHealth; set => currentHealth = value; }
    public int TotalHealth { get => totalHealth; set => totalHealth = value; }
    public int CurrentMana { get => currentMana; set => currentMana = value; }
    public int TotalMana { get => totalMana; set => totalMana = value; }
    public int Damage { get => damage; set => damage = value; }
    public int Experience { get => experience; set => experience = value; }
    public int Level { get => level; set => level = value; }
    public int Gold { get => gold; set => gold = value; }
    public int HandSize { get => handSize; set => handSize = value; }
    public char Clan { get => clan; set => clan = value; }
    public bool HasExpressBuy { get => hasExpressBuy; set => hasExpressBuy = value; }
    public int HasToDiscard { get => hasToDiscard; set => hasToDiscard = value; }
    public bool MyTurn { get => myTurn; set => myTurn = value; }
    public int RequredExp { get => requredExp; set => requredExp = value; }
    public bool CanPlayCards { get => canPlayCards; set => canPlayCards = value; }
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }
    public Transform AttackButton { get => attackButton; set => attackButton = value; }
    public bool StartedCombat { get => startedCombat; set => startedCombat = value; }
    public Transform CancelButton { get => cancelButton; set => cancelButton = value; }
    public Transform SubmitButton { get => submitButton; set => submitButton = value; }
    public Transform DefendButton { get => defendButton; set => defendButton = value; }
    public Transform DamageObjects { get => damageObjects; set => damageObjects = value; }
    public int PowerCount { get => powerCount; set => powerCount = value; }
    public List<int> Powers { get => powers; set => powers = value; }
    public Image Ability1 { get => ability1; set => ability1 = value; }
    public Image Ability2 { get => ability2; set => ability2 = value; }
    public Image Ability3 { get => ability3; set => ability3 = value; }
    public Image HeroImage { get => heroImage; set => heroImage = value; }
    public int DamageBonus { get => damageBonus; set => damageBonus = value; }
    public Transform Abilities { get => abilities; set => abilities = value; }

    //Multiplayer
    const byte LEVEL_UP = 16;
    const byte ADJUST_DISCARD_SYNC_EVENT = 25;

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
        if (eventCode == ADJUST_DISCARD_SYNC_EVENT)
        {
            object[] data = (object[])photonEvent.CustomData;
            bool increase = (bool)data[0];
            if (increase)
                hasToDiscard++;
            else
                hasToDiscard = 0;

            discardText.text = HasToDiscard.ToString();
        }
    }

    private void Start()
    {
        dmgAbsorbed = new Damage();
        increaseDmgAbsorbed.onClick.AddListener(delegate { AssignDamageAbsorbed(true); });
        decreaseDmgAbsorbed.onClick.AddListener(delegate { AssignDamageAbsorbed(false); });
    }

    public void SetHero(int health, int mana, int damage, int reqExp, int hand, char clan, Sprite image)
    {
        //Variables 
        CurrentHealth = health;
        TotalHealth = health;
        CurrentMana = mana;
        TotalMana = mana;
        Damage = damage;
        RequredExp = reqExp;
        HandSize = hand;
        level = 1;

        //UI
        SetHealth();
        SetMana();
        SetExp();
        SetGold();
        SetSprite(image);
        SetDamage();
        SetClan(clan);

        //Power
        powers = new List<int>();
    }

    public void SetHealth()
    {
        healthText.text = currentHealth + "   /   " + totalHealth;
        healthBar.maxValue = totalHealth;
        healthBar.value = currentHealth;
    }

    public void AdjustHealth(int amount, bool add)
    {
        int total = 0;

        if (add)
        {
            total = amount + currentHealth;

            if (total <= totalHealth)
            {
                currentHealth += amount;
            }
            else
            {
                currentHealth = totalHealth;
            }
        }
        else
        {
            total = currentHealth - amount;

            if (total >= 0)
            {
                currentHealth -= amount;
            }
            else
            {
                currentHealth = 0;
            }
        }

        SetHealth();
    }

    public void ResetMana()
    {
        currentMana = totalMana;

        SetMana();
    }

    public void SetExp()
    {
        expText.text = experience + "   /   " + requredExp;
        expBar.maxValue = requredExp;
        expBar.value = experience;
    }

    public void IncreaseExp(int amount)
    {
        requredExp = amount;
        experience = 0;
        SetExp();
    }

    public void GainExp(int amount)
    {
        int total = amount + experience;

        if (total < RequredExp)
        {
            experience += amount;
        }
        else
        {
            //Get the total amount of experience to increase after level up
            int expToIncreaseAfter = total - RequredExp;

            if (level != HeroManager.Instance.MaxLevel)
            {
                experience += amount;
            }

            EventManager.Instance.PostNotification(EVENT_TYPE.LEVEL_UP);
            PhotonNetwork.RaiseEvent(LEVEL_UP, null, new RaiseEventOptions { Receivers = ReceiverGroup.Others },
                SendOptions.SendReliable);

            //Increase experience if there is experience to increase afterwards
            if (expToIncreaseAfter > 0 && level != HeroManager.Instance.MaxLevel)
            {
                experience += expToIncreaseAfter;
            }
        }

        SetExp();
    }

    public void EnemyGainExp(int amount)
    {
        int total = amount + experience;

        if (total < RequredExp)
        {
            experience += amount;
        }
        else
        {
            //Get the total amount of experience to increase after level up
            int expToIncreaseAfter = total - RequredExp;

            if (level != HeroManager.Instance.MaxLevel)
            {
                experience += amount;
            }

            //Increase experience if there is experience to increase afterwards
            if (expToIncreaseAfter > 0 && level != HeroManager.Instance.MaxLevel)
            {
                experience += expToIncreaseAfter;
            }
        }

        SetExp();
    }

    public void SetMana()
    {
        manaText.text = currentMana + "   /   " + totalMana;
        manaBar.maxValue = totalMana;
        manaBar.value = currentMana;
    }

    public void AdjustMana(int amount, bool add)
    {
        int total = 0;

        if (add)
        {
            total = amount + currentMana;

            if (total <= TotalMana)
            {
                currentMana += amount;
            }
            else
            {
                currentMana = totalMana;
            }
        }
        else
        {
            total = currentMana - amount;

            if (total >= 0)
            {
                currentMana -= amount;
            }
            else
            {
                currentMana = 0;
            }
        }

        SetMana();
    }

    public void SetGold()
    {
        goldText.text = gold.ToString();
    }

    public void AdjustGold(int amount, bool add)
    {
        int total = 0;

        if (add)
        {
            gold += amount;
        }
        else
        {
            total = gold - amount;

            if (total >= 0)
            {
                gold -= amount;
            }
            else
            {
                gold = 0;
            }
        }

        SetGold();
    }

    public void SetPlayerName(string name)
    {
        playerName.text = name;
    }

    public void SetLevel()
    {
        levelText.text = level.ToString();
    }

    public void IncreaseLevel(int amount)
    {
        level = level + 1;

        SetLevel();
    }

    public void SetSprite(Sprite sprite)
    {
        heroImage.sprite = sprite;
    }

    public void SetDamage()
    {
        damageText.text = damage.ToString();
    }

    public void SetClan(char clanType)
    {
        clan = clanType;

        //if (clan == 'W')
        //{
        //    heroClan.color = new Color32(255, 60, 60, 255);
        //}
        //else if (clan == 'R')
        //{
        //    heroClan.color = new Color32(75, 255, 150, 255);
        //}
        //else if (clan == 'M')
        //{
        //    heroClan.color = new Color32(15, 129, 162, 255);
        //}
    }

    public void AdjustDamage(int amount, bool isBonus)
    {
        damage += amount;
        SetDamage();
        if (isBonus)
            damageBonus += amount;
    }

    public void AdjustDiscard(bool increase)
    {
        if (increase)
            hasToDiscard++;
        else
            hasToDiscard = 0;

        discardText.text = HasToDiscard.ToString();

        object[] data = new object[] { increase };
        PhotonNetwork.RaiseEvent(ADJUST_DISCARD_SYNC_EVENT, data, new RaiseEventOptions { Receivers = ReceiverGroup.Others },
            SendOptions.SendReliable);
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
                else if (dmgAbsorbed.TotalDamageAbsorbed() == currentHealth)
                {
                    StartCoroutine(GameManager.Instance.SetInstructionsText("Hero cannot absorb more damage."));
                }
                else if (dmgAbsorbed.DamageAbsorbed[dl.DamageSelected] == sc.totalDamage[dl.DamageSelected] ||
                    CheckAlliesAssignment(dl.DamageSelected) < sc.totalDamage[dl.DamageSelected])
                {
                    StartCoroutine(GameManager.Instance.SetInstructionsText("You don't need to assign more damage of that type."));
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
        damageForType += dmgAbsorbed.DamageAbsorbed[damageType];
        return damageForType;
    }

    public void ChangeResources(int h, int m, int d)
    {
        totalHealth = h;
        totalMana = m;
        damage = d;

        SetHealth();
        SetMana();
        SetDamage();
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (textBack != null && desc != null)
        {
            cardComponent = eventData.pointerCurrentRaycast.gameObject.transform;
            if (cardComponent.name.Contains("Exp"))
                desc.text = "Gain experience to level up and unlock powers";
            else if (cardComponent.name.Contains("Health"))
                desc.text = "If your health reaches 0, you lose";
            else if (cardComponent.name.Contains("Mana"))
                desc.text = "Mana is required to play cards";
            else if (cardComponent.name.Contains("Mana"))
                desc.text = "Mana is required to play cards";
            else if (cardComponent.name.Contains("Level") || cardComponent.name.Equals("BorderBack"))
                desc.text = "Your hero's level and unlocked powers";
            else if (cardComponent.name.Contains("Gold"))
                desc.text = "Your hero's gold";
            else if (cardComponent.name.Contains("Discard"))
                desc.text = "Discard these many cards at the start of your next turn";
            else if (cardComponent.name.Contains("Damage"))
                desc.text = "Your hero's damage in combat";

            if (!desc.text.Equals(""))
                textBack.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (textBack != null && desc != null)
        {
            textBack.gameObject.SetActive(false);
            desc.text = "";
        }
    }
}
