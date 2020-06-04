using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New StarterData", menuName = "Starter Data", order = 51)]
public class StarterData : Card
{
    [SerializeField]
    private Color color;
    [SerializeField]
    private int starterID;
    [SerializeField]
    private int goldAndManaCost;
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

    public int StarterID { get => starterID; set => starterID = value; }
    public override int GoldAndManaCost { get => goldAndManaCost; set => goldAndManaCost = value; }
    public override string EffectText1 { get => effectText1; set => effectText1 = value; }
    public override int EffectId1 { get => effectId1; set => effectId1 = value; }
    public override int AttackDamage { get => attackDamage; set => attackDamage = value; }
    public override int Health { get => health; set => health = value; }
    public override string EffectText2 { get => effectText2; set => effectText2 = value; }
    public override int EffectId2 { get => effectId2; set => effectId2 = value; }
    public override Color Color { get => color; set => color = value; }
}
