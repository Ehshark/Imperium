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

        MinionVisual mv = minion.GetComponent<MinionVisual>();
        //if (mb.hasBattlecry)
        //{
        //    GameManager.Instance.enemyMinionZone.gameObject.AddComponent<DestroyMinionListener>();
        //    EventManager.Instance.AddListener(EVENT_TYPE.DESTROY_MINION, GameManager.Instance.enemyMinionZone.gameObject.GetComponent<DestroyMinionListener>());
        //    EventManager.Instance.PostNotification(EVENT_TYPE.DESTROY_MINION);
        //}

        CommandExecutionComplete();
    }
}
