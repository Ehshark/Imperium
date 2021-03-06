﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EssentialsData", menuName = "Essential Data", order = 51)]
public class EssentialsData : Card
{
    [SerializeField]
    private int id;
    [SerializeField]
    private int goldAndManaCost;
    [SerializeField]
    private int manaCost;
    [SerializeField]
    private string effectText1;
    [SerializeField]
    private int effectId1;
    [SerializeField]
    private string effectText2;
    [SerializeField]
    private int effectId2;

    public int Id { get => id; set => id = value; }
    public override int GoldAndManaCost { get => goldAndManaCost; set => goldAndManaCost = value; }
    public override string EffectText1 { get => effectText1; set => effectText1 = value; }
    public override int EffectId1 { get => effectId1; set => effectId1 = value; }
    public override string EffectText2 { get => effectText2; set => effectText2 = value; }
    public override int EffectId2 { get => effectId2; set => effectId2 = value; }
    public int ManaCost { get => manaCost; set => manaCost = value; }

    public void Init(EssentialsDataPhoton edp)
    {
        Id = edp.Id;
        GoldAndManaCost = edp.GoldAndManaCost;
        EffectText1 = edp.EffectText1;
        EffectId1 = edp.EffectId1;
        EffectText2 = edp.EffectText2;
        EffectId2 = edp.EffectId2;
        ManaCost = edp.ManaCost;
    }
}
