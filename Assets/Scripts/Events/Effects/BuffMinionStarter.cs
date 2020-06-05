using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffMinionStarter : MonoBehaviour
{
    public void StartEvent()
    {
        //Display Start message
        StartCoroutine(GameManager.Instance.SetInstructionsText("Select one Minion to Buff"));

        //Disable Player Interaction 
        GameManager.Instance.EnableOrDisablePlayerControl(false);

        //Assign each Minion a Listener Script 
        foreach (Transform t in GameManager.Instance.GetActiveMinionZone(true))
        {
            t.gameObject.AddComponent<BuffMinionListener>();
        }
    }
}
