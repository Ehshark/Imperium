using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
