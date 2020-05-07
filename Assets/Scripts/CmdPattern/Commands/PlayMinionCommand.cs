using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayMinionCommand : Command
{
    readonly GameObject minion;

    public PlayMinionCommand(GameObject m)
    {
        minion = m;
    }

    public override void StartCommandExecution()
    {
        minion.transform.SetParent(GameManager.Instance.alliedMinionZone);
        GameManager.Instance.bottomHero.CanPlayCards = true;
        CommandExecutionComplete();
    }
}
