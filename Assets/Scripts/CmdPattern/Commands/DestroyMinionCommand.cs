using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DestroyMinionCommand : Command
{
    readonly GameObject minion;

    public DestroyMinionCommand(GameObject m)
    {
        minion = m;
    }

    public override void StartCommandExecution()
    {
        TMP_Text text = GameManager.Instance.instructionsObj.GetComponent<TMP_Text>();
        text.text = "";
        minion.transform.SetParent(GameManager.Instance.alliedDiscardPile);
        minion.transform.position = GameManager.Instance.alliedDiscardPile.position;
        CommandExecutionComplete();
    }
}
