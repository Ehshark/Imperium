using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealMinionStarter : MonoBehaviour
{
    public void StartEvent()
    {
        //Display Start message
        StartCoroutine(GameManager.Instance.SetInstructionsText("Select one Minion to Heal"));

        //Disable Player Interaction 
        GameManager.Instance.EnableOrDisablePlayerControl(false);

        //Assign each Minion a Listener Script 
        foreach (Transform t in GameManager.Instance.GetActiveMinionZone(true))
        {
            t.gameObject.AddComponent<HealMinionListener>();
        }
    }
}
