using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UntapMinionStarter : MonoBehaviour
{
    public void StartEvent()
    {
        bool tapExists = false;

        foreach (Transform t in GameManager.Instance.GetActiveMinionZone(true)) //checks if there are any tapped minions in the play area
        {
            if(t.gameObject.GetComponent<CardVisual>().IsTapped)
            {
                tapExists = true;
            }
            Debug.Log("checking for tapped minions");
        }

        if(tapExists) //if there are currently tapped minions
        {
            //Display Start message
            StartCoroutine(GameManager.Instance.SetInstructionsText("Select one tapped Minion to untap"));

            //Assign each Minion a Listener Script 
            foreach (Transform t in GameManager.Instance.GetActiveMinionZone(true))
            {
                t.gameObject.AddComponent<UntapMinionListener>();
            }
            Debug.Log("adding untap listener");
        }
        
    }
}
