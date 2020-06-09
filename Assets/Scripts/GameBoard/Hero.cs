using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hero : MonoBehaviour
{
    private int currentHealth;
    private int totalHealth;
    private int currentMana;
    private int totalMana;
    private int damage;
    private int experience;
    private int requredExp;
    private int level;
    private int gold;
    private int handSize;

    private char clan;
    //private List<Ability> abilities;
    private bool hasExpressBuy;
    private int hasToDiscard;
    private bool myTurn;
    private bool canPlayCards;
    private bool isAttacking;
    private bool startedCombat;

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
    private TMP_Text regDmgAbs;
    [SerializeField]
    private TMP_Text poisonDmgAbs;
    [SerializeField]
    private TMP_Text stealthDmgAbs;
    [SerializeField]
    private TMP_Text lifestealDmgAbs;

    [SerializeField]
    private Button increaseDmgAbsorbed;
    [SerializeField]
    private Button decreaseDmgAbsorbed;

    [SerializeField]
    private Image heroImageBorder;
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

        //UI
        SetHealth();
        SetMana();
        SetExp();
        SetGold();
        SetSprite(image);
        SetDamage();
        SetClan(clan);
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

    public void SetExp()
    {
        expText.text = experience + "   /   " + requredExp;
        expBar.maxValue = requredExp;
        expBar.value = experience;
    }

    public void GainExp(int amount)
    {
        int total = amount + experience;

        if (total <= RequredExp)
        {
            experience += amount;
        }
        else
        {
            experience = RequredExp;
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

    public void AdjustDamage(int amount)
    {
        damage += amount;
        SetDamage();
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
        damageForType += dmgAbsorbed.DamageAbsorbed[damageType];
        return damageForType;
    }
}
