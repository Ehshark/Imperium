using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New StarterData", menuName = "Starter Data", order = 51)]
public class StarterData : ScriptableObject
{
    [SerializeField]
    private Color color;
    [SerializeField]
    private int starterID;
    [SerializeField]
    private int manaCost;

    [SerializeField]
    private string effectText1;
    [SerializeField]
    private int effectId1;

    private object effect1;

    [SerializeField]
    private string effectText2;
    [SerializeField]
    private int effectId2;

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

    public int StarterID { get => starterID; set => starterID = value; }
    public int ManaCost { get => manaCost; set => manaCost = value; }
    public string EffectText1 { get => effectText1; set => effectText1 = value; }
    public int EffectId1 { get => effectId1; set => effectId1 = value; }
    public object Effect1 { get => effect1; set => effect1 = value; }
    public int AttackDamage { get => attackDamage; set => attackDamage = value; }
    public int Health { get => health; set => health = value; }
    public string CardClass { get => cardClass; set => cardClass = value; }
    public bool IsPromoted { get => isPromoted; set => isPromoted = value; }
    public bool IsTapped { get => isTapped; set => isTapped = value; }
    public bool IsSilenced { get => isSilenced; set => isSilenced = value; }
    public object EnemyDamageDealt { get => enemyDamageDealt; set => enemyDamageDealt = value; }
    public string EffectText2 { get => effectText2; set => effectText2 = value; }
    public int EffectId2 { get => effectId2; set => effectId2 = value; }
    public object Effect2 { get => effect2; set => effect2 = value; }
    public Color Color { get => color; set => color = value; }
}
