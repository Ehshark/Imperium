using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EssentialsData", menuName = "Essential Data", order = 51)]
public class EssentialsData : Card
{
    [SerializeField]
    private Color color;
    [SerializeField]
    private int id;
    [SerializeField]
    private int goldAndManaCost;

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

    public override Color Color { get => color; set => color = value; }
    public int Id { get => id; set => id = value; }
    public override int GoldAndManaCost { get => goldAndManaCost; set => goldAndManaCost = value; }
    public override string EffectText1 { get => effectText1; set => effectText1 = value; }
    public override int EffectId1 { get => effectId1; set => effectId1 = value; }
    public override object Effect1 { get => effect1; set => effect1 = value; }
    public override string EffectText2 { get => effectText2; set => effectText2 = value; }
    public override int EffectId2 { get => effectId2; set => effectId2 = value; }
    public override object Effect2 { get => effect2; set => effect2 = value; }
}
