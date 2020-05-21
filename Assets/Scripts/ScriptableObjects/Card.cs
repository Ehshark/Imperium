using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : ScriptableObject
{
    public virtual Color Color { get; set; }
    public virtual int GoldAndManaCost { get; set; }
    public virtual int MinionID { get; set; }
    public virtual string ConditionText { get; set; }
    public virtual object Condition { get; set; }
    public virtual string EffectText1 { get; set; }
    public virtual object Effect1 { get; set; }
    public virtual string EffectText2 { get; set; }
    public virtual object Effect2 { get; set; }
    public virtual int AttackDamage { get; set; }
    public virtual int Health { get; set; }
    public virtual string CardClass { get; set; }
    public virtual bool IsPromoted { get; set; }
    public virtual bool IsTapped { get; set; }
    public virtual bool IsSilenced { get; set; }
    public virtual Damage EnemyDamageDealt { get; set; }
    public virtual string AllyClass { get; set; }
    public virtual int ConditionID { get; set; }
    public virtual int EffectId1 { get; set; }
    public virtual int EffectId2 { get; set; }
    public virtual int AllyClassID { get; set; }
}
