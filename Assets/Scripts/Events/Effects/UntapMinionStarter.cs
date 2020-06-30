using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UntapMinionStarter : MonoBehaviour
{
    void StartEvent()
    {
        //Display Start message
        StartCoroutine(GameManager.Instance.SetInstructionsText("Select one tapped Minion to untap"));

        //Assign each Minion a Listener Script 
        foreach (Transform t in GameManager.Instance.GetActiveMinionZone(true))
        {
            t.gameObject.AddComponent<UntapMinionListener>();
        }
    }
}
