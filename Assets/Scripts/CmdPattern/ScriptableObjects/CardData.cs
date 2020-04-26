using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New CardData", menuName = "Card Data", order = 51)]
public class CardData : ScriptableObject
{

    [SerializeField]
    private Color color;
    [SerializeField]
    private int minionID;
    [SerializeField]
    private int goldAndManaCost;
    [SerializeField]
    private string conditionText;

    private object condition;
    [SerializeField]
    private string effectText1;

    private object effect1;

    [SerializeField]
    private string effectText2;

    private object effect2;

    [SerializeField]
    private int attackDamage;
    [SerializeField]
    private int health;
    [SerializeField]
    private string cardClass;
    [SerializeField]
    private bool isPromoted;
    [SerializeField]
    private bool isTapped;
    [SerializeField]
    private bool isSilenced;

    private object enemyDamageDealt;

    [SerializeField]
    private string allyClass;

    public Color Color { get => color; set => color = value; }
    public int GoldAndManaCost { get => goldAndManaCost; set => goldAndManaCost = value; }
    public int MinionID { get => minionID; set => minionID = value; }
    public string ConditionText { get => conditionText; set => conditionText = value; }
    public object Condition { get => condition; set => condition = value; }
    public string EffectText1 { get => effectText1; set => effectText1 = value; }
    public object Effect1 { get => effect1; set => effect1 = value; }
    public string EffectText2 { get => effectText2; set => effectText2 = value; }
    public object Effect2 { get => effect2; set => effect2 = value; }
    public int AttackDamage { get => attackDamage; set => attackDamage = value; }
    public int Health { get => health; set => health = value; }
    public string CardClass { get => cardClass; set => cardClass = value; }
    public bool IsPromoted { get => isPromoted; set => isPromoted = value; }
    public bool IsTapped { get => isTapped; set => isTapped = value; }
    public bool IsSilenced { get => isSilenced; set => isSilenced = value; }
    public object EnemyDamageDealt { get => enemyDamageDealt; set => enemyDamageDealt = value; }
    public string AllyClass { get => allyClass; set => allyClass = value; }

}
