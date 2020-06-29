using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuffMinionStarter : MonoBehaviour
{
    public void StartEvent()
    {
        //Display Start message
        GameManager.Instance.instructionsObj.GetComponent<TMP_Text>().text = "Select one Minion to Buff";

        //Disable Player Interaction 
        GameManager.Instance.EnableOrDisablePlayerControl(false);

        //Assign each Minion a Listener Script 
        foreach (Transform t in GameManager.Instance.GetActiveMinionZone(true))
        {
            t.gameObject.AddComponent<BuffMinionListener>();
        }
    }
}
