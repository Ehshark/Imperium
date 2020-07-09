using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilenceMinionStarter : MonoBehaviour
{

    public void StartEvent()
    {
        //TODO: If Silence is one of the hero powers, then do a PostNotification for Hero Power Silence).
        if (GameManager.Instance.GetActiveMinionZone(false).childCount == 0)
        {
            StartCoroutine(GameManager.Instance.SetInstructionsText("Opponent's Hero Silenced!"));

            //Call the Next Power in the Queue
            InvokeEventCommand.InvokeNextEvent();
        }
        else
        {
            StartCoroutine(GameManager.Instance.SetInstructionsText("Select a Minion to Silence"));
            Transform mz = GameManager.Instance.GetActiveMinionZone(false);
            foreach (Transform t in mz)
            {
                t.gameObject.AddComponent<SilenceListener>();
            }
        }
    }
}
