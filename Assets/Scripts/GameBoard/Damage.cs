using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage
{
    private string damageType = "";
    private int damageAmt = 0;

    public string DamageType { get => damageType; set => damageType = value; }
    public int DamageAmt { get => damageAmt; set => damageAmt = value; }
}
