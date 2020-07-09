using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UntapMinionStarter : MonoBehaviour
{
    public void StartEvent()
    {
        Transform alliedMinions = GameManager.Instance.GetActiveMinionZone(true);
        bool tapExists = false;

        foreach (Transform t in alliedMinions)
        {
            if (t.gameObject.GetComponent<CardVisual>().IsTapped)
            {
                tapExists = true;
            }
        }

        if (tapExists)
        {
            GameManager.Instance.instructionsObj.GetComponent<TMP_Text>().text = "Select one tapped Minion to untap";

            foreach (Transform t in alliedMinions)
            {
                t.gameObject.AddComponent<UntapMinionListener>();
            }
        }

        else
        {
            StartCoroutine(GameManager.Instance.SetInstructionsText("No tapped minions to untap."));
            InvokeEventCommand.InvokeNextEvent();
        }

    }
}
