using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DamagePhoton
{
    private int index;
    private int damageAmount;

    public DamagePhoton(int index, int damageAmount)
    {
        this.index = index;
        this.damageAmount = damageAmount;
    }

    public int GetIndex()
    {
        return index;
    }

    public int GetDamage()
    {
        return damageAmount;
    }
}
