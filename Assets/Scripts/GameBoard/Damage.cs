using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage
{
    private Dictionary<string, int> damageAbsorbed;

    public Damage()
    {
        damageAbsorbed = new Dictionary<string, int>
        {
            { "stealth", 0 },
            { "lifesteal", 0 },
            { "poisonTouch", 0 },
            { "damage", 0 }
        };
    }

    public Dictionary<string, int> DamageAbsorbed { get => damageAbsorbed; set => damageAbsorbed = value; }

    public int TotalDamageAbsorbed()
    {
        int tot = 0;
        foreach (KeyValuePair<string, int> entry in damageAbsorbed)
        {
            tot += entry.Value;
        }
        return tot;
    }

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
