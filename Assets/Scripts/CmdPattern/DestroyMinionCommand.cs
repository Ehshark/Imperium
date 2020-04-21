using System.Collections;
using System.Collections.Generic;
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
        Text text = GameManager.Instance.instructionsObj.gameObject.GetComponent<Text>();
        text.text = "";
        Transform emz = GameManager.Instance.enemyMinionZone;
        foreach (Transform obj in emz)
        {
            Image img = obj.GetComponentInChildren<Image>();
            img.color = Color.white;
            GameManager.Instance.CleanUpListeners();
        }
        CommandExecutionComplete();
    }
}
