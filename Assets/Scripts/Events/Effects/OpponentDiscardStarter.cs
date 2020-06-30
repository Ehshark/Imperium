using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentDiscardStarter : MonoBehaviour
{
    public void StartEvent()
    {
        //Display Start message
        StartCoroutine(GameManager.Instance.SetInstructionsText("Increased opponent's mandatory discard"));

        GameManager.Instance.ActiveHero(false).HasToDiscard++;
    }
}
