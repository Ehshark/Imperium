using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilenceMinionStarter : MonoBehaviour
{

    public void StartEvent()
    {
        bool hasMinion = false;
        Transform mz = GameManager.Instance.GetActiveMinionZone(false);
        foreach (Transform t in mz)
        {
            CardVisual cv = t.GetComponent<CardVisual>();
            if (cv.Md != null)
                hasMinion = true;
        }

        //TODO: If Silence is one of the hero powers, then do a PostNotification for Hero Power Silence).
        if (GameManager.Instance.GetActiveMinionZone(false).childCount == 0 || !hasMinion)
        {
            EffectCommand.Instance.EffectQueue.Enqueue(EVENT_TYPE.POWER_SILENCE);

            //Call the Next Power in the Queue
            InvokeEventCommand.InvokeNextEvent();
        }
        else
        {
            StartCoroutine(GameManager.Instance.SetInstructionsText("Select a Minion to Silence"));
            foreach (Transform t in mz)
            {
                CardVisual cv = t.GetComponent<CardVisual>();
                if (cv.Md != null)
                    t.gameObject.AddComponent<SilenceListener>();
            }
        }
    }
}
