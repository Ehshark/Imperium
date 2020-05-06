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
    private bool hasToDiscard;
    private bool myTurn;
    private bool canPlayCards;

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
    public bool HasToDiscard { get => hasToDiscard; set => hasToDiscard = value; }
    public bool MyTurn { get => myTurn; set => myTurn = value; }
    public int RequredExp { get => requredExp; set => requredExp = value; }
    public bool CanPlayCards { get => canPlayCards; set => canPlayCards = value; }

    //Components
    [SerializeField]
    private TextMeshProUGUI healthText;
    [SerializeField]
    private Slider healthBar;
    [SerializeField]
    private TextMeshProUGUI expText;
    [SerializeField]
    private Slider expBar;
    [SerializeField]
    private TextMeshProUGUI manaText;
    [SerializeField]
    private Slider manaBar;
    [SerializeField]
    private TextMeshProUGUI levelText;
    [SerializeField]
    private TextMeshProUGUI goldText;
    [SerializeField]
    private TextMeshProUGUI playerName;

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
}
