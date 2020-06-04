using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New MinionData", menuName = "Minion Data", order = 51)]
public class MinionData : Card
{
    [SerializeField]
    private Color color;
    [SerializeField]
    private int minionID;
    [SerializeField]
    private int goldAndManaCost;
    [SerializeField]
    private string conditionText;
    [SerializeField]
    private int conditionID;
    [SerializeField]
    private string effectText1;
    [SerializeField]
    private int effectId1;
    [SerializeField]
    private string effectText2;
    [SerializeField]
    private int effectId2;
    [SerializeField]
    private int attackDamage;
    [SerializeField]
    private int health;
    [SerializeField]
    private string cardClass;
    [SerializeField]
    private string allyClass;
    [SerializeField]
    private int allyClassID;

    public override Color Color { get => color; set => color = value; }
    public override int GoldAndManaCost { get => goldAndManaCost; set => goldAndManaCost = value; }
    public override int MinionID { get => minionID; set => minionID = value; }
    public override string ConditionText { get => conditionText; set => conditionText = value; }
    public override string EffectText1 { get => effectText1; set => effectText1 = value; }
    public override string EffectText2 { get => effectText2; set => effectText2 = value; }
    public override int AttackDamage { get => attackDamage; set => attackDamage = value; }
    public override int Health { get => health; set => health = value; }
    public override string CardClass { get => cardClass; set => cardClass = value; }
    public override string AllyClass { get => allyClass; set => allyClass = value; }
    public override int ConditionID { get => conditionID; set => conditionID = value; }
    public override int EffectId1 { get => effectId1; set => effectId1 = value; }
    public override int EffectId2 { get => effectId2; set => effectId2 = value; }
    public override int AllyClassID { get => allyClassID; set => allyClassID = value; }
}
