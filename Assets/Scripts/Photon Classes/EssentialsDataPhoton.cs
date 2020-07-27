using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class EssentialsDataPhoton : CardPhoton
{
    private int id;    
    private int goldAndManaCost;
    private int manaCost;
    private string effectText1;
    private int effectId1;
    private string effectText2;
    private int effectId2;

    public int Id { get => id; set => id = value; }
    public override int GoldAndManaCost { get => goldAndManaCost; set => goldAndManaCost = value; }
    public override string EffectText1 { get => effectText1; set => effectText1 = value; }
    public override int EffectId1 { get => effectId1; set => effectId1 = value; }
    public override string EffectText2 { get => effectText2; set => effectText2 = value; }
    public override int EffectId2 { get => effectId2; set => effectId2 = value; }
    public int ManaCost { get => manaCost; set => manaCost = value; }

    public EssentialsDataPhoton(EssentialsData essentials)
    {
        Id = essentials.Id;
        GoldAndManaCost = essentials.GoldAndManaCost;
        EffectText1 = essentials.EffectText1;
        EffectId1 = essentials.EffectId1;
        EffectText2 = essentials.EffectText2;
        EffectId2 = essentials.EffectId2;
        ManaCost = essentials.ManaCost;
    }
}
