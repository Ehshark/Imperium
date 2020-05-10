using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayMinionCommand : Command
{
    readonly GameObject minion;
    readonly Transform from;
    readonly Transform to;

    public PlayMinionCommand(GameObject m, Transform f, Transform t)
    {
        minion = m;
        from = f;
        to = t;
    }

    public override void StartCommandExecution()
    {
        GameManager.Instance.MoveCard(minion, from, to);
        CommandExecutionComplete();
    }
}
