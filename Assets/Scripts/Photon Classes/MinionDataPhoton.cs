using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MinionDataPhoton : CardPhoton
{
    private float[] color;
    private int minionID;    
    private int goldAndManaCost;
    private string conditionText;
    private int conditionID;
    private string effectText1;
    private int effectId1;
    private string effectText2;
    private int effectId2;    
    private int attackDamage;
    private int health;    
    private string cardClass;
    private string allyClass;
    private int allyClassID;

    public override float[] Color { get => color; set => color = value; }
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

    public MinionDataPhoton(MinionData minion)
    {
        Color = new float[] { minion.Color.a, minion.Color.r, minion.Color.g, minion.Color.b };
        GoldAndManaCost = minion.GoldAndManaCost;
        MinionID = minion.MinionID;
        ConditionText = minion.ConditionText;
        EffectText1 = minion.EffectText1;
        EffectText2 = minion.EffectText2;
        AttackDamage = minion.AttackDamage;
        Health = minion.Health;
        CardClass = minion.CardClass;
        AllyClass = minion.AllyClass;
        ConditionID = minion.ConditionID;
        EffectId1 = minion.EffectId1;
        EffectId2 = minion.EffectId2;
        AllyClassID = minion.AllyClassID;
    }
}
