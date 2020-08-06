using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DamagePhoton
{
    private int index;
    //private int damageAmount;
    private Dictionary<string, int> damageAbsorbed =  new Dictionary<string, int>
        {
            { "stealth", 0 },
            { "lifesteal", 0 },
            { "poisonTouch", 0 },
            { "damage", 0 }
        };

    public Dictionary<string, int> DamageAbsorbed { get => damageAbsorbed; set => damageAbsorbed = value; }

    public DamagePhoton(int index, Dictionary<string, int> d)
    {
        this.index = index;
        //this.damageAmount = damageAmount;
        damageAbsorbed = d;
    }

    public int GetIndex()
    {
        return index;
    }

    //public int GetDamage()
    //{
    //    return damageAmount;
    //}

    public void ResetDamageAbsorbed()
    {
        damageAbsorbed = new Dictionary<string, int>
        {
            { "stealth", 0 },
            { "lifesteal", 0 },
            { "poisonTouch", 0 },
            { "damage", 0 }
        };
    }
}
