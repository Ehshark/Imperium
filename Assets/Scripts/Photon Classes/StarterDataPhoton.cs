using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarterDataPhoton : CardPhoton
{
    private float[] color;
    private int starterID;
    private int goldAndManaCost;
    private string effectText1;
    private int effectId1;
    private string effectText2;
    private int effectId2;
    private int attackDamage;
    private int health;

    public int StarterID { get => starterID; set => starterID = value; }
    public override int GoldAndManaCost { get => goldAndManaCost; set => goldAndManaCost = value; }
    public override string EffectText1 { get => effectText1; set => effectText1 = value; }
    public override int EffectId1 { get => effectId1; set => effectId1 = value; }
    public override int AttackDamage { get => attackDamage; set => attackDamage = value; }
    public override int Health { get => health; set => health = value; }
    public override string EffectText2 { get => effectText2; set => effectText2 = value; }
    public override int EffectId2 { get => effectId2; set => effectId2 = value; }
    public override float[] Color { get => color; set => color = value; }

    public StarterDataPhoton(StarterData starter)
    {
        StarterID = starter.StarterID;
        GoldAndManaCost = starter.GoldAndManaCost;
        EffectId1 = starter.EffectId1;
        EffectText1 = starter.EffectText1;
        AttackDamage = starter.AttackDamage;
        Health = starter.Health;
        EffectId2 = starter.EffectId2;
        EffectText2 = starter.EffectText2;
        Color = new float[] { starter.Color.a, starter.Color.r, starter.Color.g, starter.Color.b };
    }
}
