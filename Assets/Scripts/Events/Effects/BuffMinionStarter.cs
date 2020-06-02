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

        //Get Current Player and current zone to apply too
        int currentPlayer = GameManager.Instance.GetCurrentPlayer();
        Transform zone;

        if (currentPlayer == 0)
        {
            zone = GameManager.Instance.alliedMinionZone;
        }
        else
        {
            zone = GameManager.Instance.enemyMinionZone;
        }

        //Assign each Minion a Listener Script 
        foreach (Transform t in zone)
        {
            t.gameObject.AddComponent<BuffMinionListener>();
        }
    }
}
