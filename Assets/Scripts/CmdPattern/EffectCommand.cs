using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectCommand : Command
{
    int effectId;
    public int EffectId { get => effectId; }

    public EffectCommand(int ei)
    {
        effectId = ei;
    }

    public override void StartCommandExecution()
    {

    }
}
